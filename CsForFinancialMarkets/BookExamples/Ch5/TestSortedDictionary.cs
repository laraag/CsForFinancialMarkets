// SortedDictionary.cs
//
// Stress testing a sorted dictionary.
//
// (C) Datasim Education BV 2011-2013
//

using System;
using System.Collections.Generic;

public class SortedDictionary
{
    public static void Main()
    {

        // Create hash table object.
        SortedDictionary<int, Date> businessDates;
        businessDates = new SortedDictionary<int, Date>();

        int N = 1000000; // A million dates.
        int dayIncrement = 1;
        int Year = 4004; int Month = 10; int Day = 24; // Ussher date
        Date myDate = new Date(Year, Month, Day);
        for (int j = 1; j <= N; j++)
        {
            myDate = myDate.add_workdays(dayIncrement);
            businessDates[j] = myDate;
        }


        // Read name from user.
        // Console.Write("Enter a date number (1 ... 10000): "); V2
        int name = 5000;

        // Check if name is in the hash table.
        if (businessDates.ContainsKey(name))
        {
            // Get business date and show it.
            myDate = businessDates[name];
            Console.Write("{0}-{1}-{2} : ", myDate.DateValue.Year, myDate.DateValue.Month, myDate.DateValue.Day);
        }
        else
        {
            // No business date for given name.
            Console.WriteLine("No date of business for {0}", name);
        }
    }
    
}

