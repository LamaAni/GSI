using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Storage.Spectrum
{
    /// <summary>
    /// Called to process the vector data, given the x,y positions of the worker in the stream.
    /// </summary>
    public interface ISpectrumStreamProcedure
    {
        /// <summary>
        /// Called to process the vector data.
        /// </summary>
        /// <param name="worker">The worker</param>
        /// <param name="x">The x position (in pixels) of the start location of the data.</param>
        /// <param name="y">The y position (in pixels) of the start location of the data.</param>
        /// <param name="length">The total number of pixels in the data.</param>
        /// <param name="vectorData">The data</param>
        void ProcessVectorData(SpectrumStreamWorker worker, int x, int y, int length, float[] vectorData);
    }
}
