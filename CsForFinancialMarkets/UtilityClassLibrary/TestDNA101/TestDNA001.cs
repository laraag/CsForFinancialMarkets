using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExcelDna.Integration;

namespace MyDNA
{
    public class Class1
    { // 101 test case

        [ExcelFunction(Description = "Hello World")]
        public static string Doit2(string s)
        {
            return s;
        }
        
        [ExcelFunction(Description = "Do some calcs")]
        public static double MyDisplay(double d)
        {
            dynamic xlApp = ExcelDnaUtil.Application;

            dynamic range = xlApp.Range["A1:A1"];

            return d * Math.Exp((double) range.Value2);
        }
    }
}
