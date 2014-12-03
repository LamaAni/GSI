namespace TestRun
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpCommands = new System.Windows.Forms.GroupBox();
            this.btnCalibImageAndStage = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.numScanSpeed = new GSIControls.General.NumericControl();
            this.numStageAngle = new GSIControls.General.NumericControl();
            this.label8 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.numExp = new GSIControls.General.NumericControl();
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.numMaxFrameRate = new GSIControls.General.NumericControl();
            this.label13 = new System.Windows.Forms.Label();
            this.numFrameRate = new GSIControls.General.NumericControl();
            this.label12 = new System.Windows.Forms.Label();
            this.numDx = new GSIControls.General.NumericControl();
            this.btnCalcBest = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.numPixelSize = new GSIControls.General.NumericControl();
            this.numGotoYPos = new GSIControls.General.NumericControl();
            this.numGotoXPos = new GSIControls.General.NumericControl();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnGoto = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnStageSet00 = new System.Windows.Forms.Button();
            this.btnTogglePreview = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDoForierTransform = new System.Windows.Forms.Button();
            this.btnDoReconstructImage = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numEndYPix = new GSIControls.General.NumericControl();
            this.numEndXPix = new GSIControls.General.NumericControl();
            this.numStartYPix = new GSIControls.General.NumericControl();
            this.numStartXPix = new GSIControls.General.NumericControl();
            this.chkAskWhereToCapture = new System.Windows.Forms.CheckBox();
            this.chkDoSpeedOffset = new System.Windows.Forms.CheckBox();
            this.chkInvertedImage = new System.Windows.Forms.CheckBox();
            this.ddScanOver = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnCur1 = new System.Windows.Forms.Button();
            this.btnCur0 = new System.Windows.Forms.Button();
            this.numY1 = new GSIControls.General.NumericControl();
            this.numX1 = new GSIControls.General.NumericControl();
            this.label6 = new System.Windows.Forms.Label();
            this.numY0 = new GSIControls.General.NumericControl();
            this.numX0 = new GSIControls.General.NumericControl();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCapture = new System.Windows.Forms.Button();
            this.lblStagePos = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.barProg = new System.Windows.Forms.ProgressBar();
            this.prevPannel = new GSIControls.Camera.PreviewDisplayPannel();
            this.panel1.SuspendLayout();
            this.grpCommands.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpCommands);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Controls.Add(this.btnDoForierTransform);
            this.panel1.Controls.Add(this.btnDoReconstructImage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 596);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // grpCommands
            // 
            this.grpCommands.Controls.Add(this.btnCalibImageAndStage);
            this.grpCommands.Controls.Add(this.btnSaveSettings);
            this.grpCommands.Controls.Add(this.label10);
            this.grpCommands.Controls.Add(this.numScanSpeed);
            this.grpCommands.Controls.Add(this.numStageAngle);
            this.grpCommands.Controls.Add(this.label8);
            this.grpCommands.Controls.Add(this.label16);
            this.grpCommands.Controls.Add(this.numExp);
            this.grpCommands.Controls.Add(this.btnSnapshot);
            this.grpCommands.Controls.Add(this.label14);
            this.grpCommands.Controls.Add(this.numMaxFrameRate);
            this.grpCommands.Controls.Add(this.label13);
            this.grpCommands.Controls.Add(this.numFrameRate);
            this.grpCommands.Controls.Add(this.label12);
            this.grpCommands.Controls.Add(this.numDx);
            this.grpCommands.Controls.Add(this.btnCalcBest);
            this.grpCommands.Controls.Add(this.label9);
            this.grpCommands.Controls.Add(this.numPixelSize);
            this.grpCommands.Controls.Add(this.numGotoYPos);
            this.grpCommands.Controls.Add(this.numGotoXPos);
            this.grpCommands.Controls.Add(this.label5);
            this.grpCommands.Controls.Add(this.label7);
            this.grpCommands.Controls.Add(this.btnGoto);
            this.grpCommands.Controls.Add(this.button2);
            this.grpCommands.Controls.Add(this.btnStageSet00);
            this.grpCommands.Controls.Add(this.btnTogglePreview);
            this.grpCommands.Controls.Add(this.btnSettings);
            this.grpCommands.Location = new System.Drawing.Point(12, 94);
            this.grpCommands.Name = "grpCommands";
            this.grpCommands.Size = new System.Drawing.Size(181, 446);
            this.grpCommands.TabIndex = 1;
            this.grpCommands.TabStop = false;
            this.grpCommands.Text = "Commands";
            // 
            // btnCalibImageAndStage
            // 
            this.btnCalibImageAndStage.Location = new System.Drawing.Point(6, 417);
            this.btnCalibImageAndStage.Name = "btnCalibImageAndStage";
            this.btnCalibImageAndStage.Size = new System.Drawing.Size(168, 23);
            this.btnCalibImageAndStage.TabIndex = 40;
            this.btnCalibImageAndStage.Text = "Calibrate Image && Stage";
            this.btnCalibImageAndStage.UseVisualStyleBackColor = true;
            this.btnCalibImageAndStage.Click += new System.EventHandler(this.btnCalibImageAndStage_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(115, 15);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(59, 23);
            this.btnSaveSettings.TabIndex = 39;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 325);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Scan speed";
            // 
            // numScanSpeed
            // 
            this.numScanSpeed.Enabled = false;
            this.numScanSpeed.Location = new System.Drawing.Point(81, 322);
            this.numScanSpeed.MaxValue = 1.7976931348623157E+308D;
            this.numScanSpeed.MinValue = -1.7976931348623157E+308D;
            this.numScanSpeed.Name = "numScanSpeed";
            this.numScanSpeed.Size = new System.Drawing.Size(93, 20);
            this.numScanSpeed.TabIndex = 37;
            this.numScanSpeed.Text = "0";
            this.numScanSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numScanSpeed.Units = 1D;
            this.numScanSpeed.Value = 0D;
            // 
            // numStageAngle
            // 
            this.numStageAngle.Location = new System.Drawing.Point(83, 80);
            this.numStageAngle.MaxValue = 1.7976931348623157E+308D;
            this.numStageAngle.MinValue = -1.7976931348623157E+308D;
            this.numStageAngle.Name = "numStageAngle";
            this.numStageAngle.Size = new System.Drawing.Size(92, 20);
            this.numStageAngle.TabIndex = 23;
            this.numStageAngle.Text = "1.55980775811216";
            this.numStageAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStageAngle.Units = 1D;
            this.numStageAngle.Value = 1.55980775811216D;
            this.numStageAngle.TextChanged += new System.EventHandler(this.numStageAngle_TextChanged);
            this.numStageAngle.Validated += new System.EventHandler(this.numStageAngle_Validated);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 270);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Exp. [ms]";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 83);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(62, 13);
            this.label16.TabIndex = 22;
            this.label16.Text = "StageAngle";
            // 
            // numExp
            // 
            this.numExp.Location = new System.Drawing.Point(81, 267);
            this.numExp.MaxValue = 1.7976931348623157E+308D;
            this.numExp.MinValue = -1.7976931348623157E+308D;
            this.numExp.Name = "numExp";
            this.numExp.Size = new System.Drawing.Size(93, 20);
            this.numExp.TabIndex = 35;
            this.numExp.Text = "0.1";
            this.numExp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numExp.Units = 1D;
            this.numExp.Value = 0.1D;
            this.numExp.Validated += new System.EventHandler(this.numExp_Validated);
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Location = new System.Drawing.Point(6, 388);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(168, 23);
            this.btnSnapshot.TabIndex = 22;
            this.btnSnapshot.Text = "Snapshot";
            this.btnSnapshot.UseVisualStyleBackColor = true;
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 244);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Max f. rate";
            // 
            // numMaxFrameRate
            // 
            this.numMaxFrameRate.Location = new System.Drawing.Point(81, 241);
            this.numMaxFrameRate.MaxValue = 1.7976931348623157E+308D;
            this.numMaxFrameRate.MinValue = -1.7976931348623157E+308D;
            this.numMaxFrameRate.Name = "numMaxFrameRate";
            this.numMaxFrameRate.Size = new System.Drawing.Size(93, 20);
            this.numMaxFrameRate.TabIndex = 28;
            this.numMaxFrameRate.Text = "100";
            this.numMaxFrameRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMaxFrameRate.Units = 1D;
            this.numMaxFrameRate.Value = 100D;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 299);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "Frame rate";
            // 
            // numFrameRate
            // 
            this.numFrameRate.Enabled = false;
            this.numFrameRate.Location = new System.Drawing.Point(81, 296);
            this.numFrameRate.MaxValue = 1.7976931348623157E+308D;
            this.numFrameRate.MinValue = -1.7976931348623157E+308D;
            this.numFrameRate.Name = "numFrameRate";
            this.numFrameRate.Size = new System.Drawing.Size(93, 20);
            this.numFrameRate.TabIndex = 26;
            this.numFrameRate.Text = "0";
            this.numFrameRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFrameRate.Units = 1D;
            this.numFrameRate.Value = 0D;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 218);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "dx";
            // 
            // numDx
            // 
            this.numDx.Location = new System.Drawing.Point(81, 215);
            this.numDx.MaxValue = 100000D;
            this.numDx.MinValue = 1D;
            this.numDx.Name = "numDx";
            this.numDx.Size = new System.Drawing.Size(93, 20);
            this.numDx.TabIndex = 24;
            this.numDx.Text = "14";
            this.numDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDx.Units = 1D;
            this.numDx.Value = 14D;
            // 
            // btnCalcBest
            // 
            this.btnCalcBest.Location = new System.Drawing.Point(6, 354);
            this.btnCalcBest.Name = "btnCalcBest";
            this.btnCalcBest.Size = new System.Drawing.Size(168, 23);
            this.btnCalcBest.TabIndex = 21;
            this.btnCalcBest.Text = "Calculate Best";
            this.btnCalcBest.UseVisualStyleBackColor = true;
            this.btnCalcBest.Click += new System.EventHandler(this.btnCalcBest_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 192);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Img. Pix [um]";
            // 
            // numPixelSize
            // 
            this.numPixelSize.Location = new System.Drawing.Point(81, 189);
            this.numPixelSize.MaxValue = 1.7976931348623157E+308D;
            this.numPixelSize.MinValue = -1.7976931348623157E+308D;
            this.numPixelSize.Name = "numPixelSize";
            this.numPixelSize.Size = new System.Drawing.Size(93, 20);
            this.numPixelSize.TabIndex = 19;
            this.numPixelSize.Text = "0.274708688540291";
            this.numPixelSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPixelSize.Units = 1D;
            this.numPixelSize.Value = 0.274708688540291D;
            this.numPixelSize.TextChanged += new System.EventHandler(this.numPixelSize_TextChanged);
            this.numPixelSize.Validated += new System.EventHandler(this.numPixelSize_Validated);
            // 
            // numGotoYPos
            // 
            this.numGotoYPos.Location = new System.Drawing.Point(98, 161);
            this.numGotoYPos.MaxValue = 1.7976931348623157E+308D;
            this.numGotoYPos.MinValue = -1.7976931348623157E+308D;
            this.numGotoYPos.Name = "numGotoYPos";
            this.numGotoYPos.Size = new System.Drawing.Size(76, 20);
            this.numGotoYPos.TabIndex = 16;
            this.numGotoYPos.Text = "0";
            this.numGotoYPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGotoYPos.Units = 1D;
            this.numGotoYPos.Value = 0D;
            // 
            // numGotoXPos
            // 
            this.numGotoXPos.Location = new System.Drawing.Point(98, 135);
            this.numGotoXPos.MaxValue = 1.7976931348623157E+308D;
            this.numGotoXPos.MinValue = -1.7976931348623157E+308D;
            this.numGotoXPos.Name = "numGotoXPos";
            this.numGotoXPos.Size = new System.Drawing.Size(76, 20);
            this.numGotoXPos.TabIndex = 15;
            this.numGotoXPos.Text = "0";
            this.numGotoXPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGotoXPos.Units = 1D;
            this.numGotoXPos.Value = 0D;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(78, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "X";
            // 
            // btnGoto
            // 
            this.btnGoto.Location = new System.Drawing.Point(8, 132);
            this.btnGoto.Name = "btnGoto";
            this.btnGoto.Size = new System.Drawing.Size(59, 23);
            this.btnGoto.TabIndex = 12;
            this.btnGoto.Text = "Goto";
            this.btnGoto.UseVisualStyleBackColor = true;
            this.btnGoto.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 106);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Goto 00";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnStageSet00
            // 
            this.btnStageSet00.Location = new System.Drawing.Point(115, 106);
            this.btnStageSet00.Name = "btnStageSet00";
            this.btnStageSet00.Size = new System.Drawing.Size(59, 23);
            this.btnStageSet00.TabIndex = 10;
            this.btnStageSet00.Text = "Set 00";
            this.btnStageSet00.UseVisualStyleBackColor = true;
            this.btnStageSet00.Click += new System.EventHandler(this.btnStageSet00_Click);
            // 
            // btnTogglePreview
            // 
            this.btnTogglePreview.Location = new System.Drawing.Point(6, 44);
            this.btnTogglePreview.Name = "btnTogglePreview";
            this.btnTogglePreview.Size = new System.Drawing.Size(169, 23);
            this.btnTogglePreview.TabIndex = 9;
            this.btnTogglePreview.Text = "Preview";
            this.btnTogglePreview.UseVisualStyleBackColor = true;
            this.btnTogglePreview.Click += new System.EventHandler(this.btnTogglePreview_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(6, 15);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(106, 23);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(181, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDoForierTransform
            // 
            this.btnDoForierTransform.BackColor = System.Drawing.Color.Salmon;
            this.btnDoForierTransform.Location = new System.Drawing.Point(19, 41);
            this.btnDoForierTransform.Name = "btnDoForierTransform";
            this.btnDoForierTransform.Size = new System.Drawing.Size(167, 23);
            this.btnDoForierTransform.TabIndex = 24;
            this.btnDoForierTransform.Text = "Do Forier Transofrm";
            this.btnDoForierTransform.UseVisualStyleBackColor = false;
            this.btnDoForierTransform.Click += new System.EventHandler(this.btnDoForierTransform_Click);
            // 
            // btnDoReconstructImage
            // 
            this.btnDoReconstructImage.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnDoReconstructImage.Location = new System.Drawing.Point(20, 65);
            this.btnDoReconstructImage.Name = "btnDoReconstructImage";
            this.btnDoReconstructImage.Size = new System.Drawing.Size(167, 23);
            this.btnDoReconstructImage.TabIndex = 32;
            this.btnDoReconstructImage.Text = "Do Reconstruct Image";
            this.btnDoReconstructImage.UseVisualStyleBackColor = false;
            this.btnDoReconstructImage.Click += new System.EventHandler(this.btnDoReconstructImage_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numEndYPix);
            this.panel2.Controls.Add(this.numEndXPix);
            this.panel2.Controls.Add(this.numStartYPix);
            this.panel2.Controls.Add(this.numStartXPix);
            this.panel2.Controls.Add(this.chkAskWhereToCapture);
            this.panel2.Controls.Add(this.chkDoSpeedOffset);
            this.panel2.Controls.Add(this.chkInvertedImage);
            this.panel2.Controls.Add(this.ddScanOver);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.btnCur1);
            this.panel2.Controls.Add(this.btnCur0);
            this.panel2.Controls.Add(this.numY1);
            this.panel2.Controls.Add(this.numX1);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.numY0);
            this.panel2.Controls.Add(this.numX0);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnCapture);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(200, 492);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(672, 104);
            this.panel2.TabIndex = 2;
            // 
            // numEndYPix
            // 
            this.numEndYPix.Location = new System.Drawing.Point(185, 53);
            this.numEndYPix.MaxValue = 1.7976931348623157E+308D;
            this.numEndYPix.MinValue = -1.7976931348623157E+308D;
            this.numEndYPix.Name = "numEndYPix";
            this.numEndYPix.ReadOnly = true;
            this.numEndYPix.Size = new System.Drawing.Size(47, 20);
            this.numEndYPix.TabIndex = 27;
            this.numEndYPix.Text = "200";
            this.numEndYPix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numEndYPix.Units = 1D;
            this.numEndYPix.Value = 200D;
            // 
            // numEndXPix
            // 
            this.numEndXPix.Location = new System.Drawing.Point(185, 27);
            this.numEndXPix.MaxValue = 1.7976931348623157E+308D;
            this.numEndXPix.MinValue = -1.7976931348623157E+308D;
            this.numEndXPix.Name = "numEndXPix";
            this.numEndXPix.ReadOnly = true;
            this.numEndXPix.Size = new System.Drawing.Size(47, 20);
            this.numEndXPix.TabIndex = 26;
            this.numEndXPix.Text = "200";
            this.numEndXPix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numEndXPix.Units = 1D;
            this.numEndXPix.Value = 200D;
            // 
            // numStartYPix
            // 
            this.numStartYPix.Location = new System.Drawing.Point(79, 53);
            this.numStartYPix.MaxValue = 1.7976931348623157E+308D;
            this.numStartYPix.MinValue = -1.7976931348623157E+308D;
            this.numStartYPix.Name = "numStartYPix";
            this.numStartYPix.ReadOnly = true;
            this.numStartYPix.Size = new System.Drawing.Size(47, 20);
            this.numStartYPix.TabIndex = 25;
            this.numStartYPix.Text = "200";
            this.numStartYPix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStartYPix.Units = 1D;
            this.numStartYPix.Value = 200D;
            // 
            // numStartXPix
            // 
            this.numStartXPix.Location = new System.Drawing.Point(79, 27);
            this.numStartXPix.MaxValue = 1.7976931348623157E+308D;
            this.numStartXPix.MinValue = -1.7976931348623157E+308D;
            this.numStartXPix.Name = "numStartXPix";
            this.numStartXPix.ReadOnly = true;
            this.numStartXPix.Size = new System.Drawing.Size(47, 20);
            this.numStartXPix.TabIndex = 24;
            this.numStartXPix.Text = "200";
            this.numStartXPix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStartXPix.Units = 1D;
            this.numStartXPix.Value = 200D;
            // 
            // chkAskWhereToCapture
            // 
            this.chkAskWhereToCapture.Checked = true;
            this.chkAskWhereToCapture.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAskWhereToCapture.Location = new System.Drawing.Point(465, 70);
            this.chkAskWhereToCapture.Name = "chkAskWhereToCapture";
            this.chkAskWhereToCapture.Size = new System.Drawing.Size(142, 25);
            this.chkAskWhereToCapture.TabIndex = 23;
            this.chkAskWhereToCapture.Text = "Save to specific file";
            this.chkAskWhereToCapture.UseVisualStyleBackColor = true;
            // 
            // chkDoSpeedOffset
            // 
            this.chkDoSpeedOffset.Checked = true;
            this.chkDoSpeedOffset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoSpeedOffset.Location = new System.Drawing.Point(319, 30);
            this.chkDoSpeedOffset.Name = "chkDoSpeedOffset";
            this.chkDoSpeedOffset.Size = new System.Drawing.Size(142, 25);
            this.chkDoSpeedOffset.TabIndex = 22;
            this.chkDoSpeedOffset.Text = "Speedup befor capture";
            this.chkDoSpeedOffset.UseVisualStyleBackColor = true;
            // 
            // chkInvertedImage
            // 
            this.chkInvertedImage.Location = new System.Drawing.Point(319, 9);
            this.chkInvertedImage.Name = "chkInvertedImage";
            this.chkInvertedImage.Size = new System.Drawing.Size(140, 24);
            this.chkInvertedImage.TabIndex = 20;
            this.chkInvertedImage.Text = "Inverted Image";
            this.chkInvertedImage.UseVisualStyleBackColor = true;
            // 
            // ddScanOver
            // 
            this.ddScanOver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddScanOver.FormattingEnabled = true;
            this.ddScanOver.Items.AddRange(new object[] {
            "X",
            "Y"});
            this.ddScanOver.Location = new System.Drawing.Point(260, 27);
            this.ddScanOver.Name = "ddScanOver";
            this.ddScanOver.Size = new System.Drawing.Size(53, 21);
            this.ddScanOver.TabIndex = 19;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(257, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "Scan over";
            // 
            // btnCur1
            // 
            this.btnCur1.Location = new System.Drawing.Point(131, 78);
            this.btnCur1.Name = "btnCur1";
            this.btnCur1.Size = new System.Drawing.Size(48, 23);
            this.btnCur1.TabIndex = 15;
            this.btnCur1.Text = "Cur";
            this.btnCur1.UseVisualStyleBackColor = true;
            this.btnCur1.Click += new System.EventHandler(this.btnCur1_Click);
            // 
            // btnCur0
            // 
            this.btnCur0.Location = new System.Drawing.Point(26, 78);
            this.btnCur0.Name = "btnCur0";
            this.btnCur0.Size = new System.Drawing.Size(46, 23);
            this.btnCur0.TabIndex = 14;
            this.btnCur0.Text = "Cur";
            this.btnCur0.UseVisualStyleBackColor = true;
            this.btnCur0.Click += new System.EventHandler(this.btnCur0_Click);
            // 
            // numY1
            // 
            this.numY1.Location = new System.Drawing.Point(132, 53);
            this.numY1.MaxValue = 1.7976931348623157E+308D;
            this.numY1.MinValue = -1.7976931348623157E+308D;
            this.numY1.Name = "numY1";
            this.numY1.Size = new System.Drawing.Size(47, 20);
            this.numY1.TabIndex = 10;
            this.numY1.Text = "200";
            this.numY1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numY1.Units = 1D;
            this.numY1.Value = 200D;
            this.numY1.Validated += new System.EventHandler(this.numX1_Validated);
            // 
            // numX1
            // 
            this.numX1.Location = new System.Drawing.Point(132, 27);
            this.numX1.MaxValue = 1.7976931348623157E+308D;
            this.numX1.MinValue = -1.7976931348623157E+308D;
            this.numX1.Name = "numX1";
            this.numX1.Size = new System.Drawing.Size(47, 20);
            this.numX1.TabIndex = 9;
            this.numX1.Text = "200";
            this.numX1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numX1.Units = 1D;
            this.numX1.Value = 200D;
            this.numX1.Validated += new System.EventHandler(this.numX1_Validated);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(129, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "End pos";
            // 
            // numY0
            // 
            this.numY0.Location = new System.Drawing.Point(26, 53);
            this.numY0.MaxValue = 1.7976931348623157E+308D;
            this.numY0.MinValue = -1.7976931348623157E+308D;
            this.numY0.Name = "numY0";
            this.numY0.Size = new System.Drawing.Size(47, 20);
            this.numY0.TabIndex = 7;
            this.numY0.Text = "0";
            this.numY0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numY0.Units = 1D;
            this.numY0.Value = 0D;
            this.numY0.Validated += new System.EventHandler(this.numY0_Validated);
            // 
            // numX0
            // 
            this.numX0.Location = new System.Drawing.Point(26, 27);
            this.numX0.MaxValue = 1.7976931348623157E+308D;
            this.numX0.MinValue = -1.7976931348623157E+308D;
            this.numX0.Name = "numX0";
            this.numX0.Size = new System.Drawing.Size(47, 20);
            this.numX0.TabIndex = 6;
            this.numX0.Text = "0";
            this.numX0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numX0.Units = 1D;
            this.numX0.Value = 0D;
            this.numX0.Validated += new System.EventHandler(this.numX0_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start pos";
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(465, 9);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(195, 55);
            this.btnCapture.TabIndex = 0;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblStagePos
            // 
            this.lblStagePos.AutoSize = true;
            this.lblStagePos.BackColor = System.Drawing.Color.Black;
            this.lblStagePos.ForeColor = System.Drawing.Color.White;
            this.lblStagePos.Location = new System.Drawing.Point(210, 36);
            this.lblStagePos.Name = "lblStagePos";
            this.lblStagePos.Size = new System.Drawing.Size(62, 13);
            this.lblStagePos.TabIndex = 3;
            this.lblStagePos.Text = "[Stage Pos]";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(6, 6);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "label5";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblStatus);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(200, 467);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(6);
            this.panel3.Size = new System.Drawing.Size(672, 25);
            this.panel3.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.barProg);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(200, 441);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(672, 26);
            this.panel4.TabIndex = 5;
            // 
            // barProg
            // 
            this.barProg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barProg.Location = new System.Drawing.Point(0, 0);
            this.barProg.MarqueeAnimationSpeed = 1;
            this.barProg.Name = "barProg";
            this.barProg.Size = new System.Drawing.Size(672, 26);
            this.barProg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.barProg.TabIndex = 0;
            // 
            // prevPannel
            // 
            this.prevPannel.BackColor = System.Drawing.Color.White;
            this.prevPannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevPannel.DrawFrameRate = true;
            this.prevPannel.KeepAspectRatio = true;
            this.prevPannel.Location = new System.Drawing.Point(200, 0);
            this.prevPannel.Name = "prevPannel";
            this.prevPannel.Size = new System.Drawing.Size(672, 441);
            this.prevPannel.TabIndex = 0;
            this.prevPannel.Text = "prevPannel";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 596);
            this.Controls.Add(this.lblStagePos);
            this.Controls.Add(this.prevPannel);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Main";
            this.Text = "TestRunner";
            this.panel1.ResumeLayout(false);
            this.grpCommands.ResumeLayout(false);
            this.grpCommands.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GSIControls.Camera.PreviewDisplayPannel prevPannel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox grpCommands;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnTogglePreview;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnCur1;
        private System.Windows.Forms.Button btnCur0;
        private GSIControls.General.NumericControl numX1;
        private System.Windows.Forms.Label label6;
        private GSIControls.General.NumericControl numX0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStagePos;
        private System.Windows.Forms.Button btnStageSet00;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnGoto;
        private GSIControls.General.NumericControl numGotoXPos;
        private GSIControls.General.NumericControl numGotoYPos;
        private System.Windows.Forms.Label label9;
        private GSIControls.General.NumericControl numPixelSize;
        private System.Windows.Forms.Button btnCalcBest;
        private System.Windows.Forms.Label label12;
        private GSIControls.General.NumericControl numDx;
        private System.Windows.Forms.Label label13;
        private GSIControls.General.NumericControl numFrameRate;
        private System.Windows.Forms.Label label14;
        private GSIControls.General.NumericControl numMaxFrameRate;
        private System.Windows.Forms.ComboBox ddScanOver;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox chkInvertedImage;
        private System.Windows.Forms.Button btnDoForierTransform;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnDoReconstructImage;
        private System.Windows.Forms.ProgressBar barProg;
        private System.Windows.Forms.Button btnSnapshot;
        private System.Windows.Forms.Label label16;
        private GSIControls.General.NumericControl numStageAngle;
        private System.Windows.Forms.Label label8;
        private GSIControls.General.NumericControl numExp;
        private GSIControls.General.NumericControl numY1;
        private GSIControls.General.NumericControl numY0;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkDoSpeedOffset;
        private System.Windows.Forms.Label label10;
        private GSIControls.General.NumericControl numScanSpeed;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.CheckBox chkAskWhereToCapture;
        private GSIControls.General.NumericControl numEndYPix;
        private GSIControls.General.NumericControl numEndXPix;
        private GSIControls.General.NumericControl numStartYPix;
        private GSIControls.General.NumericControl numStartXPix;
        private System.Windows.Forms.Button btnCalibImageAndStage;
    }
}

