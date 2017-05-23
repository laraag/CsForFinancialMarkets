// Shape.cs
//
// Abstract base class for shapes.
// A object derived from Shape must overrride the following methods:
// ToString()	: This method is used by Console.WriteLine(Object)
//				  to print a description of the object.
// Clone()		: The clone method is used to make a deep copy of a shape.
//
// (C) Datasim Education BV  2001-2013

using System;

	public abstract class Shape: System.ICloneable
	{
		public Shape()
		{ // Default constructor
		}

		public Shape(Shape source)
		{ // Copy constructor
		}

		public override string ToString()
		{ // Overrides object.ToString(). ToString() is used by Console.WriteLine(object)

			return "Shape";
		}

       
		// Make a deep copy of the shape. Typical implementation is: return new MyShape(this);
		public abstract object Clone();
	}