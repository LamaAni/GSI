using GSI.Camera.LumeneraControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraTester
{
    public partial class Main : Form
    {
        public Main()
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            btnStartStream.Text = "Start Stream";
            prevPannel.KeepAspectRatio = chkKeepAspectRatio.Checked;
            prevPannel.DrawFrameRate = chkDrawFrameRate.Checked;
        }

        Lt255 Camera;
        DateTime startStreamTime = DateTime.MinValue;

        private void button1_Click(object sender, EventArgs e)
        {
            CameraConnectionInfo info = LumeneraCamera.ScanForCameras().FirstOrDefault();
            if (info == null)
            {
                MessageBox.Show("Cannot find camera!");
                return;
            }

            Camera = new Lt255(info);

            prevPannel.AttachCamera(Camera);

            BinaryWriter wr=null;
            StreamWriter fpswr = null;
            Camera.OnStartCapture += (s, ev) =>
            {
                wr = new BinaryWriter(new FileStream("data.dat", FileMode.OpenOrCreate));
                fpswr = new StreamWriter("fps.csv");
            };

            Camera.OnEndCapture += (s, ev) =>
            {
                if (wr != null)
                {
                    wr.Close();
                    wr.Dispose();
                }

                if (fpswr != null)
                {
                    fpswr.Close();
                    fpswr.Dispose();
                }
            };

            Camera.ImageCaptured += (s, ev) =>
            {
                wr.Write(ev.Data);
                fpswr.WriteLine(Camera.ActualFrameRate);
            };

            Camera.PreviewImagereceived += (s, ev) =>
            {
                lblCameraElapsed.Text =
                    Camera.CameraElapsed.TotalSeconds.ToString("0000.00");
            };

            Camera.Connect();
            SetPreview(true);
        }

        private void btnStartStream_Click(object sender, EventArgs e)
        {
            if (btnStartStream.Text == "Start Stream")
            {
                Camera.StartStreamVideo();
                btnStartStream.Text = "Stop Stream";
                startStreamTime = DateTime.Now;
            }
            else
            {
                Camera.StopStreamVideo();
                btnStartStream.Text = "Start Stream";
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (btnCapture.Text == "Captrue")
            {
                Camera.StartCapture();
                btnCapture.Text = "Stop Captrue";
            }
            else
            {
                Camera.StopCapture();
                btnCapture.Text = "Captrue";
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            prevPannel.DetachCamera(Camera);
            Camera.Disconnect();
        }

        private void chkKeepAspectRatio_CheckedChanged(object sender, EventArgs e)
        {
            prevPannel.KeepAspectRatio = chkKeepAspectRatio.Checked;
        }

        private void chkDrawFrameRate_CheckedChanged(object sender, EventArgs e)
        {
            prevPannel.DrawFrameRate = chkDrawFrameRate.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            GSIControls.Camera.Lumenera.SettingsDialog dlg =
                new GSIControls.Camera.Lumenera.SettingsDialog(Camera);
            dlg.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamWriter wr = new StreamWriter("fps.csv");
            for(int i=0;i<1000;i++)
            {
                wr.WriteLine(Camera.ActualFrameRate);
                System.Threading.Thread.Sleep(1);
            }
            wr.Close();
            wr.Dispose();
        }

        void SetPreview(bool active)
        {
            if (active && !Camera.IsPreviewing)
            {
                btnTogglePreview.BackColor = Color.LightGreen;
                Camera.StartPreview();
                
            }
            else if (!active && Camera.IsPreviewing)
            {
                Camera.StopPreview();
                btnTogglePreview.BackColor = default(Color);
            }
        }

        private void btnTogglePreview_Click(object sender, EventArgs e)
        {
            SetPreview(!Camera.IsPreviewing);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Camera.Capture();
        }
    }
}
