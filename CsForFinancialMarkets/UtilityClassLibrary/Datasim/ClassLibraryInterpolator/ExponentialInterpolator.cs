 // ExponentialInterpolator.cs
 // 
 // Class to represent cubic spline interpolation
 // 
 // Code is default inline and we include some C utility functions
 // 
 // Last Modification Dates:
 // 
 // DD 2006-7-31 Kick-off code
 // DD 2006-8-1 Tested: 1) BC 2nd order terms
 // DD 2009-2-7 C# solution
 // DD 2010-5-15 Modified for linear interpolation
 // DD 2010-9-5 Now defined from a base class
 // DD 2010-9-5 Log linear (Haug 2007 page 487)
 // DD 2010-9-5 Exponential interpolator
 // 
 // (C) Datasim Education BV 2006-2010
 // 

 // All equations based on my C++ book, especially chapter 18

using System;

public class ExponentialInterpolator : BaseOneDimensionalInterpolator
{

	public ExponentialInterpolator(Vector<double> xarr, Vector<double> yarr) :base(xarr, yarr)
	{

	
	}

	public override double Solve(double xvar) 
	{  // Find the interpolated valued at a value x)

		int j = findAbscissa(xvar);	 // will give index of LHS value <= x
		

		 // Now use the formula; x in interval [ x[j], x[j+1] ]

       //  return y[j] + (xvar - x[j]) * (y[j + 1] - y[j]) / (x[j + 1] - x[j]);

         // V1
        /*double exponent = xvar;
        return Math.Pow(y[j + 1] / y[j], exponent) * y[j];*/

         // V2, Haug page 488
        double a1 = (xvar / x[j]) * ((x[j + 1] - xvar) / (x[j + 1] - x[j]));
        double a2 = (xvar / x[j+1]) * ((xvar - x[j]) / (x[j + 1] - x[j]));

        return Math.Pow(y[j],a1) * Math.Pow(y[j+1],a2);
    }
	
}


