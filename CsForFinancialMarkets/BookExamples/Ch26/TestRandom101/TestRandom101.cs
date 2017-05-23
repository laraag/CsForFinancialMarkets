// TestRandom101.cs
//
// (C) Datasim Education BV 2012
//

using System;
using System.Threading;
using System.Collections.Concurrent; // Parallel collections here
using System.Security.Cryptography;


class MyFirstRandom
{
    static void Main()
    {

        // Instantiate random number generator using system-supplied value as seed.
        Random rand = new Random();

        // Generate and display 5 random byte (integer) values.
        byte[] bytes = new byte[4];
        rand.NextBytes(bytes);
        Console.WriteLine("Five random byte values:");
        foreach (byte byteValue in bytes)
            Console.Write("{0, 5}", byteValue);
        Console.WriteLine();

        // Generate and display 5 random integers.
        Console.WriteLine("Five random integer values:");
        for (int ctr = 0; ctr <= 4; ctr++)
            Console.Write("{0,15:N0}", rand.Next());
        Console.WriteLine();


        // Generate and display 5 random integers between 0 and 100.//
        Console.WriteLine("Five random integers between 0 and 100:");
        for (int ctr = 0; ctr <= 4; ctr++)
            Console.Write("{0,8:N0}", rand.Next(101));
        Console.WriteLine();


        // Generate and display 5 random integers from 50 to 100.
        Console.WriteLine("Five random integers between 50 and 100:");
        for (int ctr = 0; ctr <= 4; ctr++)
            Console.Write("{0,8:N0}", rand.Next(50, 101));
        Console.WriteLine();


        // Generate and display 5 random floating point values from 0 to 1.
        Console.WriteLine("Five Doubles.");
        for (int ctr = 0; ctr <= 4; ctr++)
            Console.Write("{0,8:N3}", rand.NextDouble());
        Console.WriteLine();

        // Generate and display 5 random floating point values from 0 to 5.
        Console.WriteLine("Five Doubles between 0 and 5.");
        for (int ctr = 0; ctr <= 4; ctr++)
            Console.Write("{0,8:N3}", rand.NextDouble() * 5);

        Console.WriteLine();

        // Creating 'reproducible' random numbers
        Random r1 = new Random(3);
        Random r2 = new Random(3);
        Random r3 = new Random();

        Console.WriteLine(r1.Next(20) + ", " + r1.Next(30));    // 5, 20
        Console.WriteLine(r2.Next(20) + ", " + r2.Next(30));    // 5, 20

        Console.WriteLine(r3.Next(20) + ", " + r3.Next(30));    // each time different


        // Using 'safe seeds' and thread-safe 
        ThreadLocal<Random> ran = new ThreadLocal<Random> (() => new Random(Guid.NewGuid().GetHashCode()) );
        Console.WriteLine(ran.Value.Next(20) + ", " + ran.Value.Next(30));   
        
        // Cryptpgraphic generators
        var ranC = System.Security.Cryptography.RandomNumberGenerator.Create();
        byte[] myBytes = new byte[32];
        ranC.GetBytes(myBytes);

        int val0 = BitConverter.ToInt32(myBytes, 0);
        int val1 = BitConverter.ToInt32(myBytes, 1);
        int val2 = BitConverter.ToInt32(myBytes, 2);
        int val3 = BitConverter.ToInt32(myBytes, 3);

        Console.WriteLine("{0}, {1}, {2}, {3}", val0, val1, val2, val3);   

    }
}

// Sample console output might appear as follows:
//    Five random byte values:
//      194  185  239   54  116
//    Five random integer values:
//        507,353,531  1,509,532,693  2,125,074,958  1,409,512,757    652,767,128
//    Five random integers between 0 and 100:
//          16      78      94      79      52
//    Five random integers between 50 and 100:
//          56      66      96      60      65
//    Five Doubles.
//       0.943   0.108   0.744   0.563   0.415
//    Five Doubles between 0 and 5.
//       2.934   3.130   0.292   1.432   4.369 