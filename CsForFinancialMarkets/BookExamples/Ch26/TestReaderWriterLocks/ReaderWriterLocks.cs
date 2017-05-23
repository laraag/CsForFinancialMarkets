// ReaderWriterlLocks.cs
//
// Using reader/writer locks.
//
// (C) Datasim Education BV 2012
//

using System;
using System.Collections.Generic;
using System.Threading;

struct Tick
{ //  Model number of trades (in Technical Analysis, for example)

    public DateTime time;
    public int price;
    public int qty;
}


// We read and write ticks at random moments to show the effects of 
// relaxed locking.

public class ReaderWriterLock
{
    static List<Tick> tickQueue = new List<Tick>(); // Tick or bar collection

    // Define the lock
    static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

    // Parameters 
    static int N = 7;
    static int M = 2;

    public static void Main()
    {
        new Thread(Read).Start();
        new Thread(Write).Start("A");
        new Thread(WriteUpgradable).Start("C");
        new Thread(Write).Start("B");

        Console.WriteLine("Queue size: {0}", tickQueue.Count);
    }

    static void Read()
    {
        for (int n = 1; n <= N; n++)
        {
            rwl.EnterReadLock();
            foreach (Tick t in tickQueue) Console.WriteLine("Tick Time read at: {0}", t.time);
            Thread.Sleep(10);

            //LockStatus(rwl); // User-defined

            rwl.ExitReadLock();
        }
    }

    static void Write(object ID) 
    { // Write lock

        for (int n = 1; n <= M; n++)
        {
            rwl.EnterWriteLock();
          
            // Create a tick and add to queue
            Tick tick;
            tick.time = DateTime.Now;
            tick.price = M - n;
            tick.qty = n + 2;
            tickQueue.Add(tick);

            Console.WriteLine("Tick Added by thread at time, queue size, ID: {0}, [{1}], {2}", tick.time, tickQueue.Count, ID);

            Thread.Sleep(1000);
            //LockStatus(rwl); // User-defined

            rwl.ExitWriteLock();
        }
    }

    static void WriteUpgradable(object ID)
    { // Write lock that starts as a read lock and becomes a write lock if 
      // the list size is greater than 2

         rwl.EnterUpgradeableReadLock();

         //  if (tickQueue.Count > 0) or we could have some other test here
         {
             rwl.EnterWriteLock();

                // Create a tick and add to queue
                Tick tick;
                tick.time = new DateTime(5767, 1, 1);
                tick.price = M*M;
                tick.qty = N*N;
                tickQueue.Add(tick);

                Console.WriteLine("Upgradable added by thread at time, queue size, ID: {0}, [{1}], {2}", tick.time, tickQueue.Count, ID);
                Thread.Sleep(1000);
              
             rwl.ExitWriteLock();
        }

        rwl.ExitUpgradeableReadLock();
    }

    // Monitoring locks
    static void LockStatus (ReaderWriterLockSlim rwl)  
    { // Are locks of different flavours being held?

          Console.WriteLine("ReadlockHeld, WriteLockHeld, IsUpgradeableReadLockHeld: {0} {1} {2}", 
                      rwl.IsReadLockHeld, rwl.IsWriteLockHeld, rwl.IsUpgradeableReadLockHeld);
    }

}
