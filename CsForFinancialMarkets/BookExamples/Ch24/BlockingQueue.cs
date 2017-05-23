// BlockingQueue.cs
//
// A blocking queue using Monitors. When an element is enqueued then
// waiting threads are notified. 
//
// (C) Datasim Education BV 2009
//

using System;
using System.Collections.Generic;
using System.Threading;

public class BlockingQueue<T>
{
    // The locking object
    object syncLock = new object();

    // The multithreaded queue is composed of a 'normal' queue
    Queue<T> queue = new Queue<T>();

    public void Enqueue(T t)
    {
        lock (syncLock)
        {
            queue.Enqueue(t);
            Monitor.Pulse(syncLock); // Notify waiting thread of lock status change
        }
        // lock is released
    }

    public T Dequeue()
    {
        lock (syncLock)
        {
            while (queue.Count == 0) // Wait until the queue is non-empty
            {
                Monitor.Wait(syncLock);
            }
            return queue.Dequeue();
        }
    }
}