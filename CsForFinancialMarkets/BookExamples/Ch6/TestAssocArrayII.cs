// TestAssocArrayII.cs
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


class SetFactory<T> where T : new()
{
    public Set<T> createSet() 
    { 
        Set<T> s = new Set<T>();
        s.Insert(new T()); // etc.
        return s;
    }
}

public abstract class Factory
{
    public abstract object create();
}


public class DoubleFactory : Factory
{
    public override object create()
    {
        Set<double> s = new Set<double>();
        s.Insert(12.0); // etc.
        return s;
    }

}


class AssocArrayTestMainII    
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
        
        Set<string> RowNames = new Set<string>();
	    RowNames.Insert("A1");
	    RowNames.Insert("A2");
	    RowNames.Insert("A3");
	    RowNames.Insert("A4");
        RowNames.Insert("B1");

        Set<string> ColNames = new Set<string>();
        ColNames.Insert("C1");
        ColNames.Insert("C2");
        ColNames.Insert("C3");
        ColNames.Insert("C4");
        ColNames.Insert("C5");

	    //double defaultValue = 10.0;

	    // Contents of associative matrix (numeric values)
        NumericMatrix<double> mat1 = new NumericMatrix<double>(RowNames.Size(), ColNames.Size());
        mat1.initCells(3.0);

        AssocMatrix<string, string, double> myMat = new AssocMatrix<string, string, double>(RowNames, ColNames, mat1);

        Factory fs = new DoubleFactory();

        object b;
        b = fs.create();
        Set<double> s1 = (Set<double>)b;

        Console.WriteLine(s1.Size());
    
}

}

