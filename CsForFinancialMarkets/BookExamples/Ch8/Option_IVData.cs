// OptionData_IV.cs
//
// Using Binary Serializer.
//
// (C) Datasim Education BV 2011
//

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[Serializable] public class OptionData
{
    // Key data
    [XmlAttribute ("OptionId")] public string ID;

    // Member data public for convenience
    public double r;		// Interest rate
    [XmlIgnore] public double sig;	    // Volatility
    public double K;		// Strike price
    public double T;		// Expiry date
    public double b;		// Cost of carry

    public string otyp;	    // Option name (call, put)


    public void print()
    {
        Console.WriteLine("\nOption data:\n");

        Console.WriteLine("Option ID: {0}", ID);
        Console.WriteLine("interest: {0}", r);
        Console.WriteLine("volatility: {0}", sig);
        Console.WriteLine("Strike: {0}", K);
        Console.WriteLine("Expiry: {0}", T);
        Console.WriteLine("Cost of carry: {0}", b);
        Console.WriteLine("Option type: {0}", otyp);
    }
}