// PDE_BS.cs
//
// Hard-coded class for the Black Scholes equation. Put options.
//
// (C) Datasim Education BC 2013
//

using System;

public class Pde_BS : IBSPde
{
    private double T, K, vol, r, D, Smax;

    public Pde_BS(double expiry, double strike, double volatility, 
                    double interest, double dividend, double truncation)
    {
        T = expiry;
        K = strike;
        vol = volatility;
        r = interest;
        D = dividend;
        Smax = truncation;
    }

    public double sigma(double x, double t)
    {
 
        double sigmaS = vol*vol;
        return 0.5 * sigmaS * x * x;
    }

    public double mu(double x, double t)
    {
        return (r - D) * x;
    }

    public double b(double x, double t)
    {
        return -r;
    }

    public double f(double x, double t)
    {
        return 0.0;
    }

    public double BCL(double t)
    {
         // Put option
         return K * Math.Exp(-r * (T - t));
    }

    public double BCR(double t)
    {
        return 0.0; // P
    }

    public double IC(double x)
    {
        // Put: max(0, K - x)
        if (x < K)
            return K - x;

        return 0.0;
    }

    public double Constraint(double x)
    {
         return K - x;
    }
}