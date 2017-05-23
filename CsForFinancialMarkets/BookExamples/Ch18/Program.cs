using System;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder;

public class C1 : DynamicObject
{ // DynamicObject is the base class for all dynamic behaviour at run-time

    public C1() { }
    public void draw() { Console.WriteLine("a draw is called"); }

    // Represents invoke member dynamic operation at call site; needed
    // for custom binding.
    public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args,
                                            out object result)
    { // Mainly for graceful degradation

        Console.WriteLine(binder.Name + " was called");
        result = null;
        return true;
    }
}

public class C2
{
    public C2() { }
}

namespace DynamicBinding
{
    class Program
    {
        public static dynamic GetObject() 
        { 
            return new C1();
           //   return new C2();
        }

        static void Main()
        {

            dynamic d = GetObject();

            try
            {
                d.draw(); // OK if C1 instance; run-time error if C2
                d.print();
            }
            catch (RuntimeBinderException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
