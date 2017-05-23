// SleepTest.cs
//
// Small application to test Thread.Sleep()
//
// (C) Datasim Education BV  2002-2004

using System;
using System.Threading;

public class SleepTest
{
	// Frame counter
	private static int s_frame=0;

	public static void Main()
	{
		// Start animation thread
		Thread t=new Thread(new ThreadStart(Animation));
		t.Start();
	}

	// Display animation (ThreadStart delegate implementation)
	private static void Animation()
	{
		// Never ending loop
		while (true)
		{
			// Display the next animation frame
			DisplayNextFrame();

			// Sleep for 40ms (25 frames/second)
			Thread.Sleep(40);
		}
	}

	// Display next animation frame
	private static void DisplayNextFrame()
	{
		Console.WriteLine("Displaying frame: {0}", s_frame++);
	}
}