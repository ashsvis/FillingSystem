using FillingSystemHelper;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class WagonTypeUserControl : UserControl
    {
        private int _rowIndex = -1;
        private DataTable table;
        private Point editorFormLocation;

        public WagonTypeUserControl()
        {
            InitializeComponent();
            table = new DataTable();
            table.Columns.Add(new DataColumn("Тип", typeof(int)));
            table.Columns.Add(new DataColumn("Диаметр", typeof(int)));
            table.Columns.Add(new DataColumn("Высота горловины", typeof(int)));
            table.Columns.Add(new DataColumn("Взлив по умолчанию", typeof(int)));
            dataGridView1.DataSource = table;
        }

        private void WagonTypeUserControl_Load(object sender, EventArgs e)
        {
            UpdateWaggonTypesList();
        }

        public void UpdateWaggonTypesList(int rowindex = -1)
        {
            if (rowindex >= 0 && dataGridView1.Rows.Count > 0 && rowindex < dataGridView1.Rows.Count)
                dataGridView1.CurrentCell = dataGridView1[0, rowindex];
        }

        public void Build(DataTable data, int ntype = -1)
        {
            table.Rows.Clear();
            if (data == null) return;
            foreach (var row in data.Rows.Cast<DataRow>())
            {
                var nType = (int)row["Ntype"];
                var diameter = (int)row["Diameter"];
                var throat = (int)row["Throat"];
                var deflevel = (int)row["Deflevel"];
                table.Rows.Add(nType, diameter, throat, deflevel);
            }
            if (ntype >= 0)
            {
                for (var i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (ntype == (int)dataGridView1[0, i].Value)
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

        private void miAddWaggonType_Click(object sender, EventArgs e)
        {
            ShowWaggonTypeInsertDialog();
        }

        private void miEditWaggonType_Click(object sender, EventArgs e)
        {
            ShowWaggonTypeEditDialog(_rowIndex);
        }

        private void miDeleteWaggonType_Click(object sender, EventArgs e)
        {
            ShowWaggonTypeDeleteDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (!CommonData.EnteredAsAdmin())
            {
                e.Cancel = true;
                return;
            }
            miEditWaggonType.Enabled = dataGridView1.Rows.Count > 0;
            miDeleteWaggonType.Enabled = dataGridView1.Rows.Count > 0;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CommonData.EnteredAsAdmin())
                ShowWaggonTypeEditDialog(e.RowIndex);
        }

        private void ShowWaggonTypeInsertDialog()
        {
            CloseEditorForm();
            var editorForm = new WagonTypeDataEditorForm() { Location = editorFormLocation };
            editorForm.Update(false, 0, 0, 0, 0);
            editorForm.Show(this);

            editorForm.OnCancel += (o, e) => editorForm.Close();

            editorForm.OnOk += (o, e) =>
            {
                onCreate?.Invoke(this, new WagonTypeEventArgs()
                {
                    Ntype = e.Ntype,
                    Diameter = e.Diameter,
                    Throat = e.Throat,
                    DefLevel = e.DefLevel
                });
                UpdateWaggonTypesList();
                editorForm.Close();
            };

            editorForm.OnCloseForm += (o, e) => editorFormLocation = e.Location;
        }

        private event WagonTypeEventHandler onCreate;

        public event WagonTypeEventHandler OnCreate
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

        private void ShowWaggonTypeEditDialog(int rowIndex)
        {
            CloseEditorForm();
            if (rowIndex >= 0 && dataGridView1.Rows.Count > 0 && rowIndex < dataGridView1.Rows.Count)
            {
                var ntype = (int)dataGridView1[0, rowIndex].Value;
                var diameter = (int)dataGridView1[1, rowIndex].Value;
                var throat = (int)dataGridView1[2, rowIndex].Value;
                var defLevel = (int)dataGridView1[3, rowIndex].Value;

                var editorForm = new WagonTypeDataEditorForm() { Location = editorFormLocation };
                editorForm.Update(true, ntype, diameter, throat, defLevel);
                editorForm.Show(this);

                editorForm.OnCancel += (o, e) => editorForm.Close();


                editorForm.OnOk += (o, e) =>
                {
                    onChange?.Invoke(this, new WagonTypeEventArgs()
                    {
                        Ntype = e.Ntype,
                        Diameter = e.Diameter,
                        Throat = e.Throat,
                        DefLevel = e.DefLevel
                    });
                    UpdateWaggonTypesList(rowIndex);
                    editorForm.Close();
                };

                editorForm.OnCloseForm += (o, e) => editorFormLocation = e.Location;
            }
        }

        private event WagonTypeEventHandler onChange;

        public event WagonTypeEventHandler OnChange
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

        private void ShowWaggonTypeDeleteDialog()
        {
            if (_rowIndex < 0) return;
            var ntype = (int)dataGridView1[0, _rowIndex].Value;
            onDelete?.Invoke(this, new WagonTypeEventArgs()
            {
                Ntype = ntype
            });
        }

        private event WagonTypeEventHandler onDelete;

        public event WagonTypeEventHandler OnDelete
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

        private void WagonTypeUserControl_Leave(object sender, EventArgs e)
        {
            CloseEditorForm();
        }

        private static void CloseEditorForm()
        {
            var editorForm = Application.OpenForms.OfType<WagonTypeDataEditorForm>().FirstOrDefault();
            if (editorForm != null)
                editorForm.Close();
        }
    }
}
