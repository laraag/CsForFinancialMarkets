// TestAffineModels.cs
//
// (C) Datasim Education BV 2010-2013
//

using System;

class TestAffineModels
{
    static void Main()
    {

        {
            // Parameters for Vasicek model
            double r = 0.05;
            double kappa = 0.15;
            double vol = 0.01;
            double theta = r;

            VasicekModel vasicek = new VasicekModel(kappa, theta, vol, r);

            Console.WriteLine(vasicek.P(0.0, 1.0));
            Console.WriteLine(vasicek.P(0.0, 5.0));
            Console.WriteLine(vasicek.R(0.0, 5.0));
        }
        {

            double r = 0.05;
            double kappa = 0.15;
            double vol = 0.1;
            double theta = r;


            OptionType type = OptionType.Call;
           // OptionType type = OptionType.Put;
            double t = 0.0;
            double T = 1.0;
            double s = 5.0;
            double K = 0.67;
            CIRModel cir = new CIRModel(kappa, theta, vol, r);

            Console.WriteLine("cir bond price {0}", cir.P(0.0, 2.0));
        //    Console.WriteLine(cir.R(0.0, 5.0));

            OptionPricer bv = new OptionPricer(t, T, s, K, type);

            bv.Visit(cir);
            Console.WriteLine("CIR option {0}", bv.price);
        }

        {
            //OptionType type = OptionType.Call;
            OptionType type = OptionType.Call;
            double t = 0.0;
            double T = 1.0;
            double s = 5.0;
            double K = 0.67;

            double r = 0.05;
            double kappa = 0.15;
            double vol = 0.1;
            double theta = r;

            VasicekModel vasicek = new VasicekModel(kappa, theta, vol, r);
            OptionPricer bv = new OptionPricer(t, T, s, K, type);
            bv.Visit(vasicek);

            Console.WriteLine("Vasicek price {0}", bv.price);
        }

        {
            //OptionType type = OptionType.Call;
            OptionType type = OptionType.Put;
            double t = 0.0;
            double T = 1.0;
            double s = 5.0;
            double K = 0.67;

            OptionPricer bv = new OptionPricer(t, T, s, K, type);
      //     bv.Visit(vasicek);

         //   Console.WriteLine(bv.price);


            // Some Casting
            // Upcasting 
            double r = 0.05;
            double kappa = 0.15;
            double vol = 0.1;
            double theta = r;

            BondModel bondModel = new VasicekModel(kappa, theta, vol, r);

            bondModel = new CIRModel(kappa, theta, vol, r);

            // Downcasting: correct and incorrect versions.

            CIRModel cirModel = (CIRModel)bondModel;    // OK

            // Cannot convert, hence we get a run-time error.
          //  VasicekModel vasicekModel = (VasicekModel)bondModel;    // OK

            // Two special operators.
            BondModel bondModel2 = new VasicekModel(kappa, theta, vol, r);
            CIRModel cirModel2 = bondModel2 as CIRModel;
            if (cirModel2 == null)
            {
                Console.WriteLine("Conversion not successful");
            }

            // The 'is' operator
            if (bondModel2 is VasicekModel) // YES
            {
                Console.WriteLine("This is a Vasicek model");
            }
            else if (bondModel2 is CIRModel)
            {
                 Console.WriteLine("This is a CIR model");
            }
            else
            {
                Console.WriteLine("Oops, not Vasicek and not CIR");

            }

       /*     // Use of 'var'

            var v1 = 10;
            var v2 = "hello";
            var v3 = new DateTime();


            int v1A = 10;
            string v2B = "hello";
            DateTime v3C = new DateTime();

            // Will not compile
            var x = 12;
         //   x = "12";   // Compile-time error; x is an int


            var numbers = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };*/
        }
            

    }
}