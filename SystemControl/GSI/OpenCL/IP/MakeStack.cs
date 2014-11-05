using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSI.Coading;

namespace GSI.OpenCL.FFT
{
    /// <summary>
    /// Creates an fft open CL command. For more then one vector of fft.
    /// </summary>
    public class MakeStack : IDisposable
    {
        /// <summary>
        /// Creates the opencl fft from a 2D array. (Radix2, zeropad
        /// </summary>
        /// <param name="dataVectorSize">The number of bytes in a data vector pixel</param>
        /// <param name="lineSize">The number of vectors in a line.</param>
        /// <param name="numberOfLines">The total number of lines.</param>
        /// <param name="vectorSize">A single vector size in pixels.</param>
        public MakeStack(int numberOfLines, int lineSize, int vectorSize, int dataVectorSize)
        {
            NumberOfSamples = -1;
            NumberOfLines = numberOfLines;
            LineSize = lineSize;
            VectorSize = vectorSize;
            DataVectorSize = dataVectorSize;
        }

        ~MakeStack()
        {
            Dispose();
        }

        #region members
        
        /// <summary>
        /// The number of bytes in a data vector.
        /// </summary>
        public int DataVectorSize { get; private set; }

        /// <summary>
        /// The total number of lines. Used to calculate indexing.
        /// </summary>
        public int NumberOfLines { get; private set; }

        /// <summary>
        /// The number of pixels in a vector.
        /// </summary>
        public int VectorSize { get; private set; }

        /// <summary>
        /// The number of vectors in a line.
        /// </summary>
        public int LineSize { get; private set; }

        /// <summary>
        /// The total number of samples currently loaded.
        /// </summary>
        public int NumberOfSamples { get; private set; }

        /// <summary>
        /// The total number of pixels loaded.
        /// </summary>
        public int TotalNumberOfPixels { get { return NumberOfSamples * VectorSize; } }

        /// <summary>
        /// The number of bytes in a single pixel. RGBA.
        /// </summary>
        public const int NumberOfBytesPerPixel = 4;

        private byte[] m_image;

        /// <summary>
        /// The image data.
        /// </summary>
        public byte[] Image
        {
            get { return m_image; }
            private set { m_image = value; }
        }

        private byte[] m_Vectors;

        /// <summary>
        /// The vectors data loaded.
        /// </summary>
        public byte[] VectorData
        {
            get { return m_Vectors; }
            private set { m_Vectors = value; }
        }

        private static float[] m_dummycolormask = new float[1];

        private float[] m_ColorMask;

        /// <summary>
        /// The colormask to apply to the data before averaging.
        /// </summary>
        public float[] ColorMask
        {
            get { return m_ColorMask; }
            private set { m_ColorMask = value; }
        }

        /// <summary>
        /// If true then using a colormask to convert the image.
        /// </summary>
        public bool UsingColorMask { get { return m_ColorMask != null; } }

        #endregion

        #region helper methods

        /// <summary>
        /// The total memory required for for a single vector in bytes.
        /// </summary>
        /// <param name="vectorSize"></param>
        /// <returns></returns>
        public static double CalculateMemoryRequieredForVectorInBytes(int vectorSize)
        {
            return sizeof(byte) * NumberOfBytesPerPixel * vectorSize; // image.
        }


        #endregion

        #region Data methods

        /// <summary>
        /// Sets the processing data.
        /// </summary>
        public void SetData(int count, byte[] vectors, byte[] img = null)
        {
            NumberOfSamples=count;
            if (vectors.Length != DataVectorSize * TotalNumberOfPixels)
                throw new Exception("Incompatibple vectors length for DataVectorSize and count. assert L=DataVectorSize * TotalNumberOfPixels");

            Image = img;
            VectorData = vectors;

            if (Image == null || Image.Length != TotalNumberOfPixels * NumberOfBytesPerPixel)
                Image = new byte[TotalNumberOfPixels * NumberOfBytesPerPixel];
        }

        #endregion

        #region Do FFT

        static string m_source = null;

        /// <summary>
        /// The code for the kernel.
        /// </summary>
        public static string KernelCode
        {
            get
            {
                if (m_source == null)
                {
                    m_source = System.Reflection.Assembly.GetExecutingAssembly().GetStringResource("GSI.OpenCL.IP.makestack.c");
                }
                return m_source;
            }
        }

        /*
	const int numberOfLines, // total number of lines in the sample.
	const int lineSize, // the number of vectors in a line,
	const int vectorSize, // the number of pixels in a vector.
	const int vectorDataPixelLength, // the length of a single pixel data.
	const int usecolormask, // if 1 then use the colormask. 
	global float* colormask, // the mask to apply to the colors. 
	// [RGBA_IMAGE_N_BYTES * vectorDataPixelLength * 3  or null] 
	// the three diffrent data sets are for RGB values.
	global unsigned char* vectors, // the data source to read from.
	global unsigned char* imagedata // the image data to write to.
         */
        /// <summary>
        /// Run the fft.
        /// </summary>
        public void Run()
        {
            GpuTask.Run(KernelCode, "makeimagedata", (k) =>
                {
                    k.SetParamter<int>(NumberOfLines, true);
                    k.SetParamter<int>(LineSize, true);
                    k.SetParamter<int>(VectorSize, true);
                    k.SetParamter<int>(DataVectorSize, true);
                    k.SetParamter<int>(UsingColorMask ? 1 : 0, true);
                    if (UsingColorMask)
                        k.SetBufferParameter<float>(ref m_ColorMask, true);
                    else k.SetBufferParameter<float>(ref m_dummycolormask, true);
                    k.SetBufferParameter<byte>(ref m_Vectors, true);
                    k.SetBufferParameter<byte>(ref m_image, false);
                },
                (k) =>
                {
                    m_image = k.GetBufferValue<byte>(7);
                }, NumberOfSamples);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
