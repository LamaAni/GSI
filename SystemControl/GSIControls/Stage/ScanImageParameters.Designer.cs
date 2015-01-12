namespace GSIControls.Stage
{
    partial class ScanImageParameters
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
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numMaxScanSpeed = new GSIControls.General.NumericControl();
            this.numTimeUnits = new GSIControls.General.NumericControl();
            this.numSpatialUnits = new GSIControls.General.NumericControl();
            this.numScanSpeed = new GSIControls.General.NumericControl();
            this.numExp = new GSIControls.General.NumericControl();
            this.numMaxFrameRate = new GSIControls.General.NumericControl();
            this.numFrameRate = new GSIControls.General.NumericControl();
            this.numDxInPixels = new GSIControls.General.NumericControl();
            this.numPixelSize = new GSIControls.General.NumericControl();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 232);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(131, 20);
            this.label10.TabIndex = 50;
            this.label10.Text = "Scan speed[su/s]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 160);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 20);
            this.label8.TabIndex = 48;
            this.label8.Text = "Exposure [tu]";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 88);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(116, 20);
            this.label14.TabIndex = 46;
            this.label14.Text = "Max f. rate [#/s]";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 196);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(116, 20);
            this.label13.TabIndex = 44;
            this.label13.Text = "Frame rate[#/s]";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 48);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(109, 20);
            this.label12.TabIndex = 42;
            this.label12.Text = "DeltaX [pixels]";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 12);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 20);
            this.label9.TabIndex = 40;
            this.label9.Text = "Pixel size [su]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 268);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 20);
            this.label1.TabIndex = 51;
            this.label1.Text = "Spatial Units[su]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 304);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 20);
            this.label2.TabIndex = 53;
            this.label2.Text = "Time units[tu]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 124);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 20);
            this.label3.TabIndex = 56;
            this.label3.Text = "Max speed[su/s]";
            // 
            // numMaxScanSpeed
            // 
            this.numMaxScanSpeed.Location = new System.Drawing.Point(153, 121);
            this.numMaxScanSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numMaxScanSpeed.MaxValue = 1.7976931348623157E+308D;
            this.numMaxScanSpeed.MinValue = -1.7976931348623157E+308D;
            this.numMaxScanSpeed.Name = "numMaxScanSpeed";
            this.numMaxScanSpeed.Size = new System.Drawing.Size(111, 26);
            this.numMaxScanSpeed.TabIndex = 55;
            this.numMaxScanSpeed.Text = "-1";
            this.numMaxScanSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMaxScanSpeed.Units = 1D;
            this.numMaxScanSpeed.Value = -1D;
            // 
            // numTimeUnits
            // 
            this.numTimeUnits.Location = new System.Drawing.Point(153, 301);
            this.numTimeUnits.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numTimeUnits.MaxValue = 1.7976931348623157E+308D;
            this.numTimeUnits.MinValue = -1.7976931348623157E+308D;
            this.numTimeUnits.Name = "numTimeUnits";
            this.numTimeUnits.Size = new System.Drawing.Size(111, 26);
            this.numTimeUnits.TabIndex = 54;
            this.numTimeUnits.Text = "0.001";
            this.numTimeUnits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTimeUnits.Units = 1D;
            this.numTimeUnits.Value = 0.001D;
            this.numTimeUnits.Validated += new System.EventHandler(this.numericValueValidated);
            // 
            // numSpatialUnits
            // 
            this.numSpatialUnits.Location = new System.Drawing.Point(153, 265);
            this.numSpatialUnits.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numSpatialUnits.MaxValue = 1.7976931348623157E+308D;
            this.numSpatialUnits.MinValue = -1.7976931348623157E+308D;
            this.numSpatialUnits.Name = "numSpatialUnits";
            this.numSpatialUnits.Size = new System.Drawing.Size(111, 26);
            this.numSpatialUnits.TabIndex = 52;
            this.numSpatialUnits.Text = "1E-06";
            this.numSpatialUnits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSpatialUnits.Units = 1D;
            this.numSpatialUnits.Value = 1E-06D;
            this.numSpatialUnits.Validated += new System.EventHandler(this.numericValueValidated);
            // 
            // numScanSpeed
            // 
            this.numScanSpeed.Enabled = false;
            this.numScanSpeed.Location = new System.Drawing.Point(153, 229);
            this.numScanSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numScanSpeed.MaxValue = 1.7976931348623157E+308D;
            this.numScanSpeed.MinValue = -1.7976931348623157E+308D;
            this.numScanSpeed.Name = "numScanSpeed";
            this.numScanSpeed.ReadOnly = true;
            this.numScanSpeed.Size = new System.Drawing.Size(111, 26);
            this.numScanSpeed.TabIndex = 49;
            this.numScanSpeed.Text = "0";
            this.numScanSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numScanSpeed.Units = 1D;
            this.numScanSpeed.Value = 0D;
            this.numScanSpeed.Validated += new System.EventHandler(this.numericValueValidated);
            // 
            // numExp
            // 
            this.numExp.Location = new System.Drawing.Point(153, 157);
            this.numExp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numExp.MaxValue = 1.7976931348623157E+308D;
            this.numExp.MinValue = -1.7976931348623157E+308D;
            this.numExp.Name = "numExp";
            this.numExp.Size = new System.Drawing.Size(111, 26);
            this.numExp.TabIndex = 47;
            this.numExp.Text = "1";
            this.numExp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numExp.Units = 1D;
            this.numExp.Value = 1D;
            this.numExp.Validated += new System.EventHandler(this.numericValueValidated);
            // 
            // numMaxFrameRate
            // 
            this.numMaxFrameRate.Location = new System.Drawing.Point(153, 85);
            this.numMaxFrameRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numMaxFrameRate.MaxValue = 1.7976931348623157E+308D;
            this.numMaxFrameRate.MinValue = -1.7976931348623157E+308D;
            this.numMaxFrameRate.Name = "numMaxFrameRate";
            this.numMaxFrameRate.Size = new System.Drawing.Size(111, 26);
            this.numMaxFrameRate.TabIndex = 45;
            this.numMaxFrameRate.Text = "-1";
            this.numMaxFrameRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMaxFrameRate.Units = 1D;
            this.numMaxFrameRate.Value = -1D;
            this.numMaxFrameRate.Validated += new System.EventHandler(this.numericValueValidated);
            // 
            // numFrameRate
            // 
            this.numFrameRate.Enabled = false;
            this.numFrameRate.Location = new System.Drawing.Point(153, 193);
            this.numFrameRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numFrameRate.MaxValue = 1.7976931348623157E+308D;
            this.numFrameRate.MinValue = -1.7976931348623157E+308D;
            this.numFrameRate.Name = "numFrameRate";
            this.numFrameRate.ReadOnly = true;
            this.numFrameRate.Size = new System.Drawing.Size(111, 26);
            this.numFrameRate.TabIndex = 43;
            this.numFrameRate.Text = "0";
            this.numFrameRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFrameRate.Units = 1D;
            this.numFrameRate.Value = 0D;
            this.numFrameRate.Validated += new System.EventHandler(this.numericValueValidated);
            // 
            // numDx
            // 
            this.numDxInPixels.Location = new System.Drawing.Point(153, 45);
            this.numDxInPixels.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numDxInPixels.MaxValue = 100000D;
            this.numDxInPixels.MinValue = 1D;
            this.numDxInPixels.Name = "numDx";
            this.numDxInPixels.Size = new System.Drawing.Size(111, 26);
            this.numDxInPixels.TabIndex = 1;
            this.numDxInPixels.Text = "1";
            this.numDxInPixels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDxInPixels.Units = 1D;
            this.numDxInPixels.Value = 1D;
            this.numDxInPixels.Validated += new System.EventHandler(this.numericValueValidated);
            // 
            // numPixelSize
            // 
            this.numPixelSize.Location = new System.Drawing.Point(153, 9);
            this.numPixelSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numPixelSize.MaxValue = 1.7976931348623157E+308D;
            this.numPixelSize.MinValue = -1.7976931348623157E+308D;
            this.numPixelSize.Name = "numPixelSize";
            this.numPixelSize.Size = new System.Drawing.Size(111, 26);
            this.numPixelSize.TabIndex = 39;
            this.numPixelSize.Text = "1";
            this.numPixelSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPixelSize.Units = 1D;
            this.numPixelSize.Value = 1D;
            this.numPixelSize.Validated += new System.EventHandler(this.numericValueValidated);
            // 
            // ScanImageParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numMaxScanSpeed);
            this.Controls.Add(this.numTimeUnits);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numSpatialUnits);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numScanSpeed);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numExp);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.numMaxFrameRate);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.numFrameRate);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numDxInPixels);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numPixelSize);
            this.Name = "ScanImageParameters";
            this.Size = new System.Drawing.Size(276, 341);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private General.NumericControl numScanSpeed;
        private System.Windows.Forms.Label label8;
        private General.NumericControl numExp;
        private System.Windows.Forms.Label label14;
        private General.NumericControl numMaxFrameRate;
        private System.Windows.Forms.Label label13;
        private General.NumericControl numFrameRate;
        private System.Windows.Forms.Label label12;
        private General.NumericControl numDxInPixels;
        private System.Windows.Forms.Label label9;
        private General.NumericControl numPixelSize;
        private System.Windows.Forms.Label label1;
        private General.NumericControl numSpatialUnits;
        private General.NumericControl numTimeUnits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private General.NumericControl numMaxScanSpeed;
    }
}
