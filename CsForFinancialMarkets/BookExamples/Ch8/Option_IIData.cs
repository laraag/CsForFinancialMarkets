// Option_IIData.cs
//
// Class that uses DataContractSerializer for persistence.
//
// (C) Datasim Education BV 2011
//

using System;
using System.IO;
using System.Runtime.Serialization;

[DataContract] public class OptionData
{
    // Member data public for convenience
    [DataMember] public double r;		// Interest rate
    [DataMember] public double sig;   // Volatility
    [DataMember] public double K;		// Strike price
    [DataMember] public double T;		// Expiry date
    [DataMember] public double b;		// Cost of carry

    [DataMember] public string otyp;	    // Option name (call, put)

    
    public void print()
    {
        Console.WriteLine("\nOption data:\n");
        Console.WriteLine("interest: {0}", r);
        Console.WriteLine("volatility: {0}", sig);
        Console.WriteLine("Strike: {0}", K);
        Console.WriteLine("Expiry: {0}", T);
        Console.WriteLine("Cost of carry: {0}", b);
        Console.WriteLine("Option type: {0}", otyp);
    }
}