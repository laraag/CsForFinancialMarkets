// GenericMethod.cs
//
// (C) Datasim Education BV  2005-2013

using System;
using System.Drawing;

public class GenericMethod
{
	public static void Main()
	{
		// Create object with the generic swap method
		GenericMethod gm=new GenericMethod();

		// Value type
		double d1=2.79;
		double d2=3.14;
		
		// Struct (value type)
		Point p1=new Point(1, 2);
		Point p2=new Point(3, 4);

		// Class (reference type)
		Exception e1=new Exception("Exception 1");
		Exception e2=new Exception("Exception 2");

		// Print values
		Console.WriteLine("Original values");
		Console.WriteLine("Doubles: {0}, {1}", d1, d2);
		Console.WriteLine("Points: {0}, {1}", p1, p2);
		Console.WriteLine("Exceptions: {0}, {1}", e1, e2);

		// Swap the values
		gm.Swap<double>(ref d1, ref d2);
		gm.Swap<Point>(ref p1, ref p2);
		gm.Swap<Exception>(ref e1, ref e2);

		// Print values again
		Console.WriteLine("\nSwapped values");
		Console.WriteLine("Doubles: {0}, {1}", d1, d2);
		Console.WriteLine("Points: {0}, {1}", p1, p2);
		Console.WriteLine("Exceptions: {0}, {1}", e1, e2);

		// Swap the values
		gm.Swap(ref d1, ref d2);
		gm.Swap(ref p1, ref p2);
		gm.Swap(ref e1, ref e2);

		// Print values again
		Console.WriteLine("\nSwapped again values");
		Console.WriteLine("Doubles: {0}, {1}", d1, d2);
		Console.WriteLine("Points: {0}, {1}", p1, p2);
		Console.WriteLine("Exceptions: {0}, {1}", e1, e2);

		// Call with specifying the type
		Console.WriteLine("\n\nCall generic print with type");
		gm.Print<double>(3.14);
		gm.Print<Exception>(new Exception("Exception"));

		// Call without specifying the type
		Console.WriteLine("\nCall generic print without type");
		gm.Print(3.14);
		gm.Print(new Exception("Exception"));
	}

	/// <summary>
	/// Generic swap method.
	/// Swap two values.
	/// </summary>
	/// <typeparam name="T">The type of the values to swap.</typeparam>
	/// <param name="t1">The first value to swap.</param>
	/// <param name="t2">The second value to swap.</param>
	public void Swap<T>(ref T t1, ref T t2)
	{
		T tmp=t1;
		t1=t2;
		t2=tmp;
	}

	/// <summary>
	/// Generic print method.
	/// </summary>
	/// <param name="value">The value to print.</param>
	void Print<T>(T value)
	{
		System.Console.WriteLine(value);
	}
}