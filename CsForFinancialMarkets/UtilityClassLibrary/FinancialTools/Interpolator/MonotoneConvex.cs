// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// MonotoneConvex.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
// This is a simplified version of Monotone Convex interpolation, presented in Hagan-West 2008. We ported and adapted only some parts of VBA 
// code available at www.fimod.co.za to C#. See the www.fimod.co.za for a complete VBA version.
// A complete C++ implementation of Monotone Convex interpolation is available in QuantLib (www.quantlib.org)

 // In this version, we consider only the case 'dLambda == 0'
public class MonotoneConvex
{
     // Data members
    double[] f;                     
    double[] fdiscrete;             
    public double[] dTerms;         
    int i_N;                 
    public double[] dValues;        
    double[] dInterpolantatNode;    
    double x;                       
    double g0;                      
    double g1;                      
    double G;                       
    double eta;                     
    double A;                       

     // Constructor
    public MonotoneConvex(double[] t, double[] values, bool inputAreForwards, bool negativeForwardsAllowed) 
    {
         // To data members
        dTerms = t;
        dValues = values;            
        i_N = dValues.Count();
         // Calculate instantaneous forward rates
        fi_estimates(inputAreForwards, negativeForwardsAllowed);
    }

    public MonotoneConvex(double[] t, double[] values)
    {
        List<double> rl = new List<double>(values);
        rl.Insert(0, values[0]);

        List<double> tl = new List<double>(t);
        tl.Insert(0, 0);
        tl.Insert(0, 0);
         // To data members
        dTerms = tl.ToArray();
        dValues = rl.ToArray();
        i_N = dValues.Count();
         // Calculate instantaneous forward rates
        fi_estimates(false,false);
    }

    public void fi_estimates(bool bInputsareForwards, bool bNegative_Forwards_Allowed)
    {
        fdiscrete = new double[i_N]; 
        f = new double[i_N];
        dInterpolantatNode = new double[i_N];

         // extend the curve to time 0, for the purpose of calculating forward at time 1
        dTerms[0] = 0;
        dValues[0] = dValues[1];
         // step 1
        if (bInputsareForwards == false)
        {
            for (int j = 1; j < i_N; j++)
            {
                fdiscrete[j] = (dTerms[j + 1] * dValues[j] - dTerms[j] * dValues[j - 1]) / (dTerms[j + 1] - dTerms[j]);
                dInterpolantatNode[j] = dValues[j];
            }
        }
        else
        {
            double termrate = 0.0;
            for (int j = 0; j < i_N; j++)
            {
                fdiscrete[j] = dValues[j];
                termrate = termrate + fdiscrete[j] * (dTerms[j + 1] - dTerms[j]);
            }
        }
             //    'f_i estimation under the ameliorated method
             //    'numbering refers to Wilmott paper
             //    'step 2
             //    '(22)
            for (int j = 1; j < i_N - 1; j++)
            {
              f[j] = (dTerms[j + 1] - dTerms[j]) / (dTerms[j + 2] - dTerms[j]) * fdiscrete[j + 1]
                       + (dTerms[j + 2] - dTerms[j + 1]) / (dTerms[j + 2] - dTerms[j]) * fdiscrete[j];
            }
             //    '(23)
            f[0] = fdiscrete[1] - 0.5 * (f[1] - fdiscrete[1]);
             //    '(24)
            f[i_N - 1] = fdiscrete[i_N - 1] - 0.5 * (f[i_N - 2] - fdiscrete[i_N - 1]);
             //    'step 3
            if (bNegative_Forwards_Allowed == false)
            {
                f[0] = bound(0, f[0], 2 * fdiscrete[1]);
                for (int j = 1; j < i_N - 1; j++)
                {
                    f[j] = bound(0, f[j], 2 * Math.Min(fdiscrete[j], fdiscrete[j + 1]));
                }
                f[i_N - 1] = bound(0, f[i_N - 1], 2 * fdiscrete[i_N - 1]);
            }
    }

    public double Interpolant(double Term)
    {
        double output = 0.0;
                
        if (Term <= 0) { output = f[0]; }
        else if (Term > dTerms[i_N])
        {
            output = Interpolant(dTerms[i_N]) * dTerms[i_N] / Term + Forward(dTerms[i_N]) * (1.0 - dTerms[i_N] / Term);
        }
        else
        {
            int i = LastIndex(dTerms, Term);
             //  'the x in (25)
            x = (Term - dTerms[i + 1]) / (dTerms[i + 2] - dTerms[i + 1]);
            g0 = f[i] - fdiscrete[i + 1];
            g1 = f[i + 1] - fdiscrete[i + 1];

            if (x == 0 || x == 1) { G = 0; }
            else if ((g0 < 0 && -0.5 * g0 <= g1 && g1 <= -2 * g0) || (g0 > 0 && -0.5 * g0 >= g1 && g1 >= -2 * g0))
            {
                 // 'zone (i)
                G = g0 * (x - 2 * Math.Pow(x, 2) + Math.Pow(x, 3)) + g1 * (-Math.Pow(x, 2) + Math.Pow(x, 3));
            }

            else if ((g0 < 0 && g1 > -2 * g0) || (g0 > 0 && g1 < -2 * g0))
            {
                 //    'zone (ii)
                 //    '(29)
                eta = (g1 + 2 * g0) / (g1 - g0);
                 //    '(28)
                if (x <= eta)
                {
                    G = g0 * x;
                }
                else
                {
                    G = g0 * x + (g1 - g0) * Math.Pow(x - eta, 3) / Math.Pow(1 - eta, 2) / 3;
                }
            }
            else if ((g0 > 0 && 0 > g1 && g1 > -0.5 * g0) || (g0 < 0 && 0 < g1 && g1 < -0.5 * g0))
            {
                 //    'zone (iii)
                 //    '(31)
                eta = 3 * g1 / (g1 - g0);
                 //    '(30)
                if (x < eta)
                { // Then
                    G = g1 * x - 1.0 / 3.0 * (g0 - g1) * (Math.Pow(eta - x, 3) / Math.Pow(eta, 2) - eta);
                }
                else
                {     //    Else
                    G = (2.0 / 3.0 * g1 + 1.0 / 3.0 * g0) * eta + g1 * (x - eta);
                }
            }
            else if (g0 == 0 || g1 == 0) { G = 0; }
            else
            {
                 //    'zone (iv)
                 //    '(33)
                eta = g1 / (g1 + g0);
                 //    '(34)
                A = -g0 * g1 / (g0 + g1);
                 //    '(32)
                if (x <= eta)
                {
                    G = A * x - 1.0 / 3.0 * (g0 - A) * (Math.Pow(eta - x, 3) / Math.Pow(eta, 2) - eta);

                }
                else
                {
                    G = (2.0 / 3.0 * A + 1.0 / 3.0 * g0) * eta + A * (x - eta) + (g1 - A) / 3.0 * Math.Pow(x - eta, 3) / Math.Pow(1 - eta, 2);
                }
                 //  '(12)

            }
            output = 1 / Term * (dTerms[i + 1] * dInterpolantatNode[i] + (Term - dTerms[i + 1]) * fdiscrete[i + 1] + (dTerms[i + 2] - dTerms[i + 1]) * G);
         }
        return output;
    }
    
    public double Forward(double Term)
    {
        double output = 0.0;
         //          'numbering refers to Wilmott paper

        if (Term <= 0) { output = f[0]; }
        else if (Term > dTerms[i_N]) { output = Forward(dTerms[i_N]); }
        else
        {
            int i = LastIndex(dTerms, Term);
             // 'the x in (25)
            x = (Term - dTerms[i + 1]) / (dTerms[i + 2] - dTerms[i + 1]);
            g0 = f[i] - fdiscrete[i + 1];
            g1 = f[i + 1] - fdiscrete[i + 1];
            if (x == 0) { G = g0; }
            else if (x == 1) { G = g1; }
            else if ((g0 < 0 && -0.5 * g0 <= g1 && g1 <= -2 * g0) || (g0 > 0 && -0.5 * g0 >= g1 && g1 >= -2 * g0))
            {
                 //    'zone (i)
                G = g0 * (1 - 4 * x + 3 * Math.Pow(x, 2)) + g1 * (-2 * x + 3 * Math.Pow(x, 2));
            }
            else if ((g0 < 0 && g1 > -2 * g0) || (g0 > 0 && g1 < -2 * g0))
            {
                 //    'zone (ii)
                 //    '(29)
                eta = (g1 + 2 * g0) / (g1 - g0);
                 //    '(28)
                if (x <= eta) { G = g0; }
                else { G = g0 + (g1 - g0) * Math.Pow((x - eta) / (1 - eta), 2); }
            }
            else if ((g0 > 0 && 0 > g1 && g1 > -0.5 * g0) || (g0 < 0 && 0 < g1 && g1 < -0.5 * g0))
            {
                 //    'zone (iii)
                 //    '(31)
                eta = 3 * g1 / (g1 - g0);
                 //    '(30)
                if (x < eta) { G = g1 + (g0 - g1) * Math.Pow((eta - x) / eta, 2); }
                else { G = g1; }
            }
            else if (g0 == 0 || g1 == 0) { G = 0; }
            else
            {
                 //    'zone (iv)
                 //    '(33)
                eta = g1 / (g1 + g0);
                 //    '(34)
                A = -g0 * g1 / (g0 + g1);
                 //    '(32)
                if (x <= eta) { G = A + (g0 - A) * Math.Pow((eta - x) / eta, 2); }
                else { G = A + (g1 - A) * Math.Pow((eta - x) / (1 - eta), 2); }
            }
             //  '(26)
            output = G + fdiscrete[i + 1];
        }
        return output;
    }

    public double[] GetInterpolatedRate(double[] Terms)
    {
        int n = Terms.Length;
        double[] output = new double[n];
        for (int i = 0; i < n; i++) 
        {
            output[i] = Interpolant(Terms[i]);
        }
        return output;
    }

    public double[] GetInterpolatedFwd(double[] Terms)
    {
        int n = Terms.Length;
        double[] output = new double[n];
        for (int i = 0; i < n; i++)
        {
            output[i] = Forward(Terms[i]);
        }
        return output;
    }

    #region Utility
    public double bound(double Minimum, double Variable, double Maximum)
    {
        double output = 0.0;
        if (Variable < Minimum) { output = Minimum; }
        else if (Variable > Maximum) { output = Maximum; }
        else output = Variable;
        return output;
    }

    public int LastIndex(double[] Terms, double Term)
    {
        int M = Terms.Count() - 1;
        int shift = 0;
        if (Term < Terms[0]) return shift;
        if (Term >= Terms[M]) return M - 2;
        double greater = Array.Find(Terms, item => item >= Term);
        return Array.IndexOf(Terms, greater) - 2 + shift;

    }
    #endregion
}