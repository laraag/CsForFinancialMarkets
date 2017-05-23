// TestThreadException.cs
//
// Catching exceptions in a MT program.
//
// (C) Datasim Education BV 2009
//

using System;
using System.Threading;

class TestThreadException
{ // How to abort a thread. Now the thread is aborted from Main

    static void Main()
    {
   
        try
        {
            new Thread(Work).Start();
        }
        catch (Exception e)
        { // MT exception NOT caught here

            Console.WriteLine("In main method"+ e.Message);
        }
     
    }

    static void Work()
    {
        try
        {
            throw new Exception(); 
        }
        catch (Exception ex)
        {
            Console.WriteLine("In start method"+ ex.Message);
            //throw new Exception();
        }
        
    }
}
