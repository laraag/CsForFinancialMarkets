// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// InterpAdapter.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 // InterpAdapter adapt input to interpolator in curve calculus. For example Df to LogDf (FromDfToInterp) and LogDf to Df (FromInterpToDf)
 // Adapt from DF to F(DF) and from F(DF) to DF

 // data adapter for interpolator interpolator. As default we start from DF and transform to...
public abstract class InterpAdapter
{
     // Data member
    protected Date refDate;
     // initialise ref date
    protected void IniRefDate(Date RefDate) { refDate = RefDate; } // set ref date

     // Derived class should implement these methods
    abstract public double FromDfToInterp(double Df, double SerialDate);  // from DF to x (r,log of r, log of df,...)
    abstract public double FromInterpToDf(double x, double SerialDate);  // from x (r,log of r, log of df,...) to DF
}

 // If we interpolate on Df: no transformations are needed
public class OnDf : InterpAdapter
{
    public OnDf() { } // a parameter less constructor
    override public double FromDfToInterp(double Df, double SerialDate) { return Df; }
    override public double FromInterpToDf(double x, double SerialDate) { return x; }
}

 // If we interpolate on log Df
public class OnLogDf : InterpAdapter
{
    public OnLogDf() { } // a parameter less constructor
    override public double FromDfToInterp(double Df, double SerialDate) { return Math.Log(Df); ; }
    override public double FromInterpToDf(double x, double SerialDate) { return Math.Exp(x); }
}

 // If we interpolate on continuous rate r
public class Onr : InterpAdapter
{
    public Onr() { } // a parameter less constructor

     // the method should return r (continuous rate)
    override public double FromDfToInterp(double Df, double SerialDate)
    {
        double t = refDate.YF_365(new Date(SerialDate)); // as example I use ACT/365, can be changed (ACT/ACT,.. etc)
        if (t == 0) return 0.0; // to avoid error ../0
        return -Math.Log(Df) / t;  // -ln(df)/t
    }

     // method should return corresponding df
    override public double FromInterpToDf(double x, double SerialDate)
    {
        double t = refDate.YF_365(new Date(SerialDate)); // as example I use ACT/365, can be changed (ACT/ACT,.. etc)
        if (t == 0) return 1.0; // t==0 df =1.0
        return Math.Exp(-x * t);  // exp(-r*t)}
    }
}

 // If we interpolate on log of r
public class OnLogr : InterpAdapter
{
    public OnLogr() { } // a parameter less constructor

     // the method should return log r (continuous rate)
    override public double FromDfToInterp(double Df, double SerialDate)
    {
        double t = refDate.YF_365(new Date(SerialDate)); // as example I use ACT/365, can be changed (ACT/ACT,.. etc)
        if (t == 0) return 0.0; // to avoid error ../0
        return Math.Log(-Math.Log(Df) / t);  // ln(-ln(df)/t)       
    }

     // method should return corresponding df
    override public double FromInterpToDf(double x, double SerialDate)
    {
        double t = refDate.YF_365(new Date(SerialDate)); // as example I use ACT/365, can be changed (ACT/ACT,.. etc)
        if (t == 0) return 1.0; // t==0 df =1.0    
        double r = Math.Exp(x);  // exp (log r) = r
        return Math.Exp(-r * t);  // exp(-r*t)
    }
}
