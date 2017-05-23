// TestFDM.cs
//
// Testing 1 factor BS model.
//
// 2011-1-31 DD C# version.
//
// (C) Datasim Education BV 2005-2013
//

using System;
using System.Collections.Generic;

class TestBlackScholes
{

static void printOneExcel(Vector<double> x, Vector<double> functionResult,
					            string title)
{ 
	// N.B. Excel has a limit of 8 charts; after that you get a run-time error

	ExcelDriver  excel = new ExcelDriver();

	excel.MakeVisible(true);		// Default is INVISIBLE!

	excel.CreateChart(x, functionResult, title, "X", "Y");
}

// Excel output as well
static void printInExcel(Vector<double> x,							// X array
					List<string> labels,							// Names of each vector
					List<Vector<double> > functionResult)	// The list of Y values
{ // Print a list of Vectors in Excel. Each vector is the output of
  // a finite difference scheme for a scalar IVP

	

	ExcelDriver  excel = new ExcelDriver();

	excel.MakeVisible(true);		// Default is INVISIBLE!


	// Don't make the string names too long!!
	excel.CreateChart(x, labels, functionResult, "FDM Scalar IVP",
						"Time Axis", "Value");

}


    public static void Main()
    {
        // Option data
        double expiry = 0.25;
        double strike = 10.0;
        double volatility = 0.30;
        double interest = 0.06;
        double dividend = 0.0;
        double truncation = 5*strike; // Magic number

        /* // P = 5.84..
        double expiry = 0.25;
        double strike = 65.0;
        double volatility = 0.30;
        double interest = 0.08;
        double dividend = 0.0;
        double truncation = 5 * strike; // Magic number
         */

        Pde_BS pde = new Pde_BS(expiry, strike, volatility, interest, dividend, truncation);

        // Numerical data 
        int J = 325;
        int NT = 300*300;

        Console.WriteLine("Explicit method, be patient ...");

        // Create the mesh
        Mesher1D mesh = new Mesher1D(0.0, truncation, expiry);
        Vector<double> xarr = new Vector<double>(mesh.xarr(J));
        Vector<double> tarr = new Vector<double>(mesh.tarr(NT));

        FDM fdm = new FDM(pde);
        FDMDirector fdir = new FDMDirector(fdm, xarr, tarr);

        fdir.Start();

	    printOneExcel(xarr, fdir.current(), "Value"); // Display in Excel
	 }
}
