// Lattice.cs
//
// Class respresenting a special kind of tree-like matrix that is
// needed in specific applications, for example binomial and trinomial 
// methods in options pricing.
//
// This structure can be used in other applications, such as computer graphics
// and interpolation schemes.
//
// The matrix is 'expanding' in the form of a lattice; we must define a 
// function that states how many elements to create in moving from step 'n' 
// to step 'n+1'. The lattice is recombining or reconnecting as is usual in 
// some applications.
//
// This class is a basic data structure that can be used in other classes using
// composition or inheritance.
//
// 2010-10-31 DD method to return vector at a given row position.
// 
// (C) Datasim Education BV 2001-2010
//

using System;

public class Lattice<T> where T : new()
{ // Generic lattice class; The data type T is a value type

    // Input
    //
    // N .. depth of lattice (numnber of rows less one)
    // type .. the degree of the lattice, e.g. binomial, trinomial
    //

	// Implement as a built-in .NET class
	private T [][] data;

	// Redundant data
	private int nrows;			// Number of rows
	private int typ;			// What kind of lattice (number of nodes); binomial == 2, trinomial == 3
   
    private void init(T value)
    {

	    data = new T[nrows+1][];
        int currentBranch = 1;	// There is always one single root

        for (int j = 0; j <= nrows; j++)
        {
            data[j] = new T[currentBranch];
            currentBranch += (typ - 1);

            for (int i = 0; i < 1 + j * (typ - 1); i++)
            {
                data[j][i] = value;
            }
        }   
	}

    // Constructors  destructor
    public Lattice()
    { // Default constructor, binomial lattice of fixed length


        typ = 2;
        nrows = 10;

        T myDefault = new T();
        init(myDefault);
    }

    public Lattice(int NRows, int NumberBranches)
    { // Number of rows and branch factor


        typ = NumberBranches;
        nrows = NRows;

        T myDefault= new T();
        init(myDefault);
        
    }

    public Lattice(int NRows, int NumberBranches, T val)
    { // + value at nodes

        typ = NumberBranches;
        nrows = NRows;

        init(val);
    }


	// Iterating in a Lattice; we need forward and backward versions
	// Return the minimum index of the outer 'row array'
    public int MinIndex
    {
        get
        {
            return 0;
        }
    }

    // Return the maximum index of outer 'row array'
    public int MaxIndex
    {
        get
        {
            return nrows;
        }
    }

    // Number of columnms at a given row
    public int NumberColumns(int row)
    {
        return 1 + row *(typ - 1);
    }

    // Accessing the elements of the lattice at a row and columns
    // No exception handling (this is the responsibility of client)
    public T Get(int row, int column)
    {
        return data[row][column];
    }

    public void Set(int row, int column, T newValue)
    {
        data[row][column] = newValue;
    }

    // Making it possible to use operator overload [i,j]; N.B. (i,j) not possible
    public T this[int row, int column]
    {// Get the element at position    

        get
        {
            return data[row][column];
        }
        // Set the element at position
        set
        {
            data[row][column] = value;
        }
    }

    public Vector<T> PyramidVector(int row)
    { // Generate the array at a given 'row'

       int startIndex = MinIndex;

        Vector<T> result = new Vector<T>(1 + row * (typ - 1), startIndex);

        for (int i = result.MinIndex; i < 1 + row * (typ - 1); i++)
        {
              result[i] = data[row][i];
        }

        return result;
    }

    public Vector<T> BasePyramidVector() 
    { // Generate the array at the large end of the lattice

        int startIndex = MinIndex;
        int maxRow = MaxIndex;

        Vector<T> result = new Vector<T>(1 + nrows *(typ - 1), startIndex);

        for( int i = result.MinIndex; i <= result.MaxIndex; i++ )
        {
            result[i] =  data[maxRow][i];          
        }

        return result;
    }

}

