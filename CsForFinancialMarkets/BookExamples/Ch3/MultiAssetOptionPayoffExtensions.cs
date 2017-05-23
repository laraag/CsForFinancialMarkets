// MultiAssetOptionPayoffExtensions.cs
//
// Implements C# 'Extension Methods' mechanism for non-intrusive
// addition of new methods to an existing class.
//
// (C) Datasim Education BV 2010-2013
//

using System;

namespace MultiAssetOptionPayoffExtensions
{

    public static class MultiAssetOptionPayoffMixins
    { // Define new methods for class Option here


        public static NumericMatrix<double> PayoffMatrix(this ITwoFactorPayoff payoffStrategy, Range<double> r1, Range<double> r2, int N1, int N2)
        { // Compute the discrete payoff matrix, in the closed range r1 X r2

            NumericMatrix<double> result = new NumericMatrix<double>(N1 + 1, N2 + 1);

            // Create the mesh point in the x and directions
            Vector<double> xarr = r1.mesh(N1);
            Vector<double> yarr = r2.mesh(N2);

            for (int i = result.MinRowIndex; i <= result.MaxRowIndex; i++)
            {
                for (int j = result.MinColumnIndex; j <= result.MaxColumnIndex; j++)
                {
                    result[i, j] = payoffStrategy.payoff(xarr[i], yarr[j]);
                }
            }

            // Create the mesh point in the x and directions

            return result;
     
        }

        public static void DisplayInExcel(this ITwoFactorPayoff payoffStrategy, Range<double> r1, Range<double> r2, int N1, int N2)
        { // Display the discrete payoff matrix in Excel

            NumericMatrix<double> matrix = PayoffMatrix(payoffStrategy, r1, r2, N1, N2);

            Vector<double> xarr = r1.mesh(N1);
            Vector<double> yarr = r2.mesh(N2);

           
            // Display in Excel
            // public void printMatrixInExcel<T>( NumericMatrix<T> matrix, Vector<T> xarr, Vector<T> yarr, string SheetName )

            ExcelMechanisms driver = new ExcelMechanisms();

            string title = "Payoff function";
            driver.printMatrixInExcel<double>(matrix, xarr, yarr, title);

        }
    }
}
