// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Ch22_Examples.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Ch22 
{
    public static void Main()
    {
        // Uncomment one of the following line to open the excel file or directly click on the 
        // file attached in the project

        // RateCurveDNAxls();           // Open RateCurveDNA.xls
        // MultiCurveCOMxls();          // Open MultiCurveCOM.xls     
    }

    public static void RateCurveDNAxls()
    {
          OpenFile("RateCurveDNA.xls");
    }

    public static void MultiCurveCOMxls()
    {
        OpenFile("MultiCurveCOM.xls");
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