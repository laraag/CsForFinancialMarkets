// TestSection6.2.cs
//
// (C) Datasim Education BV  2005-2013

using System;

/// Generic stack test porgram.
class MainClass
{
	/// Test the generic stack + generic delegat.
	static void Main()
	{
		Console.WriteLine("\n\nTesting generic stack");
		TestGenericStack();
	}

	/// Test the generic stack
	public static void TestGenericStack()
	{
		// Create stack
		int size=10;
		GenericStack<double> stack=new GenericStack<double>(size);
        GenericStack<string> stack2 = new GenericStack<string>(size);

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
			for (int i=0; i<=size+5; i++)
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

        // Using Generic methods
        int sz1 = 10; int sz2 = 6;
        GenericStack<double> stackA = new GenericStack<double>(sz1);
        GenericStack<double> stackB = new GenericStack<double>(sz2);

        GenericMethod.Swap<GenericStack<double>>(ref stackA, ref stackB);
        Console.WriteLine("Sizes of stacks: {0} {1}", stackA.Size(), stackB.Size());

        // Swap 2 doubles
        double d1 = 1.2; double d2 = 3.0;
        GenericMethod.Swap<double>(ref d1, ref d2);
        Console.WriteLine("Sizes of stacks: {0} {1}", d1, d2);
	}
}
