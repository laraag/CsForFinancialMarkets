// Thread101.cs
//
// Basic features of Thread class and how multiple threads
// interact.
//
// We use the lock keyword.
//
// (C) Datasim Education BV 2009
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Thread101
{
    class MyFirstThread
    {
        static object locker = new object(); // make it private

        static void Main()
        {
            Thread t1 = new Thread(Func3);
            t1.Start();
            t1.Name = "t1";

            Thread t2 = new Thread(Func2);
            t2.Start();
            t2.Name = "t2";

            t1.Join();
            t2.Join();


        }

        static void Func0()
        {

            lock (locker) 
            for (int j = 1; j <= 100; j++)
            {
                Console.Write("0");
            }
        }

        static void Func1()
        {
            lock (locker) 
            for (int j = 1; j <= 100; j++)
            {
                Console.Write("A");
            }
        }

        static void Func2()
        {
            lock (locker) 
            for (int j = 1; j <= 100; j++)
            {
                Console.Write("B");
            }
        }

        static void Func3()
        {
            Monitor.Enter(locker);
            try
            {
                for (int j = 1; j <= 100; j++)
                {
                    Console.Write("MON");
                }
            }
            finally { Monitor.Exit(locker); }
        }

    }
}
