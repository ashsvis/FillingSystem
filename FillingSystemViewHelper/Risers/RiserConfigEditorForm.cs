using FillingSystemHelper;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class RiserConfigEditorForm : Form
    {
        public RiserConfigEditorForm()
        {
            InitializeComponent();
            riserConfigUserControl1.OnCancel += (o, e) => onCancel.Invoke(o, e);
            riserConfigUserControl1.OnOk += (o, e) => onOk.Invoke(o, e);
        }

        private void RiserConfigEditorForm_Load(object sender, EventArgs e)
        {
            if (Location == Point.Empty)
                CenterToScreen();
        }

        public void Build(Operations kind, int overpass, int way, string product, string ipAddr)
        {
            riserConfigUserControl1.Update(kind, overpass, way, product, ipAddr);

        }

        public void Build(Operations kind, int overpass, int way, string product, int riser, string ipAddr, int ipPort, int node, int func)
        {
            riserConfigUserControl1.Update(kind, overpass, way, product, riser, ipAddr, ipPort, node, func);
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

        private void RiserConfigEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            onCloseForm?.Invoke(this, new CloseFormEventArgs() { Location = this.Location });
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

        private event RiserConfigEventHandler onOk;

        public event RiserConfigEventHandler OnOk
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
    }
}
