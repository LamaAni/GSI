﻿using GSI.Timing;
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
        /// Called when an image is received.
        /// </summary>
        event EventHandler<ImagereceivedEventArgs> ImageCaptured;

        /// <summary>
        /// Called when a preview image has been received.
        /// </summary>
        event EventHandler<ImagereceivedEventArgs> PreviewImagereceived;

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
        /// Strart stop video streaming.
        /// </summary>
        /// <param name="enabled">If true then start the streaming</param>
        void Stream(bool enabled = true);

        /// <summary>
        /// Single shot camera capture.
        /// </summary>
        void Capture();

        /// <summary>
        /// Starts the continues data capture.
        /// </summary>
        /// <returns></returns>
        void StartCapture(int max_images = -1);

        /// <summary>
        /// Ends the continues data captrue.
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
