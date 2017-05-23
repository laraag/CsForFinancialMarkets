// TestNestedLocking.cs
//
// Using nested locking to give improved concurrency. This example
// simulates a repository/Blackboared of large (heterogeneous)data sets.
//
// (C) Datasim Education BV 2009-2013
//
using System;
using System.Threading;

public class NestedLockingSample
{
    static void Main()
    {

        int N = 100;
        int StartIndex = 0;

        // Create matrices
        NumericMatrix<double> mat1 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
        NumericMatrix<double> mat2 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
        NumericMatrix<double> mat3 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);

        for (int i = mat1.MinColumnIndex; i <= mat1.MaxColumnIndex; i++)
        {
            for (int j = mat1.MinRowIndex; j <= mat1.MaxRowIndex; j++)
            {
                mat1[j, i] = 2.0;
                mat2[j, i] = 3.0;
                mat3[j, i] = 3.0;
            }
        }

        RecursiveDataStructure myData = new RecursiveDataStructure(mat1, mat2, mat3);

        // Create threads and generate the random numbers
        Thread t1 = new Thread(new ThreadStart(myData.Compute_Mat1));
        Thread t2 = new Thread(new ThreadStart(myData.Compute_Mat2));

        // Fire up the two threads
        t1.Start();
        t2.Start();

        // Block calling thread until the others have completed
        // All threads wait here for all other threads to arrive
        t1.Join();
        t2.Join();

        myData.Compute_Sum();

        Console.WriteLine("Done.");
    }
}

public class RecursiveDataStructure
{ // A class containing 3 updatable matrices

    private NumericMatrix<double> m1;
    private NumericMatrix<double> m2;
    private NumericMatrix<double> m3;

    // Define the locks
    object lock1;
    object lock2;


    public RecursiveDataStructure(NumericMatrix<double> mat1, NumericMatrix<double> mat2, NumericMatrix<double> mat3)
    {
        m1 = mat1;
        m2 = mat2;
        m3 = mat3;

        lock1 = new object();
        lock2 = new object();
    }


    public void Compute_Mat1()
    {
       // Initialise matrix 1

        lock (lock1)
        {
            for (int i = m1.MinColumnIndex; i <= m1.MaxColumnIndex; i++)
            {
                for (int j = m1.MinRowIndex; j <= m1.MaxRowIndex; j++)
                {
                    m1[j, i] = 0.0;
                }
            }
        }
    }

    public void Compute_Mat2()
    {
        // Initialise matrix 2

        lock (lock2)
        {
            for (int i = m1.MinColumnIndex; i <= m1.MaxColumnIndex; i++)
            {
                for (int j = m1.MinRowIndex; j <= m1.MaxRowIndex; j++)
                {
                    m2[j, i] = 0.0;
                }
            }
        }
    }


    public void Compute_Sum()
    {
        // Initialise matrix 2

        // Option 1: lock current object
      lock (this) // Lock the full object
        {
            for (int i = m1.MinColumnIndex; i <= m1.MaxColumnIndex; i++)
            {
                for (int j = m1.MinRowIndex; j <= m1.MaxRowIndex; j++)
                {
                    m3[j, i] = m1[j, i] + m2[j, i];
                }
            }
        }
   

       // Option 2: Lock all data
        lock (lock1)
        {
            lock (lock2)
            {

                for (int i = m1.MinColumnIndex; i <= m1.MaxColumnIndex; i++)
                {
                    for (int j = m1.MinRowIndex; j <= m1.MaxRowIndex; j++)
                    {
                        m3[j, i] = m1[j, i] + m2[j, i];
                    }
                }

            }
        }
    }

}
