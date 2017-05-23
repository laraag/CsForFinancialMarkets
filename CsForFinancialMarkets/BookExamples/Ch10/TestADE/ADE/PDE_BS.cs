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
    char Otype;

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
    public Pde_BS(Option myOption)
    {
        T = myOption.ExpiryDate;
        K = myOption.StrikePrice;
        vol = myOption.Volatility;
        r = myOption.InterestRate;
        D = myOption.CostOfCarry; // Change!
        Smax = myOption.FarFieldCondition;
        Otype = myOption.Otype;
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
        if (Otype == 'P')
        {
            return K * Math.Exp(-r * t);
        }

        return 0.0;
    }

    public double BCR(double t)
    {
        if (Otype == 'C')
        {
            return Smax - K; // Magic number
        }
        else
        {
            return 0.0; //P
        }
    }

    public double IC(double x)
    {
        // Put: max(0, K - x)
        if (Otype == 'P')
        {
            if (x < K)
                return K - x;
            else
            {
                return 0.0;
            }
        }
        else
        {
            if (x > K)
                return x - K;
            else
            {
                return 0.0;
            }
        }

     }

    public double Constraint(double x)
    {
         return K - x;
    }
}