// TimeChangeEventArgs.cs
//
// (C) Datasim Education BV  2002-2013

using System;

public class TimeChangeEventArgs: EventArgs
{
	public DateTime dt;

	public TimeChangeEventArgs(DateTime dt)
	{ // Constructor

		this.dt=dt;
	}
}