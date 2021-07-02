namespace CameraTester
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnStartStream = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConfigure = new System.Windows.Forms.Panel();
            this.btnCaptureFps = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.chkDrawFrameRate = new System.Windows.Forms.CheckBox();
            this.chkKeepAspectRatio = new System.Windows.Forms.CheckBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblCameraElapsed = new System.Windows.Forms.Label();
            this.lblLastInfo = new System.Windows.Forms.Label();
            this.prevPannel = new GSIControls.Camera.PreviewDisplayPannel();
            this.btnTogglePreview = new System.Windows.Forms.Button();
            this.btnConfigure.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 7);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnStartStream
            // 
            this.btnStartStream.Location = new System.Drawing.Point(12, 36);
            this.btnStartStream.Name = "btnStartStream";
            this.btnStartStream.Size = new System.Drawing.Size(156, 23);
            this.btnStartStream.TabIndex = 1;
            this.btnStartStream.Text = "Start stream";
            this.btnStartStream.UseVisualStyleBackColor = true;
            this.btnStartStream.Click += new System.EventHandler(this.btnStartStream_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(93, 7);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConfigure
            // 
            this.btnConfigure.Controls.Add(this.btnCaptureFps);
            this.btnConfigure.Controls.Add(this.button1);
            this.btnConfigure.Controls.Add(this.chkDrawFrameRate);
            this.btnConfigure.Controls.Add(this.chkKeepAspectRatio);
            this.btnConfigure.Controls.Add(this.btnCapture);
            this.btnConfigure.Controls.Add(this.btnConnect);
            this.btnConfigure.Controls.Add(this.btnStartStream);
            this.btnConfigure.Controls.Add(this.btnDisconnect);
            this.btnConfigure.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConfigure.Location = new System.Drawing.Point(0, 0);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(644, 65);
            this.btnConfigure.TabIndex = 4;
            // 
            // btnCaptureFps
            // 
            this.btnCaptureFps.Location = new System.Drawing.Point(393, 34);
            this.btnCaptureFps.Name = "btnCaptureFps";
            this.btnCaptureFps.Size = new System.Drawing.Size(97, 23);
            this.btnCaptureFps.TabIndex = 7;
            this.btnCaptureFps.Text = "CaptureFps";
            this.btnCaptureFps.UseVisualStyleBackColor = true;
            this.btnCaptureFps.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(290, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Configure";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // chkDrawFrameRate
            // 
            this.chkDrawFrameRate.AutoSize = true;
            this.chkDrawFrameRate.Checked = true;
            this.chkDrawFrameRate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawFrameRate.Location = new System.Drawing.Point(175, 40);
            this.chkDrawFrameRate.Name = "chkDrawFrameRate";
            this.chkDrawFrameRate.Size = new System.Drawing.Size(101, 17);
            this.chkDrawFrameRate.TabIndex = 5;
            this.chkDrawFrameRate.Text = "Draw frame rate";
            this.chkDrawFrameRate.UseVisualStyleBackColor = true;
            this.chkDrawFrameRate.CheckedChanged += new System.EventHandler(this.chkDrawFrameRate_CheckedChanged);
            // 
            // chkKeepAspectRatio
            // 
            this.chkKeepAspectRatio.AutoSize = true;
            this.chkKeepAspectRatio.Checked = true;
            this.chkKeepAspectRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeepAspectRatio.Location = new System.Drawing.Point(175, 13);
            this.chkKeepAspectRatio.Name = "chkKeepAspectRatio";
            this.chkKeepAspectRatio.Size = new System.Drawing.Size(109, 17);
            this.chkKeepAspectRatio.TabIndex = 4;
            this.chkKeepAspectRatio.Text = "Keep aspect ratio";
            this.chkKeepAspectRatio.UseVisualStyleBackColor = true;
            this.chkKeepAspectRatio.CheckedChanged += new System.EventHandler(this.chkKeepAspectRatio_CheckedChanged);
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(290, 34);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(97, 23);
            this.btnCapture.TabIndex = 3;
            this.btnCapture.Text = "Captrue";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblCameraElapsed);
            this.panel2.Controls.Add(this.lblLastInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 334);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(644, 26);
            this.panel2.TabIndex = 5;
            // 
            // lblCameraElapsed
            // 
            this.lblCameraElapsed.AutoSize = true;
            this.lblCameraElapsed.BackColor = System.Drawing.Color.Black;
            this.lblCameraElapsed.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCameraElapsed.ForeColor = System.Drawing.Color.White;
            this.lblCameraElapsed.Location = new System.Drawing.Point(609, 0);
            this.lblCameraElapsed.Name = "lblCameraElapsed";
            this.lblCameraElapsed.Size = new System.Drawing.Size(35, 13);
            this.lblCameraElapsed.TabIndex = 1;
            this.lblCameraElapsed.Text = "label1";
            // 
            // lblLastInfo
            // 
            this.lblLastInfo.AutoSize = true;
            this.lblLastInfo.Location = new System.Drawing.Point(9, 4);
            this.lblLastInfo.Name = "lblLastInfo";
            this.lblLastInfo.Size = new System.Drawing.Size(35, 13);
            this.lblLastInfo.TabIndex = 0;
            this.lblLastInfo.Text = "label1";
            // 
            // prevPannel
            // 
            this.prevPannel.BackColor = System.Drawing.Color.White;
            this.prevPannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevPannel.DrawFrameRate = false;
            this.prevPannel.KeepAspectRatio = false;
            this.prevPannel.Location = new System.Drawing.Point(0, 65);
            this.prevPannel.Name = "prevPannel";
            this.prevPannel.Size = new System.Drawing.Size(644, 269);
            this.prevPannel.TabIndex = 6;
            this.prevPannel.Text = "previewDisplayPannel1";
            // 
            // btnTogglePreview
            // 
            this.btnTogglePreview.Location = new System.Drawing.Point(535, 71);
            this.btnTogglePreview.Name = "btnTogglePreview";
            this.btnTogglePreview.Size = new System.Drawing.Size(97, 23);
            this.btnTogglePreview.TabIndex = 8;
            this.btnTogglePreview.Text = "Preview";
            this.btnTogglePreview.UseVisualStyleBackColor = true;
            this.btnTogglePreview.Click += new System.EventHandler(this.btnTogglePreview_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 360);
            this.Controls.Add(this.btnTogglePreview);
            this.Controls.Add(this.prevPannel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnConfigure);
            this.Name = "Main";
            this.Text = "Main";
            this.btnConfigure.ResumeLayout(false);
            this.btnConfigure.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnStartStream;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Panel btnConfigure;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblLastInfo;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.CheckBox chkKeepAspectRatio;
        private GSIControls.Camera.PreviewDisplayPannel prevPannel;
        private System.Windows.Forms.CheckBox chkDrawFrameRate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCaptureFps;
        private System.Windows.Forms.Button btnTogglePreview;
        private System.Windows.Forms.Label lblCameraElapsed;
    }
}

