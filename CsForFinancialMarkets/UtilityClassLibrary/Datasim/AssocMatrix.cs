// AssocMatrix.cs
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
//	2006-3-25 DD copy from ass array
//	2009-1-8 DD update for new applications
//  2009-3-18 DD C# version
//
// (C) Datasim Component Technology 1999-2010
//

using System;
// Spreadsheet-like stuff
public struct SpreadSheetVertex<AI1, AI2>
{
		public AI1 first;
		public AI2 second;
}

public struct SpreadSheetRange<AI1, AI2>
{

    public SpreadSheetVertex<AI1, AI2> upperLeft;
    public SpreadSheetVertex<AI1, AI2> lowerRight;

}

public delegate void FunctionEval<V>(ref V cellValue);

public class AssocMatrix<AI1, AI2,V>
{
  	// The essential data, public for convenience
	public NumericMatrix<V> mat;    // The real data

	public AssocArray<AI1, int> r;	// Rows
	public AssocArray<AI2, int> c;	// Columns


	// Redundant information for performance
	public Set<AI1> keyRows;
	public Set<AI2> keyColumns;


public AssocMatrix()
{

	mat = new NumericMatrix<V>(10, 10);
	r = new AssocArray<AI1, int> ();
	c = new AssocArray<AI2, int> ();
	keyRows = new Set<AI1>();
	keyColumns = new Set<AI2>();
}

public AssocMatrix(AssocMatrix<AI1, AI2,V> mat2)
{

	mat = new NumericMatrix<V>(mat2.mat);
	r = new AssocArray<AI1, int>(mat2.r); 
    c = new AssocArray<AI2, int>(mat2.c);
	
    keyRows = new Set<AI1>(mat2.keyRows);
	keyColumns = new Set<AI2>(mat2.keyColumns);

}


    // V2
    /*
// Construct the map from a list of names and a REPEATED val
AssocMatrix(V  rowStart, V columnStart,  NumericMatrix<V> matrix) 
{

	// Must build the associative arrays, they have the same values as the
	// indices in the matrix

	VectorCollectionGenerator<V> rowGenerator = new VectorCollectionGenerator<V>();
	rowGenerator.Start = rowStart;
	rowGenerator.Increment = new V();
	rowGenerator.Size = matrix.Rows;

	VectorCollectionGenerator<V> columnGenerator = new VectorCollectionGenerator<V>();
	columnGenerator.Start = columnStart;
	columnGenerator.Increment = 1;
	columnGenerator.Size = matrix.Columns;

	mat = new NumericMatrix<V>(matrix);

//	keyRows = new SetCreator<V, int>.createSet(rowGenerator);
	//keyColumns = new SetCreator<V, int>.createSet(columnGenerator);

	
    // Build rows
	int start = mat.MinRowIndex;
	r = new AssocArray<AI1, int> ();

    foreach (AI1 row in keyRows)
    {
          r[row] = start;
          start = start + 1;
    }

    // Build columns
    start = mat.MinColumnIndex;
    c = new AssocArray<AI2, int>();

    foreach (AI2 col in keyColumns)
    {
        c[col] = start;
        start = start + 1;
    }

	// NO EXCEPTION HANDLING AT THE MOMENT
	//print(c);
}
    
    */
public AssocMatrix(Set<AI1> Rnames, Set<AI2> Cnames,
								NumericMatrix<V> matrix) 
{

    keyRows = new Set<AI1>(Rnames);
    keyColumns = new Set<AI2>(Cnames);

	// Must build the associative arrays, they have the same values as the
	// indices in the matrix

	//mat = new NumericMatrix<V>(matrix);
    mat = matrix;

	// Build rows
	int start = mat.MinRowIndex;
	r = new AssocArray<AI1, int> ();

    foreach (AI1 row in keyRows)
    {
          r[row] = start;
          start++;
    }
    
	// Build columns
	start = mat.MinColumnIndex;
	c = new AssocArray<AI2, int> ();

    foreach (AI2 col in keyColumns)
    {
          c[col] = start;
          start++;
    }
	
	// NO EXCEPTION HANDLING AT THE MOMENT
	//print(c);
}


// Change values in some range
public void modify(SpreadSheetRange<AI1, AI2> range, FunctionEval<V> function)
{ // Using a delegate to change the values in a range!
    
	// Objective is to iterate in a range and apply a function to each 
	// element in that range

		AI1 rmin = range.upperLeft.first;
		AI2 cmin = range.upperLeft.second;

		AI1 rmax = range.lowerRight.first;
		AI2 cmax = range.lowerRight.second;

		int Rmin = r[rmin]; int Rmax = r[rmax];
		int Cmin = c[cmin]; int Cmax = c[cmax];

        
		// Now must find the integer indices corresponding to these
        V d; 

		for (int ri = Rmin; ri <= Rmax; ri++)
		{
			for (int ci = Cmin; ci <= Cmax; ci++)
			{
                d = mat[ri,ci];
				function(ref d);
                mat[ri, ci] = d;
			}
		}
    
}

public NumericMatrix<V> extract(SpreadSheetRange<AI1, AI2> range)
{

	// Slice a matrix
	AI1 rmin = range.upperLeft.first;
		AI2 cmin = range.upperLeft.second;

		AI1 rmax = range.lowerRight.first;
		AI2 cmax = range.lowerRight.second;

		int Rmin = r[rmin]; int Rmax = r[rmax];
		int Cmin = c[cmin]; int Cmax = c[cmax];

		// Now must find the integer indices corresponding to these
        NumericMatrix<V> result = new NumericMatrix<V>(Rmax - Rmin + 1, Cmax - Cmin + 1, Rmin, Cmin);

		for (int ri = Rmin; ri <= Rmax; ri++)
		{
			for (int ci = Cmin; ci <= Cmax; ci++)
			{
					result[ri, ci] = mat[ri, ci];
			}
		}

		return result;

}

    

    
 // Making it possible to use operator overload [i,j]; N.B. (i,j) not possible
 public V this[ int row, int column ]
 {// Get the element at position    
      
        get
        {
           return mat[row, column];
        }
        // Set the element at position
        set
        {
            mat[row, column] = value;
        }
 }


// Return copies of keys
public Set<AI1> RowKeys
{
    get
    {
        return keyRows;
    }
} 

public Set<AI2> ColumnKeys
{
    get
    {
	    return keyColumns;
    }

}

public NumericMatrix<V> Data
{
    get
    {
    	return mat;
    }
}

public void print()
{
   
    for (int i = mat.MinRowIndex; i <= mat.MaxRowIndex; i++)
    {
        Console.WriteLine();
        for (int j = mat.MinColumnIndex; j <= mat.MaxColumnIndex; j++)
        {
            Console.Write(mat[i,j]); Console.Write(", ");
        }
    }

    Console.WriteLine();
}

}
