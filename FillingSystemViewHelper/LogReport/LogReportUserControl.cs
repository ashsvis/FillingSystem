using FillingSystemHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class LogReportUserControl : UserControl
    {
        public FillingSqlServer SqlServer { get; internal set; }

        public LogReportUserControl()
        {
            InitializeComponent();
            lvTable.Columns.Add(new ColumnHeader() { Text = "Дата", Width = 90, TextAlign = HorizontalAlignment.Center });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Время", Width = 90, TextAlign = HorizontalAlignment.Center });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Адрес устройства", Width = 260 });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Событие", Width = 600 });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Вагон", Width = 80, TextAlign = HorizontalAlignment.Center });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Тип", Width = 60, TextAlign = HorizontalAlignment.Center });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Высота", Width = 80, TextAlign = HorizontalAlignment.Right });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Ист.", Width = 80, TextAlign = HorizontalAlignment.Center });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Задание", Width = 80, TextAlign = HorizontalAlignment.Right });
            CommonData.OnUpdateWorkLogFilter += (o, e) => btnRefresh.PerformClick(); 
        }

        private event EventHandler onRefresh;

        public event EventHandler OnRefresh
        {
            add
            {
                onRefresh += value;
            }
            remove
            {
                onRefresh -= value;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lvTable.VirtualListSize = SqlServer.GetLogReportRowsCount();
            lvTable.Invalidate();
            onRefresh?.Invoke(this, EventArgs.Empty);
        }

        private event EventHandler onShowFilterDialog;

        public event EventHandler OnShowFilterDialog
        {
            add
            {
                onShowFilterDialog += value;
            }
            remove
            {
                onShowFilterDialog -= value;
            }
        }

        private void btnShowFilterForm_Click(object sender, EventArgs e)
        {
            onShowFilterDialog.Invoke(this, EventArgs.Empty);
        }

        private Dictionary<int, DataRow> cash = new Dictionary<int, DataRow>(); 

        private void lvTable_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            var data = SqlServer.GetLogReport(e.StartIndex, e.EndIndex + 1);
            cash.Clear();
            var n = e.StartIndex;
            foreach (var row in data.Rows.Cast<DataRow>())
            {
                cash.Add(n, row);
                n++;
            }
        }

        private void lvTable_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var lvi = new ListViewItem($"{e.ItemIndex}");
            lvi.SubItems.AddRange(new string[] { "", "", "", "", "", "", "", "" });
            e.Item = lvi;
            if (cash.Count == 0 || !cash.ContainsKey(e.ItemIndex))
                return;
            var row = cash[e.ItemIndex];
            var snap = (DateTime)row["Snaptime"];
            var productCode = (string)row["Product"];
            var address = string.IsNullOrEmpty(productCode)
                ? "" : $"Эстакада {row["Overpass"]}.Путь {row["Way"]}.{new ProductSelection((string)row["Product"])}.Стояк {row["Riser"]}";
            var eventInfo = (string)row["EventInfo"];
            var number = (string)row["Number"];
            var ntype = (int)row["NType"];
            var maxHeight = (int)row["MaxHeight"];
            var source = (string)row["Source"];
            var setLevel = (int)row["SetLevel"];
            lvi.Text = $"{snap.Date:dd-MM-yyyy}";
            lvi.SubItems[1].Text = $"{snap.TimeOfDay}".Split('.')[0];
            lvi.SubItems[2].Text = address;
            lvi.SubItems[3].Text = eventInfo;
            lvi.SubItems[4].Text = number;
            lvi.SubItems[5].Text = ntype > 0 ? $"{ntype}" : "";
            lvi.SubItems[6].Text = ntype > 0 ? $"{maxHeight}" : "";
            lvi.SubItems[7].Text = source;
            lvi.SubItems[8].Text = ntype > 0 ? $"{setLevel}" : "";
        }

        private void LogReportUserControl_Load(object sender, EventArgs e)
        {         
            lvTable.VirtualListSize = SqlServer.GetLogReportRowsCount();
        }
    }
}
