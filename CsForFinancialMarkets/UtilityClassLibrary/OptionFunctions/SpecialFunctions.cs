// SpecialFunctions.cs
//
// Class containing special functions that we need in other code.
//
// (C) Datasim Education BV 2010 
//

using System;

public class SpecialFunctions
{

    //////////// Gaussian functions /////////////////////////////////

    static public double n(double x)
    {

        double A = 1.0 / Math.Sqrt(2.0 * 3.1415);
        return A * Math.Exp(-x * x * 0.5); // Math class in C#

    }

    static public double N(double x)
    { // The approximation to the cumulative normal distribution


        double a1 = 0.4361836;
        double a2 = -0.1201676;
        double a3 = 0.9372980;

        double k = 1.0 / (1.0 + (0.33267 * x));

        if (x >= 0.0)
        {
            return 1.0 - n(x) * (a1 * k + (a2 * k * k) + (a3 * k * k * k));
        }
        else
        {
            return 1.0 - N(-x);
        }
    }
}
