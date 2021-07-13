using GSI.Coding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Processing
{
    public class DataProcessesor : IDisposable
    {
        /// <summary>
        /// Processes the image to create the final forier transform 
        /// and/or the stacked image.
        /// </summary>
        /// <param name="source">The image source</param>
        public DataProcessesor(Stream source)
            : this(new StackingReader(source))
        {
        }

        /// <summary>
        /// Processes the image to create the final forier
        /// </summary>
        /// <param name="source">he image source</param>
        public DataProcessesor(StackingReader source)
        {
            source.Initialize();
            Source = source;
            this.MemoryMaxSizeInBytes = DefaultMemoryMaxSizeInBytes;
        }

        #region static members

        /// <summary>
        /// The memory size to read in each step. Cannot be less then one vector size.
        /// </summary>
        public static int DefaultMemoryMaxSizeInBytes = 100000000;

        #endregion

        #region members

        /// <summary>
        /// Get the pixel index for the start of the vector data.
        /// </summary>
        /// <param name="vectorIndex"></param>
        /// <returns></returns>
        protected int GetNPixelOffsetFromVectorIndex(int vectorIndex)
        {
            int x, y;
            return GetNPixelOffsetFromVectorIndex(vectorIndex, out x, out y);
        }

        /// <summary>
        /// Get the pixel index for the start of the vector data.
        /// </summary>
        /// <param name="vectorIndex"></param>
        /// <returns></returns>
        protected int GetNPixelOffsetFromVectorIndex(int vectorIndex, out int x, out int y)
        {
            // getting the correct line. (Sets the y offset to write to).
            x = (vectorIndex / Source.LineSize) * Source.VectorSize;
            y = (vectorIndex % Source.LineSize);
            int yoffset = y * Source.VectorSize * Source.NumberOfLines;
            int numberOfPixelOffset = yoffset + x;
            return numberOfPixelOffset;
        }

        /// <summary>
        /// If true then running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If truen then aborted.
        /// </summary>
        public bool Aborted { get; private set; }

        /// <summary>
        /// The source reader.
        /// </summary>
        public StackingReader Source { get; private set; }

        /// <summary>
        /// Called when vector processing is complete.
        /// </summary>
        public event EventHandler<VectorCompleateEventArgs> VectorComplete;

        /// <summary>
        /// The memory size to use when 
        /// </summary>
        public int MemoryMaxSizeInBytes { get; set; }
        
        #endregion

        #region methods

        /// <summary>
        /// Call to abort the current run.
        /// </summary>
        public void Abort()
        {
            Aborted = true;
        }

        /// <summary>
        /// Called to do data processing on read vectors.
        /// </summary>
        /// <param name="onreceivedVectors">Action to ttake when vectors received, (start index, number read, data vectors multidim, data vector)</param>
        /// <param name="receivedVector">Called to do some actions when vectors are received.</param>
        /// <param name="useBuffers">If true, the same memory buffers will be used for faster reading. 
        /// Otherwise a new memory buffer will be created on each read (slower).</param>
        public void DoDataProcessing(Action<int, int, byte[, ,], byte[]> onreceivedVectors, bool useBuffers = true)
        {
            // set the aborted to false.
            Aborted = false;

            // calculating the number of vectors permitted by the memory cap.
            int numberOfVectorsPerBlock = GetMaxNumberOfVectorsToLoad();
            int totalVectors = Source.LineSize * Source.NumberOfLines;

            if (numberOfVectorsPerBlock == 0)
                throw new Exception("The maximal memory size dose not allow to read even one vector from this source. Increase the max memory size.");

            // limit if can read all vectors at once.
            if (numberOfVectorsPerBlock > totalVectors)
                numberOfVectorsPerBlock = totalVectors;

            byte[, ,] vectors = null;
            byte[] readbuffer = new byte[Source.StackSize * Source.VectorSize * numberOfVectorsPerBlock];

            int curVectorIndex = 0;
            double elapsedTime = 0;
            CodeTimer timer = new CodeTimer();
            timer.Start();
            // reading all the lines. 
            while (!Source.ReachedEnd)
            {
                // if the data has been aborted the no need to continue.
                if (Aborted)
                    break;
                timer.Reset();
                timer.Mark("begin");
                // reading the number of vectors in the line.
                if (!useBuffers || vectors == null)
                    vectors = new byte[numberOfVectorsPerBlock, Source.VectorSize, Source.StackSize];

                // reading the number of vectors for this round.
                int n = numberOfVectorsPerBlock > Source.NumberOfVectorsToEnd ? Source.NumberOfVectorsToEnd : numberOfVectorsPerBlock;


                // Reading the vectors.
                Source.ReadVectors(n, ref vectors, ref readbuffer);
                timer.Mark("Reading");

                // if the data has been aborted the no need to continue.
                if (Aborted)
                    break;

                if (onreceivedVectors != null)
                    onreceivedVectors(curVectorIndex, n, vectors, readbuffer);

                timer.Mark("Processing");

                timer.Mark("total", true);

                // advancing the total number of vectors.
                curVectorIndex += n;

                if (VectorComplete != null)
                {
                    elapsedTime += timer["total"].TotalMilliseconds;
                    double mspervec = elapsedTime * 1.0 / curVectorIndex;
                    double eta = (totalVectors - curVectorIndex) * mspervec;

                    VectorCompleateEventArgs args = new VectorCompleateEventArgs(
                        curVectorIndex, totalVectors, TimeSpan.FromMilliseconds(eta));

                    VectorComplete(this, args);
                }

            }
        }

        public virtual int GetMaxNumberOfVectorsToLoad()
        {
            return (int)Math.Floor(MemoryMaxSizeInBytes * 1.0 / (Source.StackSize * Source.VectorSize));
        }

        #endregion

        #region IDisposable Members

        public void Close()
        {
            Source.BaseStream.Close();
        }

        public void Dispose()
        {
            Source.Dispose();
        }

        #endregion
    }


    /// <summary>
    /// Event args to be called when the event is compleated.
    /// </summary>
    public class VectorCompleateEventArgs : EventArgs
    {
        public VectorCompleateEventArgs(int compleated, int max, TimeSpan eta)
        {
            Compleated = compleated;
            Max = max;
            Eta = eta;
        }

        /// <summary>
        /// The number of compleated vectors.
        /// </summary>
        public int Compleated { get; private set; }

        /// <summary>
        /// The max number of vectors.
        /// </summary>
        public int Max { get; private set; }

        /// <summary>
        /// The expected eta.
        /// </summary>
        public TimeSpan Eta { get; private set; }
    }
}
