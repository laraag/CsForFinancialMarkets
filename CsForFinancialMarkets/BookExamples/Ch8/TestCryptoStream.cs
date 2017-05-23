// CryptoStream.cs
//
// Program that shows the input file with Base64 tranformation.
//
// (C) Datasim Education BV  2002-2013


using System;
using System.IO;
using System.Security.Cryptography;


public class MainClassCrypto
{

	public static void Main(string[] args)
	{
		// Check number of parameters
		if (args.Length==0) 
		{
			Console.WriteLine("Usage: CryptoStream file");
			return;
		}

		// Create a stream from the file to read
		Stream stream=new FileStream(args[0], FileMode.Open, FileAccess.Read);

		// Create a Base64 transformation object
		ICryptoTransform transform=new ToBase64Transform();

		// Create a crypto stream from the input stream and the Base64 transformation
		CryptoStream cs=new CryptoStream(stream, transform, CryptoStreamMode.Read);

		// Read the transformed stream with a text reader
		TextReader reader=new StreamReader(cs);

		// Display the transformed stream
		Console.WriteLine(reader.ReadToEnd());
	}
}
