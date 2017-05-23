// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// MultiThreadOnDf.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class TestMultiThreadOnDf
{
    public static void Main()
    {
        MultiThreadOnDf();
    }
 
    public static void MultiThreadOnDf()
    {
        Console.WriteLine("ProcessorCount: {0}", Environment.ProcessorCount); // number of processor
        DateTime timer; // setting up timer

        timer = DateTime.Now;

        #region Inputs
        // Start input, reference date.
        Date refDate = (new Date(DateTime.Now));

        // I populate market rates set: from file, from real time, ...
        RateSet mktRates = new RateSet(refDate);

        // Depos            
        mktRates.Add(1.123e-2, "3m", BuildingBlockType.EURDEPO);

        // Swap Vs 3M
        mktRates.Add(1.813e-2, "1Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.096e-2, "2Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.322e-2, "3Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.529e-2, "4Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.709e-2, "5Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.862e-2, "6Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(2.991e-2, "7Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.101e-2, "8Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.197e-2, "9Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.285e-2, "10Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.443e-2, "12Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.614e-2, "15Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.711e-2, "20Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.671e-2, "25Y", BuildingBlockType.EURSWAP3M);
        mktRates.Add(3.589e-2, "30Y", BuildingBlockType.EURSWAP3M);

        double firstFixing = 1.123e-2;
        #endregion end Inputs

        List<object[]> parm = new List<object[]>();
        parm.Add(new object[] { mktRates, new Date(2015, 8, 15), firstFixing });
        parm.Add(new object[] { mktRates, new Date(2017, 8, 15), firstFixing });
        parm.Add(new object[] { mktRates, new Date(2020, 8, 15), firstFixing });

        Console.WriteLine("Press: 1 for Sequential, 2 for MultiThread");
        string line = Console.ReadLine();

        if (line == "1")
        {
            Console.WriteLine("Sequential:");
            #region Sequential
            foreach (object[] parmSet in parm)
            {
                MyDF(parmSet);
            }
            #endregion
        }
        else if (line == "2")
        {
            Console.WriteLine("MultiThread:");
            #region Solution 1
            List<Thread> TL = new List<Thread>();

            Thread T1 = new Thread(new ParameterizedThreadStart(MyDF));
            Thread T2 = new Thread(new ParameterizedThreadStart(MyDF));
            Thread T3 = new Thread(new ParameterizedThreadStart(MyDF));

            T1.Start(parm[0]);
            T2.Start(parm[1]);
            T3.Start(parm[2]);
            T1.Join(); T2.Join(); T3.Join();
            #endregion
        }
        else
        {
            Console.WriteLine("Unknown selection");
        }

        // time for the full process
        Console.WriteLine("All Done in in {0}", DateTime.Now - timer);
    }

    public static void MyDF(object rateSet)
    {
        object[] o = (object[])rateSet;

        RateSet rs = (RateSet)o[0];
        RateSet myRateSet = new RateSet((Date)o[1]);
        foreach (RateSet r in rs)
        {
            myRateSet.Add(r.V, r.M.GetPeriodStringFormat(), r.T);
        }

        Date dfDate = new Date(2025, 8, 15);

        double fixing = (double)o[2];

        #region building curve

        SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator> C = new SingleCurveBuilderSmoothingFwd<OnLogDf, SimpleCubicInterpolator>(myRateSet, fixing);

        Console.WriteLine("Ref. {0:D}: DF: {1:F5}", myRateSet.refDate.DateValue, C.DF(dfDate));

        #endregion building curve
    }
}
