// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// RateSet.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
/*
 * RateSet.cs
 * This Class is derived from System.Collections.CollectionBase and implements IClonable interface
 * It is used to collect market rate from the market. For each information taken from the market it records:
 * refDate = reference date of each rate
 * BuildingBlockType = the type of building block
 * RateValue = the value of rate
 * Period = the tenor of rate
 * RateSet is used as collection for bootstrapping
 */

public class RateSet : System.Collections.CollectionBase,IComparable // , ICloneable
{
     // Data member
    public Date refDate;  // reference date
	public BuildingBlockType  T;  // Building Block Type
    public double V;  // Rate Value 
    public Period M;  // Maturity  	

	 // Constructors
    public RateSet(Date RefDate) { this.refDate=RefDate; }  // Default constructor
    public RateSet(RateSet R) : this(R.V, R.M, R.T) { } // Copy Constructor
     // Constructor needs value or rate, tenor of rate as string and building block type
	public RateSet( double Value, string Maturity, BuildingBlockType Type )
        : this(Value,new Period(Maturity),Type){}
     // Constructor needs value or rate, tenor of rate as period and building block type
    public RateSet(double Value, Period Maturity, BuildingBlockType Type)
    {
        this.M = Maturity;
        this.V = Value;
        this.T = Type;
    }

	 // Members
     // Add a rate
	public void Add(double Value, string Maturity, BuildingBlockType Type)
	{
		RateSet aRate = new RateSet(Value,Maturity, Type);
        aRate.refDate= this.refDate;  // get the same ref Date. Needed in CompareTo
		List.Add(aRate);       
	}
    
     // Get index rate of list
	public RateSet Item(int Index)
	{
			return (RateSet) List[Index];
	}

     // Modify the class. Apply a shift in rate value of Shift ( ShiftValue(0,0.01) means 0.01 for first element)
    public void ShiftValue(int Index, double Shift) 
    {
        RateSet newRateSet = ((RateSet)List[Index]);  // get 'Index' element
        newRateSet.SetValue(newRateSet.V + Shift);  // set new value
        List[Index] = newRateSet;
    }

     // return a cloned List of RateSet with shifted value (for example ShiftedRateSet(1, 0.0005) will shift element #1 adding 0.0005 ) 
    public RateSet ShiftedRateSet(int Index, double Shift)
    {
        RateSet newRateSet = this.Clone(); // clone the obj
        newRateSet.ShiftValue(Index, Shift); // shift 'Index' element of newRateSet up of 'Shift' value
        return newRateSet;
    }

     // return a cloned List of RateSet with parallel shift from starting List (i.e. each element is shifted for same value)
    public RateSet ParallelShiftRateSet(double Shift)
    {
        RateSet newRateSet = this.Clone();
        foreach (RateSet r in newRateSet)
        {
            r.V += Shift;  // parallel shift
        }
        return newRateSet;
    }

     // return array of List of rate set. i-element of output array is a List of rate set shifted @ i element
    public RateSet[] ShiftedRateSetArray(double shift)
    {
        int n = List.Count;
        RateSet[] rsArr = new RateSet[n];
        for (int i = 0; i < n; i++)
        {
            rsArr[i] =  this.ShiftedRateSet(i, shift); // populate each element (i.e. List of rate set)of output
        }
        return rsArr;
    }

     // Modifiers
     // Set maturity
	public void SetMaturity(string Maturity)
	{
	this.M = new Period(Maturity);
	}
	 // Set value
    public void SetValue(double Value) 
	{
		this.V = Value;
	}
	 // Set type
    public void SetType(BuildingBlockType Type) 
	{
		this.T = Type;
	}
	 // Set ref Date
    public void SetRefDate(Date newRefDate) { this.refDate = newRefDate; }

     // To clone rate
    public RateSet Clone()
    {
        RateSet RateC = new RateSet(this.refDate);
        int i = this.Count;
        for (int j = 0; j < i; j++)
        {
            RateC.Add(this.Item(j).V, this.Item(j).M.GetPeriodStringFormat(), this.Item(j).T);
        }
        return RateC;
    }

     // to sort ascending
    public void SortMaturity()
    {
        ArrayList.Adapter(List).Sort();
    }

     // sort BB in ascending order and create BBs. return array of BB
    public BuildingBlock[] GetArrayOfBB() 
    {
        SortMaturity(); // sort

        IBuildingBlockFactory factory = new BuildingBlockFactory();  // my factory                 
        int N = List.Count; // Number of elements
        BuildingBlock[] outPut = new BuildingBlock[N]; // initialise outPut array
        
         // create each Building Block
        for (int i = 0; i < N; i++) 
        {
            outPut[i]= factory.CreateBuildingBlock(refDate, Item(i).V, Item(i).M.GetPeriodStringFormat(), Item(i).T);
        }

         // outPut
        return outPut;
    }

    #region ICompareble Members
    public int CompareTo(object y)
    {
        double X = refDate.add_period(this.M.GetPeriodStringFormat(),false).SerialValue;
        double Y = refDate.add_period(((RateSet)y).M.GetPeriodStringFormat()).SerialValue;
        return X.CompareTo(Y);
    }
    #endregion
}






