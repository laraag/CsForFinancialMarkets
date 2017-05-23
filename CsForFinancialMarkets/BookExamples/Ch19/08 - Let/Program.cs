// Example showing LINQ let keyword.
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
			new { Name="Bob", BirthDate=new DateTime(1940, 12, 1) },
			new { Name="Susan", BirthDate=new DateTime(1938, 10, 11) },
			new { Name="Andrew", BirthDate=new DateTime(1992, 1, 21) },
			new { Name="Daniel", BirthDate=new DateTime(1982, 3, 12) },
			new { Name="Diane", BirthDate=new DateTime(2010, 5, 30) },
		};
		persons.Print("Persons: ");

		// Get all persons with age between 18 and 65.
		// Needs calculation of age two times.
		var query=from person in persons where (DateTime.Now.Year - person.BirthDate.Year) >= 18 && (DateTime.Now.Year - person.BirthDate.Year) <= 65 select person;
		query.Print("\nPersons between 18 and 65 (normal version): ");

		// Using 'let' introduces temporary variable to avoid multiple age calculations.
		var query2=from person in persons let age = DateTime.Now.Year - person.BirthDate.Year where age >= 18 && age <= 65 select person;
		query2.Print("\nPersons between 18 and 65 (let version): ");
	}

	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}
