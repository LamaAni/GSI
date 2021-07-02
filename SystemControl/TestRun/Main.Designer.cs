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
            this.stageControl = new GSIControls.Stage.StageControlBox();
            this.scanParameters = new GSIControls.Stage.ScanImageParameters();
            this.btnCalibImageAndStage = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.btnTogglePreview = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDoForierTransform = new System.Windows.Forms.Button();
            this.btnDoReconstructImage = new System.Windows.Forms.Button();
            this.pnlScanControl = new System.Windows.Forms.Panel();
            this.scanRange = new GSIControls.Stage.ScanRangeBox();
            this.chkAskWhereToCapture = new System.Windows.Forms.CheckBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.lblStagePos = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.barProg = new System.Windows.Forms.ProgressBar();
            this.prevPannel = new GSIControls.Camera.PreviewDisplayPannel();
            this.panel1.SuspendLayout();
            this.grpCommands.SuspendLayout();
            this.pnlScanControl.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(215, 596);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // grpCommands
            // 
            this.grpCommands.Controls.Add(this.stageControl);
            this.grpCommands.Controls.Add(this.scanParameters);
            this.grpCommands.Controls.Add(this.btnCalibImageAndStage);
            this.grpCommands.Controls.Add(this.btnSaveSettings);
            this.grpCommands.Controls.Add(this.btnSnapshot);
            this.grpCommands.Controls.Add(this.btnTogglePreview);
            this.grpCommands.Controls.Add(this.btnSettings);
            this.grpCommands.Location = new System.Drawing.Point(12, 94);
            this.grpCommands.Name = "grpCommands";
            this.grpCommands.Size = new System.Drawing.Size(197, 479);
            this.grpCommands.TabIndex = 1;
            this.grpCommands.TabStop = false;
            this.grpCommands.Text = "Commands";
            // 
            // stageControl
            // 
            this.stageControl.Location = new System.Drawing.Point(14, 72);
            this.stageControl.Margin = new System.Windows.Forms.Padding(1);
            this.stageControl.Name = "stageControl";
            this.stageControl.Size = new System.Drawing.Size(167, 120);
            this.stageControl.TabIndex = 42;
            this.stageControl.DataChanged += new System.EventHandler(this.stageControl_DataChanged);
            // 
            // scanParameters
            // 
            this.scanParameters.Location = new System.Drawing.Point(9, 194);
            this.scanParameters.Margin = new System.Windows.Forms.Padding(1);
            this.scanParameters.Name = "scanParameters";
            this.scanParameters.Size = new System.Drawing.Size(184, 222);
            this.scanParameters.TabIndex = 41;
            this.scanParameters.DataChanged += new System.EventHandler(this.scanImageParameters1_DataChanged);
            // 
            // btnCalibImageAndStage
            // 
            this.btnCalibImageAndStage.Location = new System.Drawing.Point(6, 450);
            this.btnCalibImageAndStage.Name = "btnCalibImageAndStage";
            this.btnCalibImageAndStage.Size = new System.Drawing.Size(186, 23);
            this.btnCalibImageAndStage.TabIndex = 40;
            this.btnCalibImageAndStage.Text = "Calibrate Image && Stage";
            this.btnCalibImageAndStage.UseVisualStyleBackColor = true;
            this.btnCalibImageAndStage.Click += new System.EventHandler(this.btnCalibImageAndStage_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(115, 15);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(77, 23);
            this.btnSaveSettings.TabIndex = 39;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Location = new System.Drawing.Point(6, 421);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(186, 23);
            this.btnSnapshot.TabIndex = 22;
            this.btnSnapshot.Text = "Snapshot";
            this.btnSnapshot.UseVisualStyleBackColor = true;
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // btnTogglePreview
            // 
            this.btnTogglePreview.Location = new System.Drawing.Point(6, 44);
            this.btnTogglePreview.Name = "btnTogglePreview";
            this.btnTogglePreview.Size = new System.Drawing.Size(186, 23);
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
            this.btnConnect.Size = new System.Drawing.Size(197, 23);
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
            this.btnDoForierTransform.Size = new System.Drawing.Size(175, 23);
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
            this.btnDoReconstructImage.Size = new System.Drawing.Size(173, 23);
            this.btnDoReconstructImage.TabIndex = 32;
            this.btnDoReconstructImage.Text = "Do Reconstruct Image";
            this.btnDoReconstructImage.UseVisualStyleBackColor = false;
            this.btnDoReconstructImage.Click += new System.EventHandler(this.btnDoReconstructImage_Click);
            // 
            // pnlScanControl
            // 
            this.pnlScanControl.Controls.Add(this.scanRange);
            this.pnlScanControl.Controls.Add(this.chkAskWhereToCapture);
            this.pnlScanControl.Controls.Add(this.btnCapture);
            this.pnlScanControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlScanControl.Enabled = false;
            this.pnlScanControl.Location = new System.Drawing.Point(215, 489);
            this.pnlScanControl.Name = "pnlScanControl";
            this.pnlScanControl.Size = new System.Drawing.Size(657, 107);
            this.pnlScanControl.TabIndex = 2;
            // 
            // scanRange
            // 
            this.scanRange.Location = new System.Drawing.Point(5, 2);
            this.scanRange.Margin = new System.Windows.Forms.Padding(1);
            this.scanRange.Name = "scanRange";
            this.scanRange.Size = new System.Drawing.Size(437, 97);
            this.scanRange.TabIndex = 24;
            this.scanRange.DataChanged += new System.EventHandler(this.scanRange_DataChanged);
            this.scanRange.Load += new System.EventHandler(this.scanRange_Load);
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
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(454, 9);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(195, 55);
            this.btnCapture.TabIndex = 0;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // lblStagePos
            // 
            this.lblStagePos.AutoSize = true;
            this.lblStagePos.BackColor = System.Drawing.Color.Black;
            this.lblStagePos.ForeColor = System.Drawing.Color.White;
            this.lblStagePos.Location = new System.Drawing.Point(221, 21);
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
            this.panel3.Location = new System.Drawing.Point(215, 464);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(6);
            this.panel3.Size = new System.Drawing.Size(657, 25);
            this.panel3.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.barProg);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(215, 438);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(657, 26);
            this.panel4.TabIndex = 5;
            // 
            // barProg
            // 
            this.barProg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barProg.Location = new System.Drawing.Point(0, 0);
            this.barProg.MarqueeAnimationSpeed = 1;
            this.barProg.Name = "barProg";
            this.barProg.Size = new System.Drawing.Size(657, 26);
            this.barProg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.barProg.TabIndex = 0;
            // 
            // prevPannel
            // 
            this.prevPannel.BackColor = System.Drawing.Color.White;
            this.prevPannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevPannel.DrawFrameRate = true;
            this.prevPannel.KeepAspectRatio = true;
            this.prevPannel.Location = new System.Drawing.Point(215, 0);
            this.prevPannel.Name = "prevPannel";
            this.prevPannel.Size = new System.Drawing.Size(657, 438);
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
            this.Controls.Add(this.pnlScanControl);
            this.Controls.Add(this.panel1);
            this.Name = "Main";
            this.Text = "TestRunner";
            this.panel1.ResumeLayout(false);
            this.grpCommands.ResumeLayout(false);
            this.pnlScanControl.ResumeLayout(false);
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
        private System.Windows.Forms.Panel pnlScanControl;
        private System.Windows.Forms.GroupBox grpCommands;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnTogglePreview;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Label lblStagePos;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnDoForierTransform;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnDoReconstructImage;
        private System.Windows.Forms.ProgressBar barProg;
        private System.Windows.Forms.Button btnSnapshot;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.CheckBox chkAskWhereToCapture;
        private System.Windows.Forms.Button btnCalibImageAndStage;
        private GSIControls.Stage.ScanRangeBox scanRange;
        private GSIControls.Stage.ScanImageParameters scanParameters;
        private GSIControls.Stage.StageControlBox stageControl;
    }
}

