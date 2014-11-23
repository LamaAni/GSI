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
            //converter.Convert((info, tile) =>
            //{
            //    // drawing on graphics.
            //    Rectangle gregion = new Rectangle(
            //        previewXOffset + (int)(info.X / previewXRatio),
            //        previewYOffset + (int)(info.Y / previewYRatio),
            //        (int)(info.Width / previewXRatio),
            //        (int)(info.Height / previewYRatio)
            //        );
            //    g.DrawImage(tile, gregion);
            //}, 0, 0, img.Width, img.Height);

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

        unsafe void DoAutoLevel(ref byte[] data)
        {
            if (data.Length == 0)
                return;
            fixed (byte* pdata = data)
            {
                byte min = byte.MaxValue;
                byte max = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (pdata[i] > max)
                        max = pdata[i];
                    if (pdata[i] < min)
                        min = pdata[i];
                }
                if (min < 0)
                    min = 0;
                if (max > 255)
                    max = 255;
                double conversion = byte.MaxValue * 1.0 / (max - min);
                for (int i = 0; i < data.Length; i++)
                {
                    double val = (pdata[i] * conversion);
                    val = val < 0 ? 0 : val;
                    val = val > 255 ? 255 : val;
                    pdata[i] = (byte)val;
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

            double[] spectrum = GetSpectrumFromCurrentPoint();
            if (spectrum == null)
                return;

            for (int i = 0; i < spectrum.Length; i++)
                chartSpectrum.Series[0].Points.AddXY(i, spectrum[i]);
        }

        private double[] GetSpectrumFromCurrentPoint()
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
            double[] spectrum = reader.ReadSpectrumPixel<double>(_spectrumXPos, _spectrumYPos).Skip(10)
                .Select(v =>
                {
                    if (v > maxValue)
                        return maxValue;
                    else if (v < minValue)
                        return minValue;
                    return v;
                }).ToArray();
            reader.Close();
            return spectrum;
        }

        #endregion

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

                GSI.Processing.FFTProcessor gen = new GSI.Processing.FFTProcessor(source);
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

        private void btnCreatePreview_Click(object sender, EventArgs e)
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

                gen.SkipOnPixelConvert = gen.Processor.Settings.FftDataSize / 20;
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
                gen.Make(true);

                //GSI.Storage.Spectrum.SpectrumStreamReader reader = SpectrumStreamReader.Open(filename);
                //PreviewStream img = reader.Preview;
                //if (img.IsPreviewValid)
                //{
                //    MessageBox.Show("Already validated.");
                //}
                //DoValidatePreview(img);
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
            double[] spectrum = GetSpectrumFromCurrentPoint();
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
                new string[] { _spectrumXPos.ToString(), _spectrumYPos.ToString() }.Concat(
                spectrum.Select(v => v.ToString())).ToArray());

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

    }
}
