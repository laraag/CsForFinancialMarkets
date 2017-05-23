// TestTupleParts.cs
//
// Testing tuple by an example from databases; identification of parts in a warehouse.
//
// Ported from C++ to C#.
//
// (C) Datasim Education BV 2009-2013
//

using System;

/*
	Part number (integer type)
	Part name (string type)
	Colour (character type); we use codes, thus ‘R’ for ‘red’, for example
	Weight (double type)
	City (string type)

*/

public class TestTuple
{

    public static void print(Tuple<long, string, char, double, string> part)
    { // Print a tuple

        // Retrieving values from a tuple, using a member function get()
        long ID = part.Item1;
        string name = part.Item2;
        char colour = part.Item3;
        double weight = part.Item4;
        string city = part.Item5;

        Console.WriteLine("Elements' parts: {0}, {1}, {2}, {3}, {4}", ID, name, colour, weight, city);
    }
    
    public static void Main()
    {
        Tuple<int, double> myTuple = Tuple.Create(100, 200.0);
        var myTuple2 =  Tuple.Create(1, 2.0);

        Console.WriteLine("myTuple: {0}, {1}", myTuple.Item1, myTuple.Item2);
        Console.WriteLine("myTuple2: {0}, {1}", myTuple2.Item1,
                            myTuple2.Item2);

        Tuple<long, string, char, double, string> myPart 
            = Tuple.Create(1345L, (string)("10 mm screw"), 'R', 
                            0.12, (string)("London"));
     
	    print (myPart);

    }
}
