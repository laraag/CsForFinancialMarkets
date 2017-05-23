// ThreadDataTransfer.cs
//
// Showing how to pass data to a thread
//
// (C) Datasim Education  BV 2009
//

using System;
using System.Threading;

public class ThreadData_I
{

    public static void Main()
    {
        // Create a thread running the "Print" method.
        Thread ts = new Thread(Print);
       
        // Start the thread
        string message = (string)("Hi there");
        ts.Start(message);

        // Now we can do something else in main threa
        Console.WriteLine("Main method");

    }


    // The method that will be run by the thread
    public static void Print(object myMessage)
    {
        // Cast object to a string
        Console.WriteLine((string)(myMessage));
    }

}
