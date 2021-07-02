namespace TestForm
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
            this.components = new System.ComponentModel.Container();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.txtCommandBox = new System.Windows.Forms.TextBox();
            this.txtTrace = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtCommandReadTrace = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtLines = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVX = new System.Windows.Forms.TextBox();
            this.txtPY = new System.Windows.Forms.TextBox();
            this.txtPX = new System.Windows.Forms.TextBox();
            this.btnSetVelocity = new System.Windows.Forms.Button();
            this.btnSetPosition = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.chkOnEnd = new System.Windows.Forms.CheckBox();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnToggleFastMode = new System.Windows.Forms.Button();
            this.lblFastMode = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNumberOfReads = new System.Windows.Forms.TextBox();
            this.ddSelectBaud = new System.Windows.Forms.ComboBox();
            this.btnSetBaud = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.Location = new System.Drawing.Point(287, 12);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(75, 23);
            this.btnSendCommand.TabIndex = 0;
            this.btnSendCommand.Text = "Send";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtCommandBox
            // 
            this.txtCommandBox.Location = new System.Drawing.Point(12, 14);
            this.txtCommandBox.Name = "txtCommandBox";
            this.txtCommandBox.Size = new System.Drawing.Size(138, 20);
            this.txtCommandBox.TabIndex = 1;
            // 
            // txtTrace
            // 
            this.txtTrace.Location = new System.Drawing.Point(12, 226);
            this.txtTrace.Name = "txtTrace";
            this.txtTrace.Size = new System.Drawing.Size(362, 214);
            this.txtTrace.TabIndex = 2;
            this.txtTrace.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(299, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // txtX
            // 
            this.txtX.Enabled = false;
            this.txtX.Location = new System.Drawing.Point(34, 122);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(91, 20);
            this.txtX.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "x";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "y";
            // 
            // txtY
            // 
            this.txtY.Enabled = false;
            this.txtY.Location = new System.Drawing.Point(34, 148);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(91, 20);
            this.txtY.TabIndex = 6;
            // 
            // txtCommandReadTrace
            // 
            this.txtCommandReadTrace.Location = new System.Drawing.Point(380, 14);
            this.txtCommandReadTrace.Name = "txtCommandReadTrace";
            this.txtCommandReadTrace.Size = new System.Drawing.Size(174, 426);
            this.txtCommandReadTrace.TabIndex = 8;
            this.txtCommandReadTrace.Text = "";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtLines
            // 
            this.txtLines.Location = new System.Drawing.Point(165, 14);
            this.txtLines.Name = "txtLines";
            this.txtLines.Size = new System.Drawing.Size(41, 20);
            this.txtLines.TabIndex = 9;
            this.txtLines.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "vy";
            // 
            // txtVY
            // 
            this.txtVY.Location = new System.Drawing.Point(34, 88);
            this.txtVY.Name = "txtVY";
            this.txtVY.Size = new System.Drawing.Size(91, 20);
            this.txtVY.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "vx";
            // 
            // txtVX
            // 
            this.txtVX.Location = new System.Drawing.Point(34, 62);
            this.txtVX.Name = "txtVX";
            this.txtVX.Size = new System.Drawing.Size(91, 20);
            this.txtVX.TabIndex = 10;
            // 
            // txtPY
            // 
            this.txtPY.Location = new System.Drawing.Point(131, 148);
            this.txtPY.Name = "txtPY";
            this.txtPY.Size = new System.Drawing.Size(91, 20);
            this.txtPY.TabIndex = 15;
            // 
            // txtPX
            // 
            this.txtPX.Location = new System.Drawing.Point(131, 122);
            this.txtPX.Name = "txtPX";
            this.txtPX.Size = new System.Drawing.Size(91, 20);
            this.txtPX.TabIndex = 14;
            // 
            // btnSetVelocity
            // 
            this.btnSetVelocity.Location = new System.Drawing.Point(131, 60);
            this.btnSetVelocity.Name = "btnSetVelocity";
            this.btnSetVelocity.Size = new System.Drawing.Size(75, 23);
            this.btnSetVelocity.TabIndex = 16;
            this.btnSetVelocity.Text = "Update";
            this.btnSetVelocity.UseVisualStyleBackColor = true;
            this.btnSetVelocity.Click += new System.EventHandler(this.btnSetVelocity_Click);
            // 
            // btnSetPosition
            // 
            this.btnSetPosition.Location = new System.Drawing.Point(228, 120);
            this.btnSetPosition.Name = "btnSetPosition";
            this.btnSetPosition.Size = new System.Drawing.Size(75, 23);
            this.btnSetPosition.TabIndex = 17;
            this.btnSetPosition.Text = "Update";
            this.btnSetPosition.UseVisualStyleBackColor = true;
            this.btnSetPosition.Click += new System.EventHandler(this.btnSetPosition_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(287, 60);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 18;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // chkOnEnd
            // 
            this.chkOnEnd.AutoSize = true;
            this.chkOnEnd.Location = new System.Drawing.Point(213, 17);
            this.chkOnEnd.Name = "chkOnEnd";
            this.chkOnEnd.Size = new System.Drawing.Size(62, 17);
            this.chkOnEnd.TabIndex = 19;
            this.chkOnEnd.Text = "On End";
            this.chkOnEnd.UseVisualStyleBackColor = true;
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(212, 60);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(75, 23);
            this.btnInfo.TabIndex = 20;
            this.btnInfo.Text = "Get Info";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnToggleFastMode
            // 
            this.btnToggleFastMode.Location = new System.Drawing.Point(228, 86);
            this.btnToggleFastMode.Name = "btnToggleFastMode";
            this.btnToggleFastMode.Size = new System.Drawing.Size(75, 23);
            this.btnToggleFastMode.TabIndex = 21;
            this.btnToggleFastMode.Text = "FastMode";
            this.btnToggleFastMode.UseVisualStyleBackColor = true;
            this.btnToggleFastMode.Click += new System.EventHandler(this.btnToggleFastMode_Click);
            // 
            // lblFastMode
            // 
            this.lblFastMode.AutoSize = true;
            this.lblFastMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblFastMode.Location = new System.Drawing.Point(310, 90);
            this.lblFastMode.Name = "lblFastMode";
            this.lblFastMode.Size = new System.Drawing.Size(60, 13);
            this.lblFastMode.TabIndex = 22;
            this.lblFastMode.Text = "FastMode?";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Number of Reads";
            // 
            // txtNumberOfReads
            // 
            this.txtNumberOfReads.Enabled = false;
            this.txtNumberOfReads.Location = new System.Drawing.Point(112, 193);
            this.txtNumberOfReads.Name = "txtNumberOfReads";
            this.txtNumberOfReads.Size = new System.Drawing.Size(91, 20);
            this.txtNumberOfReads.TabIndex = 25;
            // 
            // ddSelectBaud
            // 
            this.ddSelectBaud.FormattingEnabled = true;
            this.ddSelectBaud.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400"});
            this.ddSelectBaud.Location = new System.Drawing.Point(195, 174);
            this.ddSelectBaud.Name = "ddSelectBaud";
            this.ddSelectBaud.Size = new System.Drawing.Size(92, 21);
            this.ddSelectBaud.TabIndex = 26;
            // 
            // btnSetBaud
            // 
            this.btnSetBaud.Location = new System.Drawing.Point(293, 172);
            this.btnSetBaud.Name = "btnSetBaud";
            this.btnSetBaud.Size = new System.Drawing.Size(75, 23);
            this.btnSetBaud.TabIndex = 27;
            this.btnSetBaud.Text = "Set Baud";
            this.btnSetBaud.UseVisualStyleBackColor = true;
            this.btnSetBaud.Click += new System.EventHandler(this.btnSetBaud_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 452);
            this.Controls.Add(this.btnSetBaud);
            this.Controls.Add(this.ddSelectBaud);
            this.Controls.Add(this.txtNumberOfReads);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblFastMode);
            this.Controls.Add(this.btnToggleFastMode);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.chkOnEnd);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSetPosition);
            this.Controls.Add(this.btnSetVelocity);
            this.Controls.Add(this.txtPY);
            this.Controls.Add(this.txtPX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtVY);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtVX);
            this.Controls.Add(this.txtLines);
            this.Controls.Add(this.txtCommandReadTrace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtTrace);
            this.Controls.Add(this.txtCommandBox);
            this.Controls.Add(this.btnSendCommand);
            this.Name = "Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.TextBox txtCommandBox;
        private System.Windows.Forms.RichTextBox txtTrace;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.RichTextBox txtCommandReadTrace;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtLines;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtVX;
        private System.Windows.Forms.TextBox txtPY;
        private System.Windows.Forms.TextBox txtPX;
        private System.Windows.Forms.Button btnSetVelocity;
        private System.Windows.Forms.Button btnSetPosition;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chkOnEnd;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnToggleFastMode;
        private System.Windows.Forms.Label lblFastMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNumberOfReads;
        private System.Windows.Forms.ComboBox ddSelectBaud;
        private System.Windows.Forms.Button btnSetBaud;
    }
}

