// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// LogLinearInterpolator.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
// It is based on Datasim interpolators code. It uses C# type for array.

public class LogLinearInterpolator : BaseOneDimensionalInterpolator
{
    public LogLinearInterpolator() { }

    public LogLinearInterpolator(double[] xarr, double[] yarr): base(xarr, yarr)
    {
    }

    public override double Solve(double xvar)
    {
        // Find the interpolated valued at a value x)
        int j = findAbscissa(xvar);	

        double exponent = ((xvar - xarr[j]) / (xarr[j + 1] - xarr[j]));
        return Math.Pow(yarr[j + 1] / yarr[j], exponent) * yarr[j];
    }
}
