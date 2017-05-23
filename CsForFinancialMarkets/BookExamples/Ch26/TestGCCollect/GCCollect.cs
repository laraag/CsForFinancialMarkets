// GCCollect.cs
//
// Showing the options to perform garbage collection.
//
// (C) Datasim Education BV 2012.
//

using System;

namespace GCCollect
{
    class Test
    {
        static void Main()
        {

            // Gen0 objects only
            GC.Collect(0);

            // Gen0 and Gen1
            GC.Collect(1);

            // Gen0, Gen1 and Gen2
            GC.Collect(2); // Same as  GC.Collect()

            // Determine when to collect (take Gen0)
            GC.Collect(0, GCCollectionMode.Forced);     // GC executed immediately
            GC.Collect(0, GCCollectionMode.Default);    // Currently the same as Forced
            GC.Collect(0, GCCollectionMode.Optimized);  // determine if current time is
                                                        // optimal to reclaim objects

            // Monitoring memory usage
            bool firstCollect = false; // Perform a collection first
            long memUsed = GC.GetTotalMemory(firstCollect);
            Console.WriteLine("Memory usage: {0}", memUsed);

            firstCollect = true; // NOT collection first
            memUsed = GC.GetTotalMemory(firstCollect);
            Console.WriteLine("Memory usage: {0}", memUsed);
        }
    }
}
