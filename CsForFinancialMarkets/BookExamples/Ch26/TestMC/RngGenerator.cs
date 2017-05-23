// RngGenerator.cs
//
// 2009-6-6 DD for RN Streams
// 2012-1-18 DD for MC pricer
//
// (C) Datasim Education BV  2002-2012

using System;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent; // Parallel collections here


public class RngGenerator
{
	private volatile BlockingCollection<double> m_queue;
	private int m_id;
    private int N;      // Total number of simulations or even block size
    private volatile bool m_stop;
    private volatile Thread m_thread;

	// Default constructor
	public RngGenerator(BlockingCollection<double> q, int id, int NumberRN)
	{
		m_queue=q;
		m_id=id;
        N = 2*NumberRN; // Box Muller
        m_stop = false;
	}

	// Start the producer
	public void Start()
	{
		m_thread=new Thread(new ThreadStart(Run));
		m_thread.Start();
	}

    // Stop the producer.
    public void Stop()
    {
        m_stop = true;
        m_thread.Join();
    }

	// The runnable part of thread
	private void Run()
	{ // Producer start function

        Random rand = new Random();
        double U1, U2, G1, G2;

        int counter = 0;

        while (!m_stop)
		{ // Create random numbers
			// Add object to synch. queue m_queue

            //    Console.WriteLine("Producer, block number {0}", counter/2);

            U1 = (double) rand.Next() / (double)System.Int32.MaxValue; // In interval (0,1)
            U2 = (double)rand.Next() / (double)System.Int32.MaxValue; // In interval (0,1)

            // Box-Muller method
            G1 = Math.Sqrt(-2.0 * Math.Log(U1)) * Math.Cos(2.0 * 3.1415 * U2);
            G2 = Math.Sqrt(-2.0 * Math.Log(U1)) * Math.Sin(2.0 * 3.1415 * U2);   
            
            // This is because the m_stop can be set to true in TryAdd
            // Instead of TryAdd and stop use a cancellation token
            while (!m_stop && !m_queue.TryAdd(G1, 100));
            while (!m_stop && !m_queue.TryAdd(G2, 100));

            counter += 2;
		}
	}
}