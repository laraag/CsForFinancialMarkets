// OptionData_V.cs
//
// Using IXmlSerializer.
//
// (C) Datasim Education BV 2011-2013
//

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class OptionDataV : IXmlSerializable
{
    // Member data public for convenience only
    // IXmlSerialisable works with private variables.
    public double? r;		// Interest rate
    public double sig;	    // Volatility
    public double K;		// Strike price
    public double T;		// Expiry date
    public double b;		// Cost of carry

    public string otyp;	    // Option name (call, put)


    public XmlSchema GetSchema() { return null; }
 
    public void ReadXml(XmlReader reader)
    {
        try
        {
            // We show how to read the data back using two different options
            // Note that data is read in the _same_ order as it was written.
            reader.ReadStartElement();
                otyp = reader.ReadElementContentAsString();
                r = reader.ReadElementContentAsDouble();

                sig = reader.ReadElementContentAsDouble();
                K = reader.ReadElementContentAsDouble();
                T = reader.ReadElementContentAsDouble();
                b = Convert.ToDouble(reader.ReadElementContentAsString());
              /*  sig = Convert.ToDouble(reader.ReadElementContentAsString());
                K = Convert.ToDouble(reader.ReadElementContentAsString());
                T = Convert.ToDouble(reader.ReadElementContentAsString());
                b = Convert.ToDouble(reader.ReadElementContentAsString());*/
                
            reader.ReadEndElement();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("TypoOption", otyp);
        writer.WriteElementString("Tasso", Convert.ToString(r));
        writer.WriteElementString("Volatilita", Convert.ToString(sig));
        writer.WriteElementString("Strike", Convert.ToString(K));
        writer.WriteElementString("Expirazione", Convert.ToString(T));
        writer.WriteElementString("CostaCarry", Convert.ToString(b));
    }

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