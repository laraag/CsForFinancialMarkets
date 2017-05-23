// Main.cs
//
// (C) Datasim Education BV  2005-2007

using System;

// Define alias 
using DoublePair=GenericPair<double, double>;

/// <summary>
/// Test program for the generic pair.
/// </summary>
public class MainClass
{
	public static void Main()
	{
		GenericPair<double, int> pair1=new GenericPair<double, int>();
		GenericPair<int, double> pair2=new GenericPair<int, double>(10, 3.14);

		Console.WriteLine("Pair 1: {0}", pair1);
		Console.WriteLine("Pair 2: {0}", pair2);

		pair1.Value1=2.79;
		pair1.Value2=60;
		Console.WriteLine("Pair 1: ({0}, {1})", pair1.Value1, pair1.Value2);

		DoublePair dp=new DoublePair(1.5, 5.1);
		Console.WriteLine("Double pair: {0}", dp);
	}
}