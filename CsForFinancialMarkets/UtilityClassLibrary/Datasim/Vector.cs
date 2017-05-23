// Vector.cs
//
// 25 Oktober 2005 Jannes Albrink
//
// Vector class. It uses the m_vecayList to store the data.
// The user can create Vectors with all kinds of start indexes.
// And it's possible to perform calculations on Vectors
//
//
// 08 November 2005 ---Update---
// Renamed variables 'Head', 'Arr' and 'Tail' to 'MinIndex', 'm_vec' and 'm_endIndex'.
// Removed try-catch block in index operator method.
// Renamed all variables in method parameters that started with an uppercase character,
// they were renamed to the same name but then starting with a lowercase character.
//
// 11 November 2005 ---Update---
// Now using Array as baseclass.
// Added all operator overloading methods
//
// 2008-12-4 DD modifications
// 2009-6-13 DD review + inner product
// 2009-10-16 DD vec*vec
//
//
// (C) Datasim Component Technology 2005-2009

using System;

public class Vector<T> : Array<T>
{
    private static BinaryOperatorT<T, T, T> addTT;
    private static BinaryOperatorT<T, T, T> subTT;
    private static BinaryOperatorT<T, T, T> mulTT;

    //Constructor with start-index is 1 and length is 15
    public Vector() : base()
    {
    }

    //Constructor with length parameter
    public Vector( int length ) : base( length )
    {
    }

    //Constructor with length and start-index parameter
    public Vector( int length, int minIndex ) : base( length, minIndex )
    {
    }

    //Constructor with length, start-index and initial value parameters
    public Vector( int length, int minIndex, T initvalue ) : base( length, minIndex, initvalue )
    {
    }

    //Constructor with copy function for a normal array and start-index 1
    public Vector( T[] source ) : base( source )
    {
    }

    //Constructor with copy function for a normal array and start-index parameters
    public Vector( T[] source, int minIndex ) : base( source, minIndex )
    {
    }

    //Constructor with copy function. Also copies the start-index from the source
    public Vector( Array<T> source ) : base( source )
    {
    }

    //Constructor with copy function. Also copies the start-index from the source
    public Vector( Vector<T> source ) : base( source )
    {
    }


    //Operator overloading for '+' operator. Adding v2 to v1
    public static Vector<T> operator + ( Vector<T> v1, Vector<T> v2 )
    {
        Vector<T> result = new Vector<T>( v1.Length, v1.MinIndex );
        int delta = v1.MinIndex - v2.MinIndex;

        if( addTT == null )
        {
            addTT = new BinaryOperatorT<T,T,T>( GenericOperatorFactory<T, T, T, Vector<T>>.Add );
        }

        for( int i = v1.MinIndex; i <= v1.MaxIndex; i++ )
        {
            result[ i ] = addTT( v1[ i ], v2[ ( i - delta ) ] );
        }
        return result;
    }

    //Operator overloading for '*' operator. Multiplying v1 and v2
    public static Vector<T> operator *(Vector<T> v1, Vector<T> v2)
    {
        Vector<T> result = new Vector<T>(v1.Length, v1.MinIndex);
        int delta = v1.MinIndex - v2.MinIndex;

        if (mulTT == null)
        {
            mulTT = new BinaryOperatorT<T, T, T>(GenericOperatorFactory<T, T, T, Vector<T>>.Multiply);
        }

        for (int i = v1.MinIndex; i <= v1.MaxIndex; i++)
        {
            result[i] = mulTT(v1[i], v2[i-delta]);
        }
        return result;   
       
    }

    //Operator overloading for '-' operator. Substracting v2 from v1
    public static Vector<T> operator -( Vector<T> v1, Vector<T> v2 )
    {
        Vector<T> result = new Vector<T>( v1.Length, v1.MinIndex );
        int delta = v1.MinIndex - v2.MinIndex;

        if( subTT == null )
        {
            subTT = new BinaryOperatorT<T,T,T>( GenericOperatorFactory<T, T, T, Vector<T>>.Subtract );
        }

        for( int i = v1.MinIndex; i <= v1.MaxIndex; i++ )
        {
            result[ i ] = subTT( v1[ i ], v2[ ( i - delta ) ] );
        }
        return result;
    }

    //operator overloading for '+' operator. Adding t1 to every index of v1
    public static Vector<T> operator + ( Vector<T> v1, T t1 )
    {
        Vector<T> result = new Vector<T>( v1.Length, v1.MinIndex );

        if( addTT == null )
        {
            addTT = new BinaryOperatorT<T,T,T>( GenericOperatorFactory<T, T, T, Vector<T>>.Add );
        }

        for( int i = v1.MinIndex; i <= v1.MaxIndex; i++ )
        {
            result[ i ] = addTT( v1[ i ], t1 );
        }
        return result;
    }

    //operator overloading for '+' operator. Adding t1 to every index of v1
    public static Vector<T> operator +( T t1, Vector<T> v1 )
    {
        return v1 + t1;
    }

    //operator overloading for '-' operator. Substracting t1 from every index of v1
    public static Vector<T> operator -( Vector<T> v1, T t1 )
    {
        Vector<T> result = new Vector<T>( v1.Length, v1.MinIndex );

        if( subTT == null )
        {
            subTT = new BinaryOperatorT<T,T,T>( GenericOperatorFactory<T, T, T, Vector<T>>.Subtract );
        }

        for( int i = v1.MinIndex; i <= v1.MaxIndex; i++ )
        {
            result[ i ] = subTT( v1[ i ], t1 );
        }
        return result;
    }

    //operator overloading for '-' operator. Substracting v1 from t1
    public static Vector<T> operator -( T t1, Vector<T> v1 )
    {
        Vector<T> result = new Vector<T>( v1.Length, v1.MinIndex );

        if( subTT == null )
        {
            subTT = new BinaryOperatorT<T,T,T>( GenericOperatorFactory<T, T, T, Vector<T>>.Subtract );
        }

        for( int i = v1.MinIndex; i <= v1.MaxIndex; i++ )
        {
            result[ i ] = subTT( t1, v1[ i ] );
        }
        return result;
    }

    //operator overloading for '*' operator. multiplying t1 with every index of v1
    public static Vector<T> operator *( Vector<T> v1, T t1 )
    {
        Vector<T> result = new Vector<T>( v1.Length, v1.MinIndex );

        if( mulTT == null )
        {
            mulTT = new BinaryOperatorT<T,T,T>( GenericOperatorFactory<T, T, T, Vector<T>>.Multiply );
        }

        for( int i = v1.MinIndex; i <= v1.MaxIndex; i++ )
        {
            result[ i ] = mulTT( v1[ i ], t1 );
        }
        return result;
    }

    //operator overloading for '*' operator. multiplying t1 with every index of v1
    public static Vector<T> operator *( T t1, Vector<T> v1 )
    {
        return v1 * t1;
    }

    public T InnerProduct(Vector<T> v2)
    {
        T result = mulTT(this[MinIndex], v2[v2.MinIndex]);

        for (int i = v2.MinIndex+1; i <= v2.MaxIndex; i++)
        {
            result = addTT(result, mulTT(this[i], v2[i]));
        }

        return result;
    }
}