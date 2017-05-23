// ApplicationDomains2.cs
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
            AppDomain dom = AppDomain.CreateDomain("<Hello domain>");
            ShowDomainInfo();

       //     dom.ExecuteAssembly("Hello.exe");

            AppDomain dom_2 = AppDomain.CreateDomain("<Second domain>");
            dom_2.DoCallBack(new CrossAppDomainDelegate(Do));

            AppDomain.Unload(dom);
            AppDomain.Unload(dom_2);

            // Destroy app dom (static method)
        }

        public static void ShowDomainInfo()
        { // Display information concerning the current domain

            AppDomain ad = AppDomain.CurrentDomain;
            Console.WriteLine();
            Console.WriteLine("FriendlyName: {0}", ad.FriendlyName);
            Console.WriteLine("Id: {0}", ad.Id);
            Console.WriteLine("IsDefaultAppDomain: {0}", ad.IsDefaultAppDomain());
        }

        static void Do()
        {
            Console.WriteLine("A method in another AppDom {0} called", AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
