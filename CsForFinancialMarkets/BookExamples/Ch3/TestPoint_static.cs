using System;

public class TestPoint
{
    public static void Main()
	{
		// Prints 1 (The static origin point)
        Console.WriteLine(Point.GetPoints());

		Point p1=new Point();				// Create point
		Console.WriteLine(Point.GetPoints());	// Prints 2

		// Remove object
		p1=null;

		// Wait for garbage collection
		while (Point.GetPoints()==2) 
		{
		//	Console.WriteLine("Waiting, it can take a while");
		}
        
        // Prints 1. p1 garbage collected
		Console.WriteLine(Point.GetPoints());	
	}
}
