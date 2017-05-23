// TestLattice101.cs
//
// Testing the Latice class (basis for binomial and trinomial
// trees). In this version we have one class for both binomial and trinomial
// trees.
//
// (C) Datasim Education BV 2003-2013
//


using System;

class TestBinomialExcel
{

    public static void Main()
    {
	
	    int typeB = 2;	// Binomial Lattice Type
	    int typeT = 3;	// Trinomial Lattice Type

    	int depth = 4;  // Number of periods of time

        double val = 4.0;

    	Lattice<double> lattice1 = new Lattice<double>(depth, typeB, val);
        Lattice<double> lattice2 = new Lattice<double>(depth, typeT, val);

	  
        // Examining the vector at base of lattice
        Vector<double> base1 = lattice1.BasePyramidVector();
        Vector<double> base2 = lattice2.BasePyramidVector();

        // Print columms of lattice
        for (int j = lattice1.MinIndex; j <= lattice1.MaxIndex; j++)
        {
            lattice1.PyramidVector(j).print();
        }
        
        string s = Console.ReadLine();

        // Arrays
        int startIndex = lattice1.MinIndex;
        Vector<double> xarr = new Vector<double>(depth + 1, startIndex);
        xarr[xarr.MinIndex] = 0.0;
        double T = 1.0;
        int NT = 10;
        double delta_T = T / NT;
        for (int j = xarr.MinIndex + 1; j <= xarr.MaxIndex; j++)
        {
            xarr[j] = xarr[j - 1] + delta_T;
        }
        
        Console.WriteLine(base1.Size); Console.WriteLine(base2.Size);

        ExcelMechanisms exl = new ExcelMechanisms();

        try
        {
            exl.printLatticeInExcel(lattice2, xarr, "Lattice");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }

}

