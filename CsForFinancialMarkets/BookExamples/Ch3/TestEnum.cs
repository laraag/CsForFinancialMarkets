// TestEnum.cs
//
// Testing enums in C#.
//
// (C) Datasim Education BV 2010
//
using System;

public enum YearCount {YF_MM, YF_365, YF_365_25, YF_BB, YF_30_360, 
                    YF_30_360E, YF_30_360E_ISDA, YF_AA, YF_AAL};

public enum OptionGreeks {Price, Delta, Gamma, Theta, Vega, Rho, CostOfCarry};

// Define enum with base type unsigned integer.
public enum PointStyle : uint
{
    // Entries defined with values. If not given then 0, 1, 2, 3, etc.
    Dot = 1,
    Cross = 2,
    Square = 4,
    Circle = 2 * Square,  // =8, can use calculations
}

public class Draw
{
    public static void Main()
    {
        Draw d = new Draw();
        Point p = new Point();

        d.DrawPoint(p, PointStyle.Dot | PointStyle.Circle);
    }

    public void DrawPoint(Point p, PointStyle ps)
    { // Draw the point

        if ((ps & PointStyle.Dot) == PointStyle.Dot)
            Console.WriteLine("Drawing Point as dot");
        if ((ps & PointStyle.Cross) == PointStyle.Cross)
            Console.WriteLine("Drawing Point as cross");
        if ((ps & PointStyle.Square) == PointStyle.Square)
            Console.WriteLine("Drawing Point as square");
        if ((ps & PointStyle.Circle) == PointStyle.Circle)
            Console.WriteLine("Drawing Point as circle");
    }
}

/* Try this
public class GetNamesTest
{
    public static void Main()
    {

        Console.WriteLine("The values of the Greeks Enum are:\n");
        foreach (string s in Enum.GetNames(typeof(OptionGreeks)))
            Console.WriteLine(s);

        Console.WriteLine("The values of the Year Count Enum are:\n");
        foreach (string s in Enum.GetNames(typeof(YearCount)))
            Console.WriteLine(s);
    }
}
*/