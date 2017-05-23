// EnumerateTypes.cs
//
// Demonstrates the use of the Reflection API to
// traverse the modules and types contained in
// the assembly.
//
// (C) Datasim Education BV  2002-2013


using System;
using System.Reflection;

public class EnumerateTypes
{
	public static void Enumerate()
	{
		try
		{			
			// Get assembly of main executable.
			Assembly assembly=Assembly.GetEntryAssembly();
			
			// Get all the modules from the assembly.
			Module[] modules=assembly.GetModules();

			// Traverse modules.
			foreach (Module module in modules)
			{
				// Get all the types from the module.
				Type[] types=module.GetTypes();

				// Traverse types.
				foreach (Type type in types)
				{
					// Write full name of the type.
					Console.WriteLine(type.FullName);		
				}
			}
		}
		catch(Exception e)
		{
			Console.WriteLine(e);
		}
	}
}