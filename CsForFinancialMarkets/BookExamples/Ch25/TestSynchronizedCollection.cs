// TestSynchronizedCollection.cs
//
// Making collections thread-safe
//
// (C) Datasim Education BV 2009-2013
//

using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;

public class SynchronizedCollection
{
    static void Main(string[] args)
    {

        // Create an array list that contains objects
        ArrayList doubles = new ArrayList();

        // Fill the array list with integers [0,9]
        for (int i = 0; i < 10; i++) doubles.Add(i);   


        // Now create synchronized list
        ArrayList synchronizedDoubles = ArrayList.Synchronized(doubles);

        Thread t1 = new Thread(delegate() { DoJob(ref synchronizedDoubles, 11.0); });
        Thread t2 = new Thread(delegate() { DoJob(ref synchronizedDoubles, -24.0); });
       
        // You can change the order in which to start threads and view output
        // In either case we get one consistent view
        t2.Start();
        t1.Start();
      
        // Then try this order and see what happens
     /*   t1.Start();
        t2.Start();*/

        t1.Join();
        t2.Join();
       

        for (int ii = 0; ii < synchronizedDoubles.Count; ++ii)
        {
            Console.WriteLine("{0}", synchronizedDoubles[ii]);
            Thread.Sleep(1000);
        }
      
        Console.WriteLine("Done.");
    }

    public static void DoJob(ref ArrayList arr, double val)
    {
        for (int ii = 0; ii < arr.Count; ++ii)
        {
            arr[ii] = val;
        }
    }
}

