using GSI.Coding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GSI.IP
{
    /// <summary>
    /// An image stream that has a preview stream capability and has internal stackings of preview stream
    /// for each preview zoom level. The number of zoom levels are determined by the number of
    /// pixels in the image, and set number of pixels per zoon level (determines accurecy of zoom).
    /// The preview stream is composed of multiple Image streams.
    /// </summary>
    public class PreviewStream : IDisposable
    {
        /// <summary>
        /// Creas a memory image stream that holds the image.
        /// Assumes stream was not initialized. This methods Seeks the stream.
        /// </summary>
        /// <param name="width">Width of the image</param>
        /// <param name="height">Height of the image</param>
        /// <param name="baseStream">The stream where the image will be stored</param>
        /// <param name="startOffset">Start offset of the stream, where the image is placed from the beginning of the stream.</param>
        public PreviewStream(int width, int height, Stream baseStream = null, long startOffset = 0,
             int numberOfPixelsInTopLevel = DefaultNumberOfPixelsInTopLevel, int zoomRatio = 3)
        {
            StartOffset = startOffset;
            Width = width;
            Height = height;
            ZoomRatio = zoomRatio;

            // getting the total number of zoom levels.
            ZoomLevels = 1;
            while (NumberOfPixels > numberOfPixelsInTopLevel * Math.Pow(zoomRatio, ZoomLevels))
            {
                ZoomLevels += 1;
            }
            ZoomStreams = null;

            // initializing stream.
            Stream to = baseStream == null ? new MemoryStream() : baseStream;
            BaseStream = to;

            // writing the header.
            Seek(StartOffset);
            WriteHeader();
            DataStartLocation = Position; // set the start position.

            // creating the streams (reverse order since heigher zoom is larger);
            ZoomStreams = new ImageStream[ZoomLevels];
            for (int i = ZoomLevels - 1; i >= 0; i--)
            {
                double zr = GetZoomRatio(0, i);
                int zw = (int)(width * zr);
                int zh = (int)(height * zr);
                ZoomStreams[i] = new ImageStream(zw, zh, to, Position);
                ZoomStreams[i].ValidatePrealocated();
                ZoomStreams[i].SeekToDataEnd();
            }

            // rewriting the header.
            Seek(StartOffset);
            WriteHeader();
        }

        /// <summary>
        /// Reads a raw image from the stream. (This function seeks the stream).
        /// </summary>
        /// <param name="from">The stream to read from</param>
        /// <param name="startOffset">Start offset of the stream, where the image  is placed from the beginning of the stream.</param>
        public PreviewStream(Stream from, long startOffset = 0)
        {
            BaseStream = from;
            StartOffset = startOffset;
            Seek(StartOffset);
            ReadHeader();
        }

        #region static methods

        /// <summary>
        /// Creates a stream reader to the file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static PreviewStream Create(string filename, int width, int height)
        {
            if (!File.Exists(filename))
                throw new Exception("File dose not exist.");
            FileStream strm = File.Create(filename);
            PreviewStream pstrm = new PreviewStream(width, height, strm);
            return pstrm;
        }

        /// <summary>
        /// Creates a stream reader to the file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static PreviewStream Open(string filename)
        {
            if (!File.Exists(filename))
                throw new Exception("File dose not exist.");
            FileStream strm = File.Open(filename, FileMode.Open, FileAccess.Read);
            PreviewStream pstrm = new PreviewStream(strm);
            return pstrm;
        }


        #endregion

        #region const and static

        /// <summary>
        /// The default number of pixels in too
        /// </summary>
        public const int DefaultNumberOfPixelsInTopLevel = 1000*1000;

        /// <summary>
        /// The default num
        /// </summary>
        public const int DefaultZoomToPixelRatio = 3;

        #endregion

        #region initialization

        private void WriteHeader()
        {
            // writing basics of the header.
            BinaryWriter wr = new BinaryWriter(BaseStream);
            wr.Write(Width);
            wr.Write(Height);
            wr.Write(DataStartLocation);
            wr.Write(ZoomLevels);
            wr.Write(ZoomRatio);
            wr.Write(IsPreviewValid);
            for (int i = 0; i < ZoomLevels; i++)
            {
                if (ZoomStreams == null)
                    wr.Write((long)0);
                else wr.Write(ZoomStreams[i].StartOffset);
            }
        }

        /// <summary>
        /// Reads the header and intializes the zoom streams.
        /// </summary>
        private void ReadHeader()
        {
            // read the header.
            Seek(0);
            BinaryReader reader = new BinaryReader(BaseStream);
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            DataStartLocation = reader.ReadInt64();
            ZoomLevels = reader.ReadInt32();
            ZoomRatio = reader.ReadInt32();
            IsPreviewValid = reader.ReadBoolean();

            ZoomStreams = new ImageStream[ZoomLevels];
            long[] startPositions = new long[ZoomLevels];
            for (int i = 0; i < ZoomLevels; i++)
            {
                startPositions[i] = reader.ReadInt64();
            }
            //reading the zooms treams.
            for (int i = 0; i < ZoomLevels; i++)
            {
                ZoomStreams[i] = new ImageStream(BaseStream, startPositions[i]);
            }
        }

        #endregion

        #region members

        /// <summary>
        /// If true the preview is valid.
        /// </summary>
        public bool IsPreviewValid { get; private set; }

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

        /// <summary>
        /// The number of zoom levls avilable. 
        /// </summary>
        public int ZoomLevels { get; private set; }

        /// <summary>
        /// The zoom stream (internal) for getting the diffrent zoom level streams.
        /// Makes the zoom streams avilable.
        /// </summary>
        private ImageStream[] ZoomStreams = null;

        /// <summary>
        /// The zoom ratio for which the stream was created.
        /// </summary>
        public int ZoomRatio { get; private set; }

        #endregion

        #region basic stream methods

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
        public void SeekToImagePosition(int x, int y, int zoomIndex)
        {
            double zr = GetZoomRatio(0, zoomIndex);
            this.ZoomStreams[zoomIndex].SeekToImagePosition((int)(x * zr), (int)(y * zr));
        }
        
        /// <summary>
        /// Flush the underliiung stream.
        /// </summary>
        public void Flush()
        {
            BaseStream.Flush();
        }

        #endregion

        #region data manipulations

        /// <summary>
        /// Makes a data buffer for the specific zoom level that can 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fromZoom"></param>
        /// <param name="toZoom"></param>
        /// <returns></returns>
        private unsafe void MakeZoomData(float[] buffer, int fromZoom, int toZoom,
            int zwidth,int zheight,
            ref float[] zoommedBuffer)
        {
            if (fromZoom == toZoom)
            {
                zoommedBuffer = new float[buffer.Length];
                Buffer.BlockCopy(buffer, 0, zoommedBuffer, 0, buffer.Length * sizeof(float));
                return;
            }
;
            double zoomRatio = GetZoomRatio(fromZoom, toZoom);
            int targetZoomPixValues = (int)(zwidth * zheight * ImageStream.NumberOfBytesPerPixel);

            // remake if needed.
            if (zoommedBuffer == null || zoommedBuffer.Length != targetZoomPixValues)
                zoommedBuffer = new float[targetZoomPixValues];

            if (fromZoom > toZoom)
            {
                // need to do reverse packaging.
                int l = zoommedBuffer.Length;
                fixed (float* pbuffer = buffer, pzbuffer = zoommedBuffer)
                {
                    for (int i = 0; i < l; i++)
                    {
                        int bidx = (int)(zoomRatio * i);
                        pzbuffer[i] = pbuffer[bidx];
                    }
                }
            }
            else // need to avarage values to get conversion.
            {
                // this actually works for both cases but is slower!
                int l = zoommedBuffer.Length;
                int maxl = buffer.Length;
                fixed (float* pbuffer = buffer, pzbuffer = zoommedBuffer)
                {
                    for (int i = 0; i < l; i++)
                    {
                        int bsidx = (int)(zoomRatio * i);
                        int beidx = (int)(zoomRatio * (i + 1));
                        if (beidx > bsidx)
                        {
                            float av = 0;
                            int avcount = 0;
                            for (int j = bsidx; j < beidx; j++)
                            {
                                if (j > maxl)
                                    break;
                                avcount += 1;
                                av += pbuffer[j];
                            }

                            pzbuffer[i] = av / avcount;
                        }
                        else pzbuffer[i] = pbuffer[bsidx];
                    }
                }
            }
        }

        #endregion

        #region data pixel reading

        /// <summary>
        /// Reads the image data in the selected region.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="buffer">The buffer to read into</param>
        /// <param name="readBuffer">Used for reading, if null then the read buffer is created</param>
        public void ReadImageData(int zoomIndex, int x, int y, int width, int height, ref float[] buffer,
            ref byte[] readBuffer)
        {
            double zr = GetZoomRatio(0, zoomIndex);
            int zx=(int)(x * zr);
            int zy=(int)(y * zr);
            int zw=(int)(width * zr);
            int zh=(int)(height * zr);
            ZoomStreams[zoomIndex].ReadImageData(zx, zy, zw, zh, ref buffer, ref readBuffer);
        }


        /// <summary>
        /// Returns the image data in the selected region.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public float[] ReadImageData(int zoomIndex, int x, int y, int width, int height)
        {
            float[] buffer = null;
            byte[] readBuffer = null;
            ReadImageData(zoomIndex, x, y, width, height, ref buffer, ref readBuffer);
            return buffer;
        }

        #endregion

        #region data pixel writing

        protected void InvalidatePreview()
        {
            if (!IsPreviewValid)
                return;
            IsPreviewValid = false;
            Seek(StartOffset);
            WriteHeader();
        }

        protected void ValidatePreview()
        {
            if (IsPreviewValid)
                return;
            IsPreviewValid = true;
            Seek(StartOffset);
            WriteHeader();
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
            InvalidatePreview();
            ZoomStreams[0].SetImageData(buffer, x, y, length, ref byteBuffer, bufferOffset);
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
            InvalidatePreview();
            ZoomStreams[0].SetImagePixels(buffer, x, y, width, height);
        }

        /// <summary>
        /// Gets the zoom taio transfer information.
        /// </summary>
        /// <param name="fromZoomLevel"></param>
        /// <param name="toZoomLevel"></param>
        /// <returns></returns>
        private double GetZoomRatio(int fromZoomLevel, int toZoomLevel)
        {
            int zoomSize = fromZoomLevel - toZoomLevel;
            double lengthRatio = Math.Pow(ZoomRatio, zoomSize);
            return lengthRatio;
        }

        #endregion

        #region zoom index validation

        public bool IsZoomLevelValid(int zoom)
        {
            if (zoom > 0 && !IsPreviewValid)
                return false;
            return true;
        }

        #endregion

        #region painting on graphics surfaces

        /// <summary>
        /// Retuns the zoom level accoding to the size in zero zoom.
        /// </summary>
        /// <param name="sizeInZoomZero"></param>
        /// <returns></returns>
        private int GetZoomIndex(int sizeInImage, int sizeInZoomZero)
        {
            double ratio = sizeInZoomZero * 1.0 / sizeInImage;
            int idx = 0;
            double cur = ratio / ZoomRatio;
            while (cur >= ZoomRatio)
            {
                idx += 1;
                cur = cur / ZoomRatio;
            }
            return idx;
        }

        /// <summary>
        /// Retruns the bits of the preview.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="gx"></param>
        /// <param name="gy"></param>
        /// <param name="gwidth"></param>
        /// <param name="gheight"></param>
        /// <returns></returns>
        public byte[] GetDrawBits(int x, int y, int width, int height)
        {
            Bitmap map = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(map);
            Draw(g, x, y, width, height, x, y, width, height);
            ImageConverter converter = new ImageConverter();
            byte[] data = (byte[])converter.ConvertTo(map, typeof(byte[]));
            return data;
        }

        /// <summary>
        /// Get the zoom from the image data.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="gx"></param>
        /// <param name="gy"></param>
        /// <param name="gwidth"></param>
        /// <param name="gheight"></param>
        public void Draw(Graphics g, int x, int y, int width, int height,
            int gx, int gy, int gwidth, int gheight, Action<float[]> processImageData=null)
        {
            CodeTimer timer = new CodeTimer();
            timer.Start();
            // calculating the zoom level from the width and height.
            bool useWidth = width > height;
            int zoomLevel = GetZoomIndex(useWidth ? gwidth : gheight, useWidth ? width : height);

            // the data as binary.
            byte[] asBinary = null;

            // getting the index for the zoom level.
            double zr = GetZoomRatio(0, zoomLevel);
            int zw = (int)(width * zr);
            int zh = (int)(height * zr);

            timer.Mark("Prepare");

            // loading the bits.
            float[] imageData = ReadImageData(zoomLevel, x, y, width, height);
            timer.Mark("Read data");

            if (processImageData != null)
                processImageData(imageData);
            timer.Mark("Process data");

            // marke image data.
            ToImageData(imageData, ref asBinary);
            timer.Mark("Make image data");

            Bitmap img = new Bitmap(zw, zh, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData dat = img.LockBits(
                new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.WriteOnly, img.PixelFormat);
            Marshal.Copy(asBinary, 0, dat.Scan0, asBinary.Length);
            img.UnlockBits(dat);
            timer.Mark("Make image");

            g.DrawImage(img, gx, gy, gwidth, gheight);
            timer.Mark("Draw on GC");
        }

        /// <summary>
        /// Makes image data from float data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="imageDataBuffer"></param>
        unsafe void ToImageData(float[] data, ref byte[] imageDataBuffer, bool doNormalization=true)
        {
            if (imageDataBuffer == null || imageDataBuffer.Length != data.Length)
                imageDataBuffer = new byte[data.Length];
            int l = data.Length;
            float max = 0, min = float.MaxValue;
            float normal = 1;
            fixed (float* pdata = data)
            fixed (byte* pbuffer = imageDataBuffer)
            {
                // calculating the image amplitude correction.
                if (doNormalization)
                {
                    for (int i = 0; i < l; i++)
                    {
                        if (pdata[i] > max)
                            max = pdata[i];
                        if (pdata[i] < min)
                            min = pdata[i];
                    }

                    normal = byte.MaxValue * 1.0F / (max - min);
                }

                for (int i = 0; i < l; i++) // attempting to reverse image to fit windows defs.
                {
                    float val = normal * pdata[i];
                    int pidx = l - i - 1;
                    if (val > 255)
                        pbuffer[pidx] = 255;
                    else if (val < 0)
                        pbuffer[pidx] = 0;
                    else pbuffer[pidx] = (byte)val;
                }
            }
        }

        #endregion

        #region preview validation

        /// <summary>
        /// Updates the preview to the current image processing result.
        /// </summary>
        public void ValidatePreview( Action<int,int> pixelsDone= null, double maxMemoryReadSizeInMBytes = 500)
        {
            // need to load all image data and convert said data to the correct preview zoom level.
            // starting with zoom level 1, we jump to last zoom level N lines by N lines.
            int maxMemoryInBytes = (int)(maxMemoryReadSizeInMBytes * 1.0 * 1e6);
            int totalNumberOfPixelsNeeded = GetNumberOfPixelsToPreviewValidate();

            int curLevel = 1;
            while (curLevel < ZoomLevels)
            {
                // doing matrix reduction. 
                // reading lines. 
                int numberOfBytesPerPixelLine =
                    ZoomStreams[curLevel - 1].Width * (int)ImageStream.NumberOfBytesPerPixel;
                int maxLinesToRead = maxMemoryInBytes / numberOfBytesPerPixelLine;
                float[] buffer = null;
                byte[] readbuffer = null;
                float[] reduced = null;
                // reading lines.
                for (int y = 0; y < ZoomStreams[curLevel - 1].Height; y += maxLinesToRead)
                {
                    int h = y + maxLinesToRead > ZoomStreams[curLevel - 1].Height ?
                        ZoomStreams[curLevel - 1].Height - y :
                        maxLinesToRead;
                    ZoomStreams[curLevel - 1].ReadImageData(0, y, ZoomStreams[curLevel - 1].Width, h,
                        ref buffer, ref readbuffer);
                    // Making the reduced matrix.
                    // we are going to take into account only the number of lines we read.
                    ReduceImageMatrixByAvarage(ref buffer, ref reduced,ZoomStreams[curLevel - 1].Width,h,
                        (int)ImageStream.NumberOfColorValuesInPixel);
                    //ReduceMatrixByZoomValue(buffer, ref reduced,
                    //    ZoomStreams[curLevel - 1].Width, h,
                    //    ZoomStreams[curLevel].Width, ZoomStreams[curLevel].Height, 
                    //    (int)ImageStream.NumberOfColorValuesInPixel);

                    // writing to stream.
                    int zh = h / ZoomRatio;
                    int zw = ZoomStreams[curLevel - 1].Width / ZoomRatio;
                    int zx = 0;
                    int zy = y / ZoomRatio;
                    if (zy > ZoomStreams[curLevel].Height)
                        zy = ZoomStreams[curLevel].Height;

                    ZoomStreams[curLevel].SetImagePixels(reduced, zx, zy, zw, zh);

                    if (pixelsDone != null)
                        pixelsDone(totalNumberOfPixelsNeeded, zw * zh);
                }

                curLevel += 1;
            }

            ValidatePreview();
        }

        public int GetNumberOfPixelsToPreviewValidate()
        {
            int totalNumberOfLinesNeeded = 0;
            for (int i = 1; i < ZoomLevels; i++)
            {
                totalNumberOfLinesNeeded += ZoomStreams[i].Height * ZoomStreams[i].Width;
            }
            return totalNumberOfLinesNeeded;
        }

        //private unsafe void ReduceMatrixByZoomValue(float[] source, ref float[] target,
        //     int sWidth, int sHeight, int tWidth, int tHeight, int nPixVals)
        //{
        //    int totalNumberOfValuesInReduced = source.Length / (ZoomRatio * ZoomRatio);
        //    if (target == null || target.Length != totalNumberOfValuesInReduced)
        //        target = new float[source.Length / (ZoomRatio * ZoomRatio)];

        //    fixed (float* pSource = source, pTarget = target)
        //    {
        //        for (int y = 0; y < tHeight; y++)
        //        {
        //            for (int x = 0; x < tWidth; x++)
        //            {
        //                int sx = x * ZoomRatio;
        //                int sy = y * ZoomRatio;
        //                float av = 0;
        //                int avcount = 0;
        //                int colorOffset = 0;
        //                for (int zx = 0; zx < ZoomRatio; zx++)
        //                {
        //                    for (int zy = 0; zy < ZoomRatio; zy++)
        //                    {
        //                        // internal zoom locations
        //                        if (zx + sx > sWidth)
        //                            continue;
        //                        if (zy + sy > sHeight)
        //                            continue;
        //                        avcount += 1;
        //                        int sourceIndex = (zy + sy) * sWidth * nPixVals + (zx + sx) * nPixVals;
        //                        av = av + pSource[sourceIndex + colorOffset];
        //                    }
        //                }
        //                av /= avcount;
        //                float red = av;

        //                colorOffset += 1;
        //                avcount = 0;
        //                av = 0;
        //                for (int zx = 0; zx < ZoomRatio; zx++)
        //                {
        //                    for (int zy = 0; zy < ZoomRatio; zy++)
        //                    {
        //                        // internal zoom locations
        //                        if (zx + sx > sWidth)
        //                            continue;
        //                        if (zy + sy > sHeight)
        //                            continue;
        //                        avcount += 1;
        //                        int sourceIndex = (zy + sy) * sWidth * nPixVals + (zx + sx) * nPixVals;
        //                        av = av + pSource[sourceIndex + colorOffset];
        //                    }
        //                }
        //                av /= avcount;
        //                float green = av;

        //                colorOffset += 1;
        //                avcount = 0;
        //                av = 0;
        //                for (int zx = 0; zx < ZoomRatio; zx++)
        //                {
        //                    for (int zy = 0; zy < ZoomRatio; zy++)
        //                    {
        //                        // internal zoom locations
        //                        if (zx + sx > sWidth)
        //                            continue;
        //                        if (zy + sy > sHeight)
        //                            continue;
        //                        avcount += 1;
        //                        int sourceIndex = (zy + sy) * sWidth * nPixVals + (zx + sx) * nPixVals;
        //                        av = av + pSource[sourceIndex + colorOffset];
        //                    }
        //                }
        //                av /= avcount;
        //                float blue = av;
        //                int reducedOffset = y * tWidth * nPixVals + x * nPixVals;
        //                pTarget[reducedOffset] = red;
        //                pTarget[reducedOffset + 1] = green;
        //                pTarget[reducedOffset + 2] = blue;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Reduces the image matrix by avaraginng on the x,y axis. This procedure is done for each pixel seperatly.
        /// </summary>
        private unsafe void ReduceImageMatrixByAvarage(
            ref float[] source, ref float[] reduced,
            int width, int height,
            int nPixVals)
        {
            int totalNumberOfValuesInReduced = source.Length / (ZoomRatio * ZoomRatio);
            if (reduced == null || reduced.Length != totalNumberOfValuesInReduced)
                reduced = new float[totalNumberOfValuesInReduced];

            // We not go thrugh the lines of the source and apply these to the target. 
            // note the offsets.
            
            // local paramters to make unsafe code run faster.
            int zoomRatio = ZoomRatio;

            fixed (float* pSource = source, pTarget = reduced)
            {
                for (int y = 0; y < height; y += zoomRatio)
                {
                    for (int x = 0; x < width; x += zoomRatio)
                    {
                        // calculating the reduced positions.
                        int reducedX = x / zoomRatio;
                        int reducedY = y / zoomRatio;

                        // avaeraging on the correct pixels.
                        for (int pvindex = 0; pvindex < nPixVals; pvindex++)// for each pixel.
                        {
                            // since the zoom ratio set the number of reduced pixels
                            // I can just scan by jumping over the correct pixels.
                            float av = 0;
                            for (int pixn = 0; pixn < zoomRatio; pixn++)
                            {
                                int sourceIndex = (x + pixn) * nPixVals + pvindex;
                                av += pSource[sourceIndex];
                            }

                            // calculate the avarage and asssign it.
                            av /= zoomRatio;
                            pTarget[reducedX + pvindex] = av;
                        }
                    }
                }
            }

            //fixed (float* pSource = source, pTarget = target)
            //{
            //    for (int y = 0; y < reducedHeight; y++)
            //    {
            //        for (int x = 0; x < reducedWidth; x++)
            //        {
            //            int sx = x * ZoomRatio;
            //            int sy = y * ZoomRatio;
            //            float av = 0;
            //            int avcount = 0;
            //            int colorOffset = 0;
            //            for (int zx = 0; zx < ZoomRatio; zx++)
            //            {
            //                for (int zy = 0; zy < ZoomRatio; zy++)
            //                {
            //                    // internal zoom locations
            //                    if (zx + sx > sWidth)
            //                        continue;
            //                    if (zy + sy > sHeight)
            //                        continue;
            //                    avcount += 1;
            //                    int sourceIndex = (zy + sy) * sWidth * nPixVals + (zx + sx) * nPixVals;
            //                    av = av + pSource[sourceIndex + colorOffset];
            //                }
            //            }
            //            av /= avcount;
            //            float red = av;

            //            colorOffset += 1;
            //            avcount = 0;
            //            av = 0;
            //            for (int zx = 0; zx < ZoomRatio; zx++)
            //            {
            //                for (int zy = 0; zy < ZoomRatio; zy++)
            //                {
            //                    // internal zoom locations
            //                    if (zx + sx > sWidth)
            //                        continue;
            //                    if (zy + sy > sHeight)
            //                        continue;
            //                    avcount += 1;
            //                    int sourceIndex = (zy + sy) * sWidth * nPixVals + (zx + sx) * nPixVals;
            //                    av = av + pSource[sourceIndex + colorOffset];
            //                }
            //            }
            //            av /= avcount;
            //            float green = av;

            //            colorOffset += 1;
            //            avcount = 0;
            //            av = 0;
            //            for (int zx = 0; zx < ZoomRatio; zx++)
            //            {
            //                for (int zy = 0; zy < ZoomRatio; zy++)
            //                {
            //                    // internal zoom locations
            //                    if (zx + sx > sWidth)
            //                        continue;
            //                    if (zy + sy > sHeight)
            //                        continue;
            //                    avcount += 1;
            //                    int sourceIndex = (zy + sy) * sWidth * nPixVals + (zx + sx) * nPixVals;
            //                    av = av + pSource[sourceIndex + colorOffset];
            //                }
            //            }
            //            av /= avcount;
            //            float blue = av;
            //            int reducedOffset = y * tWidth * nPixVals + x * nPixVals;
            //            pTarget[reducedOffset] = red;
            //            pTarget[reducedOffset + 1] = green;
            //            pTarget[reducedOffset + 2] = blue;
            //        }
            //    }
            //}
        }

        #endregion

        #region IDisposable Members

        public void Close()
        {
            if (BaseStream != null && BaseStream.CanSeek)
                BaseStream.Close();
        }

        public void Dispose()
        {
            Close();
            BaseStream = null;
        }

        #endregion
    }
}
