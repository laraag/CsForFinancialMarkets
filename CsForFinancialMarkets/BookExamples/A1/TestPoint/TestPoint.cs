 // TestPoint.cs
 // 
 // Using parametric polymorphism and generic constraints.
 // 
 // (C) Datasim Education BV  2002-2012

using System;

public abstract class TestPoint
{
	public static void Main()
	{
		Point p1;							
		p1.x=0; p1.y=0;						
		Point p2=new Point(1, 1);				

	 // Print points
	Console.WriteLine("p1: {0}", p1);		 // Point(0, 0)
	Console.WriteLine("p2: {0}", p2);		 // Point(1, 1)

     // Message-passing (p1 is the receiver and p2 is the method argument)
    
    double d = p1.distance(p2);
    Console.WriteLine("Distance, version 1: {0}", d);

     // Global, non message-passing
    double d2 = Point.distance(p1, p2);
    Console.WriteLine("Distance, version 2: {0}", d2);

	}
}
