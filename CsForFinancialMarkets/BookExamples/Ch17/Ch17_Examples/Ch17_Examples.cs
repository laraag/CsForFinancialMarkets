// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Ch17_Examples.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TestCapFloorSwaption
{
    public static void Main() 
    {
        //SimpleCapletPrice();
        //CapAsSumOfCaplets();
        //SimpleBootstrapOneCaplet();
        //SimpleBootstrap();
        //SimpleBootstrap20Y();
        //CapletVol20Y_InputInterp();
        //VolOptimization();
        //MatrixCaplet();
        //MatrixCapletWithRateCurve();
    }

    public static void SimpleCapletPrice()
    {
        #region Input Data
        Date t0 = new Date(2013, 03, 4);
        Date t_i = new Date(2014, 03, 4);
        Date t_e = new Date(2014, 07, 4);
        double yf = (t_e.SerialValue - t_i.SerialValue) / 360;
        double T = (t_i.SerialValue - t0.SerialValue) / 365.0; ;

        double sigma = 0.335;
        double strike = 0.007;
        double df_i = 0.9912;  // Df on t_i
        double df_e = 0.9882; // Df on t_e
        double fwd = ((df_i / df_e) - 1) / yf;
        double N = 100000000;  // Notional amount
        #endregion
        // Calculate d1 and d2 
        double d1 = ((Math.Log(fwd / strike) + (sigma * sigma * T * .5)) / (sigma * Math.Sqrt(T)));
        double d2 = ((Math.Log(fwd / strike) - (sigma * sigma * T * .5)) / (sigma * Math.Sqrt(T)));

        // Lambda Expression for Cumulative Normal Distribution
        Func<double, double> CND_1 = x => Formula.CND_1(x);
        Func<double, double> CND_2 = x => Formula.CND_2(x);
        Func<double, double> CND_3 = x => Formula.CND_3(x);
        Func<double, double> CND_4 = x => Formula.CND_4(x);

        // Single Caplet using CapletBlack
        double px = Formula.CapletBlack(T, yf, N, strike, sigma, df_e, fwd);

        // Price using different Cumulative normal distribution
        double px1 = N * df_e * yf * (fwd * CND_1(d1) - strike * CND_1(d2));
        double px2 = N * df_e * yf * (fwd * CND_2(d1) - strike * CND_2(d2));
        double px3 = N * df_e * yf * (fwd * CND_3(d1) - strike * CND_3(d2));
        double px4 = N * df_e * yf * (fwd * CND_4(d1) - strike * CND_4(d2));

        // Printing Output
        Console.WriteLine("(method CapletBlack) Price: {0}", px);
        Console.WriteLine("CND algo 1, Price: {0} ", px1);
        Console.WriteLine("CND algo 2, Price: {0} ", px2);
        Console.WriteLine("CND algo 3, Price: {0} ", px3);
        Console.WriteLine("CND algo 4, Price: {0} ", px4);
    }

    public static void CapAsSumOfCaplets()
    {
        #region InputData
        double[] yf = new double[] { 0.25278, 0.25556, 0.25556 };
        double[] T = new double[] { 0.25, 0.50, 0.75 };
        double[] df = new double[] { 0.9940612, 0.9915250, 0.9890414 };
        double[] fwd = new double[3];
        // calculate fwd rate in traditional way (in multi curve framework it is not correct)
        fwd[0] = ((0.9969482 / df[0]) - 1) / yf[0];
        for (int i = 1; i < df.Length; i++)
        {
            fwd[i] = ((df[i - 1] / df[i]) - 1) / yf[i];
        }
        double K = 0.0102;
        double sigma = 0.2864;
        #endregion

        // Calculate the value of each caplet
        double CapPrice = 0.0;
        for (int i = 0; i < df.Length; i++)
        {
            double Caplet = Formula.CapletBlack(T[i], yf[i], 1, K, sigma, df[i], fwd[i]);  // each caplet
            CapPrice += Formula.CapletBlack(T[i], yf[i], 1, K, sigma, df[i], fwd[i]); // cumulative price
            Console.WriteLine("Caplet #: {0}  Price: {1:n7} Cumulated Price {2:n7}", i + 1, Caplet, CapPrice);
        }
        // Printing values
        Console.WriteLine("Price Cap as sum of Caplets: {0:n7}", CapPrice);
        Console.WriteLine("Price Cap as sum of Caplets: {0:n7}", Formula.CapBlack(T, yf, 1, K, sigma, df, fwd));

    }

    public static void SimpleBootstrapOneCaplet()
    {
        #region InputData
        double[] yf = new double[] { 0.25278, 0.25556, 0.25556, 0.25 };
        double[] T = new double[] { 0.25, 0.50, 0.75, 1.0 };
        double[] df = new double[] { 0.99406, 0.99153, 0.98904, 0.98633 };
        // One curve framework: traditional fwd calc
        double[] fwd = new double[4];
        fwd[0] = ((0.99695 / df[0]) - 1) / yf[0];
        for (int i = 1; i < df.Length; i++)
        {
            fwd[i] = ((df[i - 1] / df[i]) - 1) / yf[i];
        }
        double K = 0.0102;
        double sigma1Y3M = 0.2402;
        double sigma1Y = 0.2364;
        #endregion

        // Price of 1Y3M (18 months) Cap @ vol = sigma1Y3M
        double pxCap1Y3M = Formula.CapBlack(T, yf, 1, K, sigma1Y3M, df, fwd);

        // Price of 1Y Cap @ vol=sigma1Y
        double pxCap1Y = 0.0;
        for (int i = 0; i < df.Length - 1; i++)
        {
            pxCap1Y += Formula.CapletBlack(T[i], yf[i], 1, K, sigma1Y, df[i], fwd[i]);
        }

        // Price of 3M Caplet starting in 1Y
        double pxCaplet3M = pxCap1Y3M - pxCap1Y;

        // Inizialize Newton-Raphson algorithm to find Caplet implied vol
        NumMethod.myMethodDelegate fname =
                s => pxCaplet3M - Formula.CapletBlack(T[3], yf[3], 1, K, s, df[3], fwd[3]);

        // vol is the implied vol of 3M Caplet starting in 1Y. (0.20 is a starting guess)
        double implVol = NumMethod.NewRapNum(fname, 0.20);

        // array of caplet volatility (PieceWiseConstant under1Y)
        double[] capletVol = new double[] { sigma1Y, sigma1Y, sigma1Y, implVol };

        // printing results
        Console.WriteLine("1Y Cap Vol {0:p5}", sigma1Y);
        Console.WriteLine("1Y3M Cap Vol {0:p5}", sigma1Y3M);
        Console.WriteLine("Calculated Implied 3M Caplet Vol Starting in 1Y: {0:p5}", implVol);

        double pxCaplet = 0.0;
        double pxCap = 0.0;

        Console.WriteLine("\nUsing Caplet Volatilities");
        for (int i = 0; i < df.Length; i++)
        {
            pxCaplet = Formula.CapletBlack(T[i], yf[i], 1, K, capletVol[i], df[i], fwd[i]);
            pxCap += pxCaplet;
            Console.WriteLine("Caplet #: {0} CapletVol:{1:p4} Px: {2:n5} Cumulated Price: {3:n7}", i + 1, capletVol[i], pxCaplet, pxCap);
        }
        Console.WriteLine("Price Cap as sum of Caplets using Caplet Vol: {0:n7}", pxCap);

        // reset
        pxCaplet = 0.0;
        pxCap = 0.0;
        Console.WriteLine("\nUsing Cap Volatilities");
        for (int i = 0; i < df.Length; i++)
        {
            pxCaplet = Formula.CapletBlack(T[i], yf[i], 1, K, sigma1Y3M, df[i], fwd[i]);
            pxCap += pxCaplet;
            Console.WriteLine("Caplet #: {0} CapletVol:{1:p4} Px: {2:n5} Cumulated Price: {3:n7}", i + 1, sigma1Y3M, pxCaplet, pxCap);
        }

        Console.WriteLine("Price Cap as sum of Caplets using Cap Vol: {0:n7}", pxCap); // pxCap = pxCap1Y3M

    }

    public static void SimpleBootstrap()
    {
        #region data
        // Array of discount factor
        double[] df = new double[] { 0.994236269575566, 0.991625192926998, 0.989041421054714, 0.986231394133455, 0.983496974873945, 0.980646228901972, 0.977577789248498, 0.974244477497738, 0.97067019775667, 0.966860206840406, 0.96281976156406, 0.958481789220407, 0.953769370400552, 0.948855174348367, 0.94375920324068, 0.93838613470982, 0.93274533960824, 0.926957758932136, 0.92108389607756 };
        // array of yf of each caplet
        double[] yf = new double[] { 0.255555555555556, 0.261111111111111, 0.252777777777778, 0.244444444444444, 0.261111111111111, 0.252777777777778, 0.252777777777778, 0.252777777777778, 0.252777777777778, 0.252777777777778, 0.252777777777778, 0.252777777777778, 0.252777777777778, 0.255555555555556, 0.252777777777778, 0.252777777777778, 0.255555555555556, 0.255555555555556, 0.252777777777778 };
        // array of expiry of each caplet
        double[] T_1 = new double[] { 0.252054794520548, 0.50958904109589, 0.758904109589041, 1, 1.25753424657534, 1.50684931506849, 1.75616438356164, 2.00547945205479, 2.25479452054795, 2.5041095890411, 2.75342465753425, 3.0027397260274, 3.25205479452055, 3.5041095890411, 3.75342465753425, 4.0027397260274, 4.25479452054795, 4.50684931506849, 4.75616438356164 };
        int N = df.Length; // number of elements
        double[] fwd = new double[N]; // to contain forward rate
        double[] atmfwd = new double[N]; // to contain ATM forward Cap strike
        double[] capletVol = new double[N];  // to contain each caplet volatilities
        double[] capPrice = new double[N]; // to contain Cap price
        double df_ini = 0.99702;  // first discount factor  on T[0]
        // Cap Volatility from the market
        double[] capVol = new double[] { 0.5458, 0.5458, 0.5458, 0.55289, 0.557646575342466, 0.569168493150685, 0.582581643835616, 0.595015616438357, 0.563876164383561, 0.532736712328767, 0.501597260273972, 0.470798904109589, 0.470699178082192, 0.470598356164384, 0.470498630136986, 0.470348767123288, 0.465635342465753, 0.460921917808219, 0.456259726027397 };
        #endregion
        // calculate fwd
        fwd[0] = ((df_ini / df[0]) - 1) / yf[0];
        for (int i = 1; i < df.Length; i++)
        {
            fwd[i] = ((df[i - 1] / df[i]) - 1) / yf[i];
        }

        // calculate ATM strike
        double summ = 0.0;
        for (int i = 0; i < df.Length; i++)
        {
            summ += yf[i] * df[i];
            atmfwd[i] = (df_ini - df[i]) / summ;
        }

        double shorterCap = 0.0; // Shorter Cap is the same Cap without last caplet
        // calculate cap price using cal vol
        for (int i = 0; i < N; i++)
        {
            shorterCap = 0.0;

            // Note: here I use cap Vol
            for (int j = 0; j <= i; j++)
            {
                capPrice[i] += Formula.CapletBlack(T_1[j], yf[j], 100, atmfwd[i], capVol[i], df[j], fwd[j]);
            }
            // Note: here I use Caplet vol
            for (int j = 0; j < i; j++)
            {
                shorterCap += Formula.CapletBlack(T_1[j], yf[j], 100, atmfwd[i], capletVol[j], df[j], fwd[j]);
            }
            // Inizialize Newton Raphson solver
            NumMethod.myMethodDelegate fname =
                s => capPrice[i] - shorterCap - Formula.CapletBlack(T_1[i], yf[i], 100, atmfwd[i], s, df[i], fwd[i]);
            capletVol[i] = NumMethod.NewRapNum(fname, 0.20); // the missing caplet volatility
        }
        // Print results
        Console.WriteLine("Time   ATMCapStrike     CapletVol     CapVol");
        for (int i = 0; i < capletVol.Length; i++)
        {
            // Caplet expiry is T_1+yf (expiry + caplet year factor)
            Console.WriteLine("{0:N2}     {1:p5}     {2:p5}   {3:p5}", T_1[i] + yf[i],
                atmfwd[i], capletVol[i], capVol[i]);
        }
    }

    public static void SimpleBootstrap20Y()
    {

        #region data
        // We load a set of data (not real)
        DataForCapletExample1 data1 = new DataForCapletExample1();
        double[] df = data1.df();
        double[] yf = data1.yf();
        double[] T = data1.T();
        int N = df.Length;
        double[] fwd = new double[N];
        double[] atmfwd = new double[N];
        double[] capletVol = new double[N];
        double[] capPrice = new double[N];
        double df_ini = data1.df_ini;
        double[] capVol = data1.capVol();
        #endregion
        // calculate fwd
        fwd[0] = ((df_ini / df[0]) - 1) / yf[0];
        for (int i = 1; i < df.Length; i++)
        {
            fwd[i] = ((df[i - 1] / df[i]) - 1) / yf[i];
        }

        // calculate ATM strike
        double summ = 0.0;
        for (int i = 0; i < df.Length; i++)
        {
            summ += yf[i] * df[i];
            atmfwd[i] = (df_ini - df[i]) / summ;
        }

        double shorterCap = 0.0;
        // calculate cap price using flat vol
        for (int i = 0; i < N; i++)
        {
            shorterCap = 0.0;

            for (int j = 0; j <= i; j++)
            {
                capPrice[i] += Formula.CapletBlack(T[j], yf[j], 100, atmfwd[i], capVol[i], df[j], fwd[j]);
            }
            for (int j = 0; j < i; j++)
            {

                shorterCap += Formula.CapletBlack(T[j], yf[j], 100, atmfwd[i], capletVol[j], df[j], fwd[j]);
            }

            NumMethod.myMethodDelegate fname =
                s => capPrice[i] - shorterCap - Formula.CapletBlack(T[i], yf[i], 100, atmfwd[i], s, df[i], fwd[i]);
            capletVol[i] = NumMethod.NewRapNum(fname, 0.20);

        }

        #region print results
        ExcelMechanisms exl = new ExcelMechanisms();

        Vector<double> CapletVol = new Vector<double>(capletVol, 1);
        Vector<double> CapVol = new Vector<double>(capVol, 1);
        Vector<double> xarr = new Vector<double>(T, 1);
        List<string> labels = new List<string>() { "CapletVol", "CapVol" };
        List<Vector<double>> yarrs = new List<Vector<double>>() { CapletVol, CapVol };

        exl.printInExcel<double>(xarr, labels, yarrs, "Caplet vs Cap Vol", "Term", "Volatility");
        #endregion
    }

    public static void CapletVol20Y_InputInterp()
    {
        #region Data
        // We load a set of data (not real)
        DataForCapletExample1 d = new DataForCapletExample1();
        double[] df = d.df();
        double[] yf = d.yf();
        double[] T = d.T();
        double[] fwd = d.fwd();
        double[] atmfwd = d.atmfwd();
        double[] capVol = d.capVol();
        double[] avT = d.avT();
        double[] avcapVol = d.avcapVol();
        #endregion

        // All Data available
        double[] CapletVol = Formula.CapletVolBootstrapping(T, df, fwd, yf, capVol, atmfwd);

        // Cubic input
        SimpleCubicInterpolator CubicInt = new SimpleCubicInterpolator(avT, avcapVol);
        double[] cubicInterpCapVol = CubicInt.Curve(T);
        double[] CapletVolCubicInput = Formula.CapletVolBootstrapping(T, df, fwd, yf, cubicInterpCapVol, atmfwd);

        // Linear Input
        LinearInterpolator LinearInt = new LinearInterpolator(avT, avcapVol);
        double[] linearInterpCapVol = LinearInt.Curve(T);
        double[] CapletVolLinearInput = Formula.CapletVolBootstrapping(T, df, fwd, yf, linearInterpCapVol, atmfwd);

        #region print results
        ExcelMechanisms exl = new ExcelMechanisms();
        Vector<double> CapletVol_ = new Vector<double>(CapletVol, 1);
        Vector<double> CapletVolCubicInput_ = new Vector<double>(CapletVolCubicInput, 1);
        Vector<double> CapletVolLinearInput_ = new Vector<double>(CapletVolLinearInput, 1);
        Vector<double> xarr = new Vector<double>(T, 1);
        List<string> labels = new List<string>() { "CapletVol All input", "CapVol Cubic Input", "CapVol Linear Input" };
        List<Vector<double>> yarrs = new List<Vector<double>>() { CapletVol_, CapletVolCubicInput_, CapletVolLinearInput_ };

        exl.printInExcel<double>(xarr, labels, yarrs, "Caplet Vol Input Interpolation", "Term", "Volatility");
        #endregion
    }

    public static void VolOptimization()
    {
        ExampleCapVol e = new ExampleCapVol();
        e.Solve();
    }

    public static void MatrixCaplet()
    {
        #region Data
        // We load a set of data (not real)
        DataForCapletExample2 d = new DataForCapletExample2();
        double[] df = d.df();
        double[] yf = d.yf();
        double[] T = d.T();
        double[] avT = d.avT();
        List<double[]> VolArr = d.VolArr();
        double[] fwd = d.fwd();
        double[] strike = d.strike();

        List<double[]> VolSilos = new List<double[]>();
        foreach (double[] cVol in VolArr)
        {
            LinearInterpolator LinearInt = new LinearInterpolator(avT, cVol);
            VolSilos.Add(LinearInt.Curve(T));
        }

        #endregion
        // Here you can change the MonoStrikeCaplet Builder
        CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderPWC> B = new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderPWC>(T, df, fwd, yf, avT, VolSilos, strike);

        // CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitSmooth> B = new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitSmooth>(T, df, fwd, yf, avT, VolSilos, strike);
        // and more...

        #region print results
        ExcelMechanisms exl = new ExcelMechanisms();
        Vector<double> xarr = new Vector<double>(T, 1);
        List<string> labels = new List<string>();
        foreach (double myD in strike) { labels.Add(myD.ToString()); };
        List<Vector<double>> yarrs = new List<Vector<double>>();
        foreach (double[] arr in B.CapletVolMatrix)
        {
            yarrs.Add(new Vector<double>(arr, 1));
        }

        exl.printInExcel<double>(xarr, labels, yarrs, "Caplet Vol Input Interpolation", "Term", "Volatility");
        #endregion
    }

    public static void MatrixCapletWithRateCurve()
    {
        #region Inputs
        // We load a set of data (not real)
        DataForCapletExample2 d = new DataForCapletExample2();
        RateSet rs = d.GetRateSet();
        double[] T = d.T();
        string[] avT = d.avTString();
        List<double[]> VolArr = d.VolArr();
        double[] strike = d.strike();

        // We build our multi curve
        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> Curve = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(rs, OneDimensionInterpolation.LogLinear); // discount curve

        #endregion

        #region Testing all available MonoStrikeCapletVolBuilder
        // Uncomment one of these
        CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderInputInterpLinear> B =
            new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderInputInterpLinear>(avT, Curve, strike, VolArr);

        // CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderInputInterpCubic> B =
        //    new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderInputInterpCubic>(avT, Curve, strike, VolArr);

        // CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitStd> B =
        //    new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitStd>(avT, Curve, strike, VolArr);

        // CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitSmooth> B =
        //    new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitSmooth>(avT, Curve, strike, VolArr);

        // CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitPWL> B =
        //    new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitPWL>(avT, Curve, strike, VolArr);

        // CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitCubic> B =
        //    new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitCubic>(avT, Curve, strike, VolArr);

        // CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitFunct> B =
        //    new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitFunct>(avT, Curve, strike, VolArr);

        // CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderPWC> B =
        //    new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderPWC>(avT, Curve, strike, VolArr);

        #endregion

        List<double[]> VolSilos = B.CapletVolMatrix;

        #region print results
        ExcelMechanisms exl = new ExcelMechanisms();
        Vector<double> xarr = new Vector<double>(T, 1);
        List<string> labels = new List<string>();
        foreach (double myD in strike) { labels.Add(myD.ToString()); };
        List<Vector<double>> yarrs = new List<Vector<double>>();
        foreach (double[] arr in VolSilos)
        {
            yarrs.Add(new Vector<double>(arr, 1));
        }

        exl.printInExcel<double>(xarr, labels, yarrs, "Caplet Vol Input Interpolation", "Term", "Volatility");
        #endregion
    }

    #region Data For example

    public class ExampleCapVol
    {
        public double[] df; // discount factor
        public double[] yf; // year fractions
        public double[] T; // maturity
        public double[] fwd; // fwd rates
        public double[] atmfwd;  // ATM forward
        public double[] capPremium; // containers for cap premium
        public double[] x; // available maturity for market data
        public double[] y; // available Cap volatility from market

        public ExampleCapVol() { }

        public void Solve()
        {
            #region data
            DataForCapletExample1 d = new DataForCapletExample1();
            df = d.df();
            yf = d.yf();
            T = d.T();
            fwd = d.fwd();
            atmfwd = d.atmfwd();
            x = d.avT(); // available maturity for market data
            y = d.avcapVol();  // available Cap volatility from market
            capPremium = new double[x.Length];
            #endregion end data

            double cP = 0.0;
            // Calculate Cap price using Cap volatility available
            for (int i = 0; i < x.Length; i++)
            {
                int maxJ = Array.IndexOf(T, x[i]); // right index
                cP = 0.0;
                for (int j = 0; j <= maxJ; j++)
                {
                    cP += Formula.CapletBlack(T[j], yf[j], 1, atmfwd[maxJ], y[i], df[j], fwd[j]);
                }
                capPremium[i] = cP; // collecting values
            }

            #region Setting up minimisation
            // Starting missing caplet vol guess
            double[] VolGuess = Enumerable.Repeat(y[0], y.Length).ToArray();

            double epsg = 0.000000000001;  // original setting
            double epsf = 0;
            double epsx = 0;
            int maxits = 0;
            alglib.minlmstate state;
            alglib.minlmreport rep;

            // Number of equation to match
            int NConstrains = x.Length;

            // see alglib documentation
            alglib.minlmcreatev(NConstrains, VolGuess, 0.000001, out state);
            alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
            alglib.minlmoptimize(state, function_fvec, null, null);
            alglib.minlmresults(state, out VolGuess, out rep);
            #endregion

            // Minimisation Done!

            // Uncomment to change interpolator
            LinearInterpolator interp = new LinearInterpolator(x, VolGuess);
            // SimpleCubicInterpolator interp = new SimpleCubicInterpolator(x, VolGuess);

            double[] Vols = interp.Curve(T); // Vols from interpolator

            #region print results
            ExcelMechanisms exl = new ExcelMechanisms();
            Vector<double> CapVol = new Vector<double>(Vols, 1);
            Vector<double> xarr = new Vector<double>(T, 1);
            List<string> labels = new List<string>() { "CapVol" };
            List<Vector<double>> yarrs = new List<Vector<double>>() { CapVol };

            exl.printInExcel<double>(xarr, labels, yarrs, "Caplet vs Cap Vol", "Term", "Volatility");
            #endregion
        }

        // Target function to be minimised
        public void function_fvec(double[] VolGuess, double[] fi, object obj)
        {
            // Uncomment to change interpolator
            // LinearInterpolator interp = new LinearInterpolator(x, VolGuess);
            SimpleCubicInterpolator interp = new SimpleCubicInterpolator(x, VolGuess);

            double[] Vols = interp.Curve(T); // interpolated caplet vols

            double cP = 0.0;
            // Calculate Cap price using Caplet volatility
            for (int i = 0; i < x.Length; i++)
            {
                int maxJ = Array.IndexOf(T, x[i]); // right index
                cP = 0.0;
                for (int j = 0; j <= maxJ; j++)
                {
                    cP += Formula.CapletBlack(T[j], yf[j], 1, atmfwd[maxJ], Vols[j], df[j], fwd[j]);
                }
                fi[i] = (capPremium[i] - cP) * 10000000; // minimise it!            
            }
        }
    }

    public class DataForCapletExample1
    {
        // Data used in examples (not real data)
        public double df_ini;
        double[] df_;
        double[] yf_;
        double[] T_;
        double[] capVol_;
        double[] avT_;  // available T
        double[] avcapVol_; // available capVol

        public DataForCapletExample1()
        {
            Inizialize();
        }
        void Inizialize()
        {
            df_ = new double[] { 0.994061226803011, 0.99152500666756, 0.989041421054714, 0.986334217080995, 0.983565787958503, 0.980693027218412, 0.977648294535883, 0.974282426374397, 0.97067019775667, 0.966860206840406, 0.962865395440211, 0.958481789220407, 0.953769370400552, 0.948855174348367, 0.94375920324068, 0.93838613470982, 0.93274533960824, 0.926957758932136, 0.92108389607756, 0.914883216002649, 0.908459262749902, 0.901963582887592, 0.895476653471404, 0.888761746051184, 0.881930497011907, 0.875116023504023, 0.868375537217958, 0.861480194904567, 0.854555500105515, 0.847702428993889, 0.840870734855477, 0.833990650471774, 0.827124229832796, 0.820345013670732, 0.813651744648136, 0.806820361322964, 0.800000437950235, 0.793267800370842, 0.786623845218985, 0.779845200575829, 0.773079499109701, 0.766404787927508, 0.75982507565322, 0.753099721889635, 0.746380301350497, 0.739783688760851, 0.733280109938752, 0.726800943925917, 0.720395321818202, 0.714139135088177, 0.70803701125111, 0.701879627959937, 0.695799605626669, 0.689876039094944, 0.684119824943094, 0.678339701353932, 0.672654273413933, 0.667124614401166, 0.66174843480308, 0.656337950778115, 0.651012256184437, 0.645841448575001, 0.640780573528562, 0.6357793804215, 0.63088127866917, 0.626127073217148, 0.621503342301534, 0.616851772532891, 0.612278881346368, 0.607835068245372, 0.603519646140484, 0.599186275823962, 0.594928083574279, 0.590792257910359, 0.586778368011654, 0.582758274829764, 0.578816482477501, 0.574981380418226, 0.571196707557129 };
            yf_ = new double[] { 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.252777777777778, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.252777777777778, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.252777777777778, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.252777777777778, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.25, 0.255555555555556, 0.255555555555556, 0.252777777777778, 0.252777777777778 };

            T_ = new double[] { 0.252054794520548, 0.504109589041096, 0.753424657534247, 1, 1.25205479452055, 1.5041095890411, 1.75342465753425, 2, 2.25205479452055, 2.5041095890411, 2.75342465753425, 3, 3.25205479452055, 3.5041095890411, 3.75342465753425, 4.0027397260274, 4.25479452054795, 4.50684931506849, 4.75616438356164, 5.0027397260274, 5.25479452054795, 5.50684931506849, 5.75616438356164, 6.0027397260274, 6.25479452054795, 6.50684931506849, 6.75616438356164, 7.0027397260274, 7.25479452054795, 7.50684931506849, 7.75616438356164, 8.00547945205479, 8.25753424657534, 8.50958904109589, 8.75890410958904, 9.00547945205479, 9.25753424657534, 9.50958904109589, 9.75890410958904, 10.0054794520548, 10.2575342465753, 10.5095890410959, 10.758904109589, 11.0054794520548, 11.2575342465753, 11.5095890410959, 11.758904109589, 12.0082191780822, 12.2602739726027, 12.5123287671233, 12.7616438356164, 13.0082191780822, 13.2602739726027, 13.5123287671233, 13.7616438356164, 14.0082191780822, 14.2602739726027, 14.5123287671233, 14.7616438356164, 15.0082191780822, 15.2602739726027, 15.5123287671233, 15.7616438356164, 16.0109589041096, 16.2630136986301, 16.5150684931507, 16.7643835616438, 17.0109589041096, 17.2630136986301, 17.5150684931507, 17.7643835616438, 18.0109589041096, 18.2630136986301, 18.5150684931507, 18.7643835616438, 19.0109589041096, 19.2630136986301, 19.5150684931507, 19.7643835616438 };
            capVol_ = new double[] { 0.205135842248575, 0.205135842248575, 0.205135842248575, 0.220628894800034, 0.236133979980227, 0.251629065263795, 0.267128316813581, 0.270188285731401, 0.273257189445466, 0.27632352211265, 0.279379486110683, 0.27791376970134, 0.2764479584576, 0.274970344401688, 0.273500456886727, 0.271602296516361, 0.269352735110785, 0.267105164756566, 0.264851839804836, 0.262849398230765, 0.260849747828364, 0.259000762938231, 0.257161596636201, 0.255412996979037, 0.253657719393335, 0.251912677513857, 0.250162507528085, 0.248472646853184, 0.246781994783719, 0.24509610828349, 0.243408331371291, 0.24175797793697, 0.24010432089165, 0.23844621607183, 0.236793045348183, 0.235351499555079, 0.233917611887853, 0.232475318462653, 0.231037164345184, 0.22966253401142, 0.228293680822561, 0.227033397477029, 0.225783719241742, 0.224536733734123, 0.223285431912538, 0.222080768683029, 0.220860307156708, 0.219733275474908, 0.218611352511583, 0.21748547929584, 0.216364823290215, 0.215272610166606, 0.214177277760494, 0.213085872022165, 0.211986218665705, 0.210863929686764, 0.209739522109465, 0.208606757174202, 0.207490835516159, 0.206384222628012, 0.20528639504484, 0.204185133320935, 0.20308622517027, 0.201996019005355, 0.200921404435318, 0.199847612620772, 0.198770188186297, 0.197702814056759, 0.196627690095576, 0.195573311890727, 0.194525801961007, 0.193479921599902, 0.192480272086751, 0.19148074026321, 0.19048064892887, 0.189479989490711, 0.188471533473748, 0.187474160183547, 0.186468873976134 };

            avT_ = new double[] { 0.252054794520548, 0.504109589041096, 0.753424657534247, 1.75342465753425, 2.75342465753425, 3.75342465753425, 4.75616438356164, 5.75616438356164, 6.75616438356164, 7.75616438356164, 8.75890410958904, 9.75890410958904, 10.758904109589, 11.758904109589, 12.7616438356164, 13.7616438356164, 14.7616438356164, 15.7616438356164, 16.7643835616438, 17.7643835616438, 18.7643835616438, 19.7643835616438 };

            avcapVol_ = new double[avT_.Length];

            for (int i = 0; i < avT_.Length; i++)
            {
                int k = Array.IndexOf(T_, avT_[i]);
                avcapVol_[i] = capVol_[k];
            }

            df_ini = 0.996948176;
        }
        public double[] df()
        {
            return df_;
        }
        public double[] yf()
        {
            return yf_;
        }
        public double[] T()
        {
            return T_;
        }
        public double[] capVol()
        {
            return capVol_;
        }
        public double[] avT() { return avT_; }
        public double[] avcapVol() { return avcapVol_; }
        public double[] fwd()
        {
            double[] df = this.df();
            double[] yf = this.yf();
            double[] fwd_ = new double[df.Length];
            // calculate fwd
            fwd_[0] = ((df_ini / df[0]) - 1) / yf[0];
            for (int i = 1; i < df.Length; i++)
            {
                fwd_[i] = ((df[i - 1] / df[i]) - 1) / yf[i];
            }
            return fwd_;
        }

        public double[] atmfwd()
        {
            double[] df = this.df();
            double[] yf = this.yf();
            double[] atmfwd_ = new double[df.Length];
            // calculate ATM strike
            double summ = 0.0;
            for (int i = 0; i < df.Length; i++)
            {
                summ += yf[i] * df[i];
                atmfwd_[i] = (df_ini - df[i]) / summ;
            }
            return atmfwd_;
        }
    }

    public class DataForCapletExample2
    {
        // Data used in examples (not real data)
        public double[] df() { return new double[] { 0.989158819, 0.98453947, 0.979584136, 0.973635573, 0.966828048, 0.958544648, 0.949187574, 0.938671435, 0.927409672, 0.915253363, 0.902732073, 0.889639808, 0.87654193, 0.863235341, 0.850053478, 0.836751164, 0.823702907, 0.810502912, 0.797583528, 0.784522817, 0.771761841, 0.758844333, 0.746271559, 0.733920369, 0.72206175, 0.710264711, 0.698992596, 0.687951392, 0.677437896, 0.667044529, 0.657127392, 0.647542713, 0.638468752, 0.629550732, 0.621087576, 0.612790745, 0.604927626, 0.597242662, 0.58991164, 0.582755129, 0.575930246, 0.569180262, 0.56270732, 0.556252485, 0.550076088, 0.544035969, 0.538204654, 0.532366517, 0.526744099, 0.521213461, 0.515866433, 0.510438588, 0.505175084, 0.49996491, 0.494878383, 0.489745341, 0.484727461, 0.479666584, 0.474713436 }; }
        public double[] yf() { return new double[] { 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.505555556, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.505555556, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.505555556, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.505555556, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.505555556, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.505555556, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.502777778, 0.511111111, 0.505555556, 0.511111111, 0.502777778, 0.511111111, 0.502777778 }; }
        public double[] T() { return new double[] { 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5, 9, 9.5, 10, 10.5, 11, 11.5, 12, 12.5, 13, 13.5, 14, 14.5, 15, 15.5, 16, 16.5, 17, 17.5, 18, 18.5, 19, 19.5, 20, 20.5, 21, 21.5, 22, 22.5, 23, 23.5, 24, 24.5, 25, 25.5, 26, 26.5, 27, 27.5, 28, 28.5, 29, 29.5 }; }
        public double[] avT() { return new double[] { 0.5, 1, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5, 7.5, 8.5, 10.5, 11.5, 14.5, 19.5, 24.5, 29.5 }; }
        public string[] avTString() { return new string[] { "1Y", "18M", "2Y", "3Y", "4Y", "5Y", "6Y", "7Y", "8Y", "9Y", "10Y", "12Y", "15Y", "20Y", "25Y", "30Y" }; }
        public List<double[]> VolArr()
        {
            List<double[]> Vol = new List<double[]>();
            Vol.Add(new double[] { 0.609, 0.626, 0.648, 0.522, 0.527, 0.527, 0.518, 0.508, 0.5, 0.491, 0.481, 0.465, 0.447, 0.427, 0.421, 0.419 });
            Vol.Add(new double[] { 0.636, 0.65, 0.666, 0.511, 0.496, 0.482, 0.465, 0.451, 0.44, 0.43, 0.42, 0.403, 0.386, 0.369, 0.366, 0.365 });
            Vol.Add(new double[] { 0.66, 0.672, 0.686, 0.506, 0.477, 0.45, 0.428, 0.41, 0.398, 0.387, 0.376, 0.36, 0.343, 0.329, 0.328, 0.328 });
            Vol.Add(new double[] { 0.672, 0.683, 0.695, 0.505, 0.47, 0.439, 0.414, 0.395, 0.381, 0.37, 0.36, 0.343, 0.326, 0.314, 0.313, 0.314 });
            Vol.Add(new double[] { 0.683, 0.693, 0.703, 0.504, 0.465, 0.429, 0.402, 0.382, 0.368, 0.356, 0.345, 0.329, 0.312, 0.3, 0.301, 0.303 });
            Vol.Add(new double[] { 0.705, 0.712, 0.72, 0.503, 0.458, 0.415, 0.385, 0.363, 0.347, 0.335, 0.324, 0.306, 0.29, 0.28, 0.282, 0.284 });
            Vol.Add(new double[] { 0.726, 0.73, 0.735, 0.504, 0.455, 0.407, 0.375, 0.351, 0.334, 0.321, 0.309, 0.291, 0.275, 0.266, 0.269, 0.272 });
            Vol.Add(new double[] { 0.746, 0.747, 0.749, 0.505, 0.454, 0.402, 0.369, 0.343, 0.326, 0.312, 0.3, 0.281, 0.265, 0.257, 0.26, 0.264 });
            Vol.Add(new double[] { 0.765, 0.764, 0.762, 0.507, 0.454, 0.4, 0.366, 0.339, 0.321, 0.307, 0.294, 0.274, 0.258, 0.252, 0.255, 0.258 });
            Vol.Add(new double[] { 0.784, 0.779, 0.774, 0.51, 0.455, 0.399, 0.364, 0.338, 0.318, 0.304, 0.291, 0.271, 0.255, 0.249, 0.252, 0.255 });
            Vol.Add(new double[] { 0.818, 0.807, 0.796, 0.535, 0.457, 0.399, 0.364, 0.338, 0.318, 0.303, 0.289, 0.268, 0.253, 0.247, 0.25, 0.252 });
            Vol.Add(new double[] { 0.848, 0.832, 0.816, 0.571, 0.461, 0.401, 0.367, 0.34, 0.32, 0.304, 0.291, 0.269, 0.254, 0.247, 0.25, 0.252 });
            Vol.Add(new double[] { 0.924, 0.894, 0.845, 0.592, 0.49, 0.408, 0.376, 0.35, 0.33, 0.314, 0.29, 0.277, 0.262, 0.255, 0.254, 0.253 });
            return Vol;
        }

        public double[] fwd() { return new double[] { 0.00899, 0.00918, 0.01006, 0.01195, 0.014, 0.01692, 0.01951, 0.02192, 0.02416, 0.02599, 0.02759, 0.02879, 0.02972, 0.03016, 0.03067, 0.03111, 0.03151, 0.03187, 0.03222, 0.03257, 0.03289, 0.03331, 0.03332, 0.03293, 0.03267, 0.0325, 0.03207, 0.0314, 0.03087, 0.03048, 0.02985, 0.02896, 0.02827, 0.02772, 0.0271, 0.02649, 0.02585, 0.02518, 0.02458, 0.02403, 0.02357, 0.0232, 0.02288, 0.0227, 0.02233, 0.02172, 0.02143, 0.02146, 0.02123, 0.02076, 0.02062, 0.02081, 0.02072, 0.02039, 0.02033, 0.02051, 0.02059, 0.02064, 0.02075 }; }

        public double[] strike() { return new double[] { 0.01, 0.015, 0.02, 0.0225, 0.025, 0.03, 0.035, 0.04, 0.045, 0.05, 0.06, 0.07, 0.10 }; }

        public RateSet GetRateSet()
        {
            // Reference date
            Date refDate = (new Date(DateTime.Now)).mod_foll();  // Date refDate = new Date(2012, 3, 5);

            #region Swap Market Data
            // RateSet EUR 6m swap
            RateSet rs = new RateSet(refDate);
            rs.Add(1.256e-2, "6m", BuildingBlockType.EURDEPO);
            rs.Add(1.076e-2, "1y", BuildingBlockType.EURSWAP6M);
            rs.Add(1.017e-2, "2y", BuildingBlockType.EURSWAP6M);
            rs.Add(1.11e-2, "3y", BuildingBlockType.EURSWAP6M);
            rs.Add(1.289e-2, "4y", BuildingBlockType.EURSWAP6M);
            rs.Add(1.489e-2, "5y", BuildingBlockType.EURSWAP6M);
            rs.Add(1.682e-2, "6y", BuildingBlockType.EURSWAP6M);
            rs.Add(1.853e-2, "7y", BuildingBlockType.EURSWAP6M);
            rs.Add(1.995e-2, "8y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.113e-2, "9y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.214e-2, "10y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.301e-2, "11y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.378e-2, "12y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.439e-2, "13y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.488e-2, "14y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.524e-2, "15y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.551e-2, "16y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.567e-2, "17y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.576e-2, "18y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.579e-2, "19y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.577e-2, "20y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.571e-2, "21y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.563e-2, "22y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.554e-2, "23y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.543e-2, "24y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.532e-2, "25y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.52e-2, "26y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.509e-2, "27y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.498e-2, "28y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.488e-2, "29y", BuildingBlockType.EURSWAP6M);
            rs.Add(2.479e-2, "30y", BuildingBlockType.EURSWAP6M);
            #endregion

            return rs;
        }
    }

    #endregion

    #region Extra
    public static void SwaptionSimple()
    {
        // Start input            
        // ref date
        Date refDate = new Date(2012, 3, 16);

        #region Swap Market Data
        // RateSet EUR 6m swap
        RateSet rs = new RateSet(refDate);
        rs.Add(1.256e-2, "6m", BuildingBlockType.EURDEPO);
        rs.Add(1.076e-2, "1y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.017e-2, "2y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.11e-2, "3y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.289e-2, "4y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.489e-2, "5y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.682e-2, "6y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.853e-2, "7y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.995e-2, "8y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.113e-2, "9y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.214e-2, "10y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.301e-2, "11y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.378e-2, "12y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.439e-2, "13y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.488e-2, "14y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.524e-2, "15y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.551e-2, "16y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.567e-2, "17y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.576e-2, "18y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.579e-2, "19y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.577e-2, "20y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.571e-2, "21y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.563e-2, "22y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.554e-2, "23y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.543e-2, "24y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.532e-2, "25y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.52e-2, "26y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.509e-2, "27y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.498e-2, "28y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.488e-2, "29y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.479e-2, "30y", BuildingBlockType.EURSWAP6M);
        #endregion

        // I build curve, 
        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> Curve = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(rs, OneDimensionInterpolation.LogLinear); // discount curve

        double p = Formula.Swaption(1, 0.01, "1y", "3y", true, 0.50, Curve);
        Console.WriteLine(p);
        #region Hull example
        double[] yf = new double[] { 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };
        double[] df = new double[]{Math.Exp(-0.06*5.5),Math.Exp(-0.06*6),Math.Exp(-0.06*6.5),
            Math.Exp(-0.06*7),Math.Exp(-0.06*7.5),Math.Exp(-0.06*8)};
        double pp = Formula.Swaption(100, 0.0609, 0.062, 0.2, 5, true, yf, df);
        #endregion
        Console.WriteLine(pp);
    }
    #endregion

}