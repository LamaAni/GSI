﻿using Lumenera.USB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Camera.LumeneraControl
{
    public abstract class LumeneraCameraOld : ICamera
    {
        #region static methods

        /// <summary>
        /// Returns a list of connected camera devices (string url),
        /// and connection types.
        /// </summary>
        /// <returns></returns>
        public static CameraConnectionInfo[] ScanForCameras(bool useGigEInterface = false)
        {
            dll.LucamVersion[] cameras = api.EnumCameras();
            List<CameraConnectionInfo> infos = new List<CameraConnectionInfo>();
            for (int i = 0; i < cameras.Length; i++)
            {
                infos.Add(new CameraConnectionInfo(i + 1, cameras[i], useGigEInterface));
            }
            return infos.ToArray();
        }


        #endregion

        public LumeneraCamera(CameraConnectionInfo info)
        {
            Info = info;
            PreviewDelayInMs = 30;
            _internalWatch.Start();
            DefaultFrameRate = 10;
        }

        #region members

        /// <summary>
        /// The width of the image.
        /// </summary>
        public int Width { get { return Settings.Width; } }

        /// <summary>
        /// The height of the image;
        /// </summary>
        public int Height { get { return Settings.Height; } }

        /// <summary>
        /// The camera info.
        /// </summary>
        public CameraConnectionInfo Info { get; private set; }

        /// <summary>
        /// The camera handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// The color format associated with the camera.
        /// </summary>
        public dll.LucamColorFormat ColorFormat { get; private set; }

        /// <summary>
        /// Camera format is Little endian.
        /// </summary>
        public bool IsLittleEndian { get; private set; }

        /// <summary>
        /// Retun true if the current camera is conencted.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// The total number of images received from the callback.
        /// </summary>
        public long TotalNumerOfImages { get; private set; }

        /// <summary>
        /// The total number of captrued images received.
        /// </summary>
        public long TotalNumberOfCapturedImages { get; private set; }

        /// <summary>
        /// A collection of settings that may be applied to the lumenera camera.
        /// </summary>
        public CameraSettings Settings { get; private set; }

        /// <summary>
        /// If true, then the camera controls the timestamps and sends them to
        /// the computer. (In the first two bytes of the data, which is internally parsed).
        /// </summary>
        public bool CameraTimestamps { get; private set; }

        /// <summary>
        /// Assumed clock freqeuency. TODO: add actual frequency.
        /// </summary>
        public double CameraClockFrequency { get { return 256 * Settings.FrameRate; } }

        /// <summary>
        /// The time elapsed (on camera clock) from the initial start stream.
        /// </summary>
        public TimeSpan CameraElapsed
        {
            get { return TimeSpan.FromSeconds(cameraClock * 1.0 / CameraClockFrequency); }
        }

        /// <summary>
        /// The default frame rate.
        /// </summary>
        public static int DefaultFrameRate { get; set; }

        /// <summary>
        /// The current time in the camera clock.
        /// </summary>
        public DateTime CurTime { get { return _CameraComputerClock + CameraElapsed; } }


        /// <summary>
        /// The current time in the camera clock.
        /// </summary>
        public DateTime ZeroTime { get { return _CameraComputerClock; } }

        #endregion

        #region general methods

        /// <summary>
        /// Connect to the camera.
        /// </summary>
        public void Connect(string jsonSettings = null)
        {
            if (IsConnected)
                throw new Exception("Already connected to camera.");

            // Getting the camera handle and info.
            Handle = api.CameraOpen(Info.Serial);

            api.SetFrameRate(Handle, DefaultFrameRate);

            Settings = jsonSettings == null ? new CameraSettings(api.GetFormat(Handle),
                api.GetFrameRate(Handle), api.GetProperty(Handle, dll.LucamProperty.GAIN),
                api.GetProperty(Handle, dll.LucamProperty.EXPOSURE)) :
                CameraSettings.FromJson(jsonSettings);

            Action applySettings = () =>
            {
                api.SetProperty(Handle, dll.LucamProperty.EXPOSURE,
                    Settings.Exposure);
                api.SetProperty(Handle, dll.LucamProperty.GAIN,
                    Settings.Gain);
                api.SetFrameRate(Handle, Settings.FrameRate);
                api.SetFormat(Handle, Settings.GetUnderliningFormat(), Settings.FrameRate);

                Settings.m_exposure = api.GetProperty(Handle, dll.LucamProperty.EXPOSURE);
                Settings.m_frameRate = api.GetFrameRate(Handle);
                Settings.m_gain = api.GetProperty(Handle, dll.LucamProperty.GAIN);
            };


            if (jsonSettings != null)
            {
                applySettings();
            }

            CameraTimestamps =
                dll.LucamSetProperty(Handle, dll.LucamProperty.TIMESTAMPS, 1, dll.LucamPropertyFlag.USE);

            // bind the settings object to the camera.
            Settings.SettingsChanged += (s, e) =>
                {
                    bool stoppedStream = false;
                    if (IsStreaming())
                    {
                        StopStreamVideo();
                        stoppedStream = true;
                    }

                    applySettings();

                    if (stoppedStream)
                        StartStreamVideo();

                    if (SettingsChanged != null)
                        SettingsChanged(this, null);
                };

            // adding the camera data callback, the context is the camera itself. received
            __data_received_callback = data_received_callback;
            api.AddStreamingCallback(Handle, __data_received_callback, Handle);

            // If monochrome invert vertical
            ColorFormat = (dll.LucamColorFormat)api.GetProperty(Handle, dll.LucamProperty.COLOR_FORMAT);
            if (ColorFormat == dll.LucamColorFormat.MONO)
            {
                api.SetProperty(Handle, dll.LucamProperty.FLIPPING, (float)dll.LucamFlippingFormat.Y);
            }

            IsLittleEndian = api.IsFormatLittleEndian(Handle);
            IsConnected = true;
        }

        /// <summary>
        /// Disconnects from the camera.
        /// </summary>
        public void Disconnect()
        {
            api.CameraClose(Handle);
        }

        /// <summary>
        /// Starts the video stream.
        /// </summary>
        public void StartStreamVideo()
        {
            if (IsStreaming())
                throw new Exception("Already streaming...");
            ResetAllClocks();
            api.StreamVideoStart(Handle);
        }

        /// <summary>
        /// True if streaming.
        /// </summary>
        /// <returns></returns>
        public bool IsStreaming()
        {
            return api.IsStreaming(Handle);
        }

        /// <summary>
        /// Stops the video stream.
        /// </summary>
        public void StopStreamVideo()
        {
            api.StreamVideoStop(Handle);
        }

        /// <summary>
        /// Sends the camera format (this.Format) to the camera.
        /// </summary>
        public void UpdateCameraFormat()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Image methods

        /// <summary>
        /// Converts the byte data into an image object.
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public Image ToImage(byte[] imageData)
        {
            return ToImage(imageData, this.Settings.Width, this.Settings.Height);
        }

        /// <summary>
        /// Converts the byte data into an image object.
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public abstract Image ToImage(byte[] imageData, int width, int height);

        /// <summary>
        /// Returns the avilable pixel formats.
        /// </summary>
        /// <returns></returns>
        public abstract dll.LucamPixelFormat[] GetAvilablePixelFormats();

        #endregion

        #region preview

        /// <summary>
        /// The preview default delay time to not overwhalem thread.
        /// </summary>
        public int PreviewDelayInMs { get; private set; }

        /// <summary>
        /// If true is previewing.
        /// </summary>
        public bool IsPreviewing { get; private set; }

        /// <summary>
        /// The current pending preview captured.
        /// </summary>
        Tuple<byte[], DateTime> _pendingPreview = null;

        public void StartPreview()
        {
            if (IsPreviewing)
                return;

            IsPreviewing = true;

            Task.Run(() =>
            {
                int waitIdle = PreviewDelayInMs > 100 ? PreviewDelayInMs : 100;
                while (IsPreviewing)
                {
                    if (_pendingPreview == null)
                    {
                        System.Threading.Thread.Sleep(waitIdle);
                        continue;
                    }
                    byte[] preview = _pendingPreview.Item1;
                    DateTime stamp = _pendingPreview.Item2;

                    preview = ValidateDataSize(preview);
                    if (PreviewImagereceived != null)
                    {
                        ImagereceivedEventArgs args =
                            new ImagereceivedEventArgs(Settings.Width, Settings.Height, preview,
                                Settings.PixelFormat, stamp, _CameraComputerClock);
                        PreviewImagereceived(this, args);
                    }
                    _pendingPreview = null;
                    System.Threading.Thread.Sleep(PreviewDelayInMs);
                }
            });
        }

        public void StopPreview()
        {
            IsPreviewing = false;
        }

        #endregion

        #region Capture

        Queue<Tuple<byte[], DateTime>> PendingCaptures =
            new Queue<Tuple<byte[], DateTime>>();

        //Queue<System.Threading.Tasks.TaskCompletionSource<byte[]>> PendingImageWaits = new Queue<TaskCompletionSource<byte[]>>();
        Queue<System.Threading.ManualResetEvent> PendingImageWaits = new Queue<System.Threading.ManualResetEvent>();

        /// <summary>
        /// Initializes the capture process.
        /// </summary>
        protected void InitCapture()
        {
            if (IsCapturing)
                return;
            IsCapturing = true;
            if (IsProcessingPendingImages)
                return;
            IsProcessingPendingImages = false;
            Task.Run(() =>
            {
                while (IsCapturing || PendingCaptures.Count > 0)
                {
                    if (this.cur_capture_images_count == 0)
                    {
                        this.cur_capture_images_count = -1;
                        this.StopCapture();
                        break;
                    }

                    if (PendingCaptures.Count == 0)
                    {
                        IsProcessingPendingImages = false;
                        System.Threading.Thread.Sleep(10);
                        continue;
                    }

                    IsProcessingPendingImages = true;
                    Tuple<byte[], DateTime> capture = PendingCaptures.Dequeue();

                    if (capture.Item2 < StartCaptureTS)
                        continue;

                    byte[] data = ValidateDataSize(capture.Item1);


                    if (this.cur_capture_images_count > -1)
                    {
                        this.cur_capture_images_count -= 1;
                    }

                    if (ImageCaptured != null)
                    {
                        ImagereceivedEventArgs args = new ImagereceivedEventArgs(
                            Settings.Width, Settings.Height, data, Settings.PixelFormat, capture.Item2,
                            _CameraComputerClock);

                        ImageCaptured(this, args);
                    }

                    while (PendingImageWaits.Count > 0)
                    {
                        PendingImageWaits.Dequeue().Set();
                    }
                }
                IsProcessingPendingImages = false;
            });
        }

        protected System.Threading.ManualResetEvent GetImageWaitHandle()
        {
            System.Threading.ManualResetEvent wfi = new System.Threading.ManualResetEvent(false);
            PendingImageWaits.Enqueue(wfi);
            return wfi;
        }

        int _captureCount;

        protected void PostCapture(Tuple<byte[], DateTime> image)
        {
            _captureCount++;
            PendingCaptures.Enqueue(image);
        }

        #endregion

        #region data methods

        Stopwatch _internalWatch = new Stopwatch();
        DateTime _CameraComputerClock = DateTime.MinValue;
        long cameraClock = 0;
        ushort lastTimestampRead = 0;

        int cur_capture_images_count = -1;

        /// <summary>
        /// Resets the camera clock value, should be noted by the 
        /// </summary>
        void ResetAllClocks()
        {
            lastTimestampRead = 0;
            SetZeroTime();
        }

        /// <summary>
        /// This function is called to sync betwen the camera clock to the computer clock.
        /// </summary>
        public void SetZeroTime()
        {
            cameraClock = 0;
            _CameraComputerClock = DateTime.Now;
        }

        protected abstract byte[] ValidateDataSize(byte[] data);

        dll.LucamStreamingCallback __data_received_callback;

        protected byte[] GetDataFromPointer(int n, IntPtr pData, out ushort timestamp)
        {
            byte[] image = new byte[n];
            System.Runtime.InteropServices.Marshal.Copy(pData, image, 0, n);
            timestamp = BitConverter.ToUInt16(image, 0);
            return image;
        }

        void data_received_callback(IntPtr context, IntPtr pData, int n)
        {
            bool hasPreview = IsPreviewing && _pendingPreview == null;
            ushort timestamp;
            byte[] data = null;
            data = GetDataFromPointer(n, pData, out timestamp);

            // Circular condition on the timestamp value.
            int timestampActual = timestamp < lastTimestampRead ? timestamp + ushort.MaxValue :
                timestamp;

            // Time diffrent from last.
            ushort dtInRatio = (ushort)(timestampActual - lastTimestampRead);

            // adding the clock dt.
            cameraClock += dtInRatio;

            // calculating rates.
            lastTimestampRead = timestamp;
            ActualFrameRate = 1.0 / (dtInRatio * 1.0 / CameraClockFrequency);

            if (hasPreview || IsCapturing)
            {
                Tuple<byte[], DateTime> image =
                    new Tuple<byte[], DateTime>(data, _CameraComputerClock + CameraElapsed);
                if (hasPreview)
                {
                    _pendingPreview = image;
                }

                if (IsCapturing)
                {
                    PostCapture(image);
                }
            }
        }

        #endregion

        #region ICamera Members

        public int PendingImagesCount { get { return this.PendingCaptures.Count; } }

        public bool IsCapturing { get; private set; }

        public DateTime StartCaptureTS { get; private set; }

        /// <summary>
        /// If true, then processing captured images.
        /// </summary>
        public bool IsProcessingPendingImages { get; private set; }

        public event EventHandler<ImagereceivedEventArgs> ImageCaptured;

        public double TimerResolution
        {
            get { return 1.0 / Stopwatch.Frequency; }
        }

        public void StartCapture(int max_images = -1)
        {
            this.cur_capture_images_count = max_images;

            StartCaptureTS = DateTime.Now;

            if (IsCapturing)
                return;

            // no need to save anything.
            PendingCaptures.Clear();

            if (OnStartCapture != null)
                OnStartCapture(this, null);

            InitCapture();
        }

        public void Capture()
        {
            var started = DateTime.Now;
            var handle = this.GetImageWaitHandle();
            this.SetZeroTime();
            this.StartCapture(1);
            handle.WaitOne();
            var captureTimeMs = (DateTime.Now - started).TotalMilliseconds;
            this.StopCapture();
            this.PendingCaptures.Clear();
        }

        public void StopCapture()
        {
            IsCapturing = false;

            if (OnEndCapture != null)
                OnEndCapture(this, null);
        }

        public event EventHandler<ImagereceivedEventArgs> PreviewImagereceived;

        /// <summary>
        /// Called after the settings have changed.
        /// </summary>
        public event EventHandler SettingsChanged;

        public event EventHandler OnStartCapture;

        public event EventHandler OnEndCapture;

        /// <summary>
        /// Number of frames per seconds read.
        /// </summary>
        public double ActualFrameRate { get; private set; }

        #endregion

        #region settings

        #endregion
    }
}
