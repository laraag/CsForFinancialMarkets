// FDMDirector.cpp
//
// The class that drives the whole FDM process. It
// mediates between the Mesh generation class and the 
// FDM class.
//
// (C) Datasim Education BV 2005-2011
//

using System;

class FDMDirector
{
	private FDMDirector ()
	{
	}

  	private double tprev, tnow;
	private Vector<double> xarr; // x mesh
    private Vector<double> tarr; // t mesh
	private FDM fdm;
    
    public FDMDirector (FDM fdScheme, Vector<double> xmesh, Vector<double> tmesh)
	{
        fdm = fdScheme;
        xarr = xmesh;
        tarr = tmesh;
  	}
	
	public Vector<double> current()
	{
		return fdm.current();
	}

	public void Start() // Calculate next level
	{
		// Update new mesh array in FDM scheme
		fdm.initIC(xarr);
        doit();
	}

   
	public void doit()
	{
        tnow = tprev = tarr[tarr.MinIndex];

        for (int n = tarr.MinIndex + 1; n <= tarr.MaxIndex; n++)
        {
            tnow = tarr[n];
            fdm.calculateCoefficients(xarr, tprev, tnow);
            fdm.solve(tnow);
            tprev = tnow;
        }
	}

}