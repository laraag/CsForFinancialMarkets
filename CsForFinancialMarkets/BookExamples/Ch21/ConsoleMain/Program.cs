using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsInstaller;

// add reference to COM object "Microsoft Windows Installer Object Library"
class Ch21 
{
    public static void Main()
    {
        // Uncomment one of the following line to open the excel file or directly click on the 
        // file attached in the project

        // OpenAutomationAddInsV1();
        // OpenAutomationAddInsV2();
        // InstallCOMAddIns();
    }

    public static void OpenAutomationAddInsV1()
    {
        OpenFile("Calculator.xlsm", @"Ch21\Automation Add-in v1\");
    }

    public static void OpenAutomationAddInsV2()
    {
        OpenFile("Calculator.xlsm", @"Ch21\Automation Add-in v2\");
    }

    public static void InstallCOMAddIns() 
    {
        OpenFile("COMAddInSetup.msi", @"\Ch21\COM Add-in\COMAddInSetup\Debug\");
    }
    
    public static void OpenFile(string fileName, string lastPart)
    {
        string s1 = Directory.GetCurrentDirectory();
        string pattern = "^.*(?=" + Regex.Escape("CsForFinancialMarkets") + ")";
        string root = Regex.Match(s1, pattern).Value + @"CsForFinancialMarkets\BookExamples\"+lastPart;
        string toOpen = (from f in Directory.GetFiles(root, fileName, SearchOption.AllDirectories)
                         select f).First().ToString();
        System.Diagnostics.Process.Start(toOpen);
    }
}
