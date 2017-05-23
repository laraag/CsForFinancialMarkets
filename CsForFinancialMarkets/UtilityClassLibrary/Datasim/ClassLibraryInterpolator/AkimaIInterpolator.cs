 // Akima1970Interpolator.cs
 // 
 // Interpolator using piecewise monotone rational
 // cubic polynomial.
 // 
 // 2011-2-14 DD initial code.
 // 2011-2-23 DD code copied/modified.
 // 2011-2-27 DD Hyman versions, simple formula
 // 2011-2-27 DD Hyman versions, formulae 2.1, 2.2 and correction 2.3. A few BUGS solved.
 // SOME BUG FIXES &$#(## *****
 // 2011-2-28 DD V4 up to Hyman 2.6
 // 
 // 2011-3-7 Implement Hyman V4 method 2.6 and Arithmetic Mean Method of Meng Tian equation (2)
 // to approximate the (unknown) derivative of the function.
 // 
 //  N.B. For input X and Y arrays, indexing starts at start index = 0.
 // 
 //  2011-3-15 DD Hyman 89 implementation.
 //  2011-4-26 Akima model 1970
 //  2011-4-28 DD first version ready; AG has a look.
 // 
 // (C) Datasim Education BV 2011
 // 
 // 

using System;
using System.Collections.Generic;
using System.Linq;


    public class Akima1970Interpolator : BaseOneDimensionalInterpolator
    {
         // Vectors of size n
        Vector<double> xarr;         // x values
        Vector<double> yarr;         // y values

         // Redundant arrays of size n-1
        Vector<double> h;            // h[i] = x[i+1] - x[i]
        Vector<double> delta;        // S[i] = (f[i+1] - f[i]) / h[i]

        Vector<double> d;            // Approximation to f' at mesh points

        int n;                       // Number of data points

        public override int findAbscissa(double xvar)
        {  // Will give index of LHS value <= x. Very simple algorithm
             // Value in range [1,n-1]!!!

            for (int j = 0; j <= n - 1; ++j)
            {
                if (xarr[j] <= xvar && xvar <= xarr[j + 1])
                {
                    return j;
                }

            }
             // Then x is in the interval [j, j+1].

            return 999;
        }

        public void init()
        {
             // The local mesh spacing
            h = new Vector<double>(n - 1, 0);
            for (int i = 0; i < n - 1; ++i)
            {
                h[i] = xarr[i + 1] - xarr[i];  // Hyman's Delta x[i + 1/2]
            }


             // Slope of the linear interpolant between the data points.
            delta = new Vector<double>(n - 1, 0);
            for (int i = 0; i < delta.Length; ++i)
            {
                delta[i] = (yarr[i + 1] - yarr[i]) / h[i];  // Hyman S[i+1/2]
            }

            Vector<double> w = new Vector<double>(n - 1, 0);
            for (int i = 1; i < n - 1; ++i)
            {
                w[i] = Math.Abs(delta[i] - delta[i - 1]);
            }

            double tmp1, tmp2;
             // double tol = 0.0000001;
            d = new Vector<double>(n, 0);
            for (int i = 2; i < d.Length - 2; ++i)
            {
                tmp1 = w[i + 1]; tmp2 = w[i - 1];

                /* if (tmp1 <= tol && tmp2 > tol)
                 {
                     d[i] = delta[i];
                 }

                 if (tmp2 <= tol && tmp1 > tol)
                 {
                     d[i] = delta[i-1];
                 }

                 if (tmp2 > tol  || tmp1 > tol)*/
                if (tmp1 != 0.0 || tmp2 != 0.0)
                {
                    d[i] = (w[i + 1] * delta[i - 1] + w[i - 1] * delta[i]) / (w[i + 1] + w[i - 1]);
                }

                 // if (tmp1 <= tol && tmp2 <= tol)
                else
                {
                    d[i] = (h[i] * delta[i - 1] + h[i - 1] * delta[i]) / (h[i] + h[i - 1]);
                     //   d[i] = 0.5 * (delta[i-1] + delta[i]);  // Akima's original spurious case
                }
            }

            d[0] = CreateEndPoint(xarr, yarr, 0, 0, 1, 2);
            d[1] = CreateEndPoint(xarr, yarr, 1, 0, 1, 2);
            d[n - 2] = CreateEndPoint(xarr, yarr, n - 2, n - 3, n - 2, n - 1);
            d[n - 1] = CreateEndPoint(xarr, yarr, n - 1, n - 3, n - 2, n - 1);
        }

        public Akima1970Interpolator() { }

        public Akima1970Interpolator(Vector<double> abscissa, Vector<double> RHS)
        {

            n = abscissa.Length;

             // BUG FIX
            int startIndex = 0;
            xarr = new Vector<double>(n, startIndex);
            yarr = new Vector<double>(n, startIndex);

            for (int i = 0; i < n; ++i)
            {
                xarr[i] = abscissa[i + startIndex];
            }

            for (int i = 0; i < n; ++i)
            {
                yarr[i] = RHS[i + startIndex];
            }

            init();
        }


        public Akima1970Interpolator(double[] abscissa, double[] RHS)
        {

            n = abscissa.Length;

            int startIndex = 0;
            xarr = new Vector<double>(n, startIndex);
            yarr = new Vector<double>(n, startIndex);

            for (int i = 0; i < n; ++i)
            {
                xarr[i] = abscissa[i];
            }

            for (int i = 0; i < n; ++i)
            {
                yarr[i] = RHS[i];
            }

            init();
        }

        public override double Solve(double x)
        {
             // Find which interval [x[i], x[i+1]] is in. 1 <= i <= n-1
            int i = findAbscissa(x);
            // Console.WriteLine("Index {0}", i);

            double del = (x - xarr[i]);
            double del2 = del * del;
            double del3 = del * del * del;

             // Akima
            double a0 = yarr[i];
            double a1 = d[i];
            double a2 = (3.0 * delta[i] - 2.0 * d[i] - d[i + 1]) / h[i];
            double a3 = (d[i + 1] + d[i] - 2.0 * delta[i]) / (h[i] * h[i]);

            return a0 + a1 * del + a2 * del2 + a3 * del3;

        }

        public override void Ini(IEnumerable<double> abscissa, IEnumerable<double> RHS) 
        {
            n = abscissa.Count();

            int startIndex = 0;
            xarr = new Vector<double>(n, startIndex);
            yarr = new Vector<double>(n, startIndex);

            for (int i = 0; i < n; ++i)
            {
                xarr[i] = abscissa.ElementAt(i);
            }

            for (int i = 0; i < n; ++i)
            {
                yarr[i] = RHS.ElementAt(i);
            }

            init();
        }

         // public Vector<double> Curve(Vector<double> myXarr)
         // {  // Create the interpolated curve

         //    Vector<double> result = new Vector<double>(myXarr.Size, myXarr.MinIndex);

         //    for (int j = result.MinIndex; j <= result.MaxIndex; j++)
         //    {
         //        result[j] = Solve(myXarr[j]);
         //    }

         //    return result;
         // }

        private double CreateEndPoint(
                Vector<double> Xarr, Vector<double> Yarr,
                int indexT,
                int index0, int index1, int index2)
        {
            double x0 = Yarr[index0];
            double x1 = Yarr[index1];
            double x2 = Yarr[index2];

            double t = Xarr[indexT] - Xarr[index0];
            double t1 = Xarr[index1] - Xarr[index0];
            double t2 = Xarr[index2] - Xarr[index0];

            double a = (x2 - x0 - (t2 / t1 * (x1 - x0))) / ((t2 * t2) - (t1 * t2));
            double b = (x1 - x0 - (a * t1 * t1)) / t1;
            return (2 * a * t) + b;
        }


    }
