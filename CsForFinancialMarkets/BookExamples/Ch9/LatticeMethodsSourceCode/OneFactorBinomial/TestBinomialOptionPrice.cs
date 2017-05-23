// TestBinomialOptionPrice.cs
//
// Main program to run and test the Binomial method
// for a stock option. This code 'drives' the process
// as it were.
//
// 2005-1-31 DD First official code 
// 2010-7-29 DD C# version
//
// The mediator class in the Binomial method
// 
// 
// (C) Datasim Education BV 2005-2013
//

using System;

class Mediator
{

    static IOptionFactory getFactory()
    {
	    return new ConsoleEuropeanOptionFactory();

        // Later, other factory types
    }

    static public BinomialLatticeStrategy getStrategy(double sig, double r, double k,
                                        double S, double K, int N)
    {
        Console.WriteLine("\n1. CRR, 2. JR, 3. TRG, 4. EQP, 5. Modified CRR:\n6. Cayley JR Transform: 7 Cayley CRR:" );
                           
        int choice;
        choice = Convert.ToInt32(Console.ReadLine());

        if (choice == 1)
            return new CRRStrategy(sig, r, k);

        if (choice == 2)
            return new JRStrategy(sig, r, k);

        if (choice == 3)
            return new TRGStrategy(sig, r, k);

        if (choice == 4)
            return new EQPStrategy(sig, r, k);

        if (choice == 5)
            return new ModCRRStrategy(sig, r, k, S, K, N);

        if (choice == 6)
            return new PadeJRStrategy(sig, r, k);

        if (choice == 7)
            return new PadeCRRStrategy(sig, r, k);


        return new CRRStrategy(sig, r, k);
    }

  /*  public static double EarlyImpl(double P, double S)
    {

        double K = 10.0;

        if (P > K - S)
        {
            return P;
        }
        return K - S;
    }*/

    // This could be made into a member function of Option

        public static void Main()
        {


            // Phase I: Create and initialise the option
            IOptionFactory fac = getFactory();

            int N = 200;
            Console.Write("Number of time steps: ");
            N = Convert.ToInt32(Console.ReadLine());

            double S;
            Console.Write("Underlying price: ");
            S = Convert.ToDouble(Console.ReadLine());

            Option opt = fac.create();

            double k = opt.T / N;
           
            // Create basic lattice
            double discounting = Math.Exp(-opt.r * k);

            // Phase II: Create the binomial method and forward induction
            BinomialLatticeStrategy binParams = getStrategy(opt.sig, opt.r, k, S, opt.K, N); // Factory
            BinomialMethod bn = new BinomialMethod(discounting, binParams, N);

            bn.modifyLattice(S);

            // Phase III: Backward Induction and compute option price
            Vector<double> RHS = new Vector<double>(bn.BasePyramidVector());
            if (binParams.bType == BinomialType.Additive)
            {
                RHS[RHS.MinIndex] = S * Math.Exp(N * binParams.downValue());
                for (int j = RHS.MinIndex + 1; j <= RHS.MaxIndex; j++)
                {
                    RHS[j] = RHS[j-1] * Math.Exp(binParams.upValue() - binParams.downValue());
                }
            }
            
            Vector<double> Pay = opt.PayoffVector(RHS);

            double pr = bn.getPrice(Pay);
            Console.WriteLine("European {0}", pr);

            // Binomial method with early exercise
            BinomialMethod bnEarly = new BinomialMethod(discounting, binParams, N, opt.EarlyImpl);
            bnEarly.modifyLattice(S);
            Vector<double> RHS2 = new Vector<double>(bnEarly.BasePyramidVector());
            Vector<double> Pay2 = opt.PayoffVector(RHS2);
            double pr2 = bnEarly.getPrice(Pay2);
            Console.WriteLine("American {0}",pr2);


            // Display in Excel; first create array of asset mesh points
            int startIndex = 0;
            Vector<double> xarr = new Vector<double>(N + 1, startIndex);
            xarr[xarr.MinIndex] = 0.0;
            for (int j = xarr.MinIndex + 1; j <= xarr.MaxIndex; j++)
            {
                xarr[j] = xarr[j - 1] + k;
            }

            // Display lattice in Excel
            ExcelMechanisms exl = new ExcelMechanisms();

            try
            {
                // public void printLatticeInExcel(Lattice<double> lattice, Vector<double> xarr, string SheetName)
                string sheetName = "Lattice";
                exl.printLatticeInExcel(bnEarly.getLattice(), xarr, sheetName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
}