
using System;

using Office=Microsoft.Office.Core;
using Excel=Microsoft.Office.Interop.Excel;

/// <summary>
/// Class with utility functions for Excel COM add-ins.
/// </summary>
public class AddInUtils
{
	/// <summary>
	/// Add a menu item.
	/// </summary>
	/// <param name="xlApp">The excel application to add the menu item to.</param>
	/// <param name="addin">The add-in instance to call when there is a menu item event.</param>
	/// <param name="menuName">The name of the menu to add the menu item to.</param>
	/// <param name="menuItemCaption">The caption of the new menu item.</param>
	/// <param name="menuItemKey">The key of the menu item.</param>
	/// <returns>The created menu item object.</returns>
 	public static Office.CommandBarButton AddMenuItem(Excel.Application xlApp, Office.COMAddIn addin, string menuName, string menuItemCaption, string menuItemKey)
	{
		Office.CommandBar cmdBar;
		Office.CommandBarButton button;

		// Make variable for 'missing' arguments. This is not the same as null.
		object missing=Type.Missing;

		// Get the "menuName" menu.
		cmdBar=xlApp.CommandBars[menuName];

		// If menu item not found then exit.
		if (cmdBar==null) return null;

		// Try to get the "menuItemCaption" menu item.
		button=cmdBar.FindControl(missing, missing, menuItemKey, missing, missing) as Office.CommandBarButton;

		// If menu item not found, add it.
		if (button==null)
		{
			// Add new button (menu item).
			button=cmdBar.Controls.Add(Office.MsoControlType.msoControlButton, missing, menuItemKey, missing, missing) as Office.CommandBarButton;

			// Set button's Caption, Tag, Style, and OnAction properties.
			button.Caption=menuItemCaption;
			button.Tag=menuItemKey;
			button.Style=Office.MsoButtonStyle.msoButtonCaption;

			// Use addin argument to return reference to this add-in.
			button.OnAction="!<" + addin.ProgId + ">";
		}

		// Return the created menu item.
		return button;
	}


	/// <summary>
	/// Remove the installed menu item.
	/// </summary>
	/// <param name="xlApp">The excel application to remove the menu item from.</param>
	/// <param name="removeMode">The way how the add-in is unloaded.</param>
	/// <param name="menuName">The name of the menu frome where the menu item must be removed.</param>
	/// <param name="menuItemCaption">The caption of the mneu item to remove.</param>
	public static void RemoveMenuItem(Excel.Application xlApp, Extensibility.ext_DisconnectMode removeMode, string menuName, string menuItemCaption)
	{
		// If user unloaded add-in, remove button. Otherwise, add-in is
		// being unloaded because the application is closing; in that case,
		// leave button as is.
		if (removeMode==Extensibility.ext_DisconnectMode.ext_dm_UserClosed)
		{
			// Delete custom command bar button.
			xlApp.CommandBars[menuName].Controls[menuItemCaption].Delete(Type.Missing);
		}
	}
}

