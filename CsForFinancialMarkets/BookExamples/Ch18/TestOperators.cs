// TestOperators.cs
//
// Testing generic operations using DLR instead of Reflection API.
//
// (C) Datasim Education BV 2010-2013
//

using System;
using System.Collections.Generic;
using System.Reflection;

class Operators
{

    public static void Main()
    {
        int M = 10;
        Vector<double> v1 = new Vector<double>(M);
        Vector<double> v2 = new Vector<double>(v1.Size);
        for (int j = v1.MinIndex; j <= v1.MaxIndex; j++)
        {
            v1[j] = 1.0;
            v2[j] = 2.0;
        }

        Vector<double> v3 = new Vector<double>(v1.Size);

        // Now using operator overloading
        v3 = v1 - v2;
        for (int j = v3.MinIndex; j <= v3.MaxIndex; j++)
        {
            Console.Write("{0}, ", v3[j]);
        }

        Console.WriteLine("--");

        Vector<double> v4 = new Vector<double>(v1.Size);

        // Now using operator overloading
        v4 = v1 + v2;
        for (int j = v4.MinIndex; j <= v4.MaxIndex; j++)
        {
            Console.Write("{0}, ", v4[j]);
        }

        Console.WriteLine("--");
        Vector<double> v5 = new Vector<double>(v1.Size);

        // Now using operator overloading
        v5 = v1 * v2;
        for (int j = v5.MinIndex; j <= v5.MaxIndex; j++)
        {
            Console.Write("{0}, ", v5[j]);
        }

    }
}

 

