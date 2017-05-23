// Calculator.cs
//
// Version 1.
//
// (C) Datasim Education BV  2010

using System;
using System.Runtime.InteropServices;

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
	/// We use the ClassInterface.AutoDual option. This will generate a dual COM interface for this class which can be used for both
	/// late binding and early binding. Excel Automation add-in needs a dual interface.
	/// But this has problems. First we insert a function, the dispatch IDs of the functions will change, breaking compiled
	/// early binding clients. Old clients will then call the inserted function instead of the original function at that position.
	/// Also, the object's member functions like ToString() will be exposed which is not what we want.
	/// Version 2 will solve this by using ClassInterfaceType.None and implementing COM interfaces.
    /// </summary>
    [ComVisible(true)]                                  // Makes the class visible in COM regardless of the assembly COM visible attribute.
	[ProgId("DatasimAddIns.CalculatorV1")]              // Explicit ProgID. 
	[Guid("20A394D6-E63F-422a-8308-4776D5602A66")]      // Explicit GUID. 
    [ClassInterface(ClassInterfaceType.AutoDual)]       // Automation add-ins need a dual interface.
    public class Calculator: ProgrammableBase			// The ProgrammableBase class must be COM visible too with the AutoDual option.
	{
        /// <summary>
        /// Add two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the addition.</returns>
		public double MyAdd(double v1, double v2)
		{
			return v1+v2;
		}

        /// <summary>
        /// Subtract two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the subtraction.</returns>
        public double MySubtract(double v1, double v2)
        {
            return v1-v2;
        }

        /// <summary>
        /// Multiply two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the multiplication.</returns>
        public double MyMultiply(double v1, double v2)
        {
            return v1*v2;
        }

        /// <summary>
        /// Divide two numbers.
        /// </summary>
        /// <param name="v1">The first number.</param>
        /// <param name="v2">The second number.</param>
        /// <returns>The result of the division.</returns>
        public double MyDivide(double v1, double v2)
        {
            return v1/v2;
        }

    }
}
