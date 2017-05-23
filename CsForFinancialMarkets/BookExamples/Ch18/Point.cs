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
    {
        // Normal constructor
        // Constructor must initialize all fields
        x = xVal;
        y = yVal;
    }

    public double distance<Algo>(Point p2, Algo algo)
        where Algo : IDistance
    {
        return algo.distance(this, p2);
    }

    public override string ToString()
    {
        // Redefine this method from base class 'object'
        return string.Format("Point ({0}, {1})", x, y);
    }
}

public interface IDistance
{
    double distance(Point p1, Point p2);
}

public class Pythagoras : IDistance
{
    public double distance(Point p1, Point p2)
    {
        return Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
    }
}
