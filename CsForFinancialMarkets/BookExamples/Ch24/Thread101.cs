// Thread101.cs
//
// First example. There are two threads corresponding to the main
// routine and an object, respectively.
//
// (C) Datasim Education  BV 2009-2013
//

using System;
using System.Threading;

public class Threads
{

	public static void Main()
	{
		// Create a thread running the "DoSomething" method.
		ThreadStart ts=new ThreadStart(DoSomething);
		Thread t=new Thread(ts);

		// Start the thread
		t.Start();

		// Now we can do something else
		for (int i=0; i<100; i++) Console.WriteLine("Main method");
		
	}

	// The method that will be run by the thread
	public static void DoSomething()
	{
        for (int i = 0; i < 100; i++) 
		{
			Console.WriteLine("Second thread");
		}
	}
}



public class Test
{
	private static volatile int i=0;

	// Method one() and two() can use a locally stored version of i.
	// When called by separate threads it is possible that 
	// method two() never sees the changes that method one() makes.
	public static void one() { while(true) i++; }
	public static void two() { while(true) Console.WriteLine(i); }
}
