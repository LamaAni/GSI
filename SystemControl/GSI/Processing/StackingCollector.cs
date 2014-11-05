using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Processing
{
    /// <summary>
    /// A data collector that converts images to stacked data.
    /// </summary>
    public class StackingCollector
    {
        public StackingCollector(int imgWidth, int imgHeight, double stepSize,
            bool sumVertically = false,
            bool seriesReversed = false,
            int maxNumberOfImagesPerStack = -1)
        {
            RunParallel = true;
            // Cacluations for image process.
            ImageWidth = imgWidth;
            ImageHeight = imgHeight;
            SumVertical = sumVertically;
            SeriesReversed = seriesReversed;
            StepSize = stepSize;
            ImageStack = new List<ImageData>();

            NumberOfImagesPerStack = Convert.ToInt32(Math.Ceiling(MovingAxisSize * 1.0 / StepSize));

            if (maxNumberOfImagesPerStack > 0 && NumberOfImagesPerStack > maxNumberOfImagesPerStack)
                NumberOfImagesPerStack = maxNumberOfImagesPerStack;

            if (NumberOfImagesPerStack < 3)
                throw new Exception("Number of images per stack is smaller than three, need to increase image size or decrease delta x");
        }

        #region members

        /* OLD enums
        ///// <summary>
        ///// The direction of the pixels in the image. See enum for more.
        ///// </summary>
        //public StackingCollectionImageDirection ImageDirection { get;  set; }

        ///// <summary>
        ///// The direction of the stacing of the images as compared to the image direction. 
        ///// See enum for more.
        ///// </summary>
        //public StackingCollectionSeriesDirection SeriesDirection { get;  set; }
        */

        /// <summary>
        /// The size of the moving axis.
        /// </summary>
        public int MovingAxisSize
        {
            get
            {
                return SumVertical ? ImageHeight : ImageWidth;
            }
        }

        /// <summary>
        /// The size of the constant axis.
        /// </summary>
        public int ConstantAxisSize
        {
            get
            {
                return SumVertical ? ImageWidth : ImageHeight;
            }
        }

        /// <summary>
        /// If true, the staking is done vertically to the image. 
        /// <para>In an image data vector, x is row (running index) and y is determined
        /// by the size of row.</para>
        /// </summary>
        public bool SumVertical
        {
            get;
            set;
        }

        /// <summary>
        /// If true, the series of images is in the opposite direction as the running index.  The first pixel
        /// to be collected is the last pixel in image row.
        /// <para>In an image data vector, x is row (running index) and y is determined
        /// by the size of row.</para>
        /// </summary>
        public bool SeriesReversed
        {
            get;
            set;
        }

        /// <summary>
        /// The image width.
        /// </summary>
        public int ImageWidth { get; private set; }

        /// <summary>
        /// The image height
        /// </summary>
        public int ImageHeight { get; private set; }

        /// <summary>
        /// The delta x (steps between images.
        /// </summary>
        public double StepSize { get; private set; }

        /// <summary>
        /// If true the stacking procedure run in parallel threads.
        /// <para>Default is true.</para>
        /// </summary>
        public bool RunParallel { get; set; }

        /// <summary>
        /// The number of images per stack (to create vector from).
        /// </summary>
        public int NumberOfImagesPerStack { get; private set; }

        /// <summary>
        /// The number of values per piexl.
        /// </summary>
        public int NumberOfValuesPerPiexl { get { return NumberOfImagesPerStack - 2; } }

        /// <summary>
        /// The image stack.
        /// </summary>
        public List<ImageData> ImageStack { get; private set; }

        /// <summary>
        /// Called when a vector is ready. (The vector will be deleated after).
        /// </summary>
        public event EventHandler<VectorReadyEventArgs> VectorReady;

        /// <summary>
        /// if true currently processing an image.
        /// </summary>
        public bool IsProcessingImage { get; private set; }

        #endregion

        #region methods

        /// <summary>
        /// Resets the collecto to its initial state.
        /// </summary>
        public void Reset()
        {
            // the image stack clearing.
            if (IsProcessingImage)
                throw new Exception("Cannot reset while processing an image. Thread error!");
            ImageStack.Clear();
        }

        byte[, ,] vectors = null;

        /// <summary>
        /// Add new image to the stack and push out vectors compleated via the event.
        /// </summary>
        /// <param name="image"></param>
        public void PushImage(byte[] data, int idx, DateTime timestap)
        {
            IsProcessingImage = true;
            ImageData dat = new ImageData(data, idx, timestap);
            this.ImageStack.Add(dat);
            if (this.ImageStack.Count == NumberOfImagesPerStack)
            {
                PrepareVector(ref vectors);

                if (VectorReady != null)
                {
                    int pixelsInvector = vectors.GetLength(1);
                    int valuesInPixel = vectors.GetLength(2);
                    int vectorSizeInBytes = pixelsInvector * valuesInPixel;
                    int numOfVectors = vectors.GetLength(0);
                    for (int i = 0; i < numOfVectors; i++)
                    {
                        byte[,] vector = new byte[vectors.GetLength(1), vectors.GetLength(2)];
                        System.Buffer.BlockCopy(vectors, i * vectorSizeInBytes, vector, 0, vectorSizeInBytes);
                        VectorReady(this, new VectorReadyEventArgs(vector, dat));
                    }
                }
                this.ImageStack.RemoveAt(0);
            }
            IsProcessingImage = false;
        }

        /// <summary>
        /// Returns the pixel position according 
        /// to the image step size and stacking direction.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetPixelPosition(int index)
        {
            return Convert.ToInt32(Math.Round(index * StepSize));
        }

        int GetDeltaPForLastStackPosition(int index)
        {
            return GetPixelPosition(index) - GetPixelPosition(index - 1);
        }

        /// <summary>
        /// Called to prepare a ready vector, this fucntion reduces the number
        /// of stacked images.
        /// </summary>
        /// <returns>A vector block of images [vector#, vectorIndex, Stack]</returns>
        protected byte[, ,] PrepareVector()
        {
            byte[, ,] buffer = null;
            PrepareVector(ref buffer);
            return buffer;
        }

        /// <summary>
        /// Called to prepare a ready vector, this fucntion reduces the number
        /// of stacked images.
        /// </summary>
        /// <returns>A vector block of images [vector#, vectorIndex, Stack]</returns>
        protected void PrepareVector(ref byte[, ,] buffer)
        {
            int lastImageIndex = ImageStack.Last().Index;
            int dp = GetPixelPosition(lastImageIndex) - GetPixelPosition(lastImageIndex - 1);
            int lastPos = GetPixelPosition(lastImageIndex);
            if (buffer == null ||
                buffer.GetLength(0) != dp ||
                buffer.GetLength(1) != this.ConstantAxisSize ||
                buffer.GetLength(2) != NumberOfImagesPerStack - 2)
            {
                buffer = new byte[dp, this.ConstantAxisSize, NumberOfImagesPerStack - 2];
            }
            int dataOrientationSize = ImageWidth;

            // check if running parallel.
            DoFastUnsafePIndexStacking(buffer, lastPos, dp, dataOrientationSize);
        }

        protected unsafe void DoFastUnsafePIndexStacking(byte[, ,] vecs, int lastPos, int dp, 
            int dataOrientationSize)
        {
            int cxiMultiply = vecs.GetLength(2);
            int pIndexMultiply = cxiMultiply*vecs.GetLength(1);

            fixed (byte* pvecs = vecs)
            {
                UnsafePIndexStackingRunner runner = new UnsafePIndexStackingRunner(
                    pvecs, this);

                if(RunParallel)
                {
                    Parallel.For(1, NumberOfImagesPerStack - 1, (j) =>
                    {
                        runner.Run(j, lastPos, dp,
                            dataOrientationSize, cxiMultiply, pIndexMultiply);
                        //DoFastUnsafePIndexStackingVector(pvecs, );
                    });
                }
                else
                {
                    for (int j = 1; j < NumberOfImagesPerStack - 1; j++)
                    {
                        runner.Run(j, lastPos, dp,
                            dataOrientationSize, cxiMultiply, pIndexMultiply);
                        //DoFastUnsafePIndexStackingVector(pvecs, j, lastPos, dp,
                        //    dataOrientationSize, cxiMultiply, pIndexMultiply);
                    }
                }
               
            }
        }

        private unsafe class UnsafePIndexStackingRunner
        {
            public UnsafePIndexStackingRunner(byte* _pvecs, StackingCollector collector)
            {
                Collector = collector;
                pvecs = _pvecs;
            }

            public StackingCollector  Collector { get; private set; }
            public byte* pvecs { get; private set; }

            public void Run(int j, int lastPos, int dp,
            int dataOrientationSize, int cxiMultiply, int pIndexMultiply)
            {
                ImageData img = Collector.ImageStack[j];
                fixed (byte* pdata = img.Data)
                {
                    int pixelPos = Collector.GetPixelPosition(img.Index);
                    for (int cxi = 0; cxi < Collector.ConstantAxisSize; cxi++) // the perpandicular to the scan direction.
                    {
                        for (int pIndex = 0; pIndex < dp; pIndex++)
                        {
                            int pixelIndex;
                            int imageIndex;

                            // calculate the inner row/column pixel index.
                            pixelIndex = (lastPos - pixelPos) + pIndex; // (offset)+ pixel index.

                            // if reversed then need to go to the oppoxite index for the image.
                            // the end of the row/column.
                            if (Collector.SeriesReversed)
                                pixelIndex = Collector.MovingAxisSize - pixelIndex;

                            // moveing to the correct image location.
                            if (Collector.SumVertical)
                            {
                                imageIndex = dataOrientationSize * (pixelIndex) + cxi;
                            }
                            else
                            {
                                imageIndex = dataOrientationSize * cxi + pixelIndex;
                            }

                            int location = pIndex * pIndexMultiply +
                                cxi * cxiMultiply + j - 1;

                            // setting the vecs data.
                            pvecs[location] = pdata[imageIndex];

                            //vecs[pIndex, i, j - 1] = img.Data[imageIndex];
                        }
                    }
                }
            }
        }

        /* Old indexing....
        private void DoPIndexStacking(int dp, int lastPos, byte[, ,] vecs, int dataOrientationSize, int j)
        {
            ImageData img = ImageStack[j];
            int pos = GetPixelPosition(img.Index);
            for (int pIndex = 0; pIndex < dp; pIndex++)
            {
                DoUnsafeVectorCopy(lastPos, vecs, dataOrientationSize, pIndex, j, pos, img.Data);
            }
        }

        /// <summary>
        /// Called to do unsafe copy the internal vector.
        /// (Faster?)
        /// </summary>
        /// <param name="lastPos"></param>
        /// <param name="vecs"></param>
        /// <param name="dataOrientationSize"></param>
        /// <param name="pIndex"></param>
        /// <param name="j"></param>
        /// <param name="pixelPos"></param>
        /// <param name="data"></param>
        private unsafe void DoUnsafeVectorCopy(int lastPos, byte[, ,] vecs, int dataOrientationSize,
            int pIndex, int j, int pixelPos, byte[] data)
        {
            int pixelIndex;
            int imageIndex;
            for (int i = 0; i < ConstantAxisSize; i++)
            {
                // calculate the inner row/column pixel index.
                pixelIndex = (lastPos - pixelPos) + pIndex; // (offset)+ pixel index.

                // if reversed then need to go to the oppoxite index for the image.
                // the end of the row/column.
                if (SeriesReversed)
                    pixelIndex = MovingAxisSize - pixelIndex;

                // moveing to the correct image location.
                if (SumVertical)
                {
                    imageIndex = dataOrientationSize * (pixelIndex) + i;
                }
                else
                {
                    imageIndex = dataOrientationSize * i + pixelIndex;
                }

                vecs[pIndex, i, j - 1] = data[imageIndex];
            }
        }

        */

        #endregion

    }

    //public enum StackingCollectionImageDirection
    //{
    //    /// <summary>
    //    /// The staking is done horizontally to the image. 
    //    /// <para>In an image data vector, x is row (running index) and y is determined
    //    /// by the size of row.</para>
    //    /// </summary>
    //    Horizontal,
    //    /// <summary>
    //    /// The staking is done vertically to the image. 
    //    /// <para>In an image data vector, x is row (running index) and y is determined
    //    /// by the size of row.</para>
    //    /// </summary>
    //    Vertical,
    //}

    //public enum StackingCollectionSeriesDirection
    //{
    //    /// <summary>
    //    /// The series of images is in the same direction as the image running index. The first
    //    /// pixel to be collected is the same as the first pixel in the image.
    //    /// <para>In an horizontal image data vector, x is row (running index) and y is determined
    //    /// by the size of row.</para>
    //    /// </summary>
    //    Normal,
    //    /// <summary>
    //    /// The series of images is in the opposite direction as the running index.  The first pixel
    //    /// to be collected is the last pixel in the image.
    //    /// <para>In an image data vector, x is row (running index) and y is determined
    //    /// by the size of row.</para>
    //    /// </summary>
    //    Reverse,
    //}

}
