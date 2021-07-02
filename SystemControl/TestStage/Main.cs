using GSI.Stage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class Main : Form
    {
        public Main()
        {
            //SerialPort p = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);38400
            //p.NewLine = "\r";
            Stage = new GSI.Stage.Piror.ProScan("COM1", 38400);
            Stage.StartServer();
            InitializeComponent();
        }

        public GSI.Stage.Piror.ProScan Stage { get; private set; }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StageCommand cmnd = new GSI.Stage.StageCommand(txtCommandBox.Text.Replace("\\r", "\r"), uint.Parse(txtLines.Text),
                    (rsp) =>
                    {
                        txtTrace.AppendText(rsp);
                        txtTrace.AppendText("\n");
                    });

            if (chkOnEnd.Checked)
                cmnd.StopOnResponse = (rsp) =>
                {
                    return rsp.Trim().ToLower() == "end";
                };
            Stage.Server.AppendCommand(cmnd);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            txtTrace.ResetText();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtX.Text = Stage.PositionX.ToString();
            txtY.Text = Stage.PositionY.ToString();
            lblFastMode.BackColor = Stage.IsInFastMode ? Color.Green : Color.LightGray;
            txtNumberOfReads.Text = Stage.Server.NumberOfReads.ToString();
#if DEBUG
            txtCommandReadTrace.Text = Stage.Server.GetReadDataCollectorString();
#endif
        }

        private void btnSetVelocity_Click(object sender, EventArgs e)
        {
            Stage.SetSpeed(double.Parse(txtVX.Text), double.Parse(txtVY.Text));
        }

        private void btnSetPosition_Click(object sender, EventArgs e)
        {
            Stage.SetPosition(double.Parse(txtPX.Text), double.Parse(txtPY.Text));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stage.StopStage();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            txtTrace.Text = Stage.SendCommandUntilEnd("?");
        }

        private void btnToggleFastMode_Click(object sender, EventArgs e)
        {
            Stage.SetFastMode(!Stage.IsInFastMode);
        }

        private void btnSetBaud_Click(object sender, EventArgs e)
        {
            GSI.Stage.Piror.ProScanBaudRates baudRate = GSI.Stage.Piror.ProScanBaudRates.B9600;
            switch (int.Parse(ddSelectBaud.SelectedItem.ToString()))
            {
                case 19200: baudRate = GSI.Stage.Piror.ProScanBaudRates.B19200;
                    break;
                case 38400: baudRate = GSI.Stage.Piror.ProScanBaudRates.B38400;
                    break;
            }

            Stage.SetBaudRate(baudRate);
        }
    }
}
