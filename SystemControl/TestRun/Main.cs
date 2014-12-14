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
            btnTogglePreview.BackColor = connectEndabledColor;
            ddScanOver.SelectedIndex = 0;
            UpdatePositionPixels();
        }


        #region members

        /// <summary>
        /// The last preview image recived.
        /// </summary>
        public byte[] LastPreviewImage { get; private set; }
        public DateTime LastPreviewTimeStamp = DateTime.Now;

        public Lt255 Camera { get; private set; }
        public ProScan Stage { get; private set; }

        /// <summary>
        /// The spectral context.
        /// </summary>
        public GSI.Context.SpectralContext  Context { get; private set; } 

        #endregion

        #region Initialization

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

                Stage.Angle = numStageAngle.Value;

                Stage.StartServer();
                Stage.SetPosition(0, 0);

                // attach the camera to the display.
                prevPannel.AttachCamera(Camera);

                Camera.PreviewImageRecived += (s, ev) =>
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
                

                // set the exposure.
                ignore_NumExp_validate = true;
                numExp.Text = Camera.Settings.Exposure.ToString();
                ignore_NumExp_validate = false;
            }
        }

        void Settings_SettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            numExp.Text = Camera.Settings.Exposure.ToString();
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

        void DoCapture()
        {
            CalculateScan();

            if (!CalculateScan())
            {
                MessageBox.Show("Scan calculation is invalid.");
                return;
            }

            if (new GSIControls.General.NumericControl[]{
                numX0,numX1,numY0,numY1,numDx}.Any(b => !b.IsValid))
            {
                MessageBox.Show("Input values are not valid.");
                return;
            }

            bool sumOverY = IsSummingOverY();
            double deltaX = numDx.Value;

            GSI.Processing.StackingCollector collector =
                new GSI.Processing.StackingCollector(Camera.Settings.Width, Camera.Settings.Height,
                   deltaX, sumOverY, sumOverY);

            // Create the current running context that captures the images
            // and creates the stack. The captured stack can be stored to the hdd.
            GSI.Context.SpectralWorkContext captureContext = Context.CreateWorkContext();

            // updating the frame rate.
            Camera.Settings.FrameRate = numFrameRate.Value;

            // deleting old files.
            foreach (string file in new string[] { "pos.csv", "imageprop.csv", "image.rawstack" })
            {
                if (File.Exists(file))
                    File.Delete(file);
            }

            StreamWriter pos_wr = new StreamWriter("pos.csv");
            StreamWriter imageprop_wr = new StreamWriter("imageprop.csv");
            GSI.Processing.StackingWriter stack_wr = new GSI.Processing.StackingWriter(
                new FileStream("image.rawstack", FileMode.OpenOrCreate));

            int totalNumberOfReadyVectors = 0;
            barProg.Maximum = 1;
            barProg.Value = 0;
            int image_idx = 0;

            // called when an image is captured.
            captureContext.OnImageCaptured += (s, e) =>
            {
                // write image proprties.
                imageprop_wr.WriteLine(
                    string.Join(",",
                    new object[] { 
                        e.Elapsed.TotalSeconds.ToString(),
                        e.Width,
                        e.Height,
                        e.Data.Length,
                        e.NumOfBytes,
                    }.Select(o => o.ToString())));

                if (captureContext.IsRecodringData)
                    lblStatus.Text = "Reading images (Current pending captured: "
                                + captureContext.PendingImageCount + "/" + image_idx + ")";

                // write image data.
                collector.PushImage(e.Data, image_idx, e.TimeStamp);
                image_idx++;
                barProg.Maximum = Camera.PendingImagesCount + image_idx;
                barProg.Value = image_idx;
            };

            captureContext.OnPositionCapture += (s, e) =>
            {
                if (!captureContext.IsRecodringData)
                    return;
                pos_wr.WriteLine(e.Elapsed.TotalSeconds.ToString() + "," + e.X + "," + e.Y);
            };

            double x0 = numX0.Value,
                x1 = numX1.Value,
                y1 = numY1.Value,
                y0 = numY0.Value;

            // calculating begin offset for images.
            double offset = collector.NumberOfImagesPerStack * deltaX * numPixelSize.Value;
            int scanSizeInPixels = 0;
            if (IsScanOverY())
            {
                if (y0 > y1)
                    y0 = y0 + offset;
                else y0 = y0 - offset;
                //txtCaptureInfo.Text += "StartX: " + y0 + ", EndX:" + y1;
                scanSizeInPixels = (int)Math.Floor(Math.Abs(y0 - y1) * 1.0 / numPixelSize.Value);
            }
            else
            {
                if (x0 > x1)
                    x0 = x0 + offset;
                else x0 = x0 - offset;
                //txtCaptureInfo.Text += "StartX: " + x0 + ", EndX:" + x1;
                scanSizeInPixels = (int)Math.Floor(Math.Abs(x0 - x1) * 1.0 / numPixelSize.Value);
            }

            scanSizeInPixels = scanSizeInPixels - (int)(collector.NumberOfImagesPerStack * deltaX) + 1;

            double vx = IsScanOverY() ? 0 : numScanSpeed.Value;
            double vy = !IsScanOverY() ? 0 : numScanSpeed.Value;

            stack_wr.WriteInitialize(scanSizeInPixels,
                Camera.Height,
                collector.NumberOfValuesPerPiexl,
                collector.StepSize,
                numPixelSize.Value );

            collector.VectorReady += (s, e) =>
            {
                if (totalNumberOfReadyVectors >= scanSizeInPixels)
                    return;
                stack_wr.WriteVector(e.Vector);
                totalNumberOfReadyVectors++;
            };

            Stage.DoPath(x0, y0, x1, y1,
                vx, vy, true, chkDoSpeedOffset.Checked,
                () =>
                {
                    // reached start pos.
                    lblStatus.Text = "Started";
                    captureContext.StartCapture();
                },
                () =>
                {
                    // reched end position.
                    captureContext.StopCapture();

                    // waiting for processing and writing to complete.
                    while (captureContext.IsProcessingPendingImages || stack_wr.IsWaitingForWriteEvents)
                    {
                        lblStatus.Text = "Waiting for image processing and disk writing to complete ("
                            + captureContext.PendingImageCount + ", " + stack_wr.NumberOfPendingWrites + ")";
                        System.Threading.Thread.Sleep(100);
                    }

                    pos_wr.Flush();
                    pos_wr.Close();
                    pos_wr.Dispose();

                    imageprop_wr.Flush();
                    imageprop_wr.Close();
                    imageprop_wr.Dispose();

                    stack_wr.Dispose();

                    GC.Collect();

                    lblStatus.Text = "Finished, " + totalNumberOfReadyVectors + " vectors, "
                        + image_idx + " images";

                    captureContext.Dispose();
                });
        }

        private bool IsScanOverY()
        {
            return ddScanOver.Text == "Y";
        }

        private bool IsSummingOverY()
        {
            bool sumOverY = (IsScanOverY() && !chkInvertedImage.Checked)
                || (!IsScanOverY() && chkInvertedImage.Checked);
            return sumOverY;
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
            if (!CalculateScan())
            {
                MessageBox.Show("Scan calculation is invalid.");
                return;
            }

            if(new GSIControls.General.NumericControl[]{
                numX0,numX1,numY0,numY1,numDx}.Any(b=>!b.IsValid))
            {
                MessageBox.Show("Input values are not valid.");
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

            // set the frame rate to the correct value.
            Camera.Settings.FrameRate = numFrameRate.Value;

            GSI.Context.SpectralWorkContext context = Context.CreateWorkContext();

            // scanning region.
            GSI.Context.SpectralScanRectangle rect=
                new GSI.Context.SpectralScanRectangle(
                    numX0.Value>numX1.Value ? numX1.Value : numX0.Value,
                    numY0.Value>numY1.Value ? numY1.Value : numY0.Value,
                    Math.Abs(numX0.Value-  numX1.Value),
                    Math.Abs(numY0.Value-  numY1.Value),
                    numPixelSize.Value,
                    IsSummingOverY(),
                    IsScanOverY());

            FileStream strm = new FileStream(filename, FileMode.Create);

            GSI.Context.SpectralScan scan = new GSI.Context.SpectralScan(
                context, rect,
                numDx.Value,
                numScanSpeed.Value,
                chkDoSpeedOffset.Checked);

            // updates the current text.
            barProg.Maximum = scan.LineSize;
            barProg.Value = 0;
            DateTime started = DateTime.Now;
            DateTime lastUpdated = DateTime.Now;
            Action updateText = () =>
            {
                TimeSpan offset = DateTime.Now - lastUpdated;
                if (offset.TotalMilliseconds < 20)
                    return;
                double pixTime = offset.TotalMilliseconds / scan.TotalNumberOfPixelsRead;
                TimeSpan eta = TimeSpan.FromMilliseconds((scan.NumberOfPixelsInImage - scan.TotalNumberOfPixelsRead) * pixTime);
                lblStatus.Text = string.Join(", ", new string[]{
                    "ETA [mins]: "+eta.ToString()+"",
                    "Elapsed: "+offset.TotalSeconds.ToString("000.00"),
                    "Line: "+(scan.CurrentLineIndex+1).ToString(),
                    "Vectors: "+scan.CapturedVectors.ToString(),
                    "Pixels: "+scan.TotalNumberOfPixelsRead.ToString(),
                    "Cur Vec Idx: "+(scan.CurrentVectorIndex).ToString()+"/"+scan.LineSize,
                    "Img. in mem: "+scan.ImagesInMemory
                });
                barProg.Value = scan.CurrentVectorIndex;
            };

            scan.LineCompleated += (s, e) => { updateText(); };
            scan.VectorCaptured += (s, e) => { updateText(); };

            StreamWriter posWriter = new StreamWriter("pos.csv");
            context.OnPositionCapture += (s, e) =>
                {
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
                imagetsWriter.WriteLine((e.InstigatingImage.TimeStamp - started).TotalMilliseconds.ToString());
            };

            scan.Scan(strm, () =>
            {
                strm.Close();
                strm.Dispose();
                btnCapture.Enabled = true;
                posWriter.Close();
                posWriter.Dispose();
                imagetsWriter.Close();
                imagetsWriter.Dispose();
            });
        }

        #endregion

        #region Scan calculations

        private bool CalculateScan()
        {
            if (!numPixelSize.IsValid ||
                !numMaxFrameRate.IsValid ||
                !numExp.IsValid ||
                !numDx.IsValid ||
                !numStageAngle.IsValid)
            {
                return false;
            }

            // all below in um.
            double sizeOfPixel = numPixelSize.Value; // in um.
            double deltaXInUm = numDx.Value * sizeOfPixel; // in um.

            double maxSpeedExpsure = Math.Floor(0.25 * sizeOfPixel / (Camera.Settings.Exposure * 1E-3));
            double maxSpeedFrameRate = Math.Floor(deltaXInUm * numMaxFrameRate.Value);

            double maxSpeed = maxSpeedExpsure < maxSpeedFrameRate ? maxSpeedExpsure : maxSpeedFrameRate;

            numFrameRate.Text = Math.Ceiling(maxSpeed / deltaXInUm).ToString();
            bool overY = IsSummingOverY();

            // taking scan angle into account.
            numScanSpeed.Text = maxSpeed.ToString();
            UpdatePositionPixels();

            return true;
        }

        private void UpdatePositionPixels()
        {
            double sizeOfPixel = numPixelSize.Value;
            numStartXPix.Value = numX0.Value / sizeOfPixel;
            numStartYPix.Value = numY0.Value / sizeOfPixel;

            numEndXPix.Value = numX1.Value / sizeOfPixel;
            numEndYPix.Value = numX1.Value / sizeOfPixel;
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

        private void btnCur0_Click(object sender, EventArgs e)
        {
            numX0.Text = Stage.PositionX.ToString();
            numY0.Text = Stage.PositionY.ToString();
        }

        private void btnCur1_Click(object sender, EventArgs e)
        {
            numX1.Text = Stage.PositionX.ToString();
            numY1.Text = Stage.PositionY.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoSpectralScan();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stage.SetPosition(0, 0);
        }

        private void btnGoto_Click(object sender, EventArgs e)
        {
            if (!numGotoXPos.IsValid || !numGotoYPos.IsValid)
            {
                MessageBox.Show("x or y are not valid.");
            }

            Stage.SetPosition(numGotoXPos.Value, numGotoYPos.Value);
        }

        private void btnCalcBest_Click(object sender, EventArgs e)
        {
            CalculateScan();
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

        bool ignore_NumExp_validate = false;
        private void numExp_Validated(object sender, EventArgs e)
        {
            if (ignore_NumExp_validate)
                return;
            ignore_NumExp_validate = true;
            Camera.Settings.Exposure = numExp.Value;
            ignore_NumExp_validate = false;
        }

        private void numStageAngle_TextChanged(object sender, EventArgs e)
        {
        }

        private void numStageAngle_Validated(object sender, EventArgs e)
        {
            Stage.Angle = numStageAngle.Value;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            File.WriteAllText("cam_settings.json", Camera.Settings.ToJson());
        }

        private void numX0_Validated(object sender, EventArgs e)
        {
            UpdatePositionPixels();
        }

        private void numY0_Validated(object sender, EventArgs e)
        {
            UpdatePositionPixels();
        }

        private void numX1_Validated(object sender, EventArgs e)
        {
            UpdatePositionPixels();
        }

        private void numPixelSize_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void numPixelSize_Validated(object sender, EventArgs e)
        {
            UpdatePositionPixels();
        }

        private void btnCalibImageAndStage_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                double angle, pixelSize;
                numPixelSize.Value = 0;
                numStageAngle.Value = 0;
                // assuming the current position is 0,0. doing the calibration for 100 um. 
                Stage.SetPosition(-50, 0, false);
                WaitUntilNextPreview();
                byte[] imga = CloneLastPreview();
                Stage.SetPosition(50, 0, false);
                WaitUntilNextPreview();
                byte[] imgb = CloneLastPreview();


                GSI.Calibration.SpatialRotation.FindRotationAndPixelSize(imga, imgb,
                    Camera.Width, 100, 0, out angle, out pixelSize);

                numPixelSize.Value = pixelSize;
                numStageAngle.Value = angle;
                Stage.Angle = numStageAngle.Value;
            });
        }
    }
}
