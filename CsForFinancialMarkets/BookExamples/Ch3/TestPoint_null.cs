using System;

public class TestPoint
{
    public static void Main()
    {
        // Prints 2 (The static origin points: origin and origin2)
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
        Console.WriteLine(Point.GetPoints());
    }
}