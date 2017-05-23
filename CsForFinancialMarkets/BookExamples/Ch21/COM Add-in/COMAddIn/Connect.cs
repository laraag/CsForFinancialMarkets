using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Excel=Microsoft.Office.Interop.Excel;
using Office=Microsoft.Office.Core;

namespace COMAddIn
{

	#region Read me for Add-in installation and setup information.
	// When run, the Add-in wizard prepared the registry for the Add-in.
	// At a later time, if the Add-in becomes unavailable for reasons such as:
	//   1) You moved this project to a computer other than which is was originally created on.
	//   2) You chose 'Yes' when presented with a message asking if you wish to remove the Add-in.
	//   3) Registry corruption.
	// you will need to re-register the Add-in by building the COMAddInSetup project, 
	// right click the project in the Solution Explorer, then choose install.
	#endregion
	
	/// <summary>
	///     The object for implementing an Add-in.
	/// </summary>
	/// <seealso class='IDTExtensibility2' />
	[GuidAttribute("FF970DFB-90DD-41D1-BFA4-D63532D51E25"), ProgId("COMAddIn.Connect")]
	public class Connect: Object, Extensibility.IDTExtensibility2
	{
        // The Excel application.
        private Excel.Application m_xlApp;

        // The menu item for our command.
        private Office.CommandBarButton m_menuItem;

        // Constants for the menu item to create.
        private const string m_menuName="Tools";
        private const string m_menuItemCaption="My C# COM Add-in";
        private const string m_menuItemKey="MyC#ComAddin";

		/// <summary>
		///	Implements the constructor for the Add-in object.
		///	Place your initialization code within this method.
		/// </summary>
		public Connect()
		{
		}

		/// <summary>
		///     Implements the OnConnection method of the IDTExtensibility2 interface.
		///     Receives notification that the Add-in is being loaded.
		/// </summary>
		/// <param term='application'>
		///     Root object of the host application.
		/// </param>
		/// <param term='connectMode'>
		///     Describes how the Add-in is being loaded.
		/// </param>
		/// <param term='addInInst'>
		/// Object representing this Add-in.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref System.Array custom)
		{
            // Store the Excel host application. Exit if host was not Excel.
            m_xlApp=application as Excel.Application;
            if (m_xlApp==null) return;

            // If an addInInst object given of the type COMAddin then it was loaded as COM add-in. 
            // If addInInst is the same object as myself, then I'm loaded as Automation add-in.
			if (addInInst!=this)
            {
                // Attach myself to the add-in object.
                // In that way I can call functions of this object from VBA using the add-in collection.
                Office.COMAddIn cai=addInInst as Office.COMAddIn;
                cai.Object=this;
                
                // Now install menu item and add event handler.
                m_menuItem=AddInUtils.AddMenuItem(m_xlApp, cai, m_menuName, m_menuItemCaption, m_menuItemKey);
                m_menuItem.Click+=new Office._CommandBarButtonEvents_ClickEventHandler(MyMenuHandler);
            }
		}

		/// <summary>
		///     Implements the OnDisconnection method of the IDTExtensibility2 interface.
		///     Receives notification that the Add-in is being unloaded.
		/// </summary>
		/// <param term='disconnectMode'>
		///      Describes how the Add-in is being unloaded.
		/// </param>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref System.Array custom)
		{
            AddInUtils.RemoveMenuItem(m_xlApp, disconnectMode, m_menuName, m_menuItemCaption);
		}

		/// <summary>
		///      Implements the OnAddInsUpdate method of the IDTExtensibility2 interface.
		///      Receives notification that the collection of Add-ins has changed.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnAddInsUpdate(ref System.Array custom)
		{
		}

		/// <summary>
		///      Implements the OnStartupComplete method of the IDTExtensibility2 interface.
		///      Receives notification that the host application has completed loading.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref System.Array custom)
		{
		}

		/// <summary>
		///      Implements the OnBeginShutdown method of the IDTExtensibility2 interface.
		///      Receives notification that the host application is being unloaded.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref System.Array custom)
		{
		}

        /// <summary>
        /// Menu click event handler. Called by Excel when the user clicks on our menu item.
        /// </summary>
        /// <param name="button">The button that was clicked.</param>
        /// <param name="cancelDefault">Notify if it was cancelled.</param>
        private void MyMenuHandler(Office.CommandBarButton button, ref bool cancelDefault)
        {
			// Get selected range and exit when nothing selected.
			Excel.Range range=m_xlApp.Selection as Excel.Range;
			if (range==null)
			{
				MessageBox.Show("First select a few cells");
				return;
			}

			// Get the number of cells in the collection.
			int count=range.Cells.Count;

			// Show the dialog asking for start- and end-values.
			Datasim.InputForm frm=new Datasim.InputForm(count);
			if (frm.ShowDialog()==DialogResult.Cancel) return;

			// Fill the selected range with values between start- and end-value.
			double startValue=frm.StartValue;
			double endValue=frm.EndValue;
			double stepValue=count==1?0.0:(endValue-startValue)/(double)(count-1);
			for (int i=0; i<count; i++)
			{
				(range.Cells[i+1, Type.Missing] as Excel.Range).Value2=startValue+(double)i*stepValue;
			}
        }
	}
}