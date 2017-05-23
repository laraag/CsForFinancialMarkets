 // InterestRateCalculator.cs
 // 
 // Simple functions for interest rate calculations, based on Fabozzi, Chapter 2.
 // 
 // Last Modification dates:
 // 
 // 2006-2-21 DD Kick-off
 // 2006-2-22 DD Small bug removed
 // 2009-9-25 DD port to C#
 // 
 // (C) Datasim Education BV 2006-2009
 // 

using System;

public class InterestRateCalculator
{
        // Member variables
    private double r;        // Interest rate
    private int nPeriods;    // Number of periods

        // Constructor
    public InterestRateCalculator(int numberPeriods, double interest)
    {
        nPeriods = numberPeriods;
        r = interest;
    }

        // Future value of a sum of money invested today
    public double FutureValue(double P0)
    {
        double factor = 1.0 + r;
        return P0 * Math.Pow(factor, nPeriods);
    }

        // Future value of a sum of money invested today, m periods 
        // per year. r is annual interest rate
    public double FutureValue(double P0, int m)
    {
        double R = r / m;
        int newPeriods = m * nPeriods;

            // We create a temporary object to do the job
        InterestRateCalculator myBond = new InterestRateCalculator(newPeriods, R);
        return myBond.FutureValue(P0);
    }

        // Continuous compounding
    public double FutureValueContinuous(double P0)
    {

        double growthFactor = Math.Exp(r * nPeriods);
        return P0 * growthFactor;
    }

        // Future value of an ordinary annuity
    public double OrdinaryAnnuity(double A)        {

        double factor = 1.0 + r;
        return A * ((Math.Pow(factor, nPeriods) - 1.0) / r);
    }

        // Present Value
    public double PresentValue(double Pn)        {

        double factor = 1.0 + r;
        return Pn * (1.0 / Math.Pow(factor, nPeriods));
    }

        // Present Value of a series of future values
    public double PresentValue(double[] prices)        {

        double factor = 1.0 + r;
        double PV = 0.0;
        for (int t = 0; t < nPeriods; t++)
        {
            PV += prices[t] / Math.Pow(factor, t + 1);
        }
        return PV;
    }

        // Present Value of a series of future values with constant coupon
    public double PresentValueConstant(double Coupon)
    {

        double factor = 1.0 + r;
        double PV = 0.0;
        for (int t = 0; t < nPeriods; t++)
        {
            PV += 1.0 / Math.Pow(factor, t + 1);
        }
        return PV * Coupon;
    }

        // Present Value of an ordinary annuity
    public double PresentValueOrdinaryAnnuity(double A)
    {
        double factor = 1.0 + r;
        double numerator = 1.0 - (1.0 / Math.Pow(factor, nPeriods));
        return (A * numerator) / r;
    }

        // Set and get read-write properties
    public int NumberOfPeriods
    {
        get { return nPeriods; }
        set { nPeriods = value; }
    }

    public double Interest
    {
        get { return r; }
        set { r = value; }
    }
}
