using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Text;




public class ThreadData_IV
{

    public static void Run()
    {
        // Create object with shared data
        Common obj = new Common();
        obj.message = "First message";

        new Thread(obj.Run).Start();

        Console.ReadLine();
        Console.WriteLine(obj.reply);

    }


    // Class with shared variables
    public class Common
    {
        public string message;
        public string reply;

        public void Run()
        {
            Console.WriteLine(message);
            reply = "I am the new common data";
        }
    }
}

public class Threads
{
    // Variable to indicate that the thread must stop himself
    private static volatile bool s_stop = false;

    public static void Main()
    {
        // Create a thread running the "DoSomething" method.
        ThreadStart ts = new ThreadStart(DoSomething);
        Thread t = new Thread(ts);

        // Start the thread
        t.Start();

        // Now we can do something else
        for (int i = 0; i < 10000; i++) Console.WriteLine("Main method");

        // Let the thread stop himself
        s_stop = true;
    }

    // The method that will be run by the thread
    public static void DoSomething()
    {
        while (!s_stop)
        {
            Console.WriteLine("Do something in parallel with Main");
        }
    }
}




   
   