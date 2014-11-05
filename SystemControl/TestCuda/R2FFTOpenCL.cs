using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaTester
{
    /// <summary>
    /// Creates an fft open CL command. For more then one vector of fft.
    /// </summary>
    public class R2FFTOpenCL
    {
        /// <summary>
        /// Creates the opencl fft from a 2D array. (Radix2, zeropad
        /// </summary>
        /// <param name="values">The values</param>
        /// <param name="minPadZerosLength">Min padding of zeros length. -1 use closest to 2^n</param>
        public R2FFTOpenCL(double[,] real, double[,] imag = null, int minPadZerosLength = -1)
        {
            NumberOfSamples = real.GetLength(0);
            int orgVecLength = real.GetLength(1);
            if (orgVecLength < minPadZerosLength)
                orgVecLength = minPadZerosLength;
            LogN = (int)Math.Ceiling(Math.Log(orgVecLength, 2));
            VectorLength = (int)Math.Pow(2, LogN);
            Real = new double[VectorLength * NumberOfSamples];
            Imag = new double[VectorLength * NumberOfSamples];
            for (int s = 0; s < NumberOfSamples; s++)
            {
                for (int i = 0; i < orgVecLength; i++)
                {
                    Real[s * VectorLength + i] = real[s, i];
                    if (imag != null)
                        Imag[s * VectorLength + i] = imag[s, i];
                }
            }
        }

        #region members

        /// <summary>
        /// The number of FFT's to do.
        /// </summary>
        public int NumberOfSamples { get; private set; }

        /// <summary>
        /// The vector length;
        /// </summary>
        public int VectorLength { get; private set; }

        /// <summary>
        /// The log of the length, closest to 2^n. (or min padding of values).
        /// </summary>
        public int LogN { get; private set; }

        /// <summary>
        /// The real values in the processing vector.
        /// </summary>
        public double[] Real;

        /// <summary>
        /// The imaginary values in the processing vector.
        /// </summary>
        public double[] Imag;

        #endregion

        #region Do FFT

        /// <summary>
        /// Run the fft.
        /// </summary>
        public void Run(bool isForward)
        {
            GSI.OpenCL.GpuTask.Run(
                File.ReadAllText("r2fft.c"),
                "R2FFT",
                (k) =>
                {
                    k.SetParamter<int>(LogN, true);
                    k.SetBufferParameter<double>(ref Real, false);
                    k.SetBufferParameter<double>(ref Imag, false);
                    k.SetParamter<int>(isForward ? 1 : -1, true);
                },
                (k) =>
                {
                    Real = k.GetBufferValue<double>(1);
                    Imag = k.GetBufferValue<double>(2);
                },
                NumberOfSamples);
        }

        public void Populate(ref double[,] real, ref double[,] imag)
        {
            real = new double[NumberOfSamples, VectorLength];
            imag = new double[NumberOfSamples, VectorLength];
            for (int s = 0; s < NumberOfSamples; s++)
            {
                for (int i = 0; i < VectorLength; i++)
                {
                    real[s, i] = Real[s * VectorLength + i];
                    imag[s, i] = Imag[s * VectorLength + i];
                }
            }
        }

        #endregion

        #region static methods

        public static void Forward(ref double[,] real, ref double[,] imag, int minZeroPad = -1)
        {
            FFT(false, ref real, ref imag, minZeroPad);
        }

        public static void Reverse(ref double[,] real, ref double[,] imag, int minZeroPad = -1)
        {
            FFT(false, ref real, ref imag, minZeroPad);
        }

        public static void FFT(bool isForward, ref double[,] real, ref double[,] imag, int minZeroPad = -1)
        {
            R2FFTOpenCL fft = new R2FFTOpenCL(real, imag, minZeroPad);
            fft.Run(isForward);
            fft.Populate(ref real, ref imag);
        }

        #endregion
    }
}
