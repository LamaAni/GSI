namespace GSILab
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBoxStrip1 = new GSILab.ToolWndStrip.ToolBoxStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolBox1 = new GSILab.ToolWndStrip.ToolBox();
            this.toolBox2 = new GSILab.ToolWndStrip.ToolBox();
            this.menuStrip1.SuspendLayout();
            this.toolBoxStrip1.InternalContainer.SuspendLayout();
            this.toolBoxStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(669, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolBoxStrip1
            // 
            this.toolBoxStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolBoxStrip1.InternalContainer
            // 
            this.toolBoxStrip1.InternalContainer.Controls.Add(this.toolBox2);
            this.toolBoxStrip1.InternalContainer.Controls.Add(this.panel1);
            this.toolBoxStrip1.InternalContainer.Controls.Add(this.toolBox1);
            this.toolBoxStrip1.InternalContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBoxStrip1.InternalContainer.Location = new System.Drawing.Point(0, 24);
            this.toolBoxStrip1.InternalContainer.Name = "InternalContainer";
            this.toolBoxStrip1.InternalContainer.Size = new System.Drawing.Size(669, 375);
            this.toolBoxStrip1.InternalContainer.TabIndex = 3;
            this.toolBoxStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolBoxStrip1.Name = "toolBoxStrip1";
            this.toolBoxStrip1.Size = new System.Drawing.Size(669, 399);
            this.toolBoxStrip1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(45, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 2;
            // 
            // toolBox1
            // 
            this.toolBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolBox1.Location = new System.Drawing.Point(351, 123);
            this.toolBox1.Name = "toolBox1";
            this.toolBox1.Size = new System.Drawing.Size(289, 222);
            this.toolBox1.TabIndex = 0;
            // 
            // toolBox2
            // 
            this.toolBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolBox2.Location = new System.Drawing.Point(288, 64);
            this.toolBox2.Name = "toolBox2";
            this.toolBox2.Size = new System.Drawing.Size(289, 222);
            this.toolBox2.TabIndex = 3;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 423);
            this.Controls.Add(this.toolBoxStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Gient Spectral Imagaing Lab";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolBoxStrip1.InternalContainer.ResumeLayout(false);
            this.toolBoxStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private ToolWndStrip.ToolBoxStrip toolBoxStrip1;
        private ToolWndStrip.ToolBox toolBox1;
        private System.Windows.Forms.Panel panel1;
        private ToolWndStrip.ToolBox toolBox2;
    }
}

