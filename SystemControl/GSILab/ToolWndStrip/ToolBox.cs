using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSILab.ToolWndStrip
{
    public partial class ToolBox : UserControl
    {
        public ToolBox()
        {
            ToolboxTitle = "[Toolbox]";
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeDrag();
        }

        #region members

        public string ToolboxTitle { get; private set; }

        #endregion

        #region drag events

        void InitializeDrag()
        {
            gripMain.MouseDown += gripMain_MouseDown;
            this.QueryContinueDrag += ToolBox_QueryContinueDrag;
        }

        Point startDragOffset;

        void ToolBox_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if(e.Action == DragAction.Continue)
            {
                Point mousepos = this.Parent.PointToClient(Control.MousePosition);
                this.Location = new Point(
                   mousepos.X - startDragOffset.X, mousepos.Y - startDragOffset.Y);
            }
        }

        void gripMain_MouseDown(object sender, MouseEventArgs e)
        {
            startDragOffset = e.Location;

            this.DoDragDrop(this, DragDropEffects.None);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            this.BringToFront();
            base.OnGotFocus(e);
        }

        #endregion
    }
}
