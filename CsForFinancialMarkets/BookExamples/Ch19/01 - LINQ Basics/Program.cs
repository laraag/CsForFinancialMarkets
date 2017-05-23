// LINQ basics example.
//
// Shows LINQ from the implementation of extension methods to the special LINQ syntax.
//
// (C) Datasim Edcuation BV  2009-2011

using System;
using System.Collections.Generic;
using System.Linq;


static class Program
{
	static void Main(string[] args)
    {
        // Create an array with names.
        string[] names = { "John", "Claire", "Dirk", "Harry", "Daniel", "Susan", "Diane" };

        {
            // Use the LINQ Where() method directly to select names starting with 'D'.
            // Where() uses a Lambda expression to specify the filter criteria.
            IEnumerable<string> dNames = Enumerable.Where(names, x => x[0] == 'D');
            dNames.Print("Names starting with 'd' (as direct static method): ");
        }

        {
            // Call the LINQ Where() method as extension method.
            // Simplify result using 'var' variable.
            var dNames = names.Where(x => x[0] == 'D');
            dNames.Print("Names starting with 'd' (as extension method): ");
        }

        {
            // LINQ operators can be chained. Output of one is input for next.
            // Get all names starting with 'D', ordered by name and selected transformed to upper case.
            var dNames =
                names.Where(x => x[0] == 'D').OrderBy(x => x).Select(x => x.ToUpper());
            dNames.Print("Names starting with 'd' (chained, sorted and uppercase): ");
        }

        {
            // Now use LINQ syntax. Select part is mandatory but can be just 'item' if no transformation is needed.
            // Get all names starting with 'D', ordered by name and selected transformed to upper case.
            var dNames = from item in names where item[0] == 'D' orderby item select item.ToUpper();
            dNames.Print("Names starting with 'd' (LINQ syntax): ");
        }

        {
            // Use ascending ordering (default if not specified).
            // Get all names starting with 'D', ordered by name and selected transformed to upper case.
            var dNames = from item in names where item[0] == 'D' orderby item ascending select item.ToUpper();
            dNames.Print("Names starting with 'd' (ascending): ");
        }

        {
            // Use descending ordering.
            // Get all names starting with 'D', ordered by name and selected transformed to upper case.
            var dNames = from item in names where item[0] == 'D' orderby item descending select item.ToUpper();
            dNames.Print("Names starting with 'd' (descending): ");
        }

        { // DD

            IEnumerable<string> myResult = names
                .Where(n => n.Contains('D'))
                .OrderBy(n => n.Length)
                .Select(n => n.ToLower());

            // Output: dirk, diane, daniel
            myResult.Print("Using chained query operators: ");

        }

        { // DD, comprehension syntax

            IEnumerable<string> myResult =
                from n in names
                where n.Contains('D')
                orderby n.Length
                select n.ToLower();

            // Output: dirk, diane, daniel
            myResult.Print("Using comprehension queries: ");
        }

        { // DD mixed syntax

            string first = (from n in names where n.Contains('D') orderby n.Length select n.ToLower()).First();
            Console.WriteLine("First in emitted list:{0}", first); // dirk

            string last = (from n in names where n.Contains('D') orderby n.Length select n.ToLower()).Last();
            Console.WriteLine("Last in emitted list:{0}", last); // daniel

            int count = (from n in names where n.Contains('D') orderby n.Length select n.ToLower()).Count();
            Console.WriteLine("Number of elements in emitted list:{0}", count); // 3
        }

        { // DD progressive query building 

            var filtered = names.Where(n => n.Contains('D'));
            var sorted = filtered.OrderBy(n => n.Length);
            var myQuery = sorted.Select(n => n.ToLower());

            // Output: dirk, diane, daniel
            myQuery.Print("Using progressive query building: ");
        }

        { // DD use of var

            string[] letters = { "AAAA", "ZZ", "CCCC", "D" };

            IEnumerable<string> filtered1 = letters.Where(n => n.Length > 2);
            filtered1.Print("First filter: ");

            var filtered2 = letters.Where(n => n.Length > 2);
            filtered2.Print("Second filter: ");

        }
    }

	// Extension method to print a collection.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}

}

