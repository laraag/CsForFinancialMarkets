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

public class Consumer
{
	private volatile BlockingCollection<double> m_queue;
	private int m_id;

	// Default constructor
	public Consumer(BlockingCollection<double> q, int id)
	{
		m_queue=q;
		m_id=id;
	}

	// Start consumer
	public void Start()
	{
		Thread t=new Thread(new ThreadStart(Run));
		t.Start();
	}

	// Runnable part of thread
	private void Run()
	{ // Consume start function

        double val;
		while (true)
		{
			// Retrieve object from queue
			{
				// Retrieve from synch. queue m_queue and print
				//val = (double)m_queue.Dequeue();
                val = (double)m_queue.Take();

				Console.WriteLine(String.Format("Consumer {0}, Message: {1}", m_id, val));
			}
		}
	}
}