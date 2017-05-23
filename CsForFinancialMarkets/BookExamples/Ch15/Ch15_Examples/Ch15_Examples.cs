// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Ch15_Examples.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TestSingleCurveBuilder
{
    public static void Main()
    {
        //TestVanillaSwapFloatingLegNPV();
        //CheckInputsVs6m();
        //CheckInputsVs3m();
        //TimeForBestFitVs6m();
        //TimeForBestFitVs3m();
        //CheckFwdRatesVs6m();
        //CheckFwdRatesVs3m();
        //CheckFwdRatesOIS3m();
        //FwdStartSwap();
        //Sensitivities(); 
    }

    // Calculating net present value of the floating leg of a swap
    public static void TestVanillaSwapFloatingLegNPV()
    {
        // In Single Curve World
        // NPV of floating leg of a swap con be calculated 
        // as 1-DF(T) or
        // as sum of present value of each expected cash flows
        // for details see Paul Wilmott introduces Quantitative Finance par 15.5 or
        // "Swap and other Derivatives" R.Flavell page 57 (sum Fwd(i)*Yf(i)*DF(i) = 1-DF(T))
        // We calculate the NPV of float leg  using both method ands we check that they are the same
        #region Inputs - not real data
        // Start input
        Date refDate = (new Date(DateTime.Now)).mod_foll();  // Random date 

        // Populate markets rates set: from file, from real time, ... here made up numbers
        RateSet mktRates = new RateSet(refDate);

        // Deposits
        mktRates.Add(0.742e-2, "1m", BuildingBlockType.EURDEPO);
        mktRates.Add(0.952e-2, "3m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.201e-2, "6m", BuildingBlockType.EURDEPO);
        // Swap Vs 6M
        mktRates.Add(1.287e-2, "1Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(1.465e-2, "2Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(1.652e-2, "3Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(1.819e-2, "4Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(1.922e-2, "5Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.102e-2, "6Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.425e-2, "7Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.225e-2, "8Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.556e-2, "9Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.794e-2, "10Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.968e-2, "15Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.9895e-2, "20Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.879e-2, "25Y", BuildingBlockType.EURSWAP6M);

        #endregion end Inputs

        #region start SingleCurve building

        SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        // SingleCurveBuilderSmoothingFwd<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderSmoothingFwd<OnLogDf, LinearInterpolator>(mktRates, firstFixing);double firstFixing = 1.201e-2;
        // SingleCurveBuilderInterpBestFit<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderInterpBestFit<OnLogDf, LinearInterpolator>(mktRates);
        #endregion end SingleCurve building

        // Create a schedule of a swap floating leg
        Schedule s = new Schedule(refDate, refDate.add_period("12Y", false), "6m", Rule.Backward, BusinessDayAdjustment.ModifiedFollowing,
            "0d", BusinessDayAdjustment.ModifiedFollowing);
        s.PrintSchedule();  // print it

        // NPV of floating leg as 1-DF(T)
        double FloatingLagPV = 1.0 - C.DF(s.payDates.Last());
        Console.WriteLine("1-DF(T): {0}", FloatingLagPV);

        // NPV of floating as sum of PV of each cash flow
        double CalcFloatingLagPV = 0.00;
        double[] YearFraction = s.GetYFVect(Dc._Act_360);

        for (int j = 0; j < YearFraction.GetLength(0); j++)
        {
            // formula 11.1.1 Option Pricing Formulas Espen Haug second edition
            // "Swap and Other Derivatives" R.Flavell page 53
            double df_ini = C.DF(s.fromDates[j]);
            double df_end = C.DF(s.toDates[j]);
            double fwd = (df_ini / df_end - 1) / YearFraction[j];

            CalcFloatingLagPV += fwd * YearFraction[j] * C.DF(s.payDates[j]);
        }
        Console.WriteLine("CalcFloatingLagPV: {0}", CalcFloatingLagPV);
    }

    // Check if the process will match the starting inputs
    public static void CheckInputsVs6m()
    {
        #region Inputs
        // Start input
        Date refDate = (new Date(DateTime.Now)).mod_foll();  // my ref date, here we get a random value

        // I populate markets rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        // Depos
        mktRates.Add(1.243e-2, "1m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.435e-2, "3m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.720e-2, "6m", BuildingBlockType.EURDEPO);
        // Swap Vs 6M
        mktRates.Add(1.869e-2, "1Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.316e-2, "2Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.544e-2, "3Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.745e-2, "4Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.915e-2, "5Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.057e-2, "6Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.175e-2, "7Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.273e-2, "8Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.362e-2, "9Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.442e-2, "10Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.589e-2, "12Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.750e-2, "15Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.835e-2, "20Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.787e-2, "25Y", BuildingBlockType.EURSWAP6M);

        #endregion end Inputs

        #region Uncomment One of these
        // Case A) Table 15.1
        double firstFixing = 1.720e-2; SingleCurveBuilderSmoothingFwd<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderSmoothingFwd<OnLogDf, LinearInterpolator>(mktRates, firstFixing);

        // Case B) Table 15.1
        // SingleCurveBuilderInterpBestFit<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderInterpBestFit<OnLogDf, LinearInterpolator>(mktRates);

        // Case C) Table 15.1
        // SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        #endregion

        #region print output
        IEnumerable<BuildingBlock> BBArray = mktRates.GetArrayOfBB();

        // Only Given Swap from BBArray        
        IEnumerable<BuildingBlock> OnlyGivenDepo = from c in BBArray
                                                   where c.GetType().BaseType == typeof(OnePaymentStyle)
                                                   select c;

        Console.WriteLine(C.ToString());

        Console.WriteLine("Recalc Df at Ref Date: {0}", C.DF(refDate));

        foreach (OnePaymentStyle BB in OnlyGivenDepo)
        {
            double yf = refDate.YF(BB.endDate, BB.dayCount);
            double df = C.DF(BB.endDate);
            double CalcRate = ((1 / df) - 1) / yf;
            // Console.WriteLine("{0}  Input Rate: {1}  Recalc Rate: {2}", BB.Tenor.GetPeriodStringFormat(), BB.rateValue, CalcRate);
            Console.WriteLine("{0}  Input Rate: {1}  Recalc Rate: {2} Diff: {3}", BB.Tenor.GetPeriodStringFormat(), BB.rateValue, CalcRate, (CalcRate - BB.rateValue).ToString("0.#E+0"));
        }

        // Only Given Swap from BBArray        
        IEnumerable<BuildingBlock> OnlyGivenSwap = from c in BBArray
                                                   where c.GetType().BaseType == typeof(SwapStyle)
                                                   select c;

        foreach (SwapStyle BB in OnlyGivenSwap)
        {
            // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount); // fixed is leg 1

            // dfs array of fixed lag
            Date[] dfDates = BB.scheduleLeg1.payDates; // serial date of fixed lag (each dates we should find df)

            // initialise array for df
            double[] dfFixLeg = new double[dfDates.Length];

            // calculate df
            for (int i = 0; i < dfDates.Length; i++)
            {
                dfFixLeg[i] = C.DF(dfDates[i]);
            }

            // Interpolation Methods for Curve Construction PATRICK S. HAGAN & GRAEME WEST Applied Mathematical Finance,Vol. 13, No. 2, 89–129, June 2006
            // Formula 2) page 4 
            double CalcRate = Formula.ParRate(yfFixLeg, dfFixLeg); // Calculate par rate

            // Console.WriteLine("{0}  Input Rate: {1}  Recalc Rate: {2}", BB.Tenor.GetPeriodStringFormat(), BB.rateValue, CalcRate);
            Console.WriteLine("{0}  Input Rate: {1}  Recalc Rate: {2} Diff: {3}", BB.Tenor.GetPeriodStringFormat(), BB.rateValue, CalcRate, (CalcRate - BB.rateValue).ToString("0.#E+0"));
        }
        #endregion end print output
    }

    // Check if the process will match the starting inputs
    public static void CheckInputsVs3m()
    {
        #region Inputs
        // Start input
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        // I populate markets rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        // Depos
        mktRates.Add(2.243e-2, "1m", BuildingBlockType.EURDEPO);
        mktRates.Add(2.435e-2, "3m", BuildingBlockType.EURDEPO);

        // Swap Vs 3M
        mktRates.Add(2.869e-2, "1Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.316e-2, "2Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.544e-2, "3Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.745e-2, "4Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.915e-2, "5Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.057e-2, "6Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.175e-2, "7Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.273e-2, "8Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.362e-2, "9Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.442e-2, "10Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.589e-2, "12Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.750e-2, "15Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.835e-2, "20Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.787e-2, "25Y", BuildingBlockType.EURSWAP3M);

        #endregion end Inputs

        // Uncomment to chose the curve
        // SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        double firstFixing = 1.435e-2; SingleCurveBuilderSmoothingFwd<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderSmoothingFwd<OnLogDf, LinearInterpolator>(mktRates, firstFixing);
        // SingleCurveBuilderInterpBestFit<OnLogDf, LinearInterpolator> C = new SingleCurveBuilderInterpBestFit<OnLogDf, LinearInterpolator>(mktRates);

        #region print output
        IEnumerable<BuildingBlock> BBArray = mktRates.GetArrayOfBB();

        // Only Given Swap from BBArray        
        IEnumerable<BuildingBlock> OnlyGivenDepo = from c in BBArray
                                                   where c.GetType().BaseType == typeof(OnePaymentStyle)
                                                   select c;

        Console.WriteLine(C.ToString());

        Console.WriteLine("Recalc Df at Ref Date: {0}", C.DF(refDate));

        foreach (OnePaymentStyle BB in OnlyGivenDepo)
        {
            double yf = refDate.YF(BB.endDate, BB.dayCount);
            double df = C.DF(BB.endDate);
            double CalcRate = ((1 / df) - 1) / yf;
            Console.WriteLine("{0}  Input Rate: {1}  Recalc Rate: {2}", BB.Tenor.GetPeriodStringFormat(), BB.rateValue, CalcRate);
        }

        // Only Given Swap from BBArray        
        IEnumerable<BuildingBlock> OnlyGivenSwap = from c in BBArray
                                                   where c.GetType().BaseType == typeof(SwapStyle)
                                                   select c;

        foreach (SwapStyle BB in OnlyGivenSwap)
        {
            // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount); // fixed is leg 1

            // dfs array of fixed lag
            Date[] dfDates = BB.scheduleLeg1.payDates; // serial date of fixed lag (each dates we should find df)

            // initialise array for df
            double[] dfFixLeg = new double[dfDates.Length];

            // calculate df
            for (int i = 0; i < dfDates.Length; i++)
            {
                dfFixLeg[i] = C.DF(dfDates[i]);
            }

            // Interpolation Methods for Curve Construction PATRICK S. HAGAN & GRAEME WEST Applied Mathematical Finance,Vol. 13, No. 2, 89–129, June 2006
            // Formula 2) page 4 
            double CalcRate = Formula.ParRate(yfFixLeg, dfFixLeg); // Calculate par rate

            Console.WriteLine("{0}  Input Rate: {1}  Recalc Rate: {2}", BB.Tenor.GetPeriodStringFormat(), BB.rateValue, CalcRate);
        }
        #endregion end print output
    }

    // Calculate the time for initialise a SingleCurveBuilderInterpBestFit
    public static void TimeForBestFitVs6m()
    {
        #region Inputs
        // Start input
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        // I populate markets rates set: from file, from real time, ... here not real data
        RateSet mktRates = new RateSet(refDate);

        // Depos
        mktRates.Add(2.243e-2, "1m", BuildingBlockType.EURDEPO);
        mktRates.Add(2.435e-2, "3m", BuildingBlockType.EURDEPO);
        mktRates.Add(2.620e-2, "6m", BuildingBlockType.EURDEPO);
        // Swap Vs 6M
        mktRates.Add(2.869e-2, "1Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.316e-2, "2Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.544e-2, "3Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.745e-2, "4Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.915e-2, "5Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.057e-2, "6Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.175e-2, "7Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.273e-2, "8Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.362e-2, "9Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.442e-2, "10Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.589e-2, "12Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.750e-2, "15Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.835e-2, "20Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(4.787e-2, "25Y", BuildingBlockType.EURSWAP6M);
        #endregion end Inputs

        #region building curve
        DateTime timer;

        timer = DateTime.Now;
        double firstFixing = 1.62e-2; SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator> C = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(mktRates, firstFixing);
        Console.WriteLine("\n{0} \nFitted in {1}", C.ToString(), DateTime.Now - timer);

        timer = DateTime.Now;
        SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator> C1 = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(mktRates);
        Console.WriteLine("\n{0} \nFitted in {1}", C1.ToString(), DateTime.Now - timer);

        timer = DateTime.Now;
        SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> C2 = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        Console.WriteLine("\n{0} \nFitted in {1}", C2.ToString(), DateTime.Now - timer);
        #endregion building curve
    }

    // Calculate the time for initialise a SingleCurveBuilderInterpBestFit
    public static void TimeForBestFitVs3m()
    {
        #region Inputs
        // Start input
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        // I populate market rates set: from file, from real time, ... here not real data
        RateSet mktRates = new RateSet(refDate);

        // Depos            
        mktRates.Add(1.434e-2, "3m", BuildingBlockType.EURDEPO);

        // Swap Vs 3M
        mktRates.Add(2.813e-2, "1Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.096e-2, "2Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.322e-2, "3Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.529e-2, "4Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.709e-2, "5Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.862e-2, "6Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.991e-2, "7Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.101e-2, "8Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.197e-2, "9Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.285e-2, "10Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.443e-2, "12Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.614e-2, "15Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.711e-2, "20Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.671e-2, "25Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(4.589e-2, "30Y", BuildingBlockType.EURSWAP3M);
        #endregion end Inputs

        #region building curve

        DateTime timer; // initialise the timer

        timer = DateTime.Now;
        double firstFixing = 1.434e-2; SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator> C = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(mktRates, firstFixing);
        Console.WriteLine("\n{0} \nFitted in {1}", C.ToString(), DateTime.Now - timer);

        timer = DateTime.Now;
        SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator> C1 = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(mktRates);
        Console.WriteLine("\n{0} \nFitted in {1}", C1.ToString(), DateTime.Now - timer);

        timer = DateTime.Now;
        SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> C2 = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        Console.WriteLine("\n{0} \nFitted in {1}", C2.ToString(), DateTime.Now - timer);

        #endregion building curve
    }

    // Print on excel forward rate using different curve builder
    public static void CheckFwdRatesVs6m()
    {
        #region Inputs
        // Start input
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        // Depos
        mktRates.Add(1.243e-2, "1m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.435e-2, "3m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.720e-2, "6m", BuildingBlockType.EURDEPO);
        // Swap Vs 6M
        mktRates.Add(1.869e-2, "1Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.316e-2, "2Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.544e-2, "3Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.745e-2, "4Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.915e-2, "5Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.057e-2, "6Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.175e-2, "7Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.273e-2, "8Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.362e-2, "9Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.442e-2, "10Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.589e-2, "12Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.750e-2, "15Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.835e-2, "20Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.787e-2, "25Y", BuildingBlockType.EURSWAP6M);
        #endregion end Inputs

        #region building curve
        double firstFixing = 1.720e-2;
        SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator> C1 = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(mktRates, firstFixing);
        SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> C2 = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        // More curves can be added
        #endregion end building curve

        // Containers
        List<IRateCurve> CurveList = new List<IRateCurve>();  // list containing curves
        List<string> CurveString = new List<string>();  // list containing labels

        // Populate lists
        CurveList.Add(C1); CurveString.Add(C1.ToString());
        CurveList.Add(C2); CurveString.Add(C2.ToString());

        #region printing output
        // I get the longer swap
        SwapStyle LS = (SwapStyle)mktRates.GetArrayOfBB().Last();

        Dc dc = Dc._Act_360;
        Date[] FromDate = LS.scheduleLeg2.fromDates;
        Date[] ToDate = LS.scheduleLeg2.toDates;
        int N = FromDate.Length;
        List<Vector<double>> Fwds = new List<Vector<double>>();
        double[] dt = new double[N];
        for (int i = 0; i < N; i++)
        {
            dt[i] = FromDate[0].YF(ToDate[i], Dc._30_360);
        }

        foreach (IRateCurve myC in CurveList)
        {
            double[] fwd = new double[N];
            for (int i = 0; i < N; i++)
            {
                double yf = FromDate[i].YF(ToDate[i], dc);
                double df_ini = myC.Df(FromDate[i]);
                double df_end = myC.Df(ToDate[i]);
                fwd[i] = ((df_ini / df_end) - 1) / yf;
            }
            Fwds.Add(new Vector<double>(fwd));
        }

        ExcelMechanisms exl = new ExcelMechanisms();

        exl.printInExcel(new Vector<double>(dt), CurveString, Fwds, "Fwd vs 6M", "time", "rate"); // .printInExcel<T>
        #endregion end printing output
    }

    // Print on excel forward rate using different curve builder for 3m
    public static void CheckFwdRatesVs3m()
    {
        #region Inputs
        // Start input
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        // Depos            
        mktRates.Add(0.434e-2, "3m", BuildingBlockType.EURDEPO);

        // Swap Vs 3M
        mktRates.Add(0.813e-2, "1Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(1.096e-2, "2Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(1.322e-2, "3Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(1.529e-2, "4Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(1.709e-2, "5Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(1.862e-2, "6Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(1.991e-2, "7Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.101e-2, "8Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.197e-2, "9Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.285e-2, "10Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.443e-2, "12Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.614e-2, "15Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.711e-2, "20Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.671e-2, "25Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.589e-2, "30Y", BuildingBlockType.EURSWAP3M);
        #endregion end Inputs

        #region building curve

        SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator> C1 = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(mktRates);
        double firstFixing = 0.434e-2; SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator> C2 = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(mktRates, firstFixing);
        SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> C3 = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        #endregion end building curve

        List<IRateCurve> CurveList = new List<IRateCurve>(); // list containing curve
        List<string> CurveString = new List<string>(); // list containing labels

        // populate lists
        CurveList.Add(C1); CurveString.Add(C1.ToString());
        CurveList.Add(C2); CurveString.Add(C2.ToString());
        CurveList.Add(C3); CurveString.Add(C3.ToString());

        #region printing output
        // I get the longer swap
        SwapStyle LS = (SwapStyle)mktRates.GetArrayOfBB().Last();

        Dc dc = Dc._Act_360;
        Date[] FromDate = LS.scheduleLeg2.fromDates;
        Date[] ToDate = LS.scheduleLeg2.toDates;
        int N = FromDate.Length;
        List<Vector<double>> Fwds = new List<Vector<double>>();
        double[] dt = new double[N];
        for (int i = 0; i < N; i++)
        {
            dt[i] = FromDate[0].YF(ToDate[i], Dc._30_360);
        }

        foreach (IRateCurve myC in CurveList)
        {
            double[] fwd = new double[N];
            for (int i = 0; i < N; i++)
            {
                double yf = FromDate[i].YF(ToDate[i], dc);
                double df_ini = myC.Df(FromDate[i]);
                double df_end = myC.Df(ToDate[i]);
                fwd[i] = ((df_ini / df_end) - 1) / yf;
            }
            Fwds.Add(new Vector<double>(fwd));
        }

        ExcelMechanisms exl = new ExcelMechanisms();

        exl.printInExcel(new Vector<double>(dt), CurveString, Fwds, "Fwd vs 3M", "time", "rate"); // .printInExcel<T>
        #endregion end printing output
    }

    // Print on excel forward rate using different curve builder for OIS fwd 3m
    public static void CheckFwdRatesOIS3m()
    {
        #region Inputs
        // ref date
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        mktRates.Add(2.338e-2, "1d", BuildingBlockType.EURDEPO); // 
        mktRates.Add(2.272e-2, "1w", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.241e-2, "2w", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.16e-2, "3w", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.226e-2, "1m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.299e-2, "2m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.323e-2, "3m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.344e-2, "4m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.371e-2, "5m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.39e-2, "6m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.41e-2, "7m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.4316e-2, "8m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.449e-2, "9m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.466e-2, "10m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.48e-2, "11m", BuildingBlockType.EONIASWAP); // 

        mktRates.Add(2.529e-2, "15m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.565e-2, "18m", BuildingBlockType.EONIASWAP); // 
        mktRates.Add(2.603e-2, "21m", BuildingBlockType.EONIASWAP); // 

        mktRates.Add(2.493e-2, "1Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.644e-2, "2Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.849e-2, "3Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.08e-2, "4Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.292e-2, "5Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.471e-2, "6Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.621e-2, "7Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.748e-2, "8Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.86e-2, "9Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.965e-2, "10Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(4.064e-2, "11Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(4.155e-2, "12Y", BuildingBlockType.EONIASWAP);
        // From here interpolation is need
        mktRates.Add(4.358e-2, "15Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(4.48e-2, "20Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(4.465e-2, "25Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(4.415e-2, "30Y", BuildingBlockType.EONIASWAP);

        List<IRateCurve> CurveList = new List<IRateCurve>(); // list containing curve
        List<string> CurveString = new List<string>(); // list containing labels

        #endregion end Inputs

        #region building curve
        SingleCurveBuilderStandard<OnDf, LinearInterpolator> C1 = new SingleCurveBuilderStandard<OnDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator> C2 = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(mktRates);
        #endregion end building curve

        // populate lists
        CurveList.Add(C1); CurveString.Add(C1.ToString());
        CurveList.Add(C2); CurveString.Add(C2.ToString());

        #region printing output
        // I get the longer eonia swap available from the input data
        SwapStyle LS = (SwapStyle)mktRates.GetArrayOfBB().Last();
        Schedule s = new Schedule(refDate, LS.endDate, "3m", Rule.Backward, LS.swapLeg1.SwapBusDayRollsAdj, "0d", LS.swapLeg1.SwapBusDayPayAdj);

        Dc dc = Dc._Act_360;
        Date[] FromDate = s.fromDates;
        Date[] ToDate = s.toDates;
        int N = FromDate.Length;
        List<Vector<double>> Fwds = new List<Vector<double>>();
        double[] dt = new double[N];
        for (int i = 0; i < N; i++)
        {
            dt[i] = FromDate[0].YF(ToDate[i], Dc._30_360);
        }

        foreach (IRateCurve myC in CurveList)
        {
            double[] fwd = new double[N];
            for (int i = 0; i < N; i++)
            {
                double yf = FromDate[i].YF(ToDate[i], dc);
                double df_ini = myC.Df(FromDate[i]);
                double df_end = myC.Df(ToDate[i]);
                fwd[i] = ((df_ini / df_end) - 1) / yf;
            }
            Fwds.Add(new Vector<double>(fwd));
        }

        ExcelMechanisms exl = new ExcelMechanisms();

        exl.printInExcel(new Vector<double>(dt), CurveString, Fwds, "Fwd 3M", "time", "rate"); // .printInExcel<T>
        #endregion end printing output
    }

    // Check if the process will match the starting inputs
    public static void FwdStartSwap()
    {
        #region Inputs
        // Start input
        Date refDate = new Date(2021, 10, 11);  // ref date, this is only an example

        // I populate market rates set: from file, from real time, ... here not real data
        RateSet mktRates = new RateSet(refDate);

        // Depos
        mktRates.Add(1.243e-2, "1m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.435e-2, "3m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.720e-2, "6m", BuildingBlockType.EURDEPO);
        // Swap Vs 6M
        mktRates.Add(1.869e-2, "1Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.316e-2, "2Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.544e-2, "3Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.745e-2, "4Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.915e-2, "5Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.057e-2, "6Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.175e-2, "7Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.273e-2, "8Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.362e-2, "9Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.442e-2, "10Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.589e-2, "12Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.750e-2, "15Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.835e-2, "20Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.787e-2, "25Y", BuildingBlockType.EURSWAP6M);
        #endregion end Inputs

        // my curve
        IRateCurve Curve1 = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);

        #region print output
        Console.WriteLine("Input Rate (1Y): {0},   ReCalc Rate (1Y): {1} \n\n", 1.869e-2, Curve1.SwapFwd(refDate, "1Y"));

        // Start and Tenor of each point in my fwd matrix (2Y15Y is a 15Y swap starting in 2Y)
        string[] Start = new string[] { "1Y", "2Y", "3Y", "5Y", "7Y", "10Y" };
        string[] Tenor = new string[] { "1Y", "2Y", "5Y", "7Y", "10Y" };

        // print fwd swap matrix using multi curve 
        Console.WriteLine("Matrix using {0}\n", Curve1.ToString());
        for (int i = 0; i < Start.Length; i++)
        {
            for (int t = 0; t < Tenor.Length; t++)
            {
                Console.Write("{0}{1}:{2:P3}  ", Start[i], Tenor[t], Curve1.SwapFwd(refDate.add_period(Start[i]), Tenor[t]));
            }
            Console.Write("\n\n");
        }
        #endregion end print output
    }

    // Calculate sensitivities
    public static void Sensitivities()
    {
        #region Inputs
        // Start input
        Date refDate = new Date(2019, 2, 25);

        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        // Depos
        mktRates.Add(1.243e-2, "1m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.435e-2, "3m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.720e-2, "6m", BuildingBlockType.EURDEPO);
        // Swap Vs 6M
        mktRates.Add(1.869e-2, "1Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.316e-2, "2Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.544e-2, "3Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.745e-2, "4Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.915e-2, "5Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.057e-2, "6Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.175e-2, "7Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.273e-2, "8Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.362e-2, "9Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.442e-2, "10Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.589e-2, "12Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.750e-2, "15Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.835e-2, "20Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.787e-2, "25Y", BuildingBlockType.EURSWAP6M);

        // I shift 1bp up 10y swap input rate
        int IndexShifted = 12;
        RateSet mktRates2 = mktRates.ShiftedRateSet(IndexShifted, 0.0001);

        // print out first and second market input rates
        for (int i = 0; i < mktRates.Count; i++)
        {
            Console.WriteLine("First: {0} {1}  Second: {2} {3}", mktRates.Item(i).M.GetPeriodStringFormat(), mktRates.Item(i).V, mktRates2.Item(i).M.GetPeriodStringFormat(), mktRates2.Item(i).V);
        }
        #endregion end Inputs

        #region building curve

        // First curve: using markets rates
        SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator> c1 = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(mktRates);
        // Second curve: like c1 but 10Y input rate is shifted
        SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator> c2 = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(mktRates2);
        string swapTenor = "10y"; // you can change it   
        #endregion end building curve

        #region myFunction
        Func<SwapStyle, IRateCurve, double> NPV = (BB, c) =>
        {
            #region FixLeg
            // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount); // fixed is leg 1

            // dfs array of fixed lag
            Date[] dfDates = BB.scheduleLeg1.payDates; // serial date of fixed lag (each dates we should find df)

            // # of fixed cash flows
            int n_fix = dfDates.Length;

            double NPV_fix = 0.0;
            // calculate df
            for (int i = 0; i < n_fix; i++)
            {
                NPV_fix += c.Df(dfDates[i]) * yfFixLeg[i] * BB.rateValue;  // df*yf
            }
            // NPV_fix *= BB.rateValue;
            #endregion

            #region FloatLeg
            // fixed leg data
            double[] yfFloatLeg = BB.scheduleLeg2.GetYFVect(BB.swapLeg2.DayCount); // float is leg 2

            // dfs array of fixed lag
            Date[] dfDatesFloat = BB.scheduleLeg2.payDates; // serial date of float lag (each dates we should find df)

            Date[] toDateFloat = BB.scheduleLeg2.toDates;

            // # of fixed cash flows
            int n_float = dfDatesFloat.Length;

            double[] fwd = new double[n_float];

            fwd[0] = ((1 / c.Df(toDateFloat[0])) - 1) / refDate.YF(toDateFloat[0], Dc._Act_360); ;
            for (int i = 1; i < n_float; i++)
            {
                double yf = toDateFloat[i - 1].YF(toDateFloat[i], Dc._Act_360);
                double df_ini = c.Df(toDateFloat[i - 1]);
                double df_end = c.Df(toDateFloat[i]);
                fwd[i] = ((df_ini / df_end) - 1) / yf;
            }

            double NPV_float = 0.0;
            // calculate df
            for (int i = 0; i < n_float; i++)
            {
                NPV_float += c.Df(dfDatesFloat[i]) * yfFloatLeg[i] * fwd[i];  // df*yf
            }

            #endregion
            return NPV_fix - NPV_float;
        };
        #endregion

        #region Print results

        // test forward swap starting in ref date (it should be like simple spot swap)
        double swapRate = c1.SwapFwd(refDate, swapTenor);

        // I create the swap according to standard convention
        SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, swapRate, swapTenor, BuildingBlockType.EURSWAP6M);

        // initial NPV
        double iniMTM = NPV(y, c1) * 100000000;

        // print out
        Console.WriteLine("IRS to be priced tenor: {0}. IRS to be priced rate: {1:f5}", swapTenor, swapRate);
        Console.WriteLine("{0} swap ATM fwd according the starting curve: {1:f5}. Starting P&L {2:f}", swapTenor, swapRate, iniMTM);
        Console.WriteLine("Let's shift {0} rate from {1:f5} to {2:f5}", mktRates.Item(IndexShifted).M.GetPeriodStringFormat(), mktRates.Item(IndexShifted).V, mktRates2.Item(IndexShifted).V);

        // NPV after shift
        double endMTM = NPV(y, c2) * 100000000;
        Console.WriteLine("{0} swap ATM fwd after shifting: {1:f5}. P&L after shifting {2:f}", swapTenor, c2.SwapFwd(refDate, swapTenor), endMTM);
        Console.WriteLine("Press a key to continue"); Console.ReadLine();
        #endregion
    }

    // More on sensitivities calc
    public static void MoreOnSensitivities()
    {
        #region Inputs
        // Start input
        Date refDate = new Date(2010, 10, 11);

        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        // Depos
        mktRates.Add(1.243e-2, "1m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.435e-2, "3m", BuildingBlockType.EURDEPO);
        mktRates.Add(1.720e-2, "6m", BuildingBlockType.EURDEPO);
        // Swap Vs 6M
        mktRates.Add(1.869e-2, "1Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.316e-2, "2Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.544e-2, "3Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.745e-2, "4Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(2.915e-2, "5Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.057e-2, "6Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.175e-2, "7Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.273e-2, "8Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.362e-2, "9Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.442e-2, "10Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.589e-2, "12Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.750e-2, "15Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.835e-2, "20Y", BuildingBlockType.EURSWAP6M);
        mktRates.Add(3.787e-2, "25Y", BuildingBlockType.EURSWAP6M);
        #endregion end Inputs

        #region building curve
        List<ISingleRateCurve> Curves = new List<ISingleRateCurve>();

        // initialised each class and add to list. You can add more curves

        // Setup (a) in Table 15.3
        SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> c2 = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear); Curves.Add(c2);

        // Setup (b) in Table 15.3
        SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator> c1 = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(mktRates); Curves.Add(c1);

        string swapTenor = "11y"; // you can change it     
        #endregion end building curve

        #region myFunction
        Func<SwapStyle, IRateCurve, double> NPV = (BB, c) =>
        {
            #region FixLeg
            // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount); // fixed is leg 1

            // dfs array of fixed lag
            Date[] dfDates = BB.scheduleLeg1.payDates; // serial date of fixed lag (each dates we should find df)

            // # of fixed cash flows
            int n_fix = dfDates.Length;

            double NPV_fix = 0.0;
            // calculate df
            for (int i = 0; i < n_fix; i++)
            {
                NPV_fix += c.Df(dfDates[i]) * yfFixLeg[i] * BB.rateValue;  // df*yf
            }
            // NPV_fix *= BB.rateValue;
            #endregion

            #region FloatLeg
            // fixed leg data
            double[] yfFloatLeg = BB.scheduleLeg2.GetYFVect(BB.swapLeg2.DayCount); // float is leg 2

            // dfs array of fixed lag
            Date[] dfDatesFloat = BB.scheduleLeg2.payDates; // serial date of float lag (each dates we should find df)

            Date[] toDateFloat = BB.scheduleLeg2.toDates;

            // # of fixed cash flows
            int n_float = dfDatesFloat.Length;

            double[] fwd = new double[n_float];

            fwd[0] = ((1 / c.Df(toDateFloat[0])) - 1) / refDate.YF(toDateFloat[0], Dc._Act_360); ;
            for (int i = 1; i < n_float; i++)
            {
                double yf = toDateFloat[i - 1].YF(toDateFloat[i], Dc._Act_360);
                double df_ini = c.Df(toDateFloat[i - 1]);
                double df_end = c.Df(toDateFloat[i]);
                fwd[i] = ((df_ini / df_end) - 1) / yf;
            }

            double NPV_float = 0.0;
            // calculate df
            for (int i = 0; i < n_float; i++)
            {
                NPV_float += c.Df(dfDatesFloat[i]) * yfFloatLeg[i] * fwd[i];  // df*yf
            }

            #endregion
            return NPV_fix - NPV_float;
        };
        #endregion

        #region Print results
        foreach (ISingleRateCurve C in Curves)
        {
            double swapRate = C.SwapFwd(refDate, swapTenor);

            IRateCurve[] cs = C.ShiftedCurveArray(0.0001); IRateCurve csp = C.ParallelShift(0.0001);

            // initialise some variable used in sensitivities
            double sens = 0.0;
            double runSum = 0.0;

            // standard
            Console.WriteLine(C.ToString());
            SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, swapRate, swapTenor, BuildingBlockType.EURSWAP6M);
            double iniMTM = NPV(y, C) * 100000000;
            Console.WriteLine("{0} swap ATM fwd: {1:f5}", swapTenor, swapRate);
            Console.WriteLine("Starting P&L {0:f}", iniMTM);
            int nOfRate = mktRates.Count;
            for (int i = 0; i < nOfRate; i++)
            {
                sens = NPV(y, cs[i]) * 100000000 - iniMTM;
                Console.WriteLine("{0} BPV: {1:f}", mktRates.Item(i).M.GetPeriodStringFormat(), sens);
                runSum += sens;
            }
            Console.WriteLine("Total: {0:f}", runSum);
            Console.WriteLine("Parallel Total: {0:f}", NPV(y, csp) * 100000000 - iniMTM);

            Console.WriteLine("Press a key to continue"); Console.ReadLine();
        }
        #endregion
    }
}