using FillingSystemHelper;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class WorklogFilterEditorForm : Form
    {
        public WorklogFilterEditorForm()
        {
            InitializeComponent();
            foreach (var message in _messages)
            {
                var lvi = lvEvents.Items.Add(message);
                lvi.Checked = CommonData.FilterEventsList.Contains(message);
            }
            var dt1 = CommonData.FilterDateTimeRange.First;
            var dt2 = CommonData.FilterDateTimeRange.Last;
            cbStartedTime.Checked = CommonData.FilterDateTimeRange.FirstExists;
            try
            {
                dtpStartDate.Value = cbStartedTime.Checked
                                         ? new DateTime(dt1.Year, dt1.Month, dt1.Day)
                                         : CommonData.MinRangeDate();
                dtpStartTime.Value = cbStartedTime.Checked
                                         ? new DateTime(dt1.Year, dt1.Month, dt1.Day,
                                                        dt1.Hour, dt1.Minute, dt1.Second)
                                         : CommonData.MinRangeDate();
            }
            catch (Exception)
            {
                dtpStartDate.Value = CommonData.MinRangeDate();
                dtpStartTime.Value = CommonData.MinRangeDate();
            }
            cbEndedTime.Checked = CommonData.FilterDateTimeRange.LastExists;
            try
            {
                dtpEndDate.Value = cbEndedTime.Checked
                                       ? new DateTime(dt2.Year, dt2.Month, dt2.Day)
                                       : CommonData.MaxRangeDate();
                dtpEndTime.Value = cbEndedTime.Checked
                                       ? new DateTime(dt2.Year, dt2.Month, dt2.Day,
                                                      dt2.Hour, dt2.Minute, dt2.Second)
                                       : CommonData.MaxRangeDate();

            }
            catch (Exception)
            {
                dtpEndDate.Value = CommonData.MinRangeDate();
                dtpEndTime.Value = CommonData.MinRangeDate();
            }
            var overpass = CommonData.FilterOverpassList;
            foreach (var item in lvOverpass.Items.Cast<ListViewItem>())
                item.Checked = overpass.Contains(item.Tag.ToString());
            var way = CommonData.FilterWayList;
            foreach (var item in lvWay.Items.Cast<ListViewItem>())
                item.Checked = way.Contains(item.Tag.ToString());
            var product = CommonData.FilterProductList;
            foreach (var item in lvProduct.Items.Cast<ListViewItem>())
                item.Checked = product.Contains(item.Tag.ToString());
            for (var i = 1; i <= 247; i++)
            {
                cbStartRisers.Items.Add(i.ToString("0"));
                cbEndRisers.Items.Add(i.ToString("0"));
            }
            var risers = CommonData.FilterRiserRange;
            cbStartRisers.Text = risers.Item1.ToString("0");
            cbEndRisers.Text = risers.Item2.ToString("0");

        }

        public DateTimeItem GetStartDateTime()
        {
            var item = new DateTimeItem();
            item.DateTime = new DateTime(dtpStartDate.Value.Year, dtpStartDate.Value.Month, dtpStartDate.Value.Day,
                    dtpStartTime.Value.Hour, dtpStartTime.Value.Minute, dtpStartTime.Value.Second);
            item.Exists = cbStartedTime.Checked;
            if (!item.Exists) item.DateTime = CommonData.MinRangeDate();
            return item;
        }

        public DateTimeItem GetEndDateTime()
        {
            var item = new DateTimeItem();
            item.DateTime = new DateTime(dtpEndDate.Value.Year, dtpEndDate.Value.Month, dtpEndDate.Value.Day,
                    dtpEndTime.Value.Hour, dtpEndTime.Value.Minute, dtpEndTime.Value.Second);
            item.Exists = cbEndedTime.Checked;
            if (!item.Exists) item.DateTime = CommonData.MaxRangeDate();
            return item;
        }

        public string[] GetSelectedEvents()
        {
            var list = (from lvi in lvEvents.Items.Cast<ListViewItem>() where lvi.Checked select lvi.Text).ToList();
            return list.ToArray();
        }

        public string[] GetSelectedOverpasses()
        {
            var list = (from lvi in lvOverpass.Items.Cast<ListViewItem>() where lvi.Checked select lvi.Tag.ToString()).ToArray();
            return list;
        }

        public string[] GetSelectedWays()
        {
            var list = (from lvi in lvWay.Items.Cast<ListViewItem>() where lvi.Checked select lvi.Tag.ToString()).ToArray();
            return list;
        }

        public string[] GetSelectedProducts()
        {
            var list = (from lvi in lvProduct.Items.Cast<ListViewItem>() where lvi.Checked select lvi.Tag.ToString()).ToArray();
            return list;
        }

        public int GetFirstRiser()
        {
            return int.Parse(cbStartRisers.Text);
        }

        public int GetLastRiser()
        {
            return int.Parse(cbEndRisers.Text);
        }

        private readonly string[] _messages = new[]
            {
                "Налив завершён аварийно. Сигнализатор аварийный",
                "Налив завершён аварийно. Неисправность цепи готовности",
                "Налив завершён аварийно. Неисправность сигнализатора уровня",
                "Налив завершён аварийно. Истекло время работы без связи",
                "Налив завершён аварийно. Заземление отсутствует",
                "Налив завершён аварийно. Ошибка клапана большого прохода",
                "Налив завершён аварийно. Ошибка клапана малого прохода",
                "Налив завершён аварийно. Ток сигнализатора уровня меньше минимального",
                "Налив завершён аварийно. Ток сигнализатора уровня больше максимального",
                "Налив завершён аварийно. Ток сигнализатора аварийного меньше минимального",
                "Налив завершён аварийно. Ток сигнализатора аварийного больше максимального",
                "Налив завершён аварийно. Сработал датчик рабочего положения",
                "Налив завершён аварийно. Неверное задание налива",
                "Запуск налива",
                "Налив завершён автоматически",
                "Налив завершён оператором АРМ",
                "Налив завершён аварийно. Кнопка СТОП",
                "Запуск системы",
                "Останов системы",
                "Вход в систему",
                "Выход из системы",
                "Установка соединения",
                "Обрыв соединения"
            };

        private void btnSelectAllEvents_Click(object sender, EventArgs e)
        {
            foreach (var lvi in lvEvents.Items.Cast<ListViewItem>())
                lvi.Checked = true;
            DataChanged(null, null);
        }

        private void btnClearAllEvents_Click(object sender, EventArgs e)
        {
            foreach (var lvi in lvEvents.Items.Cast<ListViewItem>())
                lvi.Checked = false;
            DataChanged(null, null);
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            var dt1 = DateTime.Now;
            dtpStartDate.Value = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
            dtpStartTime.Value = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
            dtpEndDate.Value = new DateTime(dt1.Year, dt1.Month, dt1.Day, 23, 59, 59);
            dtpEndTime.Value = new DateTime(dt1.Year, dt1.Month, dt1.Day, 23, 59, 59);
            cbStartedTime.Checked = true;
            cbEndedTime.Checked = true;
        }

        private void btnYesterday_Click(object sender, EventArgs e)
        {
            var dt1 = DateTime.Now.AddDays(-1);
            dtpStartDate.Value = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
            dtpStartTime.Value = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
            dtpEndDate.Value = new DateTime(dt1.Year, dt1.Month, dt1.Day, 23, 59, 59);
            dtpEndTime.Value = new DateTime(dt1.Year, dt1.Month, dt1.Day, 23, 59, 59);
            cbStartedTime.Checked = true;
            cbEndedTime.Checked = true;
        }

        private void btnResetRisers_Click(object sender, EventArgs e)
        {
            cbStartRisers.Text = @"1";
            cbEndRisers.Text = @"247";
            DataChanged(null, null);
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

        private void FormWorklogFilterEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            onCloseForm?.Invoke(this, new CloseFormEventArgs() { Location = this.Location });
        }

        private void DataChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            CommonData.UpdateFilterEventsList(GetSelectedEvents());
            CommonData.UpdateFilterDateTimeRange(GetStartDateTime(), GetEndDateTime());
            CommonData.UpdateFilterAddress(GetSelectedOverpasses(),
                                     GetSelectedWays(), GetSelectedProducts(), GetFirstRiser(), GetLastRiser());
            CommonData.WorkLogFilterChanged();
            btnApply.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbStartRisers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbEndRisers.Text = cbStartRisers.Text;
            btnResetRisers.Enabled = true;
            DataChanged(null, null);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cbStartedTime.Checked = false;
            cbEndedTime.Checked = false;
            dtpStartDate.MinDate = CommonData.MinRangeDate();
            dtpStartDate.MaxDate = CommonData.MaxRangeDate();
            dtpStartDate.Value = CommonData.MinRangeDate();
            dtpStartTime.MinDate = CommonData.MinRangeDate();
            dtpStartTime.MaxDate = CommonData.MaxRangeDate();
            dtpStartTime.Value = CommonData.MinRangeDate();
            dtpEndDate.MinDate = CommonData.MinRangeDate();
            dtpEndDate.MaxDate = CommonData.MaxRangeDate();
            dtpEndDate.Value = CommonData.MaxRangeDate();
            dtpEndTime.MinDate = CommonData.MinRangeDate();
            dtpEndTime.MaxDate = CommonData.MaxRangeDate();
            dtpEndTime.Value = CommonData.MaxRangeDate();
        }

        private void btnPrevDay_Click(object sender, EventArgs e)
        {
            if (dtpStartDate.Value != CommonData.MinRangeDate() && dtpStartTime.Value != CommonData.MinRangeDate())
            {
                dtpStartDate.Value -= new TimeSpan(1, 0, 0, 0);
                dtpStartTime.Value -= new TimeSpan(1, 0, 0, 0);
            }
            if (dtpEndDate.Value != CommonData.MinRangeDate() && dtpEndTime.Value != CommonData.MinRangeDate())
            {
                dtpEndDate.Value -= new TimeSpan(1, 0, 0, 0);
                dtpEndTime.Value -= new TimeSpan(1, 0, 0, 0);
            }
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            if (dtpStartDate.Value != CommonData.MaxRangeDate() && dtpStartTime.Value != CommonData.MaxRangeDate())
            {
                dtpStartDate.Value += new TimeSpan(1, 0, 0, 0);
                dtpStartTime.Value += new TimeSpan(1, 0, 0, 0);
            }
            if (dtpEndDate.Value != CommonData.MaxRangeDate() && dtpEndTime.Value != CommonData.MaxRangeDate())
            {
                dtpEndDate.Value += new TimeSpan(1, 0, 0, 0);
                dtpEndTime.Value += new TimeSpan(1, 0, 0, 0);
            }
        }

        private void cbEndRisers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnResetRisers.Enabled = true;
            DataChanged(null, null);
        }

        private void FormWorklogFilterEditor_Load(object sender, EventArgs e)
        {
            if (Location == Point.Empty)
                CenterToParent();
            btnApply.Enabled = false;
            timer1.Enabled = true;
        }

        private void lvEvents_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (sender != lvEvents)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    UncheckAll((ListView)sender);
                }
            }
            DataChanged(null, null);
        }

        private static void UncheckAll(ListView lv)
        {
            foreach (var lvi in lv.Items.Cast<ListViewItem>())
                if (lvi != null)
                    lvi.Checked = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lvWay.Enabled = lvOverpass.CheckedIndices.Count > 0;
            if (!lvWay.Enabled && lvWay.CheckedIndices.Count > 0) UncheckAll(lvWay);
            lvProduct.Enabled = lvWay.CheckedIndices.Count > 0;
            if (!lvProduct.Enabled && lvProduct.CheckedIndices.Count > 0) UncheckAll(lvProduct);
            cbStartRisers.Enabled = lvProduct.CheckedIndices.Count > 0;
            cbEndRisers.Enabled = lvProduct.CheckedIndices.Count > 0;
            if (!cbStartRisers.Enabled) cbStartRisers.Text = @"1";
            if (!cbEndRisers.Enabled) cbEndRisers.Text = @"247";
        }
    }

}
