// GenericDelegates.cs
//
// Delegates, inte
// (C) Datasim Education BV 2012-2013
//

using System;
using System.Runtime.InteropServices;

public class TestGenerateDelegates<T>
{
    // Model a scalar function
    public delegate T Transformer(T val);

    public static void Transform(T[] values, Transformer transform)
    {
        for (int n = 0; n < values.Length; n++)
        {
            values[n] = transform(values[n]);
        }
    }
}

public class TestGenerateDelegatesII
{
    // Model a scalar function
    public delegate T Transformer<T>(T val);

    public static void Transform<T>(T[] values, Transformer<T> transform)
    {
        for (int n = 0; n < values.Length; n++)
        {
            values[n] = transform(values[n]);
        }
    }
}

public interface IReset<T>
{
    void reset(T obj);
}

/*
interface IReset
{
    void reset<T>(T obj);
}
*/

class ResettableClass<T> : IReset<T> where T : ICloneable
{
    private T m_obj;

    public ResettableClass(T value) {m_obj = value;}

    public void reset(T obj) { m_obj = (T)(obj.Clone()); }
}

public class Point : ICloneable
{
    private double x;
    private double y;

    public Point() { x = y = 0.0; }
    public Point(double x1, double y1) { x = x1; y = y1; }

    public object Clone()
    {
        return new Point(x, y);
    }
}


class Test
{
    static double Square(double t) { return t * t; }
    static double Log(double t) { return Math.Log(t); }
    static double Cube(double t) { return t*t*t; }

    static void Main()
    {
        double[] arr = { 1.0, 2.0, 3.0 };

        TestGenerateDelegates<double>.Transform(arr, Square);
        foreach (double d in arr)
        {
            Console.Write(d + ","); // 1, 4, 9
        }

        TestGenerateDelegates<double>.Transform(arr, Log);
        foreach (double d in arr)
        {
            Console.Write(d + ", "); // 0, 1.38629, 2.19722
        }

        double[] arr2 = { 1.0, 2.0, 3.0 };
        TestGenerateDelegatesII.Transform<double>(arr2, Cube);
        foreach (double d in arr2)
        {
            Console.Write(d + ", "); // 0, 8, 81
        }

        // Generic interfaces
        Point pt = new Point(1.0, 2.0);
        ResettableClass<Point> rc = new ResettableClass<Point>(pt);

    }
}