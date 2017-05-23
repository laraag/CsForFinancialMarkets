// TestMonitorWaitPulse.cs
//
// Synchronisation using Monitor Wait and Pulse synchronisation 
// constructs.
//
// (C) Datasim Education  BV 2009-2013
//

using System;
using System.Threading;

public class MonitorPulse
{
    static object locker = new object();    // Step 1
    static bool go = false;                 // Step 2

	public static void Main()
    {

        Thread t1 = new Thread(Work);
        t1.Start();

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();

        // Step 4
        lock (locker)
        {
            go = true;
            Monitor.PulseAll(locker);
        }
		
	}

	
	public static void Work()
	{
        // Step 3
        lock (locker)
        {
            while (!go)
            {
                Monitor.Wait(locker);
            }
        }
        Console.WriteLine("I am awoken");
	}
}



