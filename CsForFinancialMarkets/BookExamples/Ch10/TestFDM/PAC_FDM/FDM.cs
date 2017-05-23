// FDM.cpp
//
// FDM scheme for 1 factor Black Scholes equation. For the
// moment we assume explicit Euler.
//
// The responsibility of this 
// (C) Datasim Education BV 2005-2013
//
//


class FDM
{
        IBSPde pde; // PDE interface
	
		public Vector<double> a, bb, c;	// LHS coefficients at level n
		public Vector<double> RHS;		// Inhomogeneous term
		
		public Vector<double> vecOld;
		public Vector<double> result;
   

		public FDM(IBSPde myPDE)
		{
            pde = myPDE;
		}

		public void initIC(Vector<double> xarr)
		{ // Initialise the solutin at time zero. This occurs only 
		  // at the interior mesh points of xarr (and there are J-1 
		  // of them).
		  

			vecOld = new Vector<double> (xarr.Size, xarr.MinIndex);

			// Initialise at the boundaries
			vecOld[vecOld.MinIndex] = pde.BCL(0.0);
			vecOld[vecOld.MaxIndex] = pde.BCR(0.0);

			// Now initialise values in interior of interval using
			// the initial function 'IC' from the PDE
			for (int j = xarr.MinIndex+1; j <= xarr.MaxIndex-1; j++)
			{
				vecOld[j] = pde.IC(xarr[j]);
			}

			result = vecOld;
		
		}

		public Vector<double> current()
		{
			return result;
		}

		public void calculateCoefficients(Vector<double> xarr, double tprev, double tnow)
		{ // Calculate the coefficients for the solver

			// Explicit method
		//	A = Vector<double> (xarr.Size, xarr.MinIndex, 0.0);
		//	C = A;
		//	B = Vector<double> (xarr.Size, xarr.MinIndex, 1.0);

			a = new Vector<double> (xarr.Size-2, xarr.MinIndex+1, 0.0);
			bb = new Vector<double> (xarr.Size-2, xarr.MinIndex+1, 0.0);
			c = new Vector<double> (xarr.Size-2, xarr.MinIndex+1, 0.0);
			RHS = new Vector<double> (xarr.Size-2,xarr.MinIndex+1, 0.0);

			double tmp1, tmp2;
			double k = tnow - tprev;
			double h = xarr[xarr.MinIndex+1] - xarr[xarr.MinIndex];

			for (int j = xarr.MinIndex+1; j <= xarr.MaxIndex-1; j++)
			{

				tmp1 = k * (pde.sigma(xarr[j], tprev)/(h*h));
				tmp2 = k * (pde.mu(xarr[j], tprev)* 0.5/h);
	
				a[j] = tmp1 - tmp2;
				bb[j] = 1.0 - (2.0 * tmp1) + (k * pde.b(xarr[j], tprev));
				c[j] = tmp1 + tmp2;
				RHS[j] = k * pde.f(xarr[j], tprev);
			}

		}

		public void solve (double tnow)
		{
			// Explicit method

			result[result.MinIndex] = pde.BCL(tnow);
			result[result.MaxIndex] = pde.BCR(tnow);

			for (int i = result.MinIndex+1; i <= result.MaxIndex-1; i++)
			{
				result[i] = (a[i] * vecOld[i-1])
									+ (bb[i] * vecOld[i])
									+ (c[i] * vecOld[i+1]) - RHS[i];
			}
			vecOld = result;
		}
}
