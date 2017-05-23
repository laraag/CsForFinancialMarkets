// CalculateDistance.cs
//
// Interface to calculate distance between two points.
//
// (C) Datasim Education BV  2002-2013

using System;

public interface ICalculateDistance
{
	// Calculate the distance. We can't pass a point because then we 
	// need to know about point but the point must know about this
	// interface. Then we get circular dependencies.
	double Distance(double x1, double y1, double x2, double y2);
}
