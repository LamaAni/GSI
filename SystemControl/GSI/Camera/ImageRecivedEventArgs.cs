using Lumenera.USB;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Camera
{
    /// <summary>
    /// Image received in camera event args.
    /// </summary>
    public class ImagereceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <param name="timeStamp">The time elapsed till the image from last timer reset.</param>
        /// <param name="TimeOfLastSync">The time of the last camera sync</param>
        internal ImagereceivedEventArgs(int width, int height, byte[] data,
            dll.LucamPixelFormat format, DateTime timeStamp, DateTime TimeOfLastSync)
        {
            Width = width;
            Height = height;
            Data = data;
            Format = format;
            TimeStamp = timeStamp;
            Elapsed = timeStamp - TimeOfLastSync;
        }

        /// <summary>
        /// The time elpased since the last camera sync.
        /// </summary>
        public TimeSpan Elapsed { get; private set; }

        /// <summary>
        /// The image timestamp.
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// The pixel format
        /// </summary>
        public dll.LucamPixelFormat Format { get; private set; }

        public int NumOfBytes
        {
            get
            {
                switch(Format)
                {
                    case dll.LucamPixelFormat.PF_16:
                        return 2;
                    case dll.LucamPixelFormat.PF_24:
                        return 3;
                    case dll.LucamPixelFormat.PF_32:
                        return 4;
                    case dll.LucamPixelFormat.PF_48:
                        return 5;
                    case dll.LucamPixelFormat.PF_COUNT:
                        return 1;
                }

                return 1;
            }
        }

        /// <summary>
        /// The width of the image.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the image.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The image data.
        /// </summary>
        public byte[] Data { get; private set; }
    }
}
