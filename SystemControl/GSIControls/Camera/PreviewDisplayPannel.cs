using GSI.Camera;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSIControls.Camera
{
    /// <summary>
    /// Creates a display block to preview a camera preview.
    /// </summary>
    public class PreviewDisplayPannel : General.ImageDisplayBox
    {
        public PreviewDisplayPannel()
            : this(null)
        {
            FrameRateLabel = new Label();
            KeepAspectRatio = true;
            DrawFrameRate = true;
            Overlay = new DrawSurface();
            Overlay.Dock = DockStyle.Fill;
            //this.Controls.Add(Overlay);

            FrameRateLabel.BackColor = Color.Black;
            FrameRateLabel.ForeColor = Color.White;
            FrameRateLabel.AutoSize = true;
            FrameRateLabel.Left = 10;
            FrameRateLabel.Top = 10;
            FrameRateLabel.Visible = DrawFrameRate;
            FrameRateLabel.Text = "??";
            FrameRateLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(FrameRateLabel);

            this.MouseClick += Overlay_MouseClick;
        }

        public PreviewDisplayPannel(ICamera camera)
            :base()
        {
            this.BackColor = Color.White;
            if (camera != null)
                AttachCamera(camera);
        }

        #region helper class

        public class DrawSurface : Control
        {
            public DrawSurface()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.FromArgb(0, 0, 0, 0);
            }
        }

        #endregion

        #region members

        /// <summary>
        /// An overlay for the image.
        /// </summary>
        public Control Overlay { get; private set; }

        /// <summary>
        /// Frame rate label for the image.
        /// </summary>
        protected Label FrameRateLabel { get; private set; }

        /// <summary>
        /// A handles collection to allow detaching of the camera.
        /// </summary>
        Dictionary<ICamera, EventHandler<GSI.Camera.ImagereceivedEventArgs>> drawHandlers
           = new Dictionary<ICamera, EventHandler<ImagereceivedEventArgs>>();

        /// <summary>
        /// A handles collection to allow detaching of the camera.
        /// </summary>
        Dictionary<ICamera, EventHandler> settingsChangedHandlers
           = new Dictionary<ICamera, EventHandler>();

        bool m_KeepAspectRatio = false;

        /// <summary>
        /// If true keeps the image aspect ratio.
        /// </summary>
        public bool KeepAspectRatio
        {
            get { return m_KeepAspectRatio; }
            set
            {
                m_KeepAspectRatio = value;
                ClearImage();
            }
        }

        bool m_DrawFrameRate = false;

        /// <summary>
        /// If true draws the frame rate over the window.
        /// </summary>
        public bool DrawFrameRate 
        {
            get { return m_DrawFrameRate; }
            set
            {
                m_DrawFrameRate = value;
                FrameRateLabel.Visible = value;
            }
        }

        /// <summary>
        /// The position for the cross.
        /// </summary>
        public Point? CrossPosition { get; private set; }

        #endregion

        #region methods
        void Overlay_MouseClick(object sender, MouseEventArgs e)
        {
            CrossPosition = new Point(e.X, e.Y);
            DrawCross(Overlay.CreateGraphics());
        }

        public void AttachCamera(ICamera camera)
        {
            if(drawHandlers.ContainsKey(camera))
                return;
            drawHandlers[camera] = (s, e) =>
            {
                DrawImage(e.Data, e.Width, e.Height, (ICamera)s);
            };
            camera.PreviewImagereceived += drawHandlers[camera];
            settingsChangedHandlers[camera] = (s, e) =>
            {
                ClearImage();
            };
            camera.SettingsChanged += settingsChangedHandlers[camera];
        }

        /// <summary>
        /// Detach the camera process.
        /// </summary>
        /// <param name="camera"></param>
        /// <returns>False if no event to detach found.</returns>
        public bool DetachCamera(ICamera camera)
        {
            if (!drawHandlers.ContainsKey(camera))
                return false;
            camera.PreviewImagereceived -= drawHandlers[camera];
            drawHandlers.Remove(camera);
            camera.SettingsChanged -= settingsChangedHandlers[camera];
            settingsChangedHandlers.Remove(camera);
            return true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ClearImage();
        }

        /// <summary>
        /// Draws an image on the control.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="camera">Cannot be null</param>
        public void DrawImage(byte[] data, int width, int height, ICamera camera)
        {
            DrawImage(camera.ToImage(data, width, height));
            if (DrawFrameRate)
                FrameRateLabel.Text = width + "x" + height + "px FPS: " + camera.ActualFrameRate;
        }

        Pen _crossPen = new Pen(Color.Red, 1);

        private void DrawCross(Graphics g, bool doClear=false)
        {
            if (doClear)
                g.Clear(Color.FromArgb(0, 0, 0, 0));
            if (CrossPosition != null)
            {
                float cx = CrossPosition.Value.X;
                float cy = CrossPosition.Value.Y;
                g.DrawLine(new Pen(Color.Red, 1), ValidateInRange(cx - 1000, Width), cy, ValidateInRange(cx + 1000, Width), cy);
                g.DrawLine(new Pen(Color.Red, 1), cx, ValidateInRange(cy - 1000, Height), cx, ValidateInRange(cy + 1000, Height));
            }
        }

        private static float ValidateInRange(float val, int max)
        {
            if (val < 0)
                val = 0;
            if (val > max)
                val = max;
            return val;
        }

        public override void DrawImage(Image img)
        {
            base.DrawImage(img);

            try
            {
                Graphics g = this.CreateGraphics();
                DrawCross(g);
                g.Dispose();
            }
            catch
            {
            }
        }

        #endregion
    }
}
