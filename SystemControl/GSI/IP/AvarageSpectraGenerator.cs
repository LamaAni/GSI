using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.IP
{
    public class AvarageSpectraGenerator
    {
        /// <summary>
        /// Creates a new avarage spectra generator.
        /// </summary>
        /// <param name="minThreshold">Normalized to 1, the minimal threshold, about the sum of the spectra, where the spectra will be included.</param>
        /// <param name="maxThreshold">Normalized to 1, the maximal threshold, about the sum of the spectra, where the spectra will be included.</param>
        public AvarageSpectraGenerator(double minThreshold, double maxThreshold = 1)
        {
            MinThreshold = minThreshold;
            MaxThreshold = maxThreshold;
        }

        #region members

        public double MinThreshold { get; private set; }
        public double MaxThreshold { get; private set; }

        public long TotalPixelsToBeRead { get; private set; }
        public long TotalRead { get; private set; }
        public bool IsRunning { get; private set; }

        public float[] AvarageSpectra { get; private set; }

        #endregion

        #region Spectra generation

        public unsafe void Make(Storage.Spectrum.SpectrumStreamProcessor proc, bool async=true, double totalNumberOfMBInMemory = 500)
        {
            IsRunning = true;
            Action f = () =>
            {
                // loading the spectrum
                // calculating the avarage spectrum.
                proc.SeekToPixelPosition(0); // seeking to the first pixel.

                TotalPixelsToBeRead = proc.Settings.NumberOfPixels * 2;
                // generating total sum.
                double maxSum = 0;
                int fftDataSize = proc.Settings.FftDataSize;
                proc.DoSpectrumProcessing((processor, x, y, nread, vals) =>
                {
                    // adding to the total sum and deviding by the number of values per pixel.
                    fixed (float* _vals = vals)
                    {
                        for (int i = 0; i < nread; i++)
                        {
                            // calculating the current sum,
                            double sum = 0;
                            int curIdx = fftDataSize * i;
                            for (int j = 0; j < fftDataSize; j++)
                            {
                                sum += _vals[curIdx + j];
                            }

                            if (sum > maxSum)
                                maxSum = sum;
                        }
                    }
                    TotalRead += nread;
                }, false, totalNumberOfMBInMemory);

                // reading again, now only pixels whos sum is in the specific threshold.
                proc.SeekToPixelPosition(0, 0);

                int totalInThresholdValues = 0;
                float[] spectra = new float[proc.Settings.FftDataSize];
                double minThreshold = MinThreshold;
                double maxThreshold = MaxThreshold;

                proc.DoSpectrumProcessing((processor, x, y, nread, vals) =>
                {
                    fixed (float* _vals = vals)
                    {
                        for (int i = 0; i < nread; i++)
                        {
                            // calculating the current sum,
                            double sum = 0;
                            int curIdx = fftDataSize * i;
                            for (int j = 0; j < fftDataSize; j++)
                            {
                                sum += _vals[curIdx + j];
                            }
                            sum /= (maxSum); // normalized to 1.s
                            if (sum > minThreshold && sum < maxThreshold)
                            {
                                totalInThresholdValues += 1;
                                for (int j = 0; j < fftDataSize; j++)
                                {
                                    spectra[j] += _vals[curIdx + j];
                                }
                            }
                        }
                    }
                    TotalRead += nread;
                }, false, totalNumberOfMBInMemory);

                // calculating the final spectra.
                AvarageSpectra = spectra.Select(i => i / totalInThresholdValues).ToArray();
                proc.Close();
                IsRunning = false;
            };

            if (async)
                Task.Run(f);
            else f();
        }

        #endregion
    }
}
