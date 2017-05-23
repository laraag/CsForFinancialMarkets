// ThreadDataTransferII.cs
//
// Showing how to pass data to a thread
//
// (C) Datasim Education  BV 2009
//

using System;
using System.Threading;

public class ThreadData_II
{

    public static void Main()
    {
        // Create a thread running the "Print" method.
        Thread ts = new Thread(delegate(){Print("Hi double O: ", 007);});
       
        // Start the thread
        ts.Start();

        // Now we can do something else in main threa
        Console.WriteLine("Main method");
    }


    // The method that will be run by the thread
    public static void Print(string myMessage, int number)
    {
        // Cast object to a string
        Console.WriteLine("{0}, {1}", myMessage, number);
    }
}
