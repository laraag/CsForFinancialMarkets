// BondVisitor.cs
//
// Abstract base class for Bond visitors.
// The visitor can be used to extend the functionality of
// the Bond hierarchy such as output drivers, transformation drivers, etc.
// Can be used in combination with Composites.
//
// (C) Datasim Education BV 2001-2013

using System;

public enum OptionType : uint
{
    Call = 1,
    Put = 2,
}

public abstract class BondVisitor
{
    // Default constructor
	public BondVisitor()
	{
	}

	// Copy constructor
	public BondVisitor(BondVisitor source)
	{
	}

	// Visit Vasicek.
    public abstract void Visit(VasicekModel model);
    public abstract void Visit(CIRModel model);
}

public class OptionPricer : BondVisitor
{
    // Bond and option times
    private double t;       // Present time
    private double T;       // Option expiration date
    private double s;       // Bond maturity
    double K;               // Strike price

    // Type of option using an enum
    OptionType type;        // Call or put

    // Computed value
    public double price;

    private double CallPrice(VasicekModel model)
    {
        double nu = 0.5*model.vol*model.vol*(1.0 - Math.Exp(-2.0*model.kappa*(T-t)))/model.kappa;
        nu = Math.Sqrt(nu);

        double sigP = nu*(1.0 - Math.Exp(-model.kappa*(s-T)))/model.kappa;
 
        double d1 = (Math.Log(model.P(t,s) /(model.P(t,T)*K))/ sigP) + 0.5*sigP;
        double d2 = d1 - sigP;
      /*  Console.WriteLine(" d1, d2 {0}, {1}", d1, d2);
        Console.WriteLine("{0}, {1}", model.kappa, model.vol);
        Console.WriteLine("{0}, {1}", nu, sigP);*/
        return model.P(t,s)*SpecialFunctions.N(d1) - K * model.P(t,T)*SpecialFunctions.N(d2);

    }

    private double PutPrice(VasicekModel model)
    {
        double nu = 0.5 * model.vol * model.vol * (1.0 - Math.Exp(-2.0 * model.kappa * (T - t))) / model.kappa;
        nu = Math.Sqrt(nu);

        double sigP = nu * (1.0 - Math.Exp(-model.kappa * (s - T))) / model.kappa;

        double d1 = (Math.Log(model.P(t, s) / (model.P(t, T) * K)) / sigP) + 0.5 * sigP;
        double d2 = d1 - sigP;
      /*  Console.WriteLine(" d1, d2 {0}, {1}", d1, d2);
        Console.WriteLine("{0}, {1}", model.kappa, model.vol);
        Console.WriteLine("{0}, {1}", nu, sigP);*/
        return K*model.P(t, T) * SpecialFunctions.N(-d2) - model.P(t, s) * SpecialFunctions.N(-d1);

    }

    public OptionPricer(double t, double T, double s, double K, OptionType type)
    {
        this.t = t;
        this.T = T;
        this.s = s;
        this.type = type;
        this.K = K;
    }

    public override void Visit(VasicekModel model)
    { // Price a put or call using Jamshidian (1989)

        if (type == OptionType.Call)
        {
            price = CallPrice(model);   // implements Jamshidian 1989
        }
        else
        {
            price = PutPrice(model);    // implements Jamshidian 1989
        }
    }

    public override void Visit(CIRModel model)
    {

    /*    double df = 4.0 * model.kappa * model.theta / (model.vol * model.vol);

        // Set up the NC parameter
        double thet = Math.Sqrt(model.kappa * model.kappa + 2.0 * model.vol * model.vol);
        double phi = 2.0*thet/(model.vol*model.vol*(Math.Exp(thet*(T-t)) - 1.0));
        double epsci = (model.kappa + thet)/(model.vol*model.vol);

        double rStar = Math.Log(model.A(T,s)/K) / model.B(T,s);
        Console.WriteLine(" A, B1, B2 {0}, {1}, {2}", model.A(T, s), model.B(t, s), model.B(T, s));
        double ncParam = 2.0*phi*phi*model.r*Math.Exp(thet*(T-t)) / (phi + epsci + model.B(T,s));

        Console.WriteLine(" thet, phi, epsci, r* {0}, {1}, {2}, {3}", thet, phi, epsci, rStar);
        NonCentralChiSquaredDistribution dis = new NonCentralChiSquaredDistribution(df, ncParam);

        double x = 2*rStar*(phi + epsci + model.B(T,s));

        Console.WriteLine(" df, param, x {0}, {1}, {2}", df, ncParam, x);

        price = model.P(t,s)*dis.Cdf(x);

        double ncParam2 = 2.0 * phi * phi * model.r * Math.Exp(thet * (T - t)) / (phi + epsci);
        double x2 = 2 * rStar * (phi + epsci);
        Console.WriteLine(" x2, param2 {0}, {1}", x2, ncParam2);
        NonCentralChiSquaredDistribution dis2 = new NonCentralChiSquaredDistribution(df, ncParam2);

        price = model.P(t, s) * dis.Cdf(x) - K * model.P(t, T) * dis2.Cdf(x2);

        Console.WriteLine(" P(t,s), P(t,T) {0}, {1}", model.P(t, s), model.P(t, T));*/
    }


}
