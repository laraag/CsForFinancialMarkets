 // Point.cs
 // 
 // Point with an embedded parametric polymorphic method.
 // 
 // (C) Datasim Education BV  2002-2012

using System;


public struct Point 
{
	public double x;
	public double y;

	public Point(double xVal, double yVal)
	{  // Normal constructor

		 // Constructor must initialize all fields
		x = xVal;
		y = yVal;
	}

     // Message-passing implementation
    public double distance(Point p2)
    {  // The current object is the receiver

        return Math.Sqrt((this.x - p2.x) * (this.x - p2.x) + (this.y - p2.y) * (this.y - p2.y));
    }

     // Global (non-message passing) implementation
    public static double distance(Point p1, Point p2)
    {
        return Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
    }
}