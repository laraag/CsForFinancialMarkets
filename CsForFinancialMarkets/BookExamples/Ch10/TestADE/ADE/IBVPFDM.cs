// IBVPFDM.cs
//
// 31 January 2006 Robbert van der Plas
// 2008-12-4 DD code review + optomisation etc.
// 2008-12-6 DD optimise code a bit
// 2011-2-2 C# version
//
// IBVPFDM class. 
// 
//
// (C) Datasim Education BV 2006-2013

using System.Collections.Generic;
using System;

public abstract class IBVPFDM
{

    protected Range<double> xaxis;
    protected Range<double> taxis;

    // Mesh arrays
    protected Vector<double> xarr;
    protected Vector<double> tarr;
    
    // Solutions at time levels n and n+1
    protected Vector<double> vecOld;
    public Vector<double> vecNew;

    // Results at ALL levels
    private NumericMatrix<double> res;

    // The mesh generator
    private Mesher m;

    // The implementation of the PDE (Bridge)
    protected IBSPde pde;

     // Redundant data (bits and pieces)
    protected int N, J, currentIndex, maxIndex;
    protected double DN, DJ, DJJ, k, h, h2, tprev, tnow;

    public IBVPFDM(Range<double> Xrange, Range<double> Trange)
    {

        xaxis = new Range<double>(Xrange);
        taxis = new Range<double>(Trange);

        pde = null;
        initMesh( 10, 10 );
        initIC();
    }

    public IBVPFDM(IBSPde source, Range<double> Xrange, Range<double> Trange, int JSteps, int NSteps)
    {

        xaxis = new Range<double>(Xrange);
        taxis = new Range<double>(Trange);

        pde = source;
        initMesh( JSteps, NSteps );
        initIC();
    }

    public void initMesh( int JSteps, int NSteps )
    {
        N = NSteps;
        J = JSteps;
        k = taxis.spread / N;
        h = xaxis.spread / J;
        
        h2 = h * h;

        // Other numbers
        DN = N;
        DJ = J;
        DJJ = J * J;

        // Allow range[ A, B ] in x direction and [ t1, T ] in t direction; create mesh arrays
        m = new Mesher( xaxis.low, xaxis.high, taxis.low, taxis.high );
        xarr = m.xarr( J );
        tarr = m.yarr(N);

        tarr[tarr.MinIndex] = 0.0;
        tarr[tarr.MaxIndex] = taxis.high;

        for (int n = tarr.MinIndex + 1; n < tarr.MaxIndex-1; n++)
        {
            tarr[n] = tarr[n - 1] + k;
        }

        // The 3 data structures should be 'compatible' with each other, indices
        vecOld = new Vector<double>(J+1, 1);
        vecNew = new Vector<double>(J+1, 1);
        res = new NumericMatrix<double>( N + 1, J + 1, 1, 1 );
        
    }

    public void initIC()
    { // Utility function to initialise the payoff function

        currentIndex = res.MinRowIndex;
        maxIndex = res.MaxRowIndex;

        tprev = tnow = taxis.low;

        // Initialise at the boundaries
        vecOld[ vecOld.MinIndex ] = pde.BCL( taxis.low );
        vecOld[ vecOld.MaxIndex ] = pde.BCR( taxis.high );

        // Now initialise values in interior of interval using
        // the initial function 'IC' from the PDE
        for( int j = xarr.MinIndex+1; j <= xarr.MaxIndex - 1; j++ )
        {
            vecOld[ j ] = pde.IC( xarr[ j ] );
        }

     
        res.Row( currentIndex, vecOld);
    }

    // Hook function for Template Method pattern 
    public abstract void calculateBC();
    public abstract void calculate();
  
    public NumericMatrix<double> result()
    {
        // Initialise time.
        tnow = tprev = tarr[tarr.MinIndex];
    
        res.setRow(vecNew, tarr.MinIndex);

        // The state machine; we march from t = 0 to t = T.
        for (int n = tarr.MinIndex+1; n <= tarr.MaxIndex; n++)
        {
            tnow = tarr[n];

            // The two methods that represent the variant parts 
            // of the Template Method Pattern.
            calculateBC();  
            calculate();

            // Add the current solution to the matrix of results.
            res.setRow(vecNew, n);
            
            tprev = tnow;
        }

        return res;
    }

    public Vector<double> XValues
    {
        get
        {
            return xarr;
        }
    }

    public Vector<double> TValues
    {
        get
        {
            return tarr;
        }
    }

    public Range<double> Xaxis
    {
        get
        {
            return xaxis;
        }
        set
        {
            xaxis = value;
        }
    }

    public Range<double> Taxis
    {
        get
        {
            return taxis;
        }
        set
        {
            taxis = value;
        }
    }
}