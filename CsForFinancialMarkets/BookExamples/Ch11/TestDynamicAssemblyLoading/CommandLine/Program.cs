using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main()
        {    
            Load("PythagorasDistance.dll");
            Load("TaxiDriverDistanceCalculation.dll");
        }

        static void Load(string FileName) 
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "TestDynamicAssemblyLoading.exe";
            start.UseShellExecute = false;
            start.Arguments = FileName;
            Process p = new Process();
            p.StartInfo = start;
            p.Start();
            p.WaitForExit();     
        }
    }
}
