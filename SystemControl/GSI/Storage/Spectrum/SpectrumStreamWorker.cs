using GSI.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Storage.Spectrum
{
    /// <summary>
    /// A spectrum stream worker that can manipulate a spectrum stream.
    /// </summary>
    public abstract class SpectrumStreamWorker : IDisposable
    {
        /// <summary>
        /// Creates a dummy spectrum stream worker.
        /// </summary>
        public SpectrumStreamWorker(Stream strm, SpectrumStreamSettings settings)
        {
            BaseStream = strm;
            Settings = settings;
        }

        #region members

        /// <summary>
        /// The settings associated with the stream.
        /// </summary>
        public SpectrumStreamSettings Settings { get; protected set; }

        /// <summary>
        /// The data start position.
        /// </summary>
        public long DataStartPosition { get; protected set; }

        /// <summary>
        /// The target stream.
        /// </summary>
        public Stream BaseStream { get; private set; }

        /// <summary>
        /// The position within the base stream.
        /// </summary>
        public long Position { get { return BaseStream.Position; } }

        #endregion

        #region vector to pixel

        /// <summary>
        /// Get the pixel index for a specified vector data, where the vector data is in 
        /// raw format. (Stripes).
        /// </summary>
        /// <param name="vectorIndex">The index of the vector in the raw strips format.</param>
        /// <returns>Pixel offset in number of pixels from the beginning for the storeage location.</returns>
        protected long GetPixelFromVectorIndex(int vectorIndex)
        {
            int x, y;
            return GetPixelFromVectorIndex(vectorIndex, out x, out y);
        }

        /// <summary>
        /// Get the pixel index for a specified vector data, where the vector data is in 
        /// raw format. (Stripes).
        /// </summary>
        /// <param name="vectorIndex"></param>
        /// <returns>Pixel offset in number of pixels from the beginning for the storeage location.</returns>
        protected long GetPixelFromVectorIndex(int vectorIndex, out int x, out int y)
        {
            // getting the correct line. (Sets the y offset to write to).
            x = (vectorIndex / Settings.LineSize) * Settings.VectorSize;
            y = (vectorIndex % Settings.LineSize);
            long yoffset = (long)y * (long)Settings.Width;
            long numberOfPixelOffset = yoffset + (long)x;
            return numberOfPixelOffset;
        }


        /// <summary>
        /// Returns the data position offset from x,y pixel locations.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public long GetPixelOffset(int x, int y)
        {
            return (long)(y * Settings.Width + x);
        }

        /// <summary>
        /// Returns the data offset from pixel offset.
        /// </summary>
        /// <param name="pixelOffset"></param>
        /// <returns></returns>
        public long PixelOffsetToDataOffset(long pixelOffset)
        {
            return DataStartPosition + 
                pixelOffset * (long)(Settings.FftDataSize * Settings.NumberOfPrecisionBytes);
        }

        /// <summary>
        /// Check if the pixel index is in bounds of the stream.
        /// (Without taking into account settings).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool IsInBounds(int x, int y)
        {
            long pos = PixelOffsetToDataOffset(GetPixelOffset(x, y));
            if (pos > BaseStream.Length)
                return false;
            return true;
        }

        #endregion

        #region seek methods

        /// <summary>
        /// Seeks the stream to the specfied position.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        public void Seek(long offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            BaseStream.Seek(offset, origin);
        }

        /// <summary>
        /// Seeks the data to a specific pixel offset position.
        /// </summary>
        /// <param name="pixelOffset"></param>
        public void SeekToPixelPosition(long pixelOffset)
        {
            this.Seek(PixelOffsetToDataOffset(pixelOffset));
        }

        /// <summary>
        /// Seeks the data to a specific pixel position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SeekToPixelPosition(int x, int y)
        {
            this.SeekToPixelPosition(GetPixelOffset(x, y));
        }

        /// <summary>
        /// Seeks the data to raw format vector index position.
        /// </summary>
        /// <param name="vectorIndex"></param>
        public void SeekToVectorPosition(int vectorIndex)
        {
            this.SeekToPixelPosition(GetPixelFromVectorIndex(vectorIndex));
        }

        #endregion

        #region stream methods

        /// <summary>
        /// Call to flush the base stream.
        /// </summary>
        public void Flush()
        {
            BaseStream.Flush();
        }

        #endregion

        #region IDisposable Members

        public void Close()
        {
            if (BaseStream.CanSeek)
                BaseStream.Close();
        }

        public void Dispose()
        {
            Close();
            BaseStream = null;
        }

        #endregion

        #region writing and reading

        /// <summary>
        /// Writes the location members that are at the beginning of the stream.
        /// </summary>
        protected void WriteHeader()
        {
            // writing predefined.
            Seek(0);
            BinaryWriter wr = new BinaryWriter(BaseStream);
            wr.Write(DataStartPosition);
            wr.Flush();
        }

        /// <summary>
        /// Writes a json serializeable to the stream.
        /// </summary>
        /// <param name="o"></param>
        protected void WriteJsonSerializable(IJsonObject o)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(o.ToJson());
            byte[] typename = Encoding.UTF8.GetBytes(o.GetType().ToString());
            BinaryWriter wr = new BinaryWriter(BaseStream);
            wr.Write(typename.Length);
            wr.Write(typename);
            wr.Write(bytes.Length);
            wr.Write(bytes);
            wr.Flush();
        }

        /// <summary>
        /// Reads the header members.
        /// </summary>
        protected void ReadHeader()
        {
            Seek(0);
            // reading the start positon for the data.
            BinaryReader reader = new BinaryReader(BaseStream);
            DataStartPosition = reader.ReadInt64();
        }

        /// <summary>
        /// Reads a json object from the stream.
        /// </summary>
        /// <returns></returns>
        public IJsonObject ReadJsonObject()
        {
            // readint the type if any.
            BinaryReader reader = new BinaryReader(BaseStream);
            int typenamelength = reader.ReadInt32();
            string typename = Encoding.UTF8.GetString(reader.ReadBytes(typenamelength));
            Type t = Type.GetType(typename);
            if (t.GetInterface("IJsonObject") == null)
            {
                return null;
            }
            int datalength = reader.ReadInt32();
            string data = Encoding.UTF8.GetString(reader.ReadBytes(datalength));
            return data.FromJson(t);
        }

        /// <summary>
        /// Validates that the file size matches the expected number of pixels and thier data.
        /// </summary>
        public void ValidatePrealocated()
        {
            long l = DataStartPosition + Settings.SpectrumDataSizeInBytes - BaseStream.Length;
            if (l > 0)
                BaseStream.ExtendStream(l);
        }

        #endregion
    }
}
