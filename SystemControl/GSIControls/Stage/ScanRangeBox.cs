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
using GSI.Stage;

namespace GSIControls.Stage
{
    public partial class ScanRangeBox : UserControl
    {
        public ScanRangeBox()
        {
            InitializeComponent();
            ddScanOver.SelectedIndex = 1;
        }

        #region members

        /// <summary>
        /// The position control bound to this control.
        /// </summary>
        public IPositionControl PositionControl { get; private set; }

        /// <summary>
        /// Called when the data is changed.
        /// </summary>
        public event EventHandler DataChanged;

        /// <summary>
        /// The pixel size in the stage units.
        /// </summary>
        public double PixelSize { get; private set; }

        /// <summary>
        /// If true then scan over the Y direction.
        /// </summary>
        public bool ScanOverY { get { return ddScanOver.SelectedIndex == 1; } }

        #endregion

        #region valdation

        /// <summary>
        /// The validations of the numeric values.
        /// </summary>
        public void DoValidation(bool doOffsetByPixels = false)
        {
            if (doOffsetByPixels)
            {
                numXOffset.Value = numXOffsetInPixels.Value * PixelSize;
                numYOffset.Value = numYOffsetInPixels.Value * PixelSize;
            }
            else
            {
                numXOffsetInPixels.Value = numXOffset.Value / PixelSize;
                numYOffsetInPixels.Value = numYOffset.Value / PixelSize;
            }
        }

        /// <summary>
        /// Returns the validity of the current values.
        /// </summary>
        /// <returns></returns>
        public bool IsRangeValid()
        {
            return numStartX.IsValid && numStartY.IsValid && numXOffset.IsValid && numYOffset.IsValid;
        }

        #endregion

        #region members activation

        void CallChanged(EventArgs e)
        {
            if (DataChanged != null) DataChanged(this, e);
        }

        private void numXOffset_Validated(object sender, EventArgs e)
        {
            DoValidation(false);
            CallChanged(e);
        }

        private void numYOffset_Validated(object sender, EventArgs e)
        {
            DoValidation(false);
            CallChanged(e);
        }

        private void numXOffsetInPixels_Validated(object sender, EventArgs e)
        {
            DoValidation(true);
            CallChanged(e);
        }

        private void numYOffsetInPixels_Validated(object sender, EventArgs e)
        {
            DoValidation(true);
            CallChanged(e);
        }

        private void ddScanOver_SelectedIndexChanged(object sender, EventArgs e)
        {
            CallChanged(e);
        }

        private void chkDoSpeedup_CheckedChanged(object sender, EventArgs e)
        {
            CallChanged(e);
        }

        private void doStopMotion_CheckedChanged(object sender, EventArgs e)
        {
            CallChanged(e);
        }

        #endregion

        #region external methods

        /// <summary>
        /// Binds the current control to the position control
        /// </summary>
        /// <param name="ctrl"></param>
        public void BindToPositionControl(IPositionControl ctrl)
        {
            PositionControl = ctrl;
        }

        /// <summary>
        /// Populates the controls to the scan info.
        /// </summary>
        /// <param name="info"></param>
        public void PopulateScanInfo(ScanInfo info)
        {
            info.DoSpeedup = chkDoSpeedup.Checked;
            info.DoStopMotion = chkDoStopMotion.Checked;
            info.StartX = numStartX.Value;
            info.StartY = numStartY.Value;
            info.OffsetX = numXOffset.Value;
            info.OffsetY = numYOffset.Value;
            info.Direction = ddScanOver.SelectedIndex == 0 ? ScanInfo_ScanDirection.X :
                ScanInfo_ScanDirection.Y;

            DoValidation(false);
        }

        /// <summary>
        /// Loads the paramters inside the scan info.
        /// </summary>
        /// <param name="info"></param>
        public void LoadScanInfo(ScanInfo info)
        {
            chkDoSpeedup.Checked = info.DoSpeedup;
            PixelSize = info.PixelSize;
            numStartX.Value = info.StartX;
            numStartY.Value = info.StartY;
            numXOffset.Value = info.OffsetX;
            numYOffset.Value = info.OffsetY;
            ddScanOver.SelectedIndex =
                info.Direction == ScanInfo_ScanDirection.X ? 0 : 1;

            
        }

        #endregion

        private void btnTakeCurrentPosition_Click(object sender, EventArgs e)
        {
            if (PositionControl == null)
            {
                MessageBox.Show("No position control (stage) found.");
                return;
            }
            numStartX.Value = PositionControl.PositionX;
            numStartY.Value = PositionControl.PositionY;
        }

        private void btnOffsetToCurrent_Click(object sender, EventArgs e)
        {
            if (PositionControl == null)
            {
                MessageBox.Show("No position control (stage) found.");
                return;
            }
            numXOffset.Value = PositionControl.PositionX - numStartX.Value;
            numYOffset.Value = PositionControl.PositionY - numStartY.Value;
        }

    }
}
