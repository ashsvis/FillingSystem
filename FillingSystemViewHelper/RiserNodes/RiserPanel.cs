using FillingSystemHelper;
using System;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public delegate void RiserEvent(RiserPanel panel);
    public partial class RiserPanel : UserControl
    {
        public RiserKey RiserKey { get; private set; }

        public RiserPanel(RiserKey key)
        {
            InitializeComponent();
            riserControl1.Key = key;
            riserControl1.Riser = key.Riser;
            this.RiserKey = key;
        }

        public int Number { get => riserControl1.Riser; }

        public bool Linked { get => riserControl1.Linked; }

        public event RiserEvent IsFocused;

        public event RiserEvent IsDoubleClicked;

        public event RiserEvent OnStart;

        public event RiserEvent OnStop;

        public void Build(ushort[] hregs, int nType, int setPoint, int current)
        {
            riserControl1.SetPoint = setPoint;
            riserControl1.NType = nType;
            riserControl1.Current = current;
            riserControl1.UpdateData(hregs);
            chboxSelected.Enabled = btnStart.Enabled = btnStop.Enabled = riserControl1.Linked;
        }

        /// <summary>
        /// Получение фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RiserPanel_Enter(object sender, EventArgs e)
        {
            riserControl1.Selected = true;
            riserControl1.Invalidate();
            IsFocused?.Invoke(this);
        }

        public bool IsSelected()
        {
            return chboxSelected.Checked;
        }

        /// <summary>
        /// Потеря фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RiserPanel_Leave(object sender, EventArgs e)
        {
            riserControl1.Selected = false;
            riserControl1.Invalidate();
        }

        public override string ToString()
        {
            return $"Стояк №{riserControl1.Riser}";
        }

        private void riserControl1_DoubleClick(object sender, EventArgs e)
        {
            IsDoubleClicked?.Invoke(this);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            OnStart?.Invoke(this);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            OnStop?.Invoke(this);
        }
    }

}
