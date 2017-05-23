using System;

public class Point
{ // Point class

	// Static members
	private static int numPoints=0;						// Number of points created
	private static Point origin=new Point(0.0, 0.0);	// Create origin point which is always available
    public static Point origin2; 	

	private double x;			// Space for x-coordinate
	private double y;			// Space for y-coordinate

    static Point()
    { // Static constructor

        double x = ((8 * 62) / (31 * 16)) - 1;
        double y = (88 / 2) - (11 * 4);
        origin2 = new Point(x, y);
    }
	// Constructors
	public Point(): this(0.0, 0.0)
	{ // Default constructor

       // numPoints++;
        Console.WriteLine("Default object created");	
	}

	public Point(Point s): this(s.x, s.y)
	{ // Copy constructor

    //    numPoints++;
        Console.WriteLine("Copy constructor");	
	}

	public Point(double x, double y)
	{ // Constructor with coordinates

		this.x=x;
		this.y=y;
		numPoints++;
	}

	~Point()
	{ // Finalizer invoked just before object is garbage collected

		numPoints--;	// Decrease counter
	}

	public double X
	{ // X-property

		get
		{ // Return the x-coordinate

			return x;
		}

		set
		{ // Set the x-coordinate

			x=value;
		}
	}

	public double Y
	{ // Y-property

		get
		{ // Return the y-coordinate

			return y;
		}

		set
		{ // Set the y-coordinate

			y=value;
		}
	}

	public static int GetPoints()
	{ // Return nr. of points created
	  // Note, in static members you can't use 'this'

		return numPoints;
	}

	public static Point Origin
	{ // Read only property

		get
		{
			return origin;
		}
	}
}