// TestLattice101.cs
//
// Testing the Latice class (basis for binomial and trinomial
// trees). In this version we have one class for both binomial and trinomial
// trees.
//
// (C) Datasim Education BV 2003-2013
//


using System;

class TestBinomial101
{

    public static void Main()
    {
	
	    int typeB = 2;	// Binomial Lattice Type
	    int typeT = 3;	// Trinomial Lattice Type

    	int depth = 4;  // Number of periods of time

        double val = 0.0;

    	Lattice<double> lattice1 = new Lattice<double>(depth, typeB, val);
        Lattice<double> lattice2 = new Lattice<double>(depth, typeT, val);

	    // Trinomial lattice with matrix NRows X NCols entries
	    int NRows = 3;
	    int NCols = 2;
    	int startIndex = 1;
    	int N = 10;		                // Number of time steps
	    Matrix<double> nodeStructure = new Matrix<double>(NRows, NCols, startIndex, startIndex);
	    Lattice<Matrix<double>> trinomialLattice = new Lattice<Matrix<double>>(N, typeT, nodeStructure);


        // Examining the vector at base of lattice
        Vector<double> base1 = lattice1.BasePyramidVector();
        Vector<double> base2 = lattice2.BasePyramidVector();


        Console.WriteLine(base1.Size); Console.WriteLine(base2.Size); 

    }

}

