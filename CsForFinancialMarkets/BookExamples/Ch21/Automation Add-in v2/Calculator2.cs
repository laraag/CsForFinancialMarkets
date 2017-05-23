// Calculator2.cs
//
// Version 2.
//
// (C) Datasim Education BV  2010

using System;
using System.Runtime.InteropServices;

using Excel=Microsoft.Office.Interop.Excel;

namespace Datasim
{

    /// <summary>
    /// Calculator automation add-in.
	/// Derived from ProgrammableBase to add the "Programmable" sub key to the registry.
	/// 
	/// We explicitly set the ProgID and GUID. When not given they will be automatically generated.
	/// The automatically generated ProgID will be "Namespace name"."Class name".
	/// The automatically generated GUID will be derived from the assembly name, namespace name, class name and assembly version number.
	/// This gives versioning problems when the version changes, so we specify it explicitly.
	/// 
	/// This time we use the ClassInterface.None option so no COM interface for this class will be generated. 
	/// Instead we implement a COM interface. The ProgrammableBase class does not have to be COM visible now.
	/// This has two advantages, first only the functions in the interface are exposed and not the functions from object.
	/// Secondly, we don't have versioning problems with early binding clients as long as the interface doesn't change.
	/// Any changes must be done in a new interface.
    /// </summary>
    [ComVisible(true)]                                  // Makes the class visible in COM regardless of the assembly COM visible attribute.
    [ProgId("DatasimAddIns.CalculatorV2")]              // Explicit ProgID. 
    [Guid("D93720F6-FCE8-4acb-A87F-88BC2B94FB2D")]      // Explicit GUID. 
    [ClassInterface(ClassInterfaceType.None)]           // We implement COM interfaces.
    public class Calculator2: ProgrammableBase, ICalculator, Extensibility.IDTExtensibility2
    {
		// The Excel application. Used in volatile worksheet functions.
		private Excel.Application m_xlApp=null;

		// The random number generator.
		private Random m_rnd=new Random();

        /// <summary>
        /// Add two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the addition.</returns>
        double ICalculator.MyAdd(double v1, double v2)
        {
            return v1+v2;
        }

        /// <summary>
        /// Subtract two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the subtraction.</returns>
        double ICalculator.MySubtract(double v1, double v2)
        {
            return v1-v2;
        }

        /// <summary>
        /// Multiply two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the multiplication.</returns>
        double ICalculator.MyMultiply(double v1, double v2)
        {
            return v1*v2;
        }

        /// <summary>
        /// Divide two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the division.</returns>
        double ICalculator.MyDivide(double v1, double v2)
        {
            return v1/v2;
        }

		/// <summary>
		/// Calculate the sum of a range.
		/// </summary>
		/// <param name="range">The input range.</param>
		/// <returns>The result of the summation.</returns>
		double ICalculator.MySum(Excel.Range range)
		{
			// Get the number of rows and columns in the range.
			int columns=range.Columns.Count;
			int rows=range.Rows.Count;

			// Temporary result.
			double tmp=0.0;

			// Iterate the rows and columns.
			for (int r=1; r<=rows; r++)
			{
				for (int c=1; c<=columns; c++)
				{
					// Get the value of the current cell as double and add to running result.
					tmp+=(double)(range[r, c] as Excel.Range).Value2;
				}
			}

			// Return the result.
			return tmp;
		}

		/// <summary>
		/// Return a random number between 0 and 1.
		/// </summary>
		/// <returns>The random number.</returns>
		double ICalculator.MyRandom()
		{
			// Notify Excel this is a volatile function.
			if (m_xlApp!=null) m_xlApp.Volatile(true);

			// Return random number.
			return m_rnd.NextDouble();
		}

		/// <summary>
		/// Return a random number between 0 and max.
		/// When "max" is not given, the default value is 1.0 (specified in the interface ICalculator).
		/// </summary>
		/// <param name="max">The maximum value.</param>
		/// <returns>The random number.</returns>
		double ICalculator.MyRandomMax(double max)
		{
			// Notify Excel this is a volatile function.
			if (m_xlApp!=null) m_xlApp.Volatile(true);

			// Return random number between 0 and max.
			return m_rnd.NextDouble()*max;
		}

		#region IDTExtensibility2 Members

		/// <summary>
		/// Implements the OnConnection method of the IDTExtensibility2 interface.
		/// Receives notification that the Add-in is being loaded.
		/// </summary>
		/// <param name="application">Root object of the host application.</param>
		/// <param name="connectMode">Describes how the Add-in is being loaded.</param>
		/// <param name="addInInst">Object representing this Add-in.</param>
		/// <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			// Store reference to the Excel host application (m_xlApp will become null if not loaded by Excel).
			m_xlApp=application as Excel.Application;
		}

		/// <summary>
		/// Implements the OnDisconnection method of the IDTExtensibility2 interface.
		/// Receives notification that the Add-in is being unloaded.
		/// </summary>
		/// <param name="removeMode">Describes how the Add-in is being unloaded.</param>
		/// <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnDisconnection(Extensibility.ext_DisconnectMode removeMode, ref Array custom)
		{
			// Empty implementation.
		}
		/// <summary>
		/// Implements the OnAddInsUpdate method of the IDTExtensibility2 interface.
		/// Receives notification that the collection of Add-ins has changed.
		/// </summary>
		/// <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnAddInsUpdate(ref Array custom)
		{
			// Empty implementation.
		}

		/// <summary>
		/// Implements the OnStartupComplete method of the IDTExtensibility2 interface.
		/// Receives notification that the host application has completed loading.
		/// </summary>
		/// <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnStartupComplete(ref Array custom)
		{
			// Empty implementation.
		}
		
		/// <summary>
		/// Implements the OnBeginShutdown method of the IDTExtensibility2 interface.
		/// Receives notification that the host application is being unloaded.
		/// </summary>
		/// <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnBeginShutdown(ref Array custom)
		{
			// Empty implementation.
		}

		#endregion
	}


}
