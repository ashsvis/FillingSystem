using FillingSystemHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{

    public partial class WagonUserControl : UserControl
    {
        private Point editorFormLocation;
        public FillingSqlServer SqlServer { get; internal set; }

        public WagonUserControl()
        {
            InitializeComponent();
            lvTable.Columns.Add(new ColumnHeader() { Text = "Вагон", Width = 80, TextAlign = HorizontalAlignment.Center });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Тип", Width = 60, TextAlign = HorizontalAlignment.Center });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Фактическая высота", Width = 140, TextAlign = HorizontalAlignment.Right });
            lvTable.Columns.Add(new ColumnHeader() { Text = "Количество наливов", Width = 140, TextAlign = HorizontalAlignment.Right });
        }

        private void WagonUserControl_Load(object sender, EventArgs e)
        {
            lvTable.VirtualListSize = SqlServer.GetWagonsRowsCount();
        }

        private void miAddWaggon_Click(object sender, EventArgs e)
        {
            ShowWaggonInsertDialog();
        }

        private void miEditWaggon_Click(object sender, EventArgs e)
        {
            if (!CommonData.EnteredAsAdmin()) return;
            if (lvTable.SelectedIndices.Count == 0) return;
            var index = lvTable.SelectedIndices[0];
            ShowWaggonEditDialog(index);
        }

        private void miDeleteWaggon_Click(object sender, EventArgs e)
        {
            ShowWaggonDeleteDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (!CommonData.EnteredAsAdmin())
            {
                e.Cancel = true;
                return;
            }
            miEditWaggon.Enabled = lvTable.VirtualListSize > 0;
            miDeleteWaggon.Enabled = lvTable.VirtualListSize > 0;
        }

        private void lvTable_DoubleClick(object sender, EventArgs e)
        {
            if (!CommonData.EnteredAsAdmin()) return;
            if (lvTable.SelectedIndices.Count == 0) return;
            var index = lvTable.SelectedIndices[0];
            ShowWaggonEditDialog(index);
        }

        private void ShowWaggonInsertDialog()
        {
            CloseEditorForm();
            var editorForm = new WagonDataEditorForm() { Location = editorFormLocation };
            editorForm.OnGetWagonTypes += (o, e) => onGetWagonTypes?.Invoke(o, e);
            editorForm.Update(false, "", 0, 0, 0);
            editorForm.Show(this);

            editorForm.OnCancel += (o, e) => editorForm.Close();

            editorForm.OnOk += (o, e) =>
            {
                if (SqlServer.InsertIntoWagons(e.Number, e.Ntype, e.RealHeight))
                {
                    lvTable.VirtualListSize = SqlServer.GetWagonsRowsCount();
                    lvTable.Invalidate();
                    var lvi = lvTable.FindItemWithText(e.Number);
                    lvTable.SelectedIndices.Clear();
                    lvTable.SelectedIndices.Add(lvi.Index);
                    lvi.Focused = true;
                    lvi.EnsureVisible();
                }
                editorForm.Close();
            };

            editorForm.OnCloseForm += (o, e) => editorFormLocation = e.Location;

        }

        private void lvTable_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            var index = SqlServer.GetWagonIndex(e.Text);
            e.Index = index - 1;
        }

        private void ShowWaggonEditDialog(int index)
        {
            CloseEditorForm();
            if (index >= 0 && lvTable.VirtualListSize > 0 && index < lvTable.VirtualListSize)
            {
                var data = SqlServer.GetWagons(index, 1);
                foreach (var row in data.Rows.Cast<DataRow>())
                {
                    var number = (string)row["Number"];
                    var ntype = (int)row["NType"];
                    var realHeight = (int)row["RealHeight"];
                    var fillCount = (int)row["FillCount"];
                    var editorForm = new WagonDataEditorForm() { Location = editorFormLocation };
                    editorForm.OnGetWagonTypes += (o, e) => onGetWagonTypes?.Invoke(o, e);
                    editorForm.Update(true, number, ntype, realHeight, fillCount);
                    editorForm.Show(this);

                    editorForm.OnCancel += (o, e) => editorForm.Close();

                    editorForm.OnOk += (o, e) =>
                    {
                        if (SqlServer.UpdateIntoWagons(e.Number, e.Ntype, e.RealHeight))
                        {
                            lvTable.VirtualListSize = SqlServer.GetWagonsRowsCount();
                            lvTable.Invalidate();
                        }
                        editorForm.Close();
                    };

                    editorForm.OnCloseForm += (o, e) => editorFormLocation = e.Location;
                    break;
                }

            }
        }

        private void ShowWaggonDeleteDialog()
        {
            if (lvTable.SelectedIndices.Count == 0) return;
            var index = lvTable.SelectedIndices[0];           
            var number = SqlServer.GetWagon(index);
            onDelete?.Invoke(this, new WagonEventArgs()
            {
                Number = number
            });
            lvTable.VirtualListSize = 0;
            lvTable.VirtualListSize = SqlServer.GetWagonsRowsCount();
            lvTable.Invalidate();
        }

        private event WagonEventHandler onDelete;

        public event WagonEventHandler OnDelete
        {
            add
            {
                onDelete += value;
            }
            remove
            {
                onDelete -= value;
            }
        }

        private void WagonUserControl_Leave(object sender, EventArgs e)
        {
            CloseEditorForm();
        }

        private static void CloseEditorForm()
        {
            var editorForm = Application.OpenForms.OfType<WagonDataEditorForm>().FirstOrDefault();
            if (editorForm != null)
                editorForm.Close();
        }

        private event GetWagonTypesEventHandler onGetWagonTypes;

        public event GetWagonTypesEventHandler OnGetWagonTypes
        {
            add
            {
                onGetWagonTypes += value;
            }
            remove
            {
                onGetWagonTypes -= value;
            }
        }

        private Dictionary<int, DataRow> cash = new Dictionary<int, DataRow>();

        private void lvTable_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            var data = SqlServer.GetWagons(e.StartIndex, e.EndIndex + 1);
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
            var lvi = new ListViewItem();
            lvi.SubItems.AddRange(new string[] { "", "", "" });
            e.Item = lvi;
            if ((e.ItemIndex % 2) == 1)
            {
                e.Item.BackColor = Color.FromArgb(227, 227, 227);
                e.Item.UseItemStyleForSubItems = true;
            }
            if (cash.Count == 0 || !cash.ContainsKey(e.ItemIndex))
                return;
            var row = cash[e.ItemIndex];
            var number = (string)row["Number"];
            var ntype = (int)row["NType"];
            var realHeight = (int)row["RealHeight"];
            var fillCount = (int)row["FillCount"];
            lvi.Text = $"{number}";
            lvi.SubItems[1].Text = $"{ntype}";
            lvi.SubItems[2].Text = $"{realHeight}";
            lvi.SubItems[3].Text = $"{fillCount}";
        }
    }
}
