// ICalculator.cs
//
// (C) Datasim Education BV  2010

using System;
using System.Runtime.InteropServices;

using Excel=Microsoft.Office.Interop.Excel;

namespace Datasim
{

    /// <summary>
    /// ICalculator interface.
	/// By default a dual interface is created which can be used for both early binding and late binding.
    /// </summary>
    [ComVisible(true)]                                  // Makes the interface visible in COM regardless of the assembly COM visible attribute.
    [Guid("BDDEB409-1758-496e-9F23-BF4A34965AA7")]      // Explicit GUID for the interface. 
    public interface ICalculator
    {
        /// <summary>
        /// Add two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the addition.</returns>
        double MyAdd(double v1, double v2);

        /// <summary>
        /// Subtract two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the subtraction.</returns>
        double MySubtract(double v1, double v2);

        /// <summary>
        /// Multiply two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the multiplication.</returns>
        double MyMultiply(double v1, double v2);

        /// <summary>
        /// Divide two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the division.</returns>
        double MyDivide(double v1, double v2);

		/// <summary>
		/// Calculate the sum of a range.
		/// </summary>
		/// <param name="range">The input range.</param>
		/// <returns>The result of the summation.</returns>
		double MySum(Excel.Range range);

		/// <summary>
		/// Return a random number between 0 and1.
		/// </summary>
		/// <returns>The random number.</returns>
		double MyRandom();
	
		/// <summary>
		/// Return a random number between 0 and max.
		/// When "max" is not given, the default value is 1.0
		/// </summary>
		/// <param name="max">The maximum value.</param>
		/// <returns>The random number.</returns>
		double MyRandomMax([Optional(), DefaultParameterValue(1.0)] double max);
	}

}
