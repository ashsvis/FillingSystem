using FillingSystemHelper;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class StatusForm : Form
    {
        public StatusForm()
        {
            InitializeComponent();
        }

        public RiserKey RiserKey { get; set; }

        public void Build(RiserKey riserKey, ushort[] registers)
        {
            timerWatchDog.Enabled = false;
            RiserKey = riserKey;
            Text = $"Состояние [ Стояк {riserKey.Riser} ]";
            riserStatus.UpdateData(riserKey, registers);
            timerWatchDog.Enabled = true;
        }

        private void StatusForm_Load(object sender, EventArgs e)
        {
            if (Location == Point.Empty)
                CenterToScreen();
            timerWatchDog.Enabled = true;
        }

        private event CloseFormEventHandler onCloseForm;

        public event CloseFormEventHandler OnCloseForm
        {
            add
            {
                onCloseForm += value;
            }
            remove
            {
                onCloseForm -= value;
            }
        }

        private void StatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            onCloseForm?.Invoke(this, new CloseFormEventArgs() { Location = this.Location });
        }

        private void timerWatchDog_Tick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
