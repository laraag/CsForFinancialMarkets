// TestDomainAndThreads.cs
//
// Using application domains and threads.
//
// (C) Datasim Education BV 2012-2013
//

using System;
using System.Threading;


    class TestAppDom
    {
        static void Main()
        {
            int N = 10;

            // Create N application domains and N threads
            AppDomain[] domains = new AppDomain[N];
            Thread[] threads = new Thread[N];

            for (int j = 0; j < N; j++)
            {
                domains[j] = AppDomain.CreateDomain("Client Login" + j);
                threads[j] = new Thread(LoginOtherDomain);
            }

            // Start each thread; each thread has its own app domain
            for (int j = 0; j < N; j++)
            {
                threads[j].Start(domains[j]);
            }

            // Wait for threads to join at a barrier
            for (int j = 0; j < N; j++)
            {
                threads[j].Join();
            }

            // Unload all application domains
            for (int j = 0; j < N; j++)
            {
                AppDomain.Unload(domains[j]);
            }
            
            // Destroy app dom (static method)
        }

        // Parametrised thread start
        static void LoginOtherDomain(object domain)
        {
            ((AppDomain)domain).DoCallBack(Login);

        }


        static void Login()
        {
            Client.Login("Andrea", "");
            Console.WriteLine("Logged in as: " + Client.currentUser + " on " +
                                AppDomain.CurrentDomain.FriendlyName);
        }
        

        public class Client
        {

            public static string currentUser = "";

            public static void Login(string name, string password)
            {
                if (currentUser.Length == 0)
                {
                    // Sleep to simulate authentication
                    Thread.Sleep(500);
                   
                    // Record the current user
                    currentUser = name;
                }

            }
   
        }
    }
