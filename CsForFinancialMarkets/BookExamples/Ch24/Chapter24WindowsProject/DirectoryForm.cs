using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Diagnostics;
using System.IO;


/// <summary>
/// Summary description for Form1.
/// </summary>
public class DirectoryForm : System.Windows.Forms.Form
{
	private System.Windows.Forms.Label lblSearch;
	private System.Windows.Forms.Button btnDir;
	private System.Windows.Forms.TextBox txtSearch;
	private System.Windows.Forms.RichTextBox txtDir;

	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.Container components = null;

	public DirectoryForm()
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		//
		// TODO: Add any constructor code after InitializeComponent call
		//
	}

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	protected override void Dispose( bool disposing )
	{
		if( disposing )
		{
			if (components != null) 
			{
				components.Dispose();
			}
		}
		base.Dispose( disposing );
	}

	#region Windows Form Designer generated code
	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this.lblSearch = new System.Windows.Forms.Label();
		this.txtSearch = new System.Windows.Forms.TextBox();
		this.btnDir = new System.Windows.Forms.Button();
		this.txtDir = new System.Windows.Forms.RichTextBox();
		this.SuspendLayout();
		// 
		// lblSearch
		// 
		this.lblSearch.Location = new System.Drawing.Point(8, 8);
		this.lblSearch.Name = "lblSearch";
		this.lblSearch.Size = new System.Drawing.Size(88, 18);
		this.lblSearch.TabIndex = 1;
		this.lblSearch.Text = "Search pattern";
		// 
		// txtSearch
		// 
		this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
		this.txtSearch.Location = new System.Drawing.Point(96, 8);
		this.txtSearch.Name = "txtSearch";
		this.txtSearch.Size = new System.Drawing.Size(424, 20);
		this.txtSearch.TabIndex = 2;
		this.txtSearch.Text = "c:\\*.*";
		// 
		// btnDir
		// 
		this.btnDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
		this.btnDir.Location = new System.Drawing.Point(528, 8);
		this.btnDir.Name = "btnDir";
		this.btnDir.Size = new System.Drawing.Size(40, 20);
		this.btnDir.TabIndex = 3;
		this.btnDir.Text = "Dir";
		this.btnDir.Click += new System.EventHandler(this.btnDir_Click);
		// 
		// txtDir
		// 
		this.txtDir.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
		this.txtDir.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
		this.txtDir.Location = new System.Drawing.Point(8, 40);
		this.txtDir.Name = "txtDir";
		this.txtDir.Size = new System.Drawing.Size(560, 344);
		this.txtDir.TabIndex = 4;
		this.txtDir.Text = "";
		// 
		// DirectoryForm
		// 
		this.AcceptButton = this.btnDir;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize = new System.Drawing.Size(576, 389);
		this.Controls.Add(this.txtDir);
		this.Controls.Add(this.btnDir);
		this.Controls.Add(this.txtSearch);
		this.Controls.Add(this.lblSearch);
		this.Name = "DirectoryForm";
		this.Text = "Redirecting Standard IO";
		this.ResumeLayout(false);

	}
	#endregion

	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main() 
	{
		Application.Run(new DirectoryForm());
	}

	private void btnDir_Click(object sender, System.EventArgs e)
	{
		// Create the process start info object with command and arguments
		ProcessStartInfo si=new ProcessStartInfo("cmd.exe", @"/C dir " + txtSearch.Text);

		// Disables dos box window
		si.CreateNoWindow=true;

		// Enable redirection of standard output
		si.RedirectStandardOutput=true;
		si.UseShellExecute=false;

		// Start the process
		Process p=Process.Start(si);

		// Read process output
		txtDir.Text=p.StandardOutput.ReadToEnd();
	}
}
