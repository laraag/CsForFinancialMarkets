// Example showing chaining queries using 'into'.
//
// The result of one query can be input for another query.
// To write that as one statement, we can use the "into" keyword to chaining the queries.
// Alternatively, we can create a sub query in the 'in' clause.
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
		var persons=new[] { new { Name="Allan", Age=23 }, new { Name="Susan", Age=29 }, new { Name="Andrew", Age=34 }, new { Name="Jane", Age=23 } };
		persons.Print("Persons: "); Console.WriteLine();

		// Two queries joined with "into".
		// First part gets all names of persons younger than 30, the second part only gets names starting with 'A'.
		var query=from person in persons where person.Age<30 select person.Name into name where name.StartsWith("A") select name;
		query.Print("Names of persons younger than 30 and starting with 'A' (into query): ");

		// Same query but 1st part wrapped in the "in" part of the 2nd part.
		// Inner query gets all names of persons younger than 30, the outer query only gets names starting with 'A'.
		var query2=from name in (from person in persons where person.Age<30 select person.Name) where name.StartsWith("A") select name;
		query2.Print("Names of persons younger than 30 and starting with 'A' (in sub query): ");

	}
	
	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}