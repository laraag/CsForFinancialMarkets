// Example showing sub queries.
//
// A LINQ query can contain nested queries/LINQ statements.
// Sometimes using LINQ functions is easier than LINQ syntax or even needed.
//
// (C) Datasim Education BV  2009-2011

using System;
using System.Collections.Generic;
using System.Linq;

static class Program
{
	static void Main(string[] args)
	{
		SimpleSubQuery();
		SubQuery();
	}

	static void SimpleSubQuery()
	{
		Console.WriteLine("\n*** Simple Sub Query ***");

		// Create an array with names.
		string[] names = { "John Smith", "Jane Doe", "Bob Jones", "Susan Baker" };
		names.Print("Names: ");

		// Sort names on last name.
		// 'OrderBy' clause splits string in new sequence of which the last element is used for ordering.
		// But full string is returned by the complete query.
		// We have LINQ functions and LINQ syntax versions.
		var query1 = names.OrderBy(name => name.Split().Last());
		var query2 = from name in names orderby name.Split().Last() select name;
		query1.Print("Sorted on last name (LINQ functions): ");
		query2.Print("Sorted on last name (LINQ syntax): ");
	}

	static void SubQuery()
	{
		Console.WriteLine("\n*** Sub Query ***");

		// Array with persons.
		var persons=new[] { new { Name="Bob", Age=23 }, new { Name="Susan", Age=29 }, new { Name="Andrew", Age=34 }, new { Name="Jane", Age=23 } };
		persons.Print("Persons: "); Console.WriteLine();

		// Select all persons with the lowest age.
		// The 'Where' clause has a sub query getting the lowest age which is used to filter on in the 'Where' clause.
		// The 'OrderBy' clause needs a different lambda variable name because it is nested in the 'Where' and thus conflicts 
		// with the 'Where' clause's lambda variable if the would have the same name.
		var query1=persons.Where(person => person.Age == persons.OrderBy(person2 => person2.Age).Select(person2 => person2.Age).First());
		var query2=from person in persons where person.Age == (from person2 in persons orderby person2.Age select person2.Age).First() select person;
		var query3=from person in persons where person.Age == persons.Min(person2 => person2.Age) select person;
		query1.Print("Youngest persons (LINQ functions): ");
		query2.Print("Youngest persons (LINQ syntax): ");
		query3.Print("Youngest persons (simplified): ");

		// Previously, the sub query is evaluated every iteration of the main query.
		// Better to optimize in two separate queries. 
		// (NOTE, when it was a LINQ query to a database, then it would be converted to a single SQL statement 
		// that is executed optimized in the database)
		int youngestAge=persons.Min(person => person.Age);	// Get the youngest age.
		var query4=from person in persons where person.Age == youngestAge select person;
		query4.Print("Youngest persons (optimized): ");

	}

	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}
