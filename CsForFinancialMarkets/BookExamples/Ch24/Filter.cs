// Filter.cs
//
// A Filter componengt in a Pipes and Filters pattern.
//
// (C) Datasim Education BV  2002-2009

using System;
using System.Collections;
using System.Threading;


public class Filter
{
	private volatile BlockingQueue<string> Q1; // Input pipe
    private volatile BlockingQueue<string> Q2; // Output pipe

	private int m_id;

	// Default constructor
	public Filter(BlockingQueue<string> inputPipe, BlockingQueue<string> outputPipe, int id)
	{
        Q1 = inputPipe;
        Q2 = outputPipe;

		m_id=id;
	}

	// Start the Filter
	public void Start()
	{
		Thread t=new Thread(new ThreadStart(Run));
		t.Start();
	}

	// The runnable part of thread
	private void Run()
	{
        // In this case the filter processes data from the queue from another
        // producer and then sends it on to a consumer.
        // In other words, this component plays both roles.

		int counter=0;
		while (true)
		{
			// Add object to queue
            string str = (string)Q1.Dequeue();
            str = str + " " + counter;
			Q2.Enqueue(str);
			
			// Wait a while
			Thread.Sleep(500);
            counter++;
            if (counter > 10) counter = 0;
		}

	}
}