﻿namespace TestRun
{
    partial class DoForierTransformDlg
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblProgInfo = new System.Windows.Forms.Label();
            this.pannelImageShow = new System.Windows.Forms.Panel();
            this.chartSpectrum = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnValidatePreview = new System.Windows.Forms.Button();
            this.btnStoreSpectrum = new System.Windows.Forms.Button();
            this.btnCreatePreview = new System.Windows.Forms.Button();
            this.btnShowFile = new System.Windows.Forms.Button();
            this.btnDoForier = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chkDoRgbPreview = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLoadCalibration = new System.Windows.Forms.Button();
            this.ddZeroFilling = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSelectedCalib = new System.Windows.Forms.Label();
            this.numEndWavelegnth = new GSIControls.General.NumericControl();
            this.numStartWavelength = new GSIControls.General.NumericControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpectrum)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // progBar
            // 
            this.progBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progBar.Location = new System.Drawing.Point(4, 5);
            this.progBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(1253, 35);
            this.progBar.TabIndex = 0;
            this.progBar.Click += new System.EventHandler(this.progBar_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(1149, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 45);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblProgInfo
            // 
            this.lblProgInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblProgInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProgInfo.Location = new System.Drawing.Point(4, 40);
            this.lblProgInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProgInfo.Name = "lblProgInfo";
            this.lblProgInfo.Size = new System.Drawing.Size(1253, 37);
            this.lblProgInfo.TabIndex = 3;
            this.lblProgInfo.Text = "[prog]";
            this.lblProgInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgInfo.Click += new System.EventHandler(this.lblProgInfo_Click);
            // 
            // pannelImageShow
            // 
            this.pannelImageShow.AutoSize = true;
            this.pannelImageShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pannelImageShow.Location = new System.Drawing.Point(0, 77);
            this.pannelImageShow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pannelImageShow.Name = "pannelImageShow";
            this.pannelImageShow.Size = new System.Drawing.Size(827, 672);
            this.pannelImageShow.TabIndex = 6;
            this.pannelImageShow.Click += new System.EventHandler(this.pannelImageShow_Click);
            this.pannelImageShow.Paint += new System.Windows.Forms.PaintEventHandler(this.pannelImageShow_Paint);
            this.pannelImageShow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pannelImageShow_MouseClick);
            // 
            // chartSpectrum
            // 
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.Name = "ChartArea1";
            this.chartSpectrum.ChartAreas.Add(chartArea2);
            this.chartSpectrum.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartSpectrum.Location = new System.Drawing.Point(8, 8);
            this.chartSpectrum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chartSpectrum.Name = "chartSpectrum";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.IsVisibleInLegend = false;
            series2.Name = "Series1";
            this.chartSpectrum.Series.Add(series2);
            this.chartSpectrum.Size = new System.Drawing.Size(418, 435);
            this.chartSpectrum.TabIndex = 8;
            this.chartSpectrum.Text = "chart1";
            this.chartSpectrum.Click += new System.EventHandler(this.chartSpectrum_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblProgInfo);
            this.panel1.Controls.Add(this.progBar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Size = new System.Drawing.Size(1261, 77);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 749);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1261, 45);
            this.panel2.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnValidatePreview);
            this.panel3.Controls.Add(this.btnStoreSpectrum);
            this.panel3.Controls.Add(this.btnCreatePreview);
            this.panel3.Controls.Add(this.btnShowFile);
            this.panel3.Controls.Add(this.btnDoForier);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(778, 45);
            this.panel3.TabIndex = 0;
            // 
            // btnValidatePreview
            // 
            this.btnValidatePreview.Location = new System.Drawing.Point(622, 0);
            this.btnValidatePreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnValidatePreview.Name = "btnValidatePreview";
            this.btnValidatePreview.Size = new System.Drawing.Size(152, 45);
            this.btnValidatePreview.TabIndex = 14;
            this.btnValidatePreview.Text = "Validate preview";
            this.btnValidatePreview.UseVisualStyleBackColor = true;
            this.btnValidatePreview.Click += new System.EventHandler(this.btnValidatePreview_Click);
            // 
            // btnStoreSpectrum
            // 
            this.btnStoreSpectrum.Location = new System.Drawing.Point(386, 0);
            this.btnStoreSpectrum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStoreSpectrum.Name = "btnStoreSpectrum";
            this.btnStoreSpectrum.Size = new System.Drawing.Size(152, 45);
            this.btnStoreSpectrum.TabIndex = 13;
            this.btnStoreSpectrum.Text = "Store Spectrum";
            this.btnStoreSpectrum.UseVisualStyleBackColor = true;
            this.btnStoreSpectrum.Click += new System.EventHandler(this.btnStoreSpectrum_Click);
            // 
            // btnCreatePreview
            // 
            this.btnCreatePreview.Location = new System.Drawing.Point(225, 0);
            this.btnCreatePreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCreatePreview.Name = "btnCreatePreview";
            this.btnCreatePreview.Size = new System.Drawing.Size(152, 45);
            this.btnCreatePreview.TabIndex = 12;
            this.btnCreatePreview.Text = "Create preview";
            this.btnCreatePreview.UseVisualStyleBackColor = true;
            this.btnCreatePreview.Click += new System.EventHandler(this.btnCreatePreview_Click);
            // 
            // btnShowFile
            // 
            this.btnShowFile.Location = new System.Drawing.Point(112, 0);
            this.btnShowFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnShowFile.Name = "btnShowFile";
            this.btnShowFile.Size = new System.Drawing.Size(112, 45);
            this.btnShowFile.TabIndex = 11;
            this.btnShowFile.Text = "Show File";
            this.btnShowFile.UseVisualStyleBackColor = true;
            this.btnShowFile.Click += new System.EventHandler(this.btnShowFile_Click);
            // 
            // btnDoForier
            // 
            this.btnDoForier.Location = new System.Drawing.Point(0, 0);
            this.btnDoForier.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDoForier.Name = "btnDoForier";
            this.btnDoForier.Size = new System.Drawing.Size(112, 45);
            this.btnDoForier.TabIndex = 10;
            this.btnDoForier.Text = "Do Forier";
            this.btnDoForier.UseVisualStyleBackColor = true;
            this.btnDoForier.Click += new System.EventHandler(this.btnDoForier_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.chartSpectrum);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(827, 77);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.panel4.Size = new System.Drawing.Size(434, 672);
            this.panel4.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lblSelectedCalib);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.chkDoRgbPreview);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.btnLoadCalibration);
            this.panel5.Controls.Add(this.numEndWavelegnth);
            this.panel5.Controls.Add(this.numStartWavelength);
            this.panel5.Controls.Add(this.ddZeroFilling);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(8, 443);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(418, 219);
            this.panel5.TabIndex = 9;
            // 
            // chkDoRgbPreview
            // 
            this.chkDoRgbPreview.AutoSize = true;
            this.chkDoRgbPreview.Checked = true;
            this.chkDoRgbPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoRgbPreview.Location = new System.Drawing.Point(8, 86);
            this.chkDoRgbPreview.Name = "chkDoRgbPreview";
            this.chkDoRgbPreview.Size = new System.Drawing.Size(145, 24);
            this.chkDoRgbPreview.TabIndex = 7;
            this.chkDoRgbPreview.Text = "Preview in RGB";
            this.chkDoRgbPreview.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(190, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "-";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btnLoadCalibration
            // 
            this.btnLoadCalibration.Location = new System.Drawing.Point(302, 5);
            this.btnLoadCalibration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoadCalibration.Name = "btnLoadCalibration";
            this.btnLoadCalibration.Size = new System.Drawing.Size(112, 35);
            this.btnLoadCalibration.TabIndex = 5;
            this.btnLoadCalibration.Text = "Load Calib";
            this.btnLoadCalibration.UseVisualStyleBackColor = true;
            this.btnLoadCalibration.Click += new System.EventHandler(this.btnLoadCalibration_Click);
            // 
            // ddZeroFilling
            // 
            this.ddZeroFilling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddZeroFilling.FormattingEnabled = true;
            this.ddZeroFilling.Location = new System.Drawing.Point(122, 5);
            this.ddZeroFilling.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ddZeroFilling.Name = "ddZeroFilling";
            this.ddZeroFilling.Size = new System.Drawing.Size(109, 28);
            this.ddZeroFilling.TabIndex = 2;
            this.ddZeroFilling.SelectedIndexChanged += new System.EventHandler(this.ddZeroFilling_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Zero Filling:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Wavelengths";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Selected calibration:";
            // 
            // lblSelectedCalib
            // 
            this.lblSelectedCalib.ForeColor = System.Drawing.Color.Crimson;
            this.lblSelectedCalib.Location = new System.Drawing.Point(8, 152);
            this.lblSelectedCalib.Name = "lblSelectedCalib";
            this.lblSelectedCalib.Size = new System.Drawing.Size(395, 39);
            this.lblSelectedCalib.TabIndex = 9;
            this.lblSelectedCalib.Text = "[calibration data]";
            // 
            // numEndWavelegnth
            // 
            this.numEndWavelegnth.Location = new System.Drawing.Point(212, 48);
            this.numEndWavelegnth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numEndWavelegnth.MaxValue = 1.7976931348623157E+308D;
            this.numEndWavelegnth.MinValue = -1.7976931348623157E+308D;
            this.numEndWavelegnth.Name = "numEndWavelegnth";
            this.numEndWavelegnth.Size = new System.Drawing.Size(60, 26);
            this.numEndWavelegnth.TabIndex = 4;
            this.numEndWavelegnth.Text = "800";
            this.numEndWavelegnth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numEndWavelegnth.Units = 1D;
            this.numEndWavelegnth.Value = 800D;
            // 
            // numStartWavelength
            // 
            this.numStartWavelength.Location = new System.Drawing.Point(122, 48);
            this.numStartWavelength.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numStartWavelength.MaxValue = 1.7976931348623157E+308D;
            this.numStartWavelength.MinValue = -1.7976931348623157E+308D;
            this.numStartWavelength.Name = "numStartWavelength";
            this.numStartWavelength.Size = new System.Drawing.Size(61, 26);
            this.numStartWavelength.TabIndex = 3;
            this.numStartWavelength.Text = "400";
            this.numStartWavelength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStartWavelength.Units = 1D;
            this.numStartWavelength.Value = 400D;
            // 
            // DoForierTransformDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 794);
            this.Controls.Add(this.pannelImageShow);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DoForierTransformDlg";
            this.Text = "DoForierTransform";
            this.SizeChanged += new System.EventHandler(this.DoForierTransformDlg_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.chartSpectrum)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progBar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblProgInfo;
        private System.Windows.Forms.Panel pannelImageShow;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSpectrum;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnStoreSpectrum;
        private System.Windows.Forms.Button btnCreatePreview;
        private System.Windows.Forms.Button btnShowFile;
        private System.Windows.Forms.Button btnDoForier;
        private System.Windows.Forms.Button btnValidatePreview;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox ddZeroFilling;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadCalibration;
        private GSIControls.General.NumericControl numEndWavelegnth;
        private GSIControls.General.NumericControl numStartWavelength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkDoRgbPreview;
        private System.Windows.Forms.Label lblSelectedCalib;
        private System.Windows.Forms.Label label4;
    }
}