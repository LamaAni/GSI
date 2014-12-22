using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GSI.Coading;
using GSI.JSON;
using GSI.Context;

namespace GSI.Processing
{
    /// <summary>
    /// Comines the needed information to preform a single scan, and keeps all scan information valid.
    /// Can be written and read from file.
    /// </summary>
    [DataContract(Name = "ScanInfo")]
    public class ScanInfo : IJsonObject
    {
        public ScanInfo(double startX, double startY,
            double offsetX, double offsetY,
            double exposureTime, double pixelSize, double deltaX, double timeUnits,
            double spaceUnits)
        {
            StartX = startX;
            StartY = startY;
            ExposureTime = exposureTime;
            PixelSize = pixelSize;
            DeltaX = deltaX;
            OffsetX = offsetX;
            OffsetY = offsetY;
            NativeTimeUnitsToSeconds = timeUnits;
            NativeSpatialUnitsToMeters = spaceUnits;
            CalculateScanParams();
        }

        protected ScanInfo()
        {
            CalculateScanParams();
        }

        #region members

        [DataMember]
        /// <summary>
        /// The direction of the scan.
        /// </summary>
        public ScanInfo_ScanDirection Direction { get; set; }

        [DataMember]
        /// <summary>
        /// If true then speedup is nessesary.
        /// </summary>
        public bool DoSpeedup { get; set; }

        [DataMember]
        private double m_StageAngle;

        /// <summary>
        /// The stage angle in radian.
        /// </summary>
        public double StageAngle
        {
            get { return m_StageAngle; }
            set { m_StageAngle = value; }
        }

        [DataMember]
        private double m_StartX;
        /// <summary>
        /// The start position in native units.
        /// </summary>
        public double StartX
        {
            get { return m_StartX; }
            set { m_StartX = value; }
        }

        [DataMember]
        private double m_StartY;
        /// <summary>
        /// The size of the x offset in native units.
        /// </summary>
        public double StartY
        {
            get { return m_StartY; }
            set { m_StartY = value; }
        }

        [DataMember]
        private double m_OffsetX;
        /// <summary>
        /// The size of the x offset in native units.
        /// </summary>
        public double OffsetX
        {
            get { return m_OffsetX; }
            set { m_OffsetX = value; }
        }

        [DataMember]
        private double m_OffsetY;
        /// <summary>
        /// The size of the y scan offset in native units.
        /// </summary>
        public double OffsetY
        {
            get { return m_OffsetY; }
            set { m_OffsetY = value; }
        }


        [DataMember]
        private double m_PixelSize;
        /// <summary>
        /// The size of the pixel, in native units.
        /// </summary>
        public double PixelSize
        {
            get { return m_PixelSize; }
            set { m_PixelSize = value; CalculateScanParams(); }
        }

        [DataMember]
        private double m_ExposureTime;

        /// <summary>
        /// The time, in native time units, that it takes to take a single image.
        /// </summary>
        public double ExposureTime
        {
            get { return m_ExposureTime; }
            set { m_ExposureTime = value; CalculateScanParams(); }
        }

        [DataMember]
        private double m_NativeSpatialUnitsToMeters;

        /// <summary>
        /// The conversion ratio between native units to meters.
        /// </summary>
        public double NativeSpatialUnitsToMeters
        {
            get { return m_NativeSpatialUnitsToMeters; }
            set { m_NativeSpatialUnitsToMeters = value; CalculateScanParams(); }
        }

        [DataMember]
        private double m_NativeTimeUnitsToSeconds;

        /// <summary>
        /// Convertsion between native time units to seconds.
        /// </summary>
        public double NativeTimeUnitsToSeconds
        {
            get { return m_NativeTimeUnitsToSeconds; }
            set { m_NativeTimeUnitsToSeconds = value; CalculateScanParams(); }
        }


        [DataMember]
        private double m_DeltaX;

        /// <summary>
        /// The delta x between images in native units.
        /// </summary>
        public double DeltaX
        {
            get { return m_DeltaX; }
            set { m_DeltaX = value; CalculateScanParams(); }
        }

        [DataMember]
        private double m_MaxFrameRate = -1;

        /// <summary>
        /// The maximal frame rate allowed. If less then 0 = infty.
        /// </summary>
        public double MaxFrameRate
        {
            get { return m_MaxFrameRate; }
            set { m_MaxFrameRate = value; CalculateScanParams(); }
        }

        [DataMember]
        private double m_MaxScanSpeed;

        /// <summary>
        /// The maximal scan speed in native units.
        /// </summary>
        public double MaxScanSpeed
        {
            get { return m_MaxScanSpeed; }
            set { m_MaxScanSpeed = value; }
        }

        #endregion

        #region Calculated members

        /// <summary>
        /// The frame rate for the scan. (Hz)
        /// </summary>
        public double FrameRate { get; private set; }

        /// <summary>
        /// The scan speed, calculated by other parameters. native units.
        /// </summary>
        public double ScanSpeed { get; private set; }

        /// <summary>
        /// The delta between images in pixels.
        /// </summary>
        public double DeltaXInPixels { get; private set; }

        #endregion

        #region validation of scan parameters

        /// <summary>
        /// Calculates the canning parameters associated with the scan.
        /// </summary>
        public void CalculateScanParams()
        {
            // Updating parameters.
            DeltaXInPixels = DeltaX / PixelSize;

            // finding the fastest possible speed.
            double maxSpeedExpsure = Math.Floor(0.25 * PixelSize / (ExposureTime * NativeTimeUnitsToSeconds));
            double maxSpeedFrameRate = Math.Floor(DeltaX * MaxFrameRate);
            double maxCalculatedSpeed = maxSpeedExpsure < maxSpeedFrameRate ? maxSpeedExpsure : maxSpeedFrameRate;

            // updating the scan speed.
            ScanSpeed = MaxScanSpeed > 0 && MaxScanSpeed < maxCalculatedSpeed ? MaxScanSpeed : maxCalculatedSpeed;
            // updating the frame rate.
            FrameRate = ScanSpeed / DeltaX; 
        }

        #endregion

        #region string info

        /// <summary>
        /// Returns the json representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToJson();
        }

        /// <summary>
        /// Returns the object from the json.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ScanInfo FromJson(string json)
        {
            return json.FromJson<ScanInfo>();
        }

        #endregion

        #region making parameters

        /// <summary>
        /// Convert the info to spectral scan rectangle.
        /// </summary>
        /// <returns></returns>
        public SpectralScanRectangle ToSpectralScanRectangle()
        {
            double x = OffsetX < 0 ? StartX + OffsetX : StartX;
            double y = OffsetY < 0 ? StartY + OffsetY : StartY;
            SpectralScanRectangle rect = new SpectralScanRectangle(
                x, y, Math.Abs(OffsetX), Math.Abs(OffsetY),
                PixelSize, Direction == ScanInfo_ScanDirection.Y, false);
            return rect;
        }

        #endregion
    }

    [Flags]
    public enum ScanInfo_ScanDirection 
    { 
        X=1, 
        Y=2
    };
}
