// TestSet.cs
//
// Test user-defined sets in C#.
//
// (C) Datasom Education BV 2009-2013
//

using System;

class Set101
{
    static void Main()
    {
        Set<string> names = new Set<string>();
        names.Insert("A1");
        names.Insert("A2");
        names.Insert("A3");
        names.Insert("A4");
        names.Insert("B1");

        names.print();

        Set<string> namesII = new Set<string>();
        namesII.Insert("A1");
        namesII.Insert("X2");
        namesII.Insert("X2");
        namesII.Insert("BB");
        namesII.Insert("B1");

        // Surgery
        namesII.Remove("X2");
        if (namesII.Contains("X2"))
        {
            Console.WriteLine("ugh, X2 is still there");
        }
        else
        {
            Console.WriteLine("X2 has been removed");
        }

        namesII.Replace("B1", "b1");
        namesII.print();

       
        // 'Interactions' between sets (static methods)
        Set<string> result = Set<string>.Intersection(names, namesII);
        result.print();

        result = Set<string>.Union(names, namesII);
        result.print();

        result = Set<string>.Difference(names, namesII);
        result.print();

        result = Set<string>.SymmetricDifference(names, namesII);
        result.print();


    }

}

