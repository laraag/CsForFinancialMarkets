// Point.cs
//
// (C) Datasim Education BV  2002

public struct Point
{
	public double x;
	public double y;

//	public Point(){} // Illegal: default constructor automatically generated	

	public Point(double xVal, double yVal)
	{ // Normal constructor

		// Constructor must initialize all fields
		x = xVal;
		y = yVal;
	}

	public override string ToString()
	{ // Redefine this method from base class 'object'

		return string.Format("Point ({0}, {1})", x, y);
	}
}