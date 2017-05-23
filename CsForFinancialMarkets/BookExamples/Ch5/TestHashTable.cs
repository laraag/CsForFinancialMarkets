// HashTable.cs
//
// Example program that show the HashTable class.
//
// (C) Datasim Education BV  2001-2013

using System;

public class HashTableMain
{
	public static void Main()
	{
        // Create hash table object.
        System.Collections.Hashtable birthDates;
        birthDates = new System.Collections.Hashtable();

        // Add elements (birth dates saved with names). The keys are 
        // the names of the people. These are efficiently accessed based 
        // on the hashed values.
        birthDates.Add("Anna", new DateTime(1975, 10, 2));
        birthDates.Add("Bert", new DateTime(1970, 5, 8));
        birthDates.Add("Chris", new DateTime(1971, 5, 8));

        // Read name from user.
        Console.Write("Enter a name (Anna, Bert or Chris): ");
		string name=Console.ReadLine();

        // Check if name is in the hash table.
		if (birthDates.ContainsKey(name))
		{
			// Get birth date and show it.
			DateTime birthDate = (DateTime)birthDates[name];
			Console.WriteLine("{0}'s birthdate: {1:d}", name, birthDate);
		}
		else
		{
			// No birth date for given name.
			Console.WriteLine("No date of birth for {0}", name);
		}

	}
}