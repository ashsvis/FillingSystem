using FillingSystemHelper;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class TaskDataEditorForm : Form
    {
        private int ntype;
        private RiserKey riserKey;
        private int deepLevel;
        private int workRange;

        public TaskDataEditorForm(RiserKey riserKey)
        {
            InitializeComponent();
            this.riserKey = riserKey;
            Text = $"Задание налива [ Стояк {riserKey.Riser} ]";
            lbDiameter.Text = "";
            lbThroat.Text = "";
            lbMaximum.Text = "";
            lbMinimum.Text = "";
            lbMessage.Text = "";
        }

        public void Build(TaskData taskData)
        {
            tbNumber.Text = taskData.Number;
            FillWagonTypes();
            this.ntype = taskData.NType;
            this.deepLevel = taskData.DeepLevel;
            this.workRange = taskData.WorkRange;
            cbNtype.SelectedItem = cbNtype.Items.Cast<int>().FirstOrDefault(item => item == ntype);
            tbRealHeight.Text = taskData.RealHeight > 0 ? $"{taskData.RealHeight}" : string.Empty; 
            tbSetpoint.Text = taskData.Setpoint > 0 ? $"{taskData.Setpoint}" : string.Empty;
            cbNtype.Enabled = taskData.NType == 0;
            lbNtype.Enabled = taskData.NType == 0;
            tbRealHeight.Enabled = taskData.NType == 0;
            lbRealHeight.Enabled = taskData.NType == 0;
            var argt = new WagonTypeEventArgs() { Ntype = ntype };
            if (ntype > 0)
            {
                onFindWagonType?.Invoke(this, argt);
            }
            else
            {
                lbDiameter.Text = string.Empty;
                lbThroat.Text = string.Empty;
            }
            UpdateLevelRange();
            tbSetpoint_Validated(null, null);
        }

        public void Update(int nType, int diameter, int throat, int deflevel, int deepLevel, int workRange)
        {
            this.deepLevel = deepLevel;
            this.workRange = workRange;
            if (nType > 0)
            {
                lbDiameter.Text = $"{diameter}";
                lbThroat.Text = $"{throat}";
                tbSetpoint.Text = $"{deflevel}";
            }
            else
            {
                lbDiameter.Text = string.Empty;
                lbThroat.Text = string.Empty;
                tbRealHeight.Text = string.Empty;
                tbSetpoint.Text = string.Empty;
            }
            UpdateLevelRange();
        }

        private void UpdateLevelRange()
        {
            var value = GetValue;
            if (value == null)
            {
                lbMaximum.Text = string.Empty;
                lbMinimum.Text = string.Empty;
                return;
            }
            var maxHeight = value.RealHeight > 0 ? value.RealHeight : value.Diameter + value.Throat;
            lbMinimum.Text = $"{maxHeight - deepLevel}";
            lbMaximum.Text = $"{maxHeight - deepLevel + workRange}";
        }

        private bool CheckData()
        {
            var value = GetValue;
            return value != null &&
                int.TryParse(lbMinimum.Text, out int min) &&
                int.TryParse(lbMaximum.Text, out int max) &&
                value.Setpoint >= min && value.Setpoint <= max;
        }

        private TaskData GetValue
        {
            get
            {
                if (tbNumber.Text.Length == 8 &&
                    int.TryParse(tbNumber.Text, out _) &&
                    cbNtype.SelectedItem != null &&
                    int.TryParse(tbRealHeight.Text, out int realHeight) &&
                    realHeight >= 2800 && realHeight <= 3400 &&
                    int.TryParse(tbSetpoint.Text, out int setpoint) &&
                    setpoint > 0)
                {
                    return new TaskData(tbNumber.Text, (int)cbNtype.SelectedItem, realHeight, setpoint);
                }
                return null;
            }
        }

        private void tbNumber_Validated(object sender, EventArgs e)
        {
            if (tbNumber.Text.Length == 8 && int.TryParse(tbNumber.Text, out _))
            {
                lbMessage.Text = string.Empty;
                var arg = new WagonEventArgs() { Number = tbNumber.Text };
                onFindWagon?.Invoke(this, arg);
            }
            else
                lbMessage.Text = "Ожидалось восьмизначное число номера вагона";
        }

        private event WagonEventHandler onFindWagon;

        public event WagonEventHandler OnFindWagon
        {
            add
            {
                onFindWagon += value;
            }
            remove
            {
                onFindWagon -= value;
            }
        }

        private void tbRealHeight_Validated(object sender, EventArgs e)
        {
            if (int.TryParse(tbRealHeight.Text, out int height) &&
                height >= 2800 && height <= 3400)
            {
                lbMessage.Text = string.Empty;
                UpdateLevelRange();
            }
            else
                lbMessage.Text = "Ожидалась факт.высота в диапазоне [2800..3400] мм";
        }

        private void tbSetpoint_Validated(object sender, EventArgs e)
        {
            if (int.TryParse(tbSetpoint.Text, out int setpoint) &&
                int.TryParse(lbMinimum.Text, out int min) &&
                int.TryParse(lbMaximum.Text, out int max) &&
                setpoint >= min && setpoint <= max)
                lbMessage.Text = string.Empty;
            else
                if (!string.IsNullOrWhiteSpace(lbMinimum.Text) && !string.IsNullOrWhiteSpace(lbMaximum.Text))
                    lbMessage.Text = $"Ожидалось задание в диапазоне [{lbMinimum.Text}..{lbMaximum.Text}] мм";
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

        private event TaskEventHandler onOk;

        public event TaskEventHandler OnOk
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
            if (!CheckData()) return;
            var data = GetValue;
            if (data == null) return;
            onOk?.Invoke(this, new TaskEventArgs()
            {
                Overpass = riserKey.Overpass,
                Way = riserKey.Way,
                Product = riserKey.Product,
                Riser = riserKey.Riser,
                Number = data.Number,
                Ntype = data.NType,
                RealHeight = data.RealHeight,
                Setpoint = data.Setpoint
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

        private event WagonTypeEventHandler onFindWagonType;

        public event WagonTypeEventHandler OnFindWagonType
        {
            add
            {
                onFindWagonType += value;
            }
            remove
            {
                onFindWagonType -= value;
            }
        }

        private void cbNtype_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ntype = cbNtype.SelectedItem != null ? (int)cbNtype.SelectedItem : 0;
            if (cbNtype.SelectedItem != null)
            {
                var arg = new WagonTypeEventArgs() { Ntype = ntype };
                onFindWagonType?.Invoke(this, arg);
                tbRealHeight_Validated(null, null);
            }
        }

        private void tbNumber_Enter(object sender, EventArgs e)
        {
            cbNtype.Enabled = true;
            lbNtype.Enabled = true;
            tbRealHeight.Enabled = true;
            lbRealHeight.Enabled = true;
        }

    }
}
