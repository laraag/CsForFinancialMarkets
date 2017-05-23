using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    public abstract class Option
    {
        protected double r;       // Interest rate
        protected double sig;     // Volatility
        protected double K;       // Strike price
        protected double T;       // Expiry date
        protected double b;       // Cost of carry
      
        protected string type;    // Option name (call, put)

        public Option(string optionType, double expiry, double strike, double costOfCarry,
                     double interest, double volatility)
        { // Create option instance

            type = optionType;
            T = expiry;
            K = strike;
            b = costOfCarry;
            r = interest;
            sig = volatility;
        }

        // Functions that calculate option price and sensitivities
        public double Price()
        {
            // cout << "European option\n";

            if (type == "1")
            {
                return CallPrice();
            }
            else
                return PutPrice();

        }

        abstract public double CallPrice();
        abstract public double PutPrice();

    }
}
