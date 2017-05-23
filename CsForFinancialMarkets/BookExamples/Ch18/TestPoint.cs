using System;

public class TestPoint 
{
    public static void Main()
    {
        Point p1;
        p1.x=0; p1.y=0;
        Point p2=new Point(1, 1);
        
        // Print points
        Console.WriteLine("p1: {0}", p1); // Point(0, 0)
        Console.WriteLine("p2: {0}", p2); // Point(1, 1)

        // Using algorithms
        double d = p1.distance(p2, new Pythagoras());
        Console.WriteLine("Distance: {0}", d);      
    }
}