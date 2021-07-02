using GSI.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Processing
{
    /// <summary>
    /// Writes raw data in the form of a stacking collection.
    /// </summary>
    public class StackingWriter : IDisposable
    {
        /// <summary>
        /// Creates a stacking writer that writes to a stream.
        /// </summary>
        /// <param name="strm"></param>
        /// <param name="async">If true the data will be written asynchronically form the main thread.</param>
        public StackingWriter(Stream strm, bool async=false)
        {
            Writer = new BinaryWriter(strm);
            Async = async;
        }

        #region Members

        /// <summary>
        /// Internal writer.
        /// </summary>
        public BinaryWriter Writer { get; private set; }

        /// <summary>
        /// The stream to write to.
        /// </summary>
        public Stream BaseStream { get { return Writer.BaseStream; } }

        /// <summary>
        /// If true the stacking writer has been initialized.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// The size of a line in number of vectors.
        /// </summary>
        public int LineSize { get; private set; }

        /// <summary>
        /// The number of values in each pixel.
        /// </summary>
        public int StackSize { get; private set; }

        /// <summary>
        /// The number of pixels in a vector.
        /// </summary>
        public int VectorSize { get; private set; }

        /// <summary>
        /// The pixel size of the image.
        /// </summary>
        public double PixelSize { get; private set; }

        /// <summary>
        /// The step size in the stacking collector, in pixels.
        /// </summary>
        public double StepSize { get; private set; }

        /// <summary>
        /// If true the data storing will be done asynchonically.
        /// </summary>
        public bool Async { get; private set; }

        GSI.Coading.ActionQueueExecuter m_pendingWriteCommands = new Coading.ActionQueueExecuter();

        public GSI.Coading.ActionQueueExecuter PendingWriteCommands
        {
            get { return m_pendingWriteCommands; }
        }

        /// <summary>
        /// The number of pending writes to wait for.
        /// </summary>
        public int NumberOfPendingWrites { get { return PendingWriteCommands.PendingCount; } }

        /// <summary>
        /// If true then waiting for all write events to complete.
        /// </summary>
        public bool IsWaitingForWriteEvents { get { return NumberOfPendingWrites > 0; } }

        #endregion

        #region methods

        /// <summary>
        /// Initializes the stacking writer. Sets the stack sizes that will generate the nessescary stacking
        /// collection.
        /// </summary>
        /// <param name="lineSize">the size of a line, in number of vectors</param>
        /// <param name="vectorSize">The size of a vector.</param>
        /// <param name="stackSize">The number of values for each pixel.</param>
        public void WriteInitialize(int lineSize, int vectorSize, int stackSize, double deltaX, double pixelSize)
        {
            if (Initialized)
                throw new Exception("Writer already intialized. See [this].Initialized");

            LineSize = lineSize;
            VectorSize = vectorSize;
            StackSize = stackSize;
            PixelSize = pixelSize;
            StepSize = deltaX;

            Writer.Write(LineSize);
            Writer.Write(VectorSize);
            Writer.Write(StackSize);
            Writer.Write(StepSize);
            Writer.Write(PixelSize);

            this.Initialized = true;
        }

        /// <summary>
        /// Writes a vector to the stack.
        /// </summary>
        /// <param name="vector">2d array of values that represents one stacked row.</param>
        public virtual void WriteVector(byte[,] vector)
        {
            if (!Initialized)
                throw new Exception("Writer must be initialized. See [this].Initialized");

            if (vector.GetLength(0) != VectorSize)
                throw new Exception("Vector size (size(0)) must match the intialization vector size. See [this].VectorSize");

            if (vector.GetLength(1) != StackSize)
                throw new Exception("Stack size (size(1)) must match the intialization stack size. See [this].StackSize");

            // writes the vector.
            byte[] data = new byte[VectorSize * StackSize];

            // copy the data to a new collection, so to allow async storing.
            System.Buffer.BlockCopy(vector, 0, data, 0, VectorSize * StackSize);

            // sending the event to writing, asynced.
            if (Async)
                PendingWriteCommands.AddAction(() => _doDataWritingEvent(data));
            else _doDataWritingEvent(data);
        }

        void _doDataWritingEvent(byte[] data)
        {
            // writing the data to the underlining device.
            Writer.Write(data);
        }

        /// <summary>
        /// Writes all buffered data to the stream. Same as [this].Stream.Flush()
        /// </summary>
        public virtual void Flush ()
        {
            Writer.Flush();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Closes the internal writer.
        /// </summary>
        public void Dispose()
        {
            Flush();
            Writer.Close();
            Writer.Dispose();
        }

        #endregion
    }
}
