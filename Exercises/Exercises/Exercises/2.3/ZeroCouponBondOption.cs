using System;
using System.Collections.Generic;
using System.Text;

namespace Exercise2
{
    public class ZeroCouponBondOption : Option
    {
        private double S;       // Interest rate
        public ZeroCouponBondOption(string optionType, double expiry, double strike, double costOfCarry,
                        double interest, double volatility, double s) : base(optionType, expiry, strike, costOfCarry,
                        interest, volatility)
        {
            S = s;
        }

        public override double CallPrice()
        {
            double pf = Math.Exp(-r * (S - T));

            double tmp = sig * Math.Sqrt(T);

            double d1 = (Math.Log(pf / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            double d2 = d1 - tmp;

            double callPrice = Math.Exp(-r * T) * ((pf * SpecialFunctions.N(d1)) - (K * SpecialFunctions.N(d2)));

            return callPrice;
        }

        public override double PutPrice()
        {
            double u = 0;

            //double tmp = sig * Math.Sqrt(T);

            //double d1 = (Math.Log(u / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            //double d2 = d1 - tmp;
            //return (K * Math.Exp(-r * T) * SpecialFunctions.N(-d2)) - (u * Math.Exp((b - r) * T) * SpecialFunctions.N(-d1));
            return 0;
        }

    }
}
