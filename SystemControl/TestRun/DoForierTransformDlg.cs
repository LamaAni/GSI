using GSI.IP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSI.Storage;
using GSI.Storage.Spectrum;
using GSI.Coding;
using GSI.Storage.CSV;

namespace TestRun
{
    /// <summary>
    /// Creates the forier transform.
    /// </summary>
    public partial class DoForierTransformDlg : Form
    {
        public DoForierTransformDlg()
        {
            InitializeComponent();
            LoadCurrentCalibration();
            LoadCardSelection();
        }

        #region methods

        GSI.Processing.DataProcessesor _genActive = null;

        protected override void OnLoad(EventArgs ev)
        {
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_genActive != null)
            {
                _genActive.Abort();
                _genActive = null;
            }
            Hide();
        }

        #endregion

        #region static methods

        private static string GetCalibrationFileName()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\spectralcalib.csv";
        }

        #endregion

        #region card selection

        public void LoadCardSelection()
        {
            ddCardSelect.Items.Clear();
            try
            {
                
                foreach (GSI.OpenCL.GpuTaskDeviceInfo di in GSI.OpenCL.GpuTask.GetDevices())
                    ddCardSelect.Items.Add(di);
                if (ddCardSelect.Items.Count > 0)
                    ddCardSelect.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region calibration

        public GSI.Storage.Spectrum.SpectrumCalibrationInfo CurrentCalibration { get; private set; }
        public const int PreferedZeroFill = 256;
        public void LoadCurrentCalibration()
        {
            ddZeroFilling.Items.Clear();
            ddZeroFilling.Items.Add("No Calibration - full spectrum (ignore wavelengths)");
            // loading teh current calibration.
            if (!File.Exists(GetCalibrationFileName()))
            {
                CurrentCalibration = null;
                SelectCalibration(0);
                return;
            }

            // getting the current calibration.
            CurrentCalibration = new SpectrumCalibrationInfo(File.ReadAllText(GetCalibrationFileName()));
            bool indexSelected = false;
            foreach (var kvp in CurrentCalibration)
            {
                ddZeroFilling.Items.Add("To " + kvp.Key.ToString());
                if (kvp.Key == PreferedZeroFill)
                {
                    indexSelected = true;
                    SelectCalibration(ddZeroFilling.Items.Count - 1);
                }
            }

            if (!indexSelected)
                SelectCalibration(0);
        }

        /// <summary>
        /// The selected zero fill.
        /// </summary>
        public int SelectedZeroFill { get; private set; }

        void SelectCalibration(int idx)
        {
            if (ddZeroFilling.SelectedIndex != idx)
                ddZeroFilling.SelectedIndex = idx;

            if (idx == 0)
            {
                // no calibration.
                SelectedZeroFill = PreferedZeroFill;
            }
            else
            {
                // the selected zero fill.
                SelectedZeroFill = CurrentCalibration.ElementAt(idx-1).Key;
            }

            int startIndex = 0;
            int fftDataSize = GetSelectedFFTDataSize(out startIndex, idx == 0);

            lblSelectedCalib.Text = (idx == 0 ? "No Calibration" : "Fill to " + SelectedZeroFill) +
                ", Start index: "+startIndex+", FftDataSize: " + fftDataSize + (fftDataSize <= 0 ? "!!ERROR!!" : "");
        }

        int GetSelectedFFTDataSize(out int startIndex, bool? useDefault = null)
        {
            if (useDefault == null)
                useDefault = ddZeroFilling.SelectedIndex == 0;
            if (useDefault == true)
            {
                // no calibration.
                startIndex = 0;
                return SelectedZeroFill / 2;
            }
            else
            {
                // the selected zero fill.
                Tuple<int,int> rslt=CurrentCalibration.CalculateZeroFillDataSize(SelectedZeroFill,
                    1.0 / (numEndWavelegnth.Value * 1e-9), 1.0 / (numStartWavelength.Value * 1e-9));
                startIndex = rslt.Item1;
                return rslt.Item2;
            }
        }

        int GetSelectedFFTDataSize(bool? useDefault=null)
        {
            int sidx = 0;
            return GetSelectedFFTDataSize(out sidx);
        }

        #endregion

        #region preview spectrum

        double previewXRatio = 0;
        double previewYRatio = 0;
        int previewXOffset = 0;
        int previewYOffset = 0;
        string dataFilename = null;
        string previewFilename = null;

        public virtual void LoadPreview(string filename, bool keepAspectRatio = true)
        {
            CodeTimer timer = new CodeTimer();
            timer.Start();
            dataFilename = filename;
            previewFilename = filename + ".prevdat";
            if(!File.Exists(previewFilename))
            {
                MessageBox.Show("Preview file, " + previewFilename + " not found. Press create preview.");
                return;
            }
            timer.Mark("Prepare");
            PreviewStream img = new PreviewStream(File.Open(previewFilename, FileMode.Open));
            if(!img.IsPreviewValid)
            {
                MessageBox.Show("Preview is invalid. (Validate preview not run)");
                img.Dispose();
                return;
            }

            timer.Mark("Stream");
            Graphics g = pannelImageShow.CreateGraphics();
            previewXRatio = img.Width * 1.0 / pannelImageShow.Width;
            previewYRatio = img.Height * 1.0 / pannelImageShow.Height;

            if (keepAspectRatio)
            {
                if (previewXRatio < previewYRatio)
                {
                    previewXRatio = previewYRatio;
                }
                else
                {
                    previewYRatio = previewXRatio;
                }
            }

            previewXOffset = (int)(Math.Abs(img.Width / previewXRatio - pannelImageShow.Width) / 2);
            previewYOffset = (int)(Math.Abs(img.Height / previewYRatio - pannelImageShow.Height) / 2);

            // clering..
            Rectangle gregion = new Rectangle(
                previewXOffset,
                previewYOffset,
                (int)(img.Width / previewXRatio),
                (int)(img.Height / previewYRatio)
                );

            timer.Mark("Ratios");
            g.Clear(Color.Black);
            timer.Mark("Clear");

            img.Draw(g, 0, 0, img.Width, img.Height, gregion.X, gregion.Y, gregion.Width, gregion.Height, (data) =>
            {
                DoAutoLevel(ref data);
            });
            timer.Mark("Draw");
            img.Close();
            img.Dispose();
            g.Dispose();
            lblProgInfo.Text = "Load stream: " + timer["Stream"] + ", Clear GC: " + timer["Clear"] + ", Draw: " + timer["Draw"];
        }

        unsafe void DoAutoLevel(ref float[] data)
        {
            if (data.Length == 0)
                return;
            fixed (float* pdata = data)
            {
                float min = byte.MaxValue;
                float max = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (pdata[i] > max)
                        max = pdata[i];
                    if (pdata[i] < min)
                        min = pdata[i];
                }
                // doing the conversion to 256 values.
                float conversion = byte.MaxValue * 1.0F / (max - min);
                for (int i = 0; i < data.Length; i++)
                {
                    float val = (pdata[i] * conversion);
                    val = val < 0 ? 0 : val;
                    val = val > 255 ? 255 : val;
                    pdata[i] = val;
                }
            }
        }

        void ValidatePreview(string filename, bool async=true)
        {
            dataFilename = filename;
            previewFilename = filename + ".prevdat";
            if (!File.Exists(previewFilename))
            {
                MessageBox.Show("Preview file, " + previewFilename + " not found. Press create preview.");
                return;
            }

            PreviewStream img = new PreviewStream(File.Open(previewFilename, FileMode.Open));
            if (!img.IsPreviewValid)
            {
                MessageBox.Show("Preview is invalid. (Validate preview not run)");
                img.Dispose();
                return;
            }

            Action f = () =>
            {
                CodeTimer timer = new CodeTimer();
                timer.Start();
                DateTime lastUpdated = DateTime.MinValue;
                int totalCount = img.GetNumberOfPixelsToPreviewValidate();
                int done = 0;
                progBar.Maximum = totalCount;
                img.ValidatePreview((total, read) =>
                {
                    done += read;
                    if ((DateTime.Now - lastUpdated).TotalSeconds >= 1)
                    {
                        lastUpdated = DateTime.Now;
                        double perPoint = timer.Elapsed.TotalSeconds * 1.0 / done;
                        TimeSpan left = TimeSpan.FromSeconds(perPoint * (totalCount - done));
                        lblProgInfo.Text = "Creating preview, Eta: " + left.ToShortTimespanString() + " (" + done + "/" + totalCount + ")";
                    }
                    progBar.Value = done;
                });

                img.Close();
                img.Dispose();
            };

            if (async)
                Task.Run(f);
            else f();
        }

        private static int GetPrecentageIndex(IEnumerable<int> hist, int fivepre)
        {
            double count = 0;
            int minVal = hist.IndexOfFirst((v) =>
            {
                count += v;
                if (count > fivepre)
                    return true;
                return false;
            });
            return minVal;
        }

        int _spectrumXPos = -1;
        int _spectrumYPos = -1;

        public virtual void ShowPreviewSpectrum(int x, int y)
        {
            chartSpectrum.Series[0].Points.Clear();
            x = x - previewXOffset;
            y = y - previewYOffset;
            if (dataFilename == null || x < 0 || y < 0)
            {
                return;
            }

            _spectrumXPos = (int)Math.Floor(x * previewXRatio);
            _spectrumYPos = (int)Math.Floor(y * previewXRatio);

            double[][] spectrum = GetSpectrumForScreenXY();
            if (spectrum == null)
                return;

            for (int i = 0; i < spectrum.Length; i++)
                chartSpectrum.Series[0].Points.AddXY(spectrum[i][0], spectrum[i][1]);
        }

        private double[][] GetSpectrumForScreenXY()
        {
            if (_spectrumXPos < 0 || _spectrumYPos < 0)
                return null;

            GSI.Storage.Spectrum.SpectrumStreamReader reader =
                new GSI.Storage.Spectrum.SpectrumStreamReader(
                    new FileStream(dataFilename, FileMode.Open, FileAccess.Read));

            if (reader.Settings.Width < _spectrumXPos || reader.Settings.Height < _spectrumYPos)
                return null;

            double maxValue = 1e10;
            double minValue = 0;
            double[] spectrumAmp = reader.ReadSpectrumPixel<double>(_spectrumXPos, _spectrumYPos)
                .Select(v =>
                {
                    if (v > maxValue)
                        return maxValue;
                    else if (v < minValue)
                        return minValue;
                    return v;
                }).ToArray();

            reader.Close();

            // get the location wavelength
            double[] spectrumWavelength = reader.Settings.GenerateSpectrumFrequencies()
                .Select(f => Math.Round(100.0e9 / f) / 100).ToArray();

            // get the spectrum.
            double[][] spectrum = new double[spectrumAmp.Length][];
            for (int i = 0; i < spectrumAmp.Length; i++)
            {
                spectrum[i] = new double[2];
                spectrum[i][0] = spectrumWavelength[i];
                spectrum[i][1] = spectrumAmp[i];
            }

            return spectrum;
        }

        #endregion

        /// <summary>
        /// Run the forier transform.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void btnDoForier_Click(object sender, EventArgs ev)
        {
            CallDoForier();
        }

        private void CallDoForier()
        {
            string filename = "image.rawstack";

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Raw Stack|*.rawstack";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
            }
            else
            {
                return;
            }

            // validating the current fft data size.
            if (GetSelectedFFTDataSize() <= 0)
            {
                MessageBox.Show("The current selected wavelenth range and configuration will result in a zero FFT data size, or an error.");
                return;
            }

            if (ddCardSelect.Items.Count == 0)
            {
                MessageBox.Show("There are not avilable cards to select from. Forier aborted.");
                return;
            }

            Task.Run(() =>
            {
                
                btnDoForier.Enabled = false;
                GSI.Coding.CodeTimer timer = new GSI.Coding.CodeTimer();
                timer.Start();

                // prepering preview.
                lblProgInfo.Text = "Preparing Preview";
                progBar.Value = 0;
                //Generator.ReadToImage(DisplayImage);
                if (btnCancel.Text == "Closed")
                    return;
                lblProgInfo.Text = "Forier transform";
                progBar.Value = 0;
                //Generator.WriteImageAndForier(Target, DisplayImage);
                timer.Mark("Prepare");
                string targetName = filename + ".forier.sdat";

                FileStream source = File.Open(filename, FileMode.Open);
                GSI.Processing.StackingReader reader=
                    new GSI.Processing.StackingReader(source);

                reader.Initialize();

                GSI.Storage.Spectrum.SpectrumStreamSettings settings =
                    new SpectrumStreamSettings(reader.NumberOfLines, reader.LineSize, reader.VectorSize,
                        SelectedZeroFill, reader.StepSize, reader.PixelSize);

                // configuring the calibration into the settings in the 
                if (CurrentCalibration != null && ddZeroFilling.SelectedIndex != 0)
                {
                    if (!(numEndWavelegnth.IsValid && numEndWavelegnth.IsValid))
                    {
                        MessageBox.Show("In calibration mode the start wavelength and end wavelength must be value or null.");
                        return;
                    }

                    if (numEndWavelegnth.Value > numStartWavelength.Value)
                    {
                        settings.StartFrequency = 1.0 / (numEndWavelegnth.Value * 1e-9);
                        settings.EndFrequency = 1.0 / (numStartWavelength.Value * 1e-9);
                    }
                    CurrentCalibration.FillSpectrumStreamSettings(settings);
                }
                else
                {
                    // use default setting to do zero filling.
                }

                // getting the memory of the best device (the one that will be used in the forier).
                int maxMemory = (numTotalMemoryInMB.IsValid ? (int)numTotalMemoryInMB.Value : 60) * 
                    GSI.Processing.FFTProcessor._MB_;
                GSI.Processing.FFTProcessor gen = new GSI.Processing.FFTProcessor(reader, settings, maxMemory);

                gen.Devices.Add((GSI.OpenCL.GpuTaskDeviceInfo)ddCardSelect.SelectedItem);

                _genActive = gen;
                gen.VectorComplete += (s, e) =>
                {
                    progBar.Maximum = e.Max;
                    progBar.Value = e.Compleated;
                    lblProgInfo.Text = "Fourier (" + e.Compleated + "/" + e.Max + "), eta: " + e.Eta.ToString() + " [s]";
                };

                // creating the file and marking as sparse so it will have faster writing.
                FileStream to = File.Create(targetName);

                // make the file sparse to allow random access.
                to.MarkeFileAsSparse();

                // create a preview stream if the stream is above a certain size.
                gen.ToForierTransform(to);
                to.Close();
                to.Dispose();

                timer.Mark("Transform");
                timer.Mark("Total");
                MessageBox.Show(timer.ToTraceString("Compleated", (ts) => ts.ToShortTimespanString()));
                if (btnCancel.Text == "Closed")
                    return;
                // after compleated.
                btnCancel.Text = "Close";
                gen.Close();
                gen.Dispose();

                btnDoForier.Enabled = true;
            });
        }

        private void btnShowFile_Click(object sender, EventArgs e)
        {

            string filename = "image.rawstack.sdat";

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Spectrum|*.sdat";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
            }
            else
            {
                return;
            }
            Task.Run(() =>
            {
                LoadPreview(filename);
            });
            
        }

        private void pannelImageShow_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pannelImageShow_Click(object sender, EventArgs e)
        {
        }

        private void pannelImageShow_MouseClick(object sender, MouseEventArgs e)
        {
            ShowPreviewSpectrum(e.X, e.Y);
        }

        private unsafe void btnCreatePreview_Click(object sender, EventArgs e)
        {
            string filename = "image.rawstack";

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Spectrum|*.sdat";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
            }
            else
            {
                return;
            }

            Task.Run(() =>
            {
                FileStream previewStream = File.Create(filename + ".prevdat");

                GSI.IP.SpectrumPreviewGenerator gen =
                    new SpectrumPreviewGenerator(
                        new SpectrumStreamProcessor(
                            File.Open(filename, FileMode.Open)), previewStream);

                GSI.Calibration.FrequencyToRgbConvertor convertor = null;
                float[] xCoef = null, yCoef = null, zCoef = null;
                if (chkDoRgbPreview.Checked &&  gen.Processor.Settings.StartFrequency >= 0)
                {
                    float[] frequencies = gen.Processor.Settings.GenerateSpectrumFrequencies()
                        .Select(v => (float)v).ToArray();

                    convertor =
                        new GSI.Calibration.FrequencyToRgbConvertor(frequencies);

                    xCoef = convertor.XCoef;
                    yCoef = convertor.YCoef;
                    zCoef = convertor.ZCoef;

                    gen.SkipOnPixelConvert = 0; // always take into account the entier spectrum becuse this
                    // is function dependent.
                }
                else
                {
                    // remove the DC since we have an avarage spectrum.
                    gen.SkipOnPixelConvert = gen.Processor.Settings.FftDataSize / 20;
                }
                
                progBar.Value = 0;
                progBar.Maximum = 1;
                CodeTimer timer = new CodeTimer();
                timer.Start();
                DateTime lastUpdated = DateTime.MinValue;
                gen.OnBlockComplete += (s, se) =>
                {
                    if (progBar.Maximum != se.Total)
                        progBar.Maximum = se.Total;

                    if ((DateTime.Now - lastUpdated).TotalSeconds >= 1)
                    {
                        lastUpdated = DateTime.Now;
                        double perPoint = timer.Elapsed.TotalSeconds * 1.0 / se.Done;
                        TimeSpan left = TimeSpan.FromSeconds(perPoint * (se.Total - se.Done));
                        lblProgInfo.Text = "Creating preview, Eta: " + left.ToShortTimespanString() + " (" + se.Done + "/" + se.Total + ")";
                    }
                    progBar.Value = se.Done;

                };

                gen.OnCompleated += (s, se) =>
                {
                    gen.Close();
                    gen.Dispose();
                    timer.Stop();
                    timer = null;
                };

                lblProgInfo.Text = "Creating preview";
                if (convertor == null)
                    gen.DoRGBConvert = null;
                else
                {
                    GSI.Calibration.FrequencyToRgbConvertor.ConversionParmas prs =
                        new GSI.Calibration.FrequencyToRgbConvertor.ConversionParmas(convertor.Frequencies.Length,
                            xCoef, yCoef, zCoef, convertor.ConversionMatrix);

                    // finding the normalization factor.
                    float[] maxpower = new float[prs.Count].Select(v => 255F).ToArray();

                    float maxNormal = 1F / convertor.ToRGB(maxpower, prs).Max();
                    float[] normals = //convertor.ToRGB(maxpower, prs).Select(v => 1F / v).ToArray();
                        new float[3].Select(f => maxNormal).ToArray();

                    gen.DoRGBConvert = (amp, offset, length) =>
                    {
                        return convertor.ToRGB(amp, offset, prs, normals);
                    };
                }
                gen.Make(true);

            });

        }

        private void DoValidatePreview(PreviewStream img)
        {
            progBar.Value = 0;
            img.ValidatePreview((N, read) =>
            {
                if (progBar.Maximum != N)
                {
                    progBar.Maximum = N;
                    progBar.Minimum = 0;
                }
                progBar.Value += read;
            });
        }

        private void DoForierTransformDlg_SizeChanged(object sender, EventArgs e)
        {
            if (dataFilename != null)
                LoadPreview(dataFilename, true);
        }

        private void btnStoreSpectrum_Click(object sender, EventArgs e)
        {
            double[][] spectrum = GetSpectrumForScreenXY();
            if (spectrum == null)
            {
                MessageBox.Show("No spectrum data selected.");
                return;
            }

            string filename = "data.csv";
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "CSV|*.csv";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
            }
            else
            {
                return;
            }
            string txt = string.Join("\n",
                new string[] { _spectrumXPos.ToString(), _spectrumYPos.ToString() }
                .Concat(spectrum
                .Select(v => string.Join(",", v.Select(iv => iv.ToString()).ToArray())).ToArray()));

            File.WriteAllText(filename, txt);
        }

        private void btnValidatePreview_Click(object sender, EventArgs e)
        {
            string filename = "image.rawstack.sdat";

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Spectrum|*.sdat";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
            }
            else
            {
                return;
            }
            Task.Run(() =>
            {
                ValidatePreview(filename);
            });
        }

        private void chartSpectrum_Click(object sender, EventArgs e)
        {

        }

        private void lblProgInfo_Click(object sender, EventArgs e)
        {

        }

        private void progBar_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadCalibration_Click(object sender, EventArgs e)
        {
            // loads a calibration file and stores this calibration file as the last loaded.
            OpenFileDialog dlg=new OpenFileDialog();
            dlg.Filter = "CSV|*.csv";
            if(dlg.ShowDialog()!= System.Windows.Forms.DialogResult.OK)
                return;

            GSI.Storage.Spectrum.SpectrumCalibrationInfo Calib =
                new SpectrumCalibrationInfo(File.ReadAllText(dlg.FileName));

            // overwriting calibration.
            File.WriteAllText(GetCalibrationFileName(), Calib.ToString());

            // loading the current calibration.
            LoadCurrentCalibration();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ddZeroFilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectCalibration(ddZeroFilling.SelectedIndex);
        }


    }
}
