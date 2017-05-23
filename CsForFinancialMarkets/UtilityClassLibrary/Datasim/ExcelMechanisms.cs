// ExcelMechanisms.cs
//
// 28 December 2005  Robbert van der Plas
// 2008-12-15 DD function to print arrays
// 2009-4-3 DD print associative matrices in Excel
// 2010-10-31 DD Lattice<T> display
//
// ExcelMechanisms class. 
// Useful functions for use with Excel.
//
// (C) Datasim Component Technology 2005

using System.Collections.Generic;

public class ExcelMechanisms
{
    private static ExcelDriver excel;

    static ExcelMechanisms()
    { // This static constructor ensures that only one instance of Excel is started

          excel = new ExcelDriver();
          excel.MakeVisible( true );
    }

    public ExcelMechanisms()
    {
      //  excel = new ExcelDriver();
      //  excel.MakeVisible( true );
    }

    public void printOneExcel<T>( Vector<T> x, Vector<T> functionResult, string title, string horizontal, string vertical, string legend )
    {
        List<string> legendList = new List<string>();
        legendList.Add( legend );
        List<Vector<T>> functionResultList = new List<Vector<T>>();
        functionResultList.Add( functionResult );

        excel.CreateChart( x, legendList, functionResultList, title, horizontal, vertical );
    }

    // Print a list of Vectors in Excel. Each vector is the output of
    // a finite difference scheme for a scalar IVP
    public void printInExcel<T>( Vector<T> x, List<string> labels, List<Vector<T>> functionResult, string title, string horizontal, string vertical )
    {
        excel.CreateChart( x, labels, functionResult, title, horizontal, vertical );
    }

    // Print the vector that is the difference of two vectors
    public void printDifferenceInExcel<T>( Vector<T> x, Vector<T> y1, Vector<T> y2, string title, string horizontal, string vertical, string legend )
    {
        Vector<T> diffVector = y1 - y2;

        printOneExcel( x, diffVector, title, horizontal, vertical, legend );
    }

    // Print a two-dimensional array (typically, one time level)
    public void printMatrixInExcel<T>( NumericMatrix<T> matrix, Vector<T> xarr, Vector<T> yarr, string SheetName )
    {
        List<string> rowlabels = new List<string>();
        List<string> columnlabels = new List<string>();

        for( int i = xarr.MinIndex; i <= xarr.MaxIndex; i++ )
        {
            rowlabels.Add( xarr[ i ].ToString() );
        }
        for( int i = yarr.MinIndex; i <= yarr.MaxIndex; i++ )
        {
            columnlabels.Add( yarr[ i ].ToString() );
        }

        excel.AddMatrix( SheetName, matrix, rowlabels, columnlabels );
    }

    // Print a lattice
  //  public void printLatticeInExcel<T>(Lattice<T> lattice, Vector<T> xarr, Vector<T> yarr, string SheetName)
  public void printLatticeInExcel(Lattice<double> lattice, Vector<double> xarr, string SheetName)
    {
        List<string> rowlabels = new List<string>();
 
        for (int i = xarr.MinIndex; i <= xarr.MaxIndex; i++)
        {
            rowlabels.Add(xarr[i].ToString());
        }
     
        excel.AddLattice(SheetName, lattice, rowlabels);
    }
    
    // Print a two-dimensional associative array (typically, one time level);
    // Recall: row, column and value parameters
    public void printAssocMatrixInExcel<R, C, T>(AssocMatrix<R, C, T> matrix, string SheetName)
    {
        List<string> rowlabels = new List<string>();
        List<string> columnlabels = new List<string>();
        
        /*foreach (KeyValuePair<Key, Value> kvp in str)
        {
            Console.WriteLine("{0}, {1}", kvp.Key, kvp.Value);
        }  
    */
        // Create the row and column annotations
        foreach (KeyValuePair<R, int> value in matrix.r)
        {
           rowlabels.Add(value.Key.ToString());
        }

        foreach (KeyValuePair<C, int> value in matrix.c)
        {
            columnlabels.Add(value.Key.ToString());
        } 

       excel.AddMatrix(SheetName, matrix.mat, rowlabels, columnlabels);
    }

   /* public void printMatrixInExcel<T>(Vector<T> vector, double xVal, Vector<T> yarr, string SheetName)
    {
        List<string> rowlabels = new List<string>();
        List<string> columnlabels = new List<string>();

        NumericMatrix<T> matrix = new NumericMatrix<T>(vector, 1, yarr.MaxIndex);

        rowlabels.Add(xVal.ToString());
        
        for (int i = yarr.MinIndex; i <= yarr.MaxIndex; i++)
        {
            columnlabels.Add(yarr[i].ToString());
        }


        excel.AddMatrix(SheetName, matrix, rowlabels, columnlabels);
    }*/

    // Print the matrix that is the difference of two matrices
    public void printMatrixDifferenceInExcel<T>( NumericMatrix<T> matrix1, NumericMatrix<T> matrix2, Vector<T> xarr, Vector<T> yarr, string SheetName )
    {
        NumericMatrix<T> m2 = matrix1 - matrix2;
        printMatrixInExcel( m2, xarr, yarr, SheetName );
    }

    // Print an array of matrices
    public void printTensorInExcel<T>( Tensor<T> tensor )
    {
        List<string> rowlabels = new List<string>( tensor.MaxFirstIndex );
        List<string> columnlabels = new List<string>( tensor.MaxSecondIndex );

        for( int i = tensor.MinFirstIndex; i <= tensor.MaxFirstIndex; i++ )
        {
            rowlabels.Add( "" );
        }
        for( int i = tensor.MinFirstIndex; i <= tensor.MaxFirstIndex; i++ )
        {
            columnlabels.Add( "" );
        }

        string name;

        for( int i = tensor.MaxThirdIndex; i >= tensor.MinThirdIndex; i-- )
        {
            name = i.ToString();
            excel.AddMatrix( name, tensor[ i ], rowlabels, columnlabels );
        }
    }

    // Print an array of matrices
    public void printTensorInExcel<T>( Tensor<T> tensor, Vector<T> xarr, Vector<T> yarr, string SheetName )
    {
        List<string> rowlabels = new List<string>();
        List<string> columnlabels = new List<string>();
        for( int i = xarr.MinIndex; i <= xarr.MaxIndex; i++ )
        {
            rowlabels.Add( xarr[ i ].ToString() );
        }
        for( int i = yarr.MinIndex; i <= yarr.MaxIndex; i++ )
        {
            columnlabels.Add( yarr[ i ].ToString() );
        }

        string tmp;

        for( int i = tensor.MaxThirdIndex; i >= tensor.MinThirdIndex; i-- )
        {
            tmp = SheetName + " " + i.ToString();
            excel.AddMatrix( tmp, tensor[ i ], rowlabels, columnlabels );
        }
    }

    public void printDiscreteFunctionValues( double x, double A, double B, long nSteps, string title, string horizontal, string vertical, string legend )
    {
        double h = ( B - A ) / ( double )nSteps;
        Vector<double> mesh = new Vector<double>( ( int )nSteps + 1, 1 );

        double val = A;

        for( int i = 1; i <= nSteps + 1; i++ )
        {
            mesh[ i ] = val;
            val += h;
        }

        //now array of values
        Vector<double> result = new Vector<double>( ( int )nSteps + 1, 1 );
        for( int i = 1; i <= nSteps + 1; i++ )
        {
            result[ i ] = mesh[ i ];
        }

        printOneExcel( mesh, result, title, horizontal, vertical, legend );
    }
}