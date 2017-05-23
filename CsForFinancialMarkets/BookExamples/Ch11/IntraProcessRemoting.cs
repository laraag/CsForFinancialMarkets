    // ApplicationDomains2.cs
//
// Simple use of application domains.
//
// (C) Datasim Education BV 2012
//

using System;
using System.Reflection;


namespace ApplicationDomains
{
    class TestAppDom
    {
        static void Main()
        {
            // Create app dom (static method)
            AppDomain domain = AppDomain.CreateDomain("<IntraProcessRemoting>");

            MyClass obj = (MyClass)domain.CreateInstanceAndUnwrap(
                                typeof(MyClass).Assembly.FullName,
                                typeof(MyClass).FullName);

            Console.WriteLine(obj.Hello());

            AppDomain.Unload(domain);

            // Destroy app dom (static method)
        }

        public class MyClass : MarshalByRefObject
        {

            public string Hello()
            {
                return "Hello from " + AppDomain.CurrentDomain.FriendlyName;  
            }

            public override object InitializeLifetimeService()
            {
                return null;
            }

        }
    }
}
