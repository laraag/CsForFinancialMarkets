// EWH101.cs
//
// Using event wait handler
//
// (C) Datasim Education BV 2009-2013
//

using System;
using System.Threading;

namespace EventWaitHandle101
{
    class MyFirstEWH
    {
        // Initial state is not signalled; argument is 'false'
        static EventWaitHandle wh = new AutoResetEvent(false); 

        static void Main()
        {
            new Thread(Func1).Start();
            Thread.Sleep(1000);
           
            // Thread waits until signalled by another thread
            wh.Set();

            Console.WriteLine("Main thread exited!");
        }

       static void Func1()
        {
            Console.WriteLine("Waiting...");
            wh.WaitOne(); // Block current thread until current wait handle receives a signal
            Console.WriteLine("Notified !");
        }

    }
}
