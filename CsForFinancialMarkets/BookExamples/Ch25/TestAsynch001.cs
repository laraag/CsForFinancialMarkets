// TestIAsynch101.cs
//
// Simple test case to show how callback delegates work.
// Also some code on local storage.
//
// (C) Datasim Education BV 2009-2013
//

using System;
using System.Threading;

class AsynchDelegate001
{
    // Declare asynchronous delegate
    delegate int WorkInvoker(string s);

    static void Main()
    {
        WorkInvoker method = Work;

        // Start the delegate; control returned immediately to caller
        // We also have the callback delegate 'Done' that is automatically called 
        // upon completion. 
        method.BeginInvoke("test", Done, method);

        // Caller can carry on with other operations in *parallel* here
        double d = 1.0;
        Thread.Sleep(5000);
        Console.WriteLine("Value {0}", d);

        Console.ReadLine();
              
    }

   
    // Instantiate delegate
    static int Work(string s) 
    { // In practice, this could be more 'heavyweight'
        
        return s.Length; 
    }

    static void Done(IAsyncResult cookie)
    {
        WorkInvoker method = (WorkInvoker)cookie.AsyncState; //info about asynch operation
        int result = method.EndInvoke(cookie);
        Console.WriteLine("String II length is: " + result);
    }

}
