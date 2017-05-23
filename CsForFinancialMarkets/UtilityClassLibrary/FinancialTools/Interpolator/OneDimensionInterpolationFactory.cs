// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// OneDimensionInterpolationFactory.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections;

public class OneDimInterpFactory 
{
    public IInterpolate FactoryMethod(OneDimensionInterpolation i, double[] xarr, double[] yarr) 
    {
        // many more interpolators can be added in switch, this is for demonstration purposes only
        switch (i)
        {
            case OneDimensionInterpolation.Linear:
                return new LinearInterpolator(xarr, yarr);

            case OneDimensionInterpolation.LogLinear:
                return new LogLinearInterpolator(xarr, yarr);

            case OneDimensionInterpolation.SimpleCubic:
                return new SimpleCubicInterpolator(xarr, yarr);
            default:
                break;
        }
        return null;
    }
}