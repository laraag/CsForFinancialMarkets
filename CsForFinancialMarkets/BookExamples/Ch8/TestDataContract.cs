// TestDataContract.cs
//
// Testing DC Serializer.
//
// (C) Datasim Education BV 2011-2013
//

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

/*Namespace:  System.Runtime.Serialization
Assembly:  System.Runtime.Serialization (in System.Runtime.Serialization.dll)
 */
public class DCSerialiser
{
    public static void Main()
    {
        // Create option data.
        OptionData opt = new OptionData();

        opt.r = 0.08;
        opt.sig = 0.30;
        opt.K = 65.0;
        opt.T = 0.25;
        opt.b = opt.r;  // Stock option
        opt.otyp = "C";

        // Create the serialiser and write to disk.
        DataContractSerializer ds = new DataContractSerializer(typeof(OptionData));

        using (Stream s = File.Create("Option.xml"))
            ds.WriteObject(s, opt);

        // Recreate the object from the XML file.
        OptionData opt2;

        using (Stream s = File.OpenRead("Option.xml"))
            opt2 = (OptionData) ds.ReadObject(s);

        opt2.print();

        // Extra extensions to improve readability, for example.
        XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };

        using (XmlWriter xmlW = XmlWriter.Create("Option_II.xml", settings))
        {
            ds.WriteObject(xmlW, opt); // 'ds' and 'opt' as before.
        }

        // Start the process (IE) that can process this XML file.
        // Then you view the output.
        System.Diagnostics.Process.Start("Option_II.xml");

        // Create the Net data serialiser and write to disk.
        NetDataContractSerializer ds2 = new NetDataContractSerializer();

        Console.WriteLine("\nProgram end..");

    }
}

/* Output
<?xml version="1.0" encoding="utf-8" ?> 
- <OptionData xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/">
  <ID i:nil="true" /> 
  <K>65</K> 
  <T>0.25</T> 
  <b>0.08</b> 
  <otyp>C</otyp> 
  <r>0.08</r> 
  <sig>0.3</sig> 
  </OptionData>

*/