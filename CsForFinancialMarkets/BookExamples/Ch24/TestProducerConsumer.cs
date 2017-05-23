// Main.cs
//
// Exx. works with 1 consumer, more is a bug. (queue empty)
//
// (C) Datasim Education BV  2002-2004

using System;
using System.Collections;

public class MainClass
{
	public static void Main()
	{
		// Ask number of producers
		Console.Write("Number of producers: ");
		int nrProducers=Int32.Parse(Console.ReadLine());

		// ASk number of consumers
		Console.Write("Number of consumers: ");
		int nrConsumers=Int32.Parse(Console.ReadLine());

		// Create queue
		BlockingQueue<string> q = new BlockingQueue<string>();

		// Create and start consumers
		for (int i=0; i<nrConsumers; i++) new Consumer(q, i).Start();

		// Create and start producers
		for (int i=0; i<nrProducers; i++) new Producer(q, i).Start();
	}
}