using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSIControls.General
{
    public class ImageDisplayBox : Control
    {
        public ImageDisplayBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        bool m_KeepAspectRatio = true;

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

        /// <summary>
        /// Draws an image on the control.
        /// </summary>
        public virtual void DrawImage(Image img)
        {
            int width = 0;
            int height = 0;
            int x = 0, y = 0;

            if (KeepAspectRatio)
            {
                double ratio = img.Height * 1.0 / img.Width;
                double screenRatio = this.Height * 1.0 / this.Width;
                int maxSize = 0;

                maxSize = screenRatio > ratio ? this.Width : this.Height;

                if (screenRatio > ratio)
                {
                    width = maxSize;
                    height = maxSize;
                    height = Convert.ToInt32(Math.Floor(height * ratio));
                }
                else
                {
                    height = maxSize;
                    width = maxSize;
                    width = Convert.ToInt32(Math.Floor(
                        screenRatio > 1 ? width * ratio : width * 1.0 / ratio));
                }

                x = Convert.ToInt32(Math.Floor((this.Width - width) * 1.0 / 2));
                y = Convert.ToInt32(Math.Floor((this.Height - height) * 1.0 / 2));
            }
            else
            {
                width = this.Width;
                height = this.Height;
            }

            try
            {
                Graphics g = this.CreateGraphics();
                g.DrawImage(img, x, y, width, height);
                g.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Clears the image from the pannel.
        /// </summary>
        public void ClearImage()
        {
            this.CreateGraphics().Clear(Color.White);
        }
    }
}
