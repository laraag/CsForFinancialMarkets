// TestFilter.cs
//
// Using the Pipes and Filter pattern with 3 filters and two 
// pipes. This is a pipeline (linear). 
//
// (C) Datasim Education BV  2002-2009

using System;
using System.Collections;

public class MainClass
{
	public static void Main()
	{
        // Create queues/pipes
		BlockingQueue<string> Q1 = new BlockingQueue<string>();
        BlockingQueue<string> Q2 = new BlockingQueue<string>();

		// Create the pipeline filters
        int id = 1;
        int id2 = 12;
        Producer p = new Producer(Q1, id);
        Consumer c = new Consumer(Q2, id);
        Filter f = new Filter(Q1, Q2, id2);
 
		// Start the pipeline
        p.Start();
        c.Start();
        f.Start();
	}
}