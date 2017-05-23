using System;

public abstract class Shape: ICloneable
{
	public Shape()
	{ // Default constructor

	}

	public Shape(Shape source)
	{ // Copy constructor
		
	}

	~Shape()
	{ // Destructor

	}

	public override string ToString()
	{ // Return descriptive string.

		return "Shape";
	}

	public abstract void Draw();
	public abstract object Clone(); // Implement ICloneable.Clone() as abstract method
}