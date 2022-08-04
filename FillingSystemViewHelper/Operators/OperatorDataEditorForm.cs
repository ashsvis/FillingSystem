using FillingSystemHelper;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class OperatorDataEditorForm : Form
    {
        private int access;

        public OperatorDataEditorForm()
        {
            InitializeComponent();
        }

        public void Build(bool edit, string lastname, string firstname, string secondname, int access, string department, string appointment, string password)
        {
            Text = edit ? "Редактировать пользователя" : "Новый пользователь";
            tbLastname.Text = lastname;
            tbLastname.Enabled = !edit;
            tbFirstname.Text = firstname;
            tbFirstname.Enabled = !edit;
            tbSecondname.Text = secondname;
            tbSecondname.Enabled = !edit;
            FillOperatorTypes();
            cbAccess.SelectedItem = cbAccess.Items.Cast<OperatorAccess>().FirstOrDefault(item => item.Code == access);
            cbAccess.Enabled = !edit;
            tbDepartment.Text = department;
            tbDepartment.Enabled = !edit;
            tbAppointment.Text = appointment;
            tbAppointment.Enabled = !edit;
            tbOldPassword.Text = !edit ? password : "";
            lbOldPassword.Visible = edit;
            tbOldPassword.Visible = edit;
            tbNewPassword.Text = !edit ? password : "";
            tbCheckPassword.Text = !edit ? password : "";
            this.access = 0;
            cbAccess.SelectedItem = cbAccess.Items.Cast<OperatorAccess>().FirstOrDefault(item => item.Code == access);
        }

        private void tbFillCount_TextChanged(object sender, EventArgs e)
        {
            CheckData();
        }

        private bool CheckData()
        {
            tbText_Validated(tbLastname, EventArgs.Empty);
            tbText_Validated(tbFirstname, EventArgs.Empty);
            tbText_Validated(tbSecondname, EventArgs.Empty);
            cbAccess_Validated(cbAccess, EventArgs.Empty);
            tbText_Validated(tbDepartment, EventArgs.Empty);
            tbText_Validated(tbAppointment, EventArgs.Empty);
            tbText_Validated(tbOldPassword, EventArgs.Empty);
            tbText_Validated(tbNewPassword, EventArgs.Empty);
            tbText_Validated(tbCheckPassword, EventArgs.Empty);
            if (!string.IsNullOrWhiteSpace(tbLastname.Text) &&
                !string.IsNullOrWhiteSpace(tbFirstname.Text) &&
                !string.IsNullOrWhiteSpace(tbSecondname.Text) &&
                cbAccess.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(tbDepartment.Text) &&
                !string.IsNullOrWhiteSpace(tbAppointment.Text) &&
                !string.IsNullOrWhiteSpace(tbNewPassword.Text) &&
                !string.IsNullOrWhiteSpace(tbCheckPassword.Text) &&
                tbNewPassword.Text == tbCheckPassword.Text)
            {
                if (tbOldPassword.Visible && !CheckOldPassword(tbOldPassword.Text, tbLastname.Text, tbFirstname.Text, tbSecondname.Text))
                {
                    errorProvider1.SetError(tbOldPassword, "Старый пароль не подходит");
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckOldPassword(string password, string lastname, string firstname, string secondname)
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

        private OperatorData GetValue
        {
            get
            {
                if (CheckData())
                    return new OperatorData(tbLastname.Text, tbFirstname.Text, tbSecondname.Text,
                        ((OperatorAccess)cbAccess.SelectedItem).Code, tbDepartment.Text, tbAppointment.Text, tbNewPassword.Text);
                return null;
            }
        }

        private void tbText_Validated(object sender, EventArgs e)
        {
            var tbox = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(tbox.Text))
            {
                if ((tbox == tbNewPassword || tbox == tbCheckPassword) && tbNewPassword.Text != tbCheckPassword.Text)
                    errorProvider1.SetError(tbox, "Новый пароль и проверочный пароль не совпадают");
                else
                    errorProvider1.SetError(tbox, string.Empty);
            }
            else
                errorProvider1.SetError(tbox, "Ожидалось непустое значение");
        }

        private void cbAccess_Validated(object sender, EventArgs e)
        {
            var cbox = (ComboBox)sender;
            if (cbox.SelectedItem != null)
                errorProvider1.SetError(cbox, string.Empty);
            else
                errorProvider1.SetError(cbox, "Ожидалось непустое значение");
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
            if (CheckData())
            {
                var data = GetValue;
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

        private void cbAccess_DropDown(object sender, EventArgs e)
        {
            FillOperatorTypes();
            cbAccess.Text = $"{OperatorData.Operators.Cast<OperatorAccess>().FirstOrDefault(item => item.Code == access)}";
        }

        private void FillOperatorTypes()
        {
            cbAccess.Items.Clear();
            cbAccess.Items.AddRange(OperatorData.Operators);
        }

        private void cbAccess_SelectionChangeCommitted(object sender, EventArgs e)
        {
            access = cbAccess.SelectedItem != null ? ((OperatorAccess)cbAccess.SelectedItem).Code : 0;
            CheckData();
        }
    }
}
