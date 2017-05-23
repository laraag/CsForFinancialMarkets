// Option.cs
//
// Class that represents exact solutions to European options.
//
//	Last modification dates:
//	2002-12-7 DanielDuffy first code
//	2002-12-8 DD tested call and put options for a range of Euro types
//	Some new functions for interoperability
//	2002-12-10 DD almost finished with the code for the European exact option formulae.
//  2005-10-16 DD For intro C++ book
//  2005-10-18 DD last test, everything OK?
//  2010-8-1 DD C# version
//  2010-8-11 DD SpecialFunctions.N(x) and SpecialFunctions.N(x) now from a separate class SpecialFunctions
//  2013-1-18 AG Bugs  SpecialFunctions.n vs SpecialFunctions.N (Gamma Vega Theta formulas), see lines 83,98,114,130
// (C) Datasim Education BV 2003-2010
//

using System;

public class Option
{
    public double r;		// Interest rate
    public double sig;	    // Volatility
    public double K;		// Strike price
    public double T;		// Expiry date
    public double b;		// Cost of carry

    public string otyp;	    // Option name (call, put)


    // Kernel Functions (Haug)
    private double CallPrice(double U)
    {
        Console.WriteLine("{0}, {1}, {2}, {3}, {4}", sig, T, K, r, b);
        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;
        Console.WriteLine("{0}, {1}", d1, d2);
        return (U * Math.Exp((b - r) * T) * SpecialFunctions.N(d1)) - (K * Math.Exp(-r * T) * SpecialFunctions.N(d2));

    }

    private double PutPrice(double U)
    {

        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;
        return (K * Math.Exp(-r * T) * SpecialFunctions.N(-d2)) - (U * Math.Exp((b - r) * T) * SpecialFunctions.N(-d1));

    }

    private double CallDelta(double U)
    {
        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;


        return Math.Exp((b - r) * T) * SpecialFunctions.N(d1);
    }

    private double PutDelta(double U)
    {
        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;

        return Math.Exp((b - r) * T) * (SpecialFunctions.N(d1) - 1.0);
    }

    private double CallGamma(double U)
    {
        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

       // cout << "denom, U " << tmp << ", " << U << endl;

        return (SpecialFunctions.n(d1) * Math.Exp((b - r) * T)) / (U * tmp); // AG: here SpecialFunctions.n vs SpecialFunctions.N
    }

    private double PutGamma(double U)
    {
        return CallGamma(U);
    }


    private double CallVega(double U)
    {
        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;

        return (U * Math.Exp((b - r) * T) * SpecialFunctions.n(d1) * Math.Sqrt(T)); // AG: here SpecialFunctions.n vs SpecialFunctions.N
    }

    private double PutVega(double U)
    {
        return CallVega(U);
    }

    private double CallTheta(double U)
    {

        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

        double t1 = (U * Math.Exp((b - r) * T) * SpecialFunctions.n(d1) * sig * 0.5) / Math.Sqrt(T); // AG: here SpecialFunctions.n vs SpecialFunctions.N
        double t2 = (b - r) * (U * Math.Exp((b - r) * T) * SpecialFunctions.N(d1));
        double t3 = r * K * Math.Exp(-r * T) * SpecialFunctions.N(d2);

        return -(t1 + t2 + t3);
    }


    private double PutTheta(double U)
    {

        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

        double t1 = (U * Math.Exp((b - r) * T) * SpecialFunctions.n(d1) * sig * 0.5) / Math.Sqrt(T); // AG: here SpecialFunctions.n vs SpecialFunctions.N
        double t2 = (b - r) * (U * Math.Exp((b - r) * T) * SpecialFunctions.N(-d1));
        double t3 = r * K * Math.Exp(-r * T) * SpecialFunctions.N(-d2);

        return t2 + t3 - t1;
    }

    private double CallRho(double U)
    {

        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

        if (b != 0.0)
            return T * K * Math.Exp(-r * T) * SpecialFunctions.N(d2);
        else
            return -T * CallPrice(U);

    }


    private double PutRho(double U)
    {
        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

        if (b != 0.0)
            return -T * K * Math.Exp(-r * T) * SpecialFunctions.N(-d2);
        else
            return -T * PutPrice(U);
    }


    private double CallCoc(double U)
    {

        double tmp = sig * Math.Sqrt(T);
        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;

        return T * U * Math.Exp((b - r) * T) * SpecialFunctions.N(d1);
    }


    private double PutCoc(double U)
    {
        double tmp = sig * Math.Sqrt(T);
        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;

        return -T * U * Math.Exp((b - r) * T) * SpecialFunctions.N(-d1);
    }

    private double CallElasticity(double percentageMovement, double U)
    {

        return (CallDelta(U) * U) / percentageMovement;
    }

    private double PutElasticity(double percentageMovement, double U)
    {
        return (PutDelta(U) * U) / percentageMovement;
    }

    private double CallStrike(double U)
    { // As a function of the strike price

        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

        return -Math.Exp(-r * T) * SpecialFunctions.N(d2);
    }

    private double PutStrike(double U)
    {
        double tmp = sig * Math.Sqrt(T);
        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

        return Math.Exp(-r * T) * SpecialFunctions.N(-d2);
    }

    private double CallSecondStrike(double U)
    { // As a function of the strike price

        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

        return (SpecialFunctions.N(d2) * Math.Exp(-r * T)) / (K * sig * Math.Sqrt(T));
    }

    private double PutSecondStrike(double U)
    {
        double tmp = sig * Math.Sqrt(T);

        double d1 = (Math.Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
        double d2 = d1 - tmp;

        return (SpecialFunctions.N(d2) * Math.Exp(-r * T)) / (K * sig * Math.Sqrt(T));
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void init()
    {	// Initialise all default values


        r = 0.08;
        sig = 0.30;
        K = 65.0;
        T = 0.25;
        b = r;

        otyp = "C";
    }


    public Option()
    { // Default call option

        init();
    }


    
    public Option(string optionType, string underlying)
    { // Create option type

        init();
        otyp = optionType;
    }



    // Functions that calculate option price and sensitivities
    public double Price(double U)
    {

        if (otyp == "C")
        {
            Console.WriteLine("call..{0}", U);
            return CallPrice(U);
        }
        else
            return PutPrice(U);

    }

    public double Delta(double U)
    {
        if (otyp == "C")
            return CallDelta(U);
        else
            return PutDelta(U);

    }


    public double Gamma(double U)
    {
        if (otyp == "C")
            return CallGamma(U);
        else
            return PutGamma(U);

    }

    public double Vega(double U)
    {
        if (otyp == "C")
            return CallVega(U);
        else
            return PutVega(U);

    }

    public double Theta(double U)
    {
        if (otyp == "C")
            return CallTheta(U);
        else
            return PutTheta(U);

    }

    public double Rho(double U)
    {
        if (otyp == "C")
            return CallRho(U);
        else
            return PutRho(U);

    }


    public double Coc(double U)
    { // Cost of carry

        if (otyp == "C")
            return CallCoc(U);
        else
            return PutCoc(U);

    }

    public double Elasticity(double percentageMovement, double U)
    { // Elasticity

        if (otyp == "C")
            return CallElasticity(percentageMovement, U);
        else
            return PutElasticity(percentageMovement, U);

    }



    // Modifier functions
    public void Toggle()
    { // Change option type (C/P, P/C)

        if (otyp == "C")
            otyp = "P";
        else
            otyp = "C";
    }

   
}