using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


class Ch23
{
    public static void Main()
    {
        // Uncomment one of the following line to open the excel file or directly click on the 
        // file attached in the project

        OpenSineRtdServer();      
    }

    public static void OpenSineRtdServer()
    {
        OpenFile("SineRtdServer.xlsx", @"Ch23\SineRtdServer\");
    }    

    public static void OpenFile(string fileName, string lastPart)
    {
        string s1 = Directory.GetCurrentDirectory();
        string pattern = "^.*(?=" + Regex.Escape("CsForFinancialMarkets") + ")";
        string root = Regex.Match(s1, pattern).Value + @"CsForFinancialMarkets\BookExamples\" + lastPart;
        string toOpen = (from f in Directory.GetFiles(root, fileName, SearchOption.AllDirectories)
                         select f).First().ToString();
        System.Diagnostics.Process.Start(toOpen);
    }
}
