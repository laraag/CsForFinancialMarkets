// TestFileStream.cs
//
// Small application that show reading and writing
// files using a file stream.
//
// (C) Datasim Education BV  2002-2013

using System;
using System.IO;

public class MainClass
{
	public static void Main()
	{
		try
		{
			TestFileStream();
		}
		catch (Exception e)
		{
			Console.WriteLine("Error: {0}", e.Message);
		}
	}

	// Test file stream
	public static void TestFileStream()
	{
		// Create FileStream on new file with read/write access.
		// Note that after create we use a Stream reference to access 
		// the FileStream thus we can easily replace it with another kind of stream.
		Stream s=new FileStream("data.tmp", FileMode.Create, FileAccess.ReadWrite);

		// We can use a MemoryStream instead
//		Stream s=new MemoryStream(10);

		// If we can write, write something
		if (s.CanWrite)
		{
			byte[] buffer={0, 1, 2, 3, 4, 5, 6, 7, 8, 9};	// Data to write
			s.Write(buffer, 0, buffer.Length);				// Write it
			s.Flush();										// Flush the buffer
		}

		// Set the stream position to the beginning
		if (s.CanSeek) s.Seek(0, SeekOrigin.Begin);

		// If we can read, read it
		if (s.CanRead)
		{
			byte[] buffer=new byte[s.Length];				// Create buffer with file length
			int count=s.Read(buffer, 0, buffer.Length);		// Read the whole file

			// Print byte count
			Console.WriteLine("{0} bytes read.", count);

			// Print every byte in the buffer
			foreach (byte b in buffer) Console.Write(b.ToString() + ", ");
			Console.WriteLine();
		}

		// Close the stream
		s.Close();
	}

}