// OptionPresentation.cs
//
// Some functions when working with options.
//
// 2005-10-16 DD Kick-off code (BTW it worked first time
// because of modular approach).
// 2010-8-1 DD C# version
//
// (C) Datasim Education BV 2005-2010
//

// This class display 1 line graph in Excel
// (Get it working)

public enum OptionValueType : uint {Value=1, Delta=2, Gamma=3, Vega=4, Theta=5, Rho=6, Coc=7, Elasticity=8,};

public class OptionPresentation
{
		Option curr;

		Range<double> r;	// Extent of x axis
		int nSteps;	    	// Number of subdivisions


		Vector<double> XARR;

		private OptionPresentation() {}

		public OptionPresentation(Option option,  Range<double> extent, 
								int NumberSteps)
		{
			r = extent;
			nSteps = NumberSteps;
			curr = option;

			XARR = new Vector<double>(r.mesh(nSteps));

		}

		public Vector<double> calculate(OptionValueType yval)
		{

			// Contains value at end-points
			Vector<double> result = new Vector<double>(nSteps+1, 1);

			if (yval == OptionValueType.Value)
			{
				for (int j = XARR.MinIndex; j <= XARR.MaxIndex; j++)
				{

					result[j] = curr.Price(XARR[j]);
				}

			}

			if (yval == OptionValueType.Delta)
			{
				for (int j = XARR.MinIndex; j <= XARR.MaxIndex; j++)
				{

					result[j] = curr.Delta(XARR[j]);
				}

			}

			if (yval == OptionValueType.Gamma)
			{
				for (int j = XARR.MinIndex; j <= XARR.MaxIndex; j++)
				{

					result[j] = curr.Gamma(XARR[j]);
				}
		//	print(result);
		///	int yy; cin >> yy;
			}

			if (yval == OptionValueType.Vega)
			{
				for (int j = XARR.MinIndex; j <= XARR.MaxIndex; j++)
				{

					result[j] = curr.Vega(XARR[j]);
				}

			}

			if (yval == OptionValueType.Theta)
			{
				for (int j = XARR.MinIndex; j <= XARR.MaxIndex; j++)
				{

					result[j] = curr.Theta(XARR[j]);
				}

			}

			if (yval == OptionValueType.Rho)
			{
				for (int j = XARR.MinIndex; j <= XARR.MaxIndex; j++)
				{

					result[j] = curr.Rho(XARR[j]);
				}

			}

			if (yval == OptionValueType.Coc)
			{
				for (int j = XARR.MinIndex; j <= XARR.MaxIndex; j++)
				{

					result[j] = curr.Rho(XARR[j]);
				}

			}

			return result;

		}

// RECALL: void printOneExcel(Vector<double>  x,	
//						Vector<double> functionResult,string title)

		public void displayinExcel( OptionValueType yval)
		{

			string text= "Value";
			if (yval == OptionValueType.Delta)
			{
				text = "Delta";
			}
			if (yval == OptionValueType.Gamma)
			{
				text = "Gamma";
			}

			if (yval == OptionValueType.Vega)
			{
				text = "Vega";
			}

			if (yval == OptionValueType.Theta)
			{
				text = "Theta";
			}
			if (yval == OptionValueType.Rho)
			{
				text = "Rho";
			}

			if (yval == OptionValueType.Coc)
			{
				text = "Coc";
			}

			Vector<double> yarr = calculate(yval);
		//	cout << "array ..."; int yy; cin >> yy;
		//	print(yarr);

            ExcelMechanisms excel = new ExcelMechanisms();
			excel.printOneExcel(XARR, yarr, text, text, text, text);
		}

}