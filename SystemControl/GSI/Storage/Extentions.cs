using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSI.Storage
{
    public static class Extentions 
    {
        /// <summary>
        /// Extends the stream to a specific length.
        /// </summary>
        /// <param name="strm">The stream to extend.</param>
        /// <param name="length">The length to extend the stream by.</param>
        public static void ExtendStream(this Stream strm, long length, bool fillWithZeros = false)
        {
            long curpos = strm.Position;
            long lastPos = strm.Length;
            strm.SetLength(strm.Length + length);
            strm.Flush();
            if (!fillWithZeros)
            {
                strm.Seek(1, SeekOrigin.End);
                strm.WriteByte(0);
                strm.Flush();
            }
            else
            {
                strm.Seek(lastPos, SeekOrigin.Begin);
                // filling with zeros.
                byte[] fillBuffer = new byte[(int)(1e6 * 300)];
                while (strm.Position != strm.Length)
                {
                    int l = strm.Length - strm.Position < fillBuffer.Length ?
                        (int)(strm.Length - strm.Position) : fillBuffer.Length;
                    strm.Write(fillBuffer, 0, l);
                }
                strm.Flush();
            }
            strm.Seek(curpos, SeekOrigin.Begin);
        }

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            int dwIoControlCode,
            IntPtr InBuffer,
            int nInBufferSize,
            IntPtr OutBuffer,
            int nOutBufferSize,
            ref int pBytesReturned,
            [In] ref NativeOverlapped lpOverlapped
        );

        static void MarkAsSparseFile(SafeFileHandle fileHandle)
        {
            int bytesReturned = 0;
            NativeOverlapped lpOverlapped = new NativeOverlapped();
            bool result =
                DeviceIoControl(
                    fileHandle,
                    590020, //FSCTL_SET_SPARSE,
                    IntPtr.Zero,
                    0,
                    IntPtr.Zero,
                    0,
                    ref bytesReturned,
                    ref lpOverlapped);
            if (result == false)
                throw new Win32Exception();
        }

        /// <summary>
        /// Mark this file as a sparese file.
        /// CANNOT BE UNDUN. WINDOWS ONLY.
        /// </summary>
        /// <param name="stream"></param>
        public static void MarkeFileAsSparse(this FileStream stream)
        {
            MarkAsSparseFile(stream.SafeFileHandle);
        }
    }
}
