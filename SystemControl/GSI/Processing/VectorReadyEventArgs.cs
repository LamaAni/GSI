using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Processing
{
    public class VectorReadyEventArgs : EventArgs
    {
        public VectorReadyEventArgs(byte[,] vec, ImageData instigatingImage)
        {
            Vector = vec;
            InstigatingImage = instigatingImage;
        }

        /// <summary>
        /// Three dimentional vector of stacked pixels. 
        /// xindex, yindex, zindex.
        /// </summary>
        public byte[,] Vector { get; private set; }

        /// <summary>
        /// The instigating image data. (the last in the stack).
        /// </summary>
        public ImageData InstigatingImage { get; private set; }
    }
}
