// EuropeanOptionFactory.cs
//
// Classes for creating Eurpean Options. This is
// a Factory Method pattern.
//
// (C) Datasim Education BV 2005-2010

using System;

public interface IOptionFactory
{ // An interface consists of abstract methods

		Option create();
}

public class ConsoleEuropeanOptionFactory: IOptionFactory
{
	
		public Option create()
		{
			Console.Write( "\n*** Parameters for option object ***\n");

            double r;		// Interest rate
            double sig;	    // Volatility
            double K;		// Strike price
            double T;		// Expiry date
            double b;		// Cost of carry

            string type;	// Option name (call, put)

            Console.Write("Strike: ");
            K = Convert.ToDouble(Console.ReadLine());

            Console.Write("Volatility: ");
            sig = Convert.ToDouble(Console.ReadLine());

            Console.Write("Interest rate: ");
            r = Convert.ToDouble(Console.ReadLine());

            Console.Write("Cost of carry: ");
            b = Convert.ToDouble(Console.ReadLine());

            Console.Write("Expiry date: ");
            T = Convert.ToDouble(Console.ReadLine());

            Console.Write("1. Call, 2. Put: ");
            type = Convert.ToString(Console.ReadLine());

            Option opt = new Option(type, T, K, b, r, sig);
            return opt;
          
		}
}





