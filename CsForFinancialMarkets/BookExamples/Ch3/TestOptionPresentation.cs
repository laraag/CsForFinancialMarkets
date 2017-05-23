// TestOption.cs
//
// Test program for the  solutions of European options
//
// (C) Datasim Education BV 2003-2010
//
// AG: line 55 new Range<double>(0.001, 210.0); //


using System;
using OptionExtensions; // The namespace that contains extension methods for Option

class TestOption
{
    static void Main()
 
    { // All options are European

	    // Call option on a stock
	    Option callOption = new Option();
        Console.WriteLine("Option price: {0}", callOption.Price(60.0));
	
        // Put option on a stock index
	    Option myOption = new Option();
	    myOption.otyp = "P";
	    myOption.K = 65.0;
	    myOption.T = 0.25;
	    myOption.r = 0.08;
	    myOption.sig = 0.30;

        double g = myOption.Gamma(65.0);

	    //double q = 0.0; // 0.05;		// Dividend yield
    	//myOption.b = myOption.r - q;
	    myOption.b = myOption.r; // stock

        double S = 60.0;
        //Console.WriteLine("Option type and price: {{0}, {1})", myOption.otyp, myOption.Price(S));
        Console.WriteLine("Option price: {0}", myOption.Price(S));

        // Using extension methods
        myOption.Display(S);

        double lower = 10.0;
        double upper = 100.0;
        int N = 90;
        double[] priceArray = myOption.Price(lower, upper, N);

        for (int j = 0; j < priceArray.Length; j++)
        {
            Console.WriteLine(priceArray[j]);
        }

        
	    Range<double> extent = new Range<double>(0.001, 210.0); // AG: it is better not to start from 0 as gamma is NaN for S=0
	    int NumberSteps = 210;

        OptionPresentation myPresent = new OptionPresentation(myOption, extent, NumberSteps);

    	OptionValueType val = OptionValueType.Value;
    	myPresent.displayinExcel(val);

	    val = OptionValueType.Delta;
	    myPresent.displayinExcel(val);

    	val = OptionValueType.Gamma;
	    myPresent.displayinExcel(val);

    	val = OptionValueType.Vega;
    	myPresent.displayinExcel(val);

    	val = OptionValueType.Theta;
    	myPresent.displayinExcel(val);

	    val = OptionValueType.Rho;
	    myPresent.displayinExcel(val);

    	val = OptionValueType.Coc;
	    myPresent.displayinExcel(val);


    }
}