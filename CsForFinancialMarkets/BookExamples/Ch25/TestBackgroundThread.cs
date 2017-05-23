// TestJoin.cs
//
// (C) Datasim Education BV 2009-2013
//

using System;
using System.Threading;

public class BackgroundSample
{
    static void Main(string[] args)
    {
        MyWorker w1 = new MyWorker("Tom");
        MyWorker w2 = new MyWorker("Bob");

        Thread t1 = new Thread(new ThreadStart(w1.DoJob));
        Thread t2 = new Thread(new ThreadStart(w2.DoJob));

        // Put t1 and t2 into the background
        t1.IsBackground = true;
        t2.IsBackground = true;

        t1.Start();
        t2.Start();

        // What happens if you uncomment out these joins? 
        // t1.Join();
        // t2.Join();

        Console.WriteLine("Done.");
    }
}

public class MyWorker
{
    public MyWorker(String name_)
    {
        myName = name_;
    }

    public void DoJob()
    {
        for (int ii = 0; ii < 4; ++ii)
        {
            Console.WriteLine(String.Format("{0} is working ...", myName));
            Thread.Sleep((int)(myRandom.NextDouble() * 5));
         }
    }

    private String myName;
    private Random myRandom = new Random();
}
