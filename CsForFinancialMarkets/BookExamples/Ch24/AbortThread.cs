// AbortThread.cs
//
// How to abort a thread. The thread is aborted in the start
// method.
//
// (C) Datasim Education BV 2009
//

using System;
using System.Threading;

class Abort101
{

    static void Main()
    {// The thread is aborted in the start method.

        Worker w = new Worker();
        Thread t = new Thread(w.Work);
        t.Start();

        Thread.Sleep(1000); // Main thread sleeps for 1000 ms

        Console.WriteLine("Aborting ");    
        w.Abort();
        Console.WriteLine("Aborted ");  
    }

    class Worker
    {
        volatile bool abort;

        public void Abort() { abort = true; }

        public void Work()
        {
            while (true)
            {
                CheckAbort();
                // Do stuff
                try { OtherMethod(); }
                catch (ThreadAbortException)
                {
                    Console.WriteLine("Unable to execute the method, thread has been aborted ");
                }

                finally { /* cleanup */ }
            }
        }

        void OtherMethod()
        {
            Console.WriteLine("Just another method"); 
        }

        void CheckAbort()
        {
            // Abort the currently running thread
            if (abort) Thread.CurrentThread.Abort();
        }
    }
}
