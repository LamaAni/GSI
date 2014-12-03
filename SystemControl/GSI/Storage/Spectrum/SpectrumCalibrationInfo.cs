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
    public class SpectrumCalibrationInfo
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
        }

        Dictionary<int, List<double>> paramsForCalibration = new Dictionary<int, List<double>>();
    }
}
