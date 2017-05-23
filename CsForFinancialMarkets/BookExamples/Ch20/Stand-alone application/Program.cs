// Program.cs
//
// Excel stand-alone program example.
//
// (C) Datasim Education BV  2010

using System;
using System.Windows.Forms;
using Excel=Microsoft.Office.Interop.Excel;

namespace Datasim
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create an instance of Excel and show it.
			Excel.Application xlApp=new Excel.ApplicationClass();
			xlApp.Visible=true;

			// Create new Excel Workbook with one worksheet.
			xlApp.SheetsInNewWorkbook=1;
			Excel.Workbook wb=xlApp.Workbooks.Add(Type.Missing);
			
			// Set worksheet object to first worksheet in workbook.
			Excel.Worksheet ws=wb.Worksheets[1] as Excel.Worksheet;

			// Change the name of the worksheet.
			ws.Name="MyWorksheet";
    
			// Fill the worksheet.
			int i;
			for (i=1; i<10; i++) 
			{
				(ws.Cells[i, 1] as Excel.Range).Value2=String.Format("Row {0}", i);
				(ws.Cells[i, 2] as Excel.Range).Value2=i;
			}
    
			// Fill total row.
			(ws.Cells[i, 1] as Excel.Range).Value2="Total";
			(ws.Cells[i, 2] as Excel.Range).Formula=String.Format("=Sum(B1:B{0})", i-1);

			// Make first column bold and draw line before total row.
			ws.get_Range(String.Format("A1:A{0}", i), Type.Missing).Font.Bold=true;
			ws.get_Range(String.Format("A{0}:B{1}", i-1, i-1), Type.Missing).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

			// Create chart on separate worksheet and format the chart using the chart wizard.
			Excel.Chart ch=wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing) as Excel.Chart;
            ch.ChartWizard(ws.get_Range(String.Format("A1:B{0}", i), Type.Missing), Excel.XlChartType.xlCylinderCol, 1,
                           Excel.XlRowCol.xlColumns, 1, 0, true, "Chart with values of each row",
                           "Rows", "Value", "Extra Title");

            // Create chart on same worksheet and format the chart using the chart wizard.
            ch = (ws.ChartObjects(Type.Missing) as Excel.ChartObjects).Add(150, 0, 400, 400).Chart;
            ch.ChartWizard(ws.get_Range(String.Format("A1:B{0}", i), Type.Missing), Excel.XlChartType.xl3DColumn, 1,
                           Excel.XlRowCol.xlColumns, 1, 0, true, "Chart with values of each row",
                           "Rows", "Value", "Extra Title");

			// Save workbook.
			try
			{
				wb.SaveAs("MyWorkbook.xls", Type.Missing, Type.Missing, Type.Missing,
						  Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing,
						  Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			}
			catch (System.Runtime.InteropServices.COMException)
			{
				// user cancelled save.
			}

			// Ask to print workbook.
		/*	if (MessageBox.Show("Print workbooks?", "Question", MessageBoxButtons.YesNo)==DialogResult.Yes)
			{
				wb.PrintOut(Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			}*/

			// Close the workbook.
			wb.Close(Type.Missing, Type.Missing, Type.Missing);

			// Close Excel.
			xlApp.Quit();
		}
	}
}
