// TestThreadPool.cs
//
// Using thread pools.
//
// (C) Datasim Education BV 2009
//


using System;
using System.Threading;

public class TestThreadPool
{
    public static void Main()
    {
        // Queue the task.
        ThreadPool.QueueUserWorkItem(Work);
        ThreadPool.QueueUserWorkItem(Work, 998);
        ThreadPool.QueueUserWorkItem(Work, "hello");

        Console.WriteLine("Main thread does some work, then sleeps.");
       
        Thread.Sleep(1000);

        // Set and get stuff
        int minWorker, minIOC; // Minimum number of worker and asynchronous I/O 
        // completion threads.
        // Get the current settings.
        ThreadPool.GetMinThreads(out minWorker, out minIOC);
        if (ThreadPool.SetMinThreads(4, minIOC))
        {
            // The minimum number of threads was set successfully.
            Console.WriteLine("Success, min threads {0}, {1} ", minWorker, minIOC);

        }
        else
        {
            // The minimum number of threads was not changed.
            Console.WriteLine("No change, min threads {0}, {1} ", minWorker, minIOC);
        }

        Console.WriteLine("Press any key to continue.");

        Console.ReadLine();

        Console.WriteLine("Main thread exits.");
    }

    // This thread procedure performs the task.
    static void Work(object data)
    {
      Console.WriteLine("Hello from the thread pool " + data);
    }
}


