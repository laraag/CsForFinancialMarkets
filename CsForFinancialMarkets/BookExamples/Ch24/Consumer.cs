// Consumer.cs
//
// Use a blocking queue. The consumer class takes the data from the queue 
// and uses it for its own purposes.
//
// 2009-5-14 DD new queue
//
// (C) Datasim Education BV  2002-2009

using System;
using System.Collections;
using System.Threading;


public class Consumer
{
	private volatile BlockingQueue<string> m_queue;
	private int m_id;

	// Default constructor
	public Consumer(BlockingQueue<string> q, int id)
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
	{
		while (true)
		{
			// Retrieve object to queue
			lock (m_queue)
			{
				// Retrieve from queue and print
				string str=(string)m_queue.Dequeue();
				Console.WriteLine(String.Format("Consumer {0}, Message: {1}", m_id, str));
			}
		}
	}
}