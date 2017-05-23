// ThreadDataTransferIII.cs
//
// Showing use of local variables
//
// (C) Datasim Education  BV 2009
//

using System;
using System.Threading;

public class ThreadData_III
{

    public static void Main()
    {
        // Create a thread running the "Print" method.
        Thread ts = new Thread(Print);
        ts.Start();

        // Now we can "Print" in main threa
        Print();
    }


    // The method that will be run by the thread
    public static void Print()
    {
        // Declare and use local variable
        for (int j = 0; j <= 9; j++)
        {
            Console.Write(j);
        }
    }

}
