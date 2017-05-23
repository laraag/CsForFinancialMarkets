// Processes.cs
//
// Example program that shows how to start an external process, for
// example Notepad or Excel.
//
// (C) Datasim Education BV  2002-2010

using System;
using System.Diagnostics;
using System.Threading;

public class Processes
{
	public static void Main()
	{

        foreach (Process p in Process.GetProcesses())
        { // Create a process component for each process on the local machine

              Console.WriteLine(p.BasePriority);            // Process base priority 
              Console.WriteLine(p.ProcessName);             // Process name         
              Console.WriteLine(p.Threads.Count);           // Number of threads in process
              Console.WriteLine(p.WorkingSet64/1024.0);     // Physical memory usage
        }

        Process pNotepad;
        Process pExcel;
		
		try
		{
			// Try to start "notepad.exe" and "excel.exe"
			pNotepad = Process.Start("notepad.exe");
            pExcel = Process.Start("excel.exe");
		}
		catch (Exception)
		{
			pNotepad = null;
            pExcel = null;
		}

        Console.WriteLine("# threads in Excel: " + pExcel.Threads.Count);
        pExcel.PriorityClass = ProcessPriorityClass.High; // Assign to an Enumeration
        Console.WriteLine("  base priority: {0}",  pExcel.BasePriority); // Same, read only, prints '8'
        Console.WriteLine("  priority class: {0}",  pExcel.PriorityClass); // Prints 'High'

        Thread.Sleep(5000);

        foreach (ProcessThread th in pExcel.Threads)
        {

            Console.WriteLine("State: " + th.ThreadState);

        }  

		// Wait 5 seconds before closing notepad
		Thread.Sleep(5000);
       
		// Stop notepad
		if (pNotepad != null) pNotepad.CloseMainWindow();
        if (pExcel != null) pExcel.CloseMainWindow();
	}
}