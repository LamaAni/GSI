using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSI.Storage;
using System.Drawing;

namespace GSI.IP
{
    /// <summary>
    /// Represents an image stream that can read and write raw images. 
    /// The raw image is stored in float format.
    /// </summary>
    public class ImageStream
    {
        /// <summary>
        /// Creas a memory image stream that holds the image.
        /// Assumes stream was not initialized. This methods Seeks the stream.
        /// </summary>
        /// <param name="width">Width of the image</param>
        /// <param name="height">Height of the image</param>
        /// <param name="baseStream">The stream where the image will be stored</param>
        /// <param name="startOffset">Start offset of the stream, where the image is placed from the beginning of the stream.</param>
        public ImageStream(int width, int height, Stream baseStream = null, long startOffset = 0)
        {
            StartOffset = startOffset;
            Width = width;
            Height = height;

            // initializing stream.
            BaseStream = baseStream == null ? new MemoryStream() : baseStream;
            ValidatePrealocated();

            // seeking to start offset.
            Seek(StartOffset);
            WriteImageHeader(BaseStream, width, height);
            DataStartLocation = Position; // set the start position.
            Seek(StartOffset);
        }

        /// <summary>
        /// Reads a raw image from the stream. (This function seeks the stream).
        /// </summary>
        /// <param name="from">The stream to read from</param>
        /// <param name="startOffset">Start offset of the stream, where the image  is placed from the beginning of the stream.</param>
        public ImageStream(Stream from, long startOffset = 0)
        {
            BaseStream = from;
            StartOffset = startOffset;
            Seek(StartOffset);
            int w, h;
            ReadImageHeader(from, out w, out h);
            Width = w;
            Height = h;
            DataStartLocation = Position;
        }

        #region consts


        /// <summary>
        /// The number of bytes in a preview pixel RGB value.
        /// </summary>
        public const long NumberOfBytesPerPixelValue = sizeof(float);

        /// <summary>
        /// The number of color values in a single preview pixel.
        /// </summary>
        public const long NumberOfColorValuesInPixel = 3;

        /// <summary>
        /// The max number of pixels that can be written or read in one step.
        /// </summary>
        public const int MaxNumberOfPixelsPerWriteOrRead = (int)(int.MaxValue / NumberOfBytesPerPixel);

        /// <summary>
        /// The number of bytes in a preview pixel.
        /// </summary>
        public const long NumberOfBytesPerPixel =
            NumberOfBytesPerPixelValue * NumberOfColorValuesInPixel;

        #endregion

        #region Static

        /// <summary>
        /// Writes an image header to the stream.
        /// </summary>
        /// <param name="strm"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void WriteImageHeader(Stream strm, int width, int height)
        {
            BinaryWriter wr = new BinaryWriter(strm);
            wr.Write(width);
            wr.Write(height);
            wr.Flush();
        }

        /// <summary>
        /// Gets the approximated file size for the width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static long GetFileSize(int width, int height)
        {
            return ((long)(width * height)) * NumberOfBytesPerPixel;
        }

        /// <summary>
        /// Reads the stream header.
        /// </summary>
        /// <param name="strm"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void ReadImageHeader(Stream strm, out int width, out int height)
        {
            BinaryReader reader = new BinaryReader(strm);
            width = reader.ReadInt32();
            height = reader.ReadInt32();
        }

        #endregion

        #region members

        /// <summary>
        /// The width of the image.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the image.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The base stream to read and write to.
        /// </summary>
        public Stream BaseStream { get; private set; }

        /// <summary>
        /// The start offset within the stream where the image is placed.
        /// </summary>
        public long StartOffset { get; private set; }

        /// <summary>
        /// The location within the stream of the data start.
        /// </summary>
        public long DataStartLocation { get; private set; }

        /// <summary>
        /// The position within the stream.
        /// </summary>
        public long Position { get { return BaseStream.Position; } }

        /// <summary>
        /// The number of pixels in the image.
        /// </summary>
        public long NumberOfPixels { get { return ((long)Width) * ((long)Height); } }

        #endregion

        #region Seeking

        /// <summary>
        /// Seeks to a position in the stream.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <param name="forceDirect">The stream is forced to return to the origin position before seeking.</param>
        public void Seek(long offset, SeekOrigin origin = SeekOrigin.Begin, bool forceDirect = false)
        {
            if (forceDirect || origin == SeekOrigin.Current)
            {
                BaseStream.Seek(offset, origin);
            }
            long offsetFromCurrent = origin == SeekOrigin.Begin ? offset - Position : (BaseStream.Length - offset) - Position;
            if (offsetFromCurrent == 0)
                return;
            BaseStream.Seek(offsetFromCurrent, SeekOrigin.Current);
        }

        /// <summary>
        /// Seeks to the image position.
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        public void SeekToImagePosition(int x, int y)
        {
            long xoffset = ((long)x) * NumberOfBytesPerPixel;
            long yoffset = (((long)y) * ((long)Width)) * NumberOfBytesPerPixel;
            long pos = DataStartLocation + yoffset + xoffset;
            if (pos == Position)
                return;
            Seek(pos);
        }
        
        /// <summary>
        /// Seeks to the the data end location.
        /// </summary>
        public void SeekToDataEnd()
        {
            Seek(DataStartLocation + Width * Height * NumberOfBytesPerPixel);
        }

        /// <summary>
        /// Seeks to the the data end location.
        /// </summary>
        public void SeekToDataStart()
        {
            Seek(DataStartLocation);
        }

        #endregion

        #region ip methods

        /// <summary>
        /// Reads through all pixel values and calls the function on the value.
        /// </summary>
        /// <param name="read"></param>
        public void ReadThroughAllPixelValues(Action<float> read, bool useParallel=true)
        {
            Seek(DataStartLocation);
            // reading in chnks of max 500 mb.
            byte[] buffer = null;
            long lengthInBytes = ((long)(this.Width * this.Height)) * NumberOfBytesPerPixel;
            long maxBufferLength = 1000 * 1000000;
            float[] pixelbuffer = null;
            long pos = 0;
            while (pos < lengthInBytes)
            {
                int l = (int)((pos + maxBufferLength > lengthInBytes) ? lengthInBytes - pos : maxBufferLength);
                if (buffer == null || buffer.Length != l)
                {
                    buffer = new byte[l];
                    pixelbuffer = new float[l / NumberOfBytesPerPixelValue];
                }
                BaseStream.Read(buffer, 0, l);
                Buffer.BlockCopy(buffer, 0, pixelbuffer, 0, buffer.Length);
                if (useParallel)
                {
                    Parallel.For(0, pixelbuffer.Length, (i) =>
                    {
                        read(pixelbuffer[i]);
                    });
                }
                else
                {
                    pixelbuffer.Foreach(v => read(v));
                }

                pos += l;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Validates preview stream is preallocated.
        /// </summary>
        public void ValidatePrealocated()
        {
            long requiredLength = StartOffset + GetFileSize(Width, Height);
            if (BaseStream.Length < requiredLength)
            {
                // need to expand the stream size.
                BaseStream.ExtendStream(requiredLength);
                BaseStream.Flush();
            }

        }

        /// <summary>
        /// Reads the image data in the selected region.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="buffer">The buffer to read into</param>
        /// <param name="readBuffer">Used for reading, if null then the read buffer is created</param>
        public void ReadImageData(int x, int y, int width, int height, ref float[] buffer,
            ref byte[] readBuffer)
        {
            // going to the correct position.
            SeekToImagePosition(x, y);
            bool isFullLine = x == 0 && width == Width;

            if (buffer == null || buffer.Length != width * height * ImageStream.NumberOfColorValuesInPixel)
                buffer = new float[width * height * ImageStream.NumberOfColorValuesInPixel];

            if (isFullLine)
            {
                // reading blocks.
                int numberOfBytesInBlock = (int)(buffer.Length * NumberOfBytesPerPixelValue);
                if (readBuffer == null || readBuffer.Length != numberOfBytesInBlock)
                {
                    readBuffer = new byte[numberOfBytesInBlock];
                }
                
                BaseStream.Read(readBuffer, 0, readBuffer.Length);
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, readBuffer.Length);
            }
            else
            {
                // need to read lines.
                int numberOfBytesPerLine = (int)(width * NumberOfBytesPerPixel);

                // reading blocks.
                if (readBuffer == null || readBuffer.Length != numberOfBytesPerLine)
                {
                    readBuffer = new byte[numberOfBytesPerLine];
                }

                // reading the length.
                for (int l = 0; l < height; l++)
                {
                    SeekToImagePosition(x, l + y);
                    BaseStream.Read(readBuffer, 0, readBuffer.Length);
                    Buffer.BlockCopy(readBuffer, 0, buffer, l * numberOfBytesPerLine, readBuffer.Length);
                }
            }
        }

        /// <summary>
        /// Returns the image data in the selected region.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public float[] ReadImageData(int x, int y, int width, int height)
        {
            float[] buffer = null;
            byte[] readBuffer = null;
            ReadImageData(x, y, width, height, ref buffer, ref readBuffer);
            return buffer;
        }

        /// <summary>
        /// Sets the image data according to the position within the image.
        /// </summary>
        /// <param name="buffer">The buffer to read from</param>
        /// <param name="x">Image position in pixel #</param>
        /// <param name="y">Image position in pixel #</param>
        /// <param name="length">Number of pixels to write from this position</param>
        /// <param name="offset">Buffer offset in pixels</param>
        public void SetImageData(float[] buffer, int x, int y, int length, int bufferOffset = 0)
        {
            byte[] byteBuffer = null;
            SetImageData(buffer, x, y, length, ref byteBuffer, bufferOffset);
        }

        /// <summary>
        /// Sets the image data according to the position within the image.
        /// </summary>
        /// <param name="buffer">The buffer to read from</param>
        /// <param name="x">Image position in pixel #</param>
        /// <param name="y">Image position in pixel #</param>
        /// <param name="length">Number of pixels to write from this position</param>
        /// <param name="offset">Buffer offset in pixels</param>
        public void SetImageData(float[] buffer, int x, int y, int length, ref byte[] byteBuffer, int bufferOffset = 0)
        {
            // write full lines.
            SeekToImagePosition(x, y);

            // calculating the size of the image block.
            int totalBytes = (int)(length * NumberOfBytesPerPixel);

            // writing the pixels.
            if (byteBuffer == null || byteBuffer.Length != totalBytes)
            {
                byteBuffer = null;
                GC.Collect();
                byteBuffer = new byte[totalBytes];
            }
            //byte[] byteBuffer = new byte[totalBytes];
            Buffer.BlockCopy(buffer, bufferOffset * (int)NumberOfBytesPerPixel, byteBuffer, 0, byteBuffer.Length);
            BaseStream.Write(byteBuffer, 0, byteBuffer.Length);
        }

        /// <summary>
        /// Sets the image data according to the position within the image.
        /// </summary>
        /// <param name="buffer">The buffer to read from</param>
        /// <param name="x">Image position in pixel #</param>
        /// <param name="y">Image position in pixel #</param>
        /// <param name="width">Number of pixels</param>
        /// <param name="height">Number of pixels</param>
        public void SetImagePixels(float[] buffer, int x, int y, int width, int height)
        {
            if (x + width > Width || y + height > Height)
                throw new Exception("Out of bounds.");

            if (buffer.LongLength < ((long)width) * ((long)height) * NumberOfColorValuesInPixel)
                throw new Exception("Data size dose not match the number of pixels.");

            if (buffer.LongLength * NumberOfBytesPerPixelValue > MaxNumberOfPixelsPerWriteOrRead)
                throw new Exception("Data size if too large. See MaxNumberOfPixelsPerWriteOrRead.");


            if (x == 0 && width == Width || height == 1)
            {
                SetImageData(buffer, x, y, width * height, 0);
                //// write full lines.
                //SeekToImagePosition(x, y);

                //// calculating the size of the image block.
                //int totalBytes = (int)(buffer.Length * NumberOfBytesPerPixelValue);

                //// writing the pixels.
                //byte[] byteBuffer = new byte[totalBytes];
                //Buffer.BlockCopy(buffer, 0, byteBuffer, 0, byteBuffer.Length);
                //BaseStream.Write(byteBuffer, 0, byteBuffer.Length);
            }
            else
            {
                // writing each line seperatly.
                byte[] byteBuffer = null;
                for (int i = 0; i < y - height; i++)
                {
                    SetImageData(buffer, x, y + i, width, ref byteBuffer, i * width);
                }
            }
        }
        
        /// <summary>
        /// Flushes the data to the underlining buffer.
        /// </summary>
        public void Flush()
        {
            BaseStream.Flush();
        }

        #endregion

        #region image generation.

        /// <summary>
        /// Saves the image to a jpeg.
        /// </summary>
        public void Save(DoTileStorage doStore, int tileWidth = 4000, int tileHeight = 4000)
        {
            // calculating the number of tiles.
            RawImageConverter converter = new RawImageConverter(this, tileWidth, tileHeight);
            converter.Convert(doStore, 0, 0, Width, Height);
        }

        /// <summary>
        /// Saves the image to an array of images.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="format"></param>
        /// <param name="tileWidth"></param>
        /// <param name="tileHeight"></param>
        public void Save(string filename, ImageFormat format, int tileWidth = 4000, int tileHeight = 4000)
        {
            string extention = Path.GetExtension(filename);
            string path = Path.GetDirectoryName(filename);
            string prefex = Path.GetFileNameWithoutExtension(filename);
            if (path.Trim() != "")
                path = path + "\\";
            Save((info, img) =>
            {
                img.Save(path + prefex + (info.TotalNumberOfConvertTiles == 0 ? "" : "." + info.ImageIndex) + extention, format);
            }, tileWidth, tileHeight);
        }

        #endregion
    }
}
