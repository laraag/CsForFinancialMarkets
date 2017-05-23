// OptionData.cs
//
// Test case to show the use of binary ReadDoubleers and writers for
// configuration data in finance.
//
// (C) Datasim Education BV 2011-2013
//

using System;
using System.IO;

[Serializable]
public class OptionData
{   
    public string ID;

    // Member data public for convenience
    public double r;		// Interest rate
    public double sig;	    // Volatility
    public double K;		// Strike price
    public double T;		// Expiry date
    public double b;		// Cost of carry

    public string otyp;	    // Option name (call, put)


    public void SaveData(BinaryWriter bw)
    {
      
        bw.Write(r);
        bw.Write(sig);
        bw.Write(K);
        bw.Write(T);
        bw.Write(b);
        bw.Write(otyp);

        bw.Flush();     // clear BinaryWriter buffer
    }

    public void ReadData(BinaryReader br)
    {
        r = br.ReadDouble();
        sig = br.ReadDouble();
        K = br.ReadDouble();
        T = br.ReadDouble();
        b = br.ReadDouble();
        otyp = br.ReadString();
     
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