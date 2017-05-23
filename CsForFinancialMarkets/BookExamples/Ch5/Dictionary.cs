// Dictionary.cs
//
// Example program that show the Dictionary class.
//
// (C) Datasim Education BV  2001-2003

using System;
using System.Collections.Generic;

public class DictionaryMain
{
	public static void Main()
	{
        // Create hash table object.
        System.Collections.Generic.Dictionary<string, DateTime> birthDates;
        birthDates = new System.Collections.Generic.Dictionary<string, DateTime>();

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

        // Iterate over the dictionary; print keys and values.
        foreach (System.Collections.Generic.KeyValuePair<string, DateTime> kvp in birthDates)
        {
            Console.WriteLine("Key = {0}, Value = {1}",
                kvp.Key, kvp.Value);
        }

        // Find the keys and values in the dictionary.
        System.Collections.Generic.Dictionary<string, DateTime>.KeyCollection keys = birthDates.Keys;
        foreach (string key in keys)
        {
            Console.Write(key + ", ");
        }

        Console.WriteLine();
        System.Collections.Generic.Dictionary<string, DateTime>.ValueCollection values = birthDates.Values;
        foreach (DateTime val in values)
        {
            Console.Write(val + ", ");
        }

	}
}