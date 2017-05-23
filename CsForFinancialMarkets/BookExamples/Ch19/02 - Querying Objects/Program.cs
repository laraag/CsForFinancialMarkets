// Linq example with objects.
//
// Show how to query a list with persons.
// Shows also how to query ArrayList that is not supported by LINQ.
//
// (C) Datasim Education BV 2009-2011

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// Simple class for persons.
class Person
{
	// Enum indicating gender.
	public enum GenderEnum
	{
		Male, Female
	}

	// Person properties.
	public string Name { get; set; }
	public DateTime BirthDate { set; get; }
	public GenderEnum Gender { get; set; }

	public override string ToString()
	{
		return String.Format("{0} ({1:d}) ({2})", Name, BirthDate, Gender==GenderEnum.Female?"F":"M");
	}
}

static class Program
{
	static void Main(string[] args)
	{
		QueryListOfObjects();
		QueryArrayList();
		QueryReturningAnonymousClass();
		DeferredExecution();
		DeferredExecution2();
	}

	static void QueryListOfObjects()
	{
		Console.WriteLine("*** Query list of objects ***");

		// Create a list of persons.
		var persons=new List<Person> 
		{
			new Person { Name="Bob", BirthDate=new DateTime(1972, 12, 1), Gender=Person.GenderEnum.Male },
			new Person { Name="Susan", BirthDate=new DateTime(1978, 10, 11), Gender=Person.GenderEnum.Female },
			new Person { Name="Andrew", BirthDate=new DateTime(1962, 1, 21), Gender=Person.GenderEnum.Male },
			new Person { Name="Daniel", BirthDate=new DateTime(1982, 3, 12), Gender=Person.GenderEnum.Male },
			new Person { Name="Diane", BirthDate=new DateTime(1976, 5, 30), Gender=Person.GenderEnum.Female },
		};

		// Print the collection.
		persons.Print("Persons: ");

		// Select all boys born after 1970. Only return the names instead of whole object.
		var boys=from person in persons 
				 where person.Gender==Person.GenderEnum.Male && person.BirthDate>new DateTime(1970, 1, 1) 
				 select person.Name;
		boys.Print("\nBoys born after 1970: ");
	}

	static void QueryArrayList()
	{
		Console.WriteLine("\n*** Query array list ***");

		// Create an array list of persons.
		ArrayList persons=new ArrayList 
		{
			new Person { Name="Bob", BirthDate=new DateTime(1972, 12, 1), Gender=Person.GenderEnum.Male },
			new Person { Name="Susan", BirthDate=new DateTime(1978, 10, 11), Gender=Person.GenderEnum.Female },
			new Person { Name="Andrew", BirthDate=new DateTime(1962, 1, 21), Gender=Person.GenderEnum.Male },
			new Person { Name="Daniel", BirthDate=new DateTime(1982, 3, 12), Gender=Person.GenderEnum.Male },
			new Person { Name="Diane", BirthDate=new DateTime(1976, 5, 30), Gender=Person.GenderEnum.Female },
		};

		// Print the collection.
		persons.OfType<Person>().Print("Persons: ");

		// Select all boys born after 1970. Only return the names instead of whole object.
		// OfType<Person>() extension method transforms ArrayList into 
		// an IEnumerable<Person> type so it can be used with LINQ.
		var boys=from person in persons.OfType<Person>()
				 where person.Gender==Person.GenderEnum.Male && person.BirthDate>new DateTime(1970, 1, 1)
				 select person.Name;
		boys.OfType<Person>().Print("\nBoys born after 1970: ");
	}

	static void QueryReturningAnonymousClass()
	{
		Console.WriteLine("\n*** Query Returning Anonymous Class ***");

		// Create a list of persons.
		var persons=new List<Person> 
		{
			new Person { Name="Bob", BirthDate=new DateTime(1972, 12, 1), Gender=Person.GenderEnum.Male },
			new Person { Name="Susan", BirthDate=new DateTime(1978, 10, 11), Gender=Person.GenderEnum.Female },
			new Person { Name="Andrew", BirthDate=new DateTime(1962, 1, 21), Gender=Person.GenderEnum.Male },
			new Person { Name="Daniel", BirthDate=new DateTime(1982, 3, 12), Gender=Person.GenderEnum.Male },
			new Person { Name="Diane", BirthDate=new DateTime(1976, 5, 30), Gender=Person.GenderEnum.Female },
		};

		// Print the collection.
		persons.Print("Persons: ");

		// Select all boys born after 1970. Only return the name and birth date instead of full object with gender.
		var boys=from person in persons
				 where person.Gender==Person.GenderEnum.Male && person.BirthDate>new DateTime(1970, 1, 1)
				 select new { person.Name, person.BirthDate };
		boys.Print("\nBoys born after 1970: ");

	}

	static void DeferredExecution()
	{
		Console.WriteLine("\n*** Deferred Execution ***");

		// Create a list of persons.
		var persons=new List<Person> 
		{
			new Person { Name="Bob", BirthDate=new DateTime(1972, 12, 1), Gender=Person.GenderEnum.Male },
			new Person { Name="Susan", BirthDate=new DateTime(1978, 10, 11), Gender=Person.GenderEnum.Female },
			new Person { Name="Andrew", BirthDate=new DateTime(1962, 1, 21), Gender=Person.GenderEnum.Male },
			new Person { Name="Daniel", BirthDate=new DateTime(1982, 3, 12), Gender=Person.GenderEnum.Male },
			new Person { Name="Diane", BirthDate=new DateTime(1976, 5, 30), Gender=Person.GenderEnum.Female },
		};

		// Create a Lambda statement returning all girls.
		var girls=from person in persons
				  where person.Gender==Person.GenderEnum.Female
				  select person;

		// Print all the girls.
		Console.WriteLine("All the girls:");
		foreach (Person p in girls) Console.WriteLine(p);

		// Copy the result to a list (Selection won't be changed when data changes, but an item already in the result might change since in this case references to the objects are stored).
		var girlsCopy=girls.ToList();

		// Change one person into a woman.
		persons[3].Gender=Person.GenderEnum.Female; persons[3].Name+="e";

		// Print all the girls. If query was already executed, it would return the same result as before.
		// But since it returns the new girl, the query is executed again. Thus LINQ uses deferred execution.
		Console.WriteLine("\nAll the girls again after one boy changed into a girl:");
		foreach (Person p in girls) Console.WriteLine(p);

		// Print the result of the query before the change in data.
		Console.WriteLine("\nAll the girls from copied result:");
		foreach (Person p in girlsCopy) Console.WriteLine(p);

	}

	static void DeferredExecution2()
	{
		Console.WriteLine("\n*** Deferred Execution 2 ***");

		// Create sentence in IEnumerable<> so it can be used by LINQ.
		IEnumerable<char> str="The quick brown fox jumps over the lazy dog.";
		
		// Build query to remove a, e, i, o and u.
		// Query not yet executed.
		var query=str.Where(c => c!='a');
		query=query.Where(c => c!='e');
		query=query.Where(c => c!='i');
		query=query.Where(c => c!='o');
		query=query.Where(c => c!='u');

		// Query now executed.
		foreach (char c in query) Console.Write(c);
		Console.WriteLine();

		// Building query in loop.
		// Be careful with outer variables. This will only remove 'u' because that is the value of the outer variable at time of execution.
		query=str;
		foreach (char letter in "aeiou") query=query.Where(c => c!=letter);	// letter in query is an outer variable.

		// Query now executed. This will only remove 'u' because that is the value of the outer variable at time of execution.
		foreach (char c in query) Console.Write(c);
		Console.WriteLine();

		// Building query in loop (corrected version). Ouyter variable is different in each iteration.
		query=str;
		foreach (char letter in "aeiou") { char tmp=letter; query=query.Where(c => c!=tmp); }

		// Query now executed. Correct output.
		foreach (char c in query) Console.Write(c);
		Console.WriteLine();

	}

	// Extension method to print a collection.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}

}

