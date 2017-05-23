// TestOption.cs
//
// Test program for the  solutions of European options
//
// (C) Datasim Education BV 2003-2013
//

using System;

public struct Mediator
{ // The class that directs the program flow, from data initialisaton,
    // computation and presentation

    static IOptionFactory getFactory()
    {
        return new ConsoleEuropeanOptionFactory();
    }

    public void calculate()
    {
        // 1. Choose how the data in the option will be created
        IOptionFactory fac = getFactory();

	    // 2. Create the option
        Option myOption = fac.create();

        // 3. Get the price 
        Console.Write("Give the underlying price: ");
        double S = Convert.ToDouble(Console.ReadLine());

        // 4. Display the result
        Console.WriteLine("Price: {0}", myOption.Price(S));
    }
}


class TestOption
{
  
    static void Main()
 
    { // All options are European

 
        // Major client delegates to the mediator (aka sub-contractor)
        Mediator med = new Mediator();
        med.calculate();

        // Just a test, could crop up when doing numerics
       /* double nan = 0.0f / 0.0f;        // Not a number (nan==NaN)
        double d = SpecialFunctions.N(nan);*/

    }
}