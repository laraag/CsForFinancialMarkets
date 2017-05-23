// GenericPair.cs
//
// (C) Datasim Education BV  2005-2007


using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A generic class that holds a value pair.
/// </summary>
/// <typeparam name="T1">The type of the first value.</typeparam>
/// <typeparam name="T2">The type of the second value.</typeparam>
public class GenericPair<T1, T2>
{
	private T1 m_value1;
	private T2 m_value2;

	/// <summary>
	/// Default constructor.
	/// </summary>
	public GenericPair()
	{
		m_value1=default(T1);
		m_value2=default(T2);
	}

	/// <summary>
	/// Constructor with values.
	/// </summary>
	/// <param name="value1">The first value of the pair.</param>
	/// <param name="value2">The second value of the pair.</param>
	public GenericPair(T1 value1, T2 value2)
	{
		m_value1=value1;
		m_value2=value2;
	}

	/// <summary>
	/// Access the first value of the pair.
	/// </summary>
	public T1 Value1
	{
		get { return m_value1; }
		set { m_value1=value; }
	}

	/// <summary>
	/// Access the second value of the pair.
	/// </summary>
	public T2 Value2
	{
		get { return m_value2; }
		set { m_value2=value; }
	}

	/// <summary>
	/// Return string representation.
	/// </summary>
	/// <returns>The created string.</returns>
	public override string ToString()
	{
		return String.Format("({0}, {1})", m_value1.ToString(), m_value2.ToString());
	}
}