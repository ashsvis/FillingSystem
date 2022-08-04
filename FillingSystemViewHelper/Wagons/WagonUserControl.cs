using FillingSystemHelper;
using System;
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
        private int _rowIndex = -1;
        private DataTable table;
        private Point editorFormLocation;

        public WagonUserControl()
        {
            InitializeComponent();
            table = new DataTable();
            table.Columns.Add(new DataColumn("Номер вагона-цистерны", typeof(string)));
            table.Columns.Add(new DataColumn("Тип", typeof(int)));
            table.Columns.Add(new DataColumn("Фактическая высота", typeof(int)));
            table.Columns.Add(new DataColumn("Количество наливов", typeof(int)));
            dataGridView1.DataSource = table;
        }

        private void WagonUserControl_Load(object sender, EventArgs e)
        {
            UpdateWaggonList();
        }

        public void UpdateWaggonList(int rowindex = -1)
        {
            if (rowindex >= 0 && dataGridView1.Rows.Count > 0 && rowindex < dataGridView1.Rows.Count)
                dataGridView1.CurrentCell = dataGridView1[0, rowindex];
        }

        public async Task BuildAsync(DataTable data, string nUmber = null)
        {
            table.Rows.Clear();
            if (data == null) return;
            await Task.Run(() => 
            {
                foreach (var row in data.Rows.Cast<DataRow>())
                {
                    var number = (string)row["Number"];
                    var nType = (int)row["NType"];
                    var realHeight = (int)row["RealHeight"];
                    var fillCount = (int)row["FillCount"];
                    table.Rows.Add(number, nType, realHeight, fillCount);
                }
            });
            if (nUmber != null)
            {
                for (var i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (nUmber == (string)dataGridView1[0, i].Value)
                    {
                        dataGridView1.CurrentCell = dataGridView1[0, i];
                        break;
                    }
                }
            }
        }

        public void Build(DataTable data, string nUmber = null)
        {
            table.Rows.Clear();
            if (data == null) return;
            foreach (var row in data.Rows.Cast<DataRow>())
            {
                var number = (string)row["Number"];
                var nType = (int)row["NType"];
                var realHeight = (int)row["RealHeight"];
                var fillCount = (int)row["FillCount"];
                table.Rows.Add(number, nType, realHeight, fillCount);
            }
            if (nUmber != null)
            {
                for (var i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (nUmber == (string)dataGridView1[0, i].Value)
                    {
                        dataGridView1.CurrentCell = dataGridView1[0, i];
                        break;
                    }
                }
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            CloseEditorForm();
            _rowIndex = e.RowIndex;
        }

        private void miAddWaggon_Click(object sender, EventArgs e)
        {
            ShowWaggonInsertDialog();
        }

        private void miEditWaggon_Click(object sender, EventArgs e)
        {
            ShowWaggonEditDialog(_rowIndex);
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
            miEditWaggon.Enabled = dataGridView1.Rows.Count > 0;
            miDeleteWaggon.Enabled = dataGridView1.Rows.Count > 0;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CommonData.EnteredAsAdmin())
                ShowWaggonEditDialog(e.RowIndex);
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
                onCreate?.Invoke(this, new WagonEventArgs()
                {
                    Number = e.Number,
                    Ntype = e.Ntype,
                    RealHeight = e.RealHeight
                });
                UpdateWaggonList();
                editorForm.Close();
            };

            editorForm.OnCloseForm += (o, e) => editorFormLocation = e.Location;

        }

        private event WagonEventHandler onCreate;

        public event WagonEventHandler OnCreate
        {
            add
            {
                onCreate += value;
            }
            remove
            {
                onCreate -= value;
            }
        }

        private void ShowWaggonEditDialog(int rowIndex)
        {
            CloseEditorForm();
            if (rowIndex >= 0 && dataGridView1.Rows.Count > 0 && rowIndex < dataGridView1.Rows.Count)
            {
                var number = (string)dataGridView1[0, rowIndex].Value;
                var ntype = (int)dataGridView1[1, rowIndex].Value;
                var realHeight = (int)dataGridView1[2, rowIndex].Value;
                var fillCount = (int)dataGridView1[3, rowIndex].Value;

                var editorForm = new WagonDataEditorForm() { Location = editorFormLocation };
                editorForm.OnGetWagonTypes += (o, e) => onGetWagonTypes?.Invoke(o, e);
                editorForm.Update(true, number, ntype, realHeight, fillCount);
                editorForm.Show(this);

                editorForm.OnCancel += (o, e) => editorForm.Close();


                editorForm.OnOk += (o, e) =>
                {
                    onChange?.Invoke(this, new WagonEventArgs()
                    {
                        Number = e.Number,
                        Ntype = e.Ntype,
                        RealHeight = e.RealHeight
                    });
                    UpdateWaggonList(rowIndex);
                    editorForm.Close();
                };

                editorForm.OnCloseForm += (o, e) => editorFormLocation = e.Location;

            }
        }

        private event WagonEventHandler onChange;

        public event WagonEventHandler OnChange
        {
            add
            {
                onChange += value;
            }
            remove
            {
                onChange -= value;
            }
        }

        private void ShowWaggonDeleteDialog()
        {
            if (_rowIndex < 0) return;
            var number = (string)dataGridView1[0, _rowIndex].Value;
            onDelete?.Invoke(this, new WagonEventArgs()
            {
                Number = number
            });
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
    }
}
