using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumenera.USB;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;
using GSI.JSON;

namespace GSI.Camera.LumeneraControl
{
    [DataContract(Name = "CameraSettings")]
    /// <summary>
    /// A collection of settings that may be applied to the lumenera camera.
    /// </summary>
    public class CameraSettings : IJsonObject
    {
        internal CameraSettings(dll.LucamFrameFormat underliningFormat,
            double frameRate, double gain, double exposure, bool autoUpdate = true)
        {
            this.AutoUpdate = false;
            Gain = gain; Exposure = exposure;
            FrameRate = frameRate;
            UnderliningFormat = underliningFormat;
            m_frameRate = frameRate;
            this.AutoUpdate = autoUpdate;
        }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        protected CameraSettings()
        {
        }

        #region underline members

        /// <summary>
        /// The underlining lucam format struct that is passed to the camera. 
        /// <para>DO NOT Handle this object directly. For internal use only.</para>
        /// </summary>
        private dll.LucamFrameFormat UnderliningFormat;

        /// <summary>
        /// Returns a copy of the underlining format.
        /// </summary>
        public dll.LucamFrameFormat GetUnderliningFormat()
        {
            dll.LucamFrameFormat copy = UnderliningFormat;
            return copy;
        }

        #endregion

        #region members

        [DataMember]
        /// <summary>
        /// Allows to make a set of commands before update if needed.
        /// </summary>
        public bool InHoldUpdateMode { get; private set; }

        [DataMember]
        /// <summary>
        /// If true, then the settings will auto update when a value has changed.
        /// <remarks>Pointer classed, marked with "Pointer" will update every time the pointer is updated. 
        /// Example: this.Location=this.Location</remarks>
        /// </summary>
        public bool AutoUpdate { get; set; }

        [DataMember]
        internal double m_frameRate = 100;

        /// <summary>
        /// The frame rate for the camera.
        /// </summary>
        public double FrameRate
        {
            get
            {
                return m_frameRate;
            }
            set
            {
                if (m_frameRate != value)
                {
                    m_frameRate = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        internal double m_exposure = 1;

        /// <summary>
        /// The gain for the camera.
        /// </summary>
        public double Exposure
        {
            get
            {
                return m_exposure;
            }
            set
            {
                if (m_exposure != value)
                {
                    m_exposure = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        internal double m_gain = 1;

        /// <summary>
        /// The gain for the camera.
        /// </summary>
        public double Gain
        {
            get
            {
                return m_gain;
            }
            set
            {
                if (m_gain != value)
                {
                    m_gain = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// Column decimation ratio.
        /// </summary>
        public short BinningX
        {
            get
            {
                return UnderliningFormat.BinningX;
            }
            set
            {
                if (BinningX != value)
                {
                    UnderliningFormat.BinningX = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// Row decimation ratio.
        /// </summary>
        public short BinningY
        {
            get
            {
                return UnderliningFormat.BinningY;
            }
            set
            {
                if (BinningY != value)
                {
                    UnderliningFormat.BinningY = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// Column decimation control flags.
        /// </summary>
        public dll.LucamFrameFormatFlag FlagsX
        {
            get
            {
                return UnderliningFormat.FlagsX;
            }
            set
            {
                if (FlagsX != value)
                {
                    UnderliningFormat.FlagsX = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        ///  Row decimation control flags.
        /// </summary>
        public dll.LucamFrameFormatFlag FlagsY
        {
            get
            {
                return UnderliningFormat.FlagsX;
            }
            set
            {
                if (FlagsY != value)
                {
                    UnderliningFormat.FlagsY = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// The width of the image data to be read out of the imager.
        /// </summary>
        public int Width
        {
            get
            {
                return UnderliningFormat.Width;
            }
            set
            {
                if (Width != value)
                {
                    UnderliningFormat.Width = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// The height of the image data to be read out of the imager.
        /// </summary>
        public int Height
        {
            get
            {
                return UnderliningFormat.Height;
            }
            set
            {
                if (Height != value)
                {
                    UnderliningFormat.Height = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// The pixel format of the data read out of the imager.
        /// </summary>
        public dll.LucamPixelFormat PixelFormat
        {
            get
            {
                return UnderliningFormat.PixelFormat;
            }
            set
            {
                if (PixelFormat != value)
                {
                    UnderliningFormat.PixelFormat = value;
                    CallChangedIfNeeded();
                }
            }
        }

        /// <summary>
        /// The dimensions of the image data to be read out of the imager.  Can do this
        /// because Size is a struct.
        /// </summary>
        public Lumenera.Geometry.Size Size
        {
            get
            {
                return UnderliningFormat.Size;
            }
            set
            {
                if (Size != value)
                {
                    UnderliningFormat.Size = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// Column decimation ratio.
        /// </summary>
        public short SubSampleX
        {
            get
            {
                return UnderliningFormat.SubSampleX;
            }
            set
            {
                if (SubSampleX != value)
                {
                    UnderliningFormat.SubSampleX = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// Row decimation ratio.
        /// </summary>
        public short SubSampleY
        {
            get
            {
                return UnderliningFormat.SubSampleY;
            }
            set
            {
                if (SubSampleY != value)
                {
                    UnderliningFormat.SubSampleY = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// The X offset of the image data to be read out of the imager.
        /// </summary>
        public int X
        {
            get
            {
                return UnderliningFormat.X;
            }
            set
            {
                if (X != value)
                {
                    UnderliningFormat.X = value;
                    CallChangedIfNeeded();
                }
            }
        }

        [DataMember]
        /// <summary>
        /// The Y offset of the image data to be read out of the imager.
        /// </summary>
        public int Y
        {
            get
            {
                return UnderliningFormat.Y;
            }
            set
            {
                if (Y != value)
                {
                    UnderliningFormat.Y = value;
                    CallChangedIfNeeded();
                }
            }
        }

        /// <summary>
        /// The offset of the image data to be read out of the imager.
        /// <remarks>Pointer</remarks>
        /// </summary>
        public Lumenera.Geometry.Point Location
        {
            get
            {
                return UnderliningFormat.Location;
            }
            set
            {
                UnderliningFormat.Location = value;
                CallChangedIfNeeded();
            }
        }

        /// <summary>
        /// The location and size of the image data to be read out of the imager.
        /// <remarks>Pointer</remarks>
        /// </summary>
        public Lumenera.Geometry.Rectangle ROI
        {
            get
            {
                return UnderliningFormat.ROI;
            }
            set
            {
                UnderliningFormat.ROI = value;
                CallChangedIfNeeded();
            }
        }

        #endregion

        #region updating

        /// <summary>
        /// Marks the hold update.
        /// </summary>
        public void HoldUpdate()
        {
            InHoldUpdateMode = true;
        }

        /// <summary>
        /// Resumes the update if needed.
        /// </summary>
        public void ResumeUpdate()
        {
            InHoldUpdateMode = false;
            CallChangedIfNeeded();
        }

        /// <summary>
        /// Calls the changed event. All cameras bound to this object will be updated.
        /// </summary>
        public bool InvokeChanged()
        {
            if (SettingsChanged == null)
                return false;
            SettingsChanged(this, new SettingsChangedEventArgs(GetUnderliningFormat()));
            return true;
        }

        void CallChangedIfNeeded()
        {
            if (AutoUpdate && !InHoldUpdateMode)
                this.InvokeChanged();
        }

        /// <summary>
        /// Called when settings are changed.
        /// </summary>
        public event EventHandler<SettingsChangedEventArgs> SettingsChanged;

        #endregion

        #region Json serialization

        public static CameraSettings FromJson(string json)
        {
            return json.FromJson<CameraSettings>();
        }

        #endregion
    }

    /// <summary>
    /// Event args for Lumenera camera settings changed.
    /// </summary>
    public class SettingsChangedEventArgs : EventArgs
    {
        public SettingsChangedEventArgs(dll.LucamFrameFormat format)
        {
            Format = format;
        }

        /// <summary>
        /// The format of the change.
        /// </summary>
        public dll.LucamFrameFormat Format { get; private set; }
    }
}
