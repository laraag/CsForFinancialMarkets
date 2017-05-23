// BinomialLatticeStrategy.cs
//
// Strategy pattern for creating binomial lattice. Note
// that there is a built-in Template Method pattern built
// in here.
// 
// For convenience all data is public and all code is inline.
// Furthermore, classes have minimal functionality. We can make 
// production code from this at a later stage.
//
// Last Modification Dates:
//
//	2006-4-7 DD cpp file now
//  2010-7-28 DD C# 
//  2010-11-4 DD Pade approximation
//
// (C) Datasim Education BV 2005-2010
//

using System;

public enum BinomialType : uint
{
    Additive = 1,
    Multiplicative = 2,
}


public class BinomialLatticeStrategy
{

        protected double u;
		protected double d;
		protected double p;

		protected double s;
		protected double r;
		protected double k;

        public BinomialType bType;

        public BinomialLatticeStrategy(double vol, double interest, double delta)
        {
			s = vol;
			r = interest;
			k = delta;
			bType = BinomialType.Multiplicative;
        }

        // Useful function
        public void UpdateLattice (Lattice<double> source, double rootValue) 
        {	// Find the depth of the lattice; this a Template Method Pattern

			int si = source.MinIndex;
			source[si,si] = rootValue;

			// Loop from the min index to the end index
			for (int n = source.MinIndex + 1; n <= source.MaxIndex; n++)
			{
				for (int i = 0; i < source.NumberColumns(n); i++)
				{
					source[n,i] = d * source[n-1,i];
					source[n,i+1] = u * source[n-1,i];
				}
    		}
        }

        public double downValue() { return d;}
		public double upValue() { return u;}
		public double probValue() { return p;}

}

public class CRRStrategy: BinomialLatticeStrategy
{
	
        public CRRStrategy(double vol, double interest, double delta) 
		        : base(vol, interest, delta)
        {
		        /* double e = Math.Exp((r)*k);
		        double sr = Math.Sqrt(exp(vol*vol*k) - 1.0);
		        u = e * (1.0 + sr);
		        d = e * (1.0 - sr);*/

		        double R1 = (r - 0.5 * s * s) * k;
		        double R2 = s * Math.Sqrt(k);

				u = Math.Exp(R1 + R2);
	        	d = Math.Exp(R1 - R2);

        		double discounting = Math.Exp(- r*k);
	    
	        	p = 0.5;
    }

}


public class JRStrategy: BinomialLatticeStrategy
{

        public JRStrategy(double vol, double interest, double delta) 
		                            : base(vol, interest, delta)
        {
		        double k2 = Math.Sqrt(k);
		        u = Math.Exp(s * k2);

		        d = 1.0/u;

		        p = 0.5 + ((r - 0.5 * s * s) * k2 * 0.5) / s;

        }

}

public class EQPStrategy: BinomialLatticeStrategy
{
        public EQPStrategy(double vol, double interest, double delta) 
		                        : base(vol, interest, delta)
        {

                bType = BinomialType.Additive;

	            // Needed for additive method: page 19 Clewlow/Strickland formula 2.17 
	            // "v" is "nu" here, for "v" see  page 19 formula 2.14
	            double nu = r - 0.5 * s * s;

	            double a = nu * k;
	            double b = 0.5 * Math.Sqrt( (4.0 * s * s * k) - (3.0 * nu * nu * k * k) );

	            // EQP parameters: page 19 formula 2.17
        	    u = 0.5 * a + b;														
	            d = 1.5 * a - b;

	            p = 0.5;
        }
}


public class TRGStrategy: BinomialLatticeStrategy
{
        public TRGStrategy(double vol, double interest, double delta) 
		                        : base(vol, interest, delta)
        {

                bType = BinomialType.Additive;

	            // Needed for additive method: page 19 formula 2.19 
	            // "v" is "nu" here, for "v" see  page 17 formula 2.14
	            double nu = r - 0.5 * s * s;

	            double nudt = nu * k;

	            // TRG parameters: page 19 formula 2.19
	            u = Math.Sqrt( s * s * k + nudt * nudt );
	            d = - u;

	            p = 0.5 * (1.0 + (nudt/u) );
        }
}

public class ModCRRStrategy: BinomialLatticeStrategy
{
        public ModCRRStrategy(double vol, double interest, double delta, double S, double K, int N) 
		                    : base(vol, interest, delta)
        {
		
		        // s == volatility, k = step size in time
		        double KN = Math.Log(K/S) / N;
		        double VN = s * Math.Sqrt(k);

		        u = Math.Exp(KN + VN);
		        d = Math.Exp(KN - VN);

		        p = (Math.Exp(r * k) - d) / (u - d);

        }

}

// Pade approximants

public class PadeCRRStrategy: BinomialLatticeStrategy
{
        public PadeCRRStrategy(double vol, double interest, double delta) 
		        : base(vol, interest, delta)
        {
		        double R1 = (r - 0.5 * s * s) * k;
		        double R2 = s * Math.Sqrt(k);

    	        // Cayley transform
		        double z1 = (R1 + R2);
		        double z2 = (R1 - R2);

		        u = (2.0 + z1) / (2.0 - z1);
		        d = (2.0 + z2) / (2.0 - z2);
	
    	        p = 0.5;
        }
}

public class PadeJRStrategy: BinomialLatticeStrategy
{

        public PadeJRStrategy(double vol, double interest, double delta) 
		        : base(vol, interest, delta)
        {

		        double k2 = Math.Sqrt(k);

		        // Cayley transform
		        double z = s * Math.Sqrt(k);

		        double num = 12.0 - (6.0*z) + (z*z);
		        double denom = 12.0 + (6.0*z) + (z*z);

		        d = num/denom;
		        u = denom/num;
		
		        p = 0.5 + ((r - 0.5 * s * s) * k2 * 0.5) / s;

        }
}