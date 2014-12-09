using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Calibration
{
    /// <summary>
    /// Creates a freqeucny to rgb converter given the frequencies.
    /// </summary>
    public unsafe class FrequencyToRgbConvertor
    {
        /// <summary>
        /// Creates a new converter, given the specified x frequencies.
        /// </summary>
        /// <param name="frequencies">The frequencies to calculate from. (Any amplitude vector must be the same length)</param>
        /// <param name="method">The method to calculate by.</param>
        public FrequencyToRgbConvertor(float[] frequencies, FrequencyToRGBConvertorMethod method =
             FrequencyToRGBConvertorMethod.GAUSSIAN_APPROX_sRGB)
        {
            float[] x, y, z;
            GetCoefficientsForFrequencies(frequencies, out x, out y, out z);
            XCoef = x;
            YCoef = y;
            ZCoef = z;
            Frequencies = frequencies;
            ConversionMatrix = GetRGBConversionMatrix(method);
        }

        #region members

        /// <summary>
        /// The frequencies to convert by.
        /// </summary>
        public float[] Frequencies { get; private set; }

        /// <summary>
        /// The x coefficiants for the selected method.
        /// </summary>
        public float[] XCoef { get; private set; }

        /// <summary>
        /// The y coefficiants for the selected method.
        /// </summary>
        public float[] YCoef { get; private set; }

        /// <summary>
        /// The z coefficiants for the selected method.
        /// </summary>
        public float[] ZCoef { get; private set; }

        /// <summary>
        /// The conversion matrix for the spcified convert.
        /// </summary>
        public float[,] ConversionMatrix { get; private set; }

        /// <summary>
        /// The method to convert by.
        /// </summary>
        public FrequencyToRGBConvertorMethod Method { get; private set; }

        #endregion

        #region helper classes

        /// <summary>
        /// A collection of parameters used for conversion. Use this class
        /// to store parameters fro the conversion.
        /// </summary>
        public unsafe class ConversionParmas
        {
            public ConversionParmas(int count, float[] xcoef, float[] ycoef, float[] zcoef, float[,] cmat)
            {
                XCoef = xcoef;
                YCoef = ycoef;
                ZCoef = zcoef;
                ConversionMatrix = cmat;
                Count = count;
            }

            /// <summary>
            /// The number of samples.
            /// </summary>
            public int Count { get; private set; }

            /// <summary>
            /// The x coefficiants for the selected method.
            /// </summary>
            public float[] XCoef { get; private set; }

            /// <summary>
            /// The y coefficiants for the selected method.
            /// </summary>
            public float[] YCoef { get; private set; }

            /// <summary>
            /// The z coefficiants for the selected method.
            /// </summary>
            public float[] ZCoef { get; private set; }

            /// <summary>
            /// The conversion matrix for the spcified convert.
            /// </summary>
            public float[,] ConversionMatrix { get; private set; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Converts the amplitudes into three values of rgb. If prs is null - 
        /// slower.
        /// </summary>
        /// <param name="amplitudes"></param>
        /// <param name="prs">A collection of parmaters that can be intitialzied to fixed memory values.</param>
        /// <returns></returns>
        public float[] ToRGB(float[] amplitudes, ConversionParmas prs = null, float[] normal = null)
        {
            // checking and creating params if nessesary.
            float[] rslt;
            fixed (float* amp = amplitudes)
            {
                if (prs == null)
                {
                    prs = new ConversionParmas(Frequencies.Length, XCoef, YCoef, ZCoef, ConversionMatrix);
                }
                rslt = ToRGB(amp, 0, prs, normal);
            }
            return rslt;
        }

        /// <summary>
        /// Converts the unsafe amplitudes 
        /// </summary>
        /// <param name="amp"></param>
        /// <param name="prs"></param>
        /// <returns></returns>
        public unsafe float[] ToRGB(float* amp, int offset, ConversionParmas prs, float[] normal = null)
        {
            if (prs == null)
                throw new Exception("Parameters cannot be null.");
            float[] rslt;
            fixed (float* x = prs.XCoef, y = prs.YCoef, z = prs.ZCoef)
            {
                rslt = UnsafeToRGB(prs.Count, x, y, z, amp, prs.ConversionMatrix, normal);
            }
            return rslt;
        }

        #endregion

        #region static methods

        /// <summary>
        /// Convert the frequencies and the amplitude coefficients to rgb (unnormalized, non trapz.)
        /// </summary>
        /// <param name="n">The number of samples.</param>
        /// <param name="xcoef">The coefficients of x.</param>
        /// <param name="ycoef"></param>
        /// <param name="zcoef"></param>
        /// <param name="amplitude"></param>
        /// <param name="cMat"></param>
        /// <returns></returns>
        public static float[] UnsafeToRGB(
            int n, float* xcoef, float* ycoef, float* zcoef, float* amplitude, float[,] cMat, float[] normal = null)
        {
            // calculating the integrals. 
            float x = 0, y = 0, z = 0;
            for (int i = 0; i < n - 1; i++)
            {
                x += xcoef[i] * amplitude[i];
                y += ycoef[i] * amplitude[i];
                z += zcoef[i] * amplitude[i];
            }

            // normalizing for the number of values.
            if (normal != null)
            {
                x *= normal[0];
                y *= normal[1];
                z *= normal[2];
            }

            // calculating the matrix. 
            float r = cMat[0, 0] * x + cMat[0, 1] * y + cMat[0, 2] * z;
            float g = cMat[1, 0] * x + cMat[1, 1] * y + cMat[1, 2] * z;
            float b = cMat[2, 0] * x + cMat[2, 1] * y + cMat[2, 2] * z;

            return new float[3] { r, g, b };
        }

        /// <summary>
        /// Calculates the x,y,z coefficients for the specified wavelengths.
        /// </summary>
        /// <param name="freq">The frequenices to calculate for</param>
        /// <param name="xcoef"></param>
        /// <param name="ycoef"></param>
        /// <param name="zcoef"></param>
        public static void GetCoefficientsForFrequencies(float[] freq,
            out float[] xcoef, out float[] ycoef, out float[] zcoef)
        {
            xcoef = new float[freq.Length];
            ycoef = new float[freq.Length];
            zcoef = new float[freq.Length];

            // calculating the coefficients.
            for (int i = 0; i < freq.Length; i++)
            {
                float lambda = 1.0e9F / freq[i];
                float deltaf = i + 1 == freq.Length ?
                    (float)Math.Abs(freq[i] - freq[i - 1]) :
                    (float)Math.Abs(freq[i + 1] - freq[i]);

                // the following are always numbers, so no units needed.
                xcoef[i] = 1.155F * (float)Math.Exp(-Math.Pow((lambda - 593.1F) / 48.31F, 2)) +
                    0.4168F * (float)Math.Exp(-Math.Pow((lambda - 444.1F) / 27.49F, 2));
                ycoef[i] = 1.015F * (float)Math.Exp(-Math.Pow((lambda - 555.5F) / 65.89F, 2));
                zcoef[i] = 2.153F * (float)Math.Exp(-Math.Pow((lambda - 447.9F) / 31.05F, 2));

                // applying the delta frequency, back to frequency units.
                xcoef[i] *= deltaf;
                ycoef[i] *= deltaf;
                zcoef[i] *= deltaf;
            }
        }

        /// <summary>
        /// Returns the appropriate conversion matrix for the sleected method.
        /// </summary>
        /// <param name="m_method"></param>
        /// <returns></returns>
        public static float[,] GetRGBConversionMatrix(FrequencyToRGBConvertorMethod m_method)
        {
            switch (m_method)
            {
                default: // return sRGB
                    return new float[,] { 
                    { 3.1338561F, - 1.6168667F ,- 0.4906146F }, 
                    { -0.9787684F,  1.9161415F,  0.0334540F}, 
                    { 0.0719453F, -0.2289914F,  1.4052427F, } 
                    };
            }
        }

        #endregion
    }

    public enum FrequencyToRGBConvertorMethod
    {
        GAUSSIAN_APPROX_sRGB,
    }
}
