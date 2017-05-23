// EnumerateMethods.cs
//
// Demonstrates some methods and properties of the Type class, e.g.:
//	* 'Name' property (the name of the type)
//	* 'IsClass' and 'IsInterface', says something about the declaration of the type.
//	* 'GetMethods()', gives info about the methods of the type.
//
// (C) Datasim Education BV  2002-2004


using System;
using System.Reflection;

// Simple valve class.
public class Valve
{
	public void Open()  {}
	public void Close() {}
}


// Main class
public class EnumerateMethods
{
	public static void Main()
	{
		// Get type object.
		Type t=typeof(Valve);

		// Display name of the type.
		Console.WriteLine("Reflecting on {0}", t.Name);
        
		// Is it a class and not an interface?
		if (t.IsClass) Console.WriteLine("It's a class.");
		if (t.IsInterface) Console.WriteLine("It's an interface.");
		if (t.IsPublic) Console.WriteLine("It's declared public.");


		// Display method names.
		Console.WriteLine("\nMethods of {0}:", t.Name);
		MethodInfo[] methodInfoArr=t.GetMethods();
		foreach (MethodInfo methodInfo in methodInfoArr)
		{
			// Display method.
			Console.WriteLine("- {0}", methodInfo.Name);
		}
	}
}