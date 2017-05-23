// Example showing LINQ grouping.
// 
// Let create an additional variable within a LINQ query.
//
// (C) Datasim Education BV  2009-2011

using System;
using System.Collections.Generic;
using System.Linq;

static class Program
{
	static void Main(string[] args)
	{
		// Array with persons.
		var persons=new[] 
		{ 
			new { Name="Dennis", Age=35 },
			new { Name="Bob", Age=30 },
			new { Name="Daniel", Age=35 },
			new { Name="Susan", Age=30 },
			new { Name="Andrew", Age=30 },
			new { Name="Diane", Age=35 },
		};
		persons.Print("Persons: ");

		// Group all persons by age and sort on age and name.
		var query=from person in persons orderby person.Age, person.Name group person by person.Age;
		
		// Display the result.
		// First iterate the groups.
		// Then iterate the elements in the group.
		foreach (var group in query)
		{
			Console.WriteLine("\nGroup: {0}", group.Key);
			foreach (var person in group) Console.WriteLine("- {0}", person);
		}

	}

	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}
