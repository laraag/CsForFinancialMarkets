using System;

public sealed class SealedClass
{
	public SealedClass()
	{
	}

	// Cannot add new virtual members
	public /*virtual*/ void Test()
	{
	}
}

/* Error, can't derived from sealed class
public class Derived: SealedClass
{
	public Derived()
	{
	}
}
*/


// Base class for sealed method test.
class B
{
    // Virtual base method.
    public virtual void F() { Console.WriteLine("Base"); }
}

// Derived class with sealed method.
class D1: B
{
    // Sealed must be used in combination with override.
    public sealed override void F() { Console.WriteLine("D1"); }
}

// Second derived class can't override sealed method but can still 'overload' it.
class D2: D1
{
    // Error, can not override sealed class.
    //	public override void F() {}

    // OK, can still "overload" sealed base member.
    public new void F() { Console.WriteLine("D2"); }
}


public class Test
{
	public static void Main()
	{
		SealedClass s=new SealedClass();

        B b=new D2();
        b.F();				// Calls D1.F()

        D2 d2=new D2();
        d2.F();				// Calls D2.F()
	}
}