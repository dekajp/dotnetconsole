namespace CLIAppManagerWinForm
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnListAllDrivers = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.cbDrivers = new System.Windows.Forms.ComboBox();
            this.tbConfigFile = new System.Windows.Forms.TextBox();
            this.btnConfigFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnLoadparam = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnBrowsefldr = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.tbLogFile = new System.Windows.Forms.TextBox();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbDebug = new System.Windows.Forms.CheckBox();
            this.btnGenerateCmd = new System.Windows.Forms.Button();
            this.rtbCmd = new System.Windows.Forms.RichTextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.grpConfigbox = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblLocation = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.grpConfigbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnListAllDrivers);
            this.groupBox1.Controls.Add(this.btnHelp);
            this.groupBox1.Controls.Add(this.cbDrivers);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CLIDrivers";
            // 
            // btnListAllDrivers
            // 
            this.btnListAllDrivers.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnListAllDrivers.Location = new System.Drawing.Point(3, 42);
            this.btnListAllDrivers.Name = "btnListAllDrivers";
            this.btnListAllDrivers.Size = new System.Drawing.Size(108, 27);
            this.btnListAllDrivers.TabIndex = 3;
            this.btnListAllDrivers.Text = "List All Drivers";
            this.btnListAllDrivers.UseVisualStyleBackColor = true;
            this.btnListAllDrivers.Click += new System.EventHandler(this.BtnListAllDrivers_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.Location = new System.Drawing.Point(116, 42);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(52, 26);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // cbDrivers
            // 
            this.cbDrivers.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDrivers.FormattingEnabled = true;
            this.cbDrivers.Location = new System.Drawing.Point(3, 16);
            this.cbDrivers.Name = "cbDrivers";
            this.cbDrivers.Size = new System.Drawing.Size(333, 25);
            this.cbDrivers.TabIndex = 0;
            this.cbDrivers.SelectedIndexChanged += new System.EventHandler(this.CbDrivers_SelectedIndexChanged);
            // 
            // tbConfigFile
            // 
            this.tbConfigFile.BackColor = System.Drawing.SystemColors.Info;
            this.tbConfigFile.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfigFile.Location = new System.Drawing.Point(77, 20);
            this.tbConfigFile.Name = "tbConfigFile";
            this.tbConfigFile.ReadOnly = true;
            this.tbConfigFile.Size = new System.Drawing.Size(259, 25);
            this.tbConfigFile.TabIndex = 5;
            // 
            // btnConfigFile
            // 
            this.btnConfigFile.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigFile.Location = new System.Drawing.Point(3, 18);
            this.btnConfigFile.Name = "btnConfigFile";
            this.btnConfigFile.Size = new System.Drawing.Size(68, 27);
            this.btnConfigFile.TabIndex = 4;
            this.btnConfigFile.Text = "Locate";
            this.btnConfigFile.UseVisualStyleBackColor = true;
            this.btnConfigFile.Click += new System.EventHandler(this.BtnConfigFile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveConfig);
            this.groupBox2.Controls.Add(this.btnLoadparam);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(342, 603);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CLI Parameters ";
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveConfig.Location = new System.Drawing.Point(210, 24);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(126, 27);
            this.btnSaveConfig.TabIndex = 7;
            this.btnSaveConfig.Text = "Save Config";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.BtnSaveConfig_Click);
            // 
            // btnLoadparam
            // 
            this.btnLoadparam.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadparam.Location = new System.Drawing.Point(3, 24);
            this.btnLoadparam.Name = "btnLoadparam";
            this.btnLoadparam.Size = new System.Drawing.Size(201, 27);
            this.btnLoadparam.TabIndex = 6;
            this.btnLoadparam.Text = "Load Params From Config File";
            this.btnLoadparam.UseVisualStyleBackColor = true;
            this.btnLoadparam.Click += new System.EventHandler(this.BtnLoadparam_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(4, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 540);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnBrowsefldr);
            this.groupBox3.Controls.Add(this.btnExport);
            this.groupBox3.Controls.Add(this.tbLogFile);
            this.groupBox3.Controls.Add(this.rtbOutput);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(360, 120);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(645, 619);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CLI Output";
            // 
            // btnBrowsefldr
            // 
            this.btnBrowsefldr.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowsefldr.Location = new System.Drawing.Point(6, 20);
            this.btnBrowsefldr.Name = "btnBrowsefldr";
            this.btnBrowsefldr.Size = new System.Drawing.Size(97, 27);
            this.btnBrowsefldr.TabIndex = 4;
            this.btnBrowsefldr.Text = "Browse";
            this.btnBrowsefldr.UseVisualStyleBackColor = true;
            this.btnBrowsefldr.Click += new System.EventHandler(this.BtnBrowsefldr_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(549, 20);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(86, 27);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // tbLogFile
            // 
            this.tbLogFile.BackColor = System.Drawing.SystemColors.Info;
            this.tbLogFile.Location = new System.Drawing.Point(109, 22);
            this.tbLogFile.Name = "tbLogFile";
            this.tbLogFile.ReadOnly = true;
            this.tbLogFile.Size = new System.Drawing.Size(434, 25);
            this.tbLogFile.TabIndex = 1;
            // 
            // rtbOutput
            // 
            this.rtbOutput.BackColor = System.Drawing.SystemColors.Info;
            this.rtbOutput.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOutput.ForeColor = System.Drawing.Color.Black;
            this.rtbOutput.Location = new System.Drawing.Point(3, 53);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbOutput.Size = new System.Drawing.Size(632, 560);
            this.rtbOutput.TabIndex = 0;
            this.rtbOutput.Text = "";
            this.rtbOutput.WordWrap = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbDebug);
            this.groupBox4.Controls.Add(this.btnGenerateCmd);
            this.groupBox4.Controls.Add(this.rtbCmd);
            this.groupBox4.Controls.Add(this.btnExecute);
            this.groupBox4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(360, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(645, 105);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Run";
            // 
            // cbDebug
            // 
            this.cbDebug.AutoSize = true;
            this.cbDebug.Checked = true;
            this.cbDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDebug.Location = new System.Drawing.Point(443, 78);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(70, 21);
            this.cbDebug.TabIndex = 3;
            this.cbDebug.Text = "Debug";
            this.cbDebug.UseVisualStyleBackColor = true;
            // 
            // btnGenerateCmd
            // 
            this.btnGenerateCmd.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateCmd.Location = new System.Drawing.Point(6, 75);
            this.btnGenerateCmd.Name = "btnGenerateCmd";
            this.btnGenerateCmd.Size = new System.Drawing.Size(191, 27);
            this.btnGenerateCmd.TabIndex = 2;
            this.btnGenerateCmd.Text = "Generate Console Command";
            this.btnGenerateCmd.UseVisualStyleBackColor = true;
            this.btnGenerateCmd.Click += new System.EventHandler(this.BtnGenerateCmd_Click);
            // 
            // rtbCmd
            // 
            this.rtbCmd.BackColor = System.Drawing.SystemColors.Info;
            this.rtbCmd.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbCmd.ForeColor = System.Drawing.Color.Black;
            this.rtbCmd.Location = new System.Drawing.Point(3, 19);
            this.rtbCmd.Name = "rtbCmd";
            this.rtbCmd.ReadOnly = true;
            this.rtbCmd.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedHorizontal;
            this.rtbCmd.Size = new System.Drawing.Size(632, 53);
            this.rtbCmd.TabIndex = 1;
            this.rtbCmd.Text = "";
            // 
            // btnExecute
            // 
            this.btnExecute.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.Location = new System.Drawing.Point(519, 75);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(116, 27);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Run";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // grpConfigbox
            // 
            this.grpConfigbox.Controls.Add(this.btnConfigFile);
            this.grpConfigbox.Controls.Add(this.tbConfigFile);
            this.grpConfigbox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpConfigbox.Location = new System.Drawing.Point(12, 85);
            this.grpConfigbox.Name = "grpConfigbox";
            this.grpConfigbox.Size = new System.Drawing.Size(342, 51);
            this.grpConfigbox.TabIndex = 4;
            this.grpConfigbox.TabStop = false;
            this.grpConfigbox.Text = "Config File";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblLocation.Location = new System.Drawing.Point(15, -1);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(39, 13);
            this.lblLocation.TabIndex = 5;
            this.lblLocation.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 741);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.grpConfigbox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "CLIApplication - WinConsole ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.grpConfigbox.ResumeLayout(false);
            this.grpConfigbox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbDrivers;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnGenerateCmd;
        private System.Windows.Forms.RichTextBox rtbCmd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox tbLogFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnBrowsefldr;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnListAllDrivers;
        private System.Windows.Forms.TextBox tbConfigFile;
        private System.Windows.Forms.Button btnConfigFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnLoadparam;
        private System.Windows.Forms.GroupBox grpConfigbox;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox cbDebug;
        private System.Windows.Forms.Label lblLocation;
    }
}

