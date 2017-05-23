// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestSimpleFormulas.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestSimpleFormulas
{
    public static void Main()
    {
        SimpleFormulas();
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
}
