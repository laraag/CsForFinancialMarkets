// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// BiLinearInterpolator.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
// It is based on Datasim interpolators code. It uses C# type for array.

    public struct Pair<T>
    {
        public T first;
        public T second;

        public Pair(T t1, T t2) { first = t1; second = t2; }
    }

    public class BilinearInterpolator
    {
        private double[] x1Arr;		         // Abscissa x1-values
        private double[] x2Arr;		         // Abscissa x2-values

        private double[,] gridVals;      // Function values

        public Pair<int> findAbscissa(double x, double y)
        {  // Will give index of LHS values <= x, y, z. Very simple algorithm

            // Find separate components
            int firstIndex = 0;
            int secondIndex = 0;
            
            int x1ArrL = x1Arr.Length;
            int x2ArrL = x2Arr.Length;

            // Good candidate for parallel processing
            for (int j = 0; j < x1ArrL; j++)
            {
                if (x1Arr[j] <= x && x <= x1Arr[j + 1])
                {
                    goto L1;
                }
                firstIndex++;
            }

        L1:            
            for (int j = 0; j < x2ArrL; j++)
            {
                if (x2Arr[j] <= y && y <= x2Arr[j + 1])
                {
                    goto L2;
                }
                secondIndex++;
            }

        L2:
            return new Pair<int>(firstIndex, secondIndex);
        }

        // initialise data member
        private void Ini(double[] x1arr, double[] x2arr, List<double[]> gridValues)
        {
            this.x1Arr = x1arr;
            this.x2Arr = x2arr;

            // Arrays must have the same size
            int x1L = x1arr.Length;
            int x2L = x2arr.Length;
            gridVals = new double[x1L, x2L];

            for (int c = 0; c < x2L; c++)
            {
                double[] data = gridValues[c];
                for (int r = 0; r < x1L; r++)
                {
                    gridVals[r, c] = data[r];
                }
            }
        }

        public BilinearInterpolator() { }
       
        // More constructor
        public BilinearInterpolator(double[] x1arr, double[] x2arr, List<double[]> gridValues)
        {           
            Ini(x1arr, x2arr, gridValues);
        }

        public double Solve(double x, double y)
        {  // Find the interpolated values at a point (x1Var, x2Var)

            Pair<int> p = findAbscissa(x, y);

            int i = p.first;
            int j = p.second;
            
            // See Wiki
            double Q11 = gridVals[i, j]; double Q22 = gridVals[i + 1, j + 1];
            double Q12 = gridVals[i, j + 1]; double Q21 = gridVals[i + 1, j];

            double x1 = x1Arr[i]; double x2 = x1Arr[i + 1];
            double y1 = x2Arr[j]; double y2 = x2Arr[j + 1];

            double factor = 1.0 / ((x2 - x1) * (y2 - y1));

            double val1 = (Q11 * (x2 - x) * (y2 - y) + Q21 * (x - x1) * (y2 - y) + Q12 * (x2 - x) * (y - y1) + Q22 * (x - x1) * (y - y1)) * factor;

            return val1;
        }

        // get starting x1Arr in C# array format
        public double[] GetXarr
        {
            get { return x1Arr; }
        }

        // get starting x2Arr in C# array format
        public double[] GetYarr
        {
            get { return x2Arr; }
        }
    }



