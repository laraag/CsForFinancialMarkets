// OptionExtensions.cs
//
// Implements C# 'Extension Methods' mechanism for non-intrusive
// addition of new methods to an existing class.
//
// (C) Datasim Education BV 2010-2013
//

using System;

namespace OptionExtensions
{

    public static class OptionMixins
    { // Define new methods for class Option here


        public static void Display(this Option option, double S)
        { //, Compute price and greeks for a given value of underlying
            
            Console.WriteLine("\nDisplay option price and greeks in an Extension Method\n");
            Console.WriteLine("Price: {0}", option.Price(S));
            Console.WriteLine("Delta: {0}", option.Delta(S));
            Console.WriteLine("Gamma: {0}", option.Gamma(S));
            Console.WriteLine("Vega: {0}", option.Vega(S));
            Console.WriteLine("Theta: {0}", option.Theta(S));
            Console.WriteLine("Rho: {0}", option.Rho(S));
            Console.WriteLine("Cost of Carry: {0}", option.Coc(S));
        }

        public static double[] Price(this Option option, double low, double upper, int NSteps)
        { // Compute option price for a range of stock prices

            double h = (upper - low) / NSteps;

            double S = low;
            double[] price = new double[NSteps + 1];

            for (int j = 1; j <= NSteps; j++)
            {
                price[j] = option.Price(S);
                S += h;
            }

            return price;
        }
    }
}
