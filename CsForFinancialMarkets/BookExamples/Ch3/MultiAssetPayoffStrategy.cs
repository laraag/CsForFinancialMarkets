// MultiAssetPayoffStrategy.hpp
//
// Lightweight public classes for payoff functions. You can 
// add your own payoff public classes here.
//
// 2005-7-16 DD Multi-asset (n = 2) case.
// 2005-12-20 DD Review + corresponding Factory.
// 2006-1-20 DD to reader are formulae OK?
// 2010-7-31 DD adapted for C#
// 2011-8-6 portfoiio option
//
// (C) Datasim Education BV 1998 - 2013
//

// Help functions; in new versions put them in a separate assembly
public class Comparisons
{
    public static double Max(double d1, double d2)
    {
        if (d1 > d2)
            return d1;
        return d2;
    }

    public static double Min(double d1, double d2)
    {
        if (d1 < d2)
            return d1;
        return d2;
    }
    public static double Max(double d1, double d2, double d3)
    {
       return Max(Max(d1,d2), d3);
    }

    public static double Min(double d1, double d2, double d3)
    {
        return Min(Min(d1, d2), d3);
  
    }

}


public interface ITwoFactorPayoff
{ // The unambiguous specification of two-factor payoff contracts

    // This interfaces can be specialised to various underlyings
    double payoff(double factorI1, double factorII);
}


public abstract class MultiAssetPayoffStrategy: ITwoFactorPayoff
{

		public abstract double payoff(double S1, double S2);
}


public class ExchangeStrategy : MultiAssetPayoffStrategy
{

		// No member data

		public ExchangeStrategy() { }
		public override double payoff(double S1, double S2) 
		{
			return Comparisons.Max(S1-S2, 0.0); // a1S1 - a2S2 in general
		}
}

public class RainbowStrategy : MultiAssetPayoffStrategy
{

		private double K;	// Strike
		private int w;	    // +1 call, -1 put
		private int type;	// Comparisons.Max (1) or Comparisons.Min (!1) of 2 assets

        public RainbowStrategy(int cp, double strike, int DMinDMax)
		{ K = strike; w = cp; type = DMinDMax; }

		public override double payoff(double S1, double S2) 
		{
			if (type == 1)	// Comparisons.Max
				return Comparisons.Max ( w * Comparisons.Max(S1,S2) - w*K, 0.0);

			return Comparisons.Max( w * Comparisons.Min(S1,S2) - w*K, 0.0);
		}
}

public class BasketStrategy : MultiAssetPayoffStrategy
{ // 2-asset basket option payoff

		private double K;		// Strike
		private int w;	    	// +1 call, -1 put
		private double w1, w2;	// w1 + w2 = 1

		// All public classes need default ructor
		public BasketStrategy()
		{ K = 95.0; w = +1; w1 = 0.5; w2 = 0.5;
		}

		public BasketStrategy(double strike, int cp,double weight1, double weight2)
		{ K = strike; w = cp; w1 = weight1; w2 = weight2;
		}
		public override double payoff(double S1, double S2) 
		{
			double sum = w1*S1 + w2*S2;
            return Comparisons.Max(w* (sum - K), 0.0);
		}
}

public class BestWorstStrategy : MultiAssetPayoffStrategy
{

		private double K;		// Strike
		private double w;		// +1 Best, -1 Worst
		

		public BestWorstStrategy(double cash, double BestWorst)
		{ K = cash; w = BestWorst;}

		public override double payoff(double S1, double S2) 
		{
	
			if (w == 1)	// Best
				return Comparisons.Max (S1, S2, K);

			return Comparisons.Min (S1, S2, K);
		}
}

public class QuotientStrategy : MultiAssetPayoffStrategy
{

		private double K;	// Strike
		private int w;	    // +1 call, -1 put
		

		public QuotientStrategy(double strike, int cp)
		{ K = strike; w = cp; }
		public override double payoff(double S1, double S2) 
		{
			return Comparisons.Max ( w * (S1/S2) - w*K, 0.0);

		}
}

public class QuantoStrategy : MultiAssetPayoffStrategy
{

		private double Kf;	// Strike in foreign currency
		private double fer;	// Fixed exchange rate
		private int w;  	// +1 call, -1 put
		

		public QuantoStrategy(double foreignStrike, int cp, double forExchangeRate)
		{ Kf = foreignStrike; w = cp; fer = forExchangeRate; }

		public override double payoff(double S1, double S2) 
		{
			return fer * Comparisons.Max ( w * S1 - w*Kf, 0.0);

		}
}

public class SpreadStrategy : MultiAssetPayoffStrategy
{

		private double K;		// Strike
		private int w;		    // +1 call, -1 put
		private double a, b;	// a > 0, b < 0

		public SpreadStrategy(int cp)
		{ K = 0.0; w = cp; a = 1.0; b = -1.0;}
        
        public SpreadStrategy(int cp, double strike, double A, double B)
		{ K = strike; w = cp; a = A; b = B;}

        public override double payoff(double S1, double S2) 
		{
			double sum = a*S1 + b*S2;

			return Comparisons.Max(w* (sum - K), 0.0);
		}
}

public class DualStrikeStrategy : MultiAssetPayoffStrategy
{

		private double K1, K2;
		private int w1, w2;         // calls or puts

		public DualStrikeStrategy(double strike1, double strike2, int cp1, int cp2)
		{ K1 = strike1; K2 = strike2;
		  w1 = cp1; w2 = cp2;
		}

		public override double payoff(double S1, double S2) 
		{
			return Comparisons.Max (w1*(S1-K1), w2*(S2-K2), 0.0);
		}
}

public class OutPerformanceStrategy : MultiAssetPayoffStrategy
{

		private double I1, I2;	// Values of underlyings at maturity
		private int w;		// Call +1 or put -1
		private double K;		// Strike rate of option


		public OutPerformanceStrategy(double currentRate1, double currentRate2, int cp,
								double strikeRate)
		{ I1 = currentRate1; I2 = currentRate2;
		  w = cp; K = strikeRate;
		}

		public override double payoff(double S1, double S2) 
		{

			return Comparisons.Max (w*((I1/S1) - (I2/S2)) - w* K, 0.0);

		}
}

public class BestofTwoStrategy : MultiAssetPayoffStrategy
{ // Best of 2 options

    private double K;		// Strike
    private int w;	    	// +1 call, -1 put
  

    // All public classes need default ructor
    public BestofTwoStrategy()
    {
        K = 95.0; w = +1; 
    }

    public BestofTwoStrategy(double strike, int cp)
    {
        K = strike; w = cp; 
    }
    public override double payoff(double S1, double S2)
    {
        double max = Comparisons.Max(S1, S2);
        return Comparisons.Max(w * (max - K), 0.0);
    }
}

public class WorstofTwoStrategy : MultiAssetPayoffStrategy
{ // Best of 2 options

    private double K;		// Strike
    private int w;	    	// +1 call, -1 put


    // All public classes need default ructor
    public WorstofTwoStrategy()
    {
        K = 95.0; w = +1;
    }

    public WorstofTwoStrategy(double strike, int cp)
    {
        K = strike; w = cp;
    }
    public override double payoff(double S1, double S2)
    {
        double min = Comparisons.Min(S1, S2);
        return Comparisons.Max(w * (min - K), 0.0);
    }
}

public class PortfolioStrategy : MultiAssetPayoffStrategy
{ // Portfolio option

    private double K;		// Strike
    private int w;	    	// +1 call, -1 put
    int n1, n2;             // quantities of each underlying

    // All public classes need default ructor
    public PortfolioStrategy()
    {
        K = 95.0; w = +1; n1 = n2 = 1;
    }

    public PortfolioStrategy(int N1, int N2, double strike, int cp)
    {
        K = strike; w = cp;
        n1 = N1; n2 = N2;
    }
    public override double payoff(double S1, double S2)
    {
        double min = Comparisons.Min(S1, S2);
        return Comparisons.Max(w * (n1*S1 + n2*S2 - K), 0.0);
    }
}