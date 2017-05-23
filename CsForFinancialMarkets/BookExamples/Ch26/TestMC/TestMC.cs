// TestMC.cs
//
// Main progam for C option pricer using thread-safe, load balancable and
// extendible to multiple RNGs and options.
//
// The design will probably also work on C++ using PPL, MKL etc.
//
// (C) Datasim Education BV  2002-2012
//

using System;
using System.Collections;
using System.Collections.Concurrent; // Parallel collections here
using System.Collections.Generic;

public class MainClass
{
	public static void Main()
	{
		// Ask number of producers
		Console.Write("Number of producers: ");
		int nrProducers=Int32.Parse(Console.ReadLine());

		// Ask number of consumers
		Console.Write("Number of consumers: ");
		int nrConsumers=Int32.Parse(Console.ReadLine());

        OptionData myOption;
        myOption.K = 65.0;
        myOption.T = 0.25;
        myOption.r = 0.08;
        myOption.sig = 0.3;
        myOption.type = -1;	// Put -1, Call +1

        SDE mySDE;
        mySDE.data = myOption;

        SDE mySDE2 = mySDE;
        mySDE2.data.type = 1;

        // Portfolio of options
        Vector<SDE> optArr = new Vector<SDE>(2);
        optArr[optArr.MinIndex] = mySDE;
        optArr[optArr.MinIndex+1] = mySDE2;

        int NT = 200;
        int NSim = 200000;

        // The number of numbers in the collection at any time. Can
        // experiment to test load balancing.
        int N = NT * NSim * nrConsumers;

		// Create queue
		BlockingCollection<double> q = new BlockingCollection<double>(N);
    
        // Create and start producers
        List<RngGenerator> producers=new List<RngGenerator>(nrProducers);
        for (int i = 0; i < nrProducers; i++)
        {
            RngGenerator generator=new RngGenerator(q, i, N);
            generator.Start();
            producers.Add(generator);
        }
        Console.WriteLine("Producers started...");

		// Create and start consumers
        double V0 = 60.0;
        List<MCSolver> consumers = new List<MCSolver>(nrConsumers);
        for (int i = optArr.MinIndex; i <=  optArr.MaxIndex; i++)
        {
            MCSolver solver = new MCSolver(q, i, optArr[i], NT, NSim, V0); // Euler
            solver.Start();
            consumers.Add(solver);
        }
        Console.WriteLine("Consumers started...");

        // Wait till all consumers ar finished.
        foreach (MCSolver solver in consumers) solver.Join();
        Console.WriteLine("Consumers finished...");

        // Stop all producers.
        foreach (RngGenerator generator in producers) generator.Stop();
        Console.WriteLine("Producers stopped");
	}
}