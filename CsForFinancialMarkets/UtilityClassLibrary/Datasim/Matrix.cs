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
// 2008-12-15 DD create a matrix from an array NX1
// 2009-6-12 DD support for data and reference types
// 2010-4-2 DD 2d indexers [,] for interoperability
//
// (C) Datasim Component Technology 2005-2009

// Ag added  Matrix(Array<T> array, int rowstart, int columnstart) constructor

using System;
using System.Collections.Generic;


public class Matrix<T>
{
    // Start indices
    private int m_rowstart;
    private int m_columnstart;

    // Rows and columns
    private int nr;
    private int nc;

    protected T [,] data; // Standard C# syntax

    private void initState()
    {

        data = new T[nr, nc];
    }

    public Matrix()
    {
        // Default constructor
     
        m_rowstart = 1;
        m_columnstart = 1;
        nr = 1;
        nc = 1;

        initState();
    }

    public Matrix( int rows, int columns )
    {
        //Constructor with size. Start index = 1
      
        m_rowstart = 1;
        m_columnstart = 1;

        nr = rows;
        nc = columns;

        initState();
       
    }

    public Matrix( int rows, int columns, int rowstart, int columnstart )
    {
        

        //Constructor with size & start index         
        m_rowstart = rowstart;
        m_columnstart = columnstart;

        nr = rows;
        nc = columns;

        initState();
       
    }

	// AG added
    public Matrix(Array<T> array, int rowstart, int columnstart)
    {

        // Matrix with 1 row and array.Minindex columns
        // Constructor with size & start index         
        m_rowstart = rowstart;
        m_columnstart = columnstart;

        nr = 1;
        nc = array.MinIndex;

        initState();

        for (int j = array.MinIndex; j <= array.MaxIndex; j++)
        {

            data[m_rowstart, j] = array[j];
        }

        initState();

    }
    
    public void initCells(T value)
    {
        for (int i = MinRowIndex; i <= MaxRowIndex; i++)
        {
            for (int j = MinColumnIndex; j <= MaxColumnIndex; j++)
            {
                if (value is ICloneable)
                    data[i - m_rowstart, j - m_columnstart] = (T)(value as ICloneable).Clone();
                else
                    data[i - m_rowstart, j - m_columnstart] = value;  
            }
        }
    }

    public Matrix( Matrix<T> source )
    {
        //Copy constructor         
        m_rowstart = source.m_rowstart;
        m_columnstart = source.m_columnstart;

        nr = source.nr;
        nc = source.nc;

        for (int i = MinRowIndex; i <= MaxRowIndex; i++)
        {
            for (int j = MinColumnIndex; j <= MaxColumnIndex; j++)
            {
                if (data[i - m_rowstart, j - m_columnstart] is ICloneable)
                    data[i - m_rowstart, j - m_columnstart] = (T)(source.data[i - m_rowstart, j - m_columnstart] as ICloneable).Clone();
                else 
                    data[i - m_rowstart, j - m_columnstart] = source.data[i - m_rowstart, j - m_columnstart];
            }
        }

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

    public Array<T> getRow(int rownum)
    { // Slice matrix to get a row

        Array<T> result = new Array<T>(Columns, MinColumnIndex);

        for (int i = result.MinIndex; i <= result.MaxIndex; i++)
        {
            result[i] = this[rownum, i];
        }

        return result;
    }

    public void setRow(Array<T> rowArr, int rowNum)
    {
        // Precondition: size of array == number of columns of matrix
  
        for (int i = rowArr.MinIndex; i <= rowArr.MaxIndex; i++)
        {
            this[rowNum, i] = rowArr[i];
        }
    }

    public Array<T> getColumn(int colnum)
    { // Slice matrix to get a column

        Array<T> result = new Array<T>(Rows, MinRowIndex);

        for (int i = result.MinIndex; i <= result.MaxIndex; i++)
        {
            result[i] = this[i, colnum];
        }

        return result;
    }

    public void setColumn(Array<T> colArr, int colNum)
    {
        // Precondition: size of array == number of columns of matrix

        for (int i = colArr.MinIndex; i <= colArr.MaxIndex; i++)
        {
            this[i, colNum] = colArr[i];
        }
    }

    // Keep Column and Row for backwards compatibility
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
           return data[row - m_rowstart, column - m_columnstart];
        }
        // Set the element at position
        set
        {
            data[row - m_rowstart, column - m_columnstart] = value;
        }
    }

    public void print()
    {
    
        for (int i = MinRowIndex; i <= MaxRowIndex; i++)
        {
            Console.WriteLine();
            for (int j = MinColumnIndex; j <= MaxColumnIndex; j++)
            {
                Console.Write(data[i - m_rowstart, j - m_columnstart]); Console.Write(", ");
            }
        }

        Console.WriteLine();
    }

    public void extendedPrint()
    {
        /*   Console.Write("Matrix rows and columns: "); Console.Write(Rows); Console.Write(", "); Console.Write(Columns); Console.WriteLine();
           Console.Write("Min Row index: "); Console.Write(MinRowIndex); Console.Write(", Max Row index: "); Console.Write(MaxRowIndex); Console.WriteLine();
           Console.Write("Min Column index: "); Console.Write(MinColumnIndex); Console.Write(", Max Column index: "); Console.Write(MaxColumnIndex); Console.WriteLine();
           */
        for (int i = MinRowIndex; i <= MaxRowIndex; i++)
        {
            Console.WriteLine();
            for (int j = MinColumnIndex; j <= MaxColumnIndex; j++)
            {
                Console.Write(data[i - m_rowstart, j - m_columnstart]); Console.Write(", ");
            }
        }

        Console.WriteLine();
    }



}