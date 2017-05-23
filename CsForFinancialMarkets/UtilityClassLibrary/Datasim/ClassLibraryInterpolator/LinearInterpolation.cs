using System;

    [Serializable]
    public class LinearInterpolator : BaseOneDimensionalInterpolator
    {

        public LinearInterpolator() { } // AG

        public LinearInterpolator(Vector<double> xarr, Vector<double> yarr)
            : base(xarr, yarr)
        {


        }
        public LinearInterpolator(double[] xarr, double[] yarr) : base(new Vector<double>(xarr,0), new Vector<double>(yarr,0) )
        {
        }

        public override double Solve(double xvar)
        {  // Find the interpolated valued at a value x)

            int j = findAbscissa(xvar);	 // will give index of LHS value <= x


             // Now use the formula; x in interval [ x[j], x[j+1] ]
            return y[j] + (xvar - x[j]) * (y[j + 1] - y[j]) / (x[j + 1] - x[j]);
        }

    }


 // }