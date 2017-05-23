// TestXMLSerialiser.cs
//
// Testing XML Serializer. Also XmlReader and XmlWriter.
//
// (C) Datasim Education BV 2011-2013
//

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

public class XMLSerialiser
{
    public static void Main()
    {

        Console.WriteLine("\nXML Serialiser..");

        // Create option data.
        OptionData opt = new OptionData();

        opt.ID = "IBM CALL DEC";
        opt.r = 0.08;
        opt.sig = 0.30;
        opt.K = 65.0;
        opt.T = 0.25;
        opt.b = opt.r;  // Stock option
        opt.otyp = "C";

        // Create an XML serialiser.
        XmlSerializer xs = new XmlSerializer(typeof(OptionData));

        using (Stream s = File.Create("OptionXML.xml"))
        {
            xs.Serialize(s, opt);
        }

        // Recreate the object from the XML file.
        OptionData opt2;

        using (FileStream s = File.OpenRead("OptionXML.xml"))
        {
            opt2 = (OptionData) xs.Deserialize(s);
        }

    //    opt2.print();

        // We now do the same with XmlWriter and XmlReader
        XmlWriterSettings writeSettings = new XmlWriterSettings();
        writeSettings.Indent = true;

        using (XmlWriter wr = XmlWriter.Create("OptionXML2.xml", writeSettings))
        {
            wr.WriteStartElement("Option");
         
            // The recommended way to write XML data (e.g. call XmlConvert to peform 
            // XML-compliant string conversions).
            wr.WriteStartElement("r"); wr.WriteValue(0.1); wr.WriteEndElement();

            wr.WriteStartElement("vol"); wr.WriteValue(0.2); wr.WriteEndElement();

            wr.WriteStartElement("K"); wr.WriteValue(100); wr.WriteEndElement();

            wr.WriteStartElement("T"); wr.WriteValue(0.25); wr.WriteEndElement();

            wr.WriteStartElement("coc"); wr.WriteValue(0.1); wr.WriteEndElement();

            wr.WriteStartElement("type"); wr.WriteValue("Call"); wr.WriteEndElement();

            wr.WriteEndElement();
        }

        XmlReaderSettings readSettings = new XmlReaderSettings();
        readSettings.IgnoreWhitespace = true;

        // Open XML file and print the values
    /*    using (XmlReader reader = XmlReader.Create("OptionXML2.xml", readSettings))
        while (reader.Read())
        {
            Console.Write(new string (' ', reader.Depth*2));   // Depth of current node
            Console.WriteLine(reader.NodeType);                // Type of current node      
            Console.Write(reader.Name);                        // Qualified name of current node           
            Console.WriteLine(reader.Value);                   // Value of current node       
        }*/

        using (XmlReader reader = XmlReader.Create("OptionXML2.xml", readSettings))
        {
            reader.MoveToContent();             // Skip over XML declaration

            reader.ReadStartElement("Option");  // Start with data structure

            // Read the elements in the order as they were written
            string sa = reader.ReadElementContentAsString("r", ""); Console.WriteLine(sa);
            sa = reader.ReadElementContentAsString("vol", ""); Console.WriteLine(sa);
            sa = reader.ReadElementContentAsString("K", ""); Console.WriteLine(sa);
            sa = reader.ReadElementContentAsString("T", ""); Console.WriteLine(sa);
            sa = reader.ReadElementContentAsString("coc", ""); Console.WriteLine(sa);
            sa = reader.ReadElementContentAsString("type", ""); Console.WriteLine(sa);
        }

    
        // Start the process IE) that can process this XML file.
        // Then you view the output.
        // System.Diagnostics.Process.Start("OptionXML.xml");

        // Using serialisation with inheritance.
        // Parameters for Vasicek model
        double r = 0.05;
        double kappa = 0.15;
        double vol = 0.01;
        double theta = r;

        BondModel vasicek = new VasicekModel(kappa, theta, vol, r);

        XmlSerializer xs2 = new XmlSerializer(typeof(BondModel), 
                            new Type[] {typeof(VasicekModel), typeof(CIRModel)});

        using (Stream s = File.Create("Bond.xml"))
        {
            xs2.Serialize(s, vasicek);
        }

        // Serialise an aggregate object
        DateList myDL_1 = new DateList(new Date(2009, 8, 3), new Date(2011, 8, 3), 2, 1);
        //myDL_1.PrintVectDateList();

        // Create an XML serialiser for a cash flow date list.
        XmlSerializer xsDL = new XmlSerializer(typeof(DateList));

        using (Stream s = File.Create("DateList.xml"))
        {
            xsDL.Serialize(s, myDL_1);
        }

        System.Diagnostics.Process.Start("DateList.xml"); //AG added

        // Recreate the object from the XML file.
        DateList myDL_2;

        using (FileStream s = File.OpenRead("DateList.xml"))
        {
            myDL_2 = (DateList)xsDL.Deserialize(s);
        }

        // Using IXmlSerializer.
        OptionDataV optV = new OptionDataV();
        optV.r = 0.08;
        optV.sig = 0.30;
        optV.K = 65.0;
        optV.T = 0.25;
        optV.b = opt.r;  // Stock option
        optV.otyp = "C";


        // Create an XML serialiser.
        XmlSerializer xsV = new XmlSerializer(typeof(OptionDataV));

        using (Stream s = File.Create("OptionXMLV.xml"))
        {
            xsV.Serialize(s, optV);
        }

        // Recreate the object from the XML file.
        OptionDataV optV2;

        using (FileStream s = File.OpenRead("OptionXMLV.xml"))
        {
            optV2 = (OptionDataV)xsV.Deserialize(s);
        }

        optV2.print();

        System.Diagnostics.Process.Start("OptionXMLV.xml");
    }
}

/* output
<?xml version="1.0" ?> 
- <OptionDataV>
  <TypoOption>C</TypoOption> 
  <Tasso>0.08</Tasso> 
  <Volatilita>0.3</Volatilita> 
  <Strike>65</Strike> 
  <Expirazione>0.25</Expirazione> 
  <CostaCarry>0.08</CostaCarry> 
  </OptionDataV>
 * */
