// TestNestedLock.cs
//
// Using nested locks in C#.
//
// (C) Datasim Education BV 2009
//

using System;
using System.Threading;

namespace NestedLock
{
    class MyNestedLock
    {
        static object locker = new object();

        static void Main()
        {
            lock (locker)
            {
                Console.WriteLine("I get the lock");
                Nest();
                Console.WriteLine("I still have the lock");
            }

        }

        static void Nest()
        {
            lock (locker)
            {
                Console.WriteLine("Nested lock");
            }
        }
       
    }
}
