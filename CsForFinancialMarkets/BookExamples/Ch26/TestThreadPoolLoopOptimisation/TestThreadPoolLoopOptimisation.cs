// TestThreadPoolLoopOptimisation.cs
//
// Using thread pools. Loop-level optimisation using thread pool.
//
// THIS IS A MICROCOSM OF PARALLEL DESIGN PATTERNS IN LATER APPLICATIONS.
// C# MT IS VERY POWERFUL.
//
// ? ON A 2-CORE MACHINE ==> SPEEDUP == 2!!!! run it and see.
//
// (C) Datasim Education BV 2009-2013
//


using System;
using System.Threading;
using System.Diagnostics; // For StopWatch

public class TestMatrixDecomposition
{
    public static void Main()
    {
        // Global data stucture defined by the master
        int N = 10;
        int StartIndex = 1;

        // Create a matrix
        NumericMatrix<double> A = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
        for (int i = A.MinRowIndex; i <= A.MaxRowIndex; ++i)
        {

            for (int j = A.MinColumnIndex; j <= A.MaxColumnIndex; ++j)
            {
                A[i, j] = 1.0;
            }
            A[i, i] = 4.0;
        }

        // Create workers
        int r1 = A.MinRowIndex;
        int r2 = A.MaxRowIndex / 2;

        int NSIM = 1000000;

        Worker w1 = new Worker(A, r1, r2, 5.0, NSIM); 
        Worker w2 = new Worker(A, r2+1, A.MaxRowIndex, 9, NSIM);

        Worker w3 = new Worker(A, r1, A.MaxRowIndex, 7, NSIM);


        // Create threads and start initialising the matrix
        Thread t1 = new Thread(new ThreadStart(w1.Work));
        Thread t2 = new Thread(new ThreadStart(w2.Work));
        Thread t3 = new Thread(new ThreadStart(w3.Work));

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        Console.WriteLine("1) 1 thread, 2) 2 threads: ");
        string choice;
        choice = Console.ReadLine();

        if (choice == "2")
        {
            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }
        else
        {

            t3.Start();
            t3.Join();
        }


       // Thread.Sleep(10000);

        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine(elapsedTime, "RunTime");

        Console.WriteLine("Press the any key to continue: "); Console.ReadLine();        

    //    A.extendedPrint();

        Console.WriteLine("Main thread exits.");
    }
}

    public class Worker
    {
        private int m_l; 
        private int m_u;
        double m_d;

        int NSIM;       // The number of times to execute the parallel loop

        NumericMatrix<double> m_A;

        public Worker (NumericMatrix<double> A, int lower, int upper, double d,
                        int nsimulations)
        {
            m_l = lower;
            m_u = upper;
            m_A = A;
            m_d = d;
            NSIM = nsimulations;
        }


        // This thread procedure performs the task.
        public void Work()
        {
            // Do some heavy computation

            for (int k = 0; k <= NSIM; k++)
            {
                for (int i = m_l; i <= m_u; i++)
                { // You can play around with different values

                    for (int j = m_A.MinColumnIndex; j <= m_A.MaxColumnIndex; j++)
                    {
                        m_A[i, j] = Math.Exp(m_d);
                    }
                    m_A[i, i] = Math.Sqrt(m_d);
                }

            }

        }
    }


