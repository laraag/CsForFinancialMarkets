// TestThead102.cs
//
// Create two threads on two class instances of a RNG(Random Number Generator)
//
// (C) Datasim Education BV 2009-2013
//
using System;
using System.Threading;

public class ThreadingSample
{
    static void Main(string[] args)
    {

        // Create random numbers with their own seeds
        int seed1 = 22;
        int seed2 = 3;
        RNG w1 = new RNG(seed1, 'A'); // Characters used when printing
        RNG w2 = new RNG(seed2, 'B');

        // Create threads and generate the random numbers
        Thread t1 = new Thread(new ThreadStart(w1.Compute));
        Thread t2 = new Thread(new ThreadStart(w2.Compute));

        // Fire up the two threads
        t1.Start();
        t2.Start();

        // Block calling thread until the others have completed
        // All threads wait here for all other threads to arrive
        t1.Join();
        t2.Join();

        Console.WriteLine("Done.");
    }
}

public class RNG
{ // A simple class that generates random numbers

    public RNG(int seed, char separator)
    {
        m_seed = seed;
        myRandom = new Random(m_seed);
        m_sep = separator;
    }

    public void Compute()
    {
        for (int ii = 0; ii < 10; ++ii)
        {
            Console.WriteLine(String.Format("{0} {1}, ", m_sep, myRandom.NextDouble()));
            Thread.Sleep(1000);
        }
    }

    private char m_sep;

    private Random myRandom;
    private int m_seed;
}
