// GenericStack.cs
//
// (C) Datasim Education BV  2005-2013


using System;

public class GenericStack<T>
{
	// The array for the elements
	private T[] m_items;

	// Current index in the stack
	private int m_index=0;

	/// Default constructor.
	/// Creates stack with default size.
	public GenericStack(): this(100)
	{
	}


	/// Constructor that creates a stack of a certain size.
	public GenericStack(int size)
	{
		m_items=new T[size];
	}


	/// Push an element to the stack.
	public void Push(T value)
	{
		// First check if the stack is not already full
		if (m_index>=m_items.Length) throw new ApplicationException("Stack full");

		// Add the element to the stack
		m_items[m_index++]=value;

	}

	/// Pop an element from the stack.
	/// Returns the element removed from the stack.
	public T Pop()
	{
		// Check if there are elements on the stack
		if (m_index<=0) return default(T);

		// Remove element from the stack
		return m_items[--m_index];
	}

    public int Size()
    { // Number of element in the stack

        return m_items.Length;
    }
}

// Subclassing
/*
public class SpecialStack<T> : GenericStack<T> {}

public class DoubleStack : GenericStack<double> { }

public class SuperSpecialStack<T, U> : GenericStack<T> { }
*/