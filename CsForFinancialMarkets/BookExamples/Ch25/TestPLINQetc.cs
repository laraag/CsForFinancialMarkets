// Task.cs
//
// Some experiments with TPL.
//
// (C) Datasim Education BV  2002-2011

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent; // Parallel collections here
using System.Threading.Tasks;
using System.Linq;

public class MainClass
{
    
     static double CreateMatrix(object val)
     {
         const int N = 3000;
         const int StartIndex = 0;

         NumericMatrix<double> mat1 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
         mat1[mat1.MinRowIndex, mat1.MinColumnIndex] = Convert.ToDouble(val);

         return mat1[mat1.MinRowIndex, mat1.MinColumnIndex];
     }

     public static void Main()
     {
         // 1. Create a task
         Task.Factory.StartNew(() => Console.WriteLine("A task is born"));

         // 2. Create a task with a return value
         Task<double> myTask = Task.Factory.StartNew<double>(() =>
         {
             const int N = 3000;
             const int StartIndex = 0;

             NumericMatrix<double> mat1 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
             mat1[mat1.MinRowIndex, mat1.MinColumnIndex] = 3.141;

             return mat1[mat1.MinRowIndex, mat1.MinColumnIndex];
         });

         // Can do other work in parallel here

         double result = myTask.Result;
         Console.WriteLine("Result of task {0}", result);

         // 2A. Using methods instead of lambda functions
         double val = 2.71;
         Task<double> myTaskA = Task.Factory.StartNew<double>(CreateMatrix, val);
         myTaskA.Wait();

         // Can do other work in parallel here

         double resultA = myTaskA.Result;
         Console.WriteLine("Result of task {0}", resultA);

         // 3. Decouple creation and start of a thread
         Task myTask2 = new Task(() => Console.WriteLine("; indeed, I am a delayed thread"));

         Console.Write("I'm first and you are a delayed thread");

         myTask2.RunSynchronously(); // Run synchronously on the same thread

         // 4. Task creation options
         var parent = new Task(() =>
         {

             Console.WriteLine("parent");

             Task detached = Task.Factory.StartNew(() =>
             {
                 Console.WriteLine("detached");

             });

             Task<double> child = Task.Factory.StartNew<double>(() =>
             {  // Child has to wait on the parent task to complete

                 Console.WriteLine("child");
                 const int N = 300;
                 const int StartIndex = 0;

                 NumericMatrix<double> mat1 = new NumericMatrix<double>(N, N, StartIndex, StartIndex);
                 mat1[mat1.MinRowIndex, mat1.MinColumnIndex] = 3.141;

                 return mat1[mat1.MinRowIndex, mat1.MinColumnIndex];
             }, TaskCreationOptions.AttachedToParent);

             Task detached2 = Task.Factory.StartNew(() =>
             {
                 Console.WriteLine("detached2");

             });

         });

         parent.RunSynchronously();

         // 5. Continuation
         Task t1 = Task.Factory.StartNew(() => Console.Write("before "));
         Task t2 = t1.ContinueWith(ant => Console.WriteLine("after"));


         // 6. Chained continuations, for computationally intensive operations
         Task.Factory.StartNew<double>( () => 2.0) 
             .ContinueWith (ant => ant.Result * 2.0)          // 4.0
             .ContinueWith (ant => Math.Sqrt(ant.Result))     // 2.0
             .ContinueWith (ant => ant.Result * ant.Result)   // 4.0
             .ContinueWith (ant => ant.Result / ant.Result)   // 1.0
             .ContinueWith(ant => Console.WriteLine("Result of task is:  {0}", ant.Result)); // 1.0
         
         // 7. PLINQ
         int M = 20;
         IEnumerable<int> numbers = Enumerable.Range(1, M);

         var parallelQuery = 
                              from n in numbers.AsParallel()
                              where Enumerable.Range(2, (int) Math.Sqrt(n)).All (i => n % i > 0)
                              select n;

         int [] array = parallelQuery.ToArray();
         for (int n = 0; n < array.Length; n++)
         {
             Console.Write(", {0}", array[n]);
         }

         char[] myArray = "abcde".AsParallel().Select(c => char.ToUpper(c)).ToArray();
         for (int n = 0; n < myArray.Length; n++)
         {
             Console.Write(", {0}", myArray[n]);
         }

         // Exception Handling
         try
         {
             var query = from n in Enumerable.Range(0, 1000000)
                         select 100/n;
   
             // ...
         }
      /*   catch (DivideByZeroException e)
         {
             Console.WriteLine("Sequential catch");
             Console.WriteLine(e.Message);
         }*/
         catch (AggregateException e)
         {
             Console.WriteLine("Parallel catch");

             foreach(Exception ex in e.InnerExceptions)
                 Console.WriteLine(ex.Message);
         }

         Console.WriteLine("End of program");

     }
    }