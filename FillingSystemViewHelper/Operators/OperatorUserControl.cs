using FillingSystemHelper;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class OperatorUserControl : UserControl
    {
        private DataTable data;
        private DataTable table;
        private Point editorFormLocation;

        public OperatorUserControl()
        {
            InitializeComponent();
            table = new DataTable();
            table.Columns.Add(new DataColumn("Доступ", typeof(string)));
            table.Columns.Add(new DataColumn("Фамилия", typeof(string)));
            table.Columns.Add(new DataColumn("Имя", typeof(string)));
            table.Columns.Add(new DataColumn("Отчество", typeof(string)));
            table.Columns.Add(new DataColumn("Отдел", typeof(string)));
            table.Columns.Add(new DataColumn("Должность", typeof(string)));
            dataGridView1.DataSource = table;
        }

        public void Build(DataTable data, string lAstname = null, string fIrstname = null, string sEcondname = null)
        {
            this.data = data;
            table.Rows.Clear();
            if (data == null) return;
            foreach (var row in data?.Rows.Cast<DataRow>())
            {
                var access = (int)row["Access"];
                var lastname = (string)row["Lastname"];
                var firstname = (string)row["Firstname"];
                var secondname = (string)row["Secondname"];
                var department = (string)row["Department"];
                var appointment = (string)row["Appointment"];
                table.Rows.Add(AccessCodeToText(access), lastname, firstname, secondname, department, appointment);
            }
            if (lAstname != null && fIrstname != null && sEcondname != null)
            {
                for (var i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (lAstname == (string)dataGridView1[1, i].Value &&
                        fIrstname == (string)dataGridView1[2, i].Value &&
                        sEcondname == (string)dataGridView1[3, i].Value)
                    {
                        dataGridView1.CurrentCell = dataGridView1[0, i];
                        break;
                    }
                }
            }
        }

        private string AccessCodeToText(int access)
        {
            switch (access)
            {
                case 0:
                    return "Оператор";
                case 1:
                    return "Технолог";
                case 2:
                    return "Администратор";
            }
            return "(нет данных)";
        }

        private void miAddOperator_Click(object sender, EventArgs e)
        {
            ShowOperatorInsertDialog();
        }

        private void miEditOperator_Click(object sender, EventArgs e)
        {
            ShowOperatorEditDialog();
        }

        private void miDeleteOperator_Click(object sender, EventArgs e)
        {
            ShowOperatorDeleteDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (!CommonData.EnteredAsAdmin())
            {
                e.Cancel = true;
                return;
            }
            miEditOperator.Enabled = dataGridView1.Rows.Count > 0;
            miDeleteOperator.Enabled = dataGridView1.Rows.Count > 0;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CommonData.EnteredAsAdmin())
                ShowOperatorEditDialog();
        }

        private void ShowOperatorInsertDialog()
        {
            CloseEditorForm();
            var editorForm = new OperatorDataEditorForm() { Location = editorFormLocation };
            editorForm.Build(false, "", "", "", 0, "", "", "");
            editorForm.Show(this);

            editorForm.OnCancel += (o, e) => editorForm.Close();

            editorForm.OnOk += (o, e) =>
            {
                onCreate?.Invoke(this, new OperatorEventArgs()
                {
                    Lastname = e.Lastname,
                    Firstname = e.Firstname,
                    Secondname = e.Secondname,
                    Access = e.Access,
                    Department = e.Department,
                    Appointment = e.Appointment,
                    Password = e.Password
                });
                editorForm.Close();
            };

            editorForm.OnCloseForm += (o, e) => editorFormLocation = e.Location;

        }

        private event OperatorEventHandler onCreate;

        public event OperatorEventHandler OnCreate
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

        private void ShowOperatorEditDialog()
        {
            CloseEditorForm();
            if (dataGridView1.CurrentRow != null)
            {
                var row = (dataGridView1.CurrentRow.DataBoundItem as DataRowView).Row;
                var lastname = row["Фамилия"].ToString();
                var firstname = row["Имя"].ToString();
                var secondname = row["Отчество"].ToString();

                var dataRow = data.Rows.Cast<DataRow>().FirstOrDefault(item => 
                     (string)item["Lastname"] == lastname && (string)item["Firstname"] == firstname && (string)item["Secondname"] == secondname);
                if (dataRow != null)
                {
                    var editorForm = new OperatorDataEditorForm() { Location = editorFormLocation };
                    var access = (int)dataRow["Access"];
                    var department = (string)dataRow["Department"];
                    var appointment = (string)dataRow["Appointment"];
                    var password = (string)dataRow["Password"];
                    editorForm.Build(true, lastname, firstname, secondname, access, department, appointment, password);
                    editorForm.Show(this);

                    editorForm.OnCancel += (o, e) => editorForm.Close();


                    editorForm.OnOk += (o, e) =>
                    {
                        onChange?.Invoke(this, new OperatorEventArgs()
                        {
                            Lastname = e.Lastname,
                            Firstname = e.Firstname,
                            Secondname = e.Secondname,
                            Access = e.Access,
                            Department = e.Department,
                            Appointment = e.Appointment,
                            Password = e.Password
                        });
                        editorForm.Close();
                    };

                    editorForm.OnCheckPassword += (o, e) => onCheckPassword?.Invoke(o, e);

                    editorForm.OnCloseForm += (o, e) => editorFormLocation = e.Location;
                }
            }
        }

        private event OperatorEventHandler onChange;

        public event OperatorEventHandler OnChange
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

        private void ShowOperatorDeleteDialog()
        {
            if (dataGridView1.CurrentRow != null)
            {
                var row = (dataGridView1.CurrentRow.DataBoundItem as DataRowView).Row;
                onDelete?.Invoke(this, new OperatorEventArgs()
                {
                    Lastname = row["Фамилия"].ToString(),
                    Firstname = row["Имя"].ToString(),
                    Secondname = row["Отчество"].ToString(),
                    Access = OperatorData.GetCodeByName(row["Доступ"].ToString())
                }); ;
            }
        }

        private event OperatorEventHandler onDelete;

        public event OperatorEventHandler OnDelete
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

        private event CheckPasswordEventHandler onCheckPassword;

        public event CheckPasswordEventHandler OnCheckPassword
        {
            add
            {
                onCheckPassword += value;
            }
            remove
            {
                onCheckPassword -= value;
            }
        }

        private void OperatorUserControl_Leave(object sender, EventArgs e)
        {
            CloseEditorForm();
        }

        private static void CloseEditorForm()
        {
            var editorForm = Application.OpenForms.OfType<OperatorDataEditorForm>().FirstOrDefault();
            if (editorForm != null)
                editorForm.Close();
        }
    }
}
