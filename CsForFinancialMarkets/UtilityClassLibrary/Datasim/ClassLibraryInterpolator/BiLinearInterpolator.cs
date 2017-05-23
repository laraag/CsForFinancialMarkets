 // BilinearInterpolator.cs
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
 // DD 2010-7-3 TriLinear interpolation case; V2 TriLinear<Type1, Type2> ?? e.g. Date etc.??
 // DD 2010-9-8 Trilinear
 // DD 2012-2-29 Bilinear
 // DD 2012-3-12 Bug fix.
 // AG 2012-3-13 added new constructor:public BilinearInterpolator(double[] x1arr, double[] x2arr, List <double[]> gridValues); 
 //              added a method: Ini();
 //              added default constructor;
 // AG 2012-3-15 Added 2 methods: public double[] GetXarr, public double[] GetYarr

 // 
 // (C) Datasim Education BV 2006-2012
 // 

using System;
using System.Collections.Generic;

    public struct Pair<T>
    {
        public T first;
        public T second;

        public Pair(T t1, T t2) { first = t1; second = t2; }
    }

    public class BilinearInterpolator
    {
        private Vector<double> x1Arr;		         // Abscissa x1-values
        private Vector<double> x2Arr;		         // Abscissa x2-values

        private NumericMatrix<double> gridVals;      // Function values

        public Pair<int> findAbscissa(double x, double y)
        {  // Will give index of LHS values <= x, y, z. Very simple algorithm

             // Find separate components
            int firstIndex = x1Arr.MinIndex;
            int secondIndex = x2Arr.MinIndex;

             // Good candidate for parallel processing
            for (int j = x1Arr.MinIndex; j <= x1Arr.MaxIndex - 1; j++)
            {
                if (x1Arr[j] <= x && x <= x1Arr[j + 1])
                {
                    goto L1;
                }
                firstIndex++;
            }

        L1:
            //Console.WriteLine("1st index: {0}", firstIndex);
            for (int j = x2Arr.MinIndex; j <= x2Arr.MaxIndex - 1; j++)
            {
                if (x2Arr[j] <= y && y <= x2Arr[j + 1])
                {
                    goto L2;
                }
                secondIndex++;
            }

        L2:
            //Console.WriteLine("2nd index: {0}", secondIndex);

            return new Pair<int>(firstIndex, secondIndex);
        }

         // initialise data member
        private void Ini(Vector<double> x1arr, Vector<double> x2arr, NumericMatrix<double> gridValues)
        {
             // Arrays must have the same size
            x1Arr = x1arr;
            x2Arr = x2arr;

            gridVals = gridValues;
        }

        public BilinearInterpolator() { }
        public BilinearInterpolator(Vector<double> x1arr, Vector<double> x2arr, NumericMatrix<double> gridValues)
        {
            Ini(x1arr, x2arr, gridValues);
        }

         // More constructor
        public BilinearInterpolator(double[] x1arr, double[] x2arr, List<double[]> gridValues)
        {
            int MinI = 0;
            Vector<double> x1 = new Vector<double>(x1arr, MinI);
            Vector<double> x2 = new Vector<double>(x2arr, MinI);
            NumericMatrix<double> matrix = new NumericMatrix<double>(x1.Length, x2.Length, x1.MinIndex, x2.MinIndex);
            for (int c = x2.MinIndex; c <= x2.MaxIndex; c++)
            {
                Vector<double> data = new Vector<double>(gridValues[c], MinI);
                for (int r = x1.MinIndex; r <= x1.MaxIndex; r++)
                {
                    matrix[r, c] = data[r];
                }
            }
            Ini(x1, x2, matrix);
        }


        public double Solve(double x, double y)
        {  // Find the interpolated values at a point (x1Var, x2Var)

            Pair<int> p = findAbscissa(x, y);

            int i = p.first;
            int j = p.second;

            //Console.WriteLine("Indices {0}, {1}", i, j);


             // See Wiki
            double Q11 = gridVals[i, j]; double Q22 = gridVals[i + 1, j + 1];
            double Q12 = gridVals[i, j + 1]; double Q21 = gridVals[i + 1, j];

            double x1 = x1Arr[i]; double x2 = x1Arr[i + 1];
            double y1 = x2Arr[j]; double y2 = x2Arr[j + 1];

            double factor = 1.0 / ((x2 - x1) * (y2 - y1));

            double val1 = (Q11 * (x2 - x) * (y2 - y) + Q21 * (x - x1) * (y2 - y) + Q12 * (x2 - x) * (y - y1) + Q22 * (x - x1) * (y - y1)) * factor;

            return val1;
        }

       
    }



