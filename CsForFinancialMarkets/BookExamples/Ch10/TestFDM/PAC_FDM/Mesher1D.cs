// mesher.cpp
//
// A simple mesher on a 1d domain. We divide
// an interval into J+1 mesh points, J-1 of which
// are internal mesh points.
//
// 2011-1-31 DD C# version (from C++)
//
// (C) Datasim Education 2011-2013
//
class Mesher1D
{
		public double a, b;	    // In space
		public double HT;		// In time

		public Mesher1D()
		{
			a =0.0; b = 1.0;
			HT = 1.0;
		}

		public Mesher1D (double A, double B, double T)
		{ // Describe the domain of integration

			a = A;
			b = B;
            HT = T; 
		}
            
		public Vector<double> xarr(int J)
		{
			// NB Full array (includes end points)

			double h = (b - a) / J;
			
			int size = J+1;
			int start = 1;

			Vector<double> result = new Vector<double>(size, start);
			result[result.MinIndex] = a;

			for (int j = result.MinIndex+1; j <= result.MaxIndex; j++)
			{
				result[j] = result[j-1] + h;
			}

			return result;
		}

        public Vector<double> tarr(int N)
        {
            // NB Full array (includes end points)

            double h = HT / N;

            int size = N + 1;
            int start = 1;

            Vector<double> result = new Vector<double>(size, start);
            result[result.MinIndex] = 0.0;
            result[result.MaxIndex] = HT;

            for (int j = result.MinIndex + 1; j <= result.MaxIndex-1; j++)
            {
                result[j] = result[j - 1] + h;
            }

            return result;
        }

		public double timeStep(int N)
		{
			return HT/N;
		}

}
