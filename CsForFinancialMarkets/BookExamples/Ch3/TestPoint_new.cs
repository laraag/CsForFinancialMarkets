using System;

public class TestPoint
{
	public static void Main()
	{
        // Default constructor
        Point p1 = new Point();
        Console.WriteLine("Point: ({0}, {1})", p1.X, p1.Y);
        p1.X = 2.0; p1.Y = -3.0;

        // Copy constructor
        Point p2 = new Point(p1);
        Console.WriteLine("Point: ({0}, {1})", p2.X, p2.Y);

        // Constructor with x and y coordinates
        Point p3 = new Point(1.0, 2.0);
        Console.WriteLine("Point: ({0}, {1})", p3.X, p3.Y);

   /*     // Prints 2 (The static origin points: origin and origin2)
		Console.WriteLine(Point.GetPoints());	// Prints 2 

		Point p1=new Point();					// Create point
		Console.WriteLine(Point.GetPoints());	// Prints 3

		// Remove object
		p1=null;

		// Wait for garbage collection
		while (Point.GetPoints()==3) 
		{
			Console.WriteLine("Waiting, it can take a while");
		}

        // Prints 2. p1 garbage collected
		Console.WriteLine(Point.GetPoints());	*/
	}
}
