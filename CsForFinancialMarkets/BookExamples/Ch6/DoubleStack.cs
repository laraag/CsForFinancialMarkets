// DoubleStack.cs
//
// (C) Datasim Education BV  2005-2013


using System;


/// <summary>
/// Double stack class.
/// </summary>
public class DoubleStack
{
	// The array for the elements
	private double[] m_items;

	// Current index in the stack
	private int m_index=0;

	/// <summary>
	/// Default constructor.
	/// Creates stack with default size.
	/// </summary>
	public DoubleStack(): this(100)
	{
	}

	/// <summary>
	/// Constructor that creates a stack of a certain size.
	/// </summary>
	/// <param name="size"></param>
	public DoubleStack(int size)
	{
		m_items=new double[size];
	}

	/// <summary>
	/// Push an element to the stack.
	/// </summary>
	/// <param name="value">The value to add.</param>
	public void Push(double value)
	{
		// First check if the stack is not already full
		if (m_index>=m_items.Length) throw new ApplicationException("Stack full");

		// Add the element to the stack
		m_items[m_index++]=value;
	}

	/// <summary>
	/// Pop an element from the stack.
	/// </summary>
	/// <returns>The element removed from the stack.</returns>
	public double Pop()
	{
		// Check if there are elements on the stack
		if (m_index<=0) throw new ApplicationException("Stack empty");

		// Remove element from the stack
		return m_items[--m_index];
	}
}