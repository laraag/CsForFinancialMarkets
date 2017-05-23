// Operators.cs
//
// Example program that shows the usage of various operators.
//
// (C) Datasim Education BV  2002-2013

using System;

public class Operators
{
	public static void Main()
	{
		// Modulo
		int i1=10;
		int i2=3;

		Console.WriteLine("10/3=" + i1/i2);
		Console.WriteLine("10%3=" + i1%i2);

		// Byte arithmetic
		byte b1=10;
		byte b2=20;

		//byte b3=b1+b2;				// Error. Bytes converted to int before addition
		byte b3=(byte)(b1+b2);			// Correct. Cast resulting integer back to byte
		Console.WriteLine("\nb1+b2=" + b3);

		// Shifting
		int s1=10>>2;					// %1010 >> 2 = %10 = 2
		int s2=10<<2;					// %1010 << 2 = %101000 = 40

		int s3=-10>>2;					// %11110110 >> 2 = 11111101 = -3
		int s4=-10<<2;					// %11111111111111111111111111110110 << 2  = 11111111111111111111111111111101 = -40

		Console.WriteLine("\nShift operators");
		Console.WriteLine("10>>2=" + s1);
		Console.WriteLine("10<<2=" + s2);
		Console.WriteLine("-10>>2=" + s3);
		Console.WriteLine("-10<<2=" + s4);

		// Logical
		Console.WriteLine("\nLocical operators");
		Console.Write("f1() || f2(): \n");
		Console.WriteLine(f1()||f2());
		Console.Write("f1() | f2(): \n");
		Console.WriteLine(f1()|f2());

		// Logical 2
		int a=0;
		int b=0;
		Console.WriteLine("\nLocical operators (2)");
		Console.WriteLine("a: {0}, b: {1}", a, b);
		Console.WriteLine("a++<100 | b++<100: {0}", a++<100 | b++<100);
		Console.WriteLine("a: {0}, b: {1}", a, b);
		Console.WriteLine("a++<100 || b++<100: {0}", a++<100 || b++<100);
		Console.WriteLine("a: {0}, b: {1}", a, b);
	 }

	/// <summary>
	/// Simple function for test short-cut or full logical or.
	/// </summary>
	/// <returns>Always true.</returns>
	public static bool f1()
	{
		Console.WriteLine("In f1");
		return true;
	}

	/// <summary>
	/// Simple function for test short-cut or full logical or.
	/// </summary>
	/// <returns>Always true.</returns>
	public static bool f2()
	{
		Console.WriteLine("In f2");
		return true;
	}
}