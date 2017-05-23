// Program.cs
//
// (C) Datasim Education BV  2005-2013

using System;

/// <summary>
/// Generic stack test porgram.
/// </summary>
class Program
{
	static void Main()
	{
		Console.WriteLine("Testing double stack");
		TestDoubleStack();
		Console.WriteLine("\n\nTesting generic stack");
		TestGenericStack();
	}

	/// <summary>
	/// Test the double stack.
	/// </summary>
	public static void TestDoubleStack()
	{
		// Create stack
		int size=10;
		DoubleStack stack=new DoubleStack(size);

		// Push elements on the stack
		try
		{
			for (int i=0; i<=size; i++)
			{
				stack.Push(i);
				Console.WriteLine("Push: {0}", i);
			}
		}
		catch (ApplicationException ex)
		{
			Console.WriteLine("Error while pushing values on the stack: {0}", ex.Message);
		}

		// Pop elelments from the stack
		try
		{
			while (true)
			{
				Console.WriteLine("Pop: {0}", stack.Pop());
			}
		}
		catch (ApplicationException ex)
		{
			Console.WriteLine("Error while poping values from the stack: {0}", ex.Message);
		}

	}

	/// <summary>
	/// Test the generic stack.
	/// </summary>
	public static void TestGenericStack()
	{
		// Create stack
		int size=10;
		GenericStack<double> stack=new GenericStack<double>(size);

		// Push elements on the stack
		try
		{
			for (int i=0; i<=size; i++)
			{
				stack.Push(i);
				Console.WriteLine("Push: {0}", i);
			}
		}
		catch (ApplicationException ex)
		{
			Console.WriteLine("Error while pushing values on the stack: {0}", ex.Message);
		}

		// Pop elements from the stack
		double total=0.0;
		try
		{
			while (true)
			{

				// Note, no casting needed.
				double value=stack.Pop();
				total+=value;
				Console.WriteLine("Pop: {0}", value);
			}
		}
		catch (ApplicationException ex)
		{
			Console.WriteLine("Error while poping values from the stack: {0}", ex.Message);
		}

		Console.WriteLine("Total: {0}", total);
	}

}
