// Reals.cs
//
// Some special case of working with numeric types.
//
// (C) Datasim Education BV 2010
//

using System;

public class Floats
{
  public static void Main()
  {
    // double, decimal and float literals
    double a=1D;                // a=1.0
    double b=3.14E3;            // b=3140.0 scientific notation
	decimal c=1.1234M;			// c=1.1234

    float pi=1.0F/0.0F;         // Positive infinity (pi==POSITIVE_INFINITY)
    double ni=-1.0/0.0;         // Negative infinity (ni==NEGATIVE_INFINITY)
    float nan=0.0f/0.0f;        // Not a number (nan==NaN)

    Console.WriteLine(a);
    Console.WriteLine(b);
    Console.WriteLine(c);
    Console.WriteLine(pi);
    Console.WriteLine(ni);
    Console.WriteLine(nan);

  }
}