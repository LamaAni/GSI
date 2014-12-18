namespace GSIControls.Stage
{
    partial class StageControlBox
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
            this.label16 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnGoto00 = new System.Windows.Forms.Button();
            this.btnSet00 = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.numStageAngle = new GSIControls.General.NumericControl();
            this.numYPos = new GSIControls.General.NumericControl();
            this.numXPos = new GSIControls.General.NumericControl();
            this.SuspendLayout();
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 145);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(69, 20);
            this.label16.TabIndex = 49;
            this.label16.Text = "S. Angle";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 104);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 20);
            this.label5.TabIndex = 46;
            this.label5.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 64);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 20);
            this.label7.TabIndex = 45;
            this.label7.Text = "X";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(124, 57);
            this.btnGo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(114, 35);
            this.btnGo.TabIndex = 44;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnGoto00
            // 
            this.btnGoto00.Location = new System.Drawing.Point(4, 5);
            this.btnGoto00.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGoto00.Name = "btnGoto00";
            this.btnGoto00.Size = new System.Drawing.Size(138, 35);
            this.btnGoto00.TabIndex = 43;
            this.btnGoto00.Text = "Goto 00";
            this.btnGoto00.UseVisualStyleBackColor = true;
            this.btnGoto00.Click += new System.EventHandler(this.btnGoto00_Click);
            // 
            // btnSet00
            // 
            this.btnSet00.Location = new System.Drawing.Point(150, 5);
            this.btnSet00.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSet00.Name = "btnSet00";
            this.btnSet00.Size = new System.Drawing.Size(88, 35);
            this.btnSet00.TabIndex = 42;
            this.btnSet00.Text = "Set 00";
            this.btnSet00.UseVisualStyleBackColor = true;
            this.btnSet00.Click += new System.EventHandler(this.btnSet00_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(124, 97);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(114, 35);
            this.btnStop.TabIndex = 51;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // numStageAngle
            // 
            this.numStageAngle.Location = new System.Drawing.Point(89, 142);
            this.numStageAngle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numStageAngle.MaxValue = 1.7976931348623157E+308D;
            this.numStageAngle.MinValue = -1.7976931348623157E+308D;
            this.numStageAngle.Name = "numStageAngle";
            this.numStageAngle.Size = new System.Drawing.Size(149, 26);
            this.numStageAngle.TabIndex = 50;
            this.numStageAngle.Text = "1.55980775811216";
            this.numStageAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStageAngle.Units = 1D;
            this.numStageAngle.Value = 1.55980775811216D;
            this.numStageAngle.Validated += new System.EventHandler(this.numStageAngle_Validated);
            // 
            // numYPos
            // 
            this.numYPos.Location = new System.Drawing.Point(30, 101);
            this.numYPos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numYPos.MaxValue = 1.7976931348623157E+308D;
            this.numYPos.MinValue = -1.7976931348623157E+308D;
            this.numYPos.Name = "numYPos";
            this.numYPos.Size = new System.Drawing.Size(86, 26);
            this.numYPos.TabIndex = 48;
            this.numYPos.Text = "0";
            this.numYPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numYPos.Units = 1D;
            this.numYPos.Value = 0D;
            // 
            // numXPos
            // 
            this.numXPos.Location = new System.Drawing.Point(30, 61);
            this.numXPos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numXPos.MaxValue = 1.7976931348623157E+308D;
            this.numXPos.MinValue = -1.7976931348623157E+308D;
            this.numXPos.Name = "numXPos";
            this.numXPos.Size = new System.Drawing.Size(86, 26);
            this.numXPos.TabIndex = 47;
            this.numXPos.Text = "0";
            this.numXPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numXPos.Units = 1D;
            this.numXPos.Value = 0D;
            // 
            // StageControlBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.numStageAngle);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.numYPos);
            this.Controls.Add(this.numXPos);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnGoto00);
            this.Controls.Add(this.btnSet00);
            this.Name = "StageControlBox";
            this.Size = new System.Drawing.Size(251, 185);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        public General.NumericControl numStageAngle;
        public General.NumericControl numYPos;
        public General.NumericControl numXPos;
        public System.Windows.Forms.Button btnGo;
        public System.Windows.Forms.Button btnGoto00;
        public System.Windows.Forms.Button btnSet00;
        public System.Windows.Forms.Button btnStop;
    }
}
