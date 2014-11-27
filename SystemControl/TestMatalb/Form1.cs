using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSI.IP;

namespace TestMatalb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string first = dlg.FileName;
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string second = dlg.FileName;
            Bitmap imga = (Bitmap)Bitmap.FromFile(first);
            Bitmap imgb = (Bitmap)Bitmap.FromFile(second);
            double angle=0, pixelSize=0;

            GSI.Calibration.SpatialRotation.FindRotationAndPixelSize(imga.GetImageBytes().Cast<float>().ToArray(),
                imgb.GetImageBytes().Cast<float>().ToArray(), imga.Width, 0, 100, out angle, out pixelSize);
        }
    }
}
