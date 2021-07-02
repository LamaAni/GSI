namespace GSILab.ToolWndStrip
{
    partial class ToolBox
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
            this.gripMain = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // gripMain
            // 
            this.gripMain.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.gripMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.gripMain.Location = new System.Drawing.Point(0, 0);
            this.gripMain.Name = "gripMain";
            this.gripMain.Size = new System.Drawing.Size(289, 10);
            this.gripMain.TabIndex = 0;
            // 
            // ToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.gripMain);
            this.Name = "ToolBox";
            this.Size = new System.Drawing.Size(289, 222);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel gripMain;

    }
}
