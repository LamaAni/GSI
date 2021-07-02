using Lumenera.USB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Camera.LumeneraControl
{
    public class Lt255 : LumeneraCamera
    {
        public Lt255(CameraConnectionInfo info)
            : base(info)
        {
            _GrayscalePallete=new Color[256];
            for (int i = 0; i < 256; i++)
                _GrayscalePallete[i] = Color.FromArgb(i, i, i);
            DefaultFrameRate = 100;
        }

        Color[] _GrayscalePallete;

        /// <summary>
        /// Converts the byte data into a color list.
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public override Image ToImage(byte[] imageData, int width, int height)
        {
            System.Drawing.Imaging.PixelFormat format =
                Settings.PixelFormat == dll.LucamPixelFormat.PF_16 ? PixelFormat.Format16bppGrayScale
                : PixelFormat.Format8bppIndexed;

            if (Settings.PixelFormat == dll.LucamPixelFormat.PF_16)
            {
                byte[] newdata = new byte[imageData.Length / 2];
                for (int i = 0; i < imageData.Length; i += 2)
                {
                    UInt16 val = BitConverter.ToUInt16(imageData, i);
                    newdata[i/2] = Convert.ToByte(val / 256);
                }
                imageData = newdata;
            }

            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            ColorPalette _palette = pic.Palette;
            Color[] _entries = _palette.Entries;
            for (int i = 0; i < 256; i++)
            {
                Color b = new Color();
                b = Color.FromArgb((byte)i, (byte)i, (byte)i);
                _entries[i] = b;
            }

            pic.Palette = _palette;

            //Get a reference to the images pixel data
            Rectangle dimension = new Rectangle(0, 0, width, height);
            BitmapData picData = pic.LockBits(dimension, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr pixelStartAddress = picData.Scan0;

            //Copy the pixel data into the bitmap structure
            System.Runtime.InteropServices.Marshal.Copy(imageData, 0, pixelStartAddress, imageData.Length);

            pic.UnlockBits(picData);

            for (int i = 0; i < 256; i++)
                pic.Palette.Entries[i] = Color.FromArgb(i, i, i);

            return pic;
        }

        public override dll.LucamPixelFormat[] GetAvilablePixelFormats()
        {
            return new dll.LucamPixelFormat[] { dll.LucamPixelFormat.PF_8, dll.LucamPixelFormat.PF_16 };
        }

        protected override byte[] ValidateDataSize(byte[] data)
        {
            int numberOfPixels = Settings.Width * Settings.Height *
                (Settings.PixelFormat == dll.LucamPixelFormat.PF_16 ? 2 : 1);
            if (data.Length > numberOfPixels)
            {
                return data.Take(numberOfPixels).ToArray();
            }
            return data;
        }
    }
}
