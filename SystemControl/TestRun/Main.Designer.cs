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
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.btnTogglePreview = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDoForierTransform = new System.Windows.Forms.Button();
            this.btnDoReconstructImage = new System.Windows.Forms.Button();
            this.pnlScanControl = new System.Windows.Forms.Panel();
            this.chkAskWhereToCapture = new System.Windows.Forms.CheckBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.lblStagePos = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.barProg = new System.Windows.Forms.ProgressBar();
            this.prevPannel = new GSIControls.Camera.PreviewDisplayPannel();
            this.scanRange = new GSIControls.Stage.ScanRangeBox();
            this.stageControl = new GSIControls.Stage.StageControlBox();
            this.scanParameters = new GSIControls.Stage.ScanImageParameters();
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
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(322, 917);
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
            this.grpCommands.Location = new System.Drawing.Point(18, 145);
            this.grpCommands.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCommands.Name = "grpCommands";
            this.grpCommands.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCommands.Size = new System.Drawing.Size(296, 737);
            this.grpCommands.TabIndex = 1;
            this.grpCommands.TabStop = false;
            this.grpCommands.Text = "Commands";
            // 
            // btnCalibImageAndStage
            // 
            this.btnCalibImageAndStage.Location = new System.Drawing.Point(9, 692);
            this.btnCalibImageAndStage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCalibImageAndStage.Name = "btnCalibImageAndStage";
            this.btnCalibImageAndStage.Size = new System.Drawing.Size(279, 35);
            this.btnCalibImageAndStage.TabIndex = 40;
            this.btnCalibImageAndStage.Text = "Calibrate Image && Stage";
            this.btnCalibImageAndStage.UseVisualStyleBackColor = true;
            this.btnCalibImageAndStage.Click += new System.EventHandler(this.btnCalibImageAndStage_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(172, 23);
            this.btnSaveSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(116, 35);
            this.btnSaveSettings.TabIndex = 39;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Location = new System.Drawing.Point(9, 648);
            this.btnSnapshot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(279, 35);
            this.btnSnapshot.TabIndex = 22;
            this.btnSnapshot.Text = "Snapshot";
            this.btnSnapshot.UseVisualStyleBackColor = true;
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // btnTogglePreview
            // 
            this.btnTogglePreview.Location = new System.Drawing.Point(9, 68);
            this.btnTogglePreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTogglePreview.Name = "btnTogglePreview";
            this.btnTogglePreview.Size = new System.Drawing.Size(279, 35);
            this.btnTogglePreview.TabIndex = 9;
            this.btnTogglePreview.Text = "Preview";
            this.btnTogglePreview.UseVisualStyleBackColor = true;
            this.btnTogglePreview.Click += new System.EventHandler(this.btnTogglePreview_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(9, 23);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(159, 35);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(18, 18);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(296, 35);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDoForierTransform
            // 
            this.btnDoForierTransform.BackColor = System.Drawing.Color.Salmon;
            this.btnDoForierTransform.Location = new System.Drawing.Point(28, 63);
            this.btnDoForierTransform.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDoForierTransform.Name = "btnDoForierTransform";
            this.btnDoForierTransform.Size = new System.Drawing.Size(262, 35);
            this.btnDoForierTransform.TabIndex = 24;
            this.btnDoForierTransform.Text = "Do Forier Transofrm";
            this.btnDoForierTransform.UseVisualStyleBackColor = false;
            this.btnDoForierTransform.Click += new System.EventHandler(this.btnDoForierTransform_Click);
            // 
            // btnDoReconstructImage
            // 
            this.btnDoReconstructImage.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnDoReconstructImage.Location = new System.Drawing.Point(30, 100);
            this.btnDoReconstructImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDoReconstructImage.Name = "btnDoReconstructImage";
            this.btnDoReconstructImage.Size = new System.Drawing.Size(260, 35);
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
            this.pnlScanControl.Location = new System.Drawing.Point(322, 752);
            this.pnlScanControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlScanControl.Name = "pnlScanControl";
            this.pnlScanControl.Size = new System.Drawing.Size(986, 165);
            this.pnlScanControl.TabIndex = 2;
            // 
            // chkAskWhereToCapture
            // 
            this.chkAskWhereToCapture.Checked = true;
            this.chkAskWhereToCapture.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAskWhereToCapture.Location = new System.Drawing.Point(698, 108);
            this.chkAskWhereToCapture.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAskWhereToCapture.Name = "chkAskWhereToCapture";
            this.chkAskWhereToCapture.Size = new System.Drawing.Size(213, 38);
            this.chkAskWhereToCapture.TabIndex = 23;
            this.chkAskWhereToCapture.Text = "Save to specific file";
            this.chkAskWhereToCapture.UseVisualStyleBackColor = true;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(681, 14);
            this.btnCapture.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(292, 85);
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
            this.lblStagePos.Location = new System.Drawing.Point(332, 32);
            this.lblStagePos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStagePos.Name = "lblStagePos";
            this.lblStagePos.Size = new System.Drawing.Size(91, 20);
            this.lblStagePos.TabIndex = 3;
            this.lblStagePos.Text = "[Stage Pos]";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(9, 9);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(51, 20);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "label5";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblStatus);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(322, 714);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(9);
            this.panel3.Size = new System.Drawing.Size(986, 38);
            this.panel3.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.barProg);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(322, 674);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(986, 40);
            this.panel4.TabIndex = 5;
            // 
            // barProg
            // 
            this.barProg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barProg.Location = new System.Drawing.Point(0, 0);
            this.barProg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barProg.MarqueeAnimationSpeed = 1;
            this.barProg.Name = "barProg";
            this.barProg.Size = new System.Drawing.Size(986, 40);
            this.barProg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.barProg.TabIndex = 0;
            // 
            // prevPannel
            // 
            this.prevPannel.BackColor = System.Drawing.Color.White;
            this.prevPannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevPannel.DrawFrameRate = true;
            this.prevPannel.KeepAspectRatio = true;
            this.prevPannel.Location = new System.Drawing.Point(322, 0);
            this.prevPannel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.prevPannel.Name = "prevPannel";
            this.prevPannel.Size = new System.Drawing.Size(986, 674);
            this.prevPannel.TabIndex = 0;
            this.prevPannel.Text = "prevPannel";
            // 
            // scanRange
            // 
            this.scanRange.Location = new System.Drawing.Point(8, 3);
            this.scanRange.Margin = new System.Windows.Forms.Padding(2);
            this.scanRange.Name = "scanRange";
            this.scanRange.Size = new System.Drawing.Size(656, 149);
            this.scanRange.TabIndex = 24;
            this.scanRange.DataChanged += new System.EventHandler(this.scanRange_DataChanged);
            // 
            // stageControl
            // 
            this.stageControl.Location = new System.Drawing.Point(21, 111);
            this.stageControl.Margin = new System.Windows.Forms.Padding(2);
            this.stageControl.Name = "stageControl";
            this.stageControl.Size = new System.Drawing.Size(250, 185);
            this.stageControl.TabIndex = 42;
            this.stageControl.DataChanged += new System.EventHandler(this.stageControl_DataChanged);
            // 
            // scanParameters
            // 
            this.scanParameters.Location = new System.Drawing.Point(14, 298);
            this.scanParameters.Margin = new System.Windows.Forms.Padding(2);
            this.scanParameters.Name = "scanParameters";
            this.scanParameters.Size = new System.Drawing.Size(276, 342);
            this.scanParameters.TabIndex = 41;
            this.scanParameters.DataChanged += new System.EventHandler(this.scanImageParameters1_DataChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1308, 917);
            this.Controls.Add(this.lblStagePos);
            this.Controls.Add(this.prevPannel);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnlScanControl);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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

