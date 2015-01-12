using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSI.Processing;

namespace GSIControls.Stage
{
    public partial class ScanImageParameters : UserControl
    {
        public ScanImageParameters()
        {
            InitializeComponent();
        }

        #region members

        public event EventHandler DataChanged;

        #endregion

        #region methods

        /// <summary>
        /// Check the validitiy of all numeric controls.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return this.Controls.Cast<Control>().Where(c => c is GSIControls.General.NumericControl)
                .Cast<GSIControls.General.NumericControl>().All(c => c.IsValid);
        }

        #endregion

        #region validation

        private void numericValueValidated(object sender, EventArgs e)
        {
            if (DataChanged != null)
                DataChanged(this, e);
        }

        #endregion

        #region Scan info population

        public void LoadScanInfo(ScanInfo info)
        {
            numDx.Value = info.DeltaX;
            numExp.Value = info.ExposureTime;
            numFrameRate.Value = info.FrameRate;
            numMaxFrameRate.Value = info.MaxFrameRate;
            numScanSpeed.Value = info.ScanSpeed;
            numMaxScanSpeed.Value = info.MaxScanSpeed;
            numPixelSize.Value = info.PixelSize;
            numSpatialUnits.Value = info.NativeSpatialUnitsToMeters;
            numTimeUnits.Value = info.NativeTimeUnitsToSeconds;
        }

        public void PopulateScanInfo(ScanInfo info)
        {
            info.DeltaXInPixels = numDx.Value;
            info.PixelSize = numPixelSize.Value;
            info.ExposureTime = info.ExposureTime;
            info.MaxFrameRate = numMaxFrameRate.Value;
            info.MaxScanSpeed = numMaxScanSpeed.Value;
            info.NativeSpatialUnitsToMeters = numSpatialUnits.Value;
            info.NativeTimeUnitsToSeconds = numTimeUnits.Value;
            LoadScanInfo(info);
        }

        #endregion
    }
}
