namespace CLIAppManagerWinForm
{
    partial class CLIOptionUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbCLIOption = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbCLIOption = new System.Windows.Forms.TextBox();
            this.tipCLIOption = new System.Windows.Forms.ToolTip(this.components);
            this.cbCLIOption = new System.Windows.Forms.ComboBox();
            this.gbCLIOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCLIOption
            // 
            this.gbCLIOption.Controls.Add(this.pictureBox1);
            this.gbCLIOption.Controls.Add(this.tbCLIOption);
            this.gbCLIOption.Controls.Add(this.cbCLIOption);
            this.gbCLIOption.Font = new System.Drawing.Font("Verdana", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCLIOption.Location = new System.Drawing.Point(0, -2);
            this.gbCLIOption.Name = "gbCLIOption";
            this.gbCLIOption.Size = new System.Drawing.Size(195, 43);
            this.gbCLIOption.TabIndex = 0;
            this.gbCLIOption.TabStop = false;
            this.gbCLIOption.Text = "ParamName - Param Description";
            // 
            // pictureBox1
            // 
            //this.pictureBox1.Image = global::CLIAppManagerWinForm.Properties.Resources.info;
            this.pictureBox1.Location = new System.Drawing.Point(4, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 18);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // tbCLIOption
            // 
            this.tbCLIOption.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCLIOption.Location = new System.Drawing.Point(13, 16);
            this.tbCLIOption.Name = "tbCLIOption";
            this.tbCLIOption.Size = new System.Drawing.Size(174, 23);
            this.tbCLIOption.TabIndex = 0;
            this.tbCLIOption.Leave += new System.EventHandler(this.TbCLIOption_Leave);
            // 
            // cbCLIOption
            // 
            this.cbCLIOption.FormattingEnabled = true;
            this.cbCLIOption.Location = new System.Drawing.Point(16, 15);
            this.cbCLIOption.Name = "cbCLIOption";
            this.cbCLIOption.Size = new System.Drawing.Size(171, 20);
            this.cbCLIOption.TabIndex = 1;
            this.cbCLIOption.Leave += new System.EventHandler(this.CbCLIOption_Leave);
            // 
            // CLIOptionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.gbCLIOption);
            this.Name = "CLIOptionUserControl";
            this.Size = new System.Drawing.Size(198, 44);
            this.gbCLIOption.ResumeLayout(false);
            this.gbCLIOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCLIOption;
        private System.Windows.Forms.TextBox tbCLIOption;
        private System.Windows.Forms.ToolTip tipCLIOption;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cbCLIOption;
    }
}
