using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Storage.Spectrum
{
    /// <summary>
    /// Called to process the entire specrum stream block by block.
    /// </summary>
    public class SpectrumStreamProcessor : SpectrumStreamWorker
    {
        public SpectrumStreamProcessor(Stream from)
            : base(from, null)
        {
            Initialize();
        }

        #region initialization

        /// <summary>
        /// Initializes the stream.
        /// </summary>
        public void Initialize()
        {
            ReadHeader();
            // reading the settings if any. If settings do not exist then throw error.
            Settings = ReadJsonObject() as SpectrumStreamSettings;
            if (Settings == null)
                throw new Exception("Expected settings json after 8 bits. Error in format.");
        }

        #endregion

        #region members

        /// <summary>
        /// If true the is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If true then has been aborted.
        /// </summary>
        public bool IsAborted { get; private set; }

        /// <summary>
        /// Called when a data block has been compleated.
        /// </summary>
        public event EventHandler<StreamProcessorDataReadEventArgs> BlockComplete;

        #endregion

        #region processing

        /// <summary>
        /// Abort the running stream.
        /// </summary>
        public void Abort() { IsAborted = true; }

        /// <summary>
        /// Calls to process the spectrum stream, in its entierty.
        /// </summary>
        /// <param name="doProcessing">Calls to do the processing</param>
        /// <param name="async"></param>
        public void DoSpectrumProcessing(SpectrumProcessingDelegate doProcessing, bool async,
            double MemorySizeInMB = 500)
        {
            // collecting data.
            long maxNumberOfPixelsToRead = (long)Math.Floor(MemorySizeInMB * 1e6 /
                (Settings.NumberOfPrecisionBytes * Settings.FftDataSize));

            if (maxNumberOfPixelsToRead > int.MaxValue/(Settings.NumberOfPrecisionBytes*Settings.FftDataSize))
                throw new Exception("Memory size too large for processing due to cap on array size.");

            Seek(DataStartPosition);

            IsAborted = false;
            IsRunning = true;

            // call to process.
            long pixelOffset = 0;
            byte[] buffer = null;
            float[] vals = null;
            double[] dvals=null;
            Action f = () =>
            {
                BinaryReader reader = new BinaryReader(BaseStream);
                do
                {
                    int nread = (int)(Settings.NumberOfPixels - pixelOffset > maxNumberOfPixelsToRead ?
                        maxNumberOfPixelsToRead : Settings.NumberOfPixels - pixelOffset);

                    if (nread == 0)
                        break;

                    // read size and buffer.
                    int totalNumberOfBytes = nread * Settings.NumberOfPrecisionBytes * Settings.FftDataSize;
                    if (buffer == null || buffer.Length != totalNumberOfBytes)
                    {
                        buffer = new byte[totalNumberOfBytes];
                        vals = new float[nread * Settings.FftDataSize];
                        if (Settings.IsDoublePrecision)
                            dvals = new double[nread * Settings.FftDataSize];
                    }

                    if (IsAborted)
                        break;

                    // reading from the stream.
                    reader.Read(buffer, 0, buffer.Length);

                    if (IsAborted)
                        break;

                    // reading into the values array.
                    if (Settings.IsDoublePrecision)
                    {
                        Buffer.BlockCopy(buffer, 0, dvals, 0, buffer.Length);
                        ConvertDoubleToFloat(ref dvals, ref vals);
                    }
                    else
                    {
                        Buffer.BlockCopy(buffer, 0, vals, 0, buffer.Length);
                    }
                    int x = (int)(pixelOffset % Settings.Width);
                    int y = (int)(pixelOffset / Settings.Width);

                    // calling the processing function.
                    doProcessing(this, x, y, nread, vals);

                    if (BlockComplete != null)
                        BlockComplete(this, new StreamProcessorDataReadEventArgs(x, y, nread, vals));

                    pixelOffset += nread;
                }
                while (true);
                IsRunning = false;

            };

            if (async)
                Task.Run(f);
            else f();
            
        }

        #endregion


        #region unsafe conversion methods

        /// <summary>
        /// Fast unsafe conversion of the source array to target array.
        /// </summary>
        /// <param name="source">The source </param>
        /// <param name="target">The target</param>
        public unsafe void ConvertDoubleToFloat(ref double[] source, ref float[] target, int offset = 0, int count = -1)
        {
            if (count < 0)
            {
                count = source.Length - offset;
            }

            int maxL = offset + count;
            if (source.Length < maxL || target.Length < maxL)
            {
                throw new Exception("Out of bounds of target or source array. ");
            }
            fixed (double* pSource = source)
            fixed (float* pTarget = target)
            {
                for (int i = offset; i < maxL; i++)
                    pTarget[i] = (float)pSource[i];
            }
        }

        #endregion
    }

    public class StreamProcessorDataReadEventArgs : EventArgs
    {
        /// <summary>
        /// Events args called when processing a block is compleate, This data should not be stored!.
        /// </summary>
        /// <param name="reader">The spectrum reader.</param>
        /// <param name="x">Start position in pixels. </param>
        /// <param name="y">Start position in pixels. </param>
        /// <param name="length">Number of pixels read from start position. (May be more then 1 line)</param>
        /// <param name="values">The spectrum values. [1...N 1...N]</param>
        public StreamProcessorDataReadEventArgs(int x, int y, int length, float[] values)
        {
            X = x;
            Y = y;
            Length = length;
            Values = values;
        }

        #region members

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Length { get; private set; }
        public float[] Values { get; private set; }

        #endregion
    }


    /// <summary>
    /// Process the spectrum block by block.
    /// </summary>
    /// <param name="reader">The spectrum reader.</param>
    /// <param name="x">Start position in pixels. </param>
    /// <param name="y">Start position in pixels. </param>
    /// <param name="length">Number of pixels read from start position. (May be more then 1 line)</param>
    /// <param name="values">The spectrum values. [1...N 1...N]</param>
    public delegate void SpectrumProcessingDelegate(SpectrumStreamProcessor processor, int x, int y, int length, float[] values);
}
