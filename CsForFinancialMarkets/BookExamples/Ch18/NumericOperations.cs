// NumericOperations.cs
//
// (C) Datasim Education BV 2012
//

using System;
using System.Dynamic;

public class NumericOperations
{
    public static dynamic Add(dynamic a, dynamic b)
    {
        return a + b;
    }

    public static dynamic Subtract(dynamic a, dynamic b)
    {
        return a - b;
    }

    public static dynamic Multiply(dynamic a, dynamic b)
    {
        return a * b;
    }

     // Special overloads in the case of double (performance
    // improvements)
    public static double Add(double a, double b)
    {
        return a + b;
    }

    public static double Subtract(double a, double b)
    {
        return a - b;
    }

    public static double Multiply(double a, double b)
    {
        return a * b;
    }

}
/*
public class GenericNumericOperations<T>
{

    // Type-safe version
    public static T Add<T>(T a, T b)
    {
        dynamic result = (dynamic)a + b;
        return (T) result;
    }

    public static T Multiply<T>(T a, T b)
    {
        dynamic result = (dynamic)a * b;
        return (T) result;
    }

    public static T Subtract<T>(T a, T b)
    {
        dynamic result = (dynamic)a - b;
        return (T) result;
    }

}
*/