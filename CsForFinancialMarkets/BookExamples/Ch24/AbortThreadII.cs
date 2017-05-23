// AbortThreadII.cs
//
// How to abort a thread. Now the thread is aborted from Main
//
// (C) Datasim Education BV 2009
//

using System;
using System.Threading;

class Abort101_II
{ // How to abort a thread. Now the thread is aborted from Main

    static void Main()
    {
        Worker w = new Worker();
        Thread t = new Thread(w.Work);
        t.Start();

        Thread.Sleep(1000); // Main thread sleeps for 1000 ms

        Console.WriteLine("Aborting ");    
        t.Abort();
        Console.WriteLine("Aborted ");  
    }

    class Worker
    {
        //volatile bool abort;

        //public void Abort() { abort = true; }

        public void Work()
        {
            while (true)
            {
                               
                try 
                { 
                    OtherMethod(); 
                }
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

     
    }
}
