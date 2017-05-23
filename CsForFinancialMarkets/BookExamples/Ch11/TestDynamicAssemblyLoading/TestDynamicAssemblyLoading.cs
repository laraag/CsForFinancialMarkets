// DynamicAssemblyLoading.cs
//
// Demostrates how to dynamically load an assembly.
//
// (C) Datasim Education BV  2002-2013

using System;
using System.Reflection;


// Class containing test method.
class DynamicAssemblyLoading
{
	public static void Main(string[] args)
	{
		// Check arguments
		if (args.Length==0) 
		{
			Console.WriteLine("Usage: DynamicMethodInvocation DistanceCalculationAlgorithm.dll");
			return;
		}

		try
		{
			// Load the algorithm
			Point.DistanceAlgorithm=LoadDistanceAlgorithm(args[0]);

			Point p1=new Point(10.0, 10.0);
			Point p2=new Point(20.0, 15.0);

			Console.WriteLine("p1.Distance(p2): {0}", p1.Distance(p2));
		}
		catch (Exception e)
		{
			Console.WriteLine("Error: {0}", e.Message);
		}
	}

	// Load distance algorithm
	private static ICalculateDistance LoadDistanceAlgorithm(string assemblyFile)
	{
		// Load the specified assembly
		Assembly ass=Assembly.LoadFrom(assemblyFile);

		// Get all the types from the assembly and 
		// find the class that implements our interface
		foreach (Type t in ass.GetExportedTypes()) 
		{
			if (t.IsClass)
			{
				foreach (Type i in t.GetInterfaces()) 
				{
					// Class found so create instance
					if (i==typeof(ICalculateDistance)) return (ICalculateDistance)Activator.CreateInstance(t);
				}
			}
		}

		// Not found
		return null;
	}
}