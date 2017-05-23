// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Ch12_Examples.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Ch12
{
    public static void Main()
    {
        // Uncomment one of the following line to open the excel file or directly click on the 
        // file attached in the project

        // BondBasicxls();          // Open BondBasic.xls
        // BondLoadInMemoryxls();   // Open BondLoadInMemory.xls
        // BondSerializationxls();  // Open BondSerialization.xls 
    }

    public static void BondBasicxls()
    {
        OpenFile("BondBasic.xls");
    }

    public static void BondLoadInMemoryxls()
    {
        OpenFile("BondLoadInMemory.xls");
    }

    public static void BondSerializationxls()
    {
        OpenFile("BondSerialization.xls");
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