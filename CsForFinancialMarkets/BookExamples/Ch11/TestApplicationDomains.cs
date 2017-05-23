// TestApplicationDomains.cs
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
            AppDomain dom = AppDomain.CreateDomain("Hello domain");
            ShowDomainInfo();

            try
            {
                dom.ExecuteAssembly("Hello.exe");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            AppDomain.Unload(dom);

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

    }
}
