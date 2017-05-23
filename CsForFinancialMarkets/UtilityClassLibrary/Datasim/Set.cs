/* Set.cs
* 
* Set class, based on the Generic Dictionary class
* 
* Stores unique values in a collection
* 
* © 2005 Mark de Haan 
 * 2009-6-12 DD test and SymmetricDifference()
*/

using System;
using System.Collections.Generic;


public class Set<T> : IEnumerable<T>
{
    private int defaultvalue = 0;

    Dictionary<T, int> dict = new Dictionary<T, int>();


    // return the Enumerator (enables the use of foreach with the Set

    public IEnumerator<T> GetEnumerator()
    {
        foreach( T s in dict.Keys )
        {
            // C# 2.0 supports yield to automatically return the expected value
            yield return s;
        }
    }

    /* 
        * there's a bug in visual C#. The following function fixes this.
        * see http://support.microsoft.com/default.aspx?scid=kb;EN-US;836127
        * for more information.
        */

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return null;
    }

    /* end of the bugfix */


    public Set()
    {
        dict.Clear();
    }

    // copy constructor

    public Set( Set<T> set2 )
    {
        // Deep copy of the dictionary, objects are still references
        foreach( KeyValuePair<T, int> obj in set2.dict )
        {
            this.dict.Add( obj.Key, defaultvalue );
        }
    }

    // modification functions ----------------------------------------

    public void Insert( T obj )
    {
        if( !dict.ContainsKey( obj ) )
        {
            dict.Add( obj, defaultvalue );
        }
    }

    // insert another set into this set

    public void Insert( Set<T> set2 )
    {
        Set<T> newSet = new Set<T>();
        newSet = Union( this, set2 );
        dict = newSet.dict;
    }

    // replacing an object in the set, probably not the best way to do this.

    public void Replace( T old, T obj )
    {
        dict.Remove( old );
        dict.Add( obj, defaultvalue );
    }

    // removing an item from the set

    public void Remove( T obj )
    {
        dict.Remove( obj );
    }

    // logical operators -------------------------------------------

    // Intersection: returns a set that contains the objects that the
    // input sets both contain (items that only exist in one set are
    // not copied)

    public static Set<T> Intersection( Set<T> set1, Set<T> set2 )
    {
        Set<T> newSet = new Set<T>();

        foreach( T obj in set1.dict.Keys )
        {
            if( set2.dict.ContainsKey( obj ) )
            {
                newSet.Insert( ( T )obj );
            }
        }

        return newSet;
    }

    // Union: adds two sets together by first copying one set and then
    // copying items of the 2nd set if they are not already present
    // (to make sure there are no duplicates)
    // returns another set

    public static Set<T> Union( Set<T> set1, Set<T> set2 )
    {
        Set<T> newSet = new Set<T>();

        foreach( T obj in set1.dict.Keys )
        {
            newSet.Insert( ( T )obj );
        }
        foreach( T obj in set2.dict.Keys )
        {
            if( !newSet.dict.ContainsKey( obj ) )
            {
                newSet.Insert( ( T )obj );
            }
        }

        return newSet;
    }

    // Difference: returns a set that contains only the objects that
    // only exist in one set, the objects that exist in both sets
    // are not copied.

    public static Set<T> Difference( Set<T> set1, Set<T> set2 )
    {
        Set<T> newSet = new Set<T>();

        foreach( T obj in set1.dict.Keys )
        {
            if( !set2.dict.ContainsKey( obj ) )
            {
                newSet.Insert( ( T )obj );
            }
        }
        foreach( T obj in set2.dict.Keys )
        {
            if( !set1.dict.ContainsKey( obj ) )
            {
                newSet.Insert( ( T )obj );
            }
        }
        return newSet;
    }

    // SymmetricDifference: Not yet implemented (a^b)=(a-b)|(b-a)

    public static Set<T> SymmetricDifference( Set<T> A, Set<T> B )
    {
       //V1 throw new System.NotImplementedException();

        Set<T> result = new Set<T>();

        // Everything in A or B but not both
        result = Set<T>.Union(Set<T>.Difference(A, B), Set<T>.Difference(B, A));
        return result;
    }

    // clearing the set

    public void Clear()
    {
        dict.Clear();
    }

    // information retrieval functions -------------------------------

    /*
	// Relations between sets (s1 == *this)
	bool Subset(const Set<V>& s2) const;	// s1 a subset of s2?
	bool Superset(const Set<V>& s2) const;	// s1 a superset of s2?
	bool Intersects(const Set<V>& s2) const;	// s1 and 22 have common elements? 
	*/



    // Does the set contain this object?

    public Boolean Contains( T obj )
    {
        return dict.ContainsKey( obj );
    }

    // Does the set contain the same objects as the other set?

    public Boolean equals( Set<T> set2 )
    {
        return this.dict.Equals( set2.dict );
    }

    // is the set empty?

    public Boolean Empty()
    {
        bool isEmpty = false;
        if( dict.Count == 0 )
        {
            isEmpty = true;
        }
        return isEmpty;
    }

    // Return number of objects in the set as long integer

    public int Size()
    {
        return dict.Count;
    }

    // operands -------------------------------------

    // + returns a union
    public static Set<T> operator +( Set<T> set1, Set<T> set2 )
    {
        return Union( set1, set2 );
    }

    // - returns a difference
    public static Set<T> operator -( Set<T> set1, Set<T> set2 )
    {
        return Difference( set1, set2 );
    }

    // ^ returns an intersection
    public static Set<T> operator ^( Set<T> set1, Set<T> set2 )
    {
        return Intersection( set1, set2 );
    }

    // % returns a symmetric difference
    public static Set<T> operator %( Set<T> set1, Set<T> set2 )
    {
        return SymmetricDifference( set1, set2 );
    }

    public void print()
    { // Iterate over the set and print its elements

        // Iterate container using while loop
        IEnumerator<T> e = GetEnumerator();
        while (e.MoveNext())
        {
            Console.Write("{0}, ", e.Current);
        }
        Console.WriteLine();

    }

}