using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayAndMatrix
{
    class Program
    {
        static void Main(string[] args)
        {

            // Jagged arrays

            // Lower triangular matrix
            int NR = 10;
            int NC = 10;
            double[][] lowerTriangular = new double[NR][];
            for (int j = 0; j < NC; j++)
            {
                lowerTriangular[j] = new double[j+1];
            }   



            // Generic arrays for starters
            NR = 4;
            NC = 4;
            //MatrixTwoArrayImpl<double> myMatrixStructure = new MatrixTwoArrayImpl<double>(NR, NC);
            UpperTriangularImpl<double> myMatrixStructure = new UpperTriangularImpl<double>(NR, NC);
            GenericMatrix<double, MatrixTwoArrayImpl<double> > myMatrix = new GenericMatrix<double, MatrixTwoArrayImpl<double> >(NR, NC);
            myMatrix.extendedPrint();

            for (int i = myMatrix.MinRowIndex; i <= myMatrix.MaxRowIndex; i++)
            {
                for (int j = myMatrix.MinColumnIndex; j <= myMatrix.MaxColumnIndex; j++)
                {
                    myMatrix[i, j] = -1;
                }
            }
            myMatrix.extendedPrint();

            MatrixOneArrayImpl<double> myMatrixStructure2 = new MatrixOneArrayImpl<double>(NR, NC);
            GenericMatrix<double, MatrixOneArrayImpl<double>> myMatrix2 = new GenericMatrix<double, MatrixOneArrayImpl<double>>(NR, NC);
            myMatrix2.extendedPrint();

            for (int i = myMatrix2.MinRowIndex; i <= myMatrix2.MaxRowIndex; i++)
            {
                for (int j = myMatrix2.MinColumnIndex; j <= myMatrix2.MaxColumnIndex; j++)
                {
                    myMatrix2[i, j] = 2;
                }
            }
            myMatrix2[myMatrix2.MinRowIndex, myMatrix2.MinColumnIndex] = 99;
            myMatrix2[myMatrix2.MaxRowIndex, myMatrix2.MaxColumnIndex] = 98;
          
            myMatrix2.extendedPrint();

            // Algebra and matrix manipulation
            int J = 3;


	        Vector<double> a = new Vector<double>(J,1, 1.0);
	        Vector<double> b = new Vector<double>(J,1, 2.0);
	        Vector<double> c = new Vector<double>(J,1, 1.0);
	        Vector<double> r = new Vector<double>(J,1, 0.0);				// Right-hand side

	
        	for (int i = r.MinIndex ; i <= r.MaxIndex; i++)
        	{
	        	r[i] = 1.0;
	        }

            r[2] = -1.0;
        /*   r[1] = 4.0;
            r[2] = 10.0;
            r[3] = 8.0;*/
    
            Console.WriteLine("LU stuff");
	        LUTridiagonalSolver mySolver = new LUTridiagonalSolver(a, b, c, r);
	        Vector<double> result = mySolver.solve();

            Console.WriteLine("Solution");
	        result.extendedPrint();

	       // Array
            int startIndex = -1;
            Array<double> arr = new Array<double>(10, startIndex);
            for (int j = arr.MinIndex; j <= arr.MaxIndex; j++)
            {
                arr[j] = (double)(j);
            }

            arr[arr.MinIndex] = 99.98;

          //  arr.extendedPrint();


            // Matrix
            Matrix<int> mat = new Matrix<int>(4, 4,2,1);
            mat[mat.MinRowIndex, mat.MinColumnIndex] = 99;
            mat.initCells(3);
            mat[mat.MinRowIndex, mat.MinColumnIndex] = 98;
            mat.extendedPrint();

            for (int i = mat.MinRowIndex; i <= mat.MaxRowIndex; i++)
            {
                for (int j = mat.MinColumnIndex; j <= mat.MaxColumnIndex; j++)
                {
                    mat[i, j] = i* j;
                }
            }
            mat[mat.MinRowIndex, mat.MinColumnIndex] = 98;
            mat.extendedPrint();
          
            // Vectors
            Vector<double> vec = new Vector<double>(10);
            for (int j = vec.MinIndex; j <= vec.MaxIndex; j++)
            {
                vec[j] = (double)(vec.MaxIndex - j);
            }
            vec.print();

            Vector<double> vec2 = vec + vec;
            vec2.print();

            vec2 = vec - vec;
            vec2.print();

            vec2 = vec - 3.0;
            vec2.print();

            vec2 = 2.0 + vec;
            vec2.print();


            // Numeric Matrices, initial tests
            NumericMatrix<double> mat2 = new NumericMatrix<double>(4, 4);
            mat2.initCells(1.0);
            mat2.extendedPrint();
            for (int i = mat2.MinRowIndex; i <= mat2.MaxRowIndex; i++)
            {
                for (int j = mat2.MinColumnIndex; j <= mat2.MaxColumnIndex; j++)
                {
                    mat2[i, j] = i * j;
                }
            }
            Console.WriteLine("Matrix multiplication...");
            Vector<double> vec3 = new Vector<double>(4, 1, 2.0);
           
            Vector<double> vec4 = mat2 * vec3;
            vec4.print();

            NumericMatrix<double> mat4 = mat2 * mat2;
            mat4.extendedPrint();
          
            // Numeric Matrices, testing accuracy
     /*       int rows = 10;
            int columns = 10;
            int startRow = 1;
            int startColumn = 1;
            NumericMatrix<double>  A = new NumericMatrix<double>(rows, columns, startRow, startColumn);
            A[A.MinRowIndex, A.MinColumnIndex] = 1.0; A[A.MinRowIndex, A.MinColumnIndex + 1] = 2.0;
            A[A.MinRowIndex + 1, A.MinColumnIndex] = 3.0; A[A.MinRowIndex + 1, A.MinColumnIndex + 1] = 4.0;
     //       A.extendedPrint();

            int si = 1; // Start index
            Vector<double> x = new Vector<double>(10, si);
            x[si] = 1.0;
            x[si + 1] = 2.0;

            Vector<double> y = A * x;
         //   y.extendedPrint();

            NumericMatrix<double> B = A * A;
         //   B.extendedPrint();

            B = B * B;
            B = B * B;
       //     B.extendedPrint();

            // Modify rows and columns of matrices
            Vector<double> r1 = new Vector<double>(10, 1, 3.3);
            //r1.extendedPrint();
         //   A.Row(A.MinRowIndex, r1);
         //   A.Column(A.MaxColumnIndex, r1);
            A[A.MinRowIndex, A.MinColumnIndex] = 00.0001;
            A[A.MaxRowIndex, A.MaxColumnIndex] = -99.99;
            A[A.MaxRowIndex, A.MinColumnIndex] = 88.88;
            A.extendedPrint();

            // Slices
            Vector<double> vecSlice = A.getRow(A.MaxRowIndex);
            vecSlice.extendedPrint();
            vecSlice = A.getColumn(A.MinColumnIndex);
            vecSlice.extendedPrint();

            int[, ,] tensor = new int[3, 3, 3];
                for (int i = 0; i < tensor.GetLength(0); i++)
                    for (int j = 0; j < tensor.GetLength(1); j++)
                        for (int k = 0; k < tensor.GetLength(2); k++)
                                tensor[i, j, k] = i * j * k;

         /*   for (int i = 0; i < td.GetLength(0); i++)
                for (int j = 0; j < td.GetLength(0); j++)
                    for (int k = 0; k < td.GetLength(0); k++)
                        Console.Write(td[i, j, k]); Console.Write(", ");*/
            //A.Column(2, r1);
            // Tensors
            int nrows = 2;
            int ncols = 2;
            int ndepth = 10;
            Tensor<double> myTensor = new Tensor<double>(nrows, ncols, ndepth);
           // myTensor[myTensor.MinThirdIndex] = B;

            for (int j = myTensor.MinThirdIndex + 1; j <= myTensor.MaxThirdIndex; j++)
            {
         //       myTensor[j] = A*A;
            }

            for (int j = myTensor.MinThirdIndex + 1; j <= myTensor.MaxThirdIndex; j++)
            {
              //  myTensor[j].extendedPrint();
            }
            
        }
    }
}
