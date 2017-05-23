 // HymanHermiteInterpolator_V5.cs
 // 
 // Interpolator using piecewise monotone rational
 // cubic polynomial.
 // 
 // 2011-2-14 DD initial code.
 // 2011-2-23 DD code copied/modified.
 // 2011-2-27 DD Hyman versions, simple formula
 // 2011-2-27 DD Hyman versions, formulae 2.1, 2.2 and correction 2.3. A few BUGS solved.
 // SOME BUG FIXES &$#(## *****
 // 2011-2-28 DD V5 up to Hyman 2.6
 // 
 // 2011-3-7 Implement Hyman V5 method 2.6 and Arithmetic Mean Method of Meng Tian equation (2)
 // to approximate the (unknown) derivative of the function.
 // 
 //  N.B. For input X and Y arrays, indexing starts at start index = 0.
 // 
 //  2011-3-15 DD Hyman 89 implementation.
 // 
 // (C) Datasim Education BV 2011
 // 
 // 

using System;

public class HymanHermiteInterpolator_V5 : IInterpolate
{
     // Vectors of size n
    Vector<double> xarr;         // x values
    Vector<double> yarr;         // y values

     // Redundant arrays of size n-1
    Vector<double> h;            // h[i] = x[i+1] - x[i]
    Vector<double> delta;        // S[i] = (f[i+1] - f[i]) / h[i]

    Vector<double> d;            // Approximation to f' at mesh points

    int n;                       // Number of data points

    public int findAbscissa(double xvar)
    {  // Will give index of LHS value <= x. Very simple algorithm
       // Value in range [1,n-1]!!!

        for (int j = 1; j < n; j++)
        {
            if (xarr[j] <= xvar && xvar <= xarr[j + 1])
            {
              return j;
            }
          
        }
         // Then x is in the interval [j, j+1].

        return 999;
    }

    public void init()
    {
         // The local mesh spacing
        h = new Vector<double>(n - 1, 1);
        for (int i = 1; i <= n-1; ++i)
        {
            h[i] = xarr[i + 1] - xarr[i];  // Hyman's Delta x[i + 1/2]
        }
       

         // Slope of the linear interpolant between the data points.
         // N.B. Hyman page 647, MUST BE defined at end points
        delta = new Vector<double>(n+1, 0); 
        for (int i = 1; i <= n-1; ++i)
        {
            delta[i] = (yarr[i + 1] - yarr[i]) / h[i];  // Hyman's S[i + 1/2]
        }

         // Endpoint values Hyman page 647, paragraph 4 text
        delta[0] = delta[1];
        delta[n] = delta[n - 1];
              
         // I use the Meng Tian eq. (2) to calculate/approximate derivatives.
        d = new Vector<double>(n, 1);
        for (int j = 2; j <= n-1; ++j)
        {
          //   d[i] = 0.0;
          //   if (delta[i - 1] != 0.0 && delta[i] != 0.0)
            {
                // d[i] = (h[i] * delta[i - 1] + h[i - 1] * delta[i]) / (h[i] + h[i - 1]);
            }

            if (delta[j - 1] * delta[j] < 0.0)
            {
                d[j] = 0.0;
            }
            else
            {
                d[j] = 2.0 / (1.0 / delta[j - 1] + 1.0 / delta[j]);
            }

        }

         // Calculating d[1] and d[n].
        double d1_Star = delta[1] + (delta[1] - delta[2]) * h[1]
                                / (h[1] + h[2]);
        d[1] = d1_Star;
        d[1] = ((2.0*h[1] + h[2])*delta[1] - h[1]*delta[2]) / (h[1] + h[2]);
        d[1] = (3.0 * delta[0] - d[2]) / 2.0;
        // d[1] = 0;
        double dn_Star = delta[n-1] + (delta[n-1] - delta[n - 2]) * h[n-1]
                                / (h[n-1] + h[n-2]);

        d[n] = dn_Star;
        d[n] = ((2.0 * h[n-1] + h[n-2])*delta[n-1] - h[n-1]*delta[n-2]) / (h[n-1] + h[n-2]);
        d[n] = (3.0 * delta[n - 1] - d[n - 1]) / 2.0;

        // d[n] = 0;

        ModifyDerivatives();

                 
	}

    public HymanHermiteInterpolator_V5(Vector<double> abscissa, Vector<double> RHS)
    {

        n = abscissa.Length;

         // BUG FIX
        int startIndex = 1;
        xarr = new Vector<double>(n, startIndex);
        yarr = new Vector<double>(n, startIndex);

        for (int i = 1; i <= n; ++i)
        {
            xarr[i] = abscissa[i-1];
        }

        for (int i = 1; i <= n; ++i)
        {
            yarr[i] = RHS[i - 1];
        }

        init();
    }


    public HymanHermiteInterpolator_V5(double[] abscissa, double[] RHS)
    {

        n = abscissa.Length;

        int startIndex = 1;
        xarr = new Vector<double>(n, startIndex);
        yarr = new Vector<double>(n, startIndex);

        for (int i = 1; i <= n; ++i)
        {
            xarr[i] = abscissa[i - 1];
        }

        for (int i = 1; i <= n; ++i)
        {
            yarr[i] = RHS[i - 1];
        }

        init();
    }

    bool SameSign(double a, double b, double c, double d)
    {
          if (a * b < 0.0)
            return false;

          if (a * c < 0.0)
            return false;

          if (a * d < 0.0)
            return false;

          if (b * c < 0.0)
            return false;

          if (b * d < 0.0)
            return false;

          if (c * d < 0.0)
            return false;


          return true;
    }



    void ModifyDerivatives()
    {  // Hyman 89.

        double pm, pu, pd;
        double M, sign;
        double correction;

        for (int j = 1; j <= n; ++j)
        {
            if (j == 1)
            {
                sign = Math.Sign(d[j]);
                if (d[1] * delta[1] > 0.0)
                {
                    correction = sign * Math.Min(Math.Abs(d[j]), 3.0 * Math.Abs(delta[1]));
                }
                else
                {
                    correction = 0.0;
                }

                if (correction != d[j])
                {
                    d[j] = correction;
                }
            }
            else if (j == n)
            {
                sign = Math.Sign(d[n]);
                if (d[n] * delta[n - 1] > 0.0)
                {
                    correction = sign * Math.Min(Math.Abs(d[n]), 3.0 * Math.Abs(delta[n - 1]));
                }
                else
                {
                    correction = 0.0;
                }

                if (correction != d[j])
                {
                    d[j] = correction;
                }
            }
            else
            {
                pm = (delta[j - 1] * h[j] + delta[j] * h[j - 1]) / (h[j] + h[j - 1]);
                M = 3.0 * Math.Min(Math.Min(Math.Abs(delta[j - 1]), Math.Abs(delta[j])), Math.Abs(pm));

                 // Console.WriteLine(j);
                if (j > 2)
                {
                    if ((delta[j - 1] - delta[j - 2]) * (delta[j] - delta[j - 1]) > 0.0)
                    {
                        pd = (delta[j - 1] * (2.0 * h[j - 1] + h[j - 2]) - delta[j - 2] * h[j - 1]) / (h[j - 2] + h[j - 1]);
                        if (pm * pd > 0.0 && pm * (delta[j - 1] - delta[j - 2]) > 0.0)
                        {
                            M = Math.Max(M, 1.5 * Math.Min(Math.Abs(pm), Math.Abs(pd)));
                        }
                    }

                    if (j < n - 1)
                    {
                        if ((delta[j] - delta[j - 1]) * (delta[j + 1] - delta[j]) > 0.0)
                        {
                            pu = (delta[j] * (2.0 * h[j] + h[j + 1]) - delta[j + 1] * h[j]) / (h[j] + h[j + 1]);
                            if (pm * pu > 0.0 && -pm * (delta[j] - delta[j - 1]) > 0.0)
                            {
                                M = Math.Max(M, 1.5 * Math.Min(Math.Abs(pm), Math.Abs(pu)));
                            }
                        }
                    }

                    sign = Math.Sign(d[j]);
                    if (d[j] * pm > 0.0)
                    {
                        correction = sign * Math.Min(Math.Abs(d[j]), M);
                    }
                    else
                    {
                        correction = 0.0;
                    }

                    if (correction != d[j])
                    {
                        d[j] = correction;
                    }
                }
            }

        }
    }

    public double Solve(double x)
    {
         // Equation (2.1) of Hyman

         // Find which interval [x[i], x[i+1]] is in. 1 <= i <= n-1
        int i = findAbscissa(x);
         // Console.WriteLine("Index {0}", i);
       
       
        double del = (x - xarr[i]);
        double del2 = del * del;
        double del3 = del * del * del;
        
         // (2.1) Hyman
        double c0 = yarr[i]; double c1 = d[i]; 
        double c2 = (3.0 * delta[i] - d[i+1] - 2.0 * d[i]) / h[i];
        double c3 = (d[i + 1] + d[i] -2.0 * delta[i]) / (h[i] * h[i]); 

        return c0 + c1*del + c2*del2 + c3*del3;
      
    }


    public Vector<double> Curve(Vector<double> xarr)
    {  // Create the interpolated curve

        Vector<double> result = new Vector<double>(xarr.Size, xarr.MinIndex);

        for (int j = xarr.MinIndex; j <= xarr.MaxIndex; j++)
        {
            result[j] = Solve(xarr[j]);
        }

        return result;
    }

     // AG 2apr11
    public double[] Curve(double[] xarr)
    {  // Create the interpolated curve
        return null;  //Formula.FromVect(Curve(new Vector<double>(xarr, 0)));

    }

     // inutile
    public Vector<double> Curve()
    {  // Create the interpolated curve, MEMBER DATA AS ABSCISSAE

        return new Vector<double>();
    }

}
