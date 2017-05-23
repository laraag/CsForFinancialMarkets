// TestAttribute.cs
// (C) Datasim Education BV 2002-2013

using System;
using System.Diagnostics;

// Revision is a custom attribute
[Revision("1 March 2002", "First version completed")]
[Revision("26 March 2002", "MultiplyInt renamed to Multiply")]
public class AttributeTest
{
	[Conditional("DEBUG")]
	public static void Log(string msg)
	{ // Log message (only in "DEBUG" build)

		Console.WriteLine("Logging: {0}", msg);
	}

	[Obsolete("Use the better named \"Multiply\" method instead")]
	public static int MultiplyInt(int i, int j)
	{ // Multiply integers (Method in version 1)
	  // We realised that Multiply is a better name than MultiplyInt
	  // Because of compatibility reasons we cannot just remove the MultiplyInt method
	  // Therefore we make this method "Obsolete"

		return Multiply(i, j);
	}

	public static int Multiply(int i, int j)
	{ // Multiply integers. (Version 2, better named)

		return i*j;
	}

	public static void Main()
	{ // Test application

		int i=34;
		int j=134;

		// Do multiplication
		Log("Before operation");
		int result=Multiply(i, j);
		Log("After operation");
		Console.WriteLine("{0} * {1} = {2}", i, j, result);

		// Print revision history
		Console.WriteLine();
		PrintRevisionHistory(typeof(AttributeTest));
	}

	public static void PrintRevisionHistory(Type t)
	{ // Print the revision history of class

		Console.WriteLine("Revision history of {0} class:", t.FullName);

		// Get the revision attributes
		object[] attributes=t.GetCustomAttributes(typeof(RevisionAttribute), false);

		// Print the attributes
		foreach (RevisionAttribute att in attributes)
		{
			Console.WriteLine("- {0}\t{1}", att.Date, att.Message);
		}
	}
}

// AG commented
//[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
//public class RevisionAttribute: System.Attribute
//{ // Revision attribute

//    private string date;
//    private string msg;

//    public RevisionAttribute(string date, string msg)
//    { // Constructor

//        this.date=date;
//        this.msg=msg;
//    }

//    public string Date
//    { // Date property

//        get { return this.date; }
//    }

//    public string Message
//    { // Message property 

//        get { return this.msg; }
//    }
//}