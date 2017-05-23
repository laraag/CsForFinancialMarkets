using System;

// Data class used to do long calculations.
class Data
{
	private double m_value;
	private int m_iterations;
	private double m_result;

	// Constructor.
	public Data(double value, int iterations)
	{
		m_value=value;
		m_iterations=iterations;
	}

	// Access the result.
	public double Result { get { return m_result; } }

	// Process the data.
	public void Process()
	{
		m_result=m_value;
		for (int i=0; i!=m_iterations; i++)
		{
			m_result+=Math.Sqrt(m_result);
		}
	}
}