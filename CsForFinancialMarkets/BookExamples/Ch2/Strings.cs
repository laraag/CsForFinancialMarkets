// Strings.cs
//
// Example program that shows the usage of strings.
//
// (C) Datasim Education BV  2002-2013

using System;

public class Strings
{
	/// <summary>
	/// The entry point of the program.
	/// </summary>
	public static void Main()
	{
		string s1="Hello World";				// Create new string
		Console.WriteLine(s1);

		string s2=s1 + "Hello World";			// Concatenate two strings
		Console.WriteLine(s2);

		s1="New String";						// Put new string in existing reference
		Console.WriteLine(s1);

		string str="Price:\t\u20AC10.00\\";		// "Price:	€10.00\"
 		Console.WriteLine(str);
        
		string path1="c:\\test\\newfile.txt";
		string path2=@"c:\test\newfile.txt";	// Same as above
		Console.WriteLine(path1);
		Console.WriteLine(path2);
	}
}



