// TestSemaphore001.cs
//
// First semaphore example.
//
// (C) Datasim Education BV 2009-2013
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

public class SemaphoreExample
{
    private static Semaphore pool;
    
    public static void Main()
    {
        int Max = 5;        // The capacity of the pool
        int Reserved = 4;   // Number of places currently available
        int NThreads = 5;   // The number of created threads

        pool = new Semaphore(Reserved, Max); // Reserved <= Max

        for (int i = 1; i <= NThreads; i++)
        { // Create and start threads

            Thread t = new Thread(new ParameterizedThreadStart(Worker));

            t.Start(i);
        }

    }

    private static void Worker(object id)
    {
        Console.WriteLine("Thread " + id + " wants to enter ");
        pool.WaitOne();

        Console.WriteLine("Thread " + id + " is in");
        Thread.Sleep(1000 + (int)id);
        Console.WriteLine("Thread " + id + " is leaving");
        pool.Release();
    }
}


