namespace TestMath
{
    partial class Main
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
            this.button1 = new System.Windows.Forms.Button();
            this.barRun = new System.Windows.Forms.ProgressBar();
            this.txtTimerResult = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnComposeImage = new System.Windows.Forms.Button();
            this.chkLoadVectorTest = new System.Windows.Forms.CheckBox();
            this.btnMakeImages = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numDx = new GSIControls.General.NumericControl();
            this.numDISize = new GSIControls.General.NumericControl();
            this.chkOverY = new System.Windows.Forms.CheckBox();
            this.chkReversedStacking = new System.Windows.Forms.CheckBox();
            this.txtTargetDir = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.lblTestStacking = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 18);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "BtnDoTest";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // barRun
            // 
            this.barRun.Location = new System.Drawing.Point(52, 95);
            this.barRun.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barRun.Name = "barRun";
            this.barRun.Size = new System.Drawing.Size(586, 35);
            this.barRun.TabIndex = 1;
            // 
            // txtTimerResult
            // 
            this.txtTimerResult.Location = new System.Drawing.Point(52, 174);
            this.txtTimerResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTimerResult.Multiline = true;
            this.txtTimerResult.Name = "txtTimerResult";
            this.txtTimerResult.Size = new System.Drawing.Size(584, 413);
            this.txtTimerResult.TabIndex = 2;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(812, 17);
            this.btnConvert.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(168, 35);
            this.btnConvert.TabIndex = 3;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnComposeImage
            // 
            this.btnComposeImage.Location = new System.Drawing.Point(812, 58);
            this.btnComposeImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnComposeImage.Name = "btnComposeImage";
            this.btnComposeImage.Size = new System.Drawing.Size(168, 35);
            this.btnComposeImage.TabIndex = 4;
            this.btnComposeImage.Text = "Make image";
            this.btnComposeImage.UseVisualStyleBackColor = true;
            this.btnComposeImage.Click += new System.EventHandler(this.btnComposeImage_Click);
            // 
            // chkLoadVectorTest
            // 
            this.chkLoadVectorTest.AutoSize = true;
            this.chkLoadVectorTest.Location = new System.Drawing.Point(640, 23);
            this.chkLoadVectorTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkLoadVectorTest.Name = "chkLoadVectorTest";
            this.chkLoadVectorTest.Size = new System.Drawing.Size(157, 24);
            this.chkLoadVectorTest.TabIndex = 5;
            this.chkLoadVectorTest.Text = "Load Vector Test";
            this.chkLoadVectorTest.UseVisualStyleBackColor = true;
            // 
            // btnMakeImages
            // 
            this.btnMakeImages.Location = new System.Drawing.Point(812, 229);
            this.btnMakeImages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMakeImages.Name = "btnMakeImages";
            this.btnMakeImages.Size = new System.Drawing.Size(168, 35);
            this.btnMakeImages.TabIndex = 6;
            this.btnMakeImages.Text = "MakeTestImages";
            this.btnMakeImages.UseVisualStyleBackColor = true;
            this.btnMakeImages.Click += new System.EventHandler(this.btnMakeImages_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(807, 278);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(807, 108);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "dx";
            // 
            // numDx
            // 
            this.numDx.Location = new System.Drawing.Point(918, 103);
            this.numDx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numDx.MaxValue = 1.7976931348623157E+308D;
            this.numDx.MinValue = -1.7976931348623157E+308D;
            this.numDx.Name = "numDx";
            this.numDx.Size = new System.Drawing.Size(60, 26);
            this.numDx.TabIndex = 9;
            this.numDx.Text = "1";
            this.numDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDx.Units = 1D;
            this.numDx.Value = 1D;
            // 
            // numDISize
            // 
            this.numDISize.Location = new System.Drawing.Point(918, 274);
            this.numDISize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numDISize.MaxValue = 1.7976931348623157E+308D;
            this.numDISize.MinValue = -1.7976931348623157E+308D;
            this.numDISize.Name = "numDISize";
            this.numDISize.Size = new System.Drawing.Size(60, 26);
            this.numDISize.TabIndex = 7;
            this.numDISize.Text = "4";
            this.numDISize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDISize.Units = 1D;
            this.numDISize.Value = 4D;
            // 
            // chkOverY
            // 
            this.chkOverY.AutoSize = true;
            this.chkOverY.Location = new System.Drawing.Point(670, 158);
            this.chkOverY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkOverY.Name = "chkOverY";
            this.chkOverY.Size = new System.Drawing.Size(79, 24);
            this.chkOverY.TabIndex = 11;
            this.chkOverY.Text = "OverY";
            this.chkOverY.UseVisualStyleBackColor = true;
            // 
            // chkReversedStacking
            // 
            this.chkReversedStacking.AutoSize = true;
            this.chkReversedStacking.Location = new System.Drawing.Point(670, 194);
            this.chkReversedStacking.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkReversedStacking.Name = "chkReversedStacking";
            this.chkReversedStacking.Size = new System.Drawing.Size(166, 24);
            this.chkReversedStacking.TabIndex = 12;
            this.chkReversedStacking.Text = "Reversed stacking";
            this.chkReversedStacking.UseVisualStyleBackColor = true;
            // 
            // txtTargetDir
            // 
            this.txtTargetDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtTargetDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtTargetDir.Location = new System.Drawing.Point(52, 655);
            this.txtTargetDir.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTargetDir.Name = "txtTargetDir";
            this.txtTargetDir.Size = new System.Drawing.Size(584, 26);
            this.txtTargetDir.TabIndex = 13;
            this.txtTargetDir.Text = "C:\\Users\\gsi\\Desktop\\Testing";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 626);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Target directory";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(690, 379);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(273, 38);
            this.button2.TabIndex = 15;
            this.button2.Text = "Test stacking";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblTestStacking
            // 
            this.lblTestStacking.Location = new System.Drawing.Point(690, 439);
            this.lblTestStacking.Name = "lblTestStacking";
            this.lblTestStacking.Size = new System.Drawing.Size(273, 131);
            this.lblTestStacking.TabIndex = 16;
            this.lblTestStacking.Text = "[Info about test stack here]";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 763);
            this.Controls.Add(this.lblTestStacking);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTargetDir);
            this.Controls.Add(this.chkReversedStacking);
            this.Controls.Add(this.chkOverY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numDx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numDISize);
            this.Controls.Add(this.btnMakeImages);
            this.Controls.Add(this.chkLoadVectorTest);
            this.Controls.Add(this.btnComposeImage);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.txtTimerResult);
            this.Controls.Add(this.barRun);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar barRun;
        private System.Windows.Forms.TextBox txtTimerResult;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnComposeImage;
        private System.Windows.Forms.CheckBox chkLoadVectorTest;
        private System.Windows.Forms.Button btnMakeImages;
        private GSIControls.General.NumericControl numDISize;
        private System.Windows.Forms.Label label1;
        private GSIControls.General.NumericControl numDx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkOverY;
        private System.Windows.Forms.CheckBox chkReversedStacking;
        private System.Windows.Forms.TextBox txtTargetDir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblTestStacking;
    }
}

