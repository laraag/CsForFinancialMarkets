// TrinomialParameters.cs
//
// Simple structure to hold option data and numeric data for 
// the two-factor binomial method.
//
// This is a quick and dirty data structure just to test the 
// 2-factor binomial. But it works.
//
// 2011-1-25 DD C# version.
//
// (C) Datasim Education BV 2011-2013
//

public struct TrinomialParameters
{
	// Option data
	public double sigma;
	public double T;
	public double r;
	public double K;
	public double div;			// Dividend
	public char type;			// 'C' or 'P'
	public bool exercise;		// false if European, true if American	

	// 'Numeric' data
	public int NumberOfSteps;	// Nr. of subdivisions of [0, T]
}

