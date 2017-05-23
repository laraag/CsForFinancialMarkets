// Point.cs
//
// Simple point class
//
// (C) Datasim Education BV  2002-2013

using System;

public class Point
{

	// The distance algorithm to use
	private static ICalculateDistance s_distanceAlgorithm;

	// Access the distance calculation algorithm
	public static ICalculateDistance DistanceAlgorithm
	{
		get { return s_distanceAlgorithm; }
		set { s_distanceAlgorithm=value; }
	}

	// Data members.
	private double m_x;
	private double m_y;

	// Default constructor
	public Point()
	{
		m_x=0.0;
		m_y=0.0;
	}

	// Constructor.
	public Point(double x, double y)
	{
		m_x=x; 
		m_y=y;
	}

	// X property.
	public  double X
	{
		get { return m_x; }
		set { m_x=value; }
	}

	// Y property.
	public double Y
	{
		get { return m_y; }
		set { m_y=value; }
	}

	// Calculate distance between two points
	public double Distance(Point p)
	{
		// Is there an algorithm installed
		if (s_distanceAlgorithm==null) throw new ApplicationException("No distance calculation algorithm installed.");

		// Calculate the distance using the installed algorithm
		return s_distanceAlgorithm.Distance(m_x, m_y, p.m_x, p.m_y);
	}

	// String conversion.
	public override string ToString()
	{
		return String.Format("Point({0}, {1})", m_x, m_y);
	}
}