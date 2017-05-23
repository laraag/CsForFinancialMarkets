// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// NumMethod.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * class NumMethod will implement some static method for finding root
 *  NewRap = Newton - Raphson
 *  NewRapNum = Newton - Raphson numeric with no need of derivative to be specified
 *  BisMet = bisection method
 *  Reference:  "Option Pricing Models & Volatility"  Fabrice Douglas Rouah and Gregory Vainberg
 */

public class NumMethod
    {   
         // Declares a delegate for a method that takes in an int and returns a String.
        public delegate double myMethodDelegate(double x);
      
         // Based on Option Pricing Models and Volatility using Excel-VBA page 8
        public static double NewRap(myMethodDelegate fname, myMethodDelegate dfname, double x_guess)
        {
            int Maxiter = 500;
            double Eps = 0.00001;
            double cur_x = x_guess;

            double fx, dx;
            for (int i = 1; i <= Maxiter; i++)
            {
                fx = fname(cur_x);
                dx = dfname(cur_x);
                if (Math.Abs(dx) < Eps) i = Maxiter + 1;
                cur_x = cur_x - (fx / dx);
            }
            return cur_x;
        }

         // Based on Option Pricing Models and Volatility using Excel-VBA page 9
        public static double NewRapNum(myMethodDelegate fname, double x_guess)
        {
            int Maxiter = 500;
            double Eps = 0.000001;
            double delta_x = 0.000000001; 
            double cur_x = x_guess;

            double fx, dx, fx_delta_x;
            for (int i = 1; i <= Maxiter; i++)
            {
                fx = fname(cur_x);
                fx_delta_x = fname(cur_x - delta_x);
                dx = (fx - fx_delta_x) / delta_x;
                if (Math.Abs(dx) < Eps) i = Maxiter + 1;
                cur_x = cur_x - (fx / dx);
            }            
            return cur_x;
        }

         //Based on Option Pricing Models and Volatility using Excel-VBA page 9
        public static double BisMet(myMethodDelegate fname, double a, double b)
        {
            double Eps = 0.000001;
            double midPt;
            double _a = a;
            double _b = b;

            if (fname(_b) < fname(_a))
            {
                _a = b; _b = a;
            }
            while (fname(_b) - fname(_a) > Eps)
            {
                midPt = (_b + _a) / 2;
                if (fname(midPt) < 0)
                {
                    _a = midPt;
                }
                else
                { _b = midPt; }
            }
            return (_b + _a) / 2;
        }
    }
