using GSI.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Storage.Spectrum
{
    [DataContract(Name = "SpectrumFileStreamSettings")]
    /// <summary>
    /// The colelction settings for an fft file stream. (Stored to json).
    /// </summary>
    public class SpectrumStreamSettings : IJsonObject
    {
        public SpectrumStreamSettings(int numberOfLines,
            int lineSize,
            int vectorSize,
            int fftSize,
            int fftDataSize,
            double stepSize,
            double pixelSize,
            double startWavelength = -1,
            double endWavelength = -1,
            bool isDoublePrecision = false)
        {
            NumberOfLines = numberOfLines;
            VectorSize = vectorSize;
            FftSize = fftSize;
            IsDoublePrecision = isDoublePrecision;
            LineSize = lineSize;
            StepSizeInPixels = stepSize;
            PixelSize = pixelSize;
            FftDataSize = fftDataSize;
            StartWavelength = startWavelength;
            EndWavelength = endWavelength;
        }

        /// <summary>
        /// Serialzation constructor.
        /// </summary>
        protected SpectrumStreamSettings()
        {
            if (FftDataSize == 0)
                FftDataSize = FftSize / 2;
        }

        #region members

        /// <summary>
        /// The width in pixels.
        /// </summary>
        public int Width { get { return VectorSize * NumberOfLines; } }

        /// <summary>
        /// The height in pixels.
        /// </summary>
        public int Height { get { return LineSize; } } 

        [DataMember]
        /// <summary>
        /// The line size in number of vectors.
        /// </summary>
        public int LineSize { get; private set; }

        [DataMember]
        /// <summary>
        /// The number of pixels in a vector.
        /// </summary>
        public int VectorSize { get; private set; }

        [DataMember]
        /// <summary>
        /// The number of values in an fft size.
        /// </summary>
        public int FftSize { get; private set; }

        [DataMember]
        /// <summary>
        /// The number of values in each fft vector.
        /// </summary>
        public int FftDataSize { get; private set; }

        [DataMember]
        /// <summary>
        /// The number of lines that is expected.
        /// </summary>
        public int NumberOfLines { get; private set; }

        [DataMember]
        /// <summary>
        /// The pixel size of the image.
        /// </summary>
        public double PixelSize { get; private set; }

        [DataMember]
        /// <summary>
        /// The step size in the stacking collector, in pixels.
        /// </summary>
        public double StepSizeInPixels { get; private set; }

        [DataMember]
        /// <summary>
        /// True if the current stream has double precision info.
        /// </summary>
        public bool IsDoublePrecision { get; private set; }

        /// <summary>
        /// The start wavelength that applies to the fft.
        /// </summary>
        [DataMember]
        public double StartWavelength { get; set; }

        /// <summary>
        /// The end wvelength that applies to the fft.
        /// </summary>
        [DataMember]
        public double EndWavelength { get; set; }

        /// <summary>
        /// The number of precision bytes in the stream.
        /// </summary>
        public int NumberOfPrecisionBytes { get { return IsDoublePrecision ? sizeof(double) : sizeof(float); } }

        /// <summary>
        /// The max number of pixels.
        /// </summary>
        public long NumberOfPixels { get { return (long)Width*(long)Height; } }

        /// <summary>
        /// The spectrum data size according to the precision bytes.
        /// </summary>
        public long SpectrumDataSizeInBytes
        {
            get
            {
                return (long)NumberOfPixels *
                (long)(NumberOfPrecisionBytes * FftDataSize);
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Convert the current object to json
        /// </summary>
        /// <returns></returns>
        public static SpectrumStreamSettings FromJson(string json)
        {
            return json.FromJson<SpectrumStreamSettings>();
        }

        #endregion
    }
}
