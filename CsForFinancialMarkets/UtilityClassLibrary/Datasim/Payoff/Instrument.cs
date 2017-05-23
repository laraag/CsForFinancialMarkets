// Instrument.cs
//
// Classes for instruments. Two Factor models.
//
// N.B.
// N.B. This class is special because it models what is 
// essentially a two-factor basket PUT option.
// 
// You can easily modify this class for your own objectives. 
// But take care of the data for the payoff functions!
//
// See chapters 24 and 32 of my FDM book for more details. 
//
// ALSO
//			Topper 2005, page 216 (Case Study, Basket Option)
//			(3D case)
//			Topper 2005, page 197(Case Study, Basket Optioon)
//			(2D case)
//			Bhansali 1998, pages 210-212 
//			
//
// 2006-1-31 DD new name TwoFactorOptionData + now work with a Specific
// BasketStrategy
// 2006-2-1 DD Prototype Factory created (save typing in information during debugging)
// 2010-8-1 DD C# version
//
// (C) Datasim Education BV 2006-2010
//

using System;

public class TwoFactorInstrument
{

}

public class TwoFactorOption : TwoFactorInstrument
{
	// An option uses a polymorphic payoff object
	public MultiAssetPayoffStrategy pay;

	public double r;				// Interest rate
	public double D1, D2;			// Dividends
	public double sig1, sig2;		// Volatility
	public double rho;				// Cross correlation
	public double K;				// Strike price, now place in IC
	public double T;				// Expiry date
	public double SMax1, SMax2;	// Far field condition
	public double w1, w2;
	public int type;				// Call +1 or put -1

	public TwoFactorOption()
	{
		// Initialise all data and the payoff

		// Use Topper's data as the default, Table 7.1

		r = 0.01;
		D1 = D2 = 0.0;
		sig1 = 0.1; sig2 = 0.2;
		rho = 0.5;
		
		K = 40.0;
		T = 0.5;

		w1 = 1.0;
		w2 = 20.0;

		type = -1;

		// Now create the payoff Strategy object
		/* BasketStrategy(double strike, double cp,double weight1, double weight2)
		{ K = strike; w = cp; w1 = weight1; w2 = weight2;} */
		
		pay = new BasketStrategy(K, type, w1, w2);

		// In general 1) we need a Factory object as we have done
		// in the one-dimensional case and 2) work with pointers to
		// base classes
	}

	public double payoff(double x, double y) 
	{
		return pay.payoff(x, y);
	}

}

public abstract class InstrumentFactory
{

	public abstract TwoFactorOption CreateOption();

}

public class ConsoleInstrumentFactory : InstrumentFactory
{

	public override TwoFactorOption CreateOption()
	{

		double dr;		        // Interest rate
		double dsig1, dsig2;	// Volatility
		double div1, div2;	    // Dividends
		double drho;
		double dK;		        // Strike price
		double dT;		        // Expiry date
		double dSMax1, dSMax2;	// Far field boundary

		double dw1, dw2;	    // Weights of each asset
        int dtype;              // C +1, P -1
L1:
		Console.Write( "Type Call +1, Put -1: ");
        dtype = Convert.ToInt32(Console.ReadLine());

		Console.Write( "Interest rate: ");
        dr = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Strike: ");
        dK = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Volatility 1: ");
        dsig1 = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Volatility 2: ");
        dsig2 = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Dividend 1: ");
        div1 = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Dividend 2: ");
        div2 = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Correlation: ");
        drho = Convert.ToDouble(Console.ReadLine());

		Console.Write( "First Far Field: ");
        dSMax1 = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Second Far Field: ");
        dSMax2 = Convert.ToDouble(Console.ReadLine());

		Console.Write( "weight 1: ");
        dw1 = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Weight 2: ");
        dw2 = Convert.ToDouble(Console.ReadLine());


		Console.Write( "Expiry: ");
        dT = Convert.ToDouble(Console.ReadLine());

		Console.Write( "Is this correct, carry on?");
        char c = Convert.ToChar(Console.ReadLine());


		if (c != 'y' && c != 'Y')
		{
			goto L1;
		}

		TwoFactorOption result = new TwoFactorOption();

		result.r	= dr;
		result.sig1	= dsig1;
		result.sig2	= dsig2;
		result.rho		= drho;

		result.D1		= div1;
		result.D2		= div2;
	
		result.K		= dK;
		result.T		= dT;
		result.SMax1	= dSMax1;
		result.SMax2	= dSMax2;

		result.type	= dtype;
	
		result.w1		= dw1;
		result.w2		= dw2;
	
		// Now assign the payoff function
		result.pay = new BasketStrategy(dK, dtype, dw1, dw2);


		return result;

	}	
}

public class PrototypeInstrumentFactory : InstrumentFactory
{ // Returns a prototype object

	public override TwoFactorOption CreateOption() 
	{

		TwoFactorOption result = new TwoFactorOption();

		// Results take from Topper 2005
		result.r		= 0.1;
		result.sig1	= 0.1;
		result.sig2	= 0.3;
		result.rho		= 0.5;

		result.D1		= 0.0;
		result.D2		= 0.0;
	
		result.K		= 40.0;
		result.T		= 0.5;
		result.SMax1	= result.K * 2.0;
		result.SMax2	= result.K * 2.0;

		result.type	= -1;	// Put, temp
	
		result.w1		= 1.0; // Topper
		result.w2		= 1.0;
	
		// Now assign the payoff function
		result.pay = new BasketStrategy(result.K, result.type, result.w1, result.w2);

		return result;

	}	
}
