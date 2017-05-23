// FunctionIntegrator.cs
//
// Basis information needed for numerical integration.
//
// (C) Datasim Education BV 2012
//

using System;

public class FunctionIntegrator
{

    // Models functions
    public delegate double IntegratorFunction(double x);

    // Members
    private IntegratorFunction func; // Function to be integrated
    private Range<double> range;     // Interval of integration
    private int N;                   // Number of subintervals
    private double h;                // Step size

    public FunctionIntegrator(IntegratorFunction function, Range<double> interval, int NSteps)
    {
        func = new IntegratorFunction(function);
        range = new Range<double>(interval.low, interval.high);
        N = NSteps;

        h  = range.spread / (double) N;
    }

    public void MidPoint()
    {
        double A = range.low;
	    double B = range.high;
        double res = 0.0;

        for (double x = A + (0.5 * h); x < B; x += h)
        {
            res += func(x);
        }

	    //return res*h;
        Console.WriteLine("Midpoint approx: {0}", res * h);
    }


    public void Tanh()
    {
        double A = range.low;
        double B = range.high;
        double res = 0.0;

	    for (double x = A + (0.5 * h); x < B; x += h)
        {
	       res +=  Math.Tanh(func(x) * 0.5 * h);
        }

	   // return 2.0 * res;
        Console.WriteLine("Tanh approx: {0}", 2.0 * res);
    }
}
