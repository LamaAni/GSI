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
    public class R2FFT : IDisposable
    {
        /// <summary>
        /// Creates the opencl fft from a 2D array. (Radix2, zeropad
        /// </summary>
        /// <param name="values">The values</param>
        /// <param name="minVectorLength">Min padding of zeros length. -1 use closest to 2^n</param>
        public R2FFT(int minVectorLength = -1)
        {
            NumberOfSamples = -1;
            // calculating the vector length.
            LogN = CalculateLogN(minVectorLength);
            ForierVectorLength = CalculateForierVectorLength(minVectorLength);
        }

        ~R2FFT()
        {
            Dispose();
        }

        #region static methods

        public static int CalculateLogN(int minVectorLength)
        {
            return (int)Math.Ceiling(Math.Log(minVectorLength, 2));
        }

        public static int CalculateForierVectorLength(int minVectorLength)
        {
            return (int)Math.Pow(2, CalculateLogN(minVectorLength));
        }

        #endregion 

        #region members

        /// <summary>
        /// If true, the avarage is removed.
        /// </summary>
        public bool DoSubstractAvarage { get; set; }

        /// <summary>
        /// If the value > 0, then the avarage of the vector [0.. SubstractAvarageFromVectorUpTo] will
        /// be removed from these values. (Set by Set data).
        /// </summary>
        protected int SubstractAvarageFromVectorUpTo { get; set; }

        /// <summary>
        /// The number of FFT's to do (The data vector [1....N,1...N] may be composed of multiple samples).
        /// </summary>
        public int NumberOfSamples { get; private set; }

        /// <summary>
        /// The vector length;
        /// </summary>
        public int ForierVectorLength { get; private set; }

        /// <summary>
        /// The log of the length, closest to 2^n. (or min padding of values).
        /// </summary>
        public int LogN { get; private set; }

        private float[] m_Real;

        /// <summary>
        /// The real values in the processing vector.
        /// </summary>
        public float[] Real
        {
            get { return m_Real; }
            private set { m_Real = value; }
        }

        private float[] m_Imag;

        /// <summary>
        /// The imaginary values in the processing vector.
        /// </summary>
        public float[] Imag
        {
            get { return m_Imag; }
            private set { m_Imag = value; }
        }

        private float[] m_Mag;

        /// <summary>
        /// The imaginary values in the processing vector.
        /// </summary>
        public float[] Mag
        {
            get { return m_Mag; }
            private set { m_Mag = value; }
        }

        private float[] m_Mask;

        /// <summary>
        /// The mask to apply to the data vector. (Must be of size VectorLength)
        /// If null no mask will be applied.
        /// </summary>
        public float[] Mask
        {
            get { return m_Mask; }
            private set { m_Mask = value; }
        }

        #endregion

        #region helper methods

        public static double CalculateMemoryRequieredForVectorInBytes(int forierVectorLength, bool hasMask)
        {
            return sizeof(float) * 3.0 * forierVectorLength + (hasMask ? forierVectorLength * sizeof(float) : 0);
        }

        #endregion

        #region Data methods

        /// <summary>
        /// Set the applied mask.
        /// </summary>
        /// <param name="mask"></param>
        public void SetMask(float[] mask)
        {
            if (mask == null)
            {
                Mask = null;
                return;
            }

            Mask = new float[ForierVectorLength];
            int maskLength = mask.Length > ForierVectorLength ? ForierVectorLength : mask.Length;
            for (int i = 0; i < maskLength; i++)
            {
                Mask[i] = mask[i];
            }

        }

        /// <summary>
        /// Sets the data vectors to the data. 
        /// Number of samples will change accordingly.
        /// </summary>
        /// <param name="dataVectorLength">N=The length of a single forier data length. Data vector is [1..N 1..N]</param>
        /// <param name="real">The real data.  Data vector is [1..N 1..N]</param>
        /// <param name="imag">The imaginary data.  Data vector is [1..N 1..N]</param>
        public void SetData(int dataVectorLength, float[] real, float[] imag = null)
        {
            // setting the basics vector data.
            SubstractAvarageFromVectorUpTo = DoSubstractAvarage ? dataVectorLength : 0;
            NumberOfSamples = (int)Math.Floor(real.Length * 1.0 / dataVectorLength);

            if (Real == null || real.Length != NumberOfSamples * ForierVectorLength)
            {
                Real = new float[ForierVectorLength * NumberOfSamples];
                Imag = new float[ForierVectorLength * NumberOfSamples];
                Mag = new float[ForierVectorLength * NumberOfSamples];
            }

            // reading data into the vectors.
            for (int i = 0; i < NumberOfSamples; i++)
            {
                int destIndex = ForierVectorLength * i;
                int sourceIndex = dataVectorLength * i;
                Array.Copy(real, sourceIndex, Real, destIndex, dataVectorLength);
                Array.Copy(imag, sourceIndex, Imag, destIndex, dataVectorLength);
            }
        }

        #endregion

        #region Do FFT

        static string m_r2fft_source = null;

        GSI.OpenCL.GpuTask m_runTask = null;
        GSI.OpenCL.ExecutingKernal m_runKernal = null;

        /// <summary>
        /// The code for the kernel.
        /// </summary>
        public static string KernelCode
        {
            get
            {
                if (m_r2fft_source == null)
                {
                    m_r2fft_source = System.Reflection.Assembly.GetExecutingAssembly().GetStringResource("GSI.OpenCL.FFT.r2fft.c");
                }
                return m_r2fft_source;
            }
        }

        /// <summary>
        /// Run the fft.
        /// </summary>
        public void Run(bool isForward)
        {
            if (m_runTask == null)
            {
                m_runTask = GpuTask.Create(KernelCode);
            }

            m_runTask.RunKernal("R2FFT", (k) =>
                {
                    k.SetParamter<int>(LogN, true);
                    k.SetParamter<int>(isForward ? 1 : -1, true);
                    k.SetBufferParameter<float>(ref m_Real, false);
                    k.SetBufferParameter<float>(ref m_Imag, false);
                    k.SetBufferParameter<float>(ref m_Mag, false);
                    k.SetParamter<int>(Mask != null ? 1 : 0, true);
                    k.SetParamter<int>(SubstractAvarageFromVectorUpTo, true);
                    k.SetBufferParameter<float>(ref m_Mask, true);
                },
                (k) =>
                {
                    Real = k.GetBufferValue<float>(2);
                    Imag = k.GetBufferValue<float>(3);
                    Mag = k.GetBufferValue<float>(4);
                }, NumberOfSamples);
        }

        /// <summary>
        /// Populate the resulting data into the data vector.
        /// </summary>
        /// <param name="dataVectorLength">The data vector length</param>
        /// <param name="real"></param>
        /// <param name="imag"></param>
        public  void Populate(int dataVectorLength, int dataVectorStartOffset, ref float[] real, ref float[] imag, ref float[] mag)
        {
            PopulateSinge(dataVectorLength, dataVectorStartOffset, Real, ref real);
            PopulateSinge(dataVectorLength, dataVectorStartOffset, Imag, ref imag);
            PopulateSinge(dataVectorLength, dataVectorStartOffset, Mag, ref mag);

            //// populating the fft vectors.
            //for (int i = 0; i < NumberOfSamples; i++)
            //{
            //    int destIndex = dataVectorLength * i;
            //    int sourceIndex =  ForierVectorLength* i;
            //    Array.Copy(Real, sourceIndex, real, destIndex, dataVectorLength);
            //    Array.Copy(Imag, sourceIndex, imag, destIndex, dataVectorLength);
            //    Array.Copy(Mag, sourceIndex, mag, destIndex, dataVectorLength);
            //}

            // copy the avrages. Removed due to memory errror.
            //System.Buffer.BlockCopy(Avarages, 0, avarages, 0, avarages.Length);
        }

        unsafe void PopulateSinge(int dataVectorLength, int dataVectorStartOffset, float[] source, ref float[] dest)
        {
            fixed(float* _source=source)
            fixed (float* _dest = dest)
            {
                // populating the fft vectors.
                for (int i = 0; i < NumberOfSamples; i++)
                {
                    int sourceIndex = ForierVectorLength * i;
                    int destIndex=dataVectorLength*i;
                    for (int j = 0; j < dataVectorLength; j++)
                    {
                        _dest[destIndex + j] = _source[sourceIndex + dataVectorStartOffset + j];
                    }
                }
            }
        }

        #endregion

        #region static methods

        //public static void Forward(ref double[,] real, ref double[,] imag, int minZeroPad = -1)
        //{
        //    FFT(false, ref real, ref imag, minZeroPad);
        //}

        //public static void Reverse(ref double[,] real, ref double[,] imag, int minZeroPad = -1)
        //{
        //    FFT(false, ref real, ref imag, minZeroPad);
        //}

        //public static void FFT(bool isForward, ref double[,] real, ref double[,] imag, int minZeroPad = -1)
        //{
        //    R2FFT fft = new R2FFT(minZeroPad);
        //    fft.SetData(real, imag);
        //    fft.Run(isForward);
        //    fft.Populate(ref real, ref imag);
        //}

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            m_runKernal = null;
            m_runTask = null;
        }

        #endregion
    }
}
