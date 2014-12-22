using GSI.Coding;
using GSI.OpenCL;
using GSI.Storage.Spectrum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Processing
{
    public class FFTProcessor : DataProcessesor
    {
        public const int _MB_=1000000;
        public const int _MaxNumberOfMemoryBytesInOpenCLDevice = 60;
        public const int _DefaultZeroFillTo = 256;

        /// <summary>
        /// Processes the image to create the final forier
        /// </summary>
        /// <param name="source">he image source</param>
        public FFTProcessor(StackingReader source,
            SpectrumStreamSettings settings,
            int maxNumberOfBytesInMemory = _MaxNumberOfMemoryBytesInOpenCLDevice* _MB_,
            AppodizationMaskAlgorithem appodizationMask = AppodizationMaskAlgorithem.BlackmanHarris,
            bool doSubstractAvarage = true)
            : base(source)
        {
            MemoryMaxSizeInBytes = maxNumberOfBytesInMemory / 4; // there are at least 2 vectors, read and write.
            AppodizationMask = appodizationMask;
            DoSubstractAvarage = doSubstractAvarage;
            Settings = settings;
            Devices = new List<GpuTaskDeviceInfo>();
        }

        #region members

        /// <summary>
        /// The device info associated wit the current.
        /// </summary>
        public List<GpuTaskDeviceInfo> Devices { get; private set; }

        /// <summary>
        /// The mask algorithem to use.
        /// </summary>
        public AppodizationMaskAlgorithem AppodizationMask { get; set; }

        /// <summary>
        /// Substract the avarage from the current.
        /// </summary>
        public bool DoSubstractAvarage { get; set; }

        /// <summary>
        /// The spectrum stream settings.
        /// </summary>
        public SpectrumStreamSettings Settings { get; private set; }

        #endregion

        #region methods

        public override int GetMaxNumberOfVectorsToLoad()
        {
            // need to take into account the amount of memory needed for the fft.
            double singleFFTDataVector = GSI.OpenCL.FFT.R2FFT.CalculateMemoryRequieredForVectorInBytes(
                Settings.FftSize, AppodizationMask != AppodizationMaskAlgorithem.None);
            return (int)Math.Floor(MemoryMaxSizeInBytes * 1.0 / (singleFFTDataVector * Source.VectorSize));

            // old
            // return (int)Math.Floor(MemoryMaxSizeInBytes * 1.0 / (FffVectorSize * Source.VectorSize));
        }

        public float[] CalculateApodizationMask(AppodizationMaskAlgorithem maskType)
        {
            if (maskType == AppodizationMaskAlgorithem.None)
                return null;

            double[] mask = new double[Source.StackSize];
            for (int i = 0; i < Source.StackSize; i++)
            {
                switch (maskType)
                {
                    case AppodizationMaskAlgorithem.BlackmanHarris:
                        mask[i] = .42323 +
                            .49755 * Math.Cos(Math.PI * (i * 1.0 / (Source.StackSize - 1) - 0.5)) +
                            .07922 * Math.Cos(2 * Math.PI * (i * 1.0 / (Source.StackSize - 1) - 0.5));
                        break;
                    default:
                        mask[i] = .54 + .46 * Math.Cos(Math.PI * (i * 1.0 / (Source.StackSize - 1) - 0.5));
                        break;
                }
                // calculating the mask.
            }
            //            for i=0:Np-1

            //    HG(i+1)=.54+.46.*cos(pi.*(i./(Np-1)-0.5));
            //    BH(i+1)=.42323+.49755.*cos(pi.*(i./(Np-1)-0.5))+.07922.*cos(2*pi.*(i./(Np-1)-0.5));

            //end
            return mask.Select(v => (float)v).ToArray();
        }

        /// <summary>
        /// Writes the source for the stacked image.
        /// </summary>
        /// <param name="image"></param>
        public void ToForierTransform(Stream output, ISpectrumStreamProcedure[] procedures = null)
        {
            // create the basic fft.
            GSI.OpenCL.FFT.R2FFT fft = new OpenCL.FFT.R2FFT(Settings.FftSize);
            fft.DoSubstractAvarage = DoSubstractAvarage;

            // adding the mask according to the apodiszation.
            fft.SetMask(CalculateApodizationMask(AppodizationMask));

            // creating the fft vector buffers.
            float[] realData = null;
            float[] imagData = null;
            float[] zerosVector = null;
            float[] realRslt = null;
            float[] imagRslt = null;
            float[] magRslt = null;
            byte[] writeDataBuffer = null;

            CodeTimer timer = new CodeTimer();
            GpuTaskDeviceInfo runningDevice = Devices.Count == 0 ? null : Devices[0];

            SpectrumStreamWriter writer = new SpectrumStreamWriter(output, Settings);

            if (procedures != null)
                writer.WriteProcedures.AddRange(procedures);
            writer.Initialize();

            // reading the data and writing the forier transport 
            DoDataProcessing((startIndex, nread, vectors, dataVector) =>
            {
                timer.Reset();
                timer.Start();
                int totalNumberOfFftValues = nread * Source.VectorSize * Source.StackSize;
                int totalNumberDataValuesToRead = nread * Source.VectorSize * Settings.FftDataSize;
                if (realData == null || realData.Length != nread * Source.VectorSize * Source.StackSize)
                {
                    // the first nread must be the largest that can be.
                    realData = new float[totalNumberOfFftValues];
                    imagData = new float[totalNumberOfFftValues];
                    realRslt = new float[totalNumberDataValuesToRead];
                    imagRslt = new float[totalNumberDataValuesToRead];
                    magRslt = new float[totalNumberDataValuesToRead];
                    zerosVector = new float[totalNumberOfFftValues];
                    writeDataBuffer = new byte[nread * Source.VectorSize *
                        Settings.FftDataSize * Settings.NumberOfPrecisionBytes];
                }
                else
                {
                    // need to zero out the imagData since it will be updated only later.
                    System.Buffer.BlockCopy(imagData, 0, zerosVector, 0, imagData.Length);
                }

                // prepares the data read.
                timer.Mark("Prepare data members");

                if (Aborted)
                    return;

                // reading the data into the real vector.
                ConvertByteToFloat(ref dataVector, ref realData, 0, totalNumberOfFftValues);

                // prepares the data read.
                timer.Mark("Prepare double data");

                // set the data to the fft.
                fft.SetData(Source.StackSize, realData, imagData);

                timer.Mark("Prepare fft");

                if (Aborted)
                    return;

                // run the fft.
                fft.Run(true, runningDevice);

                timer.Mark("fft run");

                fft.Populate(Settings.FftDataSize, Settings.FftDataOffsetIndex, ref realRslt, ref imagRslt, ref magRslt);

                timer.Mark("populate fft");

                if (Aborted)
                    return;

                writer.WriteVectors(startIndex, nread, magRslt);

                timer.Mark("Writing");

                timer.Mark("done", true);
            });

            // waiting for all to finish.
            writer.WaitForAllWriteActionsToCompleate();
            writer.Dispose();
        }

        private static double NormFFTVector(int l, double valr)
        {
            valr /= l;
            if (valr > 255)
                valr = 255;
            else if (valr < 0)
                valr = 0;
            return valr;
        }

        #endregion

        #region unsafe conversion methods

        /// <summary>
        /// Fast unsafe conversion of the source array to target array.
        /// </summary>
        /// <param name="source">The source </param>
        /// <param name="target">The target</param>
        public unsafe void ConvertByteToFloat(ref byte[] source, ref float[] target, int offset = 0, int count = -1)
        {
            if (count < 0)
            {
                count = source.Length - offset;
            }

            int maxL = offset + count;
            if (source.Length < maxL || target.Length < maxL)
            {
                throw new Exception("Out of bounds of target or source array. ");
            }

            fixed (byte* pSource = source)
            fixed (float* pTarget = target)
            {
                ConvertHelper helper = new ConvertHelper(pTarget, pSource);
                Parallel.For(0, maxL, (i) => helper.DoConvert(i));
            }
        }

        private unsafe class ConvertHelper
        {
            public ConvertHelper(float* target, byte* source)
            {
                Target = target;
                Source = source;
            }

            public float* Target;
            public byte* Source;

            public void DoConvert(int i)
            {
                Target[i] = (float)Source[i];
            }
        }

        #endregion
    }


    /// <summary>
    /// The algorithem to create the appodization.
    /// </summary>
    public enum AppodizationMaskAlgorithem { None, HappGenzel, BlackmanHarris };
}
