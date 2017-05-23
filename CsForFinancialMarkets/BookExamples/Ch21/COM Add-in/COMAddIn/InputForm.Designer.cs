namespace Datasim
{
	partial class InputForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblStartValue = new System.Windows.Forms.Label();
			this.txtStartValue = new System.Windows.Forms.TextBox();
			this.txtEndValue = new System.Windows.Forms.TextBox();
			this.lblEndValue = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblStartValue
			// 
			this.lblStartValue.AutoSize = true;
			this.lblStartValue.Location = new System.Drawing.Point(12, 9);
			this.lblStartValue.Name = "lblStartValue";
			this.lblStartValue.Size = new System.Drawing.Size(61, 13);
			this.lblStartValue.TabIndex = 0;
			this.lblStartValue.Text = "Start value:";
			// 
			// txtStartValue
			// 
			this.txtStartValue.Location = new System.Drawing.Point(79, 6);
			this.txtStartValue.Name = "txtStartValue";
			this.txtStartValue.Size = new System.Drawing.Size(100, 20);
			this.txtStartValue.TabIndex = 1;
			// 
			// txtEndValue
			// 
			this.txtEndValue.Location = new System.Drawing.Point(79, 32);
			this.txtEndValue.Name = "txtEndValue";
			this.txtEndValue.Size = new System.Drawing.Size(100, 20);
			this.txtEndValue.TabIndex = 3;
			// 
			// lblEndValue
			// 
			this.lblEndValue.AutoSize = true;
			this.lblEndValue.Location = new System.Drawing.Point(12, 35);
			this.lblEndValue.Name = "lblEndValue";
			this.lblEndValue.Size = new System.Drawing.Size(58, 13);
			this.lblEndValue.TabIndex = 2;
			this.lblEndValue.Text = "End value:";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(15, 69);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(104, 69);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// InputForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(196, 104);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtEndValue);
			this.Controls.Add(this.lblEndValue);
			this.Controls.Add(this.txtStartValue);
			this.Controls.Add(this.lblStartValue);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "InputForm";
			this.Text = "Generate range";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InputForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblStartValue;
		private System.Windows.Forms.TextBox txtStartValue;
		private System.Windows.Forms.TextBox txtEndValue;
		private System.Windows.Forms.Label lblEndValue;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}