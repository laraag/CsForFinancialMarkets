// FeaturesVersion4.cs
//
// Showing simple examples of new features in .NET 4.0.
//
// (C) Datasim Education BV 2011-2013
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewFeaturesVersion4
{
    class Features
    {
        // Optional parameters
        static void Func(int x = 1, int y = 2) { Console.WriteLine("{0} , {1}", x, y); }

        // Dynamic objects
        static dynamic Sum(dynamic x, dynamic y)
        {
            Console.WriteLine("Sum {0}", x + y);
            return x + y; 
        }

        static void Main()
        {
            // Calling optional parameters
            Func();         // 1,2
            Func(3);        // 3,2
            Func(4, 3);     // 4,3


            // Named arguments
            Func(y:10, x:20);   // 20, 10
            Func(y: 10);        // 1, 10
            Func(x: 10);        // 10, 2
            Func(x:-90, y: 10); // -90, 10

            // Mixing positional and named parameters
            Func(-100, y:200);
            // Func(x:100, 200); Error, named argumnts must appear after positional arguments

            // Dynamic binding
            int x = 2; int y = 4;
            int z = Sum(x, y);

            double a = 2.0; double b = 3.0;
            double c = Sum(a, b);

        }
    }
}
