namespace GSILab.ToolWndStrip
{
    partial class ToolBoxStrip
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
            this.buttonsPannel = new System.Windows.Forms.Panel();
            this.pannelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnScrollLeft = new System.Windows.Forms.Button();
            this.btnScrollRight = new System.Windows.Forms.Button();
            this.internalContainer = new System.Windows.Forms.Panel();
            this.buttonsPannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonsPannel
            // 
            this.buttonsPannel.Controls.Add(this.pannelButtons);
            this.buttonsPannel.Controls.Add(this.btnScrollLeft);
            this.buttonsPannel.Controls.Add(this.btnScrollRight);
            this.buttonsPannel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonsPannel.Location = new System.Drawing.Point(0, 0);
            this.buttonsPannel.Name = "buttonsPannel";
            this.buttonsPannel.Size = new System.Drawing.Size(634, 24);
            this.buttonsPannel.TabIndex = 2;
            // 
            // pannelButtons
            // 
            this.pannelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pannelButtons.Location = new System.Drawing.Point(23, 0);
            this.pannelButtons.Name = "pannelButtons";
            this.pannelButtons.Size = new System.Drawing.Size(588, 24);
            this.pannelButtons.TabIndex = 0;
            // 
            // btnScrollLeft
            // 
            this.btnScrollLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnScrollLeft.Image = global::GSILab.Properties.Resources.resultset_previous;
            this.btnScrollLeft.Location = new System.Drawing.Point(0, 0);
            this.btnScrollLeft.Name = "btnScrollLeft";
            this.btnScrollLeft.Size = new System.Drawing.Size(23, 24);
            this.btnScrollLeft.TabIndex = 2;
            this.btnScrollLeft.UseVisualStyleBackColor = true;
            this.btnScrollLeft.Visible = false;
            // 
            // btnScrollRight
            // 
            this.btnScrollRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnScrollRight.Image = global::GSILab.Properties.Resources.resultset_next;
            this.btnScrollRight.Location = new System.Drawing.Point(611, 0);
            this.btnScrollRight.Name = "btnScrollRight";
            this.btnScrollRight.Size = new System.Drawing.Size(23, 24);
            this.btnScrollRight.TabIndex = 1;
            this.btnScrollRight.UseVisualStyleBackColor = true;
            this.btnScrollRight.Visible = false;
            // 
            // internalContainer
            // 
            this.internalContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.internalContainer.Location = new System.Drawing.Point(0, 24);
            this.internalContainer.Name = "internalContainer";
            this.internalContainer.Size = new System.Drawing.Size(634, 217);
            this.internalContainer.TabIndex = 3;
            // 
            // ToolBoxStrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.internalContainer);
            this.Controls.Add(this.buttonsPannel);
            this.Name = "ToolBoxStrip";
            this.Size = new System.Drawing.Size(634, 241);
            this.buttonsPannel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel buttonsPannel;
        private System.Windows.Forms.Button btnScrollLeft;
        private System.Windows.Forms.Button btnScrollRight;
        public System.Windows.Forms.Panel internalContainer;
        private System.Windows.Forms.FlowLayoutPanel pannelButtons;

    }
}
