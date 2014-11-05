using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSILab
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            //this.IsMdiContainer = true;
        }

        #region events

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // close the main window
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // About
            MessageBox.Show( "GSI Lab work controller\nBuilt by Zav Shotan n Moshe Lindner for ISI","About");
        }

        #endregion

        private void Main_Load(object sender, EventArgs e)
        {
            //ToolStripTest test = new ToolStripTest();
            //test.Parent = this;
            //test.Show();
            //test.BringToFront();
        }

    }
}
