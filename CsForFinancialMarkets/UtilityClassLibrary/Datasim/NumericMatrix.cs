// NumericMatrix.cs
// 
// Matrix class for use in mathematical manipulations.
//
// 2008-1-5 DD modifications and review
//
// (C) Datasim Education BV 2009

using System;
using System.Collections.Generic;


public class NumericMatrix<T> : Matrix<T>
{
    private static BinaryOperatorT<T, T, T> addTT;
    private static BinaryOperatorT<T, T, T> subTT;
    private static BinaryOperatorT<T, T, T> mulTT;


    //constructors
    public NumericMatrix() : base()
    {
    }

    public NumericMatrix( int rows, int columns ) : base( rows, columns )
    {
    }

    public NumericMatrix( int rows, int columns, int rowstart, int columnstart ) : base( rows, columns, rowstart, columnstart )
    {
    }

    public NumericMatrix( NumericMatrix<T> source ) : base( source )
    {
    }

	// AG added
    public NumericMatrix(Array<T> array, int rowstart, int columnstart)
        : base(array, rowstart, columnstart)
    {

    }
     // Operators

    // Addition
    public static NumericMatrix<T> operator +( NumericMatrix<T> m1, NumericMatrix<T> m2 )
    {
        NumericMatrix<T> temp_matrix = new NumericMatrix<T>( m1.Rows, m1.Columns, m1.MinRowIndex, m1.MinColumnIndex );

        int i, j;

        if( addTT == null )
        {
            addTT = GenericOperatorFactory<T, T, T, Vector<T>>.Add;
          //  addTT = new BinaryOperatorT<T, T, T>(GenericOperatorFactory<T, T, T, Vector<T>>.Add);
        }
        
        for( i = m1.MinColumnIndex; i <= m1.MaxColumnIndex; i++ )
            for( j = m1.MinRowIndex; j <= m1.MaxRowIndex; j++ )
                temp_matrix[ j, i ] = addTT( m1[ j, i ], m2[ j, i ] );

        return temp_matrix;
    }

    // Subtraction
    public static NumericMatrix<T> operator -( NumericMatrix<T> m1, NumericMatrix<T> m2 )
    {
        NumericMatrix<T> temp_matrix = new NumericMatrix<T>( m1.Rows, m1.Columns, m1.MinRowIndex, m1.MinColumnIndex );

        int i, j;

        if( subTT == null )
        {
            subTT = GenericOperatorFactory<T, T, T, Vector<T>>.Subtract;
        }

        for( i = m1.MinColumnIndex; i <= m1.MaxColumnIndex; i++ )
        {
            for( j = m1.MinRowIndex; j <= m1.MaxRowIndex; j++ )
            {
               //temp_matrix[ j, i ] = subTT( m1[ j, i ], m2[ j, i ] );
                temp_matrix[j, i] = subTT(m1[j, i], m2[j, i]);

            }
        }
        return temp_matrix;
    }

    // Matrix multiplication
    public static NumericMatrix<T> operator *( NumericMatrix<T> m1, NumericMatrix<T> m2 )
    {
        NumericMatrix<T> result = new NumericMatrix<T>( m1.Rows, m2.Columns, m1.MinRowIndex, m1.MinColumnIndex );

        if( mulTT == null )
        {
            mulTT = GenericOperatorFactory<T, T, T, Vector<T>>.Multiply;
        }

        if( addTT == null )
        {
            addTT = GenericOperatorFactory<T, T, T, Vector<T>>.Add;
        }

      /*  OLD int i, j, k, l;

        for( i = result.MaxRowIndex; i >= result.MinRowIndex; i-- )
        {
            for( j = result.MinColumnIndex; j <= result.MaxColumnIndex; j++ )
            {
                for( k = m1.MinColumnIndex, l = m2.MaxRowIndex; k <= m1.MaxColumnIndex; k++, l-- )
                {
                    result[ i, j ] = addTT( result[ i, j ], mulTT( m1[ i, k ], m2[ l, j ] ) );
                }
            }
        }*/

        // Index variables must be declared at this scope
        int i, j, k;

        for (i = result.MinRowIndex; i <= result.MaxRowIndex; i++)
        {
            for (j = result.MinColumnIndex; j <= result.MaxColumnIndex; j++)
            {
                result[i, j] = mulTT(m1[i, m1.MinColumnIndex], m2[m2.MinRowIndex, j]);
                for (k = m1.MinColumnIndex+1; k <= m1.MaxColumnIndex; k++)
                {
                    result[i, j] = addTT(result[i, j], mulTT(m1[i, k], m2[k, j]));
                }
            }
        }

        return result;
    }

    // Multiplication with a vector
    public static Vector<T> operator *( NumericMatrix<T> m, Vector<T> v )
    {
        if( mulTT == null )
        {
            mulTT = GenericOperatorFactory<T, T, T, Vector<T>>.Multiply;
        }

        if( addTT == null )
        {
            addTT = GenericOperatorFactory<T, T, T, Vector<T>>.Add;
        }

        Vector<T> result = new Vector<T>( m.Rows );

        int i, j;

        for( i = m.MinRowIndex; i <= m.MaxRowIndex; i++ )
        {
            result[i] = mulTT(m[i, m.MinColumnIndex], v[v.MinIndex]);
            for( j = m.MinColumnIndex + 1; j <= m.MaxColumnIndex; j++ )
            {
                result[ i ] = addTT( result[ i ], mulTT( m[ i, j ], v[ j ] ) );
            }
        }

        return result;
    }

    // Scalar multiplication
    public static NumericMatrix<T> operator *( T getal, NumericMatrix<T> m )
    {

        NumericMatrix<T> result = new NumericMatrix<T>( m.Rows, m.Columns, m.MinRowIndex, m.MinColumnIndex );

        int i, j;

        if( mulTT == null )
        {
            mulTT = GenericOperatorFactory<T, T, T, Vector<T>>.Multiply;
        }

        for( i = m.MinRowIndex; i <= m.MaxRowIndex; i++ )
        {
            for( j = m.MinColumnIndex; j <= m.MaxColumnIndex; j++ )
            {
                result[ i, j ] = mulTT( getal, m[ i, j ] );
            }
        }

        return result;
    }

    
    // Unary minus
 /*   public static NumericMatrix<T> operator - (){
        NumericMatrix<T> result = new NumericMatrix<T>(this.Rows, this.Columns, this.MinRowIndex, this.MinColumnIndex);

        int i,j;

        for(i = result.MinRowIndex(); i <= result.MaxRowIndex(); i++){
            for(j = result.MinColumnIndex(); j <= result.MaxColumnIndex(); j++){
              result[i,j] = -(this[i,j]);
            }
        }
        return result;
    }
    */
}