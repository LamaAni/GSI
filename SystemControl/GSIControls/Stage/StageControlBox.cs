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
    public partial class StageControlBox : UserControl
    {
        public StageControlBox()
        {
            InitializeComponent();
        }

        #region members

        public event EventHandler DataChanged;

        public GSI.Stage.IPositionControl PositionControl { get; private set; }

        #endregion

        #region position control

        /// <summary>
        /// Binds the position control to the stage.
        /// </summary>
        /// <param name="ctrl"></param>
        public void BindBositionControl(IPositionControl ctrl)
        {
            PositionControl = ctrl;
        }

        #endregion

        #region polulation of values

        public bool IsValid()
        {
            return numStageAngle.IsValid;
        }

        public void PopulateScanInfo(ScanInfo info)
        {
            info.StageAngle = numStageAngle.Value;
        }

        public void LoadScanInfo(ScanInfo info)
        {
            numStageAngle.Value = info.StageAngle;
        }

        #endregion

        private void numStageAngle_Validated(object sender, EventArgs e)
        {
            if (DataChanged != null)
                DataChanged(this, e);
        }

        private void btnGoto00_Click(object sender, EventArgs e)
        {
            PositionControl.SetPosition(0, 0);
        }

        private void btnSet00_Click(object sender, EventArgs e)
        {
            PositionControl.SetAsHome();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            PositionControl.SetPosition(numXPos.Value, numYPos.Value);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet.");
        }
    }
}
