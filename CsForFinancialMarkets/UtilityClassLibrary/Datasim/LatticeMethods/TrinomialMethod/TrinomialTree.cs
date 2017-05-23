// TrinomialTree.cs
//
// This class represents a trinomial tree model of the asset price.
// This model assumes that the asset price can reach tree values.
// When x = lnS, than the tree values can be calculated via:
// lnS = x + dx, lnS = x, lnS = x - dx. Thus S can become larger, smaller or
// stay same with a defined way.
//
// Started 30 october 2000 (JT)
// 2006-8-5 DD Update and rewrite
// 2011-1-25 DD C# version
//
// (C) Datasim Component Technology 2000-2013

using System;

public class TrinomialTree
{

	private TrinomialParameters par;	// The source of the data

	int N;						// Redundant parameter, number of time steps

	// Representing the base for making of the trinomial tree structure (array of arrays which are vectors)
	//Vector<Vector<TV,I,FullArray<TV> >,I,FullArray<Vector<TV,I,FullArray<TV> > > >* TriTree;

	// This data structure is an array (in time 0 to N) of simpler structure. Each simpler structure
	// is a Vector (in S space) whose elements are a two-dimensional vector containg the asset price
	// S and option price (C or P)
	// Vector<Vector<double>, int>* TriTree;
	// N.B. In this version we do NOT model the tree explicitly

	//Vector<double> option; // The option data at each time level and underlying
    Vector<Vector<double> > option;



// Constructors and destructor
 
private TrinomialTree()
{// Default constructor

	// No body
}

 
private TrinomialTree(TrinomialTree tritree)
{// Copy-constructor

	// No body

}

 
public TrinomialTree(TrinomialParameters optionData)
{ // The most important constuctor

	par = optionData;

	N = par.NumberOfSteps;

	/* NOT in this version
	// Build the tree

	// Exercise: optimise all this loop stuff
	TriTree = new Vector<Vector<double, int>, int>[N + 1];

	for(int n = N; n >= 0;n--)
	{
		TriTree[n] = 
			Vector<Vector<double, int>,int>(2*n+1,-n);
	}
	
	for(int nn = N; nn >= 0 ; nn--)
	{
		for(int j = -nn; j <= nn; j++)
		{
			TriTree[nn][j] = 0.0; // Magic number but OK
		}
	}	
	*/

}

 

// Functions for declaration and initialisation of the trinomial tree
 

public double Price(double S)
{// Calculate Call price: page 54 Clewlow figure 3.3 pseudo code
 // In this case with dividend (div)

	double sig2 = par.sigma * par.sigma;

	double dt = par.T / N;
	double nu = par.r - par.div -0.5 * sig2;

	// Since trinomal is explicit FDM we have a constraint between
	// dt and dx (Clewlow inequality (3.27))
	double dx = par.sigma * Math.Sqrt(3.0 * dt) + dt*1; 
	double edx = Math.Exp(dx);

	// Temporary variables, as in Clewlow 1998
	double nu2 = nu * nu;
	double dt2 = dt * dt;

	double dx2 = dx * dx;

	double tmp1 = (sig2 * dt + nu2 * dt2) / dx2;
	double tmp2 = nu * dt / dx;

	/* double pu = 0.5 * ( ( sig2 * dt + nu2 * dt2) / dx2 + nu * dt / dx); // page 53, formula 3.6
	double pm = 1.0 - (sig2 * dt + nu2 * dt2 ) / dx2;	// page 53, formula 3.7
	double pd = 0.5 * ( (sig2 * dt + nu2 * dt2) / dx2 - nu * dt / dx);	// page 53, formula 3.8*/

	double pu = 0.5 * (tmp1 + tmp2);
	double pm = 1.0 - tmp1;
	double pd = 0.5 * (tmp1 - tmp2);

	double disc = Math.Exp(-par.r * dt);

	// Initialise asset prices at *maturity*. Norice that the start index is a negative number
	Vector<double> asset = new Vector<double>(2*N+1,-N);

	asset[-N] = S * Math.Exp(-N*dx);
	for (int n = -N + 1; n <= N; n++)
	{
		asset[n] = asset[n-1] * edx;
	}

	Console.WriteLine("finished asset\n");
	// Initialise option values at maturtity

    
//	option = new Vector<double>[N+1];
    int minIndex = 0;
    option = new Vector<Vector<double>>(N + 1, minIndex);

	for (int n= N; n >= 0; n--)
	{
		for(int j = -n; j <= n;j++)
		{
			option[n] = new Vector<double>(2*n+1,-n);
		}
	}

	int nn = N;
	if (par.type == 'C')
	{
		for(int j = -nn; j <= nn; j++)
		{
            option[nn][j] = Math.Max(0.0, asset[j] - par.K);
		}
	}
	else	// Put option
	{
		
		for(int j = -nn; j <= nn; j++)
		{
            option[nn][j] = Math.Max(0.0, par.K - asset[j]);
		}

	}

	// Step back through lattice, starting from maturity as given value n = N

	// European option
	if (par.exercise == false)
	{

		for(int n = N-1; n >= 0; n--)
		{
			for(int j =- n; j <= n; j++)
			{
				option[n][j] = disc*(pu*option[n+1][j+1]+pm*option[n+1][j]+pd*option[n+1][j-1]); // 10.6
			}
		}
	}
	else	// American put only
	{
		//cout << "American put option\n";
		double tmp;
		for(int n = N-1; n >= 0; n--)
		{
			for(int j =- n; j <= n; j++)
			{

				tmp = disc*(pu*option[n+1][j+1]+pm*option[n+1][j]+pd*option[n+1][j-1]); // 10.6
				option[n][j] = Math.Max(tmp, par.K - asset[j]); // American correction term
			}
		}
	}

    // Give the option price
	return option[0][0];
}

 	

} 