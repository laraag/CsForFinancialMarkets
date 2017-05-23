public struct SDE
{ // Defines drift + diffusion + data

	public OptionData data;				// The data for the option MC

	public double drift(double t, double X)
	{ // Drift term
	
		return (data.r)*X; // r - D
	}

	
	public double diffusion(double t, double X)
	{ // Diffusion term
	
		return data.sig * X;
		
	}

	public double diffusionDerivative(double t, double X)
	{ // Diffusion term, needed for the Milstein method
	
		return 0.5 * (data.sig) * X;
	}
} 


