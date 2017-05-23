using System;
using System.Runtime.InteropServices;

namespace XLFormulas
{
    // / <summary>
    // / Excel Automation Add-ins must have the Programmable sub key.
    // / - HKEY_CLASSES_ROOT\CLSID\{"GUID"}\Programmable.
    // / This is not done automatically when registering the COM component so we must do it ourselfs
    // / by creating the ComRegisterFunction & ComUnregisterFunction. 
    // / These functions are called automatically during (un)registration.
    // / 
    // / No need to be COM visible when derived class uses the "ClassInterfaceType.None" option and 
    // / implements COM interfaces.
    // / </summary>
    [ComVisible(false)]				 // No need to be COM visible when derived class uses "ClassInterfaceType.None" option.	
    public class ProgrammableBase
    {
        // / <summary>
        // / Called when the COM component is registered.
        // / Here we create the "HKEY_CLASSES_ROOT\CLSID\{"GUID"}\Programmable" sub key.
        // / </summary>
        // / <param name="t">The type being registered.</param>
        [ComRegisterFunction()]
        public static void RegisterFunction(Type t)
        {
            // Create the "Programmable" sub key.
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(GetSubKeyName(t, "Programmable"));

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(GetSubKeyName(t, "InprocServer32"), true);
            key.SetValue("", System.Environment.SystemDirectory + @"\mscoree.dll", Microsoft.Win32.RegistryValueKind.String);
        }

        // / <summary>
        // / Called when the COM component is unregistered.
        // / Here we delete the "HKEY_CLASSES_ROOT\CLSID\{"GUID"}\Programmable" sub key.
        // / </summary>
        // / <param name="t">The type being registered.</param>
        [ComUnregisterFunction()]
        public static void UnregisterFunction(Type t)
        {
            // Delete the "Programmable" sub key.
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKey(GetSubKeyName(t, "Programmable"));
        }

        // / <summary>
        // / Create the full "path" of the sub key for the type and sub key given.
        // / </summary>
        // / <param name="t">The type to create the sub key path for.</param>
        // / <param name="subKeyName">The sub key name to create the path for.</param>
        // / <returns>The full sub key path.</returns>
        private static string GetSubKeyName(Type t, string subKeyName)
        {
            return String.Format("CLSID\\{{{0}}}\\{1}", t.GUID.ToString().ToUpper(), subKeyName);
        }
    }
}