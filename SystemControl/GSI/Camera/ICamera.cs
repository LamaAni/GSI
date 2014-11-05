using GSI.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Camera
{
    /// <summary>
    /// Implements an interface that controls the camera. 
    /// </summary>
    public interface ICamera : ITimeKeeper
    {
        /// <summary>
        /// Called when the capture has started.
        /// </summary>
        event EventHandler OnStartCapture;

        /// <summary>
        /// Called when the capture has ended.
        /// </summary>
        event EventHandler OnEndCapture;

        /// <summary>
        /// Called when an image is recived.
        /// </summary>
        event EventHandler<ImageRecivedEventArgs> ImageCaptured;

        /// <summary>
        /// Called when a preview image has been recived.
        /// </summary>
        event EventHandler<ImageRecivedEventArgs> PreviewImageRecived;

        /// <summary>
        /// Called after the settings have changed.
        /// </summary>
        event EventHandler SettingsChanged;

        /// <summary>
        /// Returns the camera timer resolution in seconds.
        /// </summary>
        double TimerResolution { get; }
        
        /// <summary>
        /// The current time in the camera clock.
        /// </summary>
        DateTime CurTime { get; }

        /// <summary>
        /// The current time in the camera clock.
        /// </summary>
        DateTime ZeroTime { get; }

        /// <summary>
        /// The width of the image.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The height of the image;
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Starts the data capture.
        /// </summary>
        /// <returns></returns>
        void StartCapture();

        /// <summary>
        /// Ends the data captrue.
        /// </summary>
        /// <returns></returns>
        void StopCapture();

        /// <summary>
        /// True if the camera is capturing data.
        /// </summary>
        bool IsCapturing { get; }

        /// <summary>
        /// Converts the byte data into an image object.
        /// </summary>
        /// <returns></returns>
        System.Drawing.Image ToImage(byte[] data, int width, int height);

        /// <summary>
        /// Number of frames per seconds read.
        /// </summary>
        double ActualFrameRate { get; }
    }
}
