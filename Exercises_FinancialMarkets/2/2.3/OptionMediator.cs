using system;
using System.Collections.Generic;
using System.Text;

namespace Exercise2
{
    public struct OptionMediator
    { // The class that directs the program flow, from data initialisaton,
      // computation and presentation

        static IOptionFactory getFactory()
        {
            return new ConsoleEuropeanOptionFactory();
        }

        public void calculate()
        {
            // 1. Choose how the data in the option will be created
            IOptionFactory fac = getFactory();

            // 2. Create the option
            VanillaOption myOption = fac.create();

            // 3. Get the price 
            //Console.Write("Give the underlying price: ");
            //double S = Convert.ToDouble(Console.ReadLine());
            double S = 

            // 4. Display the result
            Console.WriteLine("Price: {0}", myOption.Price(S));
        }
    }

}
