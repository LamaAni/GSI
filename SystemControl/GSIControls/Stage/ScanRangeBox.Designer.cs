namespace GSIControls.Stage
{
    partial class ScanRangeBox
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numStartX = new GSIControls.General.NumericControl();
            this.numXOffset = new GSIControls.General.NumericControl();
            this.numXOffsetInPixels = new GSIControls.General.NumericControl();
            this.numYOffsetInPixels = new GSIControls.General.NumericControl();
            this.numYOffset = new GSIControls.General.NumericControl();
            this.numStartY = new GSIControls.General.NumericControl();
            this.chkDoSpeedup = new System.Windows.Forms.CheckBox();
            this.ddScanOver = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnTakeCurrentPosition = new System.Windows.Forms.Button();
            this.btnOffsetToCurrent = new System.Windows.Forms.Button();
            this.chkDoStopMotion = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Axis";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Start Pos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(139, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Offset to end";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(236, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Offset in pixels";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 53);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Y";
            // 
            // numStartX
            // 
            this.numStartX.Location = new System.Drawing.Point(45, 31);
            this.numStartX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numStartX.MaxValue = 1.7976931348623157E+308D;
            this.numStartX.MinValue = -1.7976931348623157E+308D;
            this.numStartX.Name = "numStartX";
            this.numStartX.Size = new System.Drawing.Size(94, 20);
            this.numStartX.TabIndex = 6;
            this.numStartX.Text = "0";
            this.numStartX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStartX.Units = 1D;
            this.numStartX.Value = 0D;
            // 
            // numXOffset
            // 
            this.numXOffset.Location = new System.Drawing.Point(142, 31);
            this.numXOffset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numXOffset.MaxValue = 1.7976931348623157E+308D;
            this.numXOffset.MinValue = -1.7976931348623157E+308D;
            this.numXOffset.Name = "numXOffset";
            this.numXOffset.Size = new System.Drawing.Size(94, 20);
            this.numXOffset.TabIndex = 7;
            this.numXOffset.Text = "1000";
            this.numXOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numXOffset.Units = 1D;
            this.numXOffset.Value = 1000D;
            this.numXOffset.Validated += new System.EventHandler(this.numXOffset_Validated);
            // 
            // numXOffsetInPixels
            // 
            this.numXOffsetInPixels.Location = new System.Drawing.Point(239, 31);
            this.numXOffsetInPixels.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numXOffsetInPixels.MaxValue = 1.7976931348623157E+308D;
            this.numXOffsetInPixels.MinValue = -1.7976931348623157E+308D;
            this.numXOffsetInPixels.Name = "numXOffsetInPixels";
            this.numXOffsetInPixels.Size = new System.Drawing.Size(94, 20);
            this.numXOffsetInPixels.TabIndex = 8;
            this.numXOffsetInPixels.Text = "0";
            this.numXOffsetInPixels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numXOffsetInPixels.Units = 1D;
            this.numXOffsetInPixels.Value = 0D;
            this.numXOffsetInPixels.Validated += new System.EventHandler(this.numXOffsetInPixels_Validated);
            // 
            // numYOffsetInPixels
            // 
            this.numYOffsetInPixels.Location = new System.Drawing.Point(239, 51);
            this.numYOffsetInPixels.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numYOffsetInPixels.MaxValue = 1.7976931348623157E+308D;
            this.numYOffsetInPixels.MinValue = -1.7976931348623157E+308D;
            this.numYOffsetInPixels.Name = "numYOffsetInPixels";
            this.numYOffsetInPixels.Size = new System.Drawing.Size(94, 20);
            this.numYOffsetInPixels.TabIndex = 11;
            this.numYOffsetInPixels.Text = "0";
            this.numYOffsetInPixels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numYOffsetInPixels.Units = 1D;
            this.numYOffsetInPixels.Value = 0D;
            this.numYOffsetInPixels.Validated += new System.EventHandler(this.numYOffsetInPixels_Validated);
            // 
            // numYOffset
            // 
            this.numYOffset.Location = new System.Drawing.Point(142, 51);
            this.numYOffset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numYOffset.MaxValue = 1.7976931348623157E+308D;
            this.numYOffset.MinValue = -1.7976931348623157E+308D;
            this.numYOffset.Name = "numYOffset";
            this.numYOffset.Size = new System.Drawing.Size(94, 20);
            this.numYOffset.TabIndex = 10;
            this.numYOffset.Text = "1000";
            this.numYOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numYOffset.Units = 1D;
            this.numYOffset.Value = 1000D;
            this.numYOffset.Validated += new System.EventHandler(this.numYOffset_Validated);
            // 
            // numStartY
            // 
            this.numStartY.Location = new System.Drawing.Point(45, 51);
            this.numStartY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numStartY.MaxValue = 1.7976931348623157E+308D;
            this.numStartY.MinValue = -1.7976931348623157E+308D;
            this.numStartY.Name = "numStartY";
            this.numStartY.Size = new System.Drawing.Size(94, 20);
            this.numStartY.TabIndex = 9;
            this.numStartY.Text = "0";
            this.numStartY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStartY.Units = 1D;
            this.numStartY.Value = 0D;
            // 
            // chkDoSpeedup
            // 
            this.chkDoSpeedup.Checked = true;
            this.chkDoSpeedup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoSpeedup.Location = new System.Drawing.Point(339, 48);
            this.chkDoSpeedup.Name = "chkDoSpeedup";
            this.chkDoSpeedup.Size = new System.Drawing.Size(85, 25);
            this.chkDoSpeedup.TabIndex = 25;
            this.chkDoSpeedup.Text = "Do speedup";
            this.chkDoSpeedup.UseVisualStyleBackColor = true;
            this.chkDoSpeedup.CheckedChanged += new System.EventHandler(this.chkDoSpeedup_CheckedChanged);
            // 
            // ddScanOver
            // 
            this.ddScanOver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddScanOver.FormattingEnabled = true;
            this.ddScanOver.Items.AddRange(new object[] {
            "X",
            "Y"});
            this.ddScanOver.Location = new System.Drawing.Point(392, 29);
            this.ddScanOver.Name = "ddScanOver";
            this.ddScanOver.Size = new System.Drawing.Size(41, 21);
            this.ddScanOver.TabIndex = 24;
            this.ddScanOver.SelectedIndexChanged += new System.EventHandler(this.ddScanOver_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(336, 31);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 13);
            this.label15.TabIndex = 23;
            this.label15.Text = "Scan over";
            // 
            // btnTakeCurrentPosition
            // 
            this.btnTakeCurrentPosition.Location = new System.Drawing.Point(45, 72);
            this.btnTakeCurrentPosition.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTakeCurrentPosition.Name = "btnTakeCurrentPosition";
            this.btnTakeCurrentPosition.Size = new System.Drawing.Size(93, 19);
            this.btnTakeCurrentPosition.TabIndex = 27;
            this.btnTakeCurrentPosition.Text = "Current";
            this.btnTakeCurrentPosition.UseVisualStyleBackColor = true;
            this.btnTakeCurrentPosition.Click += new System.EventHandler(this.btnTakeCurrentPosition_Click);
            // 
            // btnOffsetToCurrent
            // 
            this.btnOffsetToCurrent.Location = new System.Drawing.Point(142, 72);
            this.btnOffsetToCurrent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnOffsetToCurrent.Name = "btnOffsetToCurrent";
            this.btnOffsetToCurrent.Size = new System.Drawing.Size(93, 19);
            this.btnOffsetToCurrent.TabIndex = 28;
            this.btnOffsetToCurrent.Text = "Off. To Current";
            this.btnOffsetToCurrent.UseVisualStyleBackColor = true;
            this.btnOffsetToCurrent.Click += new System.EventHandler(this.btnOffsetToCurrent_Click);
            // 
            // chkDoStopMotion
            // 
            this.chkDoStopMotion.Location = new System.Drawing.Point(339, 72);
            this.chkDoStopMotion.Name = "chkDoStopMotion";
            this.chkDoStopMotion.Size = new System.Drawing.Size(102, 25);
            this.chkDoStopMotion.TabIndex = 29;
            this.chkDoStopMotion.Text = "Do Stopmotion";
            this.chkDoStopMotion.UseVisualStyleBackColor = true;
            this.chkDoStopMotion.CheckedChanged += new System.EventHandler(this.ddScanOver_SelectedIndexChanged);
            // 
            // ScanRangeBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkDoStopMotion);
            this.Controls.Add(this.btnOffsetToCurrent);
            this.Controls.Add(this.btnTakeCurrentPosition);
            this.Controls.Add(this.chkDoSpeedup);
            this.Controls.Add(this.ddScanOver);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.numYOffsetInPixels);
            this.Controls.Add(this.numYOffset);
            this.Controls.Add(this.numStartY);
            this.Controls.Add(this.numXOffsetInPixels);
            this.Controls.Add(this.numXOffset);
            this.Controls.Add(this.numStartX);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ScanRangeBox";
            this.Size = new System.Drawing.Size(444, 107);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label15;
        public General.NumericControl numStartX;
        public General.NumericControl numXOffset;
        public General.NumericControl numXOffsetInPixels;
        public General.NumericControl numYOffsetInPixels;
        public General.NumericControl numYOffset;
        public General.NumericControl numStartY;
        public System.Windows.Forms.CheckBox chkDoSpeedup;
        public System.Windows.Forms.ComboBox ddScanOver;
        private System.Windows.Forms.Button btnTakeCurrentPosition;
        private System.Windows.Forms.Button btnOffsetToCurrent;
        public System.Windows.Forms.CheckBox chkDoStopMotion;
    }
}
