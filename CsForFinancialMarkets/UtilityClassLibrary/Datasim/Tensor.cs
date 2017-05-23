// Tensor.cs
// 
// Tensor class. It uses a Array and NumericMatrix of type T to store the data.
// The user can create three dimensional matrices.
//
// We use Composition in this case. Another ploy would be to use inheritance.
//
// 15 December 2005 Jannes Albrink
// 2008-12-5 DD mods
// 2009-6-12 DD print()
//
// (C) Datasim Component Technology 2005-2009

using System;

public class Tensor<T>
{
    private int m_rows;
    private int m_columns;
    private int m_depth;
    private Array<NumericMatrix<T>> m_tensor;


    //Default constructor everything starts at 1 and has length 1
    public Tensor()
    {
        m_tensor = new Array<NumericMatrix<T>>( 1, 1 );
        m_tensor[ 1 ] = new NumericMatrix<T>( 1, 1, 1, 1 );

        m_rows = 1;
        m_columns = 1;
        m_depth = 1;
    }

    //Constructor with the ammount of rows, columns and depth, all startindexes are 1.
    public Tensor( int rows, int columns, int nthird )
    {
        m_tensor = new Array<NumericMatrix<T>>( nthird, 1 );

        for( int i = m_tensor.MinIndex; i <= m_tensor.MaxIndex; i++ )
        {
            m_tensor[ i ] = new NumericMatrix<T>( rows, columns, 1, 1 );
        }

        m_rows = rows;
        m_columns = columns;
        m_depth = nthird;
    }

    //Constructor with all sizes and startindexes
    public Tensor( int rows, int columns, int nthird, int rowStart, int columnStart, int thirdStart )
    {
        m_tensor = new Array<NumericMatrix<T>>( nthird, thirdStart );	// Size nthird, start index 

        for( int i = m_tensor.MinIndex; i <= m_tensor.MaxIndex; i++ )
        {
            m_tensor[ i ] = new NumericMatrix<T>( rows, columns, rowStart, columnStart );
        }

        m_rows = rows;
        m_columns = columns;
        m_depth = nthird;
    }

    //Copy constructor, does not make a deep copy when the internal types aren't primitive
    public Tensor( Tensor<T> tensor )
    {
        m_tensor = new Array<NumericMatrix<T>>( tensor.m_depth, tensor.MinThirdIndex );

        for( int i = m_tensor.MinIndex; i <= m_tensor.MaxIndex; i++ )
        {
            m_tensor[ i ] = new NumericMatrix<T>( tensor.m_rows, tensor.m_columns, tensor.MinFirstIndex, tensor.MinSecondIndex );
        }

        for( int i = tensor.MinThirdIndex; i <= tensor.MaxThirdIndex; i++ )
        {
            for( int j = tensor.MinSecondIndex; j <= tensor.MaxSecondIndex; j++ )
            {
                for( int k = tensor.MinFirstIndex; k <= tensor.MaxFirstIndex; k++ )
                {
                    m_tensor[ i ][ k, j ] = tensor[ i ][ k, j ];
                }
            }
        }
    }

    //Returns a NumericMatrix at z = nThird
    public NumericMatrix<T> this[ int nThird ]
    {
        get
        {
            return m_tensor[ nThird ];
        }
        set
        {
            m_tensor[ nThird ] = value;
        }
    }

    //Sets or Gets value T from element x,y,z
    public T this[ int row, int column, int nThird ]
    {
        get
        {
            return m_tensor[ nThird ][ row, column ];
        }
        set
        {
            m_tensor[ nThird ][ row, column ] = value;
        }
    }

    //Returns the minindex of the rows
    public int MinFirstIndex
    {
        get
        {
            return m_tensor[ m_tensor.MinIndex ].MinRowIndex;
        }
    }

    //Returns the maxindex of the rows
    public int MaxFirstIndex
    {
        get
        {
            return m_tensor[ m_tensor.MinIndex ].MaxRowIndex;
        }
    }

    //Returns the minindex of the columns
    public int MinSecondIndex
    {
        get
        {
            return m_tensor[ m_tensor.MinIndex ].MinColumnIndex;
        }
    }

    //Returns the maxindex of the columns
    public int MaxSecondIndex
    {
        get
        {
            return m_tensor[ m_tensor.MinIndex ].MaxColumnIndex;
        }
    }

    //Returns the minindex of the depth (z)
    public int MinThirdIndex
    {
        get
        {
            return m_tensor.MinIndex;
        }
    }

    //Returns the maxindex of the depth (z)
    public int MaxThirdIndex
    {
        get
        {
            return m_tensor.MaxIndex;
        }
    }

    //Returns the ammount of rows
    public int Rows
    {
        get
        {
            return m_rows;
        }
    }

    //returns the ammount of columns
    public int Columns
    {
        get
        {
            return m_columns;
        }
    }

    //Returns the ammount of NumericMatixes
    public int SizeThird
    {
        get
        {
            return m_depth;
        }
    }

    public void print()
    { // Prnt a tensor by printing each of its matrices

        for (int i = MinFirstIndex; i <= MaxFirstIndex; i++)
        {
            m_tensor[i].print();
        }

        Console.WriteLine();
    }


}