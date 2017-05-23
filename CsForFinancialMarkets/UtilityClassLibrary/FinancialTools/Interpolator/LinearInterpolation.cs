// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// LinearInterpolation.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// It is based on Datasim interpolators code. It uses C# type for array.
 
public class LinearInterpolator : BaseOneDimensionalInterpolator
{

        public LinearInterpolator() { } 

        public LinearInterpolator(double[] xarr, double[] yarr): base(xarr,yarr)
        {
        }

        public override double Solve(double xvar)
        {  // Find the interpolated valued at a value x)

            int j = findAbscissa(xvar);	 // will give index of LHS value <= x


            // Now use the formula; x in interval [ x[j], x[j+1] ]
            return yarr[j] + (xvar - xarr[j]) * (yarr[j + 1] - yarr[j]) / (xarr[j + 1] - xarr[j]);
        }

    }