 // CubicSpline.cs
 // 
 // Class to represent cubic spline interpolation
 // 
 // Code is default inline and we include some C utility functions
 // 
 // Last Modification Dates:
 // 
 // DD 2006-7-31 Kick-off code
 // DD 2006-8-1 Tested: 1) BC 2nd order terms
 // DD 2009-2-7 C# solution
 // DD 2010-9-5 redesign, less code needed
 // 
 // (C) Datasim Education BV 2006-2009
 // 

 // All equations based on my C++ book, especially chapter 18

 // Description: Given a set of mesh points Xarr (0 to N), a set of function values Yarr
 // (0 to N) at these mesh points (see equation (18.1)) find the cubic spline function
 // that agrees with Yarr at the mesh points and having either of the Boundary conditions
 // as in Equation (18.7)


/* STEPS

	1. Give Xarr and Yarr (Same dimensions!)
	2. Give B.C. (eq. 18.7))
	3. Calculate the sub, main and superdiagonal arrays of LU matrix
	4. Solve AM = b (eq. 18.8)
	5. Choose the abscissa value 'x' where you want the interpolant.
	6. Use this value in the formula for the cubic spline S
*/

 // public enum CubicSplineBC {SecondDeriv, FirstDeriv}  // ;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CubicSplineInterpolator : BaseOneDimensionalInterpolator
{

	private Vector<double> h;		 // Array of mesh sizes in x direction

	private int /*CubicSplineBC*/ type;			 // Type of BC

	private Vector<double> M;				 // Special calculated coefficients of spline
	private Vector<double> A, B, C, r;	 // Input arrays for LU decomposition

	 // For first order derivatives
    private double a, b;

	     // Private member functions
		private void calculateVectors()
		{  // A, B, C and r

			if (type == 1 /*SecondDeriv */)
			{ 
				C[C.MinIndex] = 0.0;
				r[r.MinIndex] = 0.0;

				A[A.MaxIndex] = 0.0;
				r[r.MaxIndex] = 0.0;
			}
			else
			{
				C[C.MinIndex] = 1.0;
				r[r.MinIndex] = 6.0 * ((y[1] - y[0]) - a)/h[1];

				A[A.MaxIndex] = 1.0;
				r[r.MaxIndex] = 6.0 * (b - (y[N] - y[N-1]))/h[N];
			}
		
			double tmp;

			for (int j = 1; j <= N-1; j++)
			{  // Optimise later

				C[j+1] = h[j+1]/(h[j] + h[j+1]);
				A[j] = h[j]/(h[j] + h[j+1]);

				tmp = (y[j+1] - y[j])/h[j+1] - (y[j] - y[j-1])/h[j];

				r[j+1] = (6.0 * tmp)/(h[j] + h[j+1]);
			}

		}



        public CubicSplineInterpolator() { }  // AG
    
        
        
	public CubicSplineInterpolator(Vector<double> xarr, 
								Vector<double> yarr, 
									 // CubicSplineBC BCType, 
                                    int BCType,
										 // double alpha = 0.0,	double beta = 0.0)
                                        double alpha, double beta) : base(xarr, yarr)
	{

		 // Arrays must have the same size
		x = xarr;
		y = yarr;
		N = xarr.Size-1;
		type = BCType;

	
		a = alpha;	 // LHS
		b = beta;	 // RHS

		 // Redundant internal arrays
		int si = x.MinIndex + 1;		 // !! start index
		 // cout << "si" << si;
		 // int t; cin >> t;
		double defVal = 0.0;

		 // Calculate array of offset
		h = new Vector<double>(N, si, defVal);
		for (int j = 1; j <= N; j++)
		{
			h[j] = x[j] - x[j-1];
		}
		 // print(h); cin >> t;

		 // All arrays have start index 1
		 // Compared to the equations in the book, M(j) --> M(j+1)
		M = new Vector<double>(N+1, si, defVal);  // Solution
		A = new Vector<double>(N+1, si, defVal);
		B = new Vector<double>(N+1, si, defVal + 2.0);
		C = new Vector<double>(N+1, si, defVal);
		r = new Vector<double>(N+1, si, defVal);


		 // Calculate the elements 
		calculateVectors();

	 // // /	print(A); print(B); print(C); int y; cin >> y;

		LUTridiagonalSolver mySolver = new LUTridiagonalSolver (A, B, C, r);

		 // The matrix must be diagonally dominant; we call the
		 // assert macro and the programs stops

		 // assert (mySolver.diagonallyDominant() == true); C++!
	
		M = mySolver.solve();
	
	}

    public override void Ini(IEnumerable<double> xarr, IEnumerable<double> yarr)
    {
        x = new Vector<double>(xarr.ToArray(), 0);
        y = new Vector<double>(yarr.ToArray(), 0);
        N = xarr.Count() - 1;
        type = 1;
        a = 0;	 // LHS
        b = 0;	 // RHS

         // Redundant internal arrays
        int si = x.MinIndex + 1;		 // !! start index
         // cout << "si" << si;
         // int t; cin >> t;
        double defVal = 0.0;

         // Calculate array of offset
        h = new Vector<double>(N, si, defVal);
        for (int j = 1; j <= N; j++)
        {
            h[j] = x[j] - x[j - 1];
        }
         // print(h); cin >> t;

         // All arrays have start index 1
         // Compared to the equations in the book, M(j) --> M(j+1)
        M = new Vector<double>(N + 1, si, defVal);  // Solution
        A = new Vector<double>(N + 1, si, defVal);
        B = new Vector<double>(N + 1, si, defVal + 2.0);
        C = new Vector<double>(N + 1, si, defVal);
        r = new Vector<double>(N + 1, si, defVal);


         // Calculate the elements 
        calculateVectors();

         // // /	print(A); print(B); print(C); int y; cin >> y;

        LUTridiagonalSolver mySolver = new LUTridiagonalSolver(A, B, C, r);

         // The matrix must be diagonally dominant; we call the
         // assert macro and the programs stops

         // assert (mySolver.diagonallyDominant() == true); C++!

        M = mySolver.solve();
	
    }


	public override double Solve(double xvar) 
	{  // Find the interpolated valued at a value x)

		int j = findAbscissa(xvar);	 // will give index of LHS value <= x
		

		 // Now use the formula
		double tmp = xvar - x[j];
		double tmpA = x[j+1] - xvar;
		double tmp3 = tmp * tmp * tmp;
		double tmp4 = tmpA * tmpA * tmpA;

		double A = (y[j+1] - y[j])/h[j+1] - (h[j+1] * (M[j+2] - M[j+1]))/6.0;
		double B = y[j] - (M[j+1] * h[j+1] * h[j+1])/6.0; 

		double result = (M[j+1] * tmp4)/(6.0 * h[j+1])
							+ (M[j+2] * tmp3)/(6.0 * h[j+1])
								+ (A * tmp)
									+ B;

        return result;

	}
}


