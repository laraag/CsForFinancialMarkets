using System;

public class TestPoint
{
	public static void Main()
	{
		Point p=new Point(1.0, 3.0);	// Create Point
//		Shape s=new Shape();			// Illegal, Shape can't be created because it is an abstract class
		Shape sp=new Point(4.5, 3.1);	// Create Point and assign to Shape reference

		p.Draw();				// Prints 'Draw Point(1.0, 3.0)'
		sp.Draw();				// Prints 'Draw Point(4.5, 3.1)'
	}
}