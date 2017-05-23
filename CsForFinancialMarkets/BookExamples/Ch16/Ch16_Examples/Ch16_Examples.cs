// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Ch16_Examples.cs
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

class TestMultiCurveBuilder
{
    public static void Main() 
    {
        //CheckInputs();
        //FwdInExcell();
        //Sensitivities();
        //FwdMatrix();
        //MTM_Differences();
        //PerformanceMultiCurveProcess();
        //CheckInterpOnFwd();
        //DiscountFactors();    
    }

    public static void CheckInputs()
    {
        // ref date, you should use the one you need
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        #region eonia market data
        // I populate RateSet: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        mktRates.Add(1.447e-2, "1w", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.583e-2, "2w", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.627e-2, "3w", BuildingBlockType.EONIASWAP);

        mktRates.Add(1.635e-2, "1m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.675e-2, "2m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.705e-2, "3m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.734e-2, "4m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.758e-2, "5m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.780e-2, "6m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.798e-2, "7m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.816e-2, "8m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.834e-2, "9m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.849e-2, "10m", BuildingBlockType.EONIASWAP);
        mktRates.Add(1.864e-2, "11m", BuildingBlockType.EONIASWAP);

        mktRates.Add(1.878e-2, "1Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.098e-2, "2Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.36e-2, "3Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.639e-2, "4Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(2.9e-2, "5Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.122e-2, "6Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.308e-2, "7Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.467e-2, "8Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.599e-2, "9Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.715e-2, "10Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.818e-2, "11Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(3.908e-2, "12Y", BuildingBlockType.EONIASWAP);
        // From here interpolation is need
        mktRates.Add(4.093e-2, "15Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(4.173e-2, "20Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(4.114e-2, "25Y", BuildingBlockType.EONIASWAP);
        mktRates.Add(4.001e-2, "30Y", BuildingBlockType.EONIASWAP);
        #endregion

        #region Swap Market Data
        // RateSet EUR 6m swap
        RateSet rs = new RateSet(refDate);
        rs.Add(1.06e-2, "6m", BuildingBlockType.EURDEPO);
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

        SingleCurveBuilderStandard<OnDf, LinearInterpolator> DCurve = new SingleCurveBuilderStandard<OnDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        MultiCurveBuilder<SimpleCubicInterpolator> MC = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);

        // Let's check if recalculated rate are the same of inputs rates
        foreach (RateSet BB in rs)
        {
            if (BB.T != BuildingBlockType.EURDEPO)
            {
                string str = BB.M.GetPeriodStringFormat();
                double CalcRate = MC.SwapFwd(refDate, str);
                Console.WriteLine("{0}  Input Rate: {1}  Recalc Rate: {2}", str, BB.V, CalcRate);
            }
            else
            {
                string str = BB.M.GetPeriodStringFormat();
                double CalcRate = MC.Fwd(refDate);
                Console.WriteLine("{0}  Input Rate: {1}  Recalc Rate: {2}", str, BB.V, CalcRate);
            }
        }
    }

    public static void FwdInExcell()
    {
        // Start input            
        // reference date, here a random date
        Date refDate = (new Date(DateTime.Now)).mod_foll();
        #region eonia market data
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
        rs.Add(1.26e-2, "6m", BuildingBlockType.EURDEPO);
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

        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> DCurve = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        MultiCurveBuilder<SimpleCubicInterpolator> MC = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);

        // I create the longer swap facsimile, to have a usable schedule
        SwapStyle S = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, 0.0, "30y", BuildingBlockType.EURSWAP6M);

        // Forward rate
        double[] fwds = (from c in S.scheduleLeg2.fromDates
                         select MC.Fwd(c)).ToArray();
        // discount factors
        double[] df = (from c in S.scheduleLeg2.toDates
                       select MC.Df(c)).ToArray();
        // Times
        double[] dt = (from c in S.scheduleLeg2.fromDates
                       select refDate.YF_30_360E(c)).ToArray();

        ExcelMechanisms exl = new ExcelMechanisms();
        // print value to excel
        exl.printOneExcel<double>(new Vector<double>(dt), new Vector<double>(fwds), "Multicurve forward rate", "Time in year", "Rate", "FWD 6m");
        exl.printOneExcel<double>(new Vector<double>(dt), new Vector<double>(df), "Multicurve discount factor", "Time in year", "Rate", "DF");
    }

    // sensitivities/DVO1. ATM swap has no sensitivities with respect to the discount curve 
    public static void Sensitivities()
    {
        #region Inputs
        // ref date
        Date refDate = (new Date(DateTime.Now)).mod_foll();  // Results depend also on the choice of refDate. To get the same results of Table 16.1 please use 26 Nov 1982 as refdate.

        #region Eonia market data
        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        mktRates.Add(0.647e-2, "1w", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.783e-2, "2w", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.697e-2, "3w", BuildingBlockType.EONIASWAP);

        mktRates.Add(0.645e-2, "1m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.679e-2, "2m", BuildingBlockType.EONIASWAP);
        mktRates.Add(0.715e-2, "3m", BuildingBlockType.EONIASWAP);
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
        rs.Add(1.06e-2, "6m", BuildingBlockType.EURDEPO);
        rs.Add(1.22e-2, "1y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.435e-2, "2y", BuildingBlockType.EURSWAP6M);
        rs.Add(1.672e-2, "3y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.131e-2, "4y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.172e-2, "5y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.474e-2, "6y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.643e-2, "7y", BuildingBlockType.EURSWAP6M);
        rs.Add(2.856e-2, "8y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.024e-2, "9y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.107e-2, "10y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.198e-2, "11y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.298e-2, "12y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.346e-2, "13y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.391e-2, "14y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.418e-2, "15y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.567e-2, "16y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.584e-2, "17y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.594e-2, "18y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.595e-2, "19y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.591e-2, "20y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.583e-2, "21y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.571e-2, "22y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.555e-2, "23y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.536e-2, "24y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.515e-2, "25y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.491e-2, "26y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.466e-2, "27y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.340e-2, "28y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.314e-2, "29y", BuildingBlockType.EURSWAP6M);
        rs.Add(3.219e-2, "30y", BuildingBlockType.EURSWAP6M);
        #endregion

        #endregion end Inputs

        #region building curve

        string swapTenor = "11y"; // you can change it       

        // I build my multi curve, 
        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> DCurve = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(mktRates, OneDimensionInterpolation.LogLinear); // discount curve  //era Log Lin
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
        swapRateList.Add(atmSwap + 0.01); // it has positive MtM (mark to market). It is a receiver swap with a contract rate > than Atm)
        swapRateList.Add(atmSwap - 0.01); // it has negative MtM

        // iterate for each swap:
        // see how change the sign of sensitivities for discount curve and for forwarding curve changing contract rates
        foreach (double swapRate in swapRateList)
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
            Console.WriteLine("Press a key to continue"); Console.ReadLine();

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

            Console.WriteLine("Press a key to continue"); Console.ReadLine();
        }
        #endregion
    }

    public static void FwdMatrix()
    {
        // Reference date
        // Results depend also on the choice of refDate. In our example we used 26 Nov 1982 as refDate.
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
        rs.Add(1.56e-2, "6m", BuildingBlockType.EURDEPO);
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

        // Building multi curve
        SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> DCurve = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        MultiCurveBuilder<SimpleCubicInterpolator> MC = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);

        // Start and Tenor of each point in my fwd matrix (2Y15Y is a 15Y swap starting in 2Y)
        string[] Start = new string[] { "1Y", "2Y", "3Y", "5Y", "10Y", "15Y" };
        string[] Tenor = new string[] { "1Y", "2Y", "5Y", "10Y", "15Y" };

        // print fwd swap matrix using multi curve 
        Console.WriteLine("Matrix using {0}\n", MC.ToString());
        for (int i = 0; i < Start.Length; i++)
        {
            for (int t = 0; t < Tenor.Length; t++)
            {
                Console.Write("{0}{1}:{2:P3}  ", Start[i], Tenor[t], MC.SwapFwd(refDate.add_period(Start[i]), Tenor[t]));
            }
            Console.Write("\n\n");
        }

        // I build a single curve
        SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator> SC = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(rs);
        Console.WriteLine("Press a key to continue"); Console.ReadLine();

        // print fwd swap matrix using single curve
        Console.WriteLine("Matrix using {0}\n", SC.ToString());
        for (int i = 0; i < Start.Length; i++)
        {
            for (int t = 0; t < Tenor.Length; t++)
            {
                Console.Write("{0}{1}:{2:P3}  ", Start[i], Tenor[t], SC.SwapFwd(refDate.add_period(Start[i]), Tenor[t]));
            }
            Console.Write("\n\n");
        }
        Console.WriteLine("Press a key to continue"); Console.ReadLine();

        // print difference
        Console.WriteLine("Matrix of differences\n");
        for (int i = 0; i < Start.Length; i++)
        {
            for (int t = 0; t < Tenor.Length; t++)
            {
                Console.Write("{0}{1}:{2:P3}  ", Start[i], Tenor[t], MC.SwapFwd(refDate.add_period(Start[i]), Tenor[t]) - SC.SwapFwd(refDate.add_period(Start[i]), Tenor[t]));
            }
            Console.Write("\n\n");
        }

        Console.WriteLine("Press a key to continue"); Console.ReadLine();
    }

    public static void MTM_Differences()
    {
        // Reference date
        // Results depend also on the choice of refDate. In our example we used 26 Nov 1982 as refDate.
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
        rs.Add(1.26e-2, "6m", BuildingBlockType.EURDEPO);
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

        // Building multi curve
        SingleCurveBuilderStandard<OnLogDf, LinearInterpolator> DCurve = new SingleCurveBuilderStandard<OnLogDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);
        MultiCurveBuilder<SimpleCubicInterpolator> MC = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);

        // I build a single curve
        SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator> SC = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(rs);

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

        // Standard swap
        double swapRate = 0.01; // change it and see the effect on difference in MtM
        // if swapRate<parRate: single curve MtM > multi curve MtM (in this example parRate 5Y is 2.372%)
        // if swapRate>parRate: single curve MtM < multi curve MtM
        string swapTenor = "5y";
        double N = 100000000;
        SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, swapRate, swapTenor, BuildingBlockType.EURSWAP6M);

        Console.WriteLine("Calculating Mark To Market of vanilla rec swap");
        Console.WriteLine("Nominal {0:n} SwapRate: {1:P2} Tenor: {2}", N, swapRate, swapTenor);
        Console.WriteLine("Using Multi Curve, the MtM is: {0:n2} (ref ParRate {1:P4})", NPV(y, MC) * N, MC.SwapFwd(refDate, swapTenor));
        Console.WriteLine("Using Single Curve, the MtM is: {0:n2} (ref ParRate {1:P4})", NPV(y, SC) * N, SC.SwapFwd(refDate, swapTenor));
    }

    public static void PerformanceMultiCurveProcess()
    {
        // ref date
        Date refDate = (new Date(DateTime.Now)).mod_foll();  // To make the test more robust, choose a date fixed: for example 26 Nov ...

        #region eonia market data
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
        rs.Add(1.26e-2, "6m", BuildingBlockType.EURDEPO);
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

        // initialise discount curve - it is the same for both forwarding curve
        SingleCurveBuilderStandard<OnDf, LinearInterpolator> DCurve = new SingleCurveBuilderStandard<OnDf, LinearInterpolator>(mktRates, OneDimensionInterpolation.Linear);

        // initializing the stopwatch
        Stopwatch stopwatch = new Stopwatch();

        #region MultiCurveBuilder
        Console.WriteLine("Class MultiCurveBuilder...");
        stopwatch.Start();
        MultiCurveBuilder<SimpleCubicInterpolator> MC = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);
        stopwatch.Stop();
        Console.WriteLine("Done. Time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);
        #endregion

        stopwatch.Reset();  // resetting the stopwatch

        #region MultiCurveBuilder2
        Console.WriteLine("Class MultiCurveBuilder2...");
        stopwatch.Start();
        MultiCurveBuilder2<SimpleCubicInterpolator> MC2 = new MultiCurveBuilder2<SimpleCubicInterpolator>(rs, DCurve);
        stopwatch.Stop();
        Console.WriteLine("Done. Time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);
        #endregion

        // Now we want to compare the fwd calculated for each of two classes. We use schedule for longer swap
        SwapStyle S = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, 3.29e-2, "30y", BuildingBlockType.EURSWAP6M);

        double[] fwds = (from c in S.scheduleLeg2.fromDates  // fwds from MultiCurveBuilder 
                         select MC.Fwd(c)).ToArray();

        double[] fwds2 = (from c in S.scheduleLeg2.fromDates  // fwds from MultiCurveBuilder2
                          select MC.Fwd(c)).ToArray();

        // Do you want to check that fwd are the same?
        Console.WriteLine("\nShow forward rates? y/n");
        char con = Console.ReadKey().KeyChar;
        if (con == 'y' || con == 'Y')
        {
            Console.WriteLine("\nA = MultiCurveBuilder ; B = MultiCurveBuilder2");

            for (int i = 0; i < fwds.Count(); i++)
            {
                Console.WriteLine("A:{0:P3}  B:{1:P3}  (A-B):{2:P3}", fwds[i], fwds2[i], fwds[i] - fwds[i]);
            }
        }
    }

    public static void CheckInterpOnFwd()
    {
        // Reference Date
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        #region eonia market data
        // I populate market rates set: from file, from real time, ...
        RateSet OISall = new RateSet(refDate);

        OISall.Add(0.371e-2, "1w", BuildingBlockType.EONIASWAP);
        OISall.Add(0.362e-2, "2w", BuildingBlockType.EONIASWAP);
        OISall.Add(0.354e-2, "3w", BuildingBlockType.EONIASWAP);

        OISall.Add(0.352e-2, "1m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.355e-2, "2m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.349e-2, "3m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.343e-2, "4m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.342e-2, "5m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.341e-2, "6m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.340e-2, "7m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.342e-2, "8m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.343e-2, "9m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.344e-2, "10m", BuildingBlockType.EONIASWAP);
        OISall.Add(0.347e-2, "11m", BuildingBlockType.EONIASWAP);

        OISall.Add(0.35e-2, "1Y", BuildingBlockType.EONIASWAP);
        OISall.Add(0.429e-2, "2Y", BuildingBlockType.EONIASWAP);
        OISall.Add(0.576e-2, "3Y", BuildingBlockType.EONIASWAP);
        OISall.Add(0.79e-2, "4Y", BuildingBlockType.EONIASWAP);
        OISall.Add(1.025e-2, "5Y", BuildingBlockType.EONIASWAP);
        OISall.Add(1.252e-2, "6Y", BuildingBlockType.EONIASWAP);
        OISall.Add(1.456e-2, "7Y", BuildingBlockType.EONIASWAP);
        OISall.Add(1.631e-2, "8Y", BuildingBlockType.EONIASWAP);
        OISall.Add(1.778e-2, "9Y", BuildingBlockType.EONIASWAP);
        OISall.Add(1.9070e-2, "10Y", BuildingBlockType.EONIASWAP);
        OISall.Add(2.017e-2, "11Y", BuildingBlockType.EONIASWAP);
        OISall.Add(2.113e-2, "12Y", BuildingBlockType.EONIASWAP);
        // From here interpolation is need
        OISall.Add(2.304e-2, "15Y", BuildingBlockType.EONIASWAP);
        OISall.Add(2.397e-2, "20Y", BuildingBlockType.EONIASWAP);
        OISall.Add(2.377e-2, "25Y", BuildingBlockType.EONIASWAP);
        OISall.Add(2.341e-2, "30Y", BuildingBlockType.EONIASWAP);
        #endregion

        #region Swap Market Data
        // RateSet EUR 6m swap
        RateSet SwapAll = new RateSet(refDate);
        SwapAll.Add(1.328e-2, "6m", BuildingBlockType.EURDEPO);
        SwapAll.Add(1.215e-2, "1y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(1.171e-2, "2y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(1.254e-2, "3y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(1.425e-2, "4y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(1.627e-2, "5y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(1.824e-2, "6y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(1.997e-2, "7y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.144e-2, "8y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.265e-2, "9y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.369e-2, "10y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.461e-2, "11y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.538e-2, "12y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.602e-2, "13y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.652e-2, "14y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.689e-2, "15y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.715e-2, "16y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.731e-2, "17y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.739e-2, "18y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.741e-2, "19y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.738e-2, "20y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.732e-2, "21y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.724e-2, "22y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.714e-2, "23y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.702e-2, "24y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.69e-2, "25y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.6781e-2, "26y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.666e-2, "27y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.655e-2, "28y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.644e-2, "29y", BuildingBlockType.EURSWAP6M);
        SwapAll.Add(2.634e-2, "30y", BuildingBlockType.EURSWAP6M);

        // RateSet EUR 6m swap
        RateSet SwapLess = new RateSet(refDate);
        SwapLess.Add(1.328e-2, "6m", BuildingBlockType.EURDEPO);
        SwapLess.Add(1.215e-2, "1y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(1.171e-2, "2y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(1.254e-2, "3y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(1.425e-2, "4y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(1.627e-2, "5y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(1.824e-2, "6y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(1.997e-2, "7y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.144e-2, "8y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.265e-2, "9y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(2.369e-2, "10y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.461e-2, "11y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(2.538e-2, "12y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.602e-2, "13y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.652e-2, "14y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(2.689e-2, "15y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.715e-2, "16y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.731e-2, "17y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.739e-2, "18y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.741e-2, "19y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(2.738e-2, "20y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.732e-2, "21y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.724e-2, "22y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.714e-2, "23y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.702e-2, "24y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(2.69e-2, "25y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.6781e-2, "26y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.666e-2, "27y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.655e-2, "28y", BuildingBlockType.EURSWAP6M);
        // SwapLess.Add(2.644e-2, "29y", BuildingBlockType.EURSWAP6M);
        SwapLess.Add(2.634e-2, "30y", BuildingBlockType.EURSWAP6M);
        #endregion

        #region Select curve for test
        IRateCurve myCurve = null;

        // SingleCurve: uncomment one of this if you want to test single curve
        // myCurve = new SingleCurveBuilderStandard<OnDf, LinearInterpolator>(SwapAll, OneDimensionInterpolation.Linear);
        // myCurve = new SingleCurveBuilderStandard<OnDf, LinearInterpolator>(SwapLess, OneDimensionInterpolation.Linear);
        // myCurve = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(SwapAll);
        // myCurve = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(SwapLess);
        // myCurve = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(SwapLess);
        // myCurve = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(SwapAll);
        // myCurve = new SingleCurveBuilderInterpBestFit<OnLogDf, SimpleCubicInterpolator>(SwapLess);

        // MultiCurve: uncomment one of this if you want to test multi curve
        SingleCurveBuilderStandard<OnDf, LinearInterpolator> DCurve = new SingleCurveBuilderStandard<OnDf, LinearInterpolator>(OISall, OneDimensionInterpolation.Linear);
        // myCurve = new MultiCurveBuilder<SimpleCubicInterpolator>(SwapAll, DCurve);
        // myCurve = new MultiCurveBuilder<SimpleCubicInterpolator>(SwapLess, DCurve);
        // myCurve = new MultiCurveBuilder<LinearInterpolator>(SwapAll, DCurve);
        myCurve = new MultiCurveBuilder<LinearInterpolator>(SwapLess, DCurve);

        #endregion

        #region Output data in Excel
        // I get longer swap
        SwapStyle S = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, 0.0, "30y", BuildingBlockType.EURSWAP6M);

        // rates level
        double[] fwds = (from c in S.scheduleLeg2.fromDates
                         select myCurve.Fwd(c)).ToArray();

        // Distance in year
        double[] dt = (from c in S.scheduleLeg2.fromDates
                       select refDate.YF_30_360E(c)).ToArray();

        ExcelMechanisms exl = new ExcelMechanisms();
        exl.printOneExcel<double>(new Vector<double>(dt), new Vector<double>(fwds), "Curve Test", "Time in year", "Rate", "FWD 6m");
        #endregion
    }

    public static void DiscountFactors()
    {
        // ref date
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        #region Eonia market data
        // I populate market rates set: from file, from real time, ...
        RateSet OIS = new RateSet(refDate);

        OIS.Add(0.447e-2, "1w", BuildingBlockType.EONIASWAP);
        OIS.Add(0.583e-2, "2w", BuildingBlockType.EONIASWAP);
        OIS.Add(0.627e-2, "3w", BuildingBlockType.EONIASWAP);

        OIS.Add(0.635e-2, "1m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.675e-2, "2m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.705e-2, "3m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.734e-2, "4m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.758e-2, "5m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.780e-2, "6m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.798e-2, "7m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.816e-2, "8m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.834e-2, "9m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.849e-2, "10m", BuildingBlockType.EONIASWAP);
        OIS.Add(0.864e-2, "11m", BuildingBlockType.EONIASWAP);

        OIS.Add(0.878e-2, "1Y", BuildingBlockType.EONIASWAP);
        OIS.Add(1.098e-2, "2Y", BuildingBlockType.EONIASWAP);
        OIS.Add(1.36e-2, "3Y", BuildingBlockType.EONIASWAP);
        OIS.Add(1.639e-2, "4Y", BuildingBlockType.EONIASWAP);
        OIS.Add(1.9e-2, "5Y", BuildingBlockType.EONIASWAP);
        OIS.Add(2.122e-2, "6Y", BuildingBlockType.EONIASWAP);
        OIS.Add(2.308e-2, "7Y", BuildingBlockType.EONIASWAP);
        OIS.Add(2.467e-2, "8Y", BuildingBlockType.EONIASWAP);
        OIS.Add(2.599e-2, "9Y", BuildingBlockType.EONIASWAP);
        OIS.Add(2.715e-2, "10Y", BuildingBlockType.EONIASWAP);
        OIS.Add(2.818e-2, "11Y", BuildingBlockType.EONIASWAP);
        OIS.Add(2.908e-2, "12Y", BuildingBlockType.EONIASWAP);
        // From here interpolation is need
        OIS.Add(3.093e-2, "15Y", BuildingBlockType.EONIASWAP);
        OIS.Add(3.173e-2, "20Y", BuildingBlockType.EONIASWAP);
        OIS.Add(3.114e-2, "25Y", BuildingBlockType.EONIASWAP);
        OIS.Add(3.001e-2, "30Y", BuildingBlockType.EONIASWAP);
        #endregion

        #region Swap Market Data
        // RateSet EUR 6m swap
        RateSet LIBOR = new RateSet(refDate);
        LIBOR.Add(1.26e-2, "6m", BuildingBlockType.EURDEPO);
        LIBOR.Add(1.42e-2, "1y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(1.635e-2, "2y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(1.872e-2, "3y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(2.131e-2, "4y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(2.372e-2, "5y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(2.574e-2, "6y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(2.743e-2, "7y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(2.886e-2, "8y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.004e-2, "9y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.107e-2, "10y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.198e-2, "11y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.278e-2, "12y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.344e-2, "13y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.398e-2, "14y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.438e-2, "15y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.467e-2, "16y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.484e-2, "17y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.494e-2, "18y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.495e-2, "19y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.491e-2, "20y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.483e-2, "21y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.471e-2, "22y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.455e-2, "23y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.436e-2, "24y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.415e-2, "25y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.391e-2, "26y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.366e-2, "27y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.340e-2, "28y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.314e-2, "29y", BuildingBlockType.EURSWAP6M);
        LIBOR.Add(3.29e-2, "30y", BuildingBlockType.EURSWAP6M);
        #endregion

        #region Inizialize my curves
        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> MC = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(OIS, OneDimensionInterpolation.SimpleCubic);
        SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator> SC = new SingleCurveBuilderStandard<OnLogDf, SimpleCubicInterpolator>(LIBOR, OneDimensionInterpolation.SimpleCubic);
        MultiCurveBuilder<SimpleCubicInterpolator> MC2 = new MultiCurveBuilder<SimpleCubicInterpolator>(LIBOR, MC);
        #endregion

        #region DF dates
        // I create a schedule for longer swap, get relevant dates for df
        SwapStyle S = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, 3.29e-2, "30y", BuildingBlockType.EURSWAP6M);

        double d = S.scheduleLeg2.fromDates.Last().SerialValue;
        double[] SCdf = (from c in S.scheduleLeg2.toDates
                         select SC.Df(c)).ToArray(); // single curve discount factor
        double[] MCdf = (from c in S.scheduleLeg2.toDates
                         select MC2.Df(c)).ToArray(); // multi curve discount factor            

        // We calculate single curve forward rate
        int n = S.scheduleLeg2.fromDates.Count();
        double[] SCfwd = new double[n];
        for (int j = 0; j < S.scheduleLeg2.fromDates.Count(); j++)
        {
            // formula 11.1.1 Option Pricing Formulas Espen Haug second edition
            // "Swap and Other Derivatives" R.Flavell page 53
            Date From = S.scheduleLeg2.fromDates[j];
            Date To = S.scheduleLeg2.toDates[j];
            double yf = From.YF(To, Dc._Act_360);
            double df_ini = SC.Df(From);
            double df_end = SC.Df(To);
            SCfwd[j] = (df_ini / df_end - 1) / yf;
        }

        // I check some values
        Console.WriteLine(SC.SwapFwd(refDate.AddYears(1), "2y"));
        Console.WriteLine(MC2.SwapFwd(refDate.AddYears(1), "2y"));

        // double[] SCfwd = (from c in S.scheduleLeg2.fromDates
        //                  select SC.Fwd(c)).ToArray(); // single curve fwd
        double[] MCfwd = (from c in S.scheduleLeg2.fromDates
                          select MC2.Fwd(c)).ToArray(); // multi curve fwd            

        double[] dt = (from c in S.scheduleLeg2.fromDates
                       select refDate.YF_30_360E(c)).ToArray(); // time 
        #endregion

        #region print value to excel
        ExcelMechanisms exl = new ExcelMechanisms(); // my excel mechanism
        List<string> StringList = new List<string>();  // string list
        StringList.Add("Single Curve Df");
        StringList.Add("Multi Curve Df");
        List<Vector<double>> DFs = new List<Vector<double>>(); // df list
        DFs.Add(new Vector<double>(SCdf));
        DFs.Add(new Vector<double>(MCdf));

        List<string> StringListFwd = new List<string>();  // string list
        StringListFwd.Add("Single Curve Fwd");
        StringListFwd.Add("Multi Curve Fwd");
        List<Vector<double>> Fwds = new List<Vector<double>>(); // df list
        Fwds.Add(new Vector<double>(SCfwd));
        Fwds.Add(new Vector<double>(MCfwd));

        // printing out           
        exl.printInExcel<double>(new Vector<double>(dt), StringList, DFs, "Comparing DF", "Time", "DFs");
        exl.printInExcel<double>(new Vector<double>(dt), StringListFwd, Fwds, "Comparing Fwd", "Time", "Fwds");
        #endregion
    }
}