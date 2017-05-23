// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestMonotoneConvex.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestMonotoneConvex
{
    public static void Main() 
    {
        SimplifiedMonotoneConvex();
    }

    public static void SimplifiedMonotoneConvex()
        {
            SortedList<double, double> TermRate = new SortedList<double, double>();

            #region using a different contructor
            double[] t = new double[] { 0.1, 1, 4, 9, 20, 30 };
            double[] r = new double[] { 0.081, 0.07, 0.044, 0.07, 0.04, 0.03 };

            #endregion
            // We consider inputs as continuous rates and negative forwards are not allowed  

            // Monotone Convex interpolator (Hagan-West approach)  
            MonotoneConvex HaganWest = new MonotoneConvex(t, r);
            List<double> rr = new List<double>();
            List<double> fwd = new List<double>();
            List<double> tt = new List<double>();
            double step = 0.01;
            int MaxIndex = System.Convert.ToInt32((100 * HaganWest.dTerms.Last() + 20));

            double x_ = 0.0;
            for (int i = 0; i <= MaxIndex; i++)
            {
                x_ = step * i;
                tt.Add(x_);
                double k = HaganWest.Interpolant(x_);
                rr.Add(HaganWest.Interpolant(x_));
                fwd.Add(HaganWest.Forward(x_));
            }
            // Create the abscissa values
            double[] terms = tt.ToArray();
            // Compute interpolated values
            double[] rates = HaganWest.GetInterpolatedRate(terms);
            double[] forwards = HaganWest.GetInterpolatedFwd(terms);
            // My excel mechanism
            ExcelMechanisms exl = new ExcelMechanisms();
            exl.printOneExcel<double>(new Vector<double>(tt.ToArray()), new Vector<double>(forwards)
                    , "Monotone Convex", "time", "discrete forward", "dis fwd");

            exl.printOneExcel<double>(new Vector<double>(tt.ToArray()), new Vector<double>(rates)
                        , "Monotone Convex", "time", "discrete forward", "dis fwd");
        }
}

