using System;
using System.Collections.Generic;
using System.Text;

namespace Exercise2
{

    public interface IOptionFactory
    { // An interface consists of abstract methods

        VanillaOption create();
    }

    public class ConsoleEuropeanOptionFactory : IOptionFactory
    {
        public VanillaOption create()
        {
            Console.Write("\n*** Parameters for option object ***\n");

            double r;		// Interest rate
            double sig;	    // Volatility
            double K;		// Strike price
            double T;		// Expiry date
            double b;		// Cost of carry

            string type;	// Option name (call, put)

            //Console.Write("Strike: ");
            //K = Convert.ToDouble(Console.ReadLine());

            //Console.Write("Volatility: ");
            //sig = Convert.ToDouble(Console.ReadLine());

            //Console.Write("Interest rate: ");
            //r = Convert.ToDouble(Console.ReadLine());

            //Console.Write("Cost of carry: ");
            //b = Convert.ToDouble(Console.ReadLine());

            //Console.Write("Expiry date: ");
            //T = Convert.ToDouble(Console.ReadLine());

            //Console.Write("1. Call, 2. Put: ");
            //type = Convert.ToString(Console.ReadLine());

            r = 0.05;
            sig = 0.1;
            K = 0.8;
            T = 1;
            //s = 5;
            b = 0; // ???????
            type = "1";

            VanillaOption opt = new VanillaOption(type, T, K, b, r, sig);
            return opt;

        }
       
    }


}
