// TestEnum101.cs
//
// Simple examples of enumerators.
//
// (C) Datasim Education BV 2011
//

using System;
using System.Collections.Generic;
using System.Collections;

/*
public interface IEnumerator
{
    bool MoveNext();         // Advance cursor to next position
    object Current { get;}   // Return element at current position
    void Reset();            // Move back to start
}


public interface IEnumerator<T> : IEnumerator, IDisposable
{
     object Current { get;} // Return element at current position
}

public interface IEnumerable
{
    IEnumerator GetEnumerator();   // Get an enuumerator 'handle'
}

public interface IEnumerable<T> : IEnumerable
{
    IEnumerator<T> GetEnumerator();   // Get an enuumerator 'handle'
}

*/

public class WeekDays : IEnumerable
{
    string[] days = { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat" };

    public IEnumerator GetEnumerator()
    {
        for (int i = 0; i < days.Length; i++)
        {
            yield return days[i];
        }
    }
}


class TestEnumeration
{

    public static double Sum(IEnumerable<double> collections)
    {
        double sum = 0.0;

        foreach (double t in collections) sum += t;

        return sum;
    }


    public static void Main()
    {

        // Using IEnumerator and IEnumerable.
        string myString = "ABCDEFGHIJKLMNOPQRSTXYZ";

        // The class string implements IEnumerable.
        IEnumerator<char> myEnumerator = myString.GetEnumerator();

        // Traverse string until MoveNext returns 'false'.
        while (myEnumerator.MoveNext() == true)     // Made explicit for readability
        {
            char c = myEnumerator.Current;
            Console.Write(c + ",");
        }

        Console.WriteLine();

        // Calculating the sums of arrays and lists.
        double[] arr = { 2.0, 4.0, 6.8, 8.0};
        Console.WriteLine(Sum(arr));

        List<double> arr2 = new List<double>();
        arr2.Add(2.0); arr2.Add(4.0); arr2.Add(6.0);
        Console.WriteLine(Sum(arr2));


        // Iterators
        // Create an instance of the collection class
        WeekDays week = new WeekDays();

        // Iterate with foreach
        foreach (string day in week)
        {
            Console.Write(day + ", ");
        }

    }


}
