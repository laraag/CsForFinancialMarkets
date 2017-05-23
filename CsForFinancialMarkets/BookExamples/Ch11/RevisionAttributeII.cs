// RevisionAttribute.cs
//
// Custom attribute to track revisions.
//
// (C) Datasim Education BV  2002-20013

using System;

// Revision attribute
// Has two positional arguments: Date and Message
// Has one named argument: Author
[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
public class RevisionAttribute : Attribute  //AG renamed from RevisionAttributeII
{ 
	private string m_date;
	private string m_msg;
	private string m_author;

	// Constructor
	public RevisionAttribute(string date, string msg)
	{ 
		m_date=date;
		m_msg=msg;
	}

	// Date property
	public string Date
	{ 
		get { return m_date; }
	}

	// Message property 
	public string Message
	{ 
		get { return m_msg; }
	}

	// Author property
	public string Author
	{
		get { return m_author; }
		set { m_author=value; }
	}
}
