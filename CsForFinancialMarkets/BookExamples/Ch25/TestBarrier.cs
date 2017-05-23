// TestBarrier.cs
//
// Testing barriers; tasks are synchronised at the end of phases. 
//
// (C) Datasim Education BV 2009-2013
//
using System;
using System.Threading;
using System.Threading.Tasks;        // For parallel tasks, e.g. Parallel for    

class MyBarrier
{
    const int N = 3;
    const int StartIndex = 0;

    // Create DATASIM EDUCATION BV matrices
    static NumericMatrix<double> mat1 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
    static NumericMatrix<double> mat2 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
    static NumericMatrix<double> mat3 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);

    // The storage (array of matrices)
    static int Size = 2;
    static Array<NumericMatrix<double> > container 
                = new Array<NumericMatrix<double> >(Size, StartIndex);
   
    static void Main()
    {
        
        container[container.MinIndex]  = mat1;
        container[container.MinIndex+1] = mat2;
  
      
        int NTasks = 2;
        Barrier b = new Barrier(NTasks);

        Task[] tasks = new Task[NTasks];
        for (int i = 0; i < NTasks; i++)
        {
            int index = i;
            Console.WriteLine("{0} ", index);
        
            tasks[i] = Task.Factory.StartNew(() =>
            {

                // Fill each matrix, then wait.
                InitMatrices(index);
                b.SignalAndWait();
   
            });
        }

        // Everyone waits here
        ComputeSum(); // sequential code
        mat1.extendedPrint();
        mat2.extendedPrint();
        mat3.extendedPrint();

        Console.WriteLine("Press a key to exit");
        Console.ReadKey();
    }

  
    static void InitMatrices(int index)
    {
        Console.WriteLine("index {0}",index); Console.ReadLine();
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                container[index][j, i] = 2.0;
               
            }
        }
     }



    static void ComputeSum()
    {

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                mat3[j, i] = mat1[j, i] + mat2[j, i];
            }
        }

      //  mat3.extendedPrint();
    }

}