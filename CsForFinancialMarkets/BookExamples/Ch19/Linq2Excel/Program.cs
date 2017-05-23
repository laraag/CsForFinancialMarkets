// ExcelToLinq test.
//
// Download library from: http://code.google.com/p/linqtoexcel
// Add references to:
// - LinqToExcel.dll
// - Remotion.Data.Linq.dll
//
// (C) Datasim Education BV  2011


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Program
{
    private static string m_xlFile ="LinqToExcel.xlsx";
    
    static void Main(string[] args)
	{       
		DefaultExample();
		ExplicitSheetName("Countries");
		ColumnRemapping("Countries");
		AccessThroughRow("Countries");
		AccessRange("Countries", "A1", "D4");
		Noheader("CountriesNoHeader");
		Transformations("Countries");

		QuerySheets();
		QueryColumns("Countries");
	}

	/// <summary>
	/// Default example.
	/// </summary>
	public static void DefaultExample()
	{       
		Console.WriteLine("\n*** Default example. Default sheet: 'Sheet1' + header row ***");
        
		// Open XL document.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);
		
		// If you set strict mapping to true, every column in the Excel sheet must match a property in the destination class.
		// And every property in the destination class must match a column in the sheet. If not an exception is thrown.
		// When false (default), you can have extra columns not in the destination class or extra properties not in the sheet.
//		excel.StrictMapping=true; // Comment out this line and you get an exception because the CountryInfo class has properties that has no column in the sheet.

		// Get a list of countries in europe with a population > 20000000.
		var countries=from country in excel.Worksheet<CountryInfo>()
					  where country.Continent == "Europe" && country.Population>20000000
					  select country;

		// Print all selected countries.
		Console.WriteLine("Countries in Europe with population>20000000");
		foreach (var country in countries)
		{
			Console.WriteLine("Country: {0} - Capital: {1} - Population: {2}", country.Country, country.Capital, country.Population);

			// Excel data is copied locally thus data changed here won't be updated in the original sheet.
			country.Country="New name";
		}
	}

	/// <summary>
	/// Explicit sheet name example.
	/// </summary>
	/// <param name="sheetName">The name of the sheet to access.</param>
	static void ExplicitSheetName(string sheetName)
	{
		Console.WriteLine("\n*** Explicit sheet name: {0} ***", sheetName);

		// Open XL file.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);

		// Get a list of countries taken from the given sheet.
		// Note, instead of a sheet name, you can also pass a sheet index (start index 0).
		// But be aware that when using sheet indices, the order is alphabetic and not the order as the sheets appear in Excel.
		var countries=from country in excel.Worksheet<CountryInfo>(sheetName)
					  select country;

		// Print all selected countries.
		Console.WriteLine("All countries from sheet {0}", sheetName);
		foreach (var country in countries)
		{
			Console.WriteLine("Country: {0} - Capital: {1} - Population: {2}", country.Country, country.Capital, country.Population);
		}
	}

	/// <summary>
	/// Column remapping example.
	/// </summary>
	/// <param name="sheetName">The name of the sheet to access.</param>
	static void ColumnRemapping(string sheetName)
	{
		Console.WriteLine("\n*** Column remapping: Country->Nation ***");

		// Open XL file.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);

		// Add column mapping. Map the excel "Country" column to the "Nation" field.
		excel.AddMapping<CountryInfo>(x => x.Nation, "Country");

		// Get a list of nations taken from the given sheet.
		var nations=from nation in excel.Worksheet<CountryInfo>(sheetName)
					select nation;

		// Print all selected nations.
		Console.WriteLine("All nations from sheet {0}", sheetName);
		foreach (var nation in nations)
		{
			Console.WriteLine("Nation: {0} - Capital: {1} - Population: {2}", nation.Nation, nation.Capital, nation.Population);
		}
	}

	/// <summary>
	/// Access an Excel row through the LinqToExcel.Row class instead of copying it to own class.
	/// </summary>
	/// <param name="sheetName">The name of the sheet to access.</param>
	static void AccessThroughRow(string sheetName)
	{
		Console.WriteLine("\n*** Access through LinqToExcel.Row instead own class ***");

		// Open XL file.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);

		// Get a list of countries from europe with population>20000000 taken from the given sheet.
		// Note we use [] operator in where clause instead of selecting a field from a class.
		// Also note that we need to cast the population field to an int in the where clause.
		var countries=from country in excel.Worksheet(sheetName)
					  where country["Continent"]=="Europe" && country["Population"].Cast<int>()>20000000
					  select country;

		// Print all selected countries.
		Console.WriteLine("All countries in Europe with population>20000000 using LinqToExcel.Row");
		foreach (var country in countries)
		{
			Console.WriteLine("Country: {0} - Capital: {1} - Population: {2}", country["Country"], country["Capital"], country["Population"]);
		}
	}

	/// <summary>
	/// Access a range example. 
	/// </summary>
	/// <param name="sheetName">The name of the sheet to access.</param>
	/// <param name="start">The start index.</param>
	/// <param name="end">The end index.</param>
	static void AccessRange(string sheetName, string start, string end)
	{
		Console.WriteLine("\n*** Access range ({0}, {1}) ***", start, end);

		// Open XL file.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);

		// Get a list of countries taken from the given sheet.
		var countries=from country in excel.WorksheetRange<CountryInfo>(start, end, sheetName)
					  select country;

		// Print all selected countries.
		Console.WriteLine("All countries from sheet {0} range ({1}:{2})", sheetName, start, end);
		foreach (var country in countries)
		{
			Console.WriteLine("Country: {0} - Capital: {1} - Population: {2}", country.Country, country.Capital, country.Population);
		}
	}

	/// <summary>
	/// Access a sheet without header row.
	/// </summary>
	/// <param name="sheetName">The name of the sheet to access.</param>
	static void Noheader(string sheetName)
	{
		Console.WriteLine("\n*** Access sheet with no header ***");

		// Open XL file.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);

		// Get a list of countries from europe with population>20000000 taken from the given sheet without header.
		// Note that we need to use column index numbers.
		// Also note that we need to cast the population field to an int in the where clause.
		// No header can be combined with no range using the WorksheetRangeNoHeader() method.
		var countries=from country in excel.WorksheetNoHeader(sheetName)
					  where country[2]=="Europe" && country[3].Cast<int>()>20000000
					  select country;

		// Print all selected countries.
		Console.WriteLine("All countries in Europe with population>20000000 using indices");
		foreach (var country in countries)
		{
			Console.WriteLine("Country: {0} - Capital: {1} - Population: {2}", country[0], country[1], country[3]);
		}
	}

	/// <summary>
	/// Apply transformations before storing data.
	/// </summary>
	/// <param name="sheetName">The name of the sheet to access.</param>
	static void Transformations(string sheetName)
	{
		Console.WriteLine("\n*** Transformations ***");

		// Open XL document.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);

		// Add transformation 1: Divide population by 1000000.
		excel.AddTransformation<CountryInfo>(x => x.Population, cellValue => Int32.Parse(cellValue)/1000000);

		// Add transformation 2: First map the "Continent" string column to the "InEurope" boolean field.
		// Then transform the continent string to a boolean (True when continent=="Europe").
		excel.AddMapping<CountryInfo>(x => x.InEurope, "Continent");
		excel.AddTransformation<CountryInfo>(x => x.InEurope, cellValue => cellValue=="Europe");

		// Get a list of countries in europe with a population > 20000000.
		var countries=from country in excel.Worksheet<CountryInfo>(sheetName)
					  select country;

		// Print all selected countries.
		Console.WriteLine("Countries with population in million and continent transformated to boolean InEurope");
		foreach (var country in countries)
		{
			Console.WriteLine("Country: {0} (EU: {3})- Capital: {1} - Population: {2} million", country.Country, country.Capital, country.Population, country.InEurope);

			// Excel data is copied locally thus data changed here won't be updated in the original sheet.
			country.Country="New name";
		}
	}

	/// <summary>
	/// Query the sheets in a workbook.
	/// </summary>
	static void QuerySheets()
	{
		Console.WriteLine("\n*** Sheet in the workbook ***");

		// Open XL document.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);

		// Get the sheets in the worksbook.
		var sheetNames=excel.GetWorksheetNames();
		foreach (var sheet in sheetNames) Console.WriteLine("- Sheet name: {0}", sheet);
	}

	/// <summary>
	/// Query the sheets in a workbook.
	/// </summary>
	/// <param name="sheetName">The name of the sheet to access.</param>
	static void QueryColumns(string sheetName)
	{
		Console.WriteLine("\n*** Columns of sheet: {0} ***", sheetName);

		// Open XL document.
		var excel=new LinqToExcel.ExcelQueryFactory(m_xlFile);

		// Get the sheets in the worksbook.
		var columnNames=excel.GetColumnNames(sheetName);
		foreach (var column in columnNames) Console.WriteLine("- Column name: {0}", column);
	}

}
