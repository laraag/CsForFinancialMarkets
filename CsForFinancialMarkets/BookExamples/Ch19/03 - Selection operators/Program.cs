// Example showing various operators that return a selection.
//
// (C) Datasim Education BV  2009-2011

using System;
using System.Collections.Generic;
using System.Linq;


static class Program
{
	static void Main(string[] args)
    {
        // Create collection with numbers.
        int[] numbers = { 1, 4, 2, 7, 4, 7, 9, 8, 6 };
        numbers.Print("Numbers: ");

        // Take only the first four elements.
        numbers.Take(4).Print("First 4 elements: ");

        // Start taking elements as long they are smaller than 5.
        numbers.TakeWhile(x => x < 5).Print("Elements taken until it encounters element >=5: ");

        // Skip the first 2 elements.
        numbers.Skip(2).Print("Skip first 2 elements: ");

        // Skip elements as long they are smaller than 5.
        numbers.SkipWhile(x => x < 5).Print("Elements skipped until it encounters element >=5: ");

        // Reverse the elements.
        numbers.Reverse().Print("Reversed: ");

        // Remove duplicates.
        numbers.Distinct().Print("Duplicates removed: ");

        // Distinct method combined with LINQ query syntax.
        var query = (from number in numbers where number >= 4 && number <= 7 orderby number select number).Distinct();
        query.Print("Unique numbers in range [4, 7]: ");

        // DD

        // Aggregation methods
        int sum = numbers.Sum();
        Console.WriteLine("Sum of elements: {0}", sum);

        double avg = numbers.Average();
        Console.WriteLine("Average of elements: {0}", avg);

        int min = numbers.Min(); int max = numbers.Max();
        Console.WriteLine("Min, Max of elements: {0}, {1}", min, max);

        // User-defined Aggregation Methods
        string sentence = "the quick brown fox jumps over the lazy dog";

        // Split the string into individual words.
        string[] words = sentence.Split(' ');

        // Prepend each word to the beginning of the new sentence to reverse the word order.
        string reversed = words.Aggregate((workingSentence, next) =>
                                              next + " " + workingSentence);
        Print(reversed, "reversed: ");

        double[] array = { 2.0, -4.0, 10.0 };

        double sumSeed = 0.0;
        double sumSquares = array.Aggregate(sumSeed, (seed, n) => seed + n * n);
        Console.WriteLine("Sum of squares: {0}", sumSquares);   // 120

        double L1Norm = array.Aggregate(sumSeed, (seed, n) => seed + Math.Abs(n));
        Console.WriteLine("L1 norm: {0}", L1Norm);  // 16    

        double productSeed = 1.0;
        double product = array.Aggregate(productSeed, (seed, n) => seed *n);
        Console.WriteLine("Product of terms in array: {0}", product); // -80
    }

	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}
