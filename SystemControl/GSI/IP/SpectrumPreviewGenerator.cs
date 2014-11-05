﻿using GSI.Storage.Spectrum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.IP
{
    /// <summary>
    /// Implements a spectra preview generation class that makes a spectra preview from a 
    /// spectra file.
    /// </summary>
    public class SpectrumPreviewGenerator : IDisposable
    {
        /// <summary>
        /// Creates a new spectrum preview generator.
        /// </summary>
        /// <param name="reader">The spectrum preview maker</param>
        /// <param name="to">The stream to store the image to.</param>
        /// <param name="startOffset">The start offset within the spectra stream.</param>
        public SpectrumPreviewGenerator(SpectrumStreamProcessor processor, string filename)
            : this(processor, File.Create(filename))
        {
        }

        /// <summary>
        /// Creates a new spectrum preview generator.
        /// </summary>
        /// <param name="reader">The spectrum preview maker</param>
        /// <param name="to">The stream to store the image to.</param>
        /// <param name="startOffset">The start offset within the spectra stream.</param>
        public SpectrumPreviewGenerator(SpectrumStreamProcessor processor, Stream to)
            : this(processor, new PreviewStream(processor.Settings.Width, processor.Settings.Height, to))
        {
        }
        /// <summary>
        /// Creates a new spectrum preview generator.
        /// </summary>
        /// <param name="reader">The spectrum preview maker</param>
        /// <param name="preview">The stream to store the image to.</param>
        /// <param name="startOffset">The start offset within the spectra stream.</param>
        public SpectrumPreviewGenerator(SpectrumStreamProcessor processor, PreviewStream preview)
        {
            Preview = preview;
            Processor = processor;
        }

        #region static methods

        /// <summary>
        /// Creates a stream reader to the file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SpectrumPreviewGenerator Create(string filename, string previewFilename, int width, int height)
        {
            return Create(filename, new PreviewStream(width, height, File.Create(previewFilename)));
        }

        /// <summary>
        /// Creates a stream reader to the file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SpectrumPreviewGenerator Create(string filename, PreviewStream preview)
        {
            if (!File.Exists(filename))
                throw new Exception("File dose not exist.");
            FileStream strm = File.Open(filename, FileMode.Open);
            SpectrumStreamProcessor processor = new SpectrumStreamProcessor(strm);

            return new SpectrumPreviewGenerator(processor, preview);
        }

        #endregion

        #region members

        /// <summary>
        /// The preview stream.
        /// </summary>
        public PreviewStream Preview { get; private set; }

        /// <summary>
        /// The spectrum stream processor that loads the data.
        /// </summary>
        public SpectrumStreamProcessor Processor { get; private set; }

        /// <summary>
        /// If true dose the processing async.
        /// </summary>
        public bool IsAsync { get; private set; }

        /// <summary>
        /// The expected number of counts.
        /// </summary>
        public int ExpectedDoneCount { get; private set; }
        
        /// <summary>
        /// The current read count.
        /// </summary>
        public int CurCount { get; private set; }

        /// <summary>
        /// Called when a block is complete.
        /// </summary>
        public event EventHandler<SperctrumGeneratorBlockDoneEventArgs> OnBlockComplete;

        /// <summary>
        /// Called when all loading processes compleated.
        /// </summary>
        public event EventHandler OnCompleated;

        /// <summary>
        /// The current reading mode.
        /// </summary>
        public SperctrumGeneratorBlockDoneMode CurrentMode { get; private set; }

        /// <summary>
        /// Red mask to apply to vector data. If null then avarage all values.
        /// </summary>
        public float[] RedMask { get; private set; }

        /// <summary>
        /// Blue mask to apply to vector data. If null then avarage all values.
        /// </summary>
        public float[] BlueMask { get; private set; }

        /// <summary>
        /// Greend mask to apply to vector data. If null then avarage all values.
        /// </summary>
        public float[] GreenMask { get; private set; }

        /// <summary>
        /// The number of pixels to skip when converting a pixel value.
        /// </summary>
        public int SkipOnPixelConvert { get; set; }

        #endregion

        #region helper buffers

        byte[] _byteBuffer = null;
        float[] _dataBuffer = null;

        #endregion

        #region processing

        /// <summary>
        /// Abort the processing.
        /// </summary>
        public void Abort() { Processor.Abort(); }

        /// <summary>
        /// Generates the vector 
        /// </summary>
        public void Make(bool doValidate = true, bool async = true)
        {
            Action f = () =>
            {
                ExpectedDoneCount = Processor.Settings.Width * Processor.Settings.Height +
                    (doValidate ? Preview.GetNumberOfPixelsToPreviewValidate() : 0);
                CurCount = 0;
                CurrentMode = SperctrumGeneratorBlockDoneMode.CreatingSpectrum;
                Processor.DoSpectrumProcessing(DoProcessing, false);
                CurrentMode = SperctrumGeneratorBlockDoneMode.CreatingPreview;
                if (doValidate)
                    Preview.ValidatePreview((total, read) =>
                    {
                        CurCount += read;
                        InvokeBlockEvent(read);
                    });

                if (OnCompleated != null)
                    OnCompleated(this, null);
            };

            if (async)
                Task.Run(f);
            else f();
        }

        protected void InvokeBlockEvent(int thisLeg)
        {
            if (OnBlockComplete != null)
                OnBlockComplete(this, new SperctrumGeneratorBlockDoneEventArgs(ExpectedDoneCount, CurCount,
                    thisLeg, CurrentMode));
        }

        /// <summary>
        /// Writing data to the preview stream.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="length"></param>
        /// <param name="values"></param>
        void DoProcessing(SpectrumStreamProcessor processor, int x, int y, int length, float[] values)
        {
            MakePixelValues(values, ref _dataBuffer, processor.Settings.FftDataSize);
            Preview.SetImageData(_dataBuffer, x, y, length, ref _byteBuffer, 0);
            CurCount += length;
            InvokeBlockEvent(length);
        }

        /// <summary>
        /// Convert the specrtrum data to pixel data.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="data"></param>
        unsafe void MakePixelValues(float[] values, ref float[] data, int vectorLength)
        {
            bool hasRedMask = RedMask != null && RedMask.Length >= vectorLength - SkipOnPixelConvert;
            bool hasBlueMask = BlueMask != null && BlueMask.Length >= vectorLength - SkipOnPixelConvert;
            bool hasGreenMask = GreenMask != null && GreenMask.Length >= vectorLength - SkipOnPixelConvert;
            bool requiresAvarage = !hasBlueMask || !hasGreenMask || !hasRedMask;
            int numberOfPixels = values.Length / vectorLength;
            if (data == null)
                data = new float[numberOfPixels * 3];
            
            fixed (float* pValues = values, pData = data)
            {
                for (int pi = 0; pi < numberOfPixels; pi++)
                {
                    int startIndex = pi * vectorLength + SkipOnPixelConvert;
                    int endIndex = (pi + 1) * (vectorLength);
                   
                    float av = 0;
                    if (requiresAvarage)
                    {
                        for (int vi = startIndex; vi < endIndex; vi++)
                        {
                            av += pValues[vi];
                        }
                        av /= (endIndex - startIndex);
                    }

                    int pixelIndex = pi * 3;
                    if (!hasRedMask)
                    {
                        pData[pixelIndex] = av;
                    }
                    else
                    {
                        // applying mask and summing.
                        float total = 0;
                        for (int vi = 0; vi < endIndex - startIndex; vi++)
                        {
                            total += pValues[startIndex + vi] * RedMask[vi];
                        }
                        pData[pixelIndex] = total;
                    }

                    pixelIndex += 1;
                    if (!hasGreenMask)
                    {
                        pData[pixelIndex] = av;
                    }
                    else
                    {
                        // applying mask and summing.
                        float total = 0;
                        for (int vi = 0; vi < endIndex - startIndex; vi++)
                        {
                            total += pValues[startIndex + vi] * GreenMask[vi];
                        }
                        pData[pixelIndex] = total;
                    }

                    pixelIndex += 1;
                    if (!hasBlueMask)
                    {
                        pData[pixelIndex] = av;
                    }
                    else
                    {
                        // applying mask and summing.
                        float total = 0;
                        for (int vi = 0; vi < endIndex - startIndex; vi++)
                        {
                            total += pValues[startIndex + vi] * BlueMask[vi];
                        }
                        pData[pixelIndex] = total;
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Close()
        {
            if (Processor != null)
                Processor.Close();
            if (Preview != null)
                Preview.Close();
        }

        public void Dispose()
        {
            if (Processor != null)
                Processor.Dispose();
            if (Preview != null)
                Preview.Dispose();
        }

        #endregion
    }

    public class SperctrumGeneratorBlockDoneEventArgs
    {
        public SperctrumGeneratorBlockDoneEventArgs(int total, int done, int thisLeg, SperctrumGeneratorBlockDoneMode mode)
        {
            Total = total;
            Done = done;
            DoneInThisLeg = thisLeg;
            CurrentMode = mode;
        }

        public int Total { get; private set; }
        public int Done { get; private set; }
        public int DoneInThisLeg { get; private set; }
        public SperctrumGeneratorBlockDoneMode CurrentMode { get; private set; }
    }

    public enum SperctrumGeneratorBlockDoneMode { CreatingSpectrum , CreatingPreview};
}