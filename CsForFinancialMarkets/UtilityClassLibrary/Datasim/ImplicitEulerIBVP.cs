// IEulerIbvpSolver.hpp
//
// Solvers for initial boundary value problems. We 
// use an abstract base class; derived class implement
// specific behaviour.
//
// 2005-9-36 DD kick-off
// 2005-10-1 DD next rev.
// 2005-10-11 DD last revision?
// 2005-11-30 DD new cpp file
// 2006-1-10 fixng + aligning indices
// 2008-10-7 DD fitting factor
// 2008-12-15 DD ported to C#
// 2012-11-15 DD clean-up
//
// (C) Datasim Education BV 2003-2013
//

using System;

public class ImplicitEulerIBVP : IBVPFDM
{
    // We must build these in constructors

    // Notice that we store the data that 'belongs' to
    // this class. It is private and will not pollute the
    // other classes.
    private Vector<double> A, B, C; // Lower, diagonal, upper
    private Vector<double> F;	  // Right-hand side of matrix

    

    public ImplicitEulerIBVP(IIBVPImp source,Range<double> Xrange, Range<double> Trange, int NSteps, int JSteps)
        : base(source, Xrange, Trange, NSteps, JSteps)
    {
        // !!! number of unknowns is J - 1 (Dirichlet)
        A = new Vector<double>(J - 1, 1);
        B = new Vector<double>(J - 1, 1);
        C = new Vector<double>(J - 1, 1);

        F = new Vector<double>(J - 1, 1);

    }

    override public void calculateBC()
    { // Tells how to calculate sol. at n+1

        vecNew[vecNew.MinIndex] = pde.BCL(tnow);
        vecNew[vecNew.MaxIndex] = pde.BCR(tnow);

    }

    override public void calculate()
    { // Tells how to calculate sol. at n+1

		// In general we need to solve a tridiagonal system

		double tmp1, tmp2;
		
		for (int i = F.MinIndex; i <= F.MaxIndex; i++)
		{
			tmp1 = (k * fitting_factor(xarr[i+1],tnow ));
			tmp2 = (0.5 * k * h* (pde.convection(xarr[i+1], tnow)));

			// Coefficients of the U terms
			A[i] = tmp1 - tmp2;
			B[i] = -h2 - (2.0*tmp1) + (k*h2*(pde.zeroterm(xarr[i+1],tnow)));
			C[i] = tmp1 + tmp2;

			F[i] = h2*(k * (pde.RHS(xarr[i+1], tnow)) - vecOld[i+1]); //?&
		}

		// Correction term for RHS
		F[1]	-= A[1] * vecNew[vecNew.MinIndex];
		F[J-1]	-= C[J-1] * vecNew[vecNew.MaxIndex] ;


		// Now solve the system of equations
        LUTridiagonalSolver mySolver = new LUTridiagonalSolver(A, B, C, F);

		// The matrix must be diagonally dominant; we call the
		// assert macro and the programs stops


		Vector <double> solution = mySolver.solve();

		for (int ii = vecNew.MinIndex+1; ii <= vecNew.MaxIndex-1; ii++)
		{
			vecNew[ii] = solution[ii-1];
		}
			
		
}

    private double coth(double x)
    { // Hyperbolic cotangent function

        double tmp = Math.Exp(-2 * x);
        return (1.0 + tmp) / (1.0 - tmp);
    }


    // Exponential fitting code
    private double fitting_factor(double x, double t)
    { // Il'in fitting function, just a modification of the vol

        double tmp = pde.convection(x, t) * h * 0.5;

        // Special case convection == 0
        if (tmp != 0.0) // simple test
        {
            return tmp * coth(tmp / (pde.diffusion(x, t))); // Uses hyperbolic cotangent
        }
        else
        {
            return pde.diffusion(x, t);
        }
    }

}