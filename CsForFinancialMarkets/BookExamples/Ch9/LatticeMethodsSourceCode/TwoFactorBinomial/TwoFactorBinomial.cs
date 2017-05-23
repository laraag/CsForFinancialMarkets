// TwoFactorBinomial.cs
//
// This class represents a trinomial tree model of the asset price.
// This model assumes that the asset price can reach tree values.
// When x = lnS, than the tree values can be calculated via:
// lnS = x + dx, lnS = x, lnS = x - dx. Thus S can become larger, smaller or
// stay same with a defined way.
//
// Started 30 october 2000 (JT)
// 2006-8-5 DD Update and rewrite
// 2006-8-7 DD done for 2-factor binomial method
// 2010-7-30 DD C# version
//
// (C) Datasim Component Technology 2000-2013

using System;

public class TwoFactorBinomial
{
    private TwoFactorBinomialParameters par;	// The source of the data
   
    private int N;								// Redundant parameter, number of time steps

    // Probability parameters
    private double puu, pdd, pud, pdu;

    // Mesh sizes
    private double h1, h2, delta_T;

    // Array structures
    private Vector<double> asset1, asset2;

    // Data structure as in Clewlow; 2d matrix
    private NumericMatrix<double> option; 

 
    public static double Max(double d1, double d2)
    {
        if (d1 > d2)
            return d1;
        return d2;
    }

public TwoFactorBinomial(TwoFactorBinomialParameters optionData, int NSteps,
									 double S1, double S2)
{ // The most important constuctor

	par = optionData;

	N = NSteps;

	// Mesh sizes, Clewlow (2.43)-(2.44)
	delta_T = par.T/N;			            // DeltaT
	h1 = par.sigma1 * Math.Sqrt(delta_T);	// DeltaX1
	h2 = par.sigma2 * Math.Sqrt(delta_T);	// DeltaX2

	//cout << "deltas t, S... " << k << ", " << h1 << ", " << h2 << endl;

	// Parameters (prob)
	double nu1 = par.r - par.div1 - (0.5 * par.sigma1 * par.sigma1);
	double nu2 = par.r - par.div2 - (0.5 * par.sigma2 * par.sigma2);

	//cout << "nu1... " << nu1 << ", " << nu2 << endl;

	double a = h1*h2; double b = h2 * nu1 * delta_T; double c = h1 * nu2 * delta_T;
	double d = par.rho * par.sigma1 * par.sigma2 * delta_T;

	//cout << "a ..." << a << ", " << b << ", " << c << ", " << d << endl;

	double disc = Math.Exp(-(par.r) * delta_T);

	puu = disc * 0.25 * (a + b + c + d)/a; // eq. (2.45)
	pud = disc * 0.25 * (a + b - c - d)/a;
	pdu = disc * 0.25 * (a - b + c - d)/a;
	pdd = disc * 0.25 * (a - b - c + d)/a;

	//cout << "puu ..." << puu << ", " << pud << ", " << pdu << ", " << pdd << endl;

	// Asset arrays
	// Initialise asset prices at *maturity*. Norice that the start index is a negative number
	asset1 = new Vector<double>(2*N+1,-N);
	asset2 = new Vector<double>(2*N+1,-N);

	asset1[-N] = S1 * Math.Exp(-N*h1);
	asset2[-N] = S2 * Math.Exp(-N*h2);

	double edx1 = Math.Exp(h1);
	double edx2 = Math.Exp(h2);

	//cout << "edx1... disc " << edx1 << ", " << edx2 << ", " << disc << endl;

	for (int n = -N + 1; n <= N; n++)
	{
		asset1[n] = asset1[n-1] * edx1;
		asset2[n] = asset2[n-1] * edx2;
	}

	//print(asset1);
	//print(asset2);
	
	option = new NumericMatrix<double>(2*N + 1, 2*N + 1, -N, -N);

	// Calculate option price at the expiry date
	for (int j = option.MinRowIndex; j <= option.MaxRowIndex; j += 2)
	{
		//cout << j << ", ";
		for (int k = option.MinColumnIndex; k <= option.MaxColumnIndex; k += 2)
		{
	
			option[j, k] = Payoff(asset1[j], asset2[k]);
		}
	}
}


// Payoff function (modify as needed)
public double Payoff(double S1, double S2)
{

    return par.payoff(S1, S2);
}


// Functions for declaration and initialisation of the trinomial tree
 

public double Price()
{// Calculate Call price: page 44 Clewlow 

	
	// Step back through lattice, starting from maturity as given value n = N

	// European option
	if (par.exercise == false)
	{
		for(int n = N-1; n >= 0; n--)
		{
			//cout << n << "\n ";
			for(int j = -n; j <= n; j += 2)
			{
				for (int k = -n; k <= n; k += 2)
				{
					option[j, k] = puu * option[j+1, k+1] + pud * option[j+1,k-1]
									+ pdu * option[j-1, k+1] + pdd * option[j-1,k-1];
				}
			}
		}
	}
	else	// American put only
	{
	//	cout << "American exercise\n";
		for(int n = N-1; n >= 0; n--)
		{
			for(int j = -n; j <= n; j += 2)
			{
				for (int k = -n; k <= n; k += 2)
				{
					option[j,k] = puu * option[j+1, k+1] + pud * option[j+1,k-1]
									+ pdu * option[j-1, k+1] + pdd * option[j-1,k-1];

					option[j,k] = Max(option[j,k], asset1[j] - asset2[k] - (par.K));
					
				}
			}
		}
	}

	return option[0,0];
}

 // Accuracy of price on number of steps
public static Vector<double> Prices(TwoFactorBinomialParameters optionData, Vector<int> meshSizes, double S1,
												double S2) 
{
	// Caculates the price for a number of step sizes (usually increasing)

	Vector<double> result= new Vector<double> (meshSizes.Size);
	//print (result);

	// TwoFactorBinomial(TwoFactorBinomialParameters optinData,long NSteps,
	//									double S1, double S2); 

	TwoFactorBinomial local;

	for (int j = result.MinIndex; j <=  result.MaxIndex; j++)
	{
		
			local = new TwoFactorBinomial(optionData, meshSizes[j], S1, S2);
		
			result[j] = local.Price();
			//cout << local.Price() << ";;";
	}

	return result;
}

} 
