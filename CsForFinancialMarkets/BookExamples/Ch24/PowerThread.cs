// PowerThread.cs
//
// Example that shows the usage of Thread.Join().
//
// (C) Datasim Education BV  2002-2004

using System;
using System.Threading;

public class PowerThread
{

	// Calculate (m^n) / (m*n)
	public static void Main()
	{
		int m=20;
		int n=200;

		// Create a m^n calculation thread
		PowerThread pt=new PowerThread(m, n);
		Thread t=new Thread(new ThreadStart(pt.Calculate));

		// Start m^n calculation in parallel
		t.Start();

		// Now we do our own calculation while the PowerThread is calculating m^n
    
        // TODO: Compute the sum of the diagonal elements of current value 
		double result=m*n;

		// Wait till the PowerThread is finished
		t.Join();

		// Display result
		Console.WriteLine("({0}^{1}) / ({0}*{1}) = {2}", m, n, pt.result/result);
	}


	private int m, n;			// Variables for m^n
	public double result;		// The result


	// Constructor
	public PowerThread(int m, int n)
	{
		this.m=m; this.n=n;
	}

	// Calculate m^n. Supposes n>=0.
	public void Calculate()
	{
		result=m;				// Start with m^1
		for (int i=1; i<n; i++)
		{
			result*=m;			// result=result*m
			Thread.Sleep(10);	// Allow other threads to run
		}
		if (n==0) result=1;		// m^0 is always 1
	}
}