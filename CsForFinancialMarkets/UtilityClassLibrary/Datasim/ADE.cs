// ADE_.cs
//
// Solvers for initial boundary value problems. We 
// use an abstract base class; derived class implement
// specific behaviour.
//
// 2005-9-36 DD kick-off
// 2005-10-1 DD next rev.
// 2005-10-11 DD last revision?
// 2005-11-30 DD new cpp file
// 2006-1-10 fixing + aligning indices
// 2008-10-7 DD fitting factor
// 2008-12-15 DD ported to C#
// 2010-5-10 DD ADE model
// 2011-2-2 DD minimal version (no inheritance)
// 2011-3-1 DD testing
// 2011-7-26 DD Leung/Osher update strategy
// 2011-7-27 Use tarr for time meshing; OK already done!
// 2011-7-27 American options
//
// (C) Datasim Component Technology BV 2003-2011
//
using System;

public class ADE : IBVPFDM
{

    // We must build these arrays in constructors

    // Arrays of coefficient values of the PDE.
    private Vector<double> alpha;   // diffusion
    private Vector<double> beta;    // convection
    private Vector<double> gamma;   // zero-order terms
    private Vector<double> rhs;     // inhomogeneous terms

    // Intermediate values of up sweep U and down sweep V 
    // at time levels n+1 and n.
    private Vector<double> U;
    private Vector<double> V;
    private Vector<double> UOld;
    private Vector<double> VOld;

    public ADE(IIBVPImp myPDE, Range<double> Xrange, Range<double> Trange, int JSteps, int NSteps)
        : base(myPDE, Xrange, Trange, JSteps, NSteps)
    {

        // Two sweeps U and V will get the initial conditions
        U = new Vector<double>(vecOld);
        V = new Vector<double>(vecOld);

        UOld = new Vector<double>(U);
        VOld = new Vector<double>(V);

        alpha = new Vector<double>(U);
        beta = new Vector<double>(U);
        gamma = new Vector<double>(U);
        rhs = new Vector<double>(U);
    }

    public override void calculateBC()
    { // Tells how to calculate sol. at n+1

        U[U.MinIndex] = pde.BCL(tnow);
        U[U.MaxIndex] = pde.BCR(tnow);

        V[V.MinIndex] = pde.BCL(tnow);
        V[V.MaxIndex] = pde.BCR(tnow);
    }

    public override void calculate()
    { // Tells how to calculate sol. at n+1

        // Update sweeps at level n
        for (int j = U.MinIndex; j <= U.MaxIndex; j++)
        {
            UOld[j] = vecOld[j];
            VOld[j] = vecOld[j];
        }


        for (int j = U.MinIndex; j <= U.MaxIndex; j++)
        { // Coefficients calculated in parallel

            alpha[j] = k * pde.diffusion(xarr[j], tnow) / h2;
            beta[j] = (0.5 * k * (pde.convection(xarr[j], tnow))) / h;
            gamma[j] = (1.0 + alpha[j] - pde.zeroterm(xarr[j], tnow) * k);

            rhs[j] = k * pde.RHS(xarr[j], tnow);
        }

        // Upward sweep
        for (int j = U.MinIndex + 1; j <= U.MaxIndex - 1; j++)
        {
            U[j] = (1.0 - alpha[j]) * UOld[j] + (alpha[j] - beta[j]) * U[j - 1]
                                + (alpha[j] + beta[j]) * UOld[j + 1] + rhs[j];
            U[j] /= gamma[j]; 


            // American option for U
      /*      tmp = pde.Constraint(xarr[j]);
            if (U[j] < tmp)
            {
                U[j] = tmp;
            }*/
         }

        // Downward sweep
        for (int j = V.MaxIndex - 1; j >= V.MinIndex + 1; j--)
        {
            V[j] = (1.0 - alpha[j]) * VOld[j] + (alpha[j] - beta[j]) * VOld[j - 1]
                           + (alpha[j] + beta[j]) * V[j + 1] + rhs[j];
          
            V[j] /= gamma[j];

            // American option for V
        /*    tmp = pde.Constraint(xarr[j]);
            if (V[j] < tmp)
            {
                V[j] = tmp;
            }*/
        }

        for (int j = vecNew.MinIndex; j <= vecNew.MaxIndex; j++)
        { // Combine in previous loop

            vecNew[j] = 0.5 * (U[j] + V[j]);
    /*        tmp = pde.Constraint(xarr[j]);
            if (vecNew[j] < tmp)
            {
                vecNew[j] = tmp;
            }*/
            // Update strategies

        //    vecOld[j] = vecNew[j];
        }
        /*for (int j = U.MinIndex; j <= U.MaxIndex; j++)
        {
            UOld[j] = U[j];
            VOld[j] = V[j];
        }*/

    }


}