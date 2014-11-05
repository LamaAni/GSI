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
    public class RawImageConverter
    {
        public RawImageConverter(ImageStream strm, int tileWidth = 4000, int tileHeight = 4000)
        {
            if (tileWidth * tileHeight > MaxTileSizeInPixels)
                throw new Exception("Tile size too large. See MaxTileSizeInPixels");
            TileWidth = tileHeight;
            TileHeight = tileHeight;
            UseImageTiling = true;
            Source = strm;
            ValueToPixel = null;
        }

        public const int MaxTileSizeInPixels = 4000*4000;

        #region srtatic members

        /// <summary>
        /// Converts a byte to value.
        /// </summary>
        /// <returns></returns>
        public static byte ConvertByteToValue(float val)
        {
            if (val > 255)
                return 255;
            if (val < 0)
                return 0;
            return (byte)val;
        }

        /// <summary>
        /// Unsafe copy buffer. Use with care.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="length"></param>
        public static unsafe void ConvertToByteValues(float[] source, byte[] target, int length, Func<float, byte> pixToValue = null)
        {
            if (source.Length != target.Length)
                throw new Exception("Incompatible array lengths.");

            if (source.Length < length)
                throw new Exception("Copy lengths is out of array bounds.");

            bool usePixToValue = pixToValue == null ? false : true;
            fixed (byte* pTarget = target)
            fixed (float* pSource = source)
            {
                if (usePixToValue)
                {
                    for (int i = 0; i < length; i++)
                        pTarget[i] = pixToValue(pSource[i]);
                }
                else
                {
                    for (int i = 0; i < length; i++)
                        pTarget[i] = (byte)pSource[i];
                }
            }
        }

        #endregion

        #region members

        /// <summary>
        /// Image value to image pixel.
        /// </summary>
        public Func<float,byte> ValueToPixel { get; set; }

        /// <summary>
        /// Single tile height.
        /// </summary>
        public int TileWidth { get; private set; }

        /// <summary>
        /// Single tile width.
        /// </summary>
        public int TileHeight { get; private set; }

        /// <summary>
        /// THe image stream.
        /// </summary>
        public ImageStream Source { get; private set; }

        /// <summary>
        /// If true the tiles are always calcualted according to zero zero.
        /// This means that the images are always loaded per tile. Default is true.
        /// </summary>
        public bool UseImageTiling {get ;set;}

        #endregion

        #region helper classes

        public class RawImageTileInfo
        {
            public RawImageTileInfo(int x, int y, int width, int height,
                int imageIndex, int convertIndex, int totalNumberOfConvertTiles)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                ImageIndex = imageIndex;
                ConvertIndex = convertIndex;
                TotalNumberOfConvertTiles = totalNumberOfConvertTiles;
            }

            public int X { get; private set; }
            public int Y { get; private set; }
            public int Height { get; private set; }
            public int Width { get; private set; }
            public int ImageIndex { get; private set; }
            public int ConvertIndex { get; private set; }
            public int TotalNumberOfConvertTiles { get; private set; }
        }

        #endregion

        #region MEthods

        /// <summary>
        /// Returns the tiles ordered by x.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        RawImageTileInfo[] GetTiles(int x, int y, int width, int height, out Rectangle range)
        {
            // splicing image to tiles.
            Func<int, int, bool, int> ToTileUnits = (v, u, floor) =>
            {
                double val = v * 1.0 / u;
                val = floor ? Math.Floor(val) : Math.Ceiling(val);
                return (int)(val * u);
            };

            int tx = UseImageTiling ? ToTileUnits(x, TileWidth, true) : x;
            int ty = UseImageTiling ? ToTileUnits(y, TileHeight, true) : y;
            int tw = UseImageTiling ? ToTileUnits(x + width, TileWidth, false) - tx : width;
            int th = UseImageTiling ? ToTileUnits(y + height, TileHeight, false) - ty : height;

            int numberOfXTiles = (int)Math.Ceiling(tw * 1.0 / TileWidth);
            int numberOfYTiles = (int)Math.Ceiling(th * 1.0 / TileHeight);

            RawImageTileInfo[] tiles = new RawImageTileInfo[numberOfXTiles * numberOfYTiles];

            // splcing into tiles.
            int maxX=tx;
            int maxY=ty;
            for (int hi = 0; hi < th / TileHeight; hi++)
            {
                for (int wi = 0; wi < tw / TileHeight; wi++)
                {
                    int tidx = hi * numberOfXTiles + wi;
                    int cx = wi * TileWidth;
                    int cy = hi * TileHeight;
                    int cw = cx + TileWidth > Source.Width ? Source.Width - cx : TileWidth ;
                    int ch = cy + TileHeight > Source.Height ? Source.Height - cy : TileHeight;

                    if (cx + cw > maxX)
                        maxX = cx + cw;
                    if (cy + ch > maxY)
                        maxY = cy + ch;

                    int imageIndex = UseImageTiling ? (cy / TileHeight) * numberOfXTiles + cx / TileWidth : -1;
                    tiles[tidx] = new RawImageTileInfo(cx, cy, cw, ch, imageIndex,
                        tidx, numberOfXTiles * numberOfYTiles);
                }
            }

            range = new Rectangle(tx, ty, maxX, maxY);

            return tiles;
        }

        /// <summary>
        /// Reads the full data of a tile.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private unsafe byte[] ReadTileBytes(RawImageTileInfo info, ref byte[] buffer)
        {
            long totalNumberOfBytes = info.Width * info.Height * ImageStream.NumberOfBytesPerPixel;
            int numberOfLineBytes = (int)(info.Width * ImageStream.NumberOfBytesPerPixel);
            if (buffer == null || buffer.Length != totalNumberOfBytes)
            {
                buffer = null;
                GC.Collect();
                buffer = new byte[totalNumberOfBytes];
            }
            // reading the values.
            DateTime time = DateTime.Now;
            TimeSpan seekTime = new TimeSpan(), readTime = new TimeSpan();//, copyTime = new TimeSpan();
            Source.BaseStream.Flush();

            for (int l = 0; l < info.Height; l++)
            {
                time = DateTime.Now;
                Source.SeekToImagePosition(info.X, info.Y + l);
                seekTime += DateTime.Now - time;
                time = DateTime.Now;

                Source.BaseStream.Read(buffer, l * numberOfLineBytes, numberOfLineBytes);
                readTime += DateTime.Now - time;
                //time = DateTime.Now;
                //Buffer.BlockCopy(copyBuffer, 0, buffer, l * numberOfLineBytes, numberOfLineBytes);
                //copyTime += DateTime.Now - time;
            }

            return buffer;
        }


        /// <summary>
        /// Converts the ImageStream to tiles.
        /// </summary>
        /// <param name="f">The result function</param>
        /// <param name="x">In pixels of image.</param>
        /// <param name="y">In pixels of image.</param>
        /// <param name="width">In pixels of image.</param>
        /// <param name="height">In pixels of image.</param>
        public void Convert(DoTileStorage f, int x, int y, int width, int height)
        {
            Rectangle range;
            RawImageTileInfo[] tiles = GetTiles(x, y, width, height, out range);
            CodeTimer timer = new CodeTimer();
            float[] pixelBuffer = null;
            byte[] pixelVals = null;
            byte[] buffer = null;
            // reading tiles by line.
            for (int tidx = 0; tidx < tiles.Length; tidx++)
            {
                timer.Reset();
                // reading tile.
                RawImageTileInfo tile = tiles[tidx];
                Bitmap tileImage = new Bitmap(tile.Width, tile.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                timer.Mark("Prepare");

                ReadTileBytes(tile, ref buffer);
                timer.Mark("Read " + buffer.Length / 1e6);

                if (pixelBuffer == null || pixelBuffer.Length * ImageStream.NumberOfBytesPerPixelValue != buffer.Length)
                {
                    pixelBuffer = null;
                    pixelVals = null;
                    GC.Collect();
                    pixelBuffer = new float[buffer.Length / ImageStream.NumberOfBytesPerPixelValue];
                    pixelVals = new byte[pixelBuffer.Length];
                }
                Buffer.BlockCopy(buffer, 0, pixelBuffer, 0, buffer.Length);
                timer.Mark("Convert");

                //Parallel.For(0, pixelBuffer.Length, (i) =>
                //{
                //    pixelVals[i] = ValueToPixel(pixelBuffer[i]);
                //});
                ConvertToByteValues(pixelBuffer, pixelVals, pixelBuffer.Length, ValueToPixel);

                timer.Mark("Process");

                BitmapData data = tileImage.LockBits(
                       new Rectangle(0, 0, tile.Width, tile.Height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                         tileImage.PixelFormat);

                // copy the array data to the image.
                Marshal.Copy(pixelVals, 0, data.Scan0, pixelVals.Length);
                tileImage.UnlockBits(data);
                timer.Mark("Makeimage");

                //// reading lines in the tile.
                //for (int l = 0; l < tile.Height; l++)
                //{
                //    BaseStream.SeekToImagePosition(tile.X, tile.Y + l);

                //    float[] pixels = BaseStream.ReadPixels(tile.Width);
                //    byte[] pixelVals = new byte[pixels.Length];
                //    Parallel.For(0, pixels.Length, (i) =>
                //    {
                //        pixelVals[i] = ValueToPixel(pixels[i]);
                //    });

                //    BitmapData data = tileImage.LockBits(
                //       new Rectangle(0, tile.Y + l, tile.Width, 1),
                //        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                //         tileImage.PixelFormat);

                //    // copy the array data to the image.
                //    Marshal.Copy(pixelVals, 0, data.Scan0, pixelVals.Length);

                //    tileImage.UnlockBits(data);
                //}

                f(tile, tileImage);
            }
        }

        #endregion
    }

    /// <summary>
    /// Dose tile storage.
    /// </summary>
    /// <param name="tileIndex">The index of the tile in the current read.</param>
    /// <param name="tileImageIndex">The index of the tile relative to image. -1 if UseImageTiling==false</param>
    /// <param name="tile">The tile</param>
    public delegate void DoTileStorage(RawImageConverter.RawImageTileInfo info, Image tile);
}
