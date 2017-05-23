// ExplicitEulerIBVP.cs
//
// 31 January 2006 Robbert van der Plas
// 2008-12-6 DD modifications
// 2012-11-15 DD clean-up

// (C) Datasim Education BV 2006-2013
//

public class ExplicitEulerIBVP : IBVPFDM
{
    private double tmp1, tmp2;			// Work variables
    private double A, B, C;	            // Coefficients in FD scheme


    public ExplicitEulerIBVP(IIBVPImp source, Range<double> Xrange, Range<double> Trange, int NSteps, int JSteps)
        : base(source, Xrange, Trange, NSteps, JSteps)
    {
    }

    // Hook function for Template Method pattern 
    override public void calculateBC()
    {
        vecNew[ vecNew.MinIndex ] = pde.BCL( tprev );
        vecNew[ vecNew.MaxIndex ] = pde.BCR( tprev );
    }


     
    override public void calculate()
    {
        // Explicit Euler schemes		
        for( int i = vecNew.MinIndex + 1; i <= vecNew.MaxIndex - 1; i++ )
        {
  
            tmp1 = k * ( pde.diffusion( xarr[ i ], tprev ) /(h*h));
            tmp2 = ( k * 0.5 * ( pde.convection( xarr[ i ], tprev ) ) / h);

       
            A = tmp1 - tmp2;
            B = 1.0 -  2.0 * tmp1;
            C = tmp1 + tmp2;

            vecNew[i] = (A * vecOld[i - 1])
                                + (B * vecOld[i])
                                + (C * vecOld[i + 1]);

            vecNew[i] = vecNew[i] / (1.0 - k * pde.zeroterm(xarr[i], tprev)); 
       
        }

    }

}