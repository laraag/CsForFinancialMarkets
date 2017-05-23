using System;
using System.Collections.Generic;
using System.Text;

namespace Exercise2
{

    public class VanillaOption
    {
        private double r;       // Interest rate
        private double sig;     // Volatility
        private double K;       // Strike price
        private double T;       // Expiry date
        private double b;       // Cost of carry

        private string type;    // Option name (call, put)


        // Kernel Functions (Haug)
        private double CallPrice(double U)
        {

            double tmp = sig * Math.Sqrt(T);

            double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            double d2 = d1 - tmp;


            return (U * Math.Exp((b - r) * T) * SpecialFunctions.N(d1)) - (K * Math.Exp(-r * T) * SpecialFunctions.N(d2));

        }

        private double PutPrice(double U)
        {

            double tmp = sig * Math.Sqrt(T);

            double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            double d2 = d1 - tmp;
            return (K * Math.Exp(-r * T) * SpecialFunctions.N(-d2)) - (U * Math.Exp((b - r) * T) * SpecialFunctions.N(-d1));

        }


        /////////////////////////////////////////////////////////////////////////////////////

        public void init()
        {   // Initialise all default values


            r = 0.08;
            sig = 0.30;
            K = 65.0;
            T = 0.25;
            b = r;

            type = "C";
        }


        public VanillaOption()
        { // Default call option

            init();
        }


        public VanillaOption(string optionType)
        {   // Create option instance of given type and default values

            init();
            type = optionType;

            // Finger trouble option
            if (type == "c")
                type = "C";

        }


        public VanillaOption(string optionType, double expiry, double strike, double costOfCarry,
                        double interest, double volatility)
        { // Create option instance

            type = optionType;
            T = expiry;
            K = strike;
            b = costOfCarry;
            r = interest;
            sig = volatility;
        }

        public VanillaOption(string optionType, string underlying)
        { // Create option type

            init();
            type = optionType;
        }



        // Functions that calculate option price and sensitivities
        public double Price(double U)
        {

            // cout << "European option\n";

            if (type == "1")
            {
                return CallPrice(U);
            }
            else
                return PutPrice(U);

        }


    }
}
