// Option.cs
//
// The option model containing relevant data and functions.
//
// (C) Datasim Component Technology BV 2003-2013
//

using System;

public struct Option
{
	public double r;		// Interest rate
	public double sig;		// Volatility
	public double K;		// Strike price
	public double T;		// Expiry date
//	double b;		        // Cost of carry
	public int type;		// 1 == Call, 2 == Put
    //bool earlyExercise;     // European/false ; American/true    
    //double H;               // Barrier 

	public double payoff(double S)
	{
//        double H = 95.0;

		if (type == 1)
		{
         /*   if (S <= H)
            {
                return 0.0;
            }*/

            return Math.Max(S - K, 0.0);
		}
		else
		{
            return Math.Max(K - S, 0.0);
		}
	}

    public Vector<double> PayoffVector(Vector<double> xarr)
    {
	    // We need the form of the lattice at the 'base' of the pyramid. This
	    // will be needed when we use backward induction 
	
	    Vector<double> result= new Vector<double>(xarr);

    	// Now use functionMechanisms 
    	/* Vector<double, int> 
			createDiscreteFunction(double (*f)(double x), const Vector<double, int>& xarr);	*/

	    for (int j = xarr.MinIndex; j <= xarr.MaxIndex; j++)
	    {
			result[j] = payoff(xarr[j]);
	    }

	    return result;
    }

    public double EarlyImpl(double P, double S)
    { // Check for ptimal exercise

        // Choose between call and put
        if (type == 1) // Call
        {
            return Math.Max(S - K, P);
        }

        else // Put
        {
            return Math.Max(K - S, P);
        }
    }
}

