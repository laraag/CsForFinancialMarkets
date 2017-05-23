// Test101ArraysAndMatrices.cs
//
// Showing basic methods of arrays, vectors, 
// matrices and numeric matrices.
//
// 2010-4-2 class for testing indexers
//
// (C) Datasim Education BV 2009-2013
//
using System;


class Arrays101
{
    static void Main()
    {
        
        int startIndex = -1;
        Array<double> arr = new Array<double>(10, startIndex);
        for (int j = arr.MinIndex; j <= arr.MaxIndex; j++)
        {
            arr[j] = (double)(j);
        }
        arr[arr.MinIndex] = 99.98;
        arr.print();

        // Create an array from a C# array
        // Array of built-in types
        int size = 10;
        int[] arr2 = new int[size];      // Store data in a contiguous block

        // Initialise 
        for (int j = 0; j < size; ++j)
        {
            arr2[j] = j + 1;
        }

        Array<int> arr3 = new Array<int>(arr2);
        arr.print();

        // Array of strings
        Array<string> arr4 = new Array<string>(20);

        // Arrays of dates
        Array<DateTime> arr5 = new Array<DateTime>(6);
        for (int j = arr5.MinIndex; j <= arr5.MaxIndex; j++)
        {
            arr5[j] = DateTime.Now;
            Console.WriteLine(arr5[j].ToString("U"));   // UTC (Coordinated Universal Time) ~ GMT
        }

        // Matrix
        int NR = 4; int NC = 4;
        int rowStart = 1; int colStart = 1;
        Matrix<int> mat = new Matrix<int>(NR, NC, rowStart, colStart); // Values undefined

       // mat[mat.MinRowIndex, mat.MinColumnIndex] = 99;
        mat.initCells(3);                                               // Initialise all values
       // mat[mat.MinRowIndex, mat.MinColumnIndex] = 98;
        mat.print();

        for (int i = mat.MinRowIndex; i <= mat.MaxRowIndex; i++)
        {
            for (int j = mat.MinColumnIndex; j <= mat.MaxColumnIndex; j++)
            {
                mat[i, j] = i;
            }
        }
     //   mat[mat.MinRowIndex, mat.MinColumnIndex] = 98;
        mat.print();

        // Slices of a matrix
        Console.WriteLine("slices");
        Array<int> rowSlice = mat.getRow(2); // 2nd index
        rowSlice.print();
        Array<int> colSlice = mat.getColumn(4); // 4th index
        colSlice.print();

        // Modify rows and columns
        mat.setRow(rowSlice, 4);
        mat.print();

        int size2 = 4; int start = 1; int val = 99;
        Array<int> slice2 = new Array<int>(size2, start, val);
        mat.setColumn(slice2, 1);
        mat.print();


        // Vectors and numeric matrices
        int J = 10;
        int sIndex = 1;

        // Size, start index and element values
        Vector<double> a = new Vector<double>(J, sIndex, 3.0);
        Vector<double> b = new Vector<double>(J, sIndex, 2.0);

        Vector<double> c = new Vector<double>(J, startIndex);
        c = a + b;
        c.print();

        c = c + 4.0;
        c.print();

        c = -4.0 + c;
        c.print();

        c = a - b;
        c.print();
        
        c = c * 2.0;
        c.print();

        c = 0.5 * c;
        c.print();

        double ip = c.InnerProduct(a);
        Console.WriteLine("Inner product {0} ", ip);

        // Matrices
        int R = 2; int C = 2;
        int startRow = 1; int startColumn = 1;
        NumericMatrix<double> A = new NumericMatrix<double>(R, C, startRow, startColumn);
        NumericMatrix<double> B = new NumericMatrix<double>(R, C, startRow, startColumn);
        A.initCells(1.0);
        A.print();
        for (int i = A.MinRowIndex; i <= A.MaxRowIndex; i++)
        {
            for (int j = A.MinColumnIndex; j <= A.MaxColumnIndex; j++)
            {
                A[i, j] = i * j;
                B[i, j] = - i * j;
            }
        }
        A.print(); B.print();

        // Interactions with scalars and vectors
        double factor = 2.0;
        A = factor * A;
        Console.WriteLine("Original matrix A");
        A.print();

        Vector<double> x = new Vector<double>(A.Columns, A.MinColumnIndex);
        for (int j = x.MinIndex; j <= x.MaxIndex; j++)
        {
            x[j] = j;
        }
        x.print();
        x = A * x;
        x.print();

        NumericMatrix<double> D = new NumericMatrix<double>(R, C, startRow, startColumn);

        D = 3.0 * A;
        D.print();

        D = A + A;
        D.print();

        D = A * A;
        D.print();

   
        // Tensors, calculate powers of a matrix and print
        int nrows = 2;
        int ncols = 2;
        int ndepth = 100;

        NumericMatrix<double> T = new NumericMatrix<double>(nrows, ncols);
        T[1, 1] = 1.0; T[2, 2] = 1.0;
        T[1, 2] = .001;  T[2, 1] = 0.0;

        Tensor<double> myTensor = new Tensor<double>(nrows, ncols, ndepth);
        myTensor[myTensor.MinThirdIndex] = T;

        for (int j = myTensor.MinThirdIndex + 1; j <= myTensor.MaxThirdIndex; j++)
        {
                myTensor[j] = T * myTensor[j-1];
        }

        for (int j = myTensor.MinThirdIndex + 1; j <= myTensor.MaxThirdIndex; j++)
        {
             // Print each so often matrix
            if ((j / 10) * 10 == j)
            {
                myTensor[j].print();
            }
        }
       
        // Out of bounds exceptions
        try
        {
            Array<int> myArr = new Array<int>(20);
            myArr[10000] = 34;
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine(e.Message);
        }
    
        

        int N = 35;
        int N2 = 100000;

        NumericMatrix<double> A2 = new NumericMatrix<double>(N, N);
        NumericMatrix<double> B2 = new NumericMatrix<double>(N, N2);

        Console.WriteLine("Processing...");
        NumericMatrix<double> C2 = A2 * B2;
     

        N = 50;
        N2 = 1000;

        Console.WriteLine("Processing...");
        NumericMatrix<double> A3 = new NumericMatrix<double>(N, N);
        NumericMatrix<double> B3 = new NumericMatrix<double>(N, N);
        NumericMatrix<double> C3;

        for (int i = 0; i <= N2; i++)
        {
            C3 = A3 * B3;
        }

  
    }
}