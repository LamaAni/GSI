using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GSILab.ToolWndStrip
{
    [Designer(typeof(ToolBoxStripDesigner))]
    public partial class ToolBoxStrip : UserControl
    {
        public ToolBoxStrip()
        {
            InitializeComponent();
        }

        #region members

        /// <summary>
        /// A collection of tool strip forms
        /// </summary>
        public List<ToolBox> Forms { get; private set; }

        /// <summary>
        /// The container box for the internal children.
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Control InternalContainer
        {
            get { return internalContainer; }
        }

        /// <summary>
        /// A list of the boxes.
        /// </summary>
        private List<ToolBox> Boxes = new List<ToolBox>();

        #endregion

        #region members

        protected override void OnLoad(EventArgs e)
        {
            // called when loaded.
            foreach (Control box in this.InternalContainer.Controls)
            {
                if (!(box is ToolBox))
                    continue;

                box.Visible = true;
                AddToolBox((ToolBox)box);
            }

            base.OnLoad(e);

            RecalculateStrip();
        }

        public void AddToolBox(ToolBox box)
        {
            Boxes.Add(box);
            this.Controls.Add(box);
        }

        #endregion

        #region strip

        protected void RecalculateStrip()
        {
            // clearing all items
            pannelButtons.Controls.Clear();
            foreach (ToolBox box in Boxes)
            {
                CheckBox chk = new CheckBox();
                chk.Text = box.ToolboxTitle;
                chk.Appearance = Appearance.Button;
                chk.FlatStyle = FlatStyle.Flat;
                chk.Checked = box.Visible;
                chk.CheckedChanged += (s, e) =>
                {
                    if (box.Visible != chk.Checked)
                    {
                        box.Visible = chk.Checked;
                        box.BringToFront();
                        if (box.Visible)
                        {
                            box.Top = chk.Bottom;
                            box.Left = chk.Left;
                        }
                    }
                };

                pannelButtons.Controls.Add(chk);
            }
        }

        protected void InitalizeStripScroll()
        {

        }

        #endregion
    }

    public class ToolBoxStripDesigner : ParentControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (this.Control is ToolBoxStrip)
            {
                this.EnableDesignMode(((ToolBoxStrip)this.Control).InternalContainer, "InternalContainer");
            }
        }
    }
}
