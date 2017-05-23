// Array.cs
//
// Array class. It uses a standard m_array of type T to store the data.
// The user can create m_arrays with all kinds of start indexes.
//
// 23 Oktober 2005 Jannes Albrink
// 08 November 2005 ---Update---
// Renamed variables 'Head' and 'Arr' to 'm_startIndex' and 'm_arr'.
// Removed variable 'Tail'.
// Added constructor to copy an Array and it's startindex.
// Added constructor to create a new Array with an initial value
// Removed try-catch block in index operator method.
// Renamed all variables in method parameters that started with an uppercase character,
// they were renamed to the same name but then starting with a lowercase character.
//
// 23 Oktober 2005 Jannes Albrink
// 11 November 2005 ---Update---
// Added method: public override String ToString()
// Method was added for test purpose only, can be removed if it's unwanted.
//
// 2008-12-4 DD code review + mods
// 2009-6-12 DD provide for reference an/or values for T
// 2010-4-2 DD implments IVectorIndexer<T>
//
// (C) Datasim Component Technology 2005-2009

using System;
using System.Text;

public class Array<T> // : IVectorIndexer<T>
{
    // Class variables

    protected T[] m_arr;            // Standard C# syntax
    protected int m_startIndex;     // User-definable start index

    // Default constructor
    public Array()
    {
        m_startIndex    = 1;
        m_arr           = new T[ 10 ];
    }

    //Constructor with start-index is 1
    public Array( int length )
    {
        m_startIndex    = 1;
        m_arr           = new T[ length ];
    }

    //Constructor with length and start-index parameters
    public Array( int length, int minIndex )
    {
        m_startIndex    = minIndex;
        m_arr           = new T[ length ];

        // Values undefined!
           
    }

    //Constructor with length, start-index and initial value parameters
    public Array( int length, int minIndex, T initvalue )
    {
        m_startIndex    = minIndex;
        m_arr           = new T[ length ];

        for( int i = 0; i < length; i++ )
        {
          if (initvalue is ICloneable) m_arr[i] = (T) (initvalue as ICloneable).Clone();

          else
                m_arr[i] = initvalue;
        }
    }

    //Constructor with copy function for a normal array and start-index 1
    public Array( T[] source )
    {
        m_startIndex    = 1;
        m_arr           = new T[ source.Length ];

        for( int i = 0; i < source.Length; i++ )
        {

           if (source[i] is ICloneable) m_arr[i] = (T) (source[i] as ICloneable).Clone();
           else 
                m_arr[i] = source[i];
            
        }
    }
  
    //Constructor with copy function for a normal array and start-index parameters
    public Array( T[] source, int minIndex )
    {
        m_startIndex    = minIndex;
        m_arr           = new T[ source.Length ];

        for( int i = 0; i < source.Length; i++ )
        {

                if (source[i] is ICloneable) m_arr[i] = (T) (source[i] as ICloneable).Clone();
                else
                    m_arr[i] = source[i];
        }
    }

    //Constructor with copy function for an Array, also copies the startindex
    public Array( Array<T> source )
    {
        m_startIndex    = source.m_startIndex;
        m_arr           = new T[ source.Length ];

        for( int i = source.MinIndex; i <= source.MaxIndex; i++ )
        {
            if (source[i] is ICloneable) m_arr[i] = (T)(source[i] as ICloneable).Clone();
            else
                this[i] = source[i];
        }
    }


    //Use instance of class as m_array, for the use of the braces
    public T this[ int pos ]
    { // User-defined position

        get
        {
            return m_arr[  pos  - m_startIndex ];
        }
        set
        {
            m_arr[ pos  - m_startIndex ] = value;
        }
    }


    // Return the length of the m_array
    public int Length // aka Size
    {
        get
        {
            return m_arr.Length;
        }
    }

    //Return the minimum index of the m_array
    public int MinIndex
    {
        get
        {
            return m_startIndex;
        }
    }

    //Return the maximum index of the m_array
    public int MaxIndex
    {
        get
        {
            return ( m_startIndex + ( m_arr.Length - 1 ) );
        }
    }

    public int Size
    {
        get
        {
            return m_arr.Length;
        }
    }

    //Return all elements in the array in a String
    override public String ToString()
    {
        StringBuilder str = new StringBuilder();
        for( int i = MinIndex; i <= MaxIndex; i++ )
        {
            str.Append( this[ i ] + " " );
        }

        return str.ToString();
    }

    public void print()
    {
       
        for (int i = MinIndex; i <= MaxIndex; i++)
        {
            Console.Write(this[i]); Console.Write(", ");
        }
        Console.WriteLine();
    }

    public void extendedPrint()
    {
    /*    Console.Write("Array, size: "); Console.Write(Size); Console.Write(", Min index: "); Console.Write(MinIndex);
        Console.Write(", Max index: "); Console.Write(MaxIndex); Console.WriteLine();
*/

        for (int i = MinIndex; i <= MaxIndex; i++)
        {
            Console.Write(this[i]); Console.Write(", ");
        }
        Console.WriteLine();

    }

}