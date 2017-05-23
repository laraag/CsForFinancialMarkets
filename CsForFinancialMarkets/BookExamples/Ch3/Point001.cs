// Point class
public class Point
{ 
	private double x;			// Space for x-coordinate
	private double y;			// Space for y-coordinate

	// Constructors

	// Default constructor
	public Point(): this(0.0, 0.0)
	{ 
	}

	// Copy constructor
	public Point(Point s): this(s.x, s.y)
	{ 
	}

	// Constructor with coordinates
	public Point(double x, double y)
	{ 
		this.x=x;
		this.y=y;
	}

	// Return the x-coordinate
	public double X()
	{ 
		return x;
	}

	// Return the y-coordinate
	public double Y()
	{ 
		return y;
	}

	// Set the x-coordinate
	public void X(double x)
	{ 
		this.x=x;
	}

	// Set the y-coordinate
	public void Y(double y)
	{ 
		this.y=y;
	}
}