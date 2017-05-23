// TestStopWatch.cs
//
// StopWatch feature in .NET.
//
// (C) Datasim Education BV 2009-2012
//


using System;
using System.Diagnostics; // For StopWatch

public class TestStopWatch
{
    public static void Main()
    {
      
        // Create and start the stopwatch
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        int N = 10000;
        int StartIndex = 1;

        // Create a matrix
        NumericMatrix<double> A = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
        for (int i = A.MinRowIndex; i <= A.MaxRowIndex; ++i)
        {

            for (int j = A.MinColumnIndex; j <= A.MaxColumnIndex; ++j)
            {
                A[i, j] = Math.Sqrt((double)(i+j));
            }
            A[i, i] = 4.0 * Math.Sqrt((double)(i + i));
        }

        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("Elapsed time {0:00}:{1:00}:{2:00}.{3:00}",
                                            ts.Hours, ts.Minutes, ts.Seconds,
                                             ts.Milliseconds / 10);
        Console.WriteLine(elapsedTime, "RunTime");
    }
}

 