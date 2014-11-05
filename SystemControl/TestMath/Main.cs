using GSI.Coding;
using GSI.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestMath
{
    public partial class Main : Form
    {
        public Main()
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }


        #region helper methods

        bool ValidateDirectory(bool showMsgBox = true)
        {
            if (!Directory.Exists(TargetDir))
            {
                if (showMsgBox)
                    MessageBox.Show("Target directory dose not exist.");
                return false;
            }
            return true;
        }

        bool ValidateFile(string file, bool showMsgBox=true)
        {
            if(!File.Exists(file))
            {
                if (showMsgBox)
                    MessageBox.Show(file+ " dose not exist.");
                return false;
            }

            return true;
        }

        private static void StoreToFile(string name, Complex[] vec)
        {
            System.IO.File.WriteAllText(name,
                string.Join("\n", vec.Select(
                n => n.Real + "," + n.Imaginary + "," + n.Magnitude)));
        }

        public static void StoreByInterlace(string name, int size, params IEnumerable<Complex>[] vectors)
        {
            StreamWriter wr = new StreamWriter(name);
            List<Complex[]> vecs = new List<Complex[]>(vectors.Select(v => v.ToArray()));

            for (int i = 0; i < size; i++)
            {
                wr.Write(i.ToString());
                for (int j = 0; j < vecs.Count; j++)
                {
                    wr.Write(",");
                    wr.Write(vecs[j][i].Magnitude);
                }
                wr.WriteLine("");
            }

            wr.Close();
            wr.Dispose();
        }

        private static Complex[] MakeVector(int matSize, double[] noise)
        {
            List<Complex> vec;
            vec = new List<Complex>();
            for (int i = 0; i < matSize; i++)
            {
                vec.Add(new System.Numerics.Complex(
                    Math.Sin(i * Math.PI / 100) +
                     5 * Math.Sin(5 * i * Math.PI / 100) +
                     2 * Math.Sin(10 * i * Math.PI / 100) +
                     noise[i]
                    , 0));
            }
            return vec.ToArray();
        }

        ImageConverter converter = new ImageConverter();

        public byte[] ReadBitmapImage(Bitmap map)
        {
            // converting to single bit grayscale.
            int bitPerPixel = 1;
            switch(map.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    bitPerPixel = 3;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    bitPerPixel = 4;
                    break;
            }

            System.Drawing.Imaging.BitmapData imageData = map.LockBits(new Rectangle(0, 0, map.Width, map.Height),
                 System.Drawing.Imaging.ImageLockMode.ReadOnly, map.PixelFormat);
            byte[] imageBytes = new byte[imageData.Width * imageData.Height * bitPerPixel];
            System.Runtime.InteropServices.Marshal.Copy(imageData.Scan0, imageBytes, 0, imageBytes.Length);
            if (bitPerPixel == 1)
                return imageBytes;
            byte[] data = new byte[map.Width * map.Height];
            for (int i = 0; i < imageBytes.Length; i += bitPerPixel)
            {
                data[i / bitPerPixel] = Convert.ToByte((imageBytes[i] + imageBytes[i + 1] + imageBytes[i + 2]) / 3);
            }
            return data;
        }

        #endregion

        #region forier test

        private void button1_Click(object sender, EventArgs e)
        {
            CodeTimer timer = new CodeTimer();
            int NumberOfRuns = 1000;
            int matSize = Convert.ToInt32(Math.Pow(2, 8));
            barRun.Minimum = 0;
            barRun.Maximum = NumberOfRuns * 4;
            barRun.Value = 0;
            List<double> vec = new List<double>();
            Random rand = new Random();
            double ampl = 10;
            for (int i = 0; i < matSize; i++)
            {
                vec.Add((rand.NextDouble() - 0.5) * ampl);
            }

            double[] noise = vec.ToArray();

            Task.Run(() =>
            {
                timer.Start();

                List<System.Numerics.Complex[]> image = new List<Complex[]>();
                List<System.Numerics.Complex[]> image2 = new List<Complex[]>();
                List<System.Numerics.Complex[]> image3 = new List<Complex[]>();

                //StoreToFile("sample.csv", MakeVector(matSize, noise));

                timer.Mark("Store org");

                for (int j = 0; j < NumberOfRuns; j++)
                {
                    image.Add(MakeVector(matSize, noise));
                    image2.Add(MakeVector(matSize, noise));
                    image3.Add(MakeVector(matSize, noise));
                    barRun.Value += 1;
                }

                timer.Mark("Prepare sample");

                image3.ForEach(v =>
                {
                    MathNet.Numerics.IntegralTransforms.Fourier.BluesteinForward(v,
                        MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);
                    barRun.Value += 1;
                });

                timer.Mark("run BluesteinForward");

                image.ForEach(v =>
                {
                    MathNet.Numerics.IntegralTransforms.Fourier.Forward(v,
                        MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);
                    barRun.Value += 1;
                });

                timer.Mark("run Noamal");

                image2.ForEach(v =>
                {
                    MathNet.Numerics.IntegralTransforms.Fourier.Radix2Forward(v,
                        MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);
                    barRun.Value += 1;
                });
                timer.Mark("run Radix");

                StoreByInterlace("rslt.csv", matSize, MakeVector(matSize, noise), image[1], image2[1], image3[1]);

                timer.Mark("Storing sampels for each");

                txtTimerResult.Text = timer.ToTraceString("Results");
            });
        }

        #endregion

        #region helper files.

        public string TargetDir { get { return txtTargetDir.Text; } }
        public string DataFile { get { return TargetDir + "\\data.dat"; } }
        public string ComposedImageFile { get { return TargetDir + "\\data.comp.jpg"; } }

        #endregion

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!ValidateDirectory())
                return;

            string[] files = Directory.GetFiles(TargetDir, "*.bmp");
            Task.Run(() =>
            {
                List<Tuple<string, byte[]>> maps = new List<Tuple<string, byte[]>>();
                int minx = int.MaxValue, miny = int.MaxValue;
                if (chkLoadVectorTest.Checked)
                {
                    int N = 100;
                    int Nx = 1280;
                    minx = Nx;
                    miny = 1;
                    for (int i = 0; i < N; i++)
                    {
                        maps.Add(new Tuple<string, byte[]>("dump",
                            new byte[Nx].Select<byte, byte>(v => 100).ToArray()));
                    }
                }
                else
                {
                    barRun.Maximum = files.Length;
                    barRun.Minimum = 0;
                    barRun.Value = 0;

                    foreach (string file in files.OrderBy(n => n))
                    {
                        Bitmap img = new Bitmap(file);

                        maps.Add(new Tuple<string, byte[]>(Path.GetFileName(file),
                            ReadBitmapImage(img)));
                        if (miny > img.Size.Height)
                            miny = img.Size.Height;
                        if (minx > img.Size.Width)
                            minx = img.Size.Width;
                        barRun.Value++;
                    }
                }

                // write to disk.
                if (File.Exists(DataFile))
                    File.Delete(DataFile);

                GSI.Processing.StackingWriter wr =
                    new GSI.Processing.StackingWriter(
                        new FileStream(DataFile, FileMode.OpenOrCreate));

                Tuple<string, byte[]>[] mapsvec = maps.ToArray();

                barRun.Maximum = mapsvec.Length;
                barRun.Minimum = 0;
                barRun.Value = 0;

                GSI.Processing.StackingCollector collector =
                    new GSI.Processing.StackingCollector(minx, miny, numDx.Value,
                        chkOverY.Checked, chkReversedStacking.Checked);

                int numberOfPixels = 0;
                int numberOfYVectors = 0;
                collector.VectorReady += (s, ev) =>
                {
                    //wr.WriteBlock(ev.VectorsBlock);
                    //numberOfPixels += ev.VectorsBlock.Length;
                    //numberOfYVectors++;
                };

                int index = 0;
                foreach (var img in mapsvec)
                {
                    collector.PushImage(img.Item2, index, DateTime.Now);
                    index++;
                    barRun.Value++;
                }

                wr.Dispose();
            });
        }

        private void btnComposeImage_Click(object sender, EventArgs e)
        {
            if (!ValidateDirectory())
                return;

            if (!ValidateFile(DataFile))
                return;

            Task.Run(() =>
            {
                StackingReader reader = new StackingReader(new FileStream(DataFile, FileMode.Open));

                //barRun.Maximum = reader.NumberOfVectors * reader.VecSize;
                //barRun.Minimum = 0;
                //barRun.Value = 0;

                //Bitmap image = chkOverY.Checked ? 
                //    new Bitmap(reader.VecSize, reader.NumberOfVectors) :
                //    new Bitmap(reader.NumberOfVectors, reader.VecSize);

                //int vecIndex=0;
                //while(!reader.ReachedEnd)
                //{
                //    foreach (byte[,] vec in reader.ReadVectors())
                //    {
                //        for (int i = 0; i < reader.VecSize; i++)
                //        {
                //            int sum = 0;
                //            for (int j = 0; j <reader.StackSize; j++)
                //            {
                //                sum = sum + vec[i, j];
                //            }
                //            byte color = Convert.ToByte(Math.Floor(sum * 1.0 / reader.StackSize));
                //            if(chkOverY.Checked)
                //            {
                //                image.SetPixel(i, vecIndex, Color.FromArgb(color, color, color));
                //            }
                //            else image.SetPixel(vecIndex, i, Color.FromArgb(color, color, color));
                //            barRun.Value++;
                //        }
                //        vecIndex++;
                //    }
                   
                //}

                if (File.Exists(ComposedImageFile))
                    File.Delete(ComposedImageFile);

                //image.Save(ComposedImageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                //image.Dispose();
                reader.Dispose();
            });
        }

        private void btnMakeImages_Click(object sender, EventArgs e)
        {
            if (!ValidateDirectory())
                return;

            int w, h;
            w = Convert.ToInt32(numDISize.Value);
            h = Convert.ToInt32(numDISize.Value);
            int l = Convert.ToInt32(numDISize.Value);

            // rect for check.
            Rectangle rect = new Rectangle(l / 2, l / 2, l / 2, l / 2);

            for (int i = 0; i < l * 2; i++)
            {
                string filename = TargetDir + "\\src_" + i + ".bmp";

                Bitmap image = new Bitmap(w, h);
                Graphics g = Graphics.FromImage(image);
                g.Clear(Color.Black);
                g.Dispose();

                int imgIdx = i;
                if (chkReversedStacking.Checked)
                    imgIdx = i;
                else imgIdx = i - l;
                if (imgIdx >= 0 && imgIdx < l)
                {
                    int pixelIdx = !chkReversedStacking.Checked ?
                        l - imgIdx - 1 : imgIdx;
                    if (chkOverY.Checked)
                        image.SetPixel(0, pixelIdx, Color.White);
                    else image.SetPixel(pixelIdx, 0, Color.White);
                }
                g.Dispose();
                image.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
                image.Dispose();
            }
        }
    }
}
