// ClockSubscriber.cs
//
// (C) Datasim Education BV  2002-2013

using System;

public class ClockSubsciber
{
	public static void Main()
	{
		// Create clock instance
		Clock clock=new Clock();

		// Subscribe event handlers for Clock.TimeChangeEvent
		clock.OnTimeChange+=new Clock.TimeChangeEventHandler(DisplayTime1);
		clock.OnTimeChange+=new Clock.TimeChangeEventHandler(DisplayTime2);

		// Start the clock
		clock.Run();
	}

	private static void DisplayTime1(object sender, TimeChangeEventArgs args)
	{ // TimeChangeEventHandler 1

		Console.WriteLine("DisplayTime 1: {0}", args.dt);
	}

	private static void DisplayTime2(object sender, TimeChangeEventArgs args)
	{ // TimeChangeEventHandler 2

		Console.WriteLine("DisplayTime 2: {0}", args.dt);
	}
}