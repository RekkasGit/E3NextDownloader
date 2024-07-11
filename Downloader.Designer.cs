namespace E3NextDownloader
{
	partial class Downloader
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBoxDownloadFramework = new System.Windows.Forms.CheckBox();
			this.radioButtonLive = new System.Windows.Forms.RadioButton();
			this.radioButtonEMU = new System.Windows.Forms.RadioButton();
			this.buttonSelectFolder = new System.Windows.Forms.Button();
			this.textBox_DownloadLocation = new System.Windows.Forms.TextBox();
			this.buttonDownload = new System.Windows.Forms.Button();
			this.labelStatus = new System.Windows.Forms.Label();
			this.pictureBoxE3N = new System.Windows.Forms.PictureBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxE3N)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkBoxDownloadFramework);
			this.groupBox1.Controls.Add(this.radioButtonLive);
			this.groupBox1.Controls.Add(this.radioButtonEMU);
			this.groupBox1.Location = new System.Drawing.Point(21, 24);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(233, 73);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Version";
			// 
			// checkBoxDownloadFramework
			// 
			this.checkBoxDownloadFramework.AutoSize = true;
			this.checkBoxDownloadFramework.Checked = true;
			this.checkBoxDownloadFramework.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxDownloadFramework.Location = new System.Drawing.Point(82, 43);
			this.checkBoxDownloadFramework.Name = "checkBoxDownloadFramework";
			this.checkBoxDownloadFramework.Size = new System.Drawing.Size(129, 17);
			this.checkBoxDownloadFramework.TabIndex = 2;
			this.checkBoxDownloadFramework.Text = "Download Framework";
			this.checkBoxDownloadFramework.UseVisualStyleBackColor = true;
			// 
			// radioButtonLive
			// 
			this.radioButtonLive.AutoSize = true;
			this.radioButtonLive.Location = new System.Drawing.Point(7, 43);
			this.radioButtonLive.Name = "radioButtonLive";
			this.radioButtonLive.Size = new System.Drawing.Size(45, 17);
			this.radioButtonLive.TabIndex = 1;
			this.radioButtonLive.Text = "Live";
			this.radioButtonLive.UseVisualStyleBackColor = true;
			// 
			// radioButtonEMU
			// 
			this.radioButtonEMU.AutoSize = true;
			this.radioButtonEMU.Checked = true;
			this.radioButtonEMU.Location = new System.Drawing.Point(7, 20);
			this.radioButtonEMU.Name = "radioButtonEMU";
			this.radioButtonEMU.Size = new System.Drawing.Size(49, 17);
			this.radioButtonEMU.TabIndex = 0;
			this.radioButtonEMU.TabStop = true;
			this.radioButtonEMU.Text = "EMU";
			this.radioButtonEMU.UseVisualStyleBackColor = true;
			// 
			// buttonSelectFolder
			// 
			this.buttonSelectFolder.Location = new System.Drawing.Point(21, 103);
			this.buttonSelectFolder.Name = "buttonSelectFolder";
			this.buttonSelectFolder.Size = new System.Drawing.Size(111, 28);
			this.buttonSelectFolder.TabIndex = 1;
			this.buttonSelectFolder.Text = "Select Folder";
			this.buttonSelectFolder.UseVisualStyleBackColor = true;
			this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
			// 
			// textBox_DownloadLocation
			// 
			this.textBox_DownloadLocation.Enabled = false;
			this.textBox_DownloadLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox_DownloadLocation.Location = new System.Drawing.Point(21, 135);
			this.textBox_DownloadLocation.Name = "textBox_DownloadLocation";
			this.textBox_DownloadLocation.Size = new System.Drawing.Size(622, 29);
			this.textBox_DownloadLocation.TabIndex = 2;
			// 
			// buttonDownload
			// 
			this.buttonDownload.Location = new System.Drawing.Point(21, 170);
			this.buttonDownload.Name = "buttonDownload";
			this.buttonDownload.Size = new System.Drawing.Size(111, 28);
			this.buttonDownload.TabIndex = 3;
			this.buttonDownload.Text = "Download!";
			this.buttonDownload.UseVisualStyleBackColor = true;
			this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
			// 
			// labelStatus
			// 
			this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelStatus.Location = new System.Drawing.Point(-3, 201);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(776, 92);
			this.labelStatus.TabIndex = 4;
			// 
			// pictureBoxE3N
			// 
			this.pictureBoxE3N.Location = new System.Drawing.Point(672, -2);
			this.pictureBoxE3N.Name = "pictureBoxE3N";
			this.pictureBoxE3N.Size = new System.Drawing.Size(117, 86);
			this.pictureBoxE3N.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxE3N.TabIndex = 5;
			this.pictureBoxE3N.TabStop = false;
			// 
			// Downloader
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(787, 294);
			this.Controls.Add(this.pictureBoxE3N);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.buttonDownload);
			this.Controls.Add(this.textBox_DownloadLocation);
			this.Controls.Add(this.buttonSelectFolder);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "Downloader";
			this.Text = "E3N Downloader";
			this.Load += new System.EventHandler(this.Downloader_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxE3N)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonLive;
		private System.Windows.Forms.RadioButton radioButtonEMU;
		private System.Windows.Forms.Button buttonSelectFolder;
		private System.Windows.Forms.TextBox textBox_DownloadLocation;
		private System.Windows.Forms.Button buttonDownload;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.CheckBox checkBoxDownloadFramework;
		private System.Windows.Forms.PictureBox pictureBoxE3N;
	}
}

