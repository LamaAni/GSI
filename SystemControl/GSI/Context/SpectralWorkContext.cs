using GSI.Camera;
using GSI.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Context
{
    /// <summary>
    /// Holds bindings for a specific work context. 
    /// Do not copy this context between work assignments and always call
    /// dispose after the work assignment has been compleated.
    /// <para>ALWAYS DISPOSE THIS OBJECT!</para>
    /// </summary>
    public class SpectralWorkContext : IDisposable
    {
        internal SpectralWorkContext(SpectralContext context)
        {
            Context = context;

            // bining events to the capturing mechanisem. 
            _positionEventHandler = new EventHandler<PositionRecivedEventArgs>(PositionReader_OnRecivedPosition);
            _imageEventHandler = new EventHandler<ImageRecivedEventArgs>(Camera_ImageCaptured);
            Context.Camera.ImageCaptured += _imageEventHandler;
            Context.PositionControl.OnRecivedPosition += _positionEventHandler;

            // creating the invocation queue. this queue will store in memory
            // new invocations if previus invocations have not compleated. 
            ImageInvokeQueue = new AsyncPendingEventQueue<ImageRecivedEventArgs>(
                (ev) =>
                {
                    if (OnImageCaptured != null)
                        OnImageCaptured(this, ev);
                }, false);
            PositionInvokeQueue = new AsyncPendingEventQueue<PositionRecivedEventArgs>((ev) =>
                {
                    if (OnPositionCapture != null)
                        OnPositionCapture(this, ev);
                }, false);
        }

        ~SpectralWorkContext()
        {
            try
            {
                Dispose();
            }
            catch { }
        }

        #region members

        EventHandler<PositionRecivedEventArgs> _positionEventHandler;
        EventHandler<ImageRecivedEventArgs> _imageEventHandler;

        /// <summary>
        /// The spctral context associated with work context. (Created it).
        /// </summary>
        public SpectralContext Context { get; private set; }

        /// <summary>
        /// If true all events will be called asyncronically to the capturing processes.
        /// </summary>
        public bool Async { get; private set; }

        /// <summary>
        /// Called when a position is captured (with time).Use CurrentTimeInSeconds for timing.
        /// The position pumping will be done async if Async=true. (Recommended).
        /// </summary>
        public event EventHandler<PositionRecivedEventArgs> OnPositionCapture;

        /// <summary>
        /// Called when an image is captured. Use CurrentTimeInSeconds for timing.
        /// The capture pumping will be done async if Async=true. (Recommended).
        /// </summary>
        public event EventHandler<ImageRecivedEventArgs> OnImageCaptured;

        /// <summary>
        /// The position reader.
        /// </summary>
        public IPositionControl PositionControl { get { return Context.PositionControl; } }

        /// <summary>
        /// The camera.
        /// </summary>
        public ICamera Camera { get { return Context.Camera; } }

        /// <summary>
        /// If true, recording data.
        /// </summary>
        public bool IsRecodringData { get { return Camera.IsCapturing; } }

        /// <summary>
        /// If true processing pending images.
        /// </summary>
        public bool IsProcessingPendingImages { get { return this.ImageInvokeQueue.IsInvoking; } }

        /// <summary>
        /// The number of pending image events in the queue
        /// </summary>
        public int PendingImageCount { get { return ImageInvokeQueue.PendingEventCount; } }

        /// <summary>
        /// If ture processing pending positions.
        /// </summary>
        public bool IsProcessingPendingPositions { get { return PositionInvokeQueue.IsInvoking; } }

        /// <summary>
        /// The number of pending position events in the queue
        /// </summary>
        public int PendingPositionCount { get { return PositionInvokeQueue.PendingEventCount; } }

        /// <summary>
        /// The position invoke queue.
        /// </summary>
        public AsyncPendingEventQueue<PositionRecivedEventArgs> PositionInvokeQueue { get; private set; }

        /// <summary>
        /// The image invoke queue.
        /// </summary>
        public AsyncPendingEventQueue<ImageRecivedEventArgs> ImageInvokeQueue { get; private set; }

        #endregion

        #region methods

        void PositionReader_OnRecivedPosition(object sender, PositionRecivedEventArgs e)
        {
            if (HasBeenDisposed)
                throw new Exception("Reached disposed work context (position event)");
            PositionInvokeQueue.PushEvent(e);
        }

        void Camera_ImageCaptured(object sender, ImageRecivedEventArgs e)
        {
            if (HasBeenDisposed)
                throw new Exception("Reached disposed work context (Image event)");
            ImageInvokeQueue.PushEvent(e);
        }

        /// <summary>
        /// Starts the image capturing (for the camera, not the context).
        /// </summary>
        /// <param name="imagefile"></param>
        /// <param name="positionsFile"></param>
        public void StartCapture()
        {
            Context.StartCapture();
        }

        /// <summary>
        /// Stops the image capturing (for the camera, not the context).
        /// </summary>
        public void StopCapture()
        {
            Context.StopCapture();
        }

        #endregion

        #region IDisposable Members
        
        /// <summary>
        /// IF true has been disposed.
        /// </summary>
        public bool HasBeenDisposed { get; private set; }

        public void Dispose()
        {
            PositionControl.OnRecivedPosition -= _positionEventHandler;
            Camera.ImageCaptured -= _imageEventHandler;
            HasBeenDisposed = true;
        }

        #endregion
    }
}
