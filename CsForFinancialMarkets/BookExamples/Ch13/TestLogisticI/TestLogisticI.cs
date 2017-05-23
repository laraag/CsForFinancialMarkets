// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestLogisticI.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Based on Datasim interpolators
class TestLogisticI
{
    static void Main(string[] args)
    {
        LogisticI();
    }

    public static void LogisticI()
    {
        Console.WriteLine("Vectors initialised: ");

        int N = 280;
        double a = -15.0;		 // Left of interval
        double b = 15.0;		 // Right of interval
        double h = (b - a) / N;
        int startIndex = 0;

        Vector<double> xarr = new Vector<double>(N + 1, startIndex, 0.0);
        Vector<double> yarr = new Vector<double>(N + 1, startIndex, 0.0);

        for (int j = xarr.MinIndex; j <= xarr.MaxIndex; j++)
        {
            xarr[j] = a + h * j;
            yarr[j] = Potpourri.Sigmoid1(xarr[j]);
            // yarr[j] = Potpourri.Sigmoid2(xarr[j]);
        }

        Console.WriteLine("Vectors initialised: ");

        int FirstDeriv = 1;

        CubicSplineInterpolator csi
                    = new CubicSplineInterpolator(xarr, yarr, FirstDeriv, 10, 10);

        // Display arrays in Excel
        ExcelMechanisms exl = new ExcelMechanisms();
        exl.printOneExcel<double>(xarr, csi.Curve(), "Logistic I", "x", "value", "I");

        // Now choose 1st order derivative at zero
        double leftBC = 1.0;
        double rightBC = -1.0;
        CubicSplineInterpolator csi2 = new CubicSplineInterpolator(xarr, yarr, FirstDeriv, leftBC, rightBC);

        // Display arrays in Excel
        exl.printOneExcel(xarr, csi2.Curve(), "Logistic II", "x", "value", "II");
    }

    public class Potpourri
    {
        public static double func(double x)
        {
            return x * x;
        }

        public static double NormalPdf(double x)
        {  // Probability function for Gauss fn.

            double A = 1.0 / Math.Sqrt(2.0 * 3.1415);
            return A * Math.Exp(-x * x * 0.5);
        }

        public static double Sigmoid1(double t)
        {
            return 1.0 / (1.0 + Math.Exp(-t));
        }

        public static double Sigmoid2(double t)
        {

            double a = 0.6;
            double b = 0.15;
            double c = 10.0;
            double d = -0.3;

            return a + (b - a) / (1.0 + Math.Exp(-c * (Math.Log(t) - d)));
        }

        public static double NormalCdf(double x)
        {  // The approximation to the cumulative normal distribution

            double a1 = 0.4361836;
            double a2 = -0.1201676;
            double a3 = 0.9372980;

            double k = 1.0 / (1.0 + (0.33267 * x));

            if (x >= 0.0)
            {
                return 1.0 - NormalPdf(x) *
                    (a1 * k + (a2 * k * k) + (a3 * k * k * k));
            }
            else
            {
                return 1.0 - NormalCdf(-x);
            }
        }

        public static double func(double x, double y)
        {
            return Math.Exp(-x * x - y * y) + 1.0;
        }

        public static double BinormalPdf(double x, double y, double rho)
        {  // Probability function for Gauss fn.

            double A = 1.0 / (2.0 * 3.1415) * Math.Sqrt(1.0 - rho * rho);
            double a = 1.0 / (2.0 * (1.0 - rho * rho));

            return A * Math.Exp(-a * (x * x - 2.0 * rho * x * y + y * y));
        }
    }
}

