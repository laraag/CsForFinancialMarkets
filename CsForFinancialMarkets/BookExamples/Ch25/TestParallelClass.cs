// TestParallelClass.cs
// Example showing data parallelism using TPL.
//
// (C) Datasim Education BV  2011-2013

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;

class Program
{
	static void Main(string[] args)
	{

		// Display number of cores.
		Console.WriteLine("Processors/cores: {0}", System.Environment.ProcessorCount);

		int choice=0;

		do
		{
			Console.WriteLine("*** Menu ***\n");
			Console.WriteLine("1. Parallel ForEach");
			Console.WriteLine("2. Parallel For");
			Console.WriteLine("3. Aborting loop");
			Console.WriteLine("4. Cancelling loop");
			Console.WriteLine("5. Thread-local storage");
			Console.WriteLine("6. Exception handling");
            Console.WriteLine("7. Parallel Invoke");
			Console.Write("\nChoice (0=exit): ");

			choice=0;
			if (Int32.TryParse(Console.ReadLine(), out choice)==false) continue;
			Console.WriteLine("\n");

			switch (choice)
			{
				case 0: break;
				case 1: ParallelForEach(); break;
				case 2: ParallelFor(); break;
				case 3: AbortingLoop(); break;
				case 4: CancellingLoop(); break;
				case 5: ThreadLocalStorage(); break;
				case 6: ExceptionHandling(); break;
                case 7: ParallelInvoke(); break;
				default: Console.WriteLine("Unknown option"); break;
			}

			Console.WriteLine("\n\n");
		}
		while (choice!=0);
	}

	// Parallel ForEach example.
	static void ParallelForEach()
	{
		Console.WriteLine("*** Parallel ForEach ***");

		// Create stop watch.
		System.Diagnostics.Stopwatch sw=new System.Diagnostics.Stopwatch();

		// The size of the array.
		const int size=10000000;

		// Create array of 'Data' objects to process.
		// Data object has Process() function that takes long to execute.
		Console.WriteLine("Creating array...");
		Data[] values=new Data[size];
		for (int i=0; i!=size; i++) values[i]=new Data((double) i, 10);

		// Sequential version.
		Console.WriteLine("\nSequential calculation...");
		sw.Restart();
		foreach (Data data in values) data.Process();
		sw.Stop();
		TimeSpan ts1=sw.Elapsed;
		Console.WriteLine("Sequential version: {0}", ts1);

		// Parallel version.
		Console.WriteLine("\nParallel calculation...");
		sw.Restart();
		Parallel.ForEach(values, data => data.Process());
		sw.Stop();
		TimeSpan ts2=sw.Elapsed; 
		Console.WriteLine("Parallel version: {0}", ts2);
		Console.WriteLine("Speed up: {0:##.00%}", (double)ts1.Ticks/(double)ts2.Ticks);
	}

	// Parallel For example.
	static void ParallelFor()
	{
		Console.WriteLine("*** Parallel For ***");

		// Create two matrices.
		// Columns m1==Rows m2.
		double[,] m1={{14, 9, 3, 6, 8}, {2, 11, 15, 3, 6}, {0, 12, 17, 23, 5}, {5, 2, 3, 5, 1}};
		double[,] m2={{12, 25, 5}, {9, 10, 9}, {8, 5, 1}, {4, 5, 5}, {6, 1, 8}};

		Console.WriteLine("Matrices:");
		Print(m1);
		Print(m2);

		// Create stop watch.
		System.Diagnostics.Stopwatch sw=new System.Diagnostics.Stopwatch();

		// Sequential processing.
		Console.WriteLine("\nSequential calculation...");
		sw.Restart();
		double[,] r1=MatrixMultiplySequential(m1, m2);
		sw.Stop();
		TimeSpan ts1=sw.Elapsed;
		Console.WriteLine("Sequential version: {0}", ts1);

		// Parallel version.
		Console.WriteLine("\nParallel calculation...");
		sw.Restart();
		double[,] r2=MatrixMultiplySequential(m1, m2);
		sw.Stop();
		TimeSpan ts2=sw.Elapsed;
		Console.WriteLine("Parallel version: {0}", ts2);
		Console.WriteLine("Speed up: {0:##.00%}", (double)ts1.Ticks/(double)ts2.Ticks);

		// Print results.
		Console.WriteLine("\nResults:");
		Print(r1);
		Print(r2);
	}

	// Matrix multiplication (sequential version).
	// Multiply two matrices. The number of columns of matrix 1 must be the same as the number of rows of matrix 2.
	// Result dimensions: result[m1.rows, m2.columns]
	// Matrix multiplication: (AB)ij = Air.Brj = Ai1*B1j + Ai2*B2j + ... + Ain*Nnj
	// result[r,c] = m1[r,0]*m2[0,c] + m1[r,1]*m2[1,c] + ... + m1[r,n-1]*m2[n-1,c]
	static double[,] MatrixMultiplySequential(double[,] m1, double[,] m2)
	{
		// Can the matrices be multiplied.
		// Number of columns first matrix must be the same as number of rows 2nd matrix
		if (m1.GetLength(1)!=m2.GetLength(0)) throw new ApplicationException("Matrix multiplication: The number of columns of matrix 1 must be the same as the number of rows of matrix 2.");

		// Get the number of rows (of m1) & columns (of m2).
		int rows=m1.GetLength(0);
		int columns=m2.GetLength(1);

		// Number of columns of m1 is the number of elements to multiply and add each iteration.
		int n=m1.GetLength(1);

		// Create result matrix.
		double[,] result=new double[rows, columns];

		// Perform multiplication to result matrix.
		for (int row=0; row<rows; row++)
		{
			for (int column=0; column<columns; column++)
			{
				double tmp=0.0;			// Temporary variable to avoid many [,] operations.
				for (int k=0; k<n; k++) tmp+=m1[row, k] * m2[k, column];
				result[row, column]=tmp;
			}
		}

		// Return the result.
		return result;
	}

	// Matrix multiplication (parallel version).
	// Multiply two matrices. The number of columns of matrix 1 must be the same as the number of rows of matrix 2.
	// Result dimensions: result[m1.rows, m2.columns]
	// Matrix multiplication: (AB)ij = Air.Brj = Ai1*B1j + Ai2*B2j + ... + Ain*Nnj
	// result[r,c] = m1[r,0]*m2[0,c] + m1[r,1]*m2[1,c] + ... + m1[r,n-1]*m2[n-1,c]
	static double[,] MatrixMultiplyParallel(double[,] m1, double[,] m2)
	{
		// Can the matrices be multiplied.
		// Number of columns first matrix must be the same as number of rows 2nd matrix
		if (m1.GetLength(1)!=m2.GetLength(0)) throw new ApplicationException("Matrix multiplication: The number of columns of matrix 1 must be the same as the number of rows of matrix 2.");

		// Get the number of rows (of m1) & columns (of m2).
		int rows=m1.GetLength(0);
		int columns=m2.GetLength(1);

		// Number of columns of m1 is the number of elements to multiply and add each iteration.
		int n=m1.GetLength(1);

		// Create result matrix.
		double[,] result=new double[rows, columns];

		// Perform multiplication to result matrix.
		// Do the outer loop in parallel.
		Parallel.For(0, rows, row =>
		{
			for (int column=0; column<columns; column++)
			{
				double tmp=0.0;			// Temporary variable to avoid many [,] operations.
				for (int k=0; k<n; k++) tmp+=m1[row, k] * m2[k, column];
				result[row, column]=tmp;
			}
		});

		// Return the result.
		return result;
	}

	// Aborting parallel loop example.
	static void AbortingLoop()
	{
		Console.WriteLine("*** Aborting Loop ***");

		// The size of the array.
		const int size=1000;

		// Create array of 'Data' objects to process.
		// Data object has Process() function that takes long to execute.
		Console.WriteLine("Creating array...");
		Data[] values=new Data[size];
		for (int i=0; i!=size; i++) values[i]=new Data(i, 10);

		// Create a queue for the results.
		ConcurrentStack<double> stack=new ConcurrentStack<double>();

		// Parallel loop result.
		ParallelLoopResult result;

		// Parallel "for" with break storing the loop result.
		Console.WriteLine("\nParallel for with break...");
		result=Parallel.For(0, size, (index, loopState) =>
		{
			// Process element.
			values[index].Process();
			
			// Only push result when result<100, else break loop.
			if (values[index].Result<100)
			{
				stack.Push(values[index].Result);
			}
			else
			{
				// Break the loop while finishing current iterations.
				Console.WriteLine("Breaking loop: {0}", index);
				loopState.Break();

				// Lowest break iteration can be lower if other thread had a lower breaking iteration.
				// It can also become lower later when a lower iteration thread end later.
				Console.WriteLine("Lowest break iteration: {0}", loopState.LowestBreakIteration);
			}
		});

		// Print the number of elements.
		Console.WriteLine("Loop finished. Result stack size: {0}", stack.Count);
		Console.WriteLine("Is completed: {0} - Lowest break iteration: {1}", result.IsCompleted, result.LowestBreakIteration);
		stack=new ConcurrentStack<double>();

		// Parallel "for" with stop storing the loop result.
		Console.WriteLine("\nParallel for with stop...");
		result=Parallel.For(0, size, (index, loopState) =>
		{
			// Not always the environment can't stop the loop fast. 
			// So you can exit the loop yourself by checking the loop state.
			if (loopState.ShouldExitCurrentIteration)
			{
				Console.WriteLine("Exited iteration: {0}", index);
				return;
			}

			// Process element.
			values[index].Process();

			// Only push result when result<100, else stop loop.
			if (values[index].Result<100)
			{
				stack.Push(values[index].Result);
			}
			else
			{
				// Stop the loop as soon as possible.
				Console.WriteLine("Stopping loop: {0}", index);
				loopState.Stop();
			}
		});
		Console.WriteLine("Is completed: {0} - Lowest break iteration: {1}", result.IsCompleted, result.LowestBreakIteration);

		// Print the number of elements.
		Console.WriteLine("Loop finished. Result stack size: {0}", stack.Count);
	}

	// Cancelling parallel loop example.
	static void CancellingLoop()
	{
		Console.WriteLine("*** Cancelling Parallel Loops ***");

		// The size of the array.
		const int size=10000000;

		// Create array of 'Data' objects to process.
		// Data object has Process() function that takes long to execute.
		Console.WriteLine("Creating array...");
		Data[] values=new Data[size];
		for (int i=0; i!=size; i++) values[i]=new Data(i, 10);

		// Create a CancellationTokenSource that manages cancellation token.
		CancellationTokenSource cts=new CancellationTokenSource();

		// Use ParallelOptions instance to store the CancellationToken.
		ParallelOptions po=new ParallelOptions();
		po.CancellationToken=cts.Token;
		
		// Create the thread that can cancel the operation.
		Thread t=new Thread(() =>
		{
			// Wait for key and cancel via cancellation token.
			Console.WriteLine("Processing, press any key to cancel...");
			Console.ReadKey(); 
			cts.Cancel();
		});
		t.Start();

		// Parallel for each.
		// ParallelOptions object passed with cancellation token. Throws exception when cancelled.
		try
		{
			Parallel.ForEach(values, po, data => data.Process());
			Console.WriteLine("Finished processing");
		}
		catch (OperationCanceledException e)
		{
			Console.WriteLine(e.Message);
		}


	}

	// Thread-local storage example.
	static void ThreadLocalStorage()
	{
		Console.WriteLine("*** Thread-Local Storage ***");

		// Create array with 1000 numbers {0, 1, 2, 3, ...}.
		int[] values=Enumerable.Range(0, 1000).ToArray();

		// Variable for the sum.
		int total = 0;

		// Calculate the sum using a parallel for loop.
		// We need to give the generic argument to specify the type of the thread-local variable.
		Console.WriteLine("Parallel for:");
		Parallel.For<int>
		(
			0, values.Length,					// The range for the loop.

			() => 0,							// Thread local-variable initialization lambda.

			(index, loopState, subTotal) =>		// Loop function lambda (current index, parallel loop state, thread local variable).
			{
				subTotal += values[index];		// Calculate new sub total.
				return subTotal;				// Return the new sub total for the next iteration.
			},

			(subTotal) =>						// Aggregation lambda.
			{
				Console.WriteLine("- Sub total thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, subTotal);
				Interlocked.Add(ref total, subTotal);	// thread-safe adding
			}
		);
		Console.WriteLine("The total is {0}", total);


		// Reset total to zero.
		total=0;

		// Calculate the sum using a parallel for-each loop.
		// First generic argument is the type in the collection, second is the type of the thread-local variable.
		Console.WriteLine("Parallel for-each:");
		Parallel.ForEach<int, int>
		(
			values,								// The collection to iterate.

			() => 0,							// Thread local-variable initialization lambda.

			(value, loopState, subTotal) =>		// Loop function lambda (current value, parallel loop state, thread local variable).
			{
				subTotal += value;				// Calculate new sub total.
				return subTotal;				// Return the new sub total for the next iteration.
			},

			(subTotal) =>						// Aggregation lambda.
			{
				Console.WriteLine("- Sub total thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, subTotal);
				Interlocked.Add(ref total, subTotal);	// thread-safe adding
			}
		);

		Console.WriteLine("The total is {0}", total);

	}

	// Exception handling example.
	// Exceptions within the loop must be handled inside the loop because they are run in a separate thread.
	// (exception must be handled in the thread where they are thrown).
	// Since parts of the loop are run by different threads, multiple exceptions can occur while running the loop.
	// The exceptions must be transfered to the creator of the parallel loop. This is possible by storing the exceptions
	// in the thread-safe data structure. Exception can then be aggregated in a AggregatedException object and thrown.
	static void ExceptionHandling()
	{
		Console.WriteLine("*** Thread-Local Storage ***");

		int size=1000;

		// Create array of 'Data' objects to process.
		// Data object has Process() function that takes long to execute.
		Console.WriteLine("Creating array...");
		Data[] values=new Data[size];
		for (int i=0; i!=size; i++) values[i]=new Data(i, 10);

		try
		{
			CauseParallelException(values);
		}
		catch (AggregateException ex)
		{
			Console.WriteLine(ex.Message);
			foreach (Exception e in ex.InnerExceptions)
			{
				Console.WriteLine("- {0}", e.Message);
			}
		}
	}

	// Function causing exceptions in a parallel loop. Used by ExcpetionHandle() example.
	static void CauseParallelException(Data[] values)
	{
		// Thread-safe data structure for storing exceptions.
		ConcurrentQueue<Exception> exceptions=new ConcurrentQueue<Exception>();

		// Run loop in parallel.
		Parallel.ForEach(values, value =>
		{
			// Exceptions must be handled in the loop.
			try
			{
				// Throw exception more or less randomly.
				int threadID=Thread.CurrentThread.ManagedThreadId;
				if (threadID%2==1) throw new ApplicationException(String.Format("Exception in thread: {0}", threadID));
				else value.Process();
			}
			catch (Exception ex)
			{
				// Store the exception in the thread-safe data structure.
				exceptions.Enqueue(ex);
			}
		});

		// Aggregate the exceptions and throw.
		if (exceptions.Count>0) throw new AggregateException("Exception(s) in parallel loop", exceptions);
	}

	// Print a matrix.
	static void Print(double[,] m)
	{
		string result="Matrix (";

		int rows=m.GetLength(0);
		int columns=m.GetLength(1);
		for (int row=0; row<rows; row++) 
		{
			result+="[";
			for (int column=0; column<columns; column++) result+=m[row, column] + ", ";
			result=result.Remove(result.Length-2, 2);
			result+="] ";
		}
			
		result=result.Remove(result.Length-1, 1);
		result+=")";

		Console.WriteLine(result);
	}

    // DD examples, Parallel Invoke
    public static void ParallelInvoke()
    {
        Parallel.Invoke(
            () => new WebClient().DownloadFile("http://www.datasim.nl", "Education/Courses.asp"),
            () => new WebClient().DownloadFile("http://www.datasim.nl", "Education/CoursesExpanded.asp") );
    }
    
}

