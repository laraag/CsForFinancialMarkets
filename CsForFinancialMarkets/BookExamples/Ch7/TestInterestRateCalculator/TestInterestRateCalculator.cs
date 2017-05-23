 // TestTestInterestRateCalculator.cs
 // 
 // Test program for 101 interest rate examples.
 // 
 // (C) Datasim Education BV 2006-2009
 // 
 // 22 Sep 12 AG: some updates
using System;
using System.IO;
using System.Globalization;  // For formatting

class TestInterestRateCalculator
{
    public static void Main()
    {
            // Future value of a sum of money invested today
        int nPeriods = 6;				 // 6 years 
        double P = Math.Pow(10.0, 7);	 // Amount invested now, 10 million
        double r = 0.092;		         // 9.2% interest per year

        InterestRateCalculator interestEngine = new InterestRateCalculator(nPeriods, r);
        double fv = interestEngine.FutureValue(P);

        Console.WriteLine("Future Value: {0} ", fv.ToString("N", CultureInfo.InvariantCulture));

            // Future value of a sum of money invested today, m periods 
            // per year. r is annual interest rate           
        int m = 2;   // Compounding per year
        double fv2 = interestEngine.FutureValue(P, m);
        Console.WriteLine("Future Value with {0} compoundings per year {1} ", m, fv2);
          
            // Future value of an ordinary annuity
        double A = 2.0 * Math.Pow(10.0, 6);
        interestEngine.Interest = 0.08;
        interestEngine.NumberOfPeriods = 15;	 // 15 years
        Console.WriteLine("Ordinary Annuity: {0} ", interestEngine.OrdinaryAnnuity(A));

            // Present Value
        double Pn = 5.0 * Math.Pow(10.0, 6);

        interestEngine.Interest = 0.10;
        interestEngine.NumberOfPeriods = 7;
        Console.WriteLine("**Present value: {0} ", interestEngine.PresentValue(Pn));

            // Present Value of a series of future values
        interestEngine.Interest = 0.0625;
        interestEngine.NumberOfPeriods = 5;
        int nPeriods2 = interestEngine.NumberOfPeriods;
        double[] futureValues = new double[nPeriods2];  // For five years
        for (long j = 0; j < nPeriods2 - 1; j++)
        {  // The first 4 years
            futureValues[j] = 100.0;
        }
        futureValues[nPeriods2 - 1] = 1100.0;

        Console.WriteLine("**Present value, series: {0} ", interestEngine.PresentValue(futureValues));

            // Present Value of an ordinary annuity
        A = 100.0;

        interestEngine.Interest = 0.09;
        interestEngine.NumberOfPeriods = 8;

        Console.WriteLine("**PV, ordinary annuity: {0}", interestEngine.PresentValueOrdinaryAnnuity(A));

            // Now test periodic testing with continuous compounding
        double P0 = Math.Pow(10.0, 8);
        r = 0.092;
        nPeriods2 = 6;
        for (int mm = 1; mm <= 100000000; mm *= 12)
        {
            Console.WriteLine("Periodic: {0}\t {1}", mm, interestEngine.FutureValue(P0, mm));
        }

        Console.WriteLine("Continuous Compounding: {0}", interestEngine.FutureValueContinuous(P0));

            // Bond pricing
        double coupon = 50;                         // Cash coupon, i.e. 10.0% rate semiannual on parValue
        int n = 40;                                 // Number of payments
        double annualInterest = 0.11;               // Interest rate annualized
        int paymentPerYear = 2;                     // Number of payment per year
        double parValue = 1000.0;
        Bond myBond = new Bond(n, annualInterest, coupon, paymentPerYear);

        double bondPrice = myBond.price(parValue);
        Console.WriteLine("Bond price: {0}", bondPrice);
    }
}
