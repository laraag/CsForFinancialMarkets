// ProvideRequire.cs
//
// Use of Delegates in C#.
//
// (C) Datasim Education BV 2008-2013
//

using System;

public struct Data
{ // Option data

	public double T;
	public double K;
	public double r;
	public double sig;
	public double b; // Cost of carry
};

public interface I1
{
   // delegate void DataSource(ref Data data); no types allowed in interface
}


/*	a) Black-Scholes (1973) stock option model: b = r
	b) b = r - q Merton (1973) stock option model with continuous dividend yield
	c) b = 0 Black (1976) futures option model
	d) b = r - rf Garman and Kohlhagen (1983) currency option model, where rf is the 
	   'foreign' interest rate
*/

   
public enum OptionType {Stock, Index, Future};

public struct GeneralisedDataSource
{ // Allows for different kinds of options; this is a function object

	public OptionType optType;

	public GeneralisedDataSource(OptionType optionType)  { optType = optionType; }
	public void init(ref Data val) 
	{
		val.T = 0.25;
		if (optType == OptionType.Future)
			val.b = 0.0;
		
		// more options

        val.K = 65.0;
        val.r = 0.08;
        val.sig = 0.3;
    }
}
    


public struct Pricer3	// One version of an implementation of ICA
{ // A class that offers an interface and requires another interface

	public delegate void DataSource(ref Data data);
    public DataSource ds;

	public double compute(double S)
	{
		// Define the data and slot
		Data data = new Data();
	
		// Connect to slot and initialise the data
		ds(ref data);
		
		double tmp = data.sig * Math.Sqrt(data.T);

		double d1 = ( Math.Log(S/data.K) + (data.b+ (data.sig*data.sig)*0.5 ) * data.T )/ tmp;
		double d2 = d1 - tmp;

		return (S * Math.Exp((data.b-data.r)*data.T) * SpecialFunctions.N(d1)) - (data.K * Math.Exp(-data.r * data.T)* SpecialFunctions.N(d2));
	}

    public static void PlainDataSource(ref Data val)
    { // Simple data source; standard stock

	    val.T = 0.25;
	    val.K = 65.0;
	    val.r = 0.08;
	    val.sig = 0.3;
	    val.b = val.r;
    }
};


public class Test_ICA
{
	static void Main()
	{
		{
		    Pricer3 pricer = new Pricer3();
            pricer.ds = Pricer3.PlainDataSource;

            double S = 60.0;
            Console.WriteLine("Stock, full generalised version: {0} ",pricer.compute(S));
    	}

	    {
		    GeneralisedDataSource mySource = new GeneralisedDataSource(OptionType.Future);
	    	Pricer3 pricer = new Pricer3();
            pricer.ds = mySource.init;

	    	double S = 60.0;
            Console.WriteLine("Stock, full generalised version: {0} ", pricer.compute(S));
	    }

	}
}
