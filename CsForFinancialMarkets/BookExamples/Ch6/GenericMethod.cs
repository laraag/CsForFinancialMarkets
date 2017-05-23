// GenericMethods.cs
//
// Some uses of generics.
//
// (C) Datasim Education BV 2012-2013
//

public class GenericMethod
{ // Non-generic class

    public static void Swap<T>(ref T x, ref T y)
    {
        T tmp = x;
        x = y;
        y = tmp;
    }
}
