// Point.cs
//
// Point class
//
// (C) Datasim Education BV  2001-2013



	public class Point: Shape
    {
        // Point class
        private double x;           // Space for x-coordinate
        private double y;           // Space for y-coordinate

        // Constructors
        public Point(): base() // Call default constructor of base class
        {
            // Default constructor
            x=y=0.0;
        }
        
        public Point(Point source): base(source)
        { 
            // Copy constructor; Call default constructor of base class
            this.x=source.x;
            this.y=source.y;
        }
        
        public Point(double x, double y): base()
        { 
            // Constructor with coordinates; Call default constructor of base class
            this.x=x;
            this.y=y;
        }
        
        public override object Clone()
        {
            // Create a new copy of Point
            return new Point(this); // Use copy constructor
        }
    }