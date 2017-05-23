// InterruptThread.cs
//
// How to interrupt a thread.
//
// (C) Datasim Education BV 2009
//

using System;
using System.Threading;

class Interrupt101
{

    static void Main()
    {
        Thread t = new Thread(delegate()
        {
            try
            {
                // This thread loops/sleeps forever if not interrupted
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
                Console.Write("by force, I have been ");
            }
            Console.WriteLine("awoken");
        });

        t.Start();
        t.Interrupt();
    }
}
