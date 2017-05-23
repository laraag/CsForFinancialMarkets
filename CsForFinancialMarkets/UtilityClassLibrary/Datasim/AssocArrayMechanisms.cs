// AssocArrayMechanisms.cs
//
// Utility functions for associative arrays. Many data types
// are concrte (we work a lot with long and double).
//
// 2009-1-8 DD kick-off,inline code
// 2009-3-18 DD C# version
//
// (C) Datasim Education BV 2009-2010
//

using System;

// Assembling related data for set and array generation
public struct VectorCollectionGenerator<Numeric>
	// 1d array, heterogeneous
{
	public Numeric Start;						// The lowest or highest value
	public Numeric Increment;	    		    // Relative distance between values (+ or - possible)
	public long Size;							// Number of elements to be generated
}

public struct MatrixCollectionGenerator <Numeric>
		// 2d matrix
{
	public VectorCollectionGenerator<Numeric> rowGenerator;
	public VectorCollectionGenerator<Numeric> columnGenerator;

}

public class SetCreator<Numeric>
{
// Functions for sets
static Set<int> createSet(int start, int increment, int Size)
{
	Set<int> result = new Set<int>();

	int current = start;

	for (long j = 1; j <= Size; ++j)
	{
		result.Insert(current);
		current = current + increment;
	}

	return result;
}

static Set<int> createSet(VectorCollectionGenerator<int> gen)
{
	Set<int> result = new Set<int>();

	int current = gen.Start;

	for (long j = 1; j <= gen.Size; ++j)
	{
		result.Insert(current);
		current += gen.Increment;
	}

	return result;
}


}

// Some functions for testing

public class Potpourri
{
    public static void func(ref double x)
    {
        x = x * 3.0;
    }

}

