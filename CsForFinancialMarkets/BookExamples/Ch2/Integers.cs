using System;

public class Integers
{
    public static void Main()
	{
		sbyte x=-10;                       // x is -10
		long y=99;                         // y is 99

		int zero=0;
		try
		{
			zero=100/zero;                 // Throws DivideByZeroException
		}
		catch (DivideByZeroException ex)
		{
			Console.WriteLine(ex);
		}


		ushort hex=0xFF;                   // 255 as hexadecimal notation

		// Create two ints. One with the highest int value(2147483647)
		// then calculate the sum of those integers. The result
		// will wrap around and does not give an overflow.
		int i1=2147483647, i2=1; 
		int sum=i1+i2;                    // Wraps to -2147483648 (smallest int)

		int m=4;
		long l1=2147483647*m;             // l1=-4
		long l2=2147483647L*m;            // l2=8589934588

		Console.WriteLine("sbyte x=-10: " + x);
		Console.WriteLine("long y=99: " + y);
		Console.WriteLine("int zero=100/0: " + zero);
		Console.WriteLine("ushort hex=0xFF: " + hex);
		Console.WriteLine("int sum=i1+i2: " + sum);
		Console.WriteLine("long l1=2147483647*m: " + l1);
		Console.WriteLine("long l1=2147483647L*m: " + l2);

	}
}
