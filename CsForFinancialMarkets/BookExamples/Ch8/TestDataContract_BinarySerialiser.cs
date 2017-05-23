// TestDataContract_BinarySerialiser.cs
//
// Testing Binary Serializer.
//
// (C) Datasim Education BV 2011-2013
//

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

public class BinarySerialiser
{
    public static void Main()
    {

        Console.WriteLine("\nBinary Serialiser..");

        // Create option data.
        OptionData opt = new OptionData();

        opt.r = 0.08;
        opt.sig = 0.30;
        opt.K = 65.0;
        opt.T = 0.25;
        opt.b = opt.r;  // Stock option
        opt.otyp = "C";

        // Create a binary formatter
        IFormatter formatter = new BinaryFormatter();

        using (FileStream s = File.Create("Option.bin"))
        {
            formatter.Serialize(s, opt);
        }

        // Recreate the object from the XML file.
        OptionData opt2;

        using (FileStream s = File.OpenRead("Option.bin"))
        {
            opt2 = (OptionData) formatter.Deserialize(s);
        }

        opt2.print();

        Console.WriteLine("\nProgram end..");

    }
}