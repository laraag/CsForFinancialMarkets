// Processes101.cs
//
// Example program that shows how to start an external process, for
// example Notepad or Excel.
//
// (C) Datasim Education BV  2002-2013

using System;
using System.Diagnostics;
using System.Threading;

public class Processes
{
	public static void Main()
	{

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

      	// Wait 10 seconds before closing notepad
		Thread.Sleep(10000);
       
		// Stop notepad
		if (pNotepad != null) pNotepad.CloseMainWindow();
        if (pExcel != null) pExcel.CloseMainWindow();
	}
}