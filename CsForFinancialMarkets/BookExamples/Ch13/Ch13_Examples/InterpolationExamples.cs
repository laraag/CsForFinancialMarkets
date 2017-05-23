// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// InterpolationExamples.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class InterpolationMethods
{
    public static void Main()
    {
        // Uncomment to run 
        // Example101();
        // SimpleFormulas();
        // LogisticI();
        // LogisticII();
        // BilinearInterpolation1();
        // BilinearInterpolation2();
    }

    // 13.12.1	The 101 Example, from A to Z
    public static void Example101()
    {
        // My excel mechanism
        ExcelMechanisms exl = new ExcelMechanisms();

        // I Create initial t and r arrays.
        Vector<double> t = new Vector<double>(new double[] { 0.1, 1, 4, 9, 20, 30 }, 0);
        Vector<double> r = new Vector<double>(new double[] { 0.081, 0.07, 0.044, 0.07, 0.04, 0.03 }, 0);

        // Compute log df
        Vector<double> logDF = new Vector<double>(r.Size, r.MinIndex);
        for (int n = logDF.MinIndex; n <= logDF.MaxIndex; ++n)
        {
            logDF[n] = Math.Log(Math.Exp(-t[n] * r[n]));
        }
        // exl.printOneExcel<double>(t, logDF, "logDF", "time", "logDF", "logDF");

        // II Hyman interpolator 
        HymanHermiteInterpolator_V4 myInterpolatorH
                        = new HymanHermiteInterpolator_V4(t, logDF);

        // Create the abscissa values f (hard-coded for the moment)
        int M = 299;
        Vector<double> term = new Vector<double>(M, 1);
        term[term.MinIndex] = 0.1;
        double step = 0.1;
        for (int j = term.MinIndex + 1; j <= term.MaxIndex; j++)
        {
            term[j] = term[j - 1] + step;
        }

        // III Compute interpolated values
        Vector<double> interpolatedlogDFH = myInterpolatorH.Curve(term);
        // exl.printOneExcel<double>(term, interpolatedlogDFH,
        //                "Hyman cubic", "time", "int logDF", "int logDF");

        // IV Compute continuously compounded risk free rate from the ZCB Z(0,t),
        // using equation (3)Hagan and West (2008).
        Vector<double> rCompounded = new Vector<double>(interpolatedlogDFH.Size,
                       interpolatedlogDFH.MinIndex);

        for (int j = rCompounded.MinIndex; j <= rCompounded.MaxIndex; j++)
        {
            rCompounded[j] = -interpolatedlogDFH[j] / term[j];
        }
        exl.printOneExcel<double>(term, rCompounded,
        "RCompound Hyman Cubic", "time", "r continuously comp.", "r cont");

        // V Compute discrete forward rates using equation (6) from Hagan and West (2008)
        Vector<double> f = new Vector<double>(rCompounded.Size,
                            rCompounded.MinIndex);
        f[f.MinIndex] = 0.081;

        for (int j = f.MinIndex + 1; j <= rCompounded.MaxIndex; j++)
        {
            f[j] = (rCompounded[j] * term[j] - rCompounded[j - 1]
                        * term[j - 1]) / (term[j] - term[j - 1]);
        }
        exl.printOneExcel<double>(term, f, "Hyman Cubic", "time", "discrete forward", "dis fwd");
    }

    // 13.12.2	Some Financial Formulas
    #region SimpleFormulas()
    static public void SimpleFormulas()
    {
        // Simple formulas based on Hagan West 2008
        double[] term = new double[] { 0.1, 1, 4, 9, 20, 30 };
        double[] zeroRate = new double[] { 0.081, 0.07, 0.05, 0.07, 0.04, 0.03 };
        double[] capFactor = CapitalisationFactor(term, zeroRate);
        double[] disFwd = DiscreteForward(term, capFactor);
        double[] discount = Discount(capFactor);
        double[] logDiscount = LogDiscount(discount);
        for (int i = 0; i < term.Length; i++)
        {
            Console.WriteLine("[A]: {0:F2}, [B]: {1}, [C]: {2:F4}, [D]: {3:F4}, [E]: {4:F4}, [F]: {5:F4} ", term[i], zeroRate[i], capFactor[i], disFwd[i], discount[i], logDiscount[i]);
        }
        Console.WriteLine();
        Console.WriteLine("[A] = Term; [B] = ContYield; [C] = CapFactor; [D] = DisFWD, [E] = discount; [F] = log of discount");
    }

    // Capitalisation from contYield given term, using equation (1) from Hagan and West (2008) 
    static public double[] CapitalisationFactor(double[] term, double[] contYield)
    {
        int n = term.Length;
        double[] output = new double[n];
        for (int i = 0; i < n; i++)
        {
            output[i] = Math.Exp(term[i] * contYield[i]);
        }
        return output;
    }

    // Calculate Discrete FWD using equation (5) from Hagan and West (2008) 
    static public double[] DiscreteForward(double[] term, double[] capFactor)
    {
        int n = term.Length;
        List<double> t = new List<double>(term);
        List<double> c = new List<double>(capFactor);
        double[] outPut = new double[n];
        t.Insert(0, 0.0);
        c.Insert(0, 1.0);

        for (int i = 0; i < n; i++)
        {
            outPut[i] = -Math.Log(c[i] / c[i + 1]) / (t[i + 1] - t[i]); // 

        }
        return outPut;
    }

    // Calculate the discount factor from the capitalization factor using equation (2)
    // from Hagan and West (2008)
    static public double[] Discount(double[] capFactor)
    {
        int n = capFactor.Length;
        double[] output = new double[n];
        for (int i = 0; i < n; i++)
        {
            output[i] = 1.0 / capFactor[i];  // it 1/capitalization factor
        }
        return output;
    }

    // Calculate the logarithm of discount factor
    static public double[] LogDiscount(double[] df)
    {
        int n = df.Length;
        double[] output = new double[n];
        for (int i = 0; i < n; i++)
        {
            output[i] = -Math.Log(df[i]);
        }
        return output;
    }
    #endregion

    // 13.12.3	Cubic Spline Interpolation
    #region CubicSplineInterpolator
    public static void LogisticI()
    {
        Console.WriteLine("Vectors initialised: ");

        int N = 280;
        double a = -15.0;		 // Left of interval
        double b = 15.0;		 // Right of interval
        double h = (b - a) / N;
        int startIndex = 0;

        Vector<double> xarr = new Vector<double>(N + 1, startIndex, 0.0);
        Vector<double> yarr = new Vector<double>(N + 1, startIndex, 0.0);

        for (int j = xarr.MinIndex; j <= xarr.MaxIndex; j++)
        {
            xarr[j] = a + h * j;
            yarr[j] = Potpourri.Sigmoid1(xarr[j]);
            // yarr[j] = Potpourri.Sigmoid2(xarr[j]);
        }

        Console.WriteLine("Vectors initialised: ");

        int FirstDeriv = 1;

        CubicSplineInterpolator csi
                    = new CubicSplineInterpolator(xarr, yarr, FirstDeriv, 10, 10);

        // Display arrays in Excel
        ExcelMechanisms exl = new ExcelMechanisms();
        exl.printOneExcel<double>(xarr, csi.Curve(), "Logistic I", "x", "value", "I");

        // Now choose 1st order derivative at zero
        double leftBC = 1.0;
        double rightBC = -1.0;
        CubicSplineInterpolator csi2 = new CubicSplineInterpolator(xarr, yarr, FirstDeriv, leftBC, rightBC);

        // Display arrays in Excel
        exl.printOneExcel(xarr, csi2.Curve(), "Logistic II", "x", "value", "II");
    }

    public static void LogisticII()
    {
        Console.WriteLine("Vectors initialised: ");

        int N = 280;
        double a = 0.0;		 // Left of interval
        double b = 14.0;		 // Right of interval
        double h = (b - a) / N;
        int startIndex = 0;

        Vector<double> xarr = new Vector<double>(N + 1, startIndex, 0.0);
        Vector<double> yarr = new Vector<double>(N + 1, startIndex, 0.0);

        for (int j = xarr.MinIndex; j <= xarr.MaxIndex; j++)
        {
            xarr[j] = a + h * j;
            yarr[j] = Potpourri.Sigmoid2(xarr[j]);
        }

        Console.WriteLine("Vectors initialised: ");

        int FirstDeriv = 1;

        CubicSplineInterpolator csi
                    = new CubicSplineInterpolator(xarr, yarr, FirstDeriv, 10, 10);

        // Display arrays in Excel
        ExcelMechanisms exl = new ExcelMechanisms();
        exl.printOneExcel<double>(xarr, csi.Curve(), "Logistic I", "x", "value", "I");

        // Now choose 1st order derivative at zero
        double leftBC = 1.0;
        double rightBC = -1.0;
        CubicSplineInterpolator csi2 = new CubicSplineInterpolator(xarr, yarr, FirstDeriv, leftBC, rightBC);

        // Display arrays in Excel
        exl.printOneExcel(xarr, csi2.Curve(), "Logistic II", "x", "value", "II");
    }

    public class Potpourri
    {
        public static double func(double x)
        {
            return x * x;
        }

        public static double NormalPdf(double x)
        {  // Probability function for Gauss fn.

            double A = 1.0 / Math.Sqrt(2.0 * 3.1415);
            return A * Math.Exp(-x * x * 0.5);
        }

        public static double Sigmoid1(double t)
        {
            return 1.0 / (1.0 + Math.Exp(-t));
        }

        public static double Sigmoid2(double t)
        {

            double a = 0.6;
            double b = 0.15;
            double c = 10.0;
            double d = -0.3;

            return a + (b - a) / (1.0 + Math.Exp(-c * (Math.Log(t) - d)));
        }

        public static double NormalCdf(double x)
        {  // The approximation to the cumulative normal distribution

            double a1 = 0.4361836;
            double a2 = -0.1201676;
            double a3 = 0.9372980;

            double k = 1.0 / (1.0 + (0.33267 * x));

            if (x >= 0.0)
            {
                return 1.0 - NormalPdf(x) *
    (a1 * k + (a2 * k * k) + (a3 * k * k * k));
            }
            else
            {
                return 1.0 - NormalCdf(-x);
            }
        }

        public static double func(double x, double y)
        {
            return Math.Exp(-x * x - y * y) + 1.0;
        }

        public static double BinormalPdf(double x, double y, double rho)
        {  // Probability function for Gauss fn.

            double A = 1.0 / (2.0 * 3.1415) * Math.Sqrt(1.0 - rho * rho);
            double a = 1.0 / (2.0 * (1.0 - rho * rho));

            return A * Math.Exp(-a * (x * x - 2.0 * rho * x * y + y * y));
        }
    }
    #endregion

    // 13.12.4	Bilinear Interpolation
    public static void BilinearInterpolation1()
    {
        Console.WriteLine("Vectors initialised: ");

        // Create mesh arrays
        int startIndex = 0;

        int N = 1;
        Vector<double> x1arr = new Vector<double>(N + 1, startIndex, 0.0);
        x1arr[0] = 20.0; x1arr[1] = 21.0;

        Vector<double> x2arr = new Vector<double>(x1arr);
        x2arr[0] = 14.0; x2arr[1] = 15.0;

        Console.WriteLine(x1arr);
        Console.WriteLine(x2arr);

        // Control values; 
        NumericMatrix<double> Control
                = new NumericMatrix<double>(N + 1, N + 1, startIndex, startIndex);
        Control[0, 0] = 91.0; Control[1, 1] = 95.0;
        Control[0, 1] = 210.0; Control[1, 0] = 162.0;

        BilinearInterpolator myInterpolator
                = new BilinearInterpolator(x1arr, x2arr, Control);

        double x = 20.2; double y = 14.5;  // 146.1

        double value = myInterpolator.Solve(x, y);
        Console.WriteLine("Interpolated value: {0}", value);
    }

    public static void BilinearInterpolation2()
    {

        // Create mesh arrays
        int startIndex = 0;

        // Number of subdivisions N,M in the x and y directions
        int N = 4;
        int M = 3;
        Vector<double> x1arr = new Vector<double>(N + 1, startIndex, 0.0);

        double a = 0.0; double b = 1.0;

        double h1 = (b - a) / (double)N;
        x1arr[x1arr.MinIndex] = a;

        for (int j = x1arr.MinIndex + 1; j <= x1arr.MaxIndex; j++)
        {
            x1arr[j] = x1arr[j - 1] + h1;
        }

        Vector<double> x2arr = new Vector<double>(M + 1, startIndex, 0.0);
        double h2 = (b - a) / (double)M;
        x1arr[x1arr.MinIndex] = a;

        for (int j = x2arr.MinIndex + 1; j <= x2arr.MaxIndex; j++)
        {
            x2arr[j] = x2arr[j - 1] + h2;
        }
        Console.WriteLine(x1arr);
        Console.WriteLine(x2arr);

        // Control values; 
        NumericMatrix<double> Control
                        = new NumericMatrix<double>(N + 1, M + 1,
                        startIndex, startIndex);

        Func<double, double, double> func = (double x1, double x2) => x1 + x2;

        for (int i = Control.MinRowIndex; i <= Control.MaxRowIndex; i++)
        {
            for (int j = Control.MinColumnIndex; j <= Control.MaxColumnIndex; j++)
            {
                Control[i, j] = func(x1arr[i], x2arr[j]);
            }
        }

        BilinearInterpolator myInterpolator
                = new BilinearInterpolator(x1arr, x2arr, Control);

        double x = 0.1; double y = 0.7;
        double value = myInterpolator.Solve(x, y);
        Console.WriteLine("Interpolated value: {0}", value);

        // Take center point (xm, ym) of each element and interpolate on it
        NumericMatrix<double> InterpolatedMatrix
                = new NumericMatrix<double>(N, M, startIndex, startIndex);

        // Abscissa points of new interpolated matrix
        Vector<double> Xarr
                = new Vector<double>(InterpolatedMatrix.Rows, startIndex, 0.0);
        Vector<double> Yarr
                = new Vector<double>(InterpolatedMatrix.Columns, startIndex, 0.0);

        for (int i = InterpolatedMatrix.MinRowIndex;
             i <= InterpolatedMatrix.MaxRowIndex; i++)
        {
            for (int j = InterpolatedMatrix.MinColumnIndex;
                 j <= InterpolatedMatrix.MaxColumnIndex; j++)
            {
                Xarr[i] = 0.5 * (x1arr[i] + x1arr[i + 1]);     // xm   
                Yarr[j] = 0.5 * (x2arr[j] + x2arr[j + 1]);   // ym

                InterpolatedMatrix[i, j]
                        = myInterpolator.Solve(Xarr[i], Yarr[j]);
            }
        }

        // Present the interpolated matrix in Excel
        ExcelMechanisms driver = new ExcelMechanisms();

        string title = "Interpolated matrix";
        driver.printMatrixInExcel<double>(InterpolatedMatrix,
                                Xarr, Yarr, title);
    }
}