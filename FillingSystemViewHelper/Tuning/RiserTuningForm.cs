using FillingSystemHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class RiserTuningForm : Form
    {
        private int tabNo;
        public RiserKey RiserKey { get; set; }

        public RiserTuningForm(int tabNo = 0)
        {
            InitializeComponent();

            riserTuningLink.OnWrite += RiserTuningLink_OnWrite;
            riserTuningLink.OnMessage += RiserTuningLink_OnMessage;
            riserTuningPlc.OnWrite += RiserTuningLink_OnWrite;
            riserTuningPlc.OnMessage += RiserTuningLink_OnMessage;
            riserTuningAdc.OnWrite += RiserTuningLink_OnWrite;
            riserTuningAdc.OnMessage += RiserTuningLink_OnMessage;
            riserTuningAlarmLevel.OnWrite += RiserTuningLink_OnWrite;
            riserTuningAlarmLevel.OnMessage += RiserTuningLink_OnMessage;
            riserTuningAnalogLevel.OnWrite += RiserTuningLink_OnWrite;
            riserTuningAnalogLevel.OnMessage += RiserTuningLink_OnMessage;

            this.tabNo = tabNo;
            tabControl1.SelectTab(tabNo);

            labMessage.Text = "";
        }

        private void RiserTuningForm_Load(object sender, EventArgs e)
        {
            if (Location == Point.Empty)
                CenterToScreen();
            timerWatchDog.Enabled = true;
        }

        public void Build(RiserKey riserKey, ushort[] registers)
        {
            timerWatchDog.Enabled = false;
            this.RiserKey = riserKey;
            UpdateCaption();
            riserTuningLink.UpdateData(riserKey, registers);
            riserTuningPlc.UpdateData(riserKey, registers);
            riserTuningAdc.UpdateData(riserKey, registers);
            riserTuningAlarmLevel.UpdateData(riserKey, registers);
            riserTuningAnalogLevel.UpdateData(riserKey, registers);
            timerWatchDog.Enabled = true;
        }

        private event WriteDataEventHandler onWriteData;

        public event WriteDataEventHandler OnWriteData
        {
            add
            {
                onWriteData += value;
            }
            remove
            {
                onWriteData -= value;
            }
        }

        /// <summary>
        /// Команда для записи новых параметров
        /// </summary>
        /// <param name="address"></param>
        /// <param name="regcount"></param>
        /// <param name="hregs"></param>
        /// <param name="changelogdata"></param>
        private void RiserTuningLink_OnWrite(RiserKey riserKey, int address, int regcount, ushort[] hregs, string[] changelogdata = null)
        {
            var args = new WriteDataEventArgs()
            {
                RiserKey = riserKey,
                RegAddr = address + 1,
                WriteData = new List<ushort>(hregs).ToArray(),
                LogData = changelogdata != null ? new List<string>(changelogdata).ToArray() : null
            };
            onWriteData?.Invoke(this, args);
        }

        private void RiserTuningLink_OnMessage(string text)
        {
            labMessage.Text = text;
            timer2.Enabled = true;
        }

        public int TabNo
        {
            get { return tabNo; }
            set
            {
                tabNo = value;
                tabControl1.SelectTab(tabNo);
            }
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

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            labMessage.Text = "";
        }

        private void timerWatchDog_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void RiserTuningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            onCloseForm?.Invoke(this, new CloseFormEventArgs() { Location = this.Location });
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabNo = tabControl1.SelectedIndex;
            UpdateCaption();
        }

        private void UpdateCaption()
        {
            switch (tabNo)
            {
                case 0:
                    Text = $"Параметры связи [ Стояк {RiserKey.Riser} ]";
                    break;
                case 1:
                    Text = $"Параметры логики [ Стояк {RiserKey.Riser} ]";
                    break;
                case 2:
                    Text = $"Параметры ADC [ Стояк {RiserKey.Riser} ]";
                    break;
                case 3:
                    Text = $"Сигнализатор аварийный [ Стояк {RiserKey.Riser} ]";
                    break;
                case 4:
                    Text = $"Сигнализатор уровня [ Стояк {RiserKey.Riser} ]";
                    break;
            }
        }
    }
}
