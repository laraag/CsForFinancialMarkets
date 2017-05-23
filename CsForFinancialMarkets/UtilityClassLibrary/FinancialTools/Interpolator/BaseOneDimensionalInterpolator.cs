// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// BaseOneDimensionalInterpolator.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// It is based on Datasim interpolators code. It uses C# type for array.

    public interface IInterpolate
    {
        // For an x value, compute the y value
        double Solve(double xvar);

        // Find x variable corresponding to a y variable
        int findAbscissa(double xvar);

        // Find interpolated curve corresponding to _any_ x array     
        double[] Curve(double[] xarr);

        // Find the curve corresponding to the input x array
        double[] Curve();
    }

  
    public abstract class BaseOneDimensionalInterpolator : IInterpolate
    {
        protected double[] xarr;		 // Abscissa x-values
        protected double[] yarr;		 // Function values

        // Number of subdivisions
        protected int n;

        public BaseOneDimensionalInterpolator() { }

        public BaseOneDimensionalInterpolator(double[] xarr, double[] yarr)
        {
            // Arrays must have the same size
            Ini(xarr, yarr);
        }

        public virtual void Ini(IEnumerable<double> xarr, IEnumerable<double> yarr)
        {
            // Arrays must have the same size
            this.xarr = xarr.ToArray<double>();
            this.yarr = yarr.ToArray<double>();
            n = xarr.Count();
        }

        // Derived classes must implement this method
        abstract public double Solve(double xvar);
        
        public double[] Curve(double[] xarr)
        {  // Create the interpolated curve

            int max = xarr.Length;
            double[] result = new double[max];

            for (int j = 0; j <max; j++)
            {
                result[j] = Solve(xarr[j]);
            }
            return result;
        }

        public double[] Curve()
        {  
            return Curve(this.xarr);
        }

        public virtual int findAbscissa(double xvar)
        {
            for (int j = 0; j < n - 1; j++)
            {
                if (xarr[j] <= xvar && xvar <= xarr[j + 1])
                {
                    return j;
                }
            }
            return -1;
        }        
    }

