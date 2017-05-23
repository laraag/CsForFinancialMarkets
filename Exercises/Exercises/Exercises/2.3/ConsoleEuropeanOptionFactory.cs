using System;
using System.Collections.Generic;
using System.Text;

namespace Exercise2
{

    public interface IOptionFactory
    { // An interface consists of abstract methods

        Option create();
    }

    public class ConsoleEuropeanOptionFactory : IOptionFactory
    {
        public Option create()
        {
            Console.Write("\n*** Parameters for option object ***\n");

            double r;		// Interest rate
            double sig;	    // Volatility
            double K;		// Strike price
            double T;		// Expiry date
            double b;       // Cost of carry
            double s;       // bond expiry
            double u;       // underlying price

            string type;    // Option name (call, put)

            //Console.Write("Strike: ");
            //K = Convert.ToDouble(Console.ReadLine());
            K = 0.8;

            //Console.Write("Volatility: ");
            //sig = Convert.ToDouble(Console.ReadLine());
            sig = 0.1;

            //Console.Write("Interest rate: ");
            //r = Convert.ToDouble(Console.ReadLine());
            r = 0.05;

            //Console.Write("Cost of carry: ");
            //b = Convert.ToDouble(Console.ReadLine());
            b = 0;

            //Console.Write("Expiry date: ");
            //T = Convert.ToDouble(Console.ReadLine());
            T = 1;

            //Console.Write("1. Call, 2. Put: ");
            //type = Convert.ToString(Console.ReadLine());
            type = "1";

            //Console.Write("Give the underlying price: ");
            //u = Convert.ToDouble(Console.ReadLine());

            s = 5;

            Option opt = new ZeroCouponBondOption(type, T, K, b, r, sig, s);
            return opt;

        }
       
    }


}
