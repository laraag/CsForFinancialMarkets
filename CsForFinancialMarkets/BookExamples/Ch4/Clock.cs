// Clock.cs
//
// (C) Datasim Edcuation BV  2002-2013

using System;
using System.Threading;

public class Clock
{
	// Define event delegate
	public delegate void TimeChangeEventHandler(object sender, TimeChangeEventArgs e);	

	// Event variable to store subscribers. Note, it also works without the "event" keyword but with "event" the framework can make a distintion
	public event TimeChangeEventHandler OnTimeChange;

	public void Run()
	{ 

		for (;;Thread.Sleep(1000)) // Infinite loop, sleeps every iteration for 1000 ms
		{
			// Get the current time
			TimeChangeEventArgs args=new TimeChangeEventArgs(DateTime.Now);

			// Raise event and call event methods
			if (OnTimeChange!=null) OnTimeChange(this, args);	// Necessary to check for null
		}
	}
}