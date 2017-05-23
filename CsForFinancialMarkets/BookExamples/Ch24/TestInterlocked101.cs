// TestInterlocked101.cs
//
// Basic atomic operations on numeric data using the Interlocked class.
//
// (C) Datasim Education BV 2009
//

using System;
using System.Threading;

class TestInterlocked101
{
    static void Main()
    {
        long sum = 10;

        // Increment the value to 11, then decrement to 10
        Interlocked.Increment(ref sum);
        Console.WriteLine(Interlocked.Read(ref sum));
        Interlocked.Decrement(ref sum);
        Console.WriteLine(Interlocked.Read(ref sum));

        // Add 20 to the current value, 30 will be the new value
        Interlocked.Add(ref sum, 20);
        Console.WriteLine(Interlocked.Read(ref sum));

        // Compare two values for equality, and if equal replace 
        // one of the values
        int comparand = 20;
        int newValue = 50;
        Interlocked.CompareExchange(ref sum, newValue, comparand); 
        Console.WriteLine(Interlocked.Read(ref sum));

        comparand = 30;
        newValue = 70;
        Interlocked.CompareExchange(ref sum, newValue, comparand);
        Console.WriteLine(Interlocked.Read(ref sum));
    }
}

