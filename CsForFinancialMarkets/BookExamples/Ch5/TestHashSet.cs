// TestHashSet.cs
//
// Example to show use of HashSet.
//
// (C) Datasim Education BV  2001-2011.

using System;
using System.Collections;
using System.Collections.Generic;

public class DictionaryMain
{

    static void ComPareSets(HashSet<Date> s1, HashSet<Date> s2)
    { // Some comparison operators.

        Console.WriteLine(s1.IsSubsetOf(s2));
        Console.WriteLine(s1.IsProperSubsetOf(s2));
        Console.WriteLine(s1.IsSupersetOf(s2));
        Console.WriteLine(s1.IsProperSupersetOf(s2));
        Console.WriteLine(s1.Overlaps(s2));
        Console.WriteLine(s1.SetEquals(s2));
    }

    static void print(HashSet<Date> mySet)
    {

        foreach (Date date in mySet)
        {
            Console.Write("Date Set, size {0}: ", mySet.Count);
            Console.Write("{0}-{1}-{2}", date.DateValue.Year, date.DateValue.Month, date.DateValue.Day);   // Print embedded DateTime object.
            Console.WriteLine();
        }
    }

    public static void Main()
    {

        // Date class is the Germani/Duffy implementation for fixed income.
        Date TerminationDate = new Date(2009, 2, 28);
        HashSet<Date> startDates = new HashSet<Date>();
        HashSet<Date> endDates = new HashSet<Date>();
        startDates.Add(new Date(2007, 1, 15));
        endDates.Add(new Date(2007, 1, 30));
        startDates.Add(new Date(2007, 2, 15));
        endDates.Add(new Date(2007, 2, 15));
        startDates.Add(new Date(2007, 3, 15));
        endDates.Add(new Date(2007, 7, 15));
        startDates.Add(new Date(2007, 9, 30));
        endDates.Add(new Date(2008, 3, 31));
        startDates.Add(new Date(2007, 10, 30));
        endDates.Add(new Date(2007, 10, 31));
        startDates.Add(new Date(2007, 11, 30));
        endDates.Add(new Date(2008, 9, 30));
        startDates.Add(new Date(2007, 12, 15));
        endDates.Add(new Date(2007, 1, 31));

        print(startDates);

        // Determine the relationships between sets.
        ComPareSets(startDates, endDates); // False, False, True, False.

        // Set operations (destructive == modifier functions)
        HashSet<Date> sIntersection = new HashSet<Date>(startDates);
        HashSet<Date> sUnion = new HashSet<Date>(startDates);

        sIntersection.IntersectWith(endDates);
        sUnion.UnionWith(endDates);

        print(sIntersection);
        print(sUnion);


        // Bit array.
        int length = 8;
        BitArray bitArr = new BitArray(length);

        // Assig all bits to false.
        for (int j = 0; j < bitArr.Length; j++)
        {
            bitArr[j] = false;
        }
        bitArr[2] = true;

        BitArray bitArr2 = new BitArray(bitArr);
        bitArr2.SetAll(true);   // All bits == true.

        // Bitwise operations.
        BitArray bitArr3 = new BitArray(bitArr);
        bitArr3.And(bitArr);
        bitArr3.Or(bitArr);
        bitArr3.Xor(bitArr);


        // Stacks (LIFO).
        Stack<Date> myStack = new Stack<Date>();

        // Push a set of dates onto stack.
        // Iterate with foreach
        foreach (Date date in startDates)
        {
            myStack.Push(date);
        }

        // Now pop the stack until we get a run-time error.
        Date myDate = new Date();
        while (myStack.Count != 0) // Avoids run-time error.
        {
            myDate = myStack.Pop();
            Console.Write("{0}-{1}-{2} : ", myDate.DateValue.Year, myDate.DateValue.Month, myDate.DateValue.Day);
        }

        Console.WriteLine("Size of stack {0}: ", myStack.Count);    // zero; stack is empty
   

        // Queues (FIFO).
        Queue<Date> myQueue = new Queue<Date>(startDates);
          
        // Push a set of dates onto stack.Iterate with foreach.
        foreach (Date date in startDates)
        {
            myQueue.Enqueue(date);
        }

        // Now pop the stack until we get a run-time error.
        Date myDate2 = new Date();
        while (myQueue.Count != 0) // Avoids run-time error.
        {
            myDate2 = myQueue.Dequeue();
            Console.Write("{0}-{1}-{2} : ", myDate.DateValue.Year, myDate.DateValue.Month, myDate.DateValue.Day);
        }

        Console.WriteLine("Size of queue {0}: ", myQueue.Count);    // zero; queue is empty
	}


}