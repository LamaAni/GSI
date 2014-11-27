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

            MathWorks.MATLAB.NET.Arrays.MWNumericArray arr =
                new MathWorks.MATLAB.NET.Arrays.MWNumericArray();
            GSI.Calibration.Tests.FindRotationAndPixelSize();
        }
    }
}
