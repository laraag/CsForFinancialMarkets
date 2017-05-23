// TestArray.cs
//
// Basic properties of C# arrays.
//
// (C) Datasim Education BV 2009
//
//

using System;

public class TestPoint
{
	public static void Main()
	{

        // Array of built-in types
        int size = 3;
        int[] arr = new int[size];      // Store data in a contiguous block
        
        // Initialise 
        for (int j = 0; j < size; ++j)
        {
            arr[j] = j + 1;
        }

        // Hard-coded array initialisation
        char[] charArr = { 'a', 'b', 'c', 'd', 'e' };
        Console.WriteLine(charArr[1]);       // 'b'

        // Arrays of user-defined types (References)
		Point [] pointArr = new Point[size];	// Array of null references

        // Initialise 
        for (int j = 0; j < size; ++j)
        { // Create the points in the array

            pointArr[j] = new Point(j, j);
        }

        // Arrays of user-defined types (Value type)
        PointStruct[] ptArr = new PointStruct[size];	

        // All values initialised to zero
        for (int j = 0; j < size; ++j)
        { 

            Console.WriteLine("{0},{1}",ptArr[j].x, ptArr[j].y);	// (0.0, 0.0)
        }

        // Short-circuit the one-dimensional arrays
        arr = null;
        pointArr = null;
        ptArr = null;

        //// Declare a reference to empty array
        //double[] newArr = null;

        // Rectangular arrays
        int N = 100;
        int M = 100;
        Point[,] mesh = new Point[N, M];

        // Initialise in a column-major fashion
        for (int i = 0; i < N; ++i)
        {
            for (int j = 0; j < M; ++j)
            {
                mesh[i,j] = new Point(i+j, i+j);
            }
        }

        // Jagged arrays, in this case lower triangular
        int NROWS = 100;
        double[][] LowerTriangularMatrix = new double[NROWS][]; // Values initialise

        // Initialise in a column-major fashion
        for (int i = 0; i < LowerTriangularMatrix.Length; ++i)
        {
            LowerTriangularMatrix[i] = new double[i + 1];
            for (int j = 0; j < LowerTriangularMatrix[i].Length; ++j)
            {
                LowerTriangularMatrix[i][j] = (double)(i + j);
            }
        }


	    // Create a square tridiagonal matrix
        NROWS = 100;
        double[][] TridiagonalMatrix = new double[NROWS][]; // Values initialise

        // Initialise in a column-major fashion

        // Initialise top left corner
        TridiagonalMatrix[0] = new double[2];
        TridiagonalMatrix[0][0] = 4.0;
        TridiagonalMatrix[0][1] = 1.0;
        
        
        // Initialise main body of matrix
        for (int i = 1; i < NROWS - 1; ++i)
        {
            TridiagonalMatrix[i] = new double[3];

            TridiagonalMatrix[i][0] = 1.0;
            TridiagonalMatrix[i][1] = 4.0;
            TridiagonalMatrix[i][2] = 1.0;
        }

        // Initialise bottom right corner
        TridiagonalMatrix[NROWS-1] = new double[2];
        TridiagonalMatrix[NROWS - 1][1] = 4.0;
        TridiagonalMatrix[NROWS - 1][0] = 1.0;


        // Three-dimensional structure
        int NX = 100;
        int NY = 200;
        int NZ = 50;
        Point3d[, ,] surface = new Point3d[NX, NY, NZ];

        // Initialise the tensor or 3d points
        for (int i = 0; i < NX; ++i)
        {
            for (int j = 0; j < NY; ++j)
            {
                for (int k = 0; k < NZ; ++k)
                {
                    surface[i, j, k].X = 1.0;
                    surface[i, j, k].Y = 1.0;
                    surface[i, j, k].Z = 1.0;
                }
            }
        }

        // Out of bounds exceptions
        try
        {
            int[] myArr = new int[3];
            myArr[10] = 34;
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine(e.Message); 
        }

	}
}
