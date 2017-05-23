// EWH102.cs
//
// Using event wait handler
//
// (C) Datasim Education BV 2009-2013
//

using System;
using System.Threading;

namespace ManualResetEvent101
{
    class MyFirstEWH
    {
        // Initial state is not signalled
        static EventWaitHandle wh = new ManualResetEvent(false); 

        static void Main()
        {
            Thread t1 = new Thread(Func1);
            t1.Start();
            Thread.Sleep(1000);

            Thread t2 = new Thread(Func2);
            t2.Start();
            Thread.Sleep(1000);

            // Thread waits until signalled by another thread
            wh.Set();

            Console.WriteLine("Exiting Main()");
        }

       static void Func1()
        {
            Console.WriteLine("Waiting I...");
            wh.WaitOne();
            Console.WriteLine("Notified I !");
        }

       static void Func2()
       {
           Console.WriteLine("Waiting II...");
           wh.WaitOne();
           Console.WriteLine("Notified II !");
       }
    }
}
