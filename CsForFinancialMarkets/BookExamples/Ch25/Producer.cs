// Producer.cs
//
// 2009-6-6 DD for RN Streams
//
// (C) Datasim Education BV  2002-2009

using System;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent; // Parallel collections here


public class Producer
{
	private volatile BlockingCollection<double> m_queue;
	private int m_id;

	// Default constructor
	public Producer(BlockingCollection<double> q, int id)
	{
		m_queue=q;
		m_id=id;
	}

	// Start the producer
	public void Start()
	{
		Thread t=new Thread(new ThreadStart(Run));
		t.Start();
	}

	// The runnable part of thread
	private void Run()
	{ // Producer start function

        Random rand = new Random();
        double val;

        int N = 100;
        int counter = 0;

		while (counter < N)
		{ // Create N blocks of random numbers; each block is an array
			// Add object to synch. queue m_queue
            Console.WriteLine("Block number {0}", counter);

            long M = 50;
            for (long ctr = 1; ctr <= M; ctr++)
            {
                val = rand.Next();
                //  m_queue.Enqueue(val); Sequential code
                m_queue.TryAdd(val);

            }
			
			// Wait a while
            Thread.Sleep(1000); counter++;
		}

	}
}