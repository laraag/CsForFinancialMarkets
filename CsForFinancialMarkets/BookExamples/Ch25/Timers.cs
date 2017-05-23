// Timers.cs
//
// Using timers in C#
//
// (C) Datasim Education  BV 2009-2013
//

using System;
using System.Threading;
//using System.Timers; // contains a wrapper class for Threading Timer

public class Threads
{

	public static void Main()
	{
		int whenToStart = 5000;
        int howOften = 1000;

        Timer myTimer = new Timer(DoSomething, "Anyone for tennis?", whenToStart, howOften);

        Console.ReadLine(); // on main thread
        myTimer.Dispose();

        
	}

	// The method that will be run by the thread
	public static void DoSomething(object data)
	{
        Console.WriteLine(data);
	}
}


