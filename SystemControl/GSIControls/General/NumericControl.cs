using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSIControls.General
{
    public class NumericControl : TextBox
    {
        public NumericControl()
            :base()
        {
            Units = 1;
            TextAlign = HorizontalAlignment.Right;
            MinValue = double.MinValue;
            MaxValue = double.MaxValue;
        }

        /// <summary>
        /// Text to be converted to value.
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                Validate();
            }
        }

        private double m_value;

        /// <summary>
        /// The value.
        /// </summary>
        public double Value
        {
            get { return m_value; }
            set
            {
                base.Text = m_value.ToString();
                m_value = value;
            }
        }

        /// <summary>
        /// True if the current number is valid.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// The minimal value allowed.
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// The maximal value allowed.
        /// </summary>
        public double MaxValue { get; set; }

        /// <summary>
        /// The units to for the value.
        /// </summary>
        public double Units { get; set; }

        void Validate()
        {
            double val = 0;
            IsValid = double.TryParse(this.Text, out val);
            if (IsValid && (val < MinValue || val > MaxValue))
                IsValid = false;

            m_value = val * Units;
            this.BackColor = IsValid ? default(Color) : Color.FromArgb(255, 0, 0);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Validate();
        }
    }
}
