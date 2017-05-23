// PointStruct.cs
//
// (C) Datasim Education BV  2001-2003

public struct PointStruct
{
	public double x;
	public double y;

	// Default constructor
	//	public PointStruct(){}	// Illegal. Structs always have generated default constructor. Can't make your own

	// Normal constructor
	public PointStruct(double x, double y)
	{ 
		// Constructor must initialize all fields
		this.x=x;
		this.y=y;
	}

	// Convert point to string representation
	public override string ToString()
	{
		return string.Format("PointStruct ({0}, {1})", x, y);
	}
}