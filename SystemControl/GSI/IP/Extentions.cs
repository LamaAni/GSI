using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace GSI.IP
{
    public static class Extentions
    {

        /// <summary>
        /// Returns all the bytes in the image.
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] GetImageBytes(this Bitmap img, System.Drawing.Imaging.PixelFormat format
            = System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
        {
            BitmapData data=img.LockBits(new Rectangle(0,0,img.Width,img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly,
                 format);
            byte[] rslt = new byte[data.Height * data.Stride];
            Marshal.Copy(data.Scan0, rslt, 0, data.Height * data.Stride);
            img.UnlockBits(data);
            return rslt;
        }

        /// <summary>
        /// Sets the entire image bytes starting at location 0, until the end of data.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="data"></param>
        /// <param name="format"></param>
        public static void SetImageBytes(this Bitmap img, byte[] data, System.Drawing.Imaging.PixelFormat format
            = System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
        {
            BitmapData bits = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite,
                 format);

            if (data.Length > bits.Stride * bits.Height)
            {
                img.UnlockBits(bits);
                throw new Exception("Data length is too large for image.");
            }

            Marshal.Copy(data, 0, bits.Scan0, data.Length);
            img.UnlockBits(bits);

        }
    }
}
