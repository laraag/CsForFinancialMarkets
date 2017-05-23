// TwoFactorBinomialParameters.hpp
//
// Simple structure to hold option data and numeric data for 
// the two-factor binomial method.
//
// This is a quick and dirty data structure just to test the 
// 2-factor binomial. But it works.
//
// (C) Datasim Education BV 2006
//

public class TwoFactorBinomialParameters
{
    public ITwoFactorPayoff pay;      // Payoff function

	// Option data
	public double sigma1;
    public double sigma2;
	public double T;
	public double r;
	public double K;
	public double div1, div2;			// Dividends
	public double rho;					// Correlation
	public bool exercise;				// false if European, true if American	

	// Default constuctor, prototype object
	public TwoFactorBinomialParameters()
	{

		sigma1 = 0.2;
		sigma2 = 0.3;
		T = 1.0;						// One year
		r = 0.06;
		K = 1;
		div1 = 0.03;
		div2 = 0.04;
		rho = 0.5;
		exercise = true;
	}

    public double payoff(double S1, double S2)
    {
        return pay.payoff(S1, S2);
    }

 }

