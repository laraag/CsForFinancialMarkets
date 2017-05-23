// GenericMatrixImpl.cs
//
// Implementations of generic matrix. We can choose
// between different implementations 1) 2d array 2) 1d 
// array (performance reasons). Later we can choose others,
// for example jagged arrays.
//
// 2008-12-15 DD kick off
//
// (C) Datasim Education BV 2009
//

using System;

public abstract class GenericMatrixImpl<T> : IMatrixAccess<T>
{
	// Initialise the matrix
    public abstract void init(int Rows, int Columns);

    // Access functions 
    public abstract T Get(int row, int columns);
    public abstract void Set(T value, int row, int columns);
}

public class MatrixTwoArrayImpl<T> : GenericMatrixImpl<T>
{
    // Which data structure
    protected T[,] data; // Standard C# syntax

    public MatrixTwoArrayImpl()
    {
        init(10,10);
    }

    public MatrixTwoArrayImpl(int Rows, int Columns)
    {
        init(Rows, Columns);
    }

    // Initialise the matrix
    public override void init(int Rows, int Columns)
    {
        data = new T[Rows, Columns];
    }


    // Access functions 
    public override T Get(int row, int column)
    {
        return data[row, column];
    }

    public override void Set(T value, int row, int column)
    {
        data[row, column] = value;
    }

}

public class MatrixOneArrayImpl<T> : GenericMatrixImpl<T>
{
    // Which data structure
    protected T[] data; // Standard C# syntax

    // Save number of rows and columns
    int nr;
    int nc;
    int nrm1;

    public MatrixOneArrayImpl()
    {
        init(10,10);
    }
    public MatrixOneArrayImpl(int Rows, int Columns)
    {
        init(Rows, Columns);
    }

    // Initialise the matrix
    public override void init(int Rows, int Columns)
    {
        nr = Rows;
        nc = Columns;
        nrm1 = nr - 1;
        data = new T[nr*nc];
    }


    // Access functions 
    public override T Get(int row, int column)
    {
        return data[nrm1*row + column];
    }

    public override void Set(T value, int row, int column)
    {
        data[nrm1*row + column] = value;
    }

}

// Lower triangular matrix
public class LowerTriangularImpl<T> : GenericMatrixImpl<T>
{
    // Which data structure, using jagged matrices
    protected T[][] data; // Standard C# syntax

    public LowerTriangularImpl()
    {
        init(10, 10);
    }

    public LowerTriangularImpl(int Rows, int Columns)
    {
        init(Rows, Columns);
    }

    // Initialise the matrix
    public override void init(int Rows, int Columns)
    {
        data = new T[Rows][];
        for (int j = 0; j < Columns; j++)
        {
            data[j] = new T[j + 1];
        }   
    }


    // Access functions 
    public override T Get(int row, int column)
    {
        // row <= column
        return data[row][column];
    }

    public override void Set(T value, int row, int column)
    {
        data[row][column] = value;
    }

}

// Upper triangular matrix
public class UpperTriangularImpl<T> : GenericMatrixImpl<T>
{
    // Which data structure
    protected T[][] data; // Standard C# syntax

    public UpperTriangularImpl()
    {
        init(10, 10);
    }

    public UpperTriangularImpl(int Rows, int Columns)
    {
        init(Rows, Columns);
    }

    // Initialise the matrix
    public override void init(int Rows, int Columns)
    {
        data = new T[Rows][];
        for (int j = 0; j < Columns; j++)
        {
            data[j] = new T[Rows - j];
        }
    }


    // Access functions 
    public override T Get(int row, int column)
    {
        // row >= column
        return data[row][column];
    }

    public override void Set(T value, int row, int column)
    {
        data[row][column] = value;
    }

}
