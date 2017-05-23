// TimersWrapper.cs
//
// Using timers in C# using the thtread wrapper class which is
// more convenient.
//
// (C) Datasim Education  BV 2009-2013
//

using System;
using System.Timers; // contains a wrapper class for Threading Timer

public class Threads
{

	public static void Main()
	{
	//	int whenToStart = 5000;
    //    int howOften = 1000;

        Timer myTimer = new Timer();

        myTimer.Interval = 1000;
        myTimer.Elapsed += DoSomething;    // Use event instead of a delegate
        myTimer.Start();
        Console.ReadLine(); // on main thread

        myTimer.Stop();
        
        Console.ReadLine(); // on main thread

        myTimer.Elapsed += DoSomething2;    // Use event instead of a delegate
        myTimer.Start();
        Console.ReadLine(); // on main thread

        myTimer.Dispose();

      
	}

	// The method that will be run by the thread
	public static void DoSomething(object sender, EventArgs e)
	{
        Console.WriteLine("wake up");
	}

    public static void DoSomething2(object sender, EventArgs e)
    {
        Console.WriteLine("wake up, again");
    }
}


