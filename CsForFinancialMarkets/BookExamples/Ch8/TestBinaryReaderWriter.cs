// TestBinaryReaderWriter.cs
//
// Program that shows the use of a binary- reader and writer.
//
// (C) Datasim Education BV  2002-2013

using System;
using System.IO;

public class BinaryReaderWriter
{
	public static void Main()
	{
		// Open file stream
		Stream stream=File.Open("Settings.ini", FileMode.Create);

		// Create binary reader and writer
		BinaryWriter writer=new BinaryWriter(stream);
		BinaryReader reader=new BinaryReader(stream);

		// Write "settings"
		writer.Write(true);		// Write boolean
		writer.Write(3);		// Write int
		writer.Write(3.14);		// Write double
		writer.Write("string");	// Write string
		writer.Flush();         // Write stream data to backing
                                // store immediately

		// Seek to begin
		stream.Seek(0, SeekOrigin.Begin);

		// Read "settings"
		Console.WriteLine("Boolean: {0}", reader.ReadBoolean());
		Console.WriteLine("Integer: {0}", reader.ReadInt32());
		Console.WriteLine("Double: {0}", reader.ReadDouble());
		Console.WriteLine("String: {0}", reader.ReadString());

		// Close stream
		stream.Close();


        // Using BinaryReader and BinaryWriter with class data
        Stream s = new FileStream("data.tmp", FileMode.Create, FileAccess.ReadWrite);

        // Create option data
        OptionData opt = new OptionData();

        opt.r = 0.08;
        opt.sig = 0.30;
        opt.K = 65.0;
        opt.T = 0.25;
        opt.b = opt.r;  // Stock option
        opt.otyp = "C";

        // BinaryWriter based on file stream
        BinaryWriter optWriter = new BinaryWriter(s);
        opt.SaveData(optWriter);

        optWriter.Close();

        // BinaryERader based on file stream
        Stream s2; 
        BinaryReader optReader = null;
        try
        {
            s2 = new FileStream("data.tmp", FileMode.Open, FileAccess.ReadWrite);
            optReader = new BinaryReader(s2);
            opt.ReadData(optReader);

            opt.print();
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (optReader != null)
            {
                optReader.Close();
            }
        }
               
	}
}
