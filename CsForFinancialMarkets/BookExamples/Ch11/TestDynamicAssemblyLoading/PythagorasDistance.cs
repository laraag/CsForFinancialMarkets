// PythagorasDistanceCalculation.cs
//
// Distance calculation implementation using Pythagoras.
//
// (C) Datasim Education BV  2002-2013

using System;

public class PythagorasDistanceCalculation: ICalculateDistance
{
	public double Distance(double x1, double y1, double x2, double y2)
	{
		double dx=x1-x2;
		double dy=y1-y2;

		return Math.Sqrt(dx*dx+dy*dy);
	}
}