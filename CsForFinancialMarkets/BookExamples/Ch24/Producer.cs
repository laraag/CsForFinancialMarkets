// Producer.cs
//
// A multi-threaded class whose instances push string data
// onto a multi-threaded queue.
//
// (C) Datasim Education BV  2002-2009
//

using System;
using System.Collections;
using System.Threading;


public class Producer
{
	private volatile BlockingQueue<string> m_queue;
	private int m_id;

	// Default constructor
	public Producer(BlockingQueue<string> q, int id)
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
	{
		int counter=0;
		while (true)
		{ // The producer produces forever, no 'poison pill'

			// Add object to queue
			m_queue.Enqueue(String.Format("Producer {0}, Object {1}", m_id, counter++));
			
			// Wait a while
			Thread.Sleep(500);
		}

	}
}