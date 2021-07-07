using GSI.Camera.LumeneraControl;
using GSI.Coding;
using GSI.IP;
using GSI.Stage.Piror;
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
using GSI.IP;

namespace TestRun
{
    public partial class Main : Form
    {
        public Main()
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            grpCommands.Enabled = false;
            pnlScanControl.Enabled = false;
            btnTogglePreview.BackColor = connectEndabledColor;

        }

        #region members

        /// <summary>
        /// The last preview image received.
        /// </summary>
        public byte[] LastPreviewImage { get; private set; }
        public DateTime LastPreviewTimeStamp = DateTime.Now;

        public Lt255 Camera { get; private set; }
        public ProScan Stage { get; private set; }

        public GSI.Processing.ScanInfo ScanInfo { get; private set; } 

        /// <summary>
        /// The spectral context.
        /// </summary>
        public GSI.Context.SpectralContext  Context { get; private set; } 

        #endregion

        #region Initialization

        /// <summary>
        /// Returns true if this is a new scan info.
        /// </summary>
        /// <returns></returns>
        public bool LoadScanInfo()
        {
            string path = GetScanInfoFileName();
            bool isNew = false;
            if (!File.Exists(path))
            {
                // default scan info.
                ScanInfo = new GSI.Processing.ScanInfo(
                    0, 0, 0, 0, Camera.Settings.Exposure, 1, 1, 1, 1e-6);
                isNew = true;
            }
            else
            {
                // read from file.
                ScanInfo = GSI.Processing.ScanInfo.FromJson(File.ReadAllText(path));
            }

            return isNew;
        }

        private static string GetScanInfoFileName()
        {
            string path =
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                Path.DirectorySeparatorChar + "scaninfo.json";
            return path;
        }

        public void StoreScanInfo()
        {
            string path = GetScanInfoFileName();
            File.WriteAllText(path, ScanInfo.ToString());
        }

        static Color connectEndabledColor = Color.LightGreen;
        
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.BackColor == connectEndabledColor)
            {
                Camera.StopStreamVideo();
                prevPannel.DetachCamera(Camera);
                Camera.Disconnect();
                Stage.StopServer();

                Context.Dispose();
                Context = null;
                Camera = null;
                Stage = null;

                // need to disconnect.
                btnConnect.BackColor = default(Color);
                btnConnect.Text = "Connect";
            }
            else
            {
                // need to connect.
                btnConnect.BackColor = connectEndabledColor;
                btnConnect.Text = "Disconnect";
                // attempting to find camera.
                CameraConnectionInfo info = LumeneraCamera.ScanForCameras().FirstOrDefault();
                if (info == null)
                {
                    MessageBox.Show("Cannot find camera");
                    return;
                }

                Stage = new ProScan("COM1");
                Camera = new Lt255(info);

                // Create the spectral context that allows for the scanning.
                Context = new GSI.Context.SpectralContext(Stage, Camera);
                Camera.Connect(File.Exists("cam_settings.json") ? File.ReadAllText("cam_settings.json") : null);

                Stage.StartServer();
                Stage.SetPosition(0, 0);

                // attach the camera to the display.
                prevPannel.AttachCamera(Camera);

                Camera.PreviewImagereceived += (s, ev) =>
                {
                    SetLastPreviewData(ev.Data, ev.TimeStamp);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Stage.PositionX.ToString("0000000.0"));
                    sb.Append(",");
                    sb.Append(Stage.PositionY.ToString("0000000.0"));
                    sb.Append(" (");
                    sb.Append(Stage.AbsolutePositionX.ToString("0000000.0"));
                    sb.Append(",");
                    sb.Append(Stage.AbsolutePositionY.ToString("0000000.0"));
                    sb.Append(")");

                    lblStagePos.Text = sb.ToString();
                };

                Camera.StartStreamVideo();
                Camera.Settings.SettingsChanged += Settings_SettingsChanged;
                SetPreview(btnTogglePreview.BackColor == connectEndabledColor);
                grpCommands.Enabled = true;
                pnlScanControl.Enabled = true;
                if (LoadScanInfo())
                {
                    ScanInfo.ExposureTime = Camera.Settings.Exposure;
                }

                stageControl.BindBositionControl(Stage);
                scanRange.BindToPositionControl(Stage);

                StoreScanInfo();
                UpdateControlsToScanInfo();
                
            }
        }

        bool _isUpdatingControls = false;

        private void UpdateControlsToScanInfo()
        {
            if (_isUpdatingControls)
                return;
            _isUpdatingControls = true;

            // updating controls.
            Stage.Angle = ScanInfo.StageAngle;
            scanRange.LoadScanInfo(ScanInfo);
            scanParameters.LoadScanInfo(ScanInfo);
            stageControl.LoadScanInfo(ScanInfo);

            // just in case/should be always 
            ScanInfo.CalculateScanParams();

            // setting calculated.
            Camera.Settings.Exposure = ScanInfo.ExposureTime;
            ScanInfo.ExposureTime = Camera.Settings.Exposure;
            Camera.Settings.FrameRate = ScanInfo.FrameRate;

            _isUpdatingControls = false;
        }

        void Settings_SettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            ScanInfo.ExposureTime = Camera.Settings.Exposure;
            UpdateControlsToScanInfo();
        }

        #endregion

        #region Move functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Vx"></param>
        /// <param name="Vy"></param>
        /// <param name="X0"></param>
        /// <param name="Y0"></param>
        /// <param name="DeltaX"></param>
        /// <param name="DeltaY"></param>
        /// <param name="waitForReturn"></param>
        public void MoveDeltaWithSpeed(int Vx, int Vy, int X0, int Y0, int DeltaX, int DeltaY, bool waitForReturn)
        {

        }

        #endregion

        #region Preview functions

        void SetPreview(bool active)
        {
            if (active && !Camera.IsPreviewing)
            {
                btnTogglePreview.BackColor = connectEndabledColor;
                Camera.StartPreview();
            }
            else if (!active && Camera.IsPreviewing)
            {
                Camera.StopPreview();
                btnTogglePreview.BackColor = default(Color);
            }
        }

        void SetLastPreviewData(byte[] data, DateTime stamp)
        {
            LastPreviewImage = data;
            LastPreviewTimeStamp = stamp;
        }

        void WaitUntilNextPreview()
        {
            DateTime lastStamp = LastPreviewTimeStamp;
            while (lastStamp >= LastPreviewTimeStamp)
                System.Threading.Thread.Sleep(10);
        }

        byte[] CloneLastPreview()
        {
            if (LastPreviewImage == null)
                return null;
            byte[] data = new byte[LastPreviewImage.Length];
            Buffer.BlockCopy(LastPreviewImage, 0, data, 0, data.Length);
            return data;
        }

        unsafe byte[] CloneLastPreviewAsRGB()
        {
            if (LastPreviewImage == null)
                return null;

            byte[] singleclone = CloneLastPreview();
            byte[] data = new byte[singleclone.Length * 3];
            fixed (byte* _source = singleclone, _target = data)
            {
                for (int i = 0; i < singleclone.Length; i++)
                {
                    int tidx = i * 3;
                    _target[tidx] = _target[tidx + 1] = _target[tidx + 2] = _source[i];
                }
            }
            return data;
        }

        #endregion

        #region Capture methods

        bool GetAndValidate(TextBox[] boxes, out double[] vals)
        {
            vals = new double[boxes.Length];
            int idx = 0;
            foreach (var b in boxes)
            {
                double val;
                if (!double.TryParse(b.Text, out val))
                    return false;
                vals[idx] = val;
                idx++;
            }
            return true;
        }

        #endregion

        #region Image reconstruct

        void DoReconstructImage(string filename)
        {
            GSI.Processing.StackingReader reader = new
                GSI.Processing.StackingReader(new FileStream(filename, FileMode.Open));
            GSI.Processing.RawImageProcessor generator =
                new GSI.Processing.RawImageProcessor(reader);

            if(reader.HasPartialLastLine)
            {
                MessageBox.Show("Note! Image last line is partial.");
            }

            barProg.Maximum = reader.NumberOfLines * reader.LineSize;
            barProg.Value = 0;
            generator.VectorComplete += (s, e) =>
            {
                barProg.Value = e.Compleated;
            };

            if (System.IO.File.Exists(filename + ".imgdump.temp"))
                System.IO.File.Delete(filename + ".imgdump.temp");

            FileStream dumpStream = new FileStream(filename + ".imgdump.temp", FileMode.CreateNew);
            ImageStream map = generator.ReadToImage(dumpStream);

            if (map != null)
                map.Save(filename + ".rslt.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            if (dumpStream.CanSeek)
                try
                {
                    dumpStream.Close();
                }
                catch
                {

                }
            File.Delete(filename + ".imgdump.temp");
            dumpStream.Dispose();
            reader.Dispose();
        }

        #endregion

        #region forier transform

        DoForierTransformDlg _dftdlg = null;

        void DoForierTransform()
        {
            if (_dftdlg == null)
                _dftdlg = new DoForierTransformDlg();
            if (_dftdlg.Visible)
                return;
            _dftdlg.Show();
        }

        #endregion

        #region Specal Scan capture

        public void DoSpectralScan()
        {
            btnCapture.Enabled = false;
            if (!scanParameters.IsValid())
            {
                MessageBox.Show("Scan parameters (exp, dx ... ) parameters are invalid.");
                return;
            }

            if(!scanRange.IsRangeValid())
            {
                MessageBox.Show("The scan range is invalid.");
                return;
            }

            if (!stageControl.IsValid())
            {
                MessageBox.Show("The stage angle is not valid.");
                return;
            }

            string filename = "image.rawstack";
            if(chkAskWhereToCapture.Checked)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Raw Stack|*.rawstack";
                if(dlg.ShowDialog()== System.Windows.Forms.DialogResult.OK)
                {
                    filename = dlg.FileName;
                }
                else
                {
                    btnCapture.Enabled = true;
                    return;
                }
            }

            GSI.Context.SpectralWorkContext context = Context.CreateWorkContext();

            // Scan region.
            GSI.Context.SpectralScanRectangle rect = ScanInfo.ToSpectralScanRectangle();

            FileStream strm = new FileStream(filename, FileMode.Create);

            GSI.Context.SpectralScan scan = new GSI.Context.SpectralScan(
                context, rect,
                ScanInfo.DeltaX,
                ScanInfo.DeltaXInPixels,
                ScanInfo.ScanSpeed,
                ScanInfo.DoSpeedup,
                ScanInfo.DoStopMotion);

            // updates the current text.
            barProg.Maximum = scan.LineSize;
            barProg.Value = 0;
            DateTime started = DateTime.Now;
            DateTime lastUpdated = DateTime.Now;
            string lastText = "[Preparing to scan...]";
            int lastPosition = 0;
            Action updateText = () =>
            {
                TimeSpan offset = DateTime.Now - lastUpdated;
                
                double pixTime = scan.TotalNumberOfPixelsRead < 1 ? 0 : offset.TotalMilliseconds / scan.TotalNumberOfPixelsRead;
                TimeSpan eta = TimeSpan.FromMilliseconds((scan.NumberOfPixelsInImage - scan.TotalNumberOfPixelsRead) * pixTime);
                lastText = string.Join(", ", new string[]{
                        "ETA [mins]: "+eta.ToString()+"",
                        "Elapsed: "+offset.TotalSeconds.ToString("000.00"),
                        "Line: "+(scan.CurrentLineIndex+1).ToString(),
                        "Vectors: "+scan.CapturedVectors.ToString(),
                        "Pixels: "+scan.TotalNumberOfPixelsRead.ToString(),
                        "Cur Vec Idx: "+(scan.CurrentVectorIndex).ToString()+"/"+scan.LineSize,
                        "Img. in mem: "+scan.ImagesInMemory});
                lastPosition= scan.CurrentVectorIndex;
                lastUpdated = DateTime.Now;
            };

            scan.LineCompleated += (s, e) => { updateText(); };
            scan.VectorCaptured += (s, e) => { updateText(); };

            StreamWriter posWriter = new StreamWriter("pos.csv");
            context.OnPositionCapture += (s, e) =>
                {
                    return;
                    posWriter.WriteLine(string.Join(",", new string[]{
                        (DateTime.Now - started).TotalMilliseconds.ToString(),
                        Stage.PositionX.ToString(),
                        Stage.PositionY.ToString(),
                        Stage.AbsolutePositionX.ToString(),
                        Stage.AbsolutePositionY.ToString()
                    }));
                };

            StreamWriter imagetsWriter = new StreamWriter("imagedt.csv");
            scan.VectorWritten += (s, e) =>
            {
                return;
                imagetsWriter.WriteLine((e.InstigatingImage.TimeStamp - started).TotalMilliseconds.ToString());
            };

            bool doUpdateCurrentValues = true;

            Task.Run(() =>
            {
                while (doUpdateCurrentValues)
                {
                    lblStatus.Text = lastText;
                    barProg.Value = lastPosition;
                    System.Threading.Thread.Sleep(10);
                }
            });

            scan.Scan(strm, () =>
            {
                System.Threading.Thread.Sleep(50);
                doUpdateCurrentValues = false;
                strm.Close();
                strm.Dispose();
                btnCapture.Enabled = true;
                posWriter.Close();
                posWriter.Dispose();
                imagetsWriter.Close();
                imagetsWriter.Dispose();
                TimeSpan totalTime = DateTime.Now - started;
                MessageBox.Show("Finished in " + totalTime);
            });
        }

        #endregion

        private void btnSettings_Click(object sender, EventArgs e)
        {
            GSIControls.Camera.Lumenera.SettingsDialog dlg =
                new GSIControls.Camera.Lumenera.SettingsDialog(Camera);
            dlg.Show();
        }

        private void btnTogglePreview_Click(object sender, EventArgs e)
        {
            SetPreview(!Camera.IsPreviewing);
        }

        private void btnStageSet00_Click(object sender, EventArgs e)
        {
            Stage.SendCommand("PS 0,0");
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            DoSpectralScan();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stage.SetPosition(0, 0);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            //string json = Camera.Settings.ToJson();
            Bitmap map = new Bitmap("rslt.png");
            map.Save("rslt.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void btnDoReconstructImage_Click(object sender, EventArgs e)
        {
            string filename = "image.rawstack";
            if (chkAskWhereToCapture.Checked)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Raw Stack|*.rawstack";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filename = dlg.FileName;
                }
                else
                {
                    MessageBox.Show("Aborted");
                    return;
                }
            }

            Task.Run(() =>
            {
                DoReconstructImage(filename);
            });
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnSnapshot_Click(object sender, EventArgs e)
        {
            string filename = "";
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Bit Map|*.bmp";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
            }
            else return;

            Bitmap map = new Bitmap(Camera.Settings.Width, Camera.Settings.Height);
            map.SetImageBytes(CloneLastPreviewAsRGB());
            map.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private void btnDoForierTransform_Click(object sender, EventArgs e)
        {
            DoForierTransform();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            File.WriteAllText("cam_settings.json", Camera.Settings.ToJson());
        }

        private void numX0_Validated(object sender, EventArgs e)
        {
        }

        private void numY0_Validated(object sender, EventArgs e)
        {
        }

        private void numX1_Validated(object sender, EventArgs e)
        {
        }

        private void numPixelSize_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void numPixelSize_Validated(object sender, EventArgs e)
        {
        }

        private void btnCalibImageAndStage_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                double angle, pixelSize;
                ScanInfo.PixelSize = 1;
                ScanInfo.StageAngle = 0;
                UpdateControlsToScanInfo();

                // assuming the current position is 0,0. doing the calibration for 100 um. 
                Stage.SetPosition(-50, 0, false);
                WaitUntilNextPreview();
                byte[] imga = CloneLastPreview();
                Stage.SetPosition(50, 0, false);
                WaitUntilNextPreview();
                byte[] imgb = CloneLastPreview();

                GSI.Calibration.SpatialRotation.FindRotationAndPixelSize(imga, imgb,
                    Camera.Width, 100, 0, out angle, out pixelSize);

                ScanInfo.StageAngle = angle;
                ScanInfo.PixelSize = pixelSize;
                UpdateControlsToScanInfo();
            });
        }

        private void scanRange_DataChanged(object sender, EventArgs e)
        {
            // update and populate.
            scanRange.PopulateScanInfo(ScanInfo);
            StoreScanInfo();
        }

        private void stageControl_DataChanged(object sender, EventArgs e)
        {
            stageControl.PopulateScanInfo(ScanInfo);
            StoreScanInfo();
        }

        private void scanImageParameters1_DataChanged(object sender, EventArgs e)
        {
            scanParameters.PopulateScanInfo(ScanInfo);
            // checking for the exposure.

            UpdateControlsToScanInfo();
            StoreScanInfo();
        }

        private void scanRange_Load(object sender, EventArgs e)
        {

        }
    }
}
