using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDna.Integration;

namespace OptionFunctions
{
    public class OptionFunctions
    {
        [ExcelFunction(Description = "Compute exact solution for a European option", Category = "Option Functions")]
        public static double OptionPrice(
            [ExcelArgument(Description = @"is whether the option is a call (""C"") or a put (""P"")")] string optionType,
            [ExcelArgument(Description = @"is the current value of the underlying stock")] double underlying,
            [ExcelArgument(Description = @"is the risk-free rate through expiry")] double interestRate,
            [ExcelArgument(Description = @"is the implied volatility at expiry")] double volatility,
            [ExcelArgument(Description = @"is the option's strike price")] double strikePrice,
            [ExcelArgument(Description = @"is the time to maturity in years")] double timeToMaturity,
            [ExcelArgument(Description = @"is the cost of carry")] double costOfCarry)
        {
            // Some basic validation - trying to avoid fatal StackOverflow in SpecialFunctions.N
            if (underlying <= 0.0 ||
                volatility <= 0.0 ||
                timeToMaturity <= 0.0 ||
                strikePrice <= 0.0)
            {
                // Exception will be returned to Excel as #VALUE.
                throw new ArgumentException();
            }

            Option o = new Option();
            o.otyp = optionType;
            o.r = interestRate;
            o.sig = volatility;
            o.K = strikePrice;
            o.T = timeToMaturity;
            o.b = costOfCarry;

            return o.Price(underlying);
        }

        [ExcelFunction(Description = "Compute exact solution for a European option, and returns price and Greeks as a two-column, six-row array with names and values", Category = "Option Functions")]
        public static object[,] OptionPriceGreeks(
            [ExcelArgument(Description = @"is whether the option is a call (""C"") or a put (""P"")")] string optionType,
            [ExcelArgument(Description = @"is the current value of the underlying stock")] double underlying,
            [ExcelArgument(Description = @"is the risk-free rate through expiry")] double interestRate,
            [ExcelArgument(Description = @"is the implied volatility at expiry")] double volatility,
            [ExcelArgument(Description = @"is the option's strike price")] double strikePrice,
            [ExcelArgument(Description = @"is the time to maturity in years")] double timeToMaturity,
            [ExcelArgument(Description = @"is the cost of carry")] double costOfCarry)
        {
            // Some basic validation - trying to avoid fatal StackOverflow in SpecialFunctions.N
            if (underlying <= 0.0 ||
                volatility <= 0.0 ||
                timeToMaturity <= 0.0 ||
                strikePrice <= 0.0)
            {
                // Exception will be returned to Excel as #VALUE.
                throw new ArgumentException();
            }

            Option o = new Option();
            o.otyp = optionType;
            o.r = interestRate;
            o.sig = volatility;
            o.K = strikePrice;
            o.T = timeToMaturity;
            o.b = costOfCarry;

            return new object[6, 2]
            {
                {"Price", o.Price(underlying)},
                {"Gamma", o.Gamma(underlying)},
                {"Vega", o.Vega(underlying)},
                {"Theta", o.Theta(underlying)},
                {"Rho", o.Rho(underlying)},
                {"Coc", o.Coc(underlying)}
            };
        }
    }
}
