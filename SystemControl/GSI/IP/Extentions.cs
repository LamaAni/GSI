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
            = System.Drawing.Imaging.PixelFormat.Format16bppGrayScale)
        {
            BitmapData data=img.LockBits(new Rectangle(new Point(0,0,),img.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly,
                 format);
            byte[] rslt=new byte[data.Width*data.Height*data.Stride];
            Marshal.Copy(data.Scan0,rslt,0,data.Width*data.Height*data.Stride);
            img.UnlockBits(data);
            return rslt;
        }
    }
}
