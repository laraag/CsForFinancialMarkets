// ----------------------------------------------------------------------
// DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author for the use of this code in any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Appendix4.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

class Appendix4
{
    public static void Main() 
    {
        CapFloorSwaptionxls();  // Open the file  CapFloorSwaption.xls
    }

    public static void CapFloorSwaptionxls()
    {
        OpenFile("CapFloorSwaption.xls");
    }

    public static void OpenFile(string fileName)
    {
        string s1 = Directory.GetCurrentDirectory();
        string pattern = "^.*(?=" + Regex.Escape("CsForFinancialMarkets") + ")";
        string root = Regex.Match(s1, pattern).Value + @"CsForFinancialMarkets\BookExamples\";
        string toOpen = (from f in Directory.GetFiles(root, fileName, SearchOption.AllDirectories)
                         select f).First().ToString();
        System.Diagnostics.Process.Start(toOpen);
    }
}