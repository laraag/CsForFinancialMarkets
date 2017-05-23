// ParallelDataStructures.cs
//
// Data structures for parallel programming in C#
//
// (C) Datasim Education BV 2009-2013
//

using System;
using System.Threading;
using System.Threading.Tasks;        // For parallel tasks, e.g. Parallel for     
using System.Collections.Concurrent; // Parallel collections here

namespace ParallelData
{
    class ParallelData
    {
  
        static void Main()
        {

            // Concurrent queue
            ConcurrentQueue<int> myQueue = new ConcurrentQueue<int>();

            int N = 10;
            for (int j = 0; j < N; j++)
            {
                 myQueue.Enqueue(j);
            }
            Console.WriteLine("Size of queue: {0} ", myQueue.Count);

            int s;
            for (int j = 0; j < N; j++)
            {
                myQueue.TryDequeue(out s); // 
                Console.WriteLine(s);
            }
            Console.WriteLine("Size of queue: {0} ", myQueue.Count);

            // Blocking collection for use in Producer-Consumer applications
            int BufferSize = 20;
            BlockingCollection<double> myBlockedQueue = new BlockingCollection<double>(BufferSize);
            Console.WriteLine("Size of block queue: {0} ", myBlockedQueue.Count);

            // Enqueue the blocked queue
            for (int j = 0; j < BufferSize; j++)
            {
                myBlockedQueue.Add(j);
            }
            Console.WriteLine("Size of block queue: {0} ", myBlockedQueue.Count);

            // Dequeue the blocked queue
            while (myBlockedQueue.Count > 0)
            {
                Console.WriteLine(myBlockedQueue.Take());
            }
            Console.WriteLine("Size of block queue: {0} ", 
                                            myBlockedQueue.Count);

        }

    }
}
