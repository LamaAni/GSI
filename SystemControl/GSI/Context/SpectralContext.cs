using GSI.Camera;
using GSI.Stage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Context
{
    /// <summary>
    /// A context element that handles the measurement of the spectral image.
    /// And the correlation between the diffrent system elements.
    /// </summary>
    public class SpectralContext : IDisposable
    {
        /// <summary>
        /// Construct a spectral context.
        /// </summary>
        /// <param name="posControl"></param>
        public SpectralContext(IPositionControl posControl, ICamera camera)
        {
            Camera = camera;
            PositionControl = posControl;
        }

        #region members

        /// <summary>
        /// The position reader.
        /// </summary>
        public IPositionControl PositionControl { get; private set; }

        /// <summary>
        /// The camera.
        /// </summary>
        public ICamera Camera { get; private set; }

        #endregion

        #region Data collection

        /// <summary>
        /// Creates a work context for the spectral context. Remember to dispose this object!.
        /// </summary>
        /// <returns></returns>
        public SpectralWorkContext CreateWorkContext()
        {
            return new SpectralWorkContext(this);
        }

        /// <summary>
        /// If true, recording data.
        /// </summary>
        public bool IsRecodringData { get { return Camera.IsCapturing; } }

        /// <summary>
        /// Starts the data recoding info the specified data files.
        /// </summary>
        /// <param name="imagefile"></param>
        /// <param name="positionsFile"></param>
        public void StartCapture()
        {
            Camera.StartCapture();
        }

        /// <summary>
        /// Stops the data recording.
        /// </summary>
        public void StopCapture()
        {
            Camera.StopCapture();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
