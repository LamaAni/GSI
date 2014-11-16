using GSI.Coding;
using GSI.IP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Processing
{
    /// <summary>
    /// Processes the image to create the final forier transform 
    /// and/or the stacked image.
    /// </summary>
    public class RawImageProcessor : DataProcessesor
    {
        /// <summary>
        /// Processes the image to create the final forier transform 
        /// and/or the stacked image.
        /// </summary>
        /// <param name="source">The image source</param>
        public RawImageProcessor(Stream source)
            : this(new StackingReader(source))
        {
        }

        /// <summary>
        /// Processes the image to create the final forier
        /// </summary>
        /// <param name="source">he image source</param>
        public RawImageProcessor(StackingReader source)
            :base(source)
        {
            UseGPU = false;
        }

        #region members

        /// <summary>
        /// If true then use the gpu.
        /// </summary>
        public bool UseGPU { get; private set; }

        #endregion

        #region methods

        /// <summary>
        /// Writes the source for the stacked image.
        /// <param name="image">The image to copy the data into.</param>
        /// </summary>
        public ImageStream ReadToImage(Stream imgstream = null)
        {
            // the image data to be transfered into the image.
            //byte[, ,] imaged = new byte[data.Height, data.Width, data.Stride / data.Width];

            //byte[] imageData = new byte[data.Height * data.Stride];
            //Marshal.Copy(data.Scan0, imageData, 0, imageData.Length);
            //Buffer.BlockCopy(imageData, 0, imaged, 0, imageData.Length);

            // gpu kernal for making the image.
            GSI.OpenCL.FFT.MakeImageData gpumake = new OpenCL.FFT.MakeImageData(
                Source.NumberOfLines, Source.LineSize, Source.VectorSize, Source.StackSize);

            //byte[] imagedata = new byte[Source.VectorSize * Source.NumberOfLines * Source.LineSize * GSI.OpenCL.FFT.MakeImageData.NumberOfBytesPerPixel];
            ImageStream image = new ImageStream(Source.VectorSize * Source.NumberOfLines,
                Source.LineSize, imgstream);

            
            float[] idata = null;
            int numberOfFloatsInImageVector = (int)(Source.VectorSize * ImageStream.NumberOfColorValuesInPixel);
            int numberOfBytesInImageVector = (int)(numberOfFloatsInImageVector * ImageStream.NumberOfBytesPerPixelValue);
            float[] vecdata = new float[numberOfFloatsInImageVector];

            CodeTimer timer = new CodeTimer();

            int vectorIndex = 0;

            // reading the data.
            DoDataProcessing((startIndex, nread, vectors, dataVector) =>
            {
                timer.Reset();

                // making the image buffer.
                int numberOfFloats = numberOfFloatsInImageVector * nread;
                if (idata == null || idata.Length != numberOfFloats)
                {
                    idata = new float[numberOfFloats];
                }

                timer.Mark("Prepare");

                if(UseGPU)
                {
                    throw new Exception("Not implemented.");
                }
                else
                {
                    PopulateIDatas(nread, idata, dataVector);
                    timer.Mark("CPU calculate");
                }


                for (int vi = 0; vi < nread; vi++)
                {
                    // copying the block.
                    int x, y;
                    int pixidx = GetNPixelOffsetFromVectorIndex(vectorIndex + vi, out x, out y);
                    int didx = vi * numberOfBytesInImageVector;
                    Buffer.BlockCopy(idata, didx, vecdata, 0, numberOfBytesInImageVector);
                    image.SetImagePixels(vecdata, x, y, Source.VectorSize, 1);
                }

                timer.Mark("Blockcopy");

                vectorIndex += nread;
            });

            //Bitmap image = new Bitmap(new MemoryStream(imagedata));

            //// creating the graphics to draw the image.
            //Graphics g = Graphics.FromImage(image);
            //g.Clear(Color.Red);
            //g.Dispose();

            //// reading the bitmap data and locking the bits.
            //BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
            //    System.Drawing.Imaging.ImageLockMode.WriteOnly, image.PixelFormat);

            //Marshal.Copy(imagedata, 0, data.Scan0, imagedata.Length);

            //image.UnlockBits(data);

            return image;
        }

        #endregion

        #region CPU relpacement

        /// <summary>
        /// Populates without masks.
        /// </summary>
        /// <param name="nread"></param>
        /// <param name="idata"></param>
        /// <param name="vectors"></param>
        void PopulateIDatas(int nread, float[] idata, byte[] vectors)
        {
            Parallel.For(0, nread*Source.VectorSize, (pidx) =>
            {
                int vidx = pidx * Source.StackSize;
                int didx = pidx * GSI.OpenCL.FFT.MakeImageData.NumberOfBytesPerPixel;
                double avarage = 0;
                for (int j = 0; j < Source.StackSize; j++)
                {
                    avarage += vectors[vidx + j];
                }
                avarage /= Source.StackSize;
                byte color = avarage > 255 ? (byte)255 : (byte)avarage;
                idata[didx] = color;
                idata[didx + 1] = color;
                idata[didx + 2] = color;
            });
        }
        #endregion
    }

}
