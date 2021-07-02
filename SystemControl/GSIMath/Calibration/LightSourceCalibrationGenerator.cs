using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Calibration
{
    /// <summary>
    /// Generates a spectral calibration for a specific light source
    /// </summary>
    public class LightSourceCalibrationGenerator
    {
        /// <summary>
        /// Generates a spectral calibration value from specific frequenceis 
        /// and amplitudes.
        /// </summary>
        /// <param name="frequenceis"></param>
        /// <param name="intensity"></param>
        public LightSourceCalibrationGenerator(float[] frequenceis, float[] intensity)
        {
            int i=0;
            Tuple<float, float>[] vals = frequenceis.Select(f =>
            {
                Tuple<float, float> tpl = new Tuple<float, float>(f, intensity[i]);
                i += 1;
                return tpl;
            }).OrderBy(tpl => tpl.Item1).ToArray();

            SourceFrequencies = vals.Select(tpl => tpl.Item1 * 1.0).ToArray();
            SourceIntensities = vals.Select(tpl => tpl.Item2 * 1.0).ToArray();
        }

        #region members

        double[] SourceFrequencies;
        double[] SourceIntensities;

        #endregion

        #region methods

        public float[] InterpolateIntensityValues(float[] freq)
        {
            // fast find.
            // find closest index which is not above.
            var itrp = MathNet.Numerics.Interpolation.LinearSpline.Interpolate(
                SourceFrequencies, SourceIntensities);
            return freq.Select(f => (float)itrp.Interpolate(f)).ToArray();
        }

        /// <summary>
        /// Generates the intesity correction for the selected frequencies.
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public float[] GenerateIntensityCorrection(float[] fs)
        {
            float[] ivals = InterpolateIntensityValues(fs);

            // assuming the minimal value is zero, normalzing the maximal values to 1. 
            float maxVal = ivals.Max();
            return ivals.Select(v => 1F - v / maxVal).ToArray();
        }

        /// <summary>
        /// Generates rgb correction values for the specified data values.
        /// </summary>
        /// <param name="converter"></param>
        /// <param name="prs"></param>
        /// <param name="normals"></param>
        /// <returns></returns>
        public float[] GenerateWhiteBalanceCorrection(FrequencyToRgbConvertor converter, FrequencyToRgbConvertor.ConversionParmas prs = null, float[] normals = null)
        {
            // assuming a single white pixel.            
            float[] maxAmps = GenerateIntensityCorrection(converter.Frequencies);
            float[] rgb = converter.ToRGB(maxAmps, prs, normals);
            return new float[3] { 1, rgb[1] / rgb[0], rgb[2] / rgb[0] };
        }

        #endregion
    }
}
