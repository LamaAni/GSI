using GSI.IP;
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
    /// Creates a file stream reader to load the data from the filestream.
    /// </summary>
    public class SpectrumStreamReader : SpectrumStreamWorker
    {
        public SpectrumStreamReader(Stream from)
            : base(from, null)
        {
            Initialize();
        }

        #region consts and static

        /// <summary>
        /// Opens an FFT filestream.
        /// </summary>
        /// <param name="flename">The filename</param>
        /// <param name="autoLoadExternalPreview">if true, external previews will be loaded automatically
        /// if they exist.</param>
        /// <param name="previewExtention">The extention where to search the preview.</param>
        /// <returns></returns>
        public static SpectrumStreamReader Open(string filename, FileMode mode = FileMode.Open)
        {
            if (!File.Exists(filename))
                return null;

            SpectrumStreamReader reader = new SpectrumStreamReader(File.Open(filename, mode));
            return reader;
        }

        #endregion

        #region static methods

        /// <summary>
        /// Creates a stream reader to the file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SpectrumStreamReader Open(string filename)
        {
            if (!File.Exists(filename))
                throw new Exception("File dose not exist.");
            FileStream strm = File.Open(filename, FileMode.Open);
            return new SpectrumStreamReader(strm);
        }

        #endregion

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

        #region Reading

        /// <summary>
        /// Reads a number of data pixels.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public T[, ,] ReadSpectrumPixels<T>(int x, int y, int width, int height)
        {
            if (typeof(T) != typeof(double) && typeof(T) != typeof(float))
            {
                throw new Exception("Can only convert to types float and double at this time.");
            }

            if (!IsInBounds(x + width, y + width) || !IsInBounds(x, y))
                throw new Exception("Location of of bounds.");

            long totalNumberOfBytesToRead = (long)(width * height) *
                (long)(Settings.NumberOfPrecisionBytes * Settings.FftDataSize);

            //GetDataPositionOffset(width + x, y + height - 1) - GetDataPositionOffset(x, y);
            if (totalNumberOfBytesToRead > int.MaxValue)
                throw new Exception("Too much data, please select a smaller region");
            if (totalNumberOfBytesToRead < 0)
                throw new Exception("All input values must be positive.");

            SeekToPixelPosition(x, y);

            // reading the bytes.
            BinaryReader reader = new BinaryReader(BaseStream);
            byte[] bytes = reader.ReadBytes((int)totalNumberOfBytesToRead);
            bool isTargetDouble = typeof(T) == typeof(double);
            if (Settings.IsDoublePrecision)
            {
                if (isTargetDouble)
                {
                    T[, ,] rtvals = new T[height, width, Settings.FftDataSize];
                    Buffer.BlockCopy(bytes, 0, rtvals, 0, bytes.Length);
                    return rtvals;
                }
                else
                {
                    double[] vals = new double[bytes.Length / Settings.NumberOfPrecisionBytes];
                    Buffer.BlockCopy(bytes, 0, vals, 0, bytes.Length);
                    float[] fvals = vals.Select(v => (float)v).ToArray();
                    T[, ,] rtvals = new T[height, width, Settings.FftDataSize];
                    Buffer.BlockCopy(fvals, 0, rtvals, 0, sizeof(float) * fvals.Length);
                    return rtvals;
                }
            }
            else
            {
                if (!isTargetDouble)
                {
                    T[, ,] rtvals = new T[height, width, Settings.FftDataSize];
                    Buffer.BlockCopy(bytes, 0, rtvals, 0, bytes.Length);
                    return rtvals;
                }
                else
                {
                    float[] vals = new float[bytes.Length / Settings.NumberOfPrecisionBytes];
                    Buffer.BlockCopy(bytes, 0, vals, 0, bytes.Length);
                    double[] dvals = vals.Select(v => (double)v).ToArray();
                    T[, ,] rtvals = new T[height, width, Settings.FftDataSize];
                    Buffer.BlockCopy(dvals, 0, rtvals, 0, sizeof(double) * dvals.Length);
                    return rtvals;
                }
            }
        }

        /// <summary>
        /// Reads a number of data pixels.
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public T[] ReadSpectrumPixel<T>(int x, int y)
        {
            bool isTargetDouble = typeof(T) == typeof(double);
            T[, ,] vals = ReadSpectrumPixels<T>(x, y, 1, 1);
            T[] rtvals = new T[vals.Length];
            Buffer.BlockCopy(vals, 0, rtvals, 0, vals.Length * (isTargetDouble ? sizeof(double) : sizeof(float)));
            return rtvals;
        }

        /// <summary>
        /// Reads a spectrum pixel.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public float[] ReadSpectrumPixel(int x, int y)
        {
            return ReadSpectrumPixel<float>(x, y);
        }


        /// <summary>
        /// Read a collection of spectrum pixels.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public float[, ,] ReadSpectrumPixels(int x, int y, int width, int height)
        {
            return ReadSpectrumPixels<float>(x, y, width, height);
        }

        #endregion

    }
}
