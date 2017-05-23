// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestBilinearInterpolation2.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Based on Datasim interpolators
class TestBilinearInterpolation2
{
    static void Main(string[] args)
    {
        BilinearInterpolation2();
    }

    // 13.12.4	Bilinear Interpolation
    public static void BilinearInterpolation2()
    {
        // Create mesh arrays
        int startIndex = 0;

        // Number of subdivisions N,M in the x and y directions
        int N = 4;
        int M = 3;
        Vector<double> x1arr = new Vector<double>(N + 1, startIndex, 0.0);

        double a = 0.0; double b = 1.0;

        double h1 = (b - a) / (double)N;
        x1arr[x1arr.MinIndex] = a;

        for (int j = x1arr.MinIndex + 1; j <= x1arr.MaxIndex; j++)
        {
            x1arr[j] = x1arr[j - 1] + h1;
        }

        Vector<double> x2arr = new Vector<double>(M + 1, startIndex, 0.0);
        double h2 = (b - a) / (double)M;
        x1arr[x1arr.MinIndex] = a;

        for (int j = x2arr.MinIndex + 1; j <= x2arr.MaxIndex; j++)
        {
            x2arr[j] = x2arr[j - 1] + h2;
        }
        Console.WriteLine(x1arr);
        Console.WriteLine(x2arr);

        // Control values; 
        NumericMatrix<double> Control
                        = new NumericMatrix<double>(N + 1, M + 1,
                        startIndex, startIndex);

        Func<double, double, double> func = (double x1, double x2) => x1 + x2;

        for (int i = Control.MinRowIndex; i <= Control.MaxRowIndex; i++)
        {
            for (int j = Control.MinColumnIndex; j <= Control.MaxColumnIndex; j++)
            {
                Control[i, j] = func(x1arr[i], x2arr[j]);
            }
        }

        BilinearInterpolator myInterpolator
                = new BilinearInterpolator(x1arr, x2arr, Control);

        double x = 0.1; double y = 0.7;
        double value = myInterpolator.Solve(x, y);
        Console.WriteLine("Interpolated value: {0}", value);

        // Take center point (xm, ym) of each element and interpolate on it
        NumericMatrix<double> InterpolatedMatrix
                = new NumericMatrix<double>(N, M, startIndex, startIndex);

        // Abscissa points of new interpolated matrix
        Vector<double> Xarr
                = new Vector<double>(InterpolatedMatrix.Rows, startIndex, 0.0);
        Vector<double> Yarr
                = new Vector<double>(InterpolatedMatrix.Columns, startIndex, 0.0);

        for (int i = InterpolatedMatrix.MinRowIndex;
             i <= InterpolatedMatrix.MaxRowIndex; i++)
        {
            for (int j = InterpolatedMatrix.MinColumnIndex;
                 j <= InterpolatedMatrix.MaxColumnIndex; j++)
            {
                Xarr[i] = 0.5 * (x1arr[i] + x1arr[i + 1]);     // xm   
                Yarr[j] = 0.5 * (x2arr[j] + x2arr[j + 1]);   // ym

                InterpolatedMatrix[i, j]
                        = myInterpolator.Solve(Xarr[i], Yarr[j]);
            }
        }

        // Present the interpolated matrix in Excel
        ExcelMechanisms driver = new ExcelMechanisms();

        string title = "Interpolated matrix";
        driver.printMatrixInExcel<double>(InterpolatedMatrix,
                                Xarr, Yarr, title);
    }
}

