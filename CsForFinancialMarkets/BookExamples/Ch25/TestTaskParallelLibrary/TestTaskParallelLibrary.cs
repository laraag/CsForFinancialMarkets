// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestTaskParallelLibrary.cs
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

class TestTaskParallelLibrary
{
    public static void Main() 
    {
        UsingTPLinImplementation();
    }

    public static void UsingTPLinImplementation()
    {
        // ref date, you should use the one you need
        Date refDate = (new Date(DateTime.Now)).mod_foll();

        #region eonia market data
        // I populate markets rates set: from file, from real time, ...
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
        rs.Add(1.123e-2, "6m", BuildingBlockType.EURDEPO);
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

        MultiCurveBuilder<SimpleCubicInterpolator> MC = new MultiCurveBuilder<SimpleCubicInterpolator>(rs, DCurve);

        Console.WriteLine("ProcessorCount: {0}", Environment.ProcessorCount); // number of processor
        #region MultiCurveBuilder sequential
        Console.WriteLine("Implementing shift sequential...");
        stopwatch.Start();
        IMultiRateCurve[] MCArrayFwdSeq = MC.ShiftedCurvesArrayFwdCurveSeq(0.01);
        IMultiRateCurve[] MCArrayDfSeq = MC.ShiftedCurvesArrayDCurveSeq(0.01);
        stopwatch.Stop();
        Console.WriteLine("Done. Time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);
        #endregion

        stopwatch.Reset(); // resetting the stopwatch

        #region MultiCurveBuilder2
        Console.WriteLine("Implementing shift parallel ...");
        stopwatch.Start();
        IMultiRateCurve[] MCArrayFwd = MC.ShiftedCurvesArrayFwdCurve(0.01);
        IMultiRateCurve[] MCArrayDf = MC.ShiftedCurvesArrayDCurve(0.01);
        stopwatch.Stop();
        Console.WriteLine("Done. Time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);
        #endregion
    }
}