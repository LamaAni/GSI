using GSI.Coading;
using GSI.IP;
using GSI.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Storage.Spectrum
{
    /// <summary>
    /// Implements the FFT writer that will create an fft written file.
    /// </summary>
    public class SpectrumStreamWriter : SpectrumStreamWorker
    {
        /// <summary>
        /// Create an fft writer.
        /// </summary>
        /// <param name="to">The stream to write to.</param>
        /// <param name="settings">The spectrum settings.</param>
        /// <param name="doAsyncStorage">The storage is executed in an asynchronius mannger, in a seperate process.</param>
        public SpectrumStreamWriter(Stream to, SpectrumStreamSettings settings, bool doAsyncStorage = true)
            :base(to,settings)
        {
            DataStartPosition = -1;
            IsAsyncStorage = doAsyncStorage;
            WriteExecuter = new ActionQueueExecuter();
            WriteExecuter.OverflowCount = 10;

            //if (prevMaker == null)
            //{
            //    int skipLength = Settings.FftDataSize / 15;
            //    prevMaker = (writer, count, vectors, preview) =>
            //    {
            //        Parallel.For(0, count, (vidx) =>
            //        {
            //            float av = 0;
            //            for (int pidx = 0; pidx < Settings.VectorSize; pidx++)
            //            {
            //                // internal position offset.
            //                int vecOffset = vidx * Settings.VectorSize * Settings.FftDataSize +
            //                    pidx * Settings.FftDataSize;

            //                for (int i = skipLength; i < Settings.FftDataSize; i++)
            //                    av += vectors[vecOffset + i];
            //                av = av / (Settings.FftDataSize - skipLength);

            //                int prevOffset = vidx * Settings.VectorSize + pidx;

            //                preview[prevOffset, 0] = av;
            //                preview[prevOffset, 1] = av;
            //                preview[prevOffset, 2] = av;
            //            }
            //        });
            //    };
            //}

        }

        #region members

        /// <summary>
        /// The spectruem write procedures to be called while processing the spectrum vectors.
        /// </summary>
        public List<ISpectrumStreamProcedure> WriteProcedures { get; private set; }

        /// <summary>
        /// If true the current has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// If true then use async storage (requires more memory).
        /// </summary>
        public bool IsAsyncStorage { get; private set; }

        /// <summary>
        /// The write executing thread.
        /// </summary>
        public ActionQueueExecuter WriteExecuter { get; private set; }

        #endregion

        #region initialization

        /// <summary>
        /// Initializes the writer.
        /// </summary>
        public void Initialize()
        {
            if (IsInitialized)
                return;

            IsInitialized = true;

            // base definitions.
            WriteHeader();

            // write settings.
            WriteJsonSerializable(Settings);
            Flush();

            // Adding basic data.
            Seek(0, SeekOrigin.End);
            DataStartPosition = Position;
            WriteHeader();
            Flush();

            // marking the info.
            ValidatePrealocated();

            // flush all changes.
            Flush();
        }


        #endregion

        #region data writings

        /// <summary>
        /// Waits for all cations to compleate.
        /// </summary>
        public void WaitForAllWriteActionsToCompleate()
        {
            WriteExecuter.WaitForAllActionsToCompleate();
        }

        /// <summary>
        /// Writes the vectors to the stream. 
        /// </summary>
        /// <param name="count">The number of vectors to write.</param>
        /// <param name="vectors">The vectors</param>
        /// <param name="avarage">The avarage data values for each pixel of the vector.</param>
        public void WriteVectors(int startIndex, int count, float[] vectors)
        {
            // checking async storage.
            if (IsAsyncStorage)
            {
                float[] vecCopy = new float[vectors.Length];
                Buffer.BlockCopy(vectors, 0, vecCopy, 0, vectors.Length * Settings.NumberOfPrecisionBytes);
                WriteExecuter.AddAction(() =>
                {
                    StoreVectors(startIndex, count, vecCopy);
                });

                WriteExecuter.WaitForOverflow();
            }
            else
            {
                StoreVectors(startIndex, count, vectors);
            }
        }

        /// <summary>
        /// Internal execute of the storage.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="vectors"></param>
        void StoreVectors(int startIndex, int count, float[] vectors)
        {
            // getting the vectors.
            //byte[] vec_bytes = new byte[vectors.Length * Settings.NumberOfPrecisionBytes];
            //Buffer.BlockCopy(vectors, 0, vec_bytes, 0, vec_bytes.Length);

            // writing the data vectors with seek.
            int vecLengthInBytes =
                Settings.NumberOfPrecisionBytes * Settings.VectorSize * Settings.FftDataSize;

            byte[] vec_bytes = new byte[vecLengthInBytes];
            float[] vec_vals=new float[vecLengthInBytes/sizeof(float)];
            for (int i = 0; i < count; i++)
            {
                int x, y;
                long offset = GetPixelFromVectorIndex(startIndex + i, out x, out y);
                SeekToPixelPosition(offset);

                // making the data.
                int vecOffset = i * vecLengthInBytes;
                Buffer.BlockCopy(vectors,vecOffset,vec_vals,0,vec_bytes.Length);
                Buffer.BlockCopy(vec_vals,0,vec_bytes,0,vec_bytes.Length);

                BaseStream.Write(vec_bytes, 0, vecLengthInBytes);
                Flush();

                // calling the workers. 
                if (WriteProcedures != null && WriteProcedures.Count > 0)
                    WriteProcedures.ForEach(p => p.ProcessVectorData(this, x, y, Settings.VectorSize, vec_vals));
            }

            //float[,] preview = new float[count * Settings.VectorSize, NumberOfColorValuesInPreviewPixel];
            //PreviewFromVectors(this, count, vectors, preview);
            //float[] prevVals = new float[preview.Length]; // total preview size.
            
            //Buffer.BlockCopy(preview, 0, prevVals, 0,
            //    (int)(prevVals.Length * ImageStream.NumberOfBytesPerPixelValue));

            //// writing the preview data with seek.
            //float[] prevVec = new float[(int)(Settings.VectorSize * ImageStream.NumberOfColorValuesInPixel)];
            //int numberOfBytesPerVector = (int)(prevVec.Length * ImageStream.NumberOfBytesPerPixelValue);
            
            //for (int i = 0; i < count; i++)
            //{
            //    int x, y;
            //    GetNPixelOffsetFromVectorIndex(VectorsWritten + i, out x, out y);

            //    int prevValsOffset = i * numberOfBytesPerVector;

            //    // copy to the store vector data.
            //    Buffer.BlockCopy(prevVals, prevValsOffset, prevVec, 0, numberOfBytesPerVector);

            //    Preview.SetImageData(prevVec, x, y, Settings.VectorSize, 1);
            //    Preview.Flush();
            //}
        }

        #endregion
    }
}
