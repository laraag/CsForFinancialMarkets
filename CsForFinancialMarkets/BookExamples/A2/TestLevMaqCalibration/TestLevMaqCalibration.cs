// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestLevMaqCalibration.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevMarTest
{
    public class LevMarTest
    {
        public static void Main()
        {
            LevMar lv = new LevMar();
            lv.Go("Linear");
            lv.Go("Cubic");
            lv.Go("PWC");
        }    
    } 

    // Delegate type declaration used for method AllFwd_Linear, AllFwd_Cubic or AllFwd_PWC
    delegate double[] CalcAllFwd(double[] knownRatesStart, double[] knownFwd);

    public class LevMar
    {
        double dt;  // tau i.e. 0.25
        double[] S;  // array of maturity of each forward rate F[T,S] (8 elements)
        double[] T_star; // array of starting time of each variable rate to find F*[T,S] (3 elements)
        double[] F_star; // array of forward rate to find  F* (3 elements)
        double[] DF_mktValue; // known DF to match (3 elements)
        double[] T; // array of starting time of each rate F[T,S] (8elements)
        string mode;  // "Linear","Cubic" or "PWC"
        CalcAllFwd FwdCalculator; // delegate for method AllFwd_Linear, AllFwd_Cubic or AllFwd_PWC

        public void Go(string Mode)
        {
            // Starting data
            F_star = Enumerable.Repeat(0.05, 3).ToArray<double>();
            dt = 0.25;
            S = new double[] { 0.25, 0.50, 0.75, 1.00, 1.25, 1.50, 1.75, 2.00 };
            T_star = new double[] { 0.0, 0.75, 1.75 };
            DF_mktValue = new double[] { 0.99, 0.95, 0.91 };
            T = new double[] { 0.0, 0.25, 0.50, 0.75, 1.00, 1.25, 1.50, 1.75 };
            mode = Mode;

            // Default is PWC, else Linear or Cubic
            FwdCalculator = AllFwd_PWC;
            if (mode == "Linear")
            {
                FwdCalculator = AllFwd_Linear;
            }
            else if (mode == "Cubic")
            {
                FwdCalculator = AllFwd_Cubic;
            }

            // Printing running mode (Linear, Cubic or PWC)
            Console.WriteLine("Running " + mode);
            Console.WriteLine();

            // Printing starting guess
            Console.WriteLine("Starting guess");
            for (int i = 0; i < F_star.Length; i++)
            {
                Console.WriteLine("S: {0}\t fwd: {1:p3}\t df: {2:f7}\t df*: {3:f3}\t diff: {4:f2} ", S[Array.IndexOf(T, T_star[i])], F_star[i], CalcPxDF(S[i], F_star), DF_mktValue[i], (CalcPxDF(S[i], F_star) - DF_mktValue[i]) * 1000000);
            }
            Console.WriteLine();

            // setting up the optimiser
            double epsg = 0.0000000001;
            double epsf = 0;
            double epsx = 0;
            int maxits = 0;
            alglib.minlmstate state;
            alglib.minlmreport rep;

            // Condition to match
            int NConstrains = 3;

            // see alglib documentation
            alglib.minlmcreatev(NConstrains, F_star, 0.000001, out state);
            alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
            alglib.minlmoptimize(state, function_fvec, null, null);
            alglib.minlmresults(state, out F_star, out rep);

            // recalculate the rate using optimized solution           
            double[] f = FwdCalculator(T_star, F_star);

            // Print forward rate
            Console.WriteLine("Solution");
            for (int i = 0; i < f.Length; i++)
            {
                Console.WriteLine("F[{0},{1}] : {2:p4} \t", T[i], S[i], f[i]);
            }

            Console.WriteLine();
            Console.WriteLine("Check on DF"); // print check on df
            for (int i = 0; i < F_star.Length; i++)
            {
                double S_star = S[Array.IndexOf(T, T_star[i])]; // target maturity of df
                double RecDf = CalcPxDF(S_star, f); // recalculated DF
                Console.WriteLine("S: {0}\t fwd: {1:p3}\t df: {2:f7}\t df*: {3:f3}\t diff: {4:f2} ", S_star, F_star[i], RecDf, DF_mktValue[i], (RecDf - DF_mktValue[i]) * 1000000);
            }
        }

        public void function_fvec(double[] x, double[] fi, object obj)
        {
            // delegate to calculate fwd
            double[] f = FwdCalculator(T_star, x);

            // minimise the difference
            fi[0] = 1000000 * (DF_mktValue[0] - CalcPxDF(0.25, f));
            fi[1] = 1000000 * (DF_mktValue[1] - CalcPxDF(1.00, f));
            fi[2] = 1000000 * (DF_mktValue[2] - CalcPxDF(2.00, f));
        }

        // Calculate DF for a given maturity and given an array of rates (fwds)
        public double CalcPxDF(double maturity, double[] fwds)
        {
            int n = Array.IndexOf(S, maturity);
            double df = 1.0;
            for (int i = 0; i <= n; i++)
            {
                df *= 1 / (1 + fwds[i] * dt);  // fwd rates are equidistant for construction 
            }
            return df;
        }

        // linear interpolated rate for each starting time in T
        public double[] AllFwd_Linear(double[] knownRatesStart, double[] knownFwd)
        {
            LinearInterpolator LI = new LinearInterpolator(knownRatesStart, knownFwd);
            return LI.Curve(T);  // getting linear interpolated data
        }

        // Calculate cubic interpolated rate for each starting time in T
        public double[] AllFwd_Cubic(double[] knownRatesStart, double[] knownFwd)
        {
            SimpleCubicInterpolator CU = new SimpleCubicInterpolator(knownRatesStart, knownFwd);
            return CU.Curve(T);  // getting Cubic interpolated data
        }

        // Piecewise constant rate for each starting time in T
        public double[] AllFwd_PWC(double[] knownRatesStart, double[] knownFwd)
        {
            double[] fwd = new double[T.GetLength(0)];
            fwd[0] = knownFwd[0];
            fwd[1] = knownFwd[1];
            fwd[2] = knownFwd[1];
            fwd[3] = knownFwd[1];
            fwd[4] = knownFwd[2];
            fwd[5] = knownFwd[2];
            fwd[6] = knownFwd[2];
            fwd[7] = knownFwd[2];
            return fwd;  // piecewise constant data
        }
    }
}