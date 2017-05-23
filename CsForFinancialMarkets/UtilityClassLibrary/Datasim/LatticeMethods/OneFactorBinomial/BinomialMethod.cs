// BinomialMethod.cs
//
// An encapsulation of the Binomial Method. This is
// a kind of Mediator between the underlying data
// stucture (a lattice tree) and the different kinds
// of algorithms for initialising the tree.
//
// We have kept code inline for convenience only.
//
// Last Modification dates:
//
//	DD 2005-2-19 New cpp file
//	DD 2005-11-3 Debugging
//	DD 2006-4-7 New for get lattice
//  DD 2010-7-28 C# version
//
// (C) Datasim Education BV 2004-2010
//

using System;

public class BinomialMethod
{ // Simple model for GBM

        // Underlying data structure
		private Lattice<double> lattice;        	// Magic number == 2 means binomial
		private BinomialLatticeStrategy str;		// Reference to an algorithm

        
        // The possibility to define constraints on top of the European 
        // option solution, e.g. early exercise, barriers
        public delegate double ConstraintMethod(double Price, double S);
        ConstraintMethod con;
        bool constraintExists;

		private double disc;

        public BinomialMethod (double discounting, BinomialLatticeStrategy strategy, int N) 
        {
	
		    disc = discounting;
		    str = strategy;
		    BuildLattice(N);
            constraintExists = false;
        }

        public BinomialMethod (double discounting, BinomialLatticeStrategy strategy, int N,
            ConstraintMethod constraint) 
        {
	
		    disc = discounting;
		    str = strategy;
		    BuildLattice(N);

            con = new ConstraintMethod(constraint);
            constraintExists = true;
        }

        private void BuildLattice(int N)
        { // Build a binomial lattice

	        double val = 0.0;
		    lattice = new Lattice<double> (N, 2, val);
        }

        public void modifyLattice(double U)
        { // Forward induction; building the tree 

		    double down = str.downValue();
		    double up = str.upValue();

		    int si = lattice.MinIndex;
		    lattice[si, si] = U;

			// Loop from the min index to the end index
			for (int n = lattice.MinIndex + 1; n <= lattice.MaxIndex; n++)
			{
				for (int i = 0; i < lattice.NumberColumns(n)-1; i++)
				{
					lattice[n,i] = down * lattice[n-1,i];
					lattice[n,i+1] = up * lattice[n-1,i];
				}
			}
	
		    // Postcondition: we now have the complete lattice for the underlying asset
            
        }

        public double getPrice(Vector<double> RHS)
        { // Backward induction; calculate the price based on discete payoff function at t = T
			

                double pr = str.probValue();

                // Initialise the vector at the expiry date/MaxIndex
                int ei = lattice.MaxIndex;
		      

                // Exception handling: sizes of RHS and base vector must be the same
                for (int i = 0; i < lattice.NumberColumns(ei); i++)
                {
                    lattice[ei, i] = RHS[i];
                }

                double S;   // Value at node [n,i] before it gets overwritten
		        // Loop from the max index to the start (min) index
		        for (int n = lattice.MaxIndex - 1; n >= lattice.MinIndex; n--)
		        {
		        	for (int i = 0; i < lattice.NumberColumns(n); i++)
		        	{
                        S = lattice[n,i];
                        lattice[n, i] = disc * (pr * lattice[n + 1, i + 1] + (1.0 - pr) * lattice[n + 1, i]);

			            // Now take early exercise into account
                        if (constraintExists)
                        {
                           lattice[n,i] = con(lattice[n, i], S);
                           
                        }
                    }
		        }

		        int si = lattice.MinIndex;
                return lattice[si, si];
        }

        public Vector<double> BasePyramidVector()
        {
		    return lattice.BasePyramidVector();
        }

        // Underlying lattice
        public Lattice<double> getLattice()
        {
        	return lattice;
        }

}


