// RtdServerTopic.cs
//
// (C) Datasim Edcuation BV  2010

using System;

namespace Datasim
{
	/// <summary>
	/// Simple RTD Server Topic class that generates number from a sine function.
	/// The topic data is the scale factor to apply to the sine function and the
	/// increment to the sine function's input argument.
	/// </summary>
	public class SineRtdServerTopic: IDisposable
	{
		private double m_scaleFactor;	// The scale factor of the sine function.
		private double m_increment;		// The increment value to the sine input argument.
		private double m_currentValue;	// The current input value.

		/// <summary>
		/// Constructor with topic data.
		/// </summary>
		/// <param name="strings">The topic data.</param>
		public SineRtdServerTopic(ref Array strings)
		{
			// First parameter is the scale factor.
			m_scaleFactor=1.0; 
			if (strings.Length>=1) Double.TryParse(strings.GetValue(0).ToString(), out m_scaleFactor);

			// Second parameter is the increment.
			m_increment=2*Math.PI/360.0;
			if (strings.Length>=2) Double.TryParse(strings.GetValue(1).ToString(), out m_increment);

			// Current value starts at 0.
			m_currentValue=0.0;
		}

		/// <summary>
		/// Return the current data.
		/// </summary>
		/// <returns>The calculated data.</returns>
		public double GetData()
		{
			m_currentValue+=m_increment;
			return Math.Sin(m_currentValue)*m_scaleFactor;
		}

		/// <summary>
		/// Dispose the topic object.
		/// </summary>
		public void Dispose()
		{
			// Currently empty implementation.
			// Might be used to clean up resources.
		}

	}
}