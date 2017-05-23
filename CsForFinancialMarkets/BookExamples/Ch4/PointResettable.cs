// Point class derived from Shape, implements IResettable
public class Point: Shape, IResettable
{
	// Static members
	private static int s_numPoints=0;					// Number of points created
	private static Point s_origin=new Point(0.0, 0.0);	// Create origin point which is always available

	private double m_x;			// Space for x-coordinate
	private double m_y;			// Space for y-coordinate


	// Constructors

	// Default constructor
	public Point(): base()		// Call default constructor of base class
	{
		m_x=m_y=0.0;
		s_numPoints++;
	}

	// Copy constructor
	public Point(Point source): base(source)	// Call copy constructor of base class
	{
		m_x=source.m_x;
		m_y=source.m_y;
		s_numPoints++;
	}

	// Constructor with coordinates
	public Point(double x, double y): base()	// Call default constructor of base class
	{
		m_x=x;
		m_y=y;
		s_numPoints++;
	}

	// Finalizer invoked just before object is garbage collected
	~Point()
	{
		s_numPoints--;	// Decrease counter
	}

	// X-property
	public double X
	{
		// Return the x-coordinate
		get
		{
			return m_x;
		}

		// Set the x-coordinate
		set
		{
			m_x=value;
		}
	}

	// Y-property
	public double Y
	{
		// Return the y-coordinate
		get
		{
			return m_y;
		}

		// Set the y-coordinate
		set
		{
			m_y=value;
		}
	}

	// Return the coordinates using ref parameters
	// The input arguments must be initialised
	public void GetCoordinatesRef(ref double x, ref double y)
	{
		x=m_x;
		y=m_y;
	}

	// Return the coordinates using out parameters
	// Not needed to initialise the input arguments
	public void GetCoordinatesOut(out double x, out double y)
	{
		x=m_x;	// We must give out parameter a value here,
		y=m_y;	// else error
	}

	// Return nr. of points created
	// Note, in static members you can't use 'this'
	public static int GetPoints()
	{
		return s_numPoints;
	}

	// Read only property
	public static Point Origin
	{
		get
		{
			return s_origin;
		}
	}

	// Return descriptive string
	public override string ToString()
	{
		return base.ToString() + string.Format(": Point({0}, {1})", m_x, m_y);
	}

	// Draw point, emulated with printing text
	public override void Draw()
	{
		System.Console.WriteLine("Draw Point({0}, {1})", m_x, m_y);
	}

	// Reset the Point
	public void Reset()
	{ 
		m_x=m_y=0.0;
	}


    public override object Clone()
    { // Clone object. x=y only copies the reference to y, not the contents of y.
        // This method can be used to make a deep copy of a list containing Shapes

        return new Point(this);
    }	
}