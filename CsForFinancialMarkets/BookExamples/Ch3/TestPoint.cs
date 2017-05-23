using System;

public class TestPoint
{
	public static void Main()
	{
		Point p=new Point();	// Create point

		p.X=2.3;				// Set x-coordinate
		p.Y=3.1;				// Set y-coordinate

		// Prints "Point(2.3, 3.1)"
		Console.WriteLine("Point({0}, {1})", p.X, p.Y);

        // Test different access
        //p.Y2=100; // No access
       // Console.WriteLine("Y2 property: {0}", p.Y2);

	}
}
