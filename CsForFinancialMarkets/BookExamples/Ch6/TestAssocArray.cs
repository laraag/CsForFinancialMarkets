// TestAssocArray.cs
//
// Test program for associative arrays, matrices and other structures.
//
// 2009-3- (18, 1) Code completed.
//
// (C) Datasim Education BV 2009-2013
//

using System;
using System.Collections.Generic;
using System.Reflection;

class AssocArrayTestMain    
{
    public static void Main()
    {

        AssocArray<int, double> assArr = new  AssocArray<int, double> ();
        assArr[0] = 2.0;
        assArr[100] = 3.0;
        assArr[10] = 43.0;
        assArr[1000] = 34.0;

        Console.WriteLine(assArr[100]);
        foreach (KeyValuePair<int, double> kvp in assArr)
        {
            Console.WriteLine("{0}, {1}", kvp.Key, kvp.Value);
        }

        Set<string> names = new Set<string>();
	    names.Insert("A1");
	    names.Insert("A2");
	    names.Insert("A3");
	    names.Insert("A4");
        names.Insert("B1");

	    double defaultValue = 10.0;
        
	    AssocArray<string, double> myAssocArray = new AssocArray<string, double>(names, defaultValue);
	    myAssocArray.print();
	    myAssocArray["A4"] = 99.99;
        myAssocArray.print();


	    // Test other functions
	    AssocArray<string, double> myAssocArray2 = new AssocArray<string, double> (myAssocArray);
	    myAssocArray2.print();

	    AssocMatrix<string, string, double> myMat = new AssocMatrix<string, string, double>();
        myMat.print();
	 
        // Create an associative matrix with 2 sets and a numeric matrix
	    NumericMatrix<double> mat1 = new NumericMatrix<double>(names.Size(), names.Size());
        AssocMatrix<string, string, double> myMat2 = new AssocMatrix<string, string, double>(names, names, mat1);
        myMat2.print();

	           
	    // Now work with ranges in this associative matrix
	    SpreadSheetVertex<long, string> ul = new SpreadSheetVertex<long, string> ();	// Upper left
	    ul.first = 1;
        ul.second = "B";

	    SpreadSheetVertex<long, string> lr = new SpreadSheetVertex<long, string> ();	// Lower right
	    lr.first = 3;
	    lr.second = "D";

	    SpreadSheetRange<long, string> myRange = new SpreadSheetRange<long,string>();
	    myRange.upperLeft = ul;
	    myRange.lowerRight = lr;

    //	myMat22.modify(myRange, Modifier1);
	    //print (*(myMat2.Data()));

	    // print(myMat2.extract(myRange));

        SpreadSheetVertex<string, string> ul2 = new SpreadSheetVertex<string, string>();	// Upper left
        ul2.first = "A1";
        ul2.second = "A1";

        SpreadSheetVertex<string, string> lr2 = new SpreadSheetVertex<string, string>();	// Upper left
        lr2.first = "A4";
        lr2.second = "A4";


        SpreadSheetRange<string, string> myRange2= new SpreadSheetRange<string, string>();
        myRange2.upperLeft = ul2;
        myRange2.lowerRight = lr2;

       

        // Now work with Dates; 101 example, later more extensive, eg. I/O witb Excel
        Set<DateTime> dates = new Set<DateTime>();
        dates.Insert(DateTime.Now);
        dates.Insert(new DateTime(2009,3,17));

        NumericMatrix<double> payments = new NumericMatrix<double>(dates.Size(), dates.Size());
        AssocMatrix<DateTime, DateTime, double> Sheet = new AssocMatrix<DateTime, DateTime, double>(dates, dates, payments);
   
}

}

