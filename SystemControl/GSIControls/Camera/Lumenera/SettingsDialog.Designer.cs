namespace GSIControls.Camera.Lumenera
{
    partial class SettingsDialog
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.chkDoBinningY = new System.Windows.Forms.CheckBox();
            this.txtYSampling = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtXSampling = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtXOffset = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtYOffset = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDoBinningX = new System.Windows.Forms.CheckBox();
            this.txtFrameRate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtGain = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtExposure = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ddPixelFormat = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRestoreDefaults = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.chkDoBinningY);
            this.groupBox1.Controls.Add(this.txtYSampling);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtXSampling);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtHeight);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtWidth);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtXOffset);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtYOffset);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkDoBinningX);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 232);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image slicing";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 203);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Flip y";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(13, 180);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(50, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Flip x";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // chkDoBinningY
            // 
            this.chkDoBinningY.AutoSize = true;
            this.chkDoBinningY.Location = new System.Drawing.Point(97, 203);
            this.chkDoBinningY.Name = "chkDoBinningY";
            this.chkDoBinningY.Size = new System.Drawing.Size(87, 17);
            this.chkDoBinningY.TabIndex = 1;
            this.chkDoBinningY.Text = "Do binning Y";
            this.chkDoBinningY.UseVisualStyleBackColor = true;
            this.chkDoBinningY.CheckedChanged += new System.EventHandler(this.chkDoBinningY_CheckedChanged);
            // 
            // txtYSampling
            // 
            this.txtYSampling.Location = new System.Drawing.Point(97, 154);
            this.txtYSampling.Name = "txtYSampling";
            this.txtYSampling.Size = new System.Drawing.Size(79, 20);
            this.txtYSampling.TabIndex = 5;
            this.txtYSampling.Leave += new System.EventHandler(this.txtYSampling_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Y Sampling";
            // 
            // txtXSampling
            // 
            this.txtXSampling.Location = new System.Drawing.Point(97, 128);
            this.txtXSampling.Name = "txtXSampling";
            this.txtXSampling.Size = new System.Drawing.Size(79, 20);
            this.txtXSampling.TabIndex = 5;
            this.txtXSampling.Leave += new System.EventHandler(this.txtXSampling_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "X Sampling";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(97, 102);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(79, 20);
            this.txtHeight.TabIndex = 5;
            this.txtHeight.Leave += new System.EventHandler(this.txtHeight_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Height";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(97, 76);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(79, 20);
            this.txtWidth.TabIndex = 5;
            this.txtWidth.Leave += new System.EventHandler(this.txtWidth_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Width";
            // 
            // txtXOffset
            // 
            this.txtXOffset.Location = new System.Drawing.Point(97, 24);
            this.txtXOffset.Name = "txtXOffset";
            this.txtXOffset.Size = new System.Drawing.Size(79, 20);
            this.txtXOffset.TabIndex = 5;
            this.txtXOffset.Leave += new System.EventHandler(this.txtXOffset_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "X Offset";
            // 
            // txtYOffset
            // 
            this.txtYOffset.Location = new System.Drawing.Point(97, 50);
            this.txtYOffset.Name = "txtYOffset";
            this.txtYOffset.Size = new System.Drawing.Size(79, 20);
            this.txtYOffset.TabIndex = 3;
            this.txtYOffset.Leave += new System.EventHandler(this.txtYOffset_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y Offset";
            // 
            // chkDoBinningX
            // 
            this.chkDoBinningX.AutoSize = true;
            this.chkDoBinningX.Location = new System.Drawing.Point(97, 180);
            this.chkDoBinningX.Name = "chkDoBinningX";
            this.chkDoBinningX.Size = new System.Drawing.Size(87, 17);
            this.chkDoBinningX.TabIndex = 0;
            this.chkDoBinningX.Text = "Do binning X";
            this.chkDoBinningX.UseVisualStyleBackColor = true;
            this.chkDoBinningX.CheckedChanged += new System.EventHandler(this.chkDoBinningX_CheckedChanged);
            // 
            // txtFrameRate
            // 
            this.txtFrameRate.Location = new System.Drawing.Point(97, 19);
            this.txtFrameRate.Name = "txtFrameRate";
            this.txtFrameRate.Size = new System.Drawing.Size(79, 20);
            this.txtFrameRate.TabIndex = 5;
            this.txtFrameRate.Leave += new System.EventHandler(this.txtFrameRate_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Frame rate";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtGain);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtExposure);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.ddPixelFormat);
            this.groupBox2.Controls.Add(this.txtFrameRate);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(218, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 132);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // txtGain
            // 
            this.txtGain.Location = new System.Drawing.Point(97, 98);
            this.txtGain.Name = "txtGain";
            this.txtGain.Size = new System.Drawing.Size(79, 20);
            this.txtGain.TabIndex = 10;
            this.txtGain.Leave += new System.EventHandler(this.txtGain_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 101);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Gain";
            // 
            // txtExposure
            // 
            this.txtExposure.Location = new System.Drawing.Point(97, 72);
            this.txtExposure.Name = "txtExposure";
            this.txtExposure.Size = new System.Drawing.Size(79, 20);
            this.txtExposure.TabIndex = 8;
            this.txtExposure.Leave += new System.EventHandler(this.txtExposure_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Exposure";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Pixel format";
            // 
            // ddPixelFormat
            // 
            this.ddPixelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddPixelFormat.FormattingEnabled = true;
            this.ddPixelFormat.Location = new System.Drawing.Point(97, 45);
            this.ddPixelFormat.Name = "ddPixelFormat";
            this.ddPixelFormat.Size = new System.Drawing.Size(79, 21);
            this.ddPixelFormat.TabIndex = 7;
            this.ddPixelFormat.SelectedIndexChanged += new System.EventHandler(this.ddPixelFormat_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(343, 222);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(218, 188);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRestoreDefaults
            // 
            this.btnRestoreDefaults.Location = new System.Drawing.Point(218, 222);
            this.btnRestoreDefaults.Name = "btnRestoreDefaults";
            this.btnRestoreDefaults.Size = new System.Drawing.Size(102, 23);
            this.btnRestoreDefaults.TabIndex = 10;
            this.btnRestoreDefaults.Text = "Restore defaults";
            this.btnRestoreDefaults.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(219, 151);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(197, 35);
            this.label11.TabIndex = 9;
            this.label11.Text = "Save sets all the values in the boxes, not only the ones changed,";
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 257);
            this.Controls.Add(this.btnRestoreDefaults);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SettingsDialog";
            this.Text = "SettingsDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkDoBinningX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtYSampling;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtXSampling;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtXOffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtYOffset;
        private System.Windows.Forms.CheckBox chkDoBinningY;
        private System.Windows.Forms.TextBox txtFrameRate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtGain;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtExposure;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox ddPixelFormat;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRestoreDefaults;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}