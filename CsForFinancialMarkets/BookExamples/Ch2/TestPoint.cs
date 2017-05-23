// TestPoint.cs
//
// (C) Datasim Education BV  2002

using System;

public abstract class TestPoint
{
	public static void Main()
	{
		Point p1;								// Create point
//		Console.WriteLine("p1: {0}", p1);		// Error. Cannot use p1 before all members are initialised
		p1.x=10; p1.y=20;						// Initialize p1
		Point p2=new Point(10, 20);				// Create and initialise p2
		Point p3=p2;							// Create p3 and initialise as real copy of p2

		// Print points
		Console.WriteLine("p1: {0}", p1);		// Point(10, 20)
		Console.WriteLine("p2: {0}", p2);		// Point(10, 20)
		Console.WriteLine("p3=p2: {0}", p3);	// Point(10, 20)
		
		// Change p3. Since p3 is real copy, p2 shouldn't change (when point is a class it should change)
		p3.x=1; p3.y=2;

		// Print points
		Console.WriteLine("Unchanged p2: {0}", p2);		// Point(10, 20)
		Console.WriteLine("Changed p3: {0}", p3);		// Point(1, 2)

//		if (p1==p2) Console.WriteLine("p1 and p2 are equal");	// Error. No == operator defined
	}
}