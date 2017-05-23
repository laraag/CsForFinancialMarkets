// ShortRateModels.cs
//
// Classes for one-factor short-rate models such as Vasicek and
// Cox-Ingersoll-Ross (CIR).
//
// (C) Datasim Education BV 2010-2013
//

using System;

public interface IBondModel
{ // Common methods for pure discount bonds PDB

    // Price at time t of a PDB of maturity s
    double P(double t, double s);

    // Yield == spot rate at time t of PDB of maturity s
    double R(double t, double s);

    // Volatility of yield R(t,s)
    double YieldVolatility(double t, double s);
}


public abstract class BondModel : IBondModel
{ // Base class for all pure discount bonds

   // public double maturity;    // maturity of PDB

    // SDE data; in general this would be an SDE object
    public double kappa;    // Speed of mean reversion
    public double theta;    // Long-term level
    public double vol;      // Volatility of short rate
    public double r;        // Flat term structure             

    // Interface IBondModel
    public abstract double P(double t, double s);
    public abstract double R(double t, double s);
    public abstract double YieldVolatility(double t, double s);

    public BondModel(double kappa, double theta, double vol, double r)
    {
        this.kappa = kappa;
        this.theta = theta;
        this.vol = vol;
        this.r = r;
    }

    // Accept visitor.
    public abstract void Accept(BondVisitor visitor);
}

public class VasicekModel : BondModel
{
    // Some redundant data
    private double longTermYield;
    
    // Terms in A(t,s) * exp(-r*B(t,s))
    private double A(double t, double s)
    {
        double R = longTermYield;
        double smt = s - t;
        double exp = 1.0 - Math.Exp(-kappa*smt);

        double result = R*exp/kappa - smt*R - 0.25*vol*vol*exp*exp/(kappa*kappa*kappa);
 
        return Math.Exp(result);
    }
    
    private double B(double t, double s)
    {
        return (1.0 - Math.Exp(-kappa*(s-t)))/kappa;
    }

    public VasicekModel(double kappa, double theta, double vol, double r) : base(kappa, theta, vol, r) 
    {
        longTermYield = theta - 0.5*vol*vol/(kappa*kappa); 
    }

    public override double P(double t, double s)
    {
        return A(t,s) * Math.Exp(-r*B(t,s));
    }

    public override double R(double t, double s)
    {
       return (-Math.Log(A(t,s)) + B(t,s)*r) / (s-t);
    }

    public override double YieldVolatility(double t, double s)
    {
        return vol * (1.0 - Math.Exp(-kappa*(s-t)))/(kappa*(s-t));
    }

    // Accept visitor.
    public override void Accept(BondVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class CIRModel : BondModel
{
    // Work variables
    private double phi1, phi2, phi3;

    // Terms in A(t,s) * exp(-r*B(t,s))
    public double A(double t, double s)
    {

        double tmp1 = Math.Exp(phi1 * (s - t)); double tmp2 = Math.Exp(phi2 * (s - t));

        double val = phi1 * tmp2 / (phi2 * (tmp1 - 1.0) + phi1);

        return Math.Pow(val, phi3);
       
    }

    public double B(double t, double s)
    {
        double tmp1 = Math.Exp(phi1 * (s - t));

        return (tmp1 - 1.0) / (phi2 * (tmp1 - 1.0) + phi1);

    }

    public CIRModel(double kappa, double theta, double vol, double r)
        : base(kappa, theta, vol, r)
    {
        // Calculate the work variables
        phi1 = Math.Sqrt(kappa * kappa + 2.0 * vol * vol);
        phi2 = 0.5 * (kappa + phi1);
        phi3 = 2.0 * kappa * theta / (vol * vol);
   }

    public override double P(double t, double s)
    {
        return A(t, s) * Math.Exp(-r * B(t, s));
    }

    public override double R(double t, double s)
    {
        return (-Math.Log(A(t, s)) + B(t, s) * r) / (s - t);
    }

    public override double YieldVolatility(double t, double s)
    {
        return vol * (1.0 - Math.Exp(-kappa * (s - t))) / (kappa * (s - t));
    }

    // Accept visitor.
    public override void Accept(BondVisitor visitor)
    {
        visitor.Visit(this); 

        // Non-central chi^2 or FDM
    }
}
