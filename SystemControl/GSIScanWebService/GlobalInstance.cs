using GSI.Camera.LumeneraControl;
using GSI.Stage.Piror;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace GSIScanWebService
{
    public class GlobalInstance : IDisposable
    {
        public GlobalInstance()
        {
            Status = new InstanceStatus();
        }

        static GlobalInstance()
        {
            Current = new GlobalInstance();
            Current.Initialize();
        }

        ~GlobalInstance()
        {
            Dispose();
        }

        public static GlobalInstance Current
        {
            get;
            private set;
        }

        public InstanceStatus Status { get; private set; }

        /// <summary>
        /// The last preview image recived.
        /// </summary>
        public byte[] LastPreviewImage { get; private set; }
        public DateTime LastPreviewTimeStamp = DateTime.Now;

        public Lt255 Camera { get; private set; }
        public ProScan Stage { get; private set; }

        /// <summary>
        /// The spectral context.
        /// </summary>
        public GSI.Context.SpectralContext Context { get; private set; } 

        #region members

        #endregion

        #region Methods

        public void Initialize()
        {
            // attempting to find camera.
            CameraConnectionInfo info = LumeneraCamera.ScanForCameras().FirstOrDefault();
            if (info != null)
            {
                Camera = new Lt255(info);
                this.Status.IsCameraLoaded = true;

                Camera.PreviewImageRecived += (s, ev) =>
                {
                    SetLastPreviewData(ev.Data, ev.TimeStamp);
                    //StringBuilder sb = new StringBuilder();
                    //sb.Append(Stage.PositionX.ToString("0000000.0"));
                    //sb.Append(",");
                    //sb.Append(Stage.PositionY.ToString("0000000.0"));
                    //sb.Append(" (");
                    //sb.Append(Stage.AbsolutePositionX.ToString("0000000.0"));
                    //sb.Append(",");
                    //sb.Append(Stage.AbsolutePositionY.ToString("0000000.0"));
                    //sb.Append(")");
                };

                Camera.StartStreamVideo();
                Camera.Connect(File.Exists("cam_settings.json") ? File.ReadAllText("cam_settings.json") : null);
            }
            else
            {
                this.Status.IsCameraLoaded = false;
            }

            Status.IsStageLoaded = true;
            Stage = new ProScan("COM1");
            Stage.StartServer();


            if(Status.IsCameraLoaded && Status.IsStageLoaded)
            {
                Context = new GSI.Context.SpectralContext(Stage, Camera);
            }
        }

        void SetLastPreviewData(byte[] data, DateTime stamp)
        {
            LastPreviewImage = data;
            LastPreviewTimeStamp = stamp;
        }

        #endregion

        public void Dispose()
        {
            if (Camera == null)
                return;
            Camera.StopPreview();
            Camera.StopCapture();
            Stage.StopServer();
            Camera = null; Stage = null;
        }
    }

    public class InstanceStatus
    {
        public bool IsCameraLoaded { get; internal set; }
        public bool IsStageLoaded { get; internal set; }
    }
}