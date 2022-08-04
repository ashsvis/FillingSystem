using FillingSystemView;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class WagonDataEditorForm : Form
    {
        private int ntype;

        public WagonDataEditorForm()
        {
            InitializeComponent();
        }

        public void Update(bool edit, string number, int ntype, int realHeight, int fillCount)
        {
            Text = edit ? "Редактировать цистерну" : "Новая цистерна";
            tbNumber.Text = number;
            tbNumber.Enabled = !edit;
            lbNumber.Enabled = !edit;
            FillWagonTypes();
            this.ntype = ntype;
            cbNtype.SelectedItem = cbNtype.Items.Cast<int>().FirstOrDefault(item => item == ntype);
            tbRealHeight.Text = realHeight > 0 ? realHeight.ToString("0") : "";
            tbFillCount.Text = fillCount > 0 ? fillCount.ToString("0") : "0";
        }

        private void tbNtype_TextChanged(object sender, EventArgs e)
        {
            CheckData();
        }

        private void tbFillCount_TextChanged(object sender, EventArgs e)
        {
            CheckData();
        }

        private void CheckData()
        {
            if (tbNumber.Text.Length == 8 &&
                int.TryParse(tbNumber.Text, out _) &&
                cbNtype.SelectedItem != null &&
                int.TryParse(tbRealHeight.Text, out _))
            {
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = false;
            }
        }

        private WagonData GetValue
        {
            get
            {
                if (tbNumber.Text.Length == 8 &&
                    int.TryParse(tbNumber.Text, out _) &&
                    cbNtype.SelectedItem != null &&
                    int.TryParse(tbRealHeight.Text, out int realHeight) &&
                    realHeight >= 2800 && realHeight <= 3400)
                {
                    return new WagonData(tbNumber.Text, (int)cbNtype.SelectedItem, realHeight);
                }
                return null;
            }
        }

        private void tbNumber_Validated(object sender, EventArgs e)
        {
            if (tbNumber.Text.Length == 8 &&
                int.TryParse(tbNumber.Text, out _))
                errorProvider1.SetError(tbNumber, string.Empty);
            else
                errorProvider1.SetError(tbNumber, "Ожидалось восьмизначное число номера вагона");
        }

        private void tbRealHeight_Validated(object sender, EventArgs e)
        {
            int throat;
            if (int.TryParse(tbRealHeight.Text, out throat) &&
                throat >= 2800 && throat <= 3400)
                errorProvider1.SetError(tbRealHeight, string.Empty);
            else
                errorProvider1.SetError(tbRealHeight, "Ожидалось значение в диапазоне [2800..3400] мм");
        }

        private void tbFillCount_Validated(object sender, EventArgs e)
        {

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

        private void FormWagonDataEditor_Load(object sender, EventArgs e)
        {
            if (Location == Point.Empty)
                CenterToScreen();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            onCancel?.Invoke(this, new EventArgs());
        }

        private event WagonEventHandler onOk;

        public event WagonEventHandler OnOk
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
            var data = GetValue;
            onOk?.Invoke(this, new WagonEventArgs()
            {
                Number = data.Number,
                Ntype = data.NType,
                RealHeight = data.RealHeight,
            });
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

        private void WagonTypeDataEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            onCloseForm?.Invoke(this, new CloseFormEventArgs() { Location = this.Location });
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


        private void cbNtype_DropDown(object sender, EventArgs e)
        {
            FillWagonTypes();
            cbNtype.Text = $"{ntype}";
        }

        private void FillWagonTypes()
        {
            var arg = new GetWagonTypesEventArgs();
            onGetWagonTypes?.Invoke(this, arg);
            cbNtype.Items.Clear();
            foreach (var item in arg.WagonTypes)
            {
                cbNtype.Items.Add(item);
            }
        }

        private void cbNtype_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ntype = cbNtype.SelectedItem != null ? (int)cbNtype.SelectedItem : 0;
            CheckData();
        }
    }
}
