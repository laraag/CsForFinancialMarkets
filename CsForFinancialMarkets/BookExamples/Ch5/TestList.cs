// List<double>.cs
//
// Example program that show the List<double> class.
//
// (C) Datasim Education BV  2001-2003

using System;

public class ListMain
{

    static void print(System.Collections.Generic.List<double> arr)
    { // Simple formatted print of a list.

        Console.Write("Array: ");
        for (int j = 0; j < arr.Count; j++)
        {
            Console.Write(arr[j]); Console.Write(", ");
        }
        Console.WriteLine();
    }

	public static void Main()
	{
        // Create array list object.
		System.Collections.Generic.List<double> valArray=new System.Collections.Generic.List<double>();

        // Add elements and show count.
        valArray.Add(1.0);
        valArray.Add(2.0);
        valArray.Add(-10.99);
		Console.WriteLine("Count={0}", valArray.Count);   // 3
        print(valArray);

        // Get value at index 2 and show it.
        double value2;
		value2 = valArray[2];
		Console.WriteLine("Element 2: {0}", value2);     // -10.99

        // Use foreach to display all valArray.
        foreach (double value in valArray)
		{
			Console.WriteLine(value);                   // 1.0, 2.0
		}

        // Remove element and show count.
        valArray.Remove(1.0);
		Console.WriteLine("Count={0}", valArray.Count);    // 2

        // Clear all and show count.
        valArray.Clear();
		Console.WriteLine("Count={0}", valArray.Count);    // 0

        // Create an array and add it to valArray.
        valArray.Add(-100.0); valArray.Add(-200.0); valArray.Add(-300);
        valArray.Add(-400.0); valArray.Add(-500.0); valArray.Add(-1600.0); valArray.Add(-1600.0);

        int N = 10;
        System.Collections.Generic.List<double> valArray2 = new System.Collections.Generic.List<double>(N);

        for (int j = 0; j < valArray2.Count; j++)
        {
            valArray2[j] = j + 100.0;
        }

        // Now add valArray2 at a given position.
        int position = 3;
        valArray.InsertRange(position, valArray2);
        print(valArray2);
	}
}