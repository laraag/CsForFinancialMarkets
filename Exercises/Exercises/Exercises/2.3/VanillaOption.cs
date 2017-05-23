using System;
using System.Collections.Generic;
using System.Text;

namespace Exercise2
{
    public class VanillaOption : Option
    {
        protected double U;       // Underlying price
        public VanillaOption(string optionType, double expiry, double strike, double costOfCarry,
                        double interest, double volatility, double underlyingPrice) : base(optionType, expiry, strike, costOfCarry,
                        interest, volatility)
        {
            U = underlyingPrice;
        }

        public override double CallPrice()
        {
            double tmp = sig * Math.Sqrt(T);

            double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            double d2 = d1 - tmp;


            return (U * Math.Exp((b - r) * T) * SpecialFunctions.N(d1)) - (K * Math.Exp(-r * T) * SpecialFunctions.N(d2));
        }

        public override double PutPrice()
        {
            double tmp = sig * Math.Sqrt(T);

            double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            double d2 = d1 - tmp;
            return (K * Math.Exp(-r * T) * SpecialFunctions.N(-d2)) - (U * Math.Exp((b - r) * T) * SpecialFunctions.N(-d1));
        }

    }
}
