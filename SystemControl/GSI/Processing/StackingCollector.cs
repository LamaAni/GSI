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
        /// Resets the collector to its initial state.
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

            // creating the new image.
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

                    // calling the vector ready events. (using block copy).
                    for (int i = 0; i < numOfVectors; i++)
                    {
                        byte[,] vector = new byte[vectors.GetLength(1), vectors.GetLength(2)];
                        System.Buffer.BlockCopy(vectors, i * vectorSizeInBytes, vector, 0, vectorSizeInBytes);
                        VectorReady(this, new VectorReadyEventArgs(vector, dat));
                    }
                }

                // removing the first image from the stack and waiting for the next one.
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
            // Calculating the image indexs required for stacking.
            int lastImageIndex = ImageStack.Last().Index; // the last image index.
            int lastPixelInFrame = GetPixelPosition(lastImageIndex); // the pixel position (within the buffer) for this image.
            // the step size in pixels for each index.
            int windowStepSize = lastPixelInFrame - GetPixelPosition(lastImageIndex - 1);

            // recreating the buffer if nesseary.
            if (buffer == null ||
                buffer.GetLength(0) != windowStepSize ||
                buffer.GetLength(1) != this.ConstantAxisSize ||
                buffer.GetLength(2) != NumberOfImagesPerStack - 2)
            {
                buffer = new byte[windowStepSize, this.ConstantAxisSize, NumberOfImagesPerStack - 2];
            }

            // check if running parallel.
            DoFastUnsafePIndexStacking(ref buffer, lastPixelInFrame, windowStepSize, ImageWidth);
        }

        /// <summary>
        /// Call to execute the fast unsafe stacking.
        /// </summary>
        /// <param name="vecs"></param>
        /// <param name="lastPixelInFrame"></param>
        /// <param name="windowStepSize"></param>
        /// <param name="windowWidth"></param>
        protected unsafe void DoFastUnsafePIndexStacking(ref byte[, ,] vecs, int lastPixelInFrame, int windowStepSize, 
            int windowWidth)
        {
            fixed (byte* pvecs = vecs)
            {
                // creating the runner. (see internal params);
                UnsafePIndexStackingRunner runner = new UnsafePIndexStackingRunner(
                    pvecs, vecs.GetLength(2), vecs.GetLength(1), windowWidth, windowStepSize, lastPixelInFrame,
                    this);

                // no iterate on every stacked image in the collection
                // add add the image data to the correct location in the stacked matrix.
                // (runs by vector)
                // decisiton for parallel or not. 
                if (RunParallel)
                    Parallel.For(1, NumberOfImagesPerStack - 1, (i) => runner.Run(i));
                else for (int i = 1; i < NumberOfImagesPerStack - 1; i++)
                        runner.Run(i);
            }
        }

        /// <summary>
        /// A runner class for doing the stacking. Generated per stack.
        /// </summary>
        private unsafe class UnsafePIndexStackingRunner
        {
            /// <summary>
            /// Create a new stacking runner
            /// </summary>
            /// <param name="vectors">A pointer to the pixel vectors, where the data will be stored (3d array)</param>
            /// <param name="collector">The calling stacking collector</param>
            /// <param name="numberOfValuesInAStack">The number of values in a sigle stack</param>
            /// <param name="numberOfStacksInAVector">The number of stacks in a vector</param>
            /// <param name="windowWidth">The size of the scanning window in pixel index (the image width)</param>
            /// <param name="windowStepSize">The step size in pixels between two frames taken by the window.</param>
            /// <param name="lastPixelInFrame">The position of the last pixel in the frame.</param>
            public UnsafePIndexStackingRunner(byte* vectors,
                int numberOfValuesInAStack,
                int numberOfStacksInAVector,
                int windowWidth,
                int windowStepSize,
                int lastPixelInFrame,
                StackingCollector collector)
            {
                Collector = collector;
                Vectors = vectors;
                NumberOfValuesInAStack = numberOfValuesInAStack;
                NumberOfStacksInAVector = numberOfStacksInAVector;
                NumberOfValuesInAVector = NumberOfValuesInAStack * NumberOfStacksInAVector;
                WindowWidth = windowWidth;
                WindowStepSize = windowStepSize;
                LastPixelInFrame = lastPixelInFrame;
            }

            /// <summary>
            /// The position of the last pixel in the frame.
            /// </summary>
            public int LastPixelInFrame { get; private set; }

            /// <summary>
            /// The step size of the window between frames.
            /// </summary>
            public int WindowStepSize { get; private set; }

            /// <summary>
            /// The size of the scanning window in pixel index, (the image width).
            /// </summary>
            public int WindowWidth { get; private set; }

            /// <summary>
            /// he number of values in a stack
            /// </summary>
            public int NumberOfValuesInAStack { get; private set; }

            /// <summary>
            /// he number of stacks in a vector
            /// </summary>
            public int NumberOfStacksInAVector { get; set; }

            /// <summary>
            /// he number of values in a vector
            /// </summary>
            public int NumberOfValuesInAVector { get; private set; }

            /// <summary>
            /// The calling stacking collector
            /// </summary>
            public StackingCollector  Collector { get; private set; }

            /// <summary>
            /// The data vectors (the pixel values) of the resulting stacks.
            /// </summary>
            public byte* Vectors { get; private set; }

            /// <summary>
            /// The pixel position offset where to start storing the data?
            /// </summary>
            public int PixelIndexOffset { get; private set; }

            /// <summary>
            /// Executes the stacking collection of data.
            /// </summary>
            /// <param name="stackIndex">The image stack index</param>
            public unsafe void Run(int stackIndex)
            {
                ImageData img = Collector.ImageStack[stackIndex];
                fixed (byte* pdata = img.Data)
                {
                    // the position of the pixel in the image.
                    int pixelPos = Collector.GetPixelPosition(img.Index);

                    // running through all the stacks in the vector.
                    for (int vi = 0; vi < Collector.ConstantAxisSize; vi++) // the perpandicular to the scan direction.
                    {
                        // assigning the values in the stack by running through all
                        // the appropriate pixel indexis. (by the step size).
                        for (int pIndex = 0; pIndex < WindowStepSize; pIndex++)
                        {
                            int pixelIndex;
                            int imageIndex;

                            // calculate the inner row/column pixel index.
                            pixelIndex = (LastPixelInFrame - pixelPos) + pIndex; // (offset)+ pixel index.

                            // if reversed then need to go to the oppoxite index for the image.
                            // the end of the row/column.
                            if (Collector.SeriesReversed)
                                pixelIndex = Collector.MovingAxisSize - pixelIndex;

                            // moving to the correct image location.
                            if (Collector.SumVertical)
                            {
                                imageIndex = WindowWidth * (pixelIndex) + vi;
                            }
                            else
                            {
                                imageIndex = WindowWidth * vi + pixelIndex;
                            }

                            int location = pIndex * NumberOfValuesInAVector +
                                vi * NumberOfValuesInAStack + stackIndex - 1;

                            // setting the vecs data.
                            Vectors[location] = pdata[imageIndex];
                        }
                    }
                }
            }
        }

        #endregion
    }
}
