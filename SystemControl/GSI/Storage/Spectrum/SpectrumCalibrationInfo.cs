using GSI.Storage.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Storage.Spectrum
{
    /// <summary>
    /// Holds information about a spectrum calibration, for several types of FFT zero fills.
    /// </summary>
    public class SpectrumCalibrationInfo : IEnumerable<KeyValuePair<int, double[]>>
    {
        /// <summary>
        /// Create a new calibration info from a csv file.
        /// </summary>
        /// <param name="source"></param>
        public SpectrumCalibrationInfo(string source)
            :this(new CSVMat(source))
        {
        }

        /// <summary>
        /// Create a new calibration info from a csv file.
        /// </summary>
        /// <param name="source"></param>
        public SpectrumCalibrationInfo(CSVMat mat)
        {
            foreach (var l in mat)
            {
                if (l.Count < 4)
                    continue;
                double parser;
                if (l.Take(6).Any(v => !double.TryParse(v, out parser)))
                    continue;
                double[] vals = l.Take(6).Select(v => double.Parse(v)).ToArray();
                m_calibrationParameters[Convert.ToInt32(vals[0])] = vals.Skip(1).Take(5).ToArray();
            }
        }

        #region members

        Dictionary<int, double[]> m_calibrationParameters = new Dictionary<int, double[]>();

        /// <summary>
        /// The raw calibration paramers for zero filling.
        /// </summary>
        public Dictionary<int, double[]> CalibrationParametersByZeroFill
        {
            get { return m_calibrationParameters; }
        }

        #endregion

        #region methods

        public Tuple<double, int>[] GetFrequenciesByIndexForZeroFill(int zerofill)
        {
            // the calibration coefficients.
            double[] coef = CalibrationParametersByZeroFill[zerofill];

            Tuple<double, int>[] frequencyByIndex = new Tuple<double, int>[zerofill];
            double units = coef[1];
            bool asWavelength=coef[0]==1;
            for (int i = 0; i < zerofill; i++)
            {
                double freq=units * (coef[2] / (i + coef[3]) - coef[4]);
                if(asWavelength)
                    freq = 1.0 / freq;
                frequencyByIndex[i] = new Tuple<double, int>(freq, i);
            }
            return frequencyByIndex;
        }

        /// <summary>
        /// Calculates the start and end indices for the forier transform
        /// for the specified wavelengths. Gives the closest possible wavelength 
        /// binned to the spectrum index.
        /// </summary>
        /// <param name="startWavelength">The wavelength to start from.</param>
        /// <param name="endWavelegth">The wavelength to end with</param>
        /// <param name="start">The values of the start index, (wavelength, index)</param>
        /// <param name="end">The values of the end index, (wavelength, index)</param>
        public void GetFrequencyIndices(int zerofill, double startFreq, double endFreq,
            out Tuple<double, int> start, out Tuple<double,int> end)
        {
            if (!CalibrationParametersByZeroFill.ContainsKey(zerofill))
                throw new Exception("Cannot find appropriate zero fill. The zero fill was not defined or CSV did not contain such zero fill.");

            // the calibration coefficients.
            Tuple<double, int>[] freqByIndex = GetFrequenciesByIndexForZeroFill(zerofill);

            // now convert to the closest start wavelength and end wavelengths.
            if (startFreq > 0)
                start = FindClosest(startFreq, freqByIndex);
            else start = new Tuple<double, int>(0, 0);
            if (endFreq > 0)
                end = FindClosest(endFreq, freqByIndex);
            else end = freqByIndex.Last();
        }


        /// <summary>
        /// Helps find the closest wavelength to the requested.
        /// </summary>
        /// <param name="startWavelength"></param>
        /// <param name="freqByIndex"></param>
        /// <returns></returns>
        private static Tuple<double, int> FindClosest(double startWavelength, Tuple<double, int>[] freqByIndex)
        {
            return freqByIndex.Aggregate((a, b) =>
                Math.Abs(a.Item1 - startWavelength) < Math.Abs(b.Item1 - startWavelength) ? a : b);
        }

        /// <summary>
        /// Fills the spectrum stream settings according to the parameters provided.
        /// If parameters were not provided uses the spectrum stream settings to fill.
        /// </summary>
        /// <param name="settings">The settings to fill</param>
        /// <param name="zerofill">The zero filling</param>
        /// <param name="startWavelength">The start wavelength</param>
        /// <param name="endWavelegth">The end wavelegnth</param>
        public void FillSpectrumStreamSettings(SpectrumStreamSettings settings,
            int zerofill = -1, double startWavelength = -1, double endWavelegth = -1)
        {
            Tuple<double, int> start, end;
            GetFrequencyIndices(
                zerofill < 0 ? settings.FftSize : zerofill,
                startWavelength < 0 ? settings.StartFrequency : startWavelength,
                endWavelegth < 0 ? settings.EndFrequency : endWavelegth,
                out start, out end);

            // now update the settings.
            settings.FftDataOffsetIndex = start.Item2;
            settings.FftDataSize = end.Item2 - start.Item2;
            settings.StartFrequency = start.Item1;
            settings.EndFrequency = end.Item1;
        }

        /// <summary>
        /// Returns the calibration as a csv matrix.
        /// </summary>
        /// <returns></returns>
        public CSVMat ToCSVMat()
        {
            CSVMat mat = new CSVMat();
            int row = 0;
            foreach (var calib in CalibrationParametersByZeroFill)
            {
                mat[row].Add(calib.Key.ToString());
                mat[row].AddRange(calib.Value.Select(v => v.ToString()));
                row += 1;
            }
            return mat;
        }

        /// <summary>
        /// Returns the calibration as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToCSVMat().ToCSVString();
        }

        #endregion

        public IEnumerator<KeyValuePair<int, double[]>> GetEnumerator()
        {
            return CalibrationParametersByZeroFill.OrderBy(kvp => kvp.Key).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
