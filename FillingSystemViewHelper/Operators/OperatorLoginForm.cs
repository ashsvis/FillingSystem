using FillingSystemHelper;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class OperatorLoginForm : Form
    {
        public OperatorLoginForm()
        {
            InitializeComponent();
        }

        public void Build(DataTable data)
        {
            lbError.Text = "";
            cbUser.Items.Clear();
            if (data == null) return;
            foreach (var row in data.Rows.Cast<DataRow>())
            {
                var oper = new OperatorData(
                (string)row["Lastname"],
                (string)row["Firstname"],
                (string)row["Secondname"],
                (int)row["Access"],
                (string)row["Department"],
                (string)row["Appointment"],
                (string)row["Password"]);
                cbUser.Items.Add(oper);
            }
            if (cbUser.Items.Count > 0)
            {
                cbUser.SelectedItem = cbUser.Items[0];
                cbUser_SelectionChangeCommitted(cbUser, EventArgs.Empty);
            }
        }

        private bool CheckPassword(string password, string lastname, string firstname, string secondname)
        {
            var args = new CheckPasswordEventArgs()
            {
                Lastname = lastname,
                Firstname = firstname,
                Secondname = secondname,
                Password = password,
                IsValid = false
            };
            onCheckPassword?.Invoke(this, args);
            return args.IsValid;
        }

        private event EventHandler onCancel;

        public event EventHandler OnCancel
        {
            add
            {
                onCancel += value;
            }
            remove
            {
                onCancel -= value;
            }
        }

        private void FormOperatorDataEditor_Load(object sender, EventArgs e)
        {
            if (Location == Point.Empty)
                CenterToScreen();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            onCancel?.Invoke(this, new EventArgs());
        }

        private event OperatorEventHandler onOk;

        public event OperatorEventHandler OnOk
        {
            add
            {
                onOk += value;
            }
            remove
            {
                onOk -= value;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cbUser.SelectedItem is OperatorData data &&
                CheckPassword(tbPassword.Text, data.Lastname, data.Firstname, data.Secondname))
            {
                lbError.Text = "";
                onOk?.Invoke(this, new OperatorEventArgs()
                {
                    Lastname = data.Lastname,
                    Firstname = data.Firstname,
                    Secondname = data.Secondname,
                    Access = data.Access,
                    Department = data.Department,
                    Appointment = data.Appointment,
                    Password = data.Password
                });
            }
            else
                lbError.Text = string.IsNullOrWhiteSpace(tbPassword.Text) ? "пароль не введён" : "ошибка пароля";
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

        private void OperatorTypeDataEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            onCloseForm?.Invoke(this, new CloseFormEventArgs() { Location = this.Location });
        }

        private void cbUser_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var oper = (OperatorData)cbUser.SelectedItem;
            lbDeparment.Text = oper.Department;
            lbAppointment.Text = oper.Appointment;
            lbAccess.Text = OperatorData.GetNameByCode(oper.Access);
        }
    }
}
