// TaxiDriverDistanceCalculation.cs
//
// Distance calculation implementation using the taxi driver algorithm.
//
// (C) Datasim Education BV  2002-2013

using System;

public class TaxiDriverDistanceCalculation: ICalculateDistance
{
	public double Distance(double x1, double y1, double x2, double y2)
	{
		double dx=x1-x2;
		double dy=y1-y2;

		return Math.Abs(dx) + Math.Abs(dy);
	}
}