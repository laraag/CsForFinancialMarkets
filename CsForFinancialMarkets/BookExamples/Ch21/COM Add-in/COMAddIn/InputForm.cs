using System;
// InputForm.cs
//
// Input form to ask to range from the user.
//
// (C) Datasim Education BV  2010


using System.Windows.Forms;

namespace Datasim
{
	public partial class InputForm: Form
	{
		public double StartValue;
		public double EndValue;

		/// <summary>
		/// Create the input form with the number of elements in the range.
		/// </summary>
		/// <param name="count">The number of cells.</param>
		public InputForm(int count)
		{
			InitializeComponent();

			// Set the initial start- and end-value.
			StartValue=1.0;
			EndValue=(double)count;

			txtStartValue.Text=StartValue.ToString();
			txtEndValue.Text=EndValue.ToString();
		}

		/// <summary>
		/// The form is closing. Parse entered values.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void InputForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// If OK was pressed.
			if (this.DialogResult==DialogResult.OK)
			{
				try
				{
					// Parse the values.
					StartValue=Double.Parse(txtStartValue.Text);
					EndValue=Double.Parse(txtEndValue.Text);
				}
				catch (Exception ex)
				{
					// Cancel close when error.
					MessageBox.Show("Error parsing start- or end-value. Must be a number.", ex.Message);
					e.Cancel=true;
				}
			}
		}
	}
}
