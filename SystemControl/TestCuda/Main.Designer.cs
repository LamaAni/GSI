﻿namespace CudaTester
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnDoTestFFT = new System.Windows.Forms.Button();
            this.btnTestMulti = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "DoCudaTest";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDoTestFFT
            // 
            this.btnDoTestFFT.Location = new System.Drawing.Point(13, 68);
            this.btnDoTestFFT.Name = "btnDoTestFFT";
            this.btnDoTestFFT.Size = new System.Drawing.Size(259, 23);
            this.btnDoTestFFT.TabIndex = 1;
            this.btnDoTestFFT.Text = "Do test fft OpenCl";
            this.btnDoTestFFT.UseVisualStyleBackColor = true;
            this.btnDoTestFFT.Click += new System.EventHandler(this.btnDoTestFFT_Click);
            // 
            // btnTestMulti
            // 
            this.btnTestMulti.Location = new System.Drawing.Point(12, 187);
            this.btnTestMulti.Name = "btnTestMulti";
            this.btnTestMulti.Size = new System.Drawing.Size(259, 23);
            this.btnTestMulti.TabIndex = 2;
            this.btnTestMulti.Text = "Do test muliple GPU fft";
            this.btnTestMulti.UseVisualStyleBackColor = true;
            this.btnTestMulti.Click += new System.EventHandler(this.btnTestMulti_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnTestMulti);
            this.Controls.Add(this.btnDoTestFFT);
            this.Controls.Add(this.button1);
            this.Name = "Main";
            this.Text = "Main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDoTestFFT;
        private System.Windows.Forms.Button btnTestMulti;
    }
}

