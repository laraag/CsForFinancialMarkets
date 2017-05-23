using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Class depicting the properties of a country.
/// </summary>
internal class CountryInfo
{
	/// <summary>
	/// The country name.
	/// </summary>
	public string Country
	{
		get;
		set;
	}

	/// <summary>
	/// The capital city of the country.
	/// </summary>
	public string Capital
	{
		get;
		set;
	}

	/// <summary>
	/// The continent of the country.
	/// </summary>
	public string Continent
	{
		get;
		set;
	}

	/// <summary>
	/// The population of the country.
	/// </summary>
	public int Population
	{
		get;
		set;
	}

	/// <summary>
	/// Alternative for country name. Used for the column remapping axample.
	/// </summary>
	public string Nation
	{
		get;
		set;
	}

	/// <summary>
	/// Extra data indicating an european country.
	/// Used in mappings example.
	/// </summary>
	public bool InEurope
	{
		get;
		set;
	}
}
