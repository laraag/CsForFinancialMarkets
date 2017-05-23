// ParallelLINQ.cs
// Example showing various operators that return a single value.
// Result can't be used further with LINQ since it is not an IEnumerator<>.
//
// Parallel LINQ (PLINQ) code
//
// (C) Datasim Education BV  2009-2013

using System;
using System.Collections.Generic;
using System.Linq;


static class Program
{
	static void Main(string[] args)
	{
		SingleElementOperators();
		AggregationOperators();
		QuatifierOperators();
	}

	static void SingleElementOperators()
	{

		// First operators are element operators that return single element.
		Console.WriteLine("\n*** Single Element Operators ***\n");

		// Create collection with  numbers.AsParallel().
		int[] numbers = { 1, 4, 2, 7, 4, 7, 9, 8, 6 };
		numbers.AsParallel().Print("Numbers: ");

		// Get the first item.
		Console.WriteLine("First item: {0}",  numbers.AsParallel().First());

		// Get the first item bigger than 6. Exception when not found.
		Console.WriteLine("First item >6: {0}",  numbers.AsParallel().First(x => x>6));

		// Get the first item bigger than 9. Returns default (0) when not found.
		Console.WriteLine("First item >9 or default: {0}",  numbers.AsParallel().FirstOrDefault(x => x>9));

		// Get the last item.
		Console.WriteLine("Last item: {0}",  numbers.AsParallel().Last());

		// Get the last item bigger than 6. Exception when not found.
		Console.WriteLine("Last item >6: {0}",  numbers.AsParallel().Last(x => x>6));

		// Get the last item bigger than 9. Returns default (0) when not found.
		Console.WriteLine("Last item >9 or default: {0}",  numbers.AsParallel().LastOrDefault(x => x>9));

		// Get the 3rd item.
		Console.WriteLine("3rd item: {0}",  numbers.AsParallel().ElementAt(3));

		// Get the single item >8. If more than one result or no result, throws exception.
		Console.WriteLine("Single item >8: {0}",  numbers.AsParallel().Single(x => x>8));

		// Get the single item >9. If more than one result, throws exception. If no result, returns default (0).
		Console.WriteLine("Single item >9 or default if no result: {0}",  numbers.AsParallel().SingleOrDefault(x => x>9));

	}

	static void AggregationOperators()
	{
		// Next operators are aggregation operators that return aggregated result.
		Console.WriteLine("\n\n*** Aggregation Operators ***\n");

		// Create collection with  numbers.AsParallel().
		int[] numbers = { 1, 4, 2, 7, 4, 7, 9, 8, 6 };
		 numbers.AsParallel().Print("Numbers: ");

		// The number of elements (int).
		Console.WriteLine("Count: {0}",  numbers.AsParallel().Count());

		// The number of elements bigger than 5 (int).
		Console.WriteLine("Count > 5: {0}",  numbers.AsParallel().AsParallel().Count(x => x>5));

		// The number of elements (long).
		Console.WriteLine("Long count: {0}",  numbers.AsParallel().LongCount());

		// The number of elements bigger than 5 (long).
		Console.WriteLine("Long count > 5: {0}",  numbers.AsParallel().LongCount(x => x>5));

		// The minimum value.
		Console.WriteLine("Min: {0}",  numbers.AsParallel().Min());

		// The minimum value of the negated collection.
		Console.WriteLine("Min (negated): {0}",  numbers.AsParallel().Min(x => -x));

		// The maximum value.
		Console.WriteLine("Max: {0}",  numbers.AsParallel().Max());

		// The maximum value of the negated collection.
		Console.WriteLine("Max (negated): {0}",  numbers.AsParallel().Max(x => -x));

		// The sum of the elements.
		Console.WriteLine("Sum: {0}",  numbers.AsParallel().Sum());

		// The sum of the negated elements.
		Console.WriteLine("Sum (negated): {0}",  numbers.AsParallel().Sum(x => -x));

		// The average value.
		Console.WriteLine("Average: {0}",  numbers.AsParallel().Average());

		// The average value of the negated elements.
		Console.WriteLine("Average (negated: {0}",  numbers.AsParallel().Average(x => -x));

		// Custom aggregate function that multiplies each element.
		// Lambda is called for each element. x is the element value, y is the previous aggregation value (first is 1 by default unless seed is given).
		Console.WriteLine("Multiplied: {0}",  numbers.AsParallel().Aggregate((x,y) => y*x));

		// Array with persons.
		var persons=new[] { new { Name="Bob", Age=23 }, new { Name="Susan", Age=29 }, new { Name="Andrew", Age=34 } };
		persons.Print("Persons: ");

		// Get average age of persons.
		Console.WriteLine("Average age: {0}", persons.Average(person => person.Age));
	}

	static void QuatifierOperators()
	{
		// Next operators are aggregation operators that return aggregated result.
		Console.WriteLine("\n\n*** Quatifier Operators ***\n");

		// Create collection with  numbers.AsParallel().
		int[] numbers = { 1, 4, 2, 7, 4, 7, 9, 8, 6 };
		 numbers.AsParallel().Print("Numbers: ");

		// Contains a number?
		Console.WriteLine("Contains 3: {0}",  numbers.AsParallel().Contains(3));
		Console.WriteLine("Contains 4: {0}",  numbers.AsParallel().Contains(4));

		// Any elements?
		Console.WriteLine("Any elements: {0}",  numbers.AsParallel().Any());
		Console.WriteLine("Any elements >5: {0}",  numbers.AsParallel().Any(x => x>5));
		Console.WriteLine("Any elements >9: {0}",  numbers.AsParallel().Any(x => x>9));

		// Satisfy all elements a condition?
		Console.WriteLine("All elements >5: {0}",  numbers.AsParallel().All(x => x>5));
		Console.WriteLine("All elements >0: {0}",  numbers.AsParallel().All(x => x>0));
	}

	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}
