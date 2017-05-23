// TestSharedData.cs
//
// Simple use of application domains.
//
// (C) Datasim Education BV 2012-2013
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
            AppDomain dom = AppDomain.CreateDomain("<Domain A>");

            // Write to a named slot called "key"
            dom.SetData("key", "here it is ...");
       
            dom.DoCallBack(Do);

            AppDomain.Unload(dom);
 
            // Destroy app dom (static method)
        }

        static void Do()
        {
            // Read message from "Message" data slot
            Console.WriteLine(AppDomain.CurrentDomain.GetData("key"));
        }
    }
}
