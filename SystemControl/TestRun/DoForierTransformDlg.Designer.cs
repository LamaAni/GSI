namespace TestRun
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ddZeroFilling = new System.Windows.Forms.ComboBox();
            this.numValidStartIndex = new GSIControls.General.NumericControl();
            this.numValidEndIndex = new GSIControls.General.NumericControl();
            this.btnLoadCalibration = new System.Windows.Forms.Button();
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
            this.progBar.Location = new System.Drawing.Point(3, 3);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(832, 23);
            this.progBar.TabIndex = 0;
            this.progBar.Click += new System.EventHandler(this.progBar_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(763, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblProgInfo
            // 
            this.lblProgInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblProgInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProgInfo.Location = new System.Drawing.Point(3, 26);
            this.lblProgInfo.Name = "lblProgInfo";
            this.lblProgInfo.Size = new System.Drawing.Size(832, 24);
            this.lblProgInfo.TabIndex = 3;
            this.lblProgInfo.Text = "[prog]";
            this.lblProgInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgInfo.Click += new System.EventHandler(this.lblProgInfo_Click);
            // 
            // pannelImageShow
            // 
            this.pannelImageShow.AutoSize = true;
            this.pannelImageShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pannelImageShow.Location = new System.Drawing.Point(0, 50);
            this.pannelImageShow.Name = "pannelImageShow";
            this.pannelImageShow.Size = new System.Drawing.Size(549, 427);
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
            this.chartSpectrum.Location = new System.Drawing.Point(5, 5);
            this.chartSpectrum.Name = "chartSpectrum";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.IsVisibleInLegend = false;
            series2.Name = "Series1";
            this.chartSpectrum.Series.Add(series2);
            this.chartSpectrum.Size = new System.Drawing.Size(279, 283);
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
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(838, 50);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 477);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(838, 29);
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
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(519, 29);
            this.panel3.TabIndex = 0;
            // 
            // btnValidatePreview
            // 
            this.btnValidatePreview.Location = new System.Drawing.Point(415, 0);
            this.btnValidatePreview.Name = "btnValidatePreview";
            this.btnValidatePreview.Size = new System.Drawing.Size(101, 29);
            this.btnValidatePreview.TabIndex = 14;
            this.btnValidatePreview.Text = "Validate preview";
            this.btnValidatePreview.UseVisualStyleBackColor = true;
            this.btnValidatePreview.Click += new System.EventHandler(this.btnValidatePreview_Click);
            // 
            // btnStoreSpectrum
            // 
            this.btnStoreSpectrum.Location = new System.Drawing.Point(257, 0);
            this.btnStoreSpectrum.Name = "btnStoreSpectrum";
            this.btnStoreSpectrum.Size = new System.Drawing.Size(101, 29);
            this.btnStoreSpectrum.TabIndex = 13;
            this.btnStoreSpectrum.Text = "Store Spectrum";
            this.btnStoreSpectrum.UseVisualStyleBackColor = true;
            this.btnStoreSpectrum.Click += new System.EventHandler(this.btnStoreSpectrum_Click);
            // 
            // btnCreatePreview
            // 
            this.btnCreatePreview.Location = new System.Drawing.Point(150, 0);
            this.btnCreatePreview.Name = "btnCreatePreview";
            this.btnCreatePreview.Size = new System.Drawing.Size(101, 29);
            this.btnCreatePreview.TabIndex = 12;
            this.btnCreatePreview.Text = "Create preview";
            this.btnCreatePreview.UseVisualStyleBackColor = true;
            this.btnCreatePreview.Click += new System.EventHandler(this.btnCreatePreview_Click);
            // 
            // btnShowFile
            // 
            this.btnShowFile.Location = new System.Drawing.Point(75, 0);
            this.btnShowFile.Name = "btnShowFile";
            this.btnShowFile.Size = new System.Drawing.Size(75, 29);
            this.btnShowFile.TabIndex = 11;
            this.btnShowFile.Text = "Show File";
            this.btnShowFile.UseVisualStyleBackColor = true;
            this.btnShowFile.Click += new System.EventHandler(this.btnShowFile_Click);
            // 
            // btnDoForier
            // 
            this.btnDoForier.Location = new System.Drawing.Point(0, 0);
            this.btnDoForier.Name = "btnDoForier";
            this.btnDoForier.Size = new System.Drawing.Size(75, 29);
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
            this.panel4.Location = new System.Drawing.Point(549, 50);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(5);
            this.panel4.Size = new System.Drawing.Size(289, 427);
            this.panel4.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnLoadCalibration);
            this.panel5.Controls.Add(this.numValidEndIndex);
            this.panel5.Controls.Add(this.numValidStartIndex);
            this.panel5.Controls.Add(this.ddZeroFilling);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(5, 288);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(279, 79);
            this.panel5.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Valid Indixies";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Zero Filling:";
            // 
            // ddZeroFilling
            // 
            this.ddZeroFilling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddZeroFilling.FormattingEnabled = true;
            this.ddZeroFilling.Items.AddRange(new object[] {
            "64",
            "128",
            "256",
            "512"});
            this.ddZeroFilling.Location = new System.Drawing.Point(81, 3);
            this.ddZeroFilling.Name = "ddZeroFilling";
            this.ddZeroFilling.Size = new System.Drawing.Size(74, 21);
            this.ddZeroFilling.TabIndex = 2;
            // 
            // numValidStartIndex
            // 
            this.numValidStartIndex.Location = new System.Drawing.Point(81, 31);
            this.numValidStartIndex.MaxValue = 1.7976931348623157E+308D;
            this.numValidStartIndex.MinValue = -1.7976931348623157E+308D;
            this.numValidStartIndex.Name = "numValidStartIndex";
            this.numValidStartIndex.Size = new System.Drawing.Size(42, 20);
            this.numValidStartIndex.TabIndex = 3;
            this.numValidStartIndex.Text = "0";
            this.numValidStartIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numValidStartIndex.Units = 1D;
            this.numValidStartIndex.Value = 0D;
            // 
            // numValidEndIndex
            // 
            this.numValidEndIndex.Location = new System.Drawing.Point(129, 31);
            this.numValidEndIndex.MaxValue = 1.7976931348623157E+308D;
            this.numValidEndIndex.MinValue = -1.7976931348623157E+308D;
            this.numValidEndIndex.Name = "numValidEndIndex";
            this.numValidEndIndex.Size = new System.Drawing.Size(47, 20);
            this.numValidEndIndex.TabIndex = 4;
            this.numValidEndIndex.Text = "256";
            this.numValidEndIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numValidEndIndex.Units = 1D;
            this.numValidEndIndex.Value = 256D;
            // 
            // btnLoadCalibration
            // 
            this.btnLoadCalibration.Location = new System.Drawing.Point(182, 30);
            this.btnLoadCalibration.Name = "btnLoadCalibration";
            this.btnLoadCalibration.Size = new System.Drawing.Size(75, 23);
            this.btnLoadCalibration.TabIndex = 5;
            this.btnLoadCalibration.Text = "Load Calib";
            this.btnLoadCalibration.UseVisualStyleBackColor = true;
            // 
            // DoForierTransformDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 506);
            this.Controls.Add(this.pannelImageShow);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        private GSIControls.General.NumericControl numValidEndIndex;
        private GSIControls.General.NumericControl numValidStartIndex;
    }
}