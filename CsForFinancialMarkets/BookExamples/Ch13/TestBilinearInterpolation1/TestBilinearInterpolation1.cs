// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestBilinearInterpolation1.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Based on Datasim interpolators
class TestBilinearInterpolation1
{
    static void Main()
    {
        BilinearInterpolation1();
    }

    // 13.12.4	Bilinear Interpolation
    public static void BilinearInterpolation1()
    {
        Console.WriteLine("Vectors initialised: ");

        // Create mesh arrays
        int startIndex = 0;

        int N = 1;
        Vector<double> x1arr = new Vector<double>(N + 1, startIndex, 0.0);
        x1arr[0] = 20.0; x1arr[1] = 21.0;

        Vector<double> x2arr = new Vector<double>(x1arr);
        x2arr[0] = 14.0; x2arr[1] = 15.0;

        Console.WriteLine(x1arr);
        Console.WriteLine(x2arr);

        // Control values; 
        NumericMatrix<double> Control
                = new NumericMatrix<double>(N + 1, N + 1, startIndex, startIndex);
        Control[0, 0] = 91.0; Control[1, 1] = 95.0;
        Control[0, 1] = 210.0; Control[1, 0] = 162.0;

        BilinearInterpolator myInterpolator
                = new BilinearInterpolator(x1arr, x2arr, Control);

        double x = 20.2; double y = 14.5;  // 146.1

        double value = myInterpolator.Solve(x, y);
        Console.WriteLine("Interpolated value: {0}", value);
    }
}

