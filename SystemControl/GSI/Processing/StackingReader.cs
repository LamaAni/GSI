using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Processing
{
    /// <summary>
    /// Reads the raw stacking image data, vector by vector.
    /// </summary>
    public class StackingReader: IDisposable
    {
        /// <summary>
        /// Creates the vector reader.
        /// </summary>
        /// <param name="strm"></param>
        /// <param name="options"></param>
        public StackingReader(Stream strm)
        {
            Reader = new BinaryReader(strm);
        }

        #region Members

        /// <summary>
        /// The internal readeer associated with the stream.
        /// </summary>
        public BinaryReader Reader { get; private set; }

        /// <summary>
        /// The stream to read into.
        /// </summary>
        public Stream BaseStream { get { return Reader.BaseStream; } }

        /// <summary>
        /// The number of vectors in a line.
        /// </summary>
        public int LineSize { get; private set; }

        /// <summary>
        /// The number of pixels in a vector.
        /// </summary>
        public int VectorSize { get; private set; }

        /// <summary>
        /// The nuber of values in the pixel.
        /// </summary>
        public int StackSize { get; private set; }

        /// <summary>
        /// The pixel size of the image.
        /// </summary>
        public double PixelSize { get; private set; }

        /// <summary>
        /// The step size in the stacking collector, in pixels.
        /// </summary>
        public double StepSize { get; private set; }

        /// <summary>
        /// The number of lines in the stream.
        /// </summary>
        public int NumberOfLines { get; private set; }

        /// <summary>
        /// If true not all the lines were compleated.
        /// </summary>
        public bool HasPartialLastLine { get; private set; }

        /// <summary>
        /// The total number of bytes in a single line.
        /// </summary>
        public long NumberOfBytesInAVector { get; private set; }

        /// <summary>
        /// The total number of bytes in a single line.
        /// </summary>
        public long NumberOfBytesInALine { get; private set; }

        /// <summary>
        /// The total number of bytes in the header.
        /// </summary>
        public long NumberOfBytesInTheHeader { get; private set; }

        /// <summary>
        /// The last position for a vector read.
        /// </summary>
        public long EndPosition { get; private set; }

        /// <summary>
        /// if true then reached of the read.
        /// </summary>
        public bool ReachedEnd
        {
            get
            {
                return BaseStream.CanSeek ? BaseStream.Position >= EndPosition : true;
            }
        }

        /// <summary>
        /// The number of vectors that are left from the current position to the end of the stream.
        /// </summary>
        public int NumberOfVectorsToEnd
        {
            get
            {
                return (int)Math.Floor((EndPosition - BaseStream.Position) * 1.0 / (StackSize * VectorSize));
            }
        }

        /// <summary>
        /// If true the readere has been initialized. (Will hapeen on the first vector read).
        /// </summary>
        public bool Initialized { get; private set; }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Intiailzies the stream to get the appropreate stream parameters.
        /// </summary>
        public virtual void Initialize()
        {
            if (Initialized)
                return;

            Initialized = true;

            Reader.BaseStream.Seek(0, SeekOrigin.Begin);

            LineSize = Reader.ReadInt32();
            VectorSize = Reader.ReadInt32();
            StackSize = Reader.ReadInt32();
            StepSize = Reader.ReadDouble();
            PixelSize = Reader.ReadDouble();

            NumberOfBytesInTheHeader = sizeof(int) * 3 + sizeof(double) * 2; // header size.
            NumberOfBytesInAVector = VectorSize * StackSize;
            NumberOfBytesInALine = LineSize * NumberOfBytesInAVector;
            double numberOfLines = (Reader.BaseStream.Length - NumberOfBytesInTheHeader) * 1.0 / NumberOfBytesInALine;
            NumberOfLines =
                (int)Math.Ceiling(numberOfLines);
            HasPartialLastLine = Math.Round(numberOfLines) != numberOfLines;

            int maxNumberOfVectors = (int)Math.Floor((Reader.BaseStream.Length - NumberOfBytesInTheHeader) * 1.0 / NumberOfBytesInAVector);
            EndPosition = NumberOfBytesInAVector * maxNumberOfVectors + NumberOfBytesInTheHeader;
        }

        #endregion

        #region reading methods

        /// <summary>
        /// Reads a vector from the stream.
        /// </summary>
        /// <param name="vectors">If null a new buffer will be created. Otherwise reads into the buffer.</param>
        /// <param name="readbuffer">a buffer used for the data reading. The buffer must be of the size of n*StackSize*VectorSize. Faster reading</param>
        /// <param name="n">The number of vectors to read.</param>
        /// <returns></returns>
        public void ReadVectors(int n, ref byte[, ,] vectors, ref byte[] readbuffer)
        {
            // call to initialize if needed. (Called once).
            if (!Initialized)
                Initialize();

            if (vectors == null)
                vectors = new byte[n, VectorSize, StackSize];
            else
            {
                if (vectors.GetLength(0) < n || vectors.GetLength(1) != VectorSize || vectors.GetLength(2) != StackSize)
                    throw new Exception("Invalid vectors size. buffer size must be, [>=n,==VectorSize,==StackSize] ");
            }

            if (readbuffer == null)
                readbuffer = new byte[n * VectorSize * StackSize];
            else
            {
                if (readbuffer.LongLength < n * VectorSize * StackSize)
                    throw new Exception("Invalid read buffer size. buffer size must be, n*VectorSize*StackSize ");
            }

            // reads the data into the buffer.
            Reader.Read(readbuffer, 0, n * VectorSize * StackSize);
            System.Buffer.BlockCopy(readbuffer, 0, vectors, 0, n * VectorSize * StackSize);
        }

        /// <summary>
        /// Reads a vector from the stream.
        /// </summary>
        /// <param name="vectors">If null a new buffer will be created. Otherwise reads into the buffer.</param>
        /// <param name="readbuffer">a buffer used for the data reading. The buffer must be of the size of n*StackSize*VectorSize. Faster reading</param>
        /// <param name="n">The number of vectors to read.</param>
        /// <returns></returns>
        public void ReadVectors(int n, ref byte[, ,] vectors)
        {
            byte[] readbuffer=null;
            ReadVectors(n, ref vectors, ref readbuffer);
        }

        /// <summary>
        /// Reads a vector from the stream.
        /// </summary>
        /// <param name="n">The number of vectors to read.</param>
        /// <returns>The vectors read</returns>
        public byte[, ,] ReadVectors(int n)
        {
            byte[] readbuffer = null;
            byte[, ,] vectors = null;
            ReadVectors(n, ref vectors, ref readbuffer);
            return vectors;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Reader.Close();
            Reader.Dispose();
        }

        #endregion
    }
}
