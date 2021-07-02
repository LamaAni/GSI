using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Processing
{
    public class ImageData
    {
        internal ImageData(byte[] data, int idx, DateTime timestamp)
        {
            Data = data;
            Index = idx;
            TimeStamp = timestamp;
        }

        public DateTime TimeStamp { get; private set; }

        public int Index { get; private set; }

        public byte[] Data { get; private set; }
    }
}
