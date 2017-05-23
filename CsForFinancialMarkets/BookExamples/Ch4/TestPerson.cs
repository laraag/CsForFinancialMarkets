// Person.cs
//
// (C) Datasim Education BV  2002

using System;

// Person class implementing IName interface
public class Person: IName
{
	private string m_name;

	// Implement property from IName interface
	public string Name
	{
		// Get property implementation from IName interface
		get
	    { 
			return m_name;
	    }

		// Set property implementation NOT from IName interface
		set
		{ 
			m_name=value;
		}
	}

	public static void Main()
	{
		Person p=new Person();

		p.Name="John";
		Console.WriteLine(p.Name);
	}
}