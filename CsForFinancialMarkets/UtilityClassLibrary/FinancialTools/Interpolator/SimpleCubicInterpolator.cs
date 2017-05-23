// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// SimpleCubicInterpolator.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// It is based on Datasim interpolators code. It uses C# type for array.

    public class SimpleCubicInterpolator : BaseOneDimensionalInterpolator
    {
        double[] h;
        double[] delta;
        double[] d;

        public SimpleCubicInterpolator() { }

        public SimpleCubicInterpolator(double[] xarr, double[] yarr): base(xarr,yarr)
        {
            Ini(xarr,yarr);
        }

        public override void Ini(IEnumerable<double> xarr, IEnumerable<double> yarr)
        {
            // Arrays must have the same size
            this.xarr = xarr.ToArray<double>();
            this.yarr = yarr.ToArray<double>();
            n = xarr.Count();
            init();
        }

        public void init()
        {
            // The local mesh spacing
            h = new double[n - 1];
            for (int i = 0; i < n - 1; ++i)
            {
                h[i] = xarr[i + 1] - xarr[i];  // Hyman's Delta x[i + 1/2]
            }

            // Slope of the linear interpolant between the data points.
            // N.B. Hyman page 647, MUST BE defined at end points
            delta = new double[n + 1];
            for (int i = 0; i < n - 1; ++i)
            {
                delta[i + 1] = (yarr[i + 1] - yarr[i]) / h[i];  // Hyman's S[i + 1/2]
            }

            // Endpoint values Hyman page 647, paragraph 4 text
            delta[0] = delta[1];
            delta[n] = delta[n - 1];
            d = new double[n];

            // KRUGER APPROX.
            for (int j = 1; j < n - 1; ++j)
            {
                if (delta[j] * delta[j + 1] < 0.0)
                {
                    d[j] = 0.0;
                }
                else
                {
                    d[j] = 2.0 / (1.0 / delta[j] + 1.0 / delta[j + 1]);
                }
            }
            d[0] = (3.0 * delta[1] - d[1]) / 2.0;
            d[n - 1] = (3.0 * delta[n - 1] - d[n - 2]) / 2.0;
        }

        public override double Solve(double x)
        {
            // Equation (2.2) of Hyman
            int i = findAbscissa(x);

            double del = (x - xarr[i]);
            double del2 = del * del;
            double del3 = del * del * del;

            double c0 = yarr[i];
            double c1 = d[i];
            double c2 = (-2 * d[i] - d[i + 1] + 3 * delta[i + 1]) / h[i];
            double c3 = (d[i] + d[i + 1] - 2 * delta[i + 1]) / (h[i] * h[i]);

            return c0 + c1 * del + c2 * del2 + c3 * del3; 
        }
    }

