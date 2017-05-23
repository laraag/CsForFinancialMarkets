// matrixsolvermechanisms.cpp
//
// Classes and functions for solving linear systems of equations 
// (numerical linear algebra).
//
// Modification Dates:
//
//	DD 2003-1-14 First code (tridiagonal)
//	DD 2003-1-16 DD small corrections: still to code 2 member functions
//	DD 2003-8-2 LU decomposition of tridiagonal systems; 2 functions
//	(Boolean checks added)
//  DD 2003-8-5 Matrix iterative solvers (Jacobi, Gauss Seidel)
//	DD 2003-8-21 Debugging and good testing
//	DD 2003-9-1 Debugging Jacobi
//	DD 2004-4-10 New implementation using the Template Method pattern
//	DD 2004-4-11 Implement PSOR as a derived class. Lots of reuse
//  DD 2005-10-6 LU version for C++ Intro book. Optimised, no copy of arrays
//  DD 2005-10-15 Function to test diagonal dominance
//	DD 2006-1-10 Clean up cout stuff
//  DD 2008-12-15 ported to C#
//
// (C) Datasim Education BV 2003-2009
//

using System;

public class LUTridiagonalSolver
{ // Solve tridiagonal matrix equation

    // Defining arrays (input)
    // V2 optimise so to work with pointers
    private Vector<double> a;	// The lower-diagonal array [1..J]
    private Vector<double> b;	// The diagonal array [1..J] "baseline array"
    private Vector<double> c;	// The upper-diagonal array [1..J]
    private Vector<double> r;	// The right-hand side of the equation Au = r [1..J]

    // Work arrays

    // Coefficients of Lower and Upper matrices: A = LU
    // V2 use of Templated static vectors, but we must be careful
    private Vector<double> beta;	// Range [1..J]
    private Vector<double> gamma;	// Range [1..J-1]

    // Solutions of temporary and final problems
    private Vector<double> z;		// Range [1..J]
    private Vector<double> u;		// Range [1..J]

    private int Size;

    public LUTridiagonalSolver()
    {

        a = new Vector<double>(1);
        b = new Vector<double>(1);
        c = new Vector<double>(1);
        r = new Vector<double>(1);

    /*    Console.WriteLine("dump");
        a.extendedPrint();
        b.extendedPrint();
        c.extendedPrint();
        r.extendedPrint();*/

        Size = 1;
    }

    public LUTridiagonalSolver(Vector<double> lower, Vector<double> diagonal, Vector<double> upper, Vector<double> RHS)
    {

        a = new Vector<double>(lower);
        b = new Vector<double>(diagonal);
        c = new Vector<double>(upper);
        r = new Vector<double>(RHS);

        Size = diagonal.Size;

   /*     Console.WriteLine("dump");
        a.extendedPrint();
        b.extendedPrint();
        c.extendedPrint();
        r.extendedPrint();*/

    }

    public LUTridiagonalSolver(LUTridiagonalSolver source)
    {

        a = new Vector<double>(source.a);
        b = new Vector<double>(source.b);
        c = new Vector<double>(source.c);
        r = new Vector<double>(source.r);

        Size = source.Size;

    }


    private void calculateBetaGamma()
    { // Calculate beta and gamma

        // Size == J

        // Constructor derived from Array (size, startIndex [,value])
        beta = new Vector<double>(Size, 1);
        gamma = new Vector<double>(Size - 1, 1);

        beta[1] = b[1];
  //      Console.Write(c[1]);
        gamma[1] = c[1] / beta[1];

        for (int j = 2; j <= Size - 1; j++)
        {
            beta[j] = b[j] - (a[j] * gamma[j - 1]);
            gamma[j] = c[j] / beta[j];

        }

        beta[Size] = b[Size] - (a[Size] * gamma[Size - 1]);

        /*
       // Constructor derived from Array (size, startIndex [,value])
	beta = Vector<V,I> (Size, 1);
	gamma = Vector<V,I> (Size - 1, 1);

	beta[1] = b[1];
	gamma[1] = c[1] / beta[1];

	for (I j = 2; j <= Size-1; j++)
	{
		beta[j] = b[j] - (a[j] * gamma[j-1]);
		gamma[j] = c[j]/beta[j];

	}
	
	beta[Size] = b[Size] - (a[Size] * gamma[Size-1]);
*/  
     /*   Console.WriteLine("beta, gamma");
        beta.extendedPrint();
        gamma.extendedPrint();*/
    }

    private void calculateZU()
    { // Calculate z and u

        z = new Vector<double>(Size, 1);
        u = new Vector<double>(Size, 1);

        // Forward direction
        z[1] = r[1] / beta[1];

        for (int j = 2; j <= Size; j++)
        {
            z[j] = (r[j] - (a[j] * z[j - 1])) / beta[j];

        }

        // Backward direction
        u[Size] = z[Size];

        for (int i = Size - 1; i >= 1; i--)
        {
            u[i] = z[i] - (gamma[i] * u[i + 1]);

        }

        /*
         z = Vector<V,I> (Size, 1);
	u = Vector<V,I> (Size, 1);

	// Forward direction
	z[1] = r[1] / beta[1];

	for (I j = 2; j <= Size; j++)
	{
		z[j] = (r[j] - (a[j]*z[j-1]) ) / beta[j];
	
	}

	// Backward direction
	u[Size] = z[Size];

	for (I i = Size - 1; i >= 1; i--)
	{
		u[i] = z[i] - (gamma[i]*u[i+1]);
	
	}*/
     /*   Console.WriteLine("z, u");
        z.extendedPrint();
        z.extendedPrint();*/
    }

    // Calculate the solution to Au = r
    public Vector<double> solve()
    {
        //cout << "Solving " << endl;

        calculateBetaGamma();		// Calculate beta and gamma
        calculateZU();				// Calculate z and u

        return u;

    }

    public bool diagonallyDominant()
    {
	if (Math.Abs(b[1]) < Math.Abs(c[1]))
		return false;

	if (Math.Abs(b[Size]) < Math.Abs(a[Size]))
		return false;

	for (int j = 2; j <= Size-1; j++)
	{
		if (Math.Abs(b[j]) < Math.Abs(a[j]) + Math.Abs(c[j]) )
			return false;
	}
        

        return true;
    }


}