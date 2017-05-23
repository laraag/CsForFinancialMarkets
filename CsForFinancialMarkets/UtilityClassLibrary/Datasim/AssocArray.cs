// AssocArray.cs
//
// Associative array class. In this case we access elements by 
// using non-integral indices.
// This is a proof-of-concept (POC) class.
//
// Last modification dates:
//
//	16 August 2003 DD kick off
//	12 January 2004 DD some new member functions
//	2006-3-24 DD use Set as control; must implement exceptions
//	2009-1-8 DD update, more functionality
//  2009-3-18 DD ported to C#
//
// (C) Datasim Component Technology 1999-2010

using System;
using System.Collections.Generic;


public class AssocArray<Key, Value> : IEnumerable<KeyValuePair<Key, Value>>
{

 	Dictionary<Key, Value> str;	// The list of associative values, e.g. strings
		
	// Redundant information for performance
	Set<Key> keys;

public AssocArray()
{ // Default constructor

	str = new Dictionary<Key, Value>();
	keys = new Set<Key>();
}

 
public AssocArray( AssocArray<Key,Value> arr2)
{ // Copy constructor

	str = new Dictionary<Key, Value>(arr2.str);
	keys = new Set<Key>(arr2.keys);

}
// Construct the map from a list of names and a REPEATED val
public AssocArray( Set<Key> names,  Value val)
{

	keys = new Set<Key>(names);
    str = new Dictionary<Key, Value> ();

    // Now build up the dictionary
	foreach(Key k in names)
	{
		str[k] = val;

	}

}

 
public AssocArray( Set<Key> names,  Array<Value> valArray)
{


    keys = new Set<Key>(names);
    str = new Dictionary<Key, Value> ();

    // PREC: number of elements in Set == size of array
	int index = valArray.MinIndex;
    
	foreach(Key k in names)
	{
		str[k] = valArray[index++];
        
	}

}


// Enumerator region
public IEnumerator<KeyValuePair<Key, Value> > GetEnumerator()
{
        foreach( KeyValuePair<Key, Value>  kvp in str )
        {
            // C# 2.0 supports yield to automatically return the expected value
            // bug, so we need 2 versions of GetEnumerator()
            yield return kvp;
        }
 }

System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
{
    return null;
}

// End

 
// New overloaded indexing operator
public Value this [Key index]
{

	   get
        {
            return str[index];
        }
        set
        {
            str[index] = value;
        }


}


public long Size 
{ 
    get
    {
        return str.Count;
    }
}

 
 public Set<Key> Keys
{ 
     get
     {
	    return keys; 
     }
}

 public void print()
 {
     foreach (KeyValuePair<Key, Value> kvp in str)
     {
         Console.WriteLine("{0}, {1}", kvp.Key, kvp.Value);  
     }  
    
 }

} // End of class


