// ProgrammableBase.cs
//
// (C) Datasim Education BV  2010

using System;
using System.Runtime.InteropServices;

namespace Datasim
{
    /// <summary>
    /// Excel Automation Add-ins must have the Programmable sub key.
    /// - HKEY_CLASSES_ROOT\CLSID\{"GUID"}\Programmable.
    /// This is not done automatically when registering the COM component so we must do it ourselfs
    /// by creating the ComRegisterFunction & ComUnregisterFunction. 
    /// These functions are called automatically during (un)registration.
    /// </summary>
    [ComVisible(true)]                          // Must be COM visible when derived class uses "AutoDual" option.
    [ClassInterface(ClassInterfaceType.None)]   // This class doesn't need an interface.
    public class ProgrammableBase
    {
        /// <summary>
        /// Called when the COM component is registered.
        /// Here we create the "HKEY_CLASSES_ROOT\CLSID\{"GUID"}\Programmable" sub key.
        /// </summary>
        /// <param name="t">The type being registered.</param>
        [ComRegisterFunction()]
        public static void RegisterFunction(Type t)
        {
            // Create the "Programmable" sub key.
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(GetSubKeyName(t, "Programmable"));
        }

        /// <summary>
        /// Called when the COM component is unregistered.
        /// Here we delete the "HKEY_CLASSES_ROOT\CLSID\{"GUID"}\Programmable" sub key.
        /// </summary>
        /// <param name="t">The type being registered.</param>
        [ComUnregisterFunction()]
        public static void UnregisterFunction(Type t)
        {
            // Delete the "Programmable" sub key.
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKey(GetSubKeyName(t, "Programmable"));
        }

        /// <summary>
        /// Create the full "path" of the sub key for the type and sub key given.
        /// </summary>
        /// <param name="t">The type to create the sub key path for.</param>
        /// <param name="subKeyName">The sub key name to create the path for.</param>
        /// <returns>The full sub key path.</returns>
        private static string GetSubKeyName(Type t, string subKeyName)
        {
            return String.Format("CLSID\\{{{0}}}\\{1}", t.GUID.ToString().ToUpper(), subKeyName);
        }
    }
}