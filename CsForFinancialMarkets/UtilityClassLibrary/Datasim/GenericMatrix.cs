// Matrix.cs
//
// Oktober 2005 Peter Faas
//
// Edited on 15-12-2005 by Jannes Albrink.
// Changed copy constructor:
// data[ k ].Add( source[ i, j ] );
// to:
// data[ k ].Add( source[ j, i ] );
//
//
// Edited on 20-12-2005 by Jannes Albrink.
// Changed constructor with row, cols, rowstart, colstart:
// for( int i = 0; i <= nc; i++ ) and
// for( int j = 0; j <= nr; j++ )
// to:
// for( int i = 0; i < nc; i++ )
// for( int j = 0; j < nr; j++ )
//
// Changed default construtor, no list was created.
// Changed function names of Row and Column, since they did exactly the opposite.
// Removed some try-catch blocks
// 2008-12-4 DD code change nested Array
// 2008-12-5 DD forget nested array; use T[,]
// 2008-12-7 DD functions for slices of matrix
// 2008-12-15 DD generic matrix
//
// (C) Datasim Component Technology 2005-2009

using System;
using System.Collections.Generic;


public class GenericMatrix<T,S> where S: IMatrixAccess<T>, new()
{ // Underlying type T and structure S

    private int m_rowstart;
    private int m_columnstart;
    private int nr;
    private int nc;

   // V1 protected T [,] data; // Standard C# syntax
    S data;

   
    private void initState()
    {
        data = new S();
        data.init(nr, nc);
    }

    public GenericMatrix()
    {
        // Default constructor
     
        m_rowstart = 1;
        m_columnstart = 1;
        nr = 1;
        nc = 1;

        initState();
    }

    public GenericMatrix( int rows, int columns )
    {
        //Constructor with size. Start index = 1
      
        m_rowstart = 1;
        m_columnstart = 1;

        nr = rows;
        nc = columns;

        initState();
       
    }

    public GenericMatrix( int rows, int columns, int rowstart, int columnstart )
    {
        

        //Constructor with size & start index         
        m_rowstart = rowstart;
        m_columnstart = columnstart;

        nr = rows;
        nc = columns;

        initState();
       
    }

    public void initCells(T value)
    {
        for (int i = MinRowIndex; i <= MaxRowIndex; i++)
        {
            for (int j = MinColumnIndex; j <= MaxColumnIndex; j++)
            {
                data.Set(value, i - m_rowstart, j - m_columnstart);
            }
        }
    }

    public GenericMatrix( GenericMatrix<T,S> source )
    {
        //Copy constructor         
        m_rowstart = source.m_rowstart;
        m_columnstart = source.m_columnstart;

        nr = source.nr;
        nc = source.nc;

        initState();
    
    }

    public int MinRowIndex
    { //return minimum row index
        get
        {
            return m_rowstart;
        }
    }

    public int MaxRowIndex
    { // return maximum row index
        get
        {
            return m_rowstart + Rows - 1;
        }
    }

    public int MinColumnIndex
    { // return minimum column index
        get
        {
            return m_columnstart;
        }
    }

    public int MaxColumnIndex
    { // return maximum column index
        get
        {
            return m_columnstart + Columns - 1;
        }
    }

    public int Rows
    { // return number of rows
        get
        {
            return nr;
        }
    }

    public int Columns
    { // return number of columns
        get
        {
            return nc;
        }
    }

    // Extacting rows and columns of the matrix

    public Vector<T> getRow(int rownum)
    {

        Vector<T> result = new Vector<T>(Columns, MinColumnIndex);

        for (int i = result.MinIndex; i <= result.MaxIndex; i++)
        {
            result[i] = this[rownum, i];
        }

        return result;
    }

    public Vector<T> getColumn(int colnum)
    {

        Vector<T> result = new Vector<T>(Rows, MinRowIndex);

        for (int i = result.MinIndex; i <= result.MaxIndex; i++)
        {
            result[i] = this[i, colnum];
        }

        return result;
    }

    public void Column(int column, Array<T> array)
    { // replace column

        for (int i = array.MinIndex; i <= array.MaxIndex; i++)
        {
            this[i, column] = array[i];
        }

    }

    public void Row(int row, Array<T> array)
    { // replace row

   
        for (int i = MinColumnIndex; i <= MaxColumnIndex; i++)
        {
            this[row, i] = array[i];
        }
    }
 
     // Making it possible to use operator overload [i,j]; N.B. (i,j) not possible
    public T this[ int row, int column ]
    {// Get the element at position    
      
        get
        {
            return data.Get(row - m_rowstart, column - m_columnstart);
        }
        // Set the element at position
        set
        {
            data.Set(value, row - m_rowstart, column - m_columnstart);
        }
    }

    public void extendedPrint()
    {
      for (int i = MinRowIndex; i <= MaxRowIndex; i++)
        {
            Console.WriteLine();
            for (int j = MinColumnIndex; j <= MaxColumnIndex; j++)
            {
                Console.Write(data.Get(i - m_rowstart, j - m_columnstart)); Console.Write(", ");
            }
        }

        Console.WriteLine();
    }

}