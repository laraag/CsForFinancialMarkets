// Thing.cs
//
// (C) Datasim Education BV  2002

using System;

public class Thing: IScreen, IPrinter
{
	void IScreen.Print()
	{ // Print implementation for screen

		Console.WriteLine("Printing to the screen");
	}

	void IPrinter.Print()
	{ // Print implementation for printer

		Console.WriteLine("Printing to the printer");
	}


	public static void Main()
	{
		Thing t=new Thing();

//		t.Print();					// Illegal since interface is implemented explicitly

		((IScreen)t).Print();		// Print to screen
		((IPrinter)t).Print();		// Print to printer
	}
}