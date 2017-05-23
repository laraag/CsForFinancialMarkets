// Consumer.cs
//
// Use a blocking queue
//
// 2009-5-14 DD new queue
// 2009-6-6 DD for RN Stream
//
// (C) Datasim Education BV  2002-2009

using System;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent; // Parallel collections here

public class MCSolver
{
	private volatile BlockingCollection<double> m_queue;
	private int m_id;
    private SDE mySDE;
    int N, NSim;
    double S_0;
    double VOld, VNew;
    int coun;
    private volatile Thread m_thread;

	// Default constructor
	public MCSolver(BlockingCollection<double> q, int id, SDE sde, int NSteps, int NSimulations, double initialValue)
	{
		m_queue=q;
		m_id=id;
        mySDE = sde;
        N = NSteps;
        S_0 = initialValue;
        NSim = NSimulations;
        coun = 0;
	}

	// Start consumer
	public void Start()
	{
		m_thread=new Thread(new ThreadStart(Run));
		m_thread.Start();
	}

    // Wait till finished.
    public void Join()
    {
        m_thread.Join();
    }

	// Runnable part of thread
	private void Run()
	{ // Consume start function

        Console.WriteLine("Consumer, block number {0}", coun++);

        // Create the basic SDE (Context class)
	    Range<double> range = new Range<double>(0.0, mySDE.data.T);
	    
	    Vector<double> x = range.mesh(N);
	
	    double k = mySDE.data.T / (double)N;
	    double sqrk = Math.Sqrt(k);

	    // Normal random number
        double dW;
	    double price = 0.0;	// Option price
        Random rand = new Random();
	   
        // A.
	    for (long i = 1; i <= NSim; ++i)
	    { // Calculate a path at each iteration
			
		    if ((i/20000) * 20000 == i)
		    { // Give status after each 1000th iteration

                Console.WriteLine(i);
		    }

            VOld = S_0;
		    for (int index = x.MinIndex+1; index <= x.MaxIndex; ++index)
		    {

			    // Create a random number
                dW = m_queue.Take(); 

         	    // The FDM (in this case explicit Euler)
			    VNew = VOld  + (k * mySDE.drift(x[index-1], VOld))
						+ (sqrk * mySDE.diffusion(x[index-1], VOld) * dW);

			    VOld = VNew;
		    }

            double tmp = mySDE.data.myPayOffFunction(VNew);
		    price += (tmp) / (double)NSim;
	    }
	
	    // D. Finally, discounting the average price
	    price *= Math.Exp(-mySDE.data.r * mySDE.data.T);

        Console.WriteLine("Price {0}", price);	

	}
}