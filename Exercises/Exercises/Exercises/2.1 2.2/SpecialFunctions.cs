using System;

namespace Exercise2
{
    public class SpecialFunctions
    {

        //////////// Gaussian functions /////////////////////////////////

        static public double n(double x)
        {
            double A = 1.0 / Math.Sqrt(2.0 * 3.1415);
            return A * Math.Exp(-x * x * 0.5); // Math class in C#
        }

        static public double N(double x)
        { // The approximation to the cumulative normal distribution

            //If x is NAN or infinity, it throws a System.StackOverflowException (the execution stack overflows by having too many nested method calls)
            //This is because the logical operators always return false
            double a1 = 0.4361836;
            double a2 = -0.1201676;
            double a3 = 0.9372980;

            try
            {
                double k = 1.0 / (1.0 + (0.33267 * x));

                if (x >= 0.0)
                {
                    return 1.0 - n(x) * (a1 * k + (a2 * k * k) + (a3 * k * k * k));
                }
                else if (x < 0.0)
                {
                    double d = N(-x);
                    return 1.0 - d;
                }
                else
                {//If x is NAN or Infinity

                    Console.WriteLine($"The value passed to the CND function couldn't be evaluated. Value: {x}");
                    throw new Exception($"The value passed to the CND function couldn't be evaluated. Value: {x}");
                    //return x;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown on CDN function: {e}");
                throw new Exception($"The value passed to the CND function couldn't be evaluated. Value: {x}");
                //return x;
            }
        }
    }
}