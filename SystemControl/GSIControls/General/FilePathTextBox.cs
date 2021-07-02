using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSIControls.General
{
    public class FilePathTextBox : Panel
    {
        public FilePathTextBox()
        {
            Button btn = new Button();
            btn.Text = "src";
            btn.Width = 30;
            btn.Dock = DockStyle.Right;
            this.Controls.Add(btn);

            TextBox box = new TextBox();
            box.Dock = DockStyle.Fill;
            box.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            box.AutoCompleteSource = AutoCompleteSource.FileSystem;
            this.Controls.Add(box);

            InnerTextBox = box;
            InnerButton = btn;
            btn.Click += btn_Click;
        }

        void btn_Click(object sender, EventArgs e)
        {
            if(IsPath)
            {

            }
        }

        public TextBox InnerTextBox { get; private set; }

        public Button InnerButton { get; private set; }

        public string Text { get { return InnerTextBox.Text; } set { InnerTextBox.Text = value; } }

        /// <summary>
        /// True to select path.
        /// </summary>
        public bool IsPath { get; set; }
    }
}
