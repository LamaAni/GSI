using GSI.Camera.LumeneraControl;
using Lumenera.USB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSIControls.Camera.Lumenera
{
    /// <summary>
    /// Opens a settings dialog to control the settings collection. 
    /// </summary>
    public partial class SettingsDialog : Form
    {
        public SettingsDialog(LumeneraCamera camera)
        {
            InitializeComponent();
            Camera = camera;
            UpdateFields();
        }

        #region initialization members

        void UpdateFields()
        {
            holdUpdate = true;
            txtFrameRate.Text = Settings.FrameRate.ToString();
            txtXSampling.Text = (Settings.FlagsX == dll.LucamFrameFormatFlag.Binning ?
                Settings.BinningX : Settings.SubSampleX).ToString();
            txtYSampling.Text = (Settings.FlagsX == dll.LucamFrameFormatFlag.Binning ?
                Settings.BinningY : Settings.SubSampleY).ToString();

            chkDoBinningX.Checked = Settings.FlagsX == dll.LucamFrameFormatFlag.Binning;
            chkDoBinningY.Checked = Settings.FlagsY == dll.LucamFrameFormatFlag.Binning;

            txtHeight.Text = Settings.Height.ToString();
            txtWidth.Text = Settings.Width.ToString();
            ddPixelFormat.Items.Clear();
            Camera.GetAvilablePixelFormats().ToList().ForEach((v) =>
            {
                int index = ddPixelFormat.Items.Add(v);
                if (v == Settings.PixelFormat)
                    ddPixelFormat.SelectedIndex = index;
            });
            txtXOffset.Text = Settings.X.ToString();
            txtYOffset.Text = Settings.Y.ToString();
            txtExposure.Text = Settings.Exposure.ToString();
            txtGain.Text = Settings.Gain.ToString();
            holdUpdate = false;
        }

        #endregion

        #region Mode members

        bool holdUpdate = false;

        /// <summary>
        /// The autoupdate mode that was called when the dialog was shown.
        /// </summary>
        public bool LastAutoUpdate { get; private set; }

        /// <summary>
        /// The settings collection associated with the dialog.
        /// </summary>
        public CameraSettings Settings { get { return Camera.Settings; } }

        /// <summary>
        /// The camera for the settings.
        /// </summary>
        public LumeneraCamera Camera { get; private set; }

        /// <summary>
        /// Shows the settings form in dialog mode (not autosave).
        /// </summary>
        /// <returns></returns>
        public new DialogResult ShowDialog()
        {
            LastAutoUpdate = Settings.AutoUpdate;
            Settings.AutoUpdate = false;
            btnSave.Visible = true;
            DialogResult rslt = base.ShowDialog();
            return rslt;
        }

        /// <summary>
        /// Shows the settings form in window mode (autosave).
        /// </summary>
        /// <returns></returns>
        public new void Show()
        {
            LastAutoUpdate = Settings.AutoUpdate;
            Settings.AutoUpdate = true;
            btnSave.Visible = false;
            base.Show();
        }


        protected override void OnClosed(EventArgs e)
        {
            Settings.AutoUpdate = LastAutoUpdate;
            base.OnClosed(e);
        }

        #endregion

        private void txtYOffset_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            int val;
            txtYOffset.ForeColor = Color.Red;
            if (int.TryParse(txtYOffset.Text, out val))
            {
                Settings.Y = val;
                txtYOffset.ForeColor = default(Color);
            }
        }

        private void txtXOffset_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            int val;
            txtXOffset.ForeColor = Color.Red;
            if (int.TryParse(txtXOffset.Text, out val))
            {
                Settings.X = val;
                txtXOffset.ForeColor = default(Color);
            }
        }

        private void txtWidth_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            int val;
            txtWidth.ForeColor = Color.Red;
            if (int.TryParse(txtWidth.Text, out val))
            {
                val = (val / 8) * 8;
                holdUpdate = true;
                txtWidth.Text = val.ToString();
                holdUpdate = false;
                Settings.Width = val;
                txtWidth.ForeColor = default(Color);
            }
        }

        private void txtHeight_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            int val;
            txtHeight.ForeColor = Color.Red;
            if (int.TryParse(txtHeight.Text, out val))
            {
                val = (val / 8) * 8;
                holdUpdate = true;
                txtHeight.Text = val.ToString();
                holdUpdate = false;
                Settings.Height = val;
                txtHeight.ForeColor = default(Color);
            }
        }

        private void txtXSampling_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            short val;
            txtXSampling.ForeColor = Color.Red;
            if (short.TryParse(txtXSampling.Text, out val))
            {
                Settings.HoldUpdate();
                Settings.SubSampleX = val;
                Settings.BinningX = val;
                Settings.ResumeUpdate();
                txtXSampling.ForeColor = default(Color);
            }
        }

        private void txtYSampling_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            short val;
            txtYSampling.ForeColor = Color.Red;
            if (short.TryParse(txtYSampling.Text, out val))
            {
                Settings.HoldUpdate();
                Settings.SubSampleY = val;
                Settings.BinningY = val;
                Settings.ResumeUpdate();
                txtYSampling.ForeColor = default(Color);
            }
        }

        private void chkDoBinningX_CheckedChanged(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            Settings.FlagsX = chkDoBinningX.Checked ?
                dll.LucamFrameFormatFlag.Binning : default(dll.LucamFrameFormatFlag);
        }

        private void chkDoBinningY_CheckedChanged(object sender, EventArgs e)
        {
            Settings.FlagsY = chkDoBinningY.Checked ?
                dll.LucamFrameFormatFlag.Binning : default(dll.LucamFrameFormatFlag);
        }

        private void txtFrameRate_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            double val;
            txtFrameRate.ForeColor = Color.Red;
            if (double.TryParse(txtFrameRate.Text, out val))
            {
                Settings.FrameRate = val;
                txtFrameRate.ForeColor = default(Color);
            }
        }

        private void txtExposure_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            double val;
            txtExposure.ForeColor = Color.Red;
            if (double.TryParse(txtExposure.Text, out val))
            {
                Settings.Exposure = val;
                txtExposure.ForeColor = default(Color);
            }
        }

        private void txtGain_Leave(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            double val;
            txtGain.ForeColor = Color.Red;
            if (double.TryParse(txtGain.Text, out val))
            {
                Settings.Gain = val;
                txtGain.ForeColor = default(Color);
            }
        }

        private void ddPixelFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            Settings.PixelFormat =
                (dll.LucamPixelFormat)ddPixelFormat.SelectedItem;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            Settings.InvokeChanged();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (holdUpdate)
                return;
            this.Close();
        }
    }
}
