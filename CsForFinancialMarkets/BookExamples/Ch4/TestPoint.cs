using System;

public class TestPoint
{
	public static void Main()
	{
        // Test ICloneable
        Shape sc1 = new Point(1.4, 1.2); // Create Point
        // Shape sc2=new Point(sc1); // Illegal. Tries to call Point(Shape)
        Shape sc3 = (Shape)sc1.Clone(); // OK. Copy allowed
        // Copy constructor (non-polymorphic)
        Point pt = new Point(1.5, 2.3);
        Point pt2 = new Point(pt);
	}
}