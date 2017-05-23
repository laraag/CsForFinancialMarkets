using System;

public class TestPoint
{
	public static void Main()
	{
		Point p=new Point(1.0, 3.0);	// Create Point
		p.Draw();

		p.Reset();
		p.Draw();

		// Test 'is' operator
		Shape s=new Point(10, 20);
		if (s is IResettable)			// Does shape implement IResettable interface
		{
			IResettable r=(IResettable)s;	// Cast Shape to IResettable
			r.Reset();						// Reset shape via interface
		}
		s.Draw();

		// Look if the Shape reference really references a Point
		if (s is Point) Console.WriteLine("Shape really is a Point");

		// Test 'as' operator
		Shape s2=new Point(10, 20);
		IResettable r2=s2 as IResettable;		// Cast shape to IResettable
		if (r2!=null) r2.Reset();				// If cast succeeded, reset shape via interface 
		s2.Draw();
	}
}