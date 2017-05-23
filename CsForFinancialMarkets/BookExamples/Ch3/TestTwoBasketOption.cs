// TestTwoAssetOption.cs
//
// Testng two-asset option payoffs
//
// (C) Datasim Education BV 2006-2013
//

using System;

using MultiAssetOptionPayoffExtensions;

class TestTwoAssetOption
{
    public InstrumentFactory GetInstrumentFactory()
    {

        return new ConsoleInstrumentFactory();
    }

    public static void Main()
    {
      //  InstrumentFactory myFactory = new ConsoleInstrumentFactory(); //GetInstrumentFactory();
      // TwoFactorOption myOption = myFactory.CreateOption();

        int type = 1;  // Call/put
        double w1 = 1.0;
        double w2 = -1.0;
        double K = 5.0;

        BasketStrategy myPayoff = new BasketStrategy(K, type, w1, w2);

        Range<double> r1= new Range<double>(0.0, 200.0);
        int N1 = 20;

        Range<double> r2 = new Range<double>(0.0, 200.0);
        int N2 = 20;

        // Calling extension methods
        NumericMatrix<double> matrix = myPayoff.PayoffMatrix(r1, r2, N1, N2);

        Console.WriteLine("Starting Excel, please wait..");

        myPayoff.DisplayInExcel(r1, r2, N1, N2);
    }
}

