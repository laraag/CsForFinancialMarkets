 // BadeOneDimensionalInterpolator.cs
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
 // DD 2010-9-5 Abstract base class (uses template method pattern) and 
 // new interface  // AG
 // BaseOneDimensionalInterpolator(double[] xarr, double[] yarr)  // AG
 // 
 // (C) Datasim Education BV 2006-2010
 // 
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IInterpolate
{
     // For an x value, compute the y value
    double Solve(double xvar);

     // Find x variable corresponding to a y variable
    int findAbscissa(double xvar);

     // Find interpolated curve corresponding to _any_ x array
    Vector<double> Curve(Vector<double> xarr);
     // AG 20nov10
    double[] Curve(double[] xarr);

     // Find the curve corresponding to the input x array
    Vector<double> Curve();
}

[Serializable]
public abstract class BaseOneDimensionalInterpolator : IInterpolate
{
	protected Vector<double> x;		 // Abscissa x-values
	protected Vector<double> y;		 // Function values

     // Number of subdivisions
	protected int N;

    public BaseOneDimensionalInterpolator() { }  // AG

     // public BaseOneDimensionalInterpolator(double[] xarr, double[] yarr)  // AG
     // {
     //    x = new Vector<double>(xarr,1);
     //    y = new Vector<double>(yarr,1);
     //    N = xarr.Length - 1;
     // }

	public BaseOneDimensionalInterpolator(Vector<double> xarr, Vector<double> yarr) 
	{

		 // Arrays must have the same size
		x = xarr;
		y = yarr;
		N = xarr.Size-1;

	}

    public virtual void Ini(IEnumerable<double> xarr, IEnumerable<double> yarr)
    {
        x = new Vector<double>(xarr.ToArray(),0);
        y = new Vector<double>(yarr.ToArray(),0);
        N = xarr.Count() -1;
    }

     // Derived classes must implement this method
    abstract public double Solve(double xvar); 
	
	
	public Vector<double> Curve(Vector<double> xarr) 
	{  // Create the interpolated curve

        Vector<double> result = new Vector<double>(xarr.Size, xarr.MinIndex);

		for (int j = xarr.MinIndex; j <= xarr.MaxIndex; j++)	
		{

			result[j] = Solve(xarr[j]);
		}

		return result;
	}

	public Vector<double> Curve() 
	{  // Create the interpolated curve, MEMBER DATA AS ABSCISSAE

		return Curve(x);
	}

   
    public virtual int findAbscissa(double xvar)
    {  // Will give index of LHS value <= x. Very simple algorithm

        int index = 0;

        for (int j = 0; j <= N - 1; j++)
        {
            if (x[j] <= xvar && xvar <= x[j + 1])
            {
                return index;
            }
            index++;
        }
         // Then x is in the interval [j, j+1]

        return index;
    }

     // AG 20nov10
    public double[] Curve(double[] xarr)
    {  // Create the interpolated curve

        Vector<double> V = Curve(new Vector<double>(xarr, 0));
        double[] outPut = new double[V.Length];
        int j = 0;
        for (int i = V.MinIndex; i <= V.MaxIndex; i++)
        {
            outPut[j] = V[i];
            j++;
        }
        return outPut;
    }
}


