// TestBackgroundWorker101.cs
//
// Showing the use of BackgroundWorker class
//
// (C) Dataskim Education BV 2009-2013
//


using System;
using System.Threading;
using System.ComponentModel; // BackgroundWorker (BW) defined here

 
class Program
{
  static BackgroundWorker bw;

  static void Main()
  {
    bw = new BackgroundWorker();


    // Properties
    bw.WorkerReportsProgress = true;                    // BW can report 
    bw.WorkerSupportsCancellation = true;               // BW supports asynchronous cancellation

    // Events
    bw.DoWork += bwDoWork;                             // Called when RunWorkerAsync is called
    bw.ProgressChanged += bwProgressChanged;           // Called when ReportProgress is called
    bw.RunWorkerCompleted += bwRunWorkerCompleted;     // Called when background operation has completed,
                                                        // has been cancelled or has raised an exception
 
    // Running in asynch mode?
    if (bw.IsBusy)
    {
        Console.WriteLine("BackgroundWorker is running an asynchronous operation");
    }
    else
    {
        Console.WriteLine("BackgroundWorker is NOT running an asynchronous operation");
    }


    // Start execution of a background operation
    bw.RunWorkerAsync ("Hello to worker");

    if (bw.IsBusy)
    {
        Console.WriteLine("BackgroundWorker is running an asynchronous operation");
    }
    else
    {
        Console.WriteLine("BackgroundWorker is NOT running an asynchronous operation");
    }

    Console.WriteLine ("Press Enter in the next 5 seconds to cancel");
    Console.ReadLine();

    if (bw.IsBusy) bw.CancelAsync();    // Request cancellation of pending background operation

    Console.ReadLine();
  }
 
  static void bwDoWork (object sender, DoWorkEventArgs e)
  {
    int Increment = 20;
    for (int i = 0; i <= 100; i += Increment)
    {
      if (bw.CancellationPending)   // Has app requested cancellation of 
                                    // background operation?
      { 
          e.Cancel = true; return; 
      }

      bw.ReportProgress (i);

      Thread.Sleep (1000);      
    }       
                                  
    e.Result = 77;    // This gets passed to RunWorkerCompleted event
  }
 
  static void bwRunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
  {
      if (e.Cancelled)
      {
          Console.WriteLine("You cancelled!");
      }
      else if (e.Error != null)
      {
          Console.WriteLine("Worker exception: " + e.Error.ToString());
      }
      else
      {
          Console.WriteLine("Complete: " + e.Result);      // from DoWork
      }

  }
 
  static void bwProgressChanged (object sender, ProgressChangedEventArgs e)
  {
    Console.WriteLine ("Reached " + e.ProgressPercentage + "%");
  }
}

