// EuropeanOptionFactory.cs
//
// Classes for creating Eurpean Options. This is
// a Factory Method pattern.
//
// (C) Datasim Education BV 2005-2013

using System;

public interface IOptionFactory
{
		Option create();
}

public class ConsoleEuropeanOptionFactory: IOptionFactory
{
	
		public Option create()
		{
			Console.Write( "\n*** Parameters for option object ***\n");

			Option opt = new Option();

			Console.Write( "Strike: ");
			opt.K = Convert.ToDouble(Console.ReadLine());

			Console.Write( "Volatility: ");
			opt.sig = Convert.ToDouble(Console.ReadLine());

			Console.Write( "Interest rate: ");
			opt.r = Convert.ToDouble(Console.ReadLine());
           

			Console.Write( "Expiry date: ");
			opt.T = Convert.ToDouble(Console.ReadLine());

			Console.Write( "1. Call, 2. Put: ");
			opt.type = Convert.ToInt32(Console.ReadLine());

			return opt;
		}
}





