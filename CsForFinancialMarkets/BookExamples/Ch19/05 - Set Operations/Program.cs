// Example showing LINQ set operations.
//
// (C) Datasim Education BV  2009-2011

using System;
using System.Collections.Generic;
using System.Linq;


static class Program
{
	static void Main(string[] args)
	{
		// Create two arrays with numbers.
		int[] a1= { 1, 5, 3, 9, 1 };
		int[] a2= { 8, 3, 5, 7 };

		a1.Print("Array 1: ");
		a2.Print("Array 2: ");

		// Combine the arrays.

		// Concatenation. Elements can occur multiple times.
		a1.Concat(a2).Print("a1.Concat(a2): ");
		
		// Union (all unique elements from either set).
		a1.Union(a2).Print("a1.Union(a2): ");

		// Intersection (only elements existing  in both sets).
		a1.Intersect(a2).Print("a1.Intersect(a2): ");

		// Difference (only elements existing  in one set and not both).
		a1.Except(a2).Print("a1.Except(a2): ");
	}

	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}