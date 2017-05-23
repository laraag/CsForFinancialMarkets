// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// MultiThreadOnSwap.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

static class MultiThreadOnSwap
{
    public static void Main() 
    {
        //SensitivitiesParallel();
        //SensitivitiesSequentialVsParallel();
        //VanillaSwaps();
    }   

    // sensitivities/DVO1. ATM swap has no sensitivities with respect to the discount curve 
    public static void SensitivitiesParallel()
    {
        #region Inputs
        // Start input            
        // Start input, reference date.
        Date refDate = (new Date(DateTime.Now)).mod_foll();
        #region Eonia market data
        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        mktRates.Add(0.447e-2, "1w", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.583e-2, "2w", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.627e-2, "3w", BuildingBlockType.EONIASWAP);

        mktRates.Add(0.635e-2, "1m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.675e-2, "2m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.705e-2, "3m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.734e-2, "4m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.758e-2, "5m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.780e-2, "6m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.798e-2, "7m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.816e-2, "8m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.834e-2, "9m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.849e-2, "10m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.864e-2, "11m", BuildingBlockType.EONIASWAP);

        mktRates.Add(0.878e-2, "1Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.098e-2, "2Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.36e-2, "3Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.639e-2, "4Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.9e-2, "5Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.122e-2, "6Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.308e-2, "7Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.467e-2, "8Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.599e-2, "9Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.715e-2, "10Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.818e-2, "11Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.908e-2, "12Y", BuildingBlockType.EONIASWAP);
        // From here interpolation is need
        mktRates.Add(3.093e-2, "15Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.173e-2, "20Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.114e-2, "25Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.001e-2, "30Y", BuildingBlockType.EONIASWAP);
        #endregion

        #region Swap Market Data
        // RateSet EUR 6m swap
        RateSet rs = new RateSet(refDate);
        rs.Add(1.16e-2, "6m", BuildingBlockType.EURDEPO);
        rs.Add(1.42e-2, "1y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.635e-2, "2y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.872e-2, "3y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.131e-2, "4y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.372e-2, "5y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.574e-2, "6y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.743e-2, "7y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.886e-2, "8y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.004e-2, "9y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.107e-2, "10y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.198e-2, "11y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.278e-2, "12y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.344e-2, "13y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.398e-2, "14y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.438e-2, "15y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.467e-2, "16y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.484e-2, "17y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.494e-2, "18y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.495e-2, "19y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.491e-2, "20y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.483e-2, "21y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.471e-2, "22y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.455e-2, "23y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.436e-2, "24y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.415e-2, "25y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.391e-2, "26y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.366e-2, "27y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.340e-2, "28y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.314e-2, "29y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.29e-2, "30y", BuildingBlockType.EURSWAP6M);
        #endregion

        #endregion end Inputs

        #region building curve

        string swapTenor = "11y"; // you can change it       

        // I build my multi curve, 
        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> DCurve = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(mktRates, OneDimensionInterpolation.LogLinear); // discount curve
        MultiCurveBuilder<SimpleCubicInterpolator> C = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);  // multi curve
        #endregion end building curve

        #region myFunction
        // my function to calculate Net Present Value of a Vanilla Swap (receiver swap)
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

            Date[] FromDateFloat = BB.scheduleLeg2.fromDates;

            // # of fixed cash flows
            int n_float = dfDatesFloat.Length;

            double[] fwd = new double[n_float]; // fwd rate container

            // getting fwd rates
            for (int i = 0; i < n_float; i++)
            {
                fwd[i] = c.Fwd(FromDateFloat[i]);
            }

            double NPV_float = 0.0;
            // calculate df
            for (int i = 0; i < n_float; i++)
            {
                NPV_float += c.Df(dfDatesFloat[i]) * yfFloatLeg[i] * fwd[i];  // df*yf
            }

            #endregion
            return NPV_fix - NPV_float;  // NPV
        };
        #endregion

        #region Print results

        double atmSwap = C.SwapFwd(refDate, swapTenor);  // At The Money swap (i.e. par rate)

        List<double> swapRateList = new List<double>(); // lists of  swap to analyze
        swapRateList.Add(atmSwap); // it is ATM
        swapRateList.Add(atmSwap + 0.01); // it has positive mark to market (MtM). It is a receiver swap with a contract rate > than Atm)
        swapRateList.Add(atmSwap - 0.01); // it has negative MtM

        // iterate for each swap:
        // see how change the sign of sensitivities for discount curve and for forwarding curve changing contract rates
        Console.WriteLine("Executing parallel loop...");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Parallel.ForEach(swapRateList, swapRate =>
        {
            Console.WriteLine("Pricing Receiver Swap {0}, Atm Rate: {1:f6}, Contract Rate: {2:f6}", swapTenor, atmSwap, swapRate);
            IRateCurve[] cs = C.ShiftedCurvesArrayFwdCurve(0.0001);
            IRateCurve csp = C.ParallelShiftFwdCurve(0.0001);

            // initialise some variable used in sensitivities
            double sens = 0.0;
            double runSum = 0.0;

            // Standard swap
            SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, swapRate, swapTenor, BuildingBlockType.EURSWAP6M);
            double iniMTM = NPV(y, C) * 100000000;  // initial mark to market for 100ml receiver contract

            Console.WriteLine("Starting Mark To Market {0:f}", iniMTM);
            Console.WriteLine("Sensitivities to Curve used for forward rate: ");
            int nOfRate = rs.Count;  // iterate for market input for forwarding curve
            for (int i = 0; i < nOfRate; i++)
            {
                sens = NPV(y, cs[i]) * 100000000 - iniMTM;
                Console.WriteLine("{0} BPV: {1:f}", rs.Item(i).M.GetPeriodStringFormat(), sens);
                runSum += sens;
            }
            Console.WriteLine("Total: {0:f}", runSum);
            Console.WriteLine("\nParallel Shift Total: {0:f}", NPV(y, csp) * 100000000 - iniMTM); // parallel shift

            // reset some variable used in sensitivities
            sens = 0.0;
            runSum = 0.0;

            Console.WriteLine("Sensitivities to Discount Curve:");
            // let's consider discounting curve
            IRateCurve[] DCrvs = C.ShiftedCurvesArrayDCurve(0.0001); // shifting each bucket
            IMultiRateCurve DCrvp = C.ParallelShiftDCurve(0.0001); // parallel shift
            nOfRate = mktRates.Count; // iterate for market input for discounting curve
            for (int i = 0; i < nOfRate; i++)
            {
                sens = NPV(y, DCrvs[i]) * 100000000 - iniMTM;
                Console.WriteLine("{0} BPV: {1:f}", mktRates.Item(i).M.GetPeriodStringFormat(), sens);
                runSum += sens;
            }
            Console.WriteLine("Total: {0:f}", runSum);
            Console.WriteLine("\nParallel Shift Total: {0:f}", NPV(y, DCrvp) * 100000000 - iniMTM);
        });

        stopwatch.Stop();
        Console.WriteLine("Parallel loop time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);
        #endregion
    }

    // sensitivities/DVO1. ATM swap has no sensitivities with respect to the discount curve 
    public static void SensitivitiesSequentialVsParallel()
    {
        #region Inputs
        // Start input, reference date.
        Date refDate = (new Date(DateTime.Now)).mod_foll();
        #region Eonia market data
        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        mktRates.Add(0.447e-2, "1w", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.583e-2, "2w", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.627e-2, "3w", BuildingBlockType.EONIASWAP);

        mktRates.Add(0.635e-2, "1m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.675e-2, "2m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.705e-2, "3m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.734e-2, "4m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.758e-2, "5m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.780e-2, "6m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.798e-2, "7m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.816e-2, "8m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.834e-2, "9m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.849e-2, "10m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.864e-2, "11m", BuildingBlockType.EONIASWAP);

        mktRates.Add(0.878e-2, "1Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.098e-2, "2Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.36e-2, "3Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.639e-2, "4Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.9e-2, "5Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.122e-2, "6Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.308e-2, "7Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.467e-2, "8Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.599e-2, "9Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.715e-2, "10Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.818e-2, "11Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.908e-2, "12Y", BuildingBlockType.EONIASWAP);
        // From here interpolation is need
        mktRates.Add(3.093e-2, "15Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.173e-2, "20Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.114e-2, "25Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.001e-2, "30Y", BuildingBlockType.EONIASWAP);
        #endregion

        #region Swap Market Data
        // RateSet EUR 6m swap
        RateSet rs = new RateSet(refDate);
        rs.Add(1.16e-2, "6m", BuildingBlockType.EURDEPO);
        rs.Add(1.42e-2, "1y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.635e-2, "2y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.872e-2, "3y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.131e-2, "4y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.372e-2, "5y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.574e-2, "6y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.743e-2, "7y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.886e-2, "8y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.004e-2, "9y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.107e-2, "10y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.198e-2, "11y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.278e-2, "12y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.344e-2, "13y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.398e-2, "14y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.438e-2, "15y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.467e-2, "16y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.484e-2, "17y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.494e-2, "18y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.495e-2, "19y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.491e-2, "20y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.483e-2, "21y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.471e-2, "22y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.455e-2, "23y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.436e-2, "24y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.415e-2, "25y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.391e-2, "26y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.366e-2, "27y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.340e-2, "28y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.314e-2, "29y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.29e-2, "30y", BuildingBlockType.EURSWAP6M);
        #endregion

        #endregion end Inputs

        #region building curve

        string swapTenor = "11y"; // you can change it       

        // I build my multi curve, 
        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> DCurve = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(mktRates, OneDimensionInterpolation.LogLinear); // discount curve
        MultiCurveBuilder<SimpleCubicInterpolator> C = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);  // multi curve
        #endregion end building curve

        #region myFunction
        // my function to calculate Net Present Value of a Vanilla Swap (receiver swap)
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

            Date[] FromDateFloat = BB.scheduleLeg2.fromDates;

            // # of fixed cash flows
            int n_float = dfDatesFloat.Length;

            double[] fwd = new double[n_float]; // fwd rate container

            // getting fwd rates
            for (int i = 0; i < n_float; i++)
            {
                fwd[i] = c.Fwd(FromDateFloat[i]);
            }

            double NPV_float = 0.0;
            // calculate df
            for (int i = 0; i < n_float; i++)
            {
                NPV_float += c.Df(dfDatesFloat[i]) * yfFloatLeg[i] * fwd[i];  // df*yf
            }

            #endregion
            return NPV_fix - NPV_float;  // NPV
        };
        #endregion

        #region Print results

        double atmSwap = C.SwapFwd(refDate, swapTenor);  // At The Money swap (i.e. par rate)

        List<double> swapRateList = new List<double>(); // lists of  swap to analyze
        swapRateList.Add(atmSwap); // it is ATM
        swapRateList.Add(atmSwap + 0.01); // it has positive MtM (mark to markets). It is a receiver swap with a contract rate > than Atm)
        swapRateList.Add(atmSwap - 0.01); // it has negative MtM

        List<string[]> allInfo = new List<string[]>();

        // iterate for each swap:
        // see how change the sign of sensitivities for discount curve and for forwarding curve changing contract rates
        #region sequencial
        Console.WriteLine("Executing sequential loop...");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        foreach (double swapRate in swapRateList)
        {
            List<string> Info = new List<string>();

            Info.Add("Sequential. Contract Rate " + swapRate);

            IRateCurve[] cs = C.ShiftedCurvesArrayFwdCurveSeq(0.0001);
            IRateCurve csp = C.ParallelShiftFwdCurve(0.0001);
            // initialise some variable used in sensitivities
            double sens = 0.0;
            double runSum = 0.0;

            // Standard swap
            SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, swapRate, swapTenor, BuildingBlockType.EURSWAP6M);
            double iniMTM = NPV(y, C) * 100000000;  // initial mark to market for 100ml receiver contract

            Info.Add(String.Format("Starting Mark To Market {0:f}", iniMTM)); // info to visualise
            Info.Add("Sensitivities to Curve used for forward rate: "); // info to visualise

            int nOfRate = rs.Count;  // iterate for market input for forwarding curve
            string bpvFwd = "";
            for (int i = 0; i < nOfRate; i++)
            {
                sens = NPV(y, cs[i]) * 100000000 - iniMTM;
                bpvFwd += (String.Format("[{0} BPV: {1:f}]", rs.Item(i).M.GetPeriodStringFormat(), sens)); // info to visualise
                runSum += sens;
            }
            Info.Add(bpvFwd);
            Info.Add(String.Format("Total: {0:f}", runSum)); // info
            Info.Add(String.Format("Parallel Shift Total: {0:f}", NPV(y, csp) * 100000000 - iniMTM)); // parallel shift

            // reset some variable used in sensitivities
            sens = 0.0;
            runSum = 0.0;

            Info.Add(String.Format("Sensitivities to Discount Curve:")); // info
            // let's consider discounting curve
            IRateCurve[] DCrvs = C.ShiftedCurvesArrayDCurveSeq(0.0001); // shifting each bucket
            IMultiRateCurve DCrvp = C.ParallelShiftDCurve(0.0001); // parallel shift
            nOfRate = mktRates.Count; // iterate for market input for discounting curve
            string bpvDf = "";
            for (int i = 0; i < nOfRate; i++)
            {
                sens = NPV(y, DCrvs[i]) * 100000000 - iniMTM;
                bpvDf += (String.Format("[{0} BPV: {1:f}]", mktRates.Item(i).M.GetPeriodStringFormat(), sens));
                runSum += sens;
            }
            Info.Add(bpvDf);
            Info.Add(String.Format("Total: {0:f}", runSum)); // info
            Info.Add(String.Format("Parallel Shift Total: {0:f}\n\n", NPV(y, DCrvp) * 100000000 - iniMTM));
            allInfo.Add(Info.ToArray<string>());
        }
        stopwatch.Stop();
        Console.WriteLine("Sequential loop time in milliseconds: {0}", stopwatch.ElapsedMilliseconds
            );

        # endregion
        stopwatch.Reset();

        #region parallel
        Console.WriteLine("Executing parallel loop...");
        stopwatch.Start();
        Parallel.ForEach(swapRateList, swapRate =>     // multi thread version   
        {
            List<string> Info = new List<string>();

            Info.Add("Parallel. Contract Rate " + swapRate);

            IRateCurve[] cs = C.ShiftedCurvesArrayFwdCurve(0.0001);
            IRateCurve csp = C.ParallelShiftFwdCurve(0.0001);
            // initialise some variable used in sensitivities
            double sens = 0.0;
            double runSum = 0.0;

            // Standard swap
            SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, swapRate, swapTenor, BuildingBlockType.EURSWAP6M);
            double iniMTM = NPV(y, C) * 100000000;  // initial mark to market for 100ml receiver contract

            Info.Add(String.Format("Starting Mark To Market {0:f}", iniMTM)); // info to visualise
            Info.Add("Sensitivities to Curve used for forward rate: "); // info to visualise

            int nOfRate = rs.Count;  // iterate for market input for forwarding curve
            string bpvFwd = "";
            for (int i = 0; i < nOfRate; i++)
            {
                sens = NPV(y, cs[i]) * 100000000 - iniMTM;
                bpvFwd += (String.Format("[{0} BPV: {1:f}]", rs.Item(i).M.GetPeriodStringFormat(), sens)); // info to visualise
                runSum += sens;
            }
            Info.Add(bpvFwd);
            Info.Add(String.Format("Total: {0:f}", runSum)); // info
            Info.Add(String.Format("Parallel Shift Total: {0:f}", NPV(y, csp) * 100000000 - iniMTM)); // parallel shift

            // reset some variable used in sensitivities
            sens = 0.0;
            runSum = 0.0;

            Info.Add(String.Format("Sensitivities to Discount Curve:")); // info
            // let's consider discounting curve
            IRateCurve[] DCrvs = C.ShiftedCurvesArrayDCurve(0.0001); // shifting each bucket
            IMultiRateCurve DCrvp = C.ParallelShiftDCurve(0.0001); // parallel shift
            nOfRate = mktRates.Count; // iterate for market input for discounting curve
            string bpvDf = "";
            for (int i = 0; i < nOfRate; i++)
            {
                sens = NPV(y, DCrvs[i]) * 100000000 - iniMTM;
                bpvDf += (String.Format("[{0} BPV: {1:f}]", mktRates.Item(i).M.GetPeriodStringFormat(), sens));
                runSum += sens;
            }
            Info.Add(bpvDf);
            Info.Add(String.Format("Total: {0:f}", runSum)); // info
            Info.Add(String.Format("Parallel Shift Total: {0:f}\n\n", NPV(y, DCrvp) * 100000000 - iniMTM));
            allInfo.Add(Info.ToArray<string>());
        });
        stopwatch.Stop();
        Console.WriteLine("Parallel loop time in milliseconds: {0}", stopwatch.ElapsedMilliseconds
            );
        #endregion

        Console.WriteLine("Computation complete. Print results? y/n");
        char con = Console.ReadKey().KeyChar;
        if (con == 'y' || con == 'Y')
        {
            // Printing out info
            Console.WriteLine();
            foreach (string[] info in allInfo)
            {
                foreach (string s in info)
                { Console.WriteLine(s); }
            }
        }
        #endregion
    }

    // just testing
    public static void VanillaSwaps()
    {
        // Start input, reference date.
        Date refDate = (new Date(DateTime.Now)).mod_foll();
        #region Eonia market data
        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        mktRates.Add(0.447e-2, "1w", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.583e-2, "2w", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.627e-2, "3w", BuildingBlockType.EONIASWAP);

        mktRates.Add(0.635e-2, "1m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.675e-2, "2m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.705e-2, "3m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.734e-2, "4m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.758e-2, "5m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.780e-2, "6m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.798e-2, "7m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.816e-2, "8m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.834e-2, "9m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.849e-2, "10m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.864e-2, "11m", BuildingBlockType.EONIASWAP);

        mktRates.Add(0.878e-2, "1Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.098e-2, "2Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.36e-2, "3Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.639e-2, "4Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.9e-2, "5Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.122e-2, "6Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.308e-2, "7Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.467e-2, "8Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.599e-2, "9Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.715e-2, "10Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.818e-2, "11Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.908e-2, "12Y", BuildingBlockType.EONIASWAP);
        // From here interpolation is need
        mktRates.Add(3.093e-2, "15Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.173e-2, "20Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.114e-2, "25Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.001e-2, "30Y", BuildingBlockType.EONIASWAP);
        #endregion

        #region Swap Market Data
        // RateSet EUR 6m swap
        RateSet rs = new RateSet(refDate);
        rs.Add(1.16e-2, "6m", BuildingBlockType.EURDEPO);
        rs.Add(1.42e-2, "1y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.635e-2, "2y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.872e-2, "3y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.131e-2, "4y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.372e-2, "5y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.574e-2, "6y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.743e-2, "7y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.886e-2, "8y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.004e-2, "9y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.107e-2, "10y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.198e-2, "11y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.278e-2, "12y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.344e-2, "13y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.398e-2, "14y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.438e-2, "15y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.467e-2, "16y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.484e-2, "17y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.494e-2, "18y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.495e-2, "19y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.491e-2, "20y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.483e-2, "21y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.471e-2, "22y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.455e-2, "23y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.436e-2, "24y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.415e-2, "25y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.391e-2, "26y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.366e-2, "27y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.340e-2, "28y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.314e-2, "29y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.29e-2, "30y", BuildingBlockType.EURSWAP6M);
        #endregion
        // end input

        #region Inizialize my curves
        // Building multi curve
        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> DCurve = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(mktRates, OneDimensionInterpolation.LogLinear); // discount curve
        MultiCurveBuilder<SimpleCubicInterpolator> MC = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);  // multi curve

        #endregion
        string SwapTenor = "11y";
        double SwapRate = 0.03;
        bool Payer = true;
        double Nominal = 100000000;
        VanillaSwap V = new VanillaSwap(MC, SwapRate, SwapTenor, Payer, Nominal);
        Console.WriteLine(V.BPVParallelShiftDCurve(0.0001));
        Console.WriteLine(V.BPVParallelShiftFwdCurve(0.0001));
        Stopwatch stopwatch = new Stopwatch();
        Console.WriteLine("Implementing shift sequential...");
        stopwatch.Start();
        double[] bpv = V.BPVShiftedDCurve(0.0001);
        stopwatch.Stop();
        Console.WriteLine("Done. Time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);
        stopwatch.Reset();
        Console.WriteLine("Implementing shift sequential...");
        stopwatch.Start();
        double[] bpv2 = V.BPVShiftedDCurveSeq(0.0001);
        stopwatch.Stop();
        Console.WriteLine("Done. Time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);

        Console.WriteLine(V.NPV());
        foreach (double d in bpv)
        {
            Console.WriteLine(d);
        }
    }   
}