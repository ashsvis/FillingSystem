using System;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class DialogOkUserControl : UserControl
    {
        public DialogOkUserControl(string message, string caption = null)
        {
            InitializeComponent();
            labMessage.Text = message;
            labCaption.Text = caption ?? "Сообщение";
        }

        private event EventHandler onOk;

        public event EventHandler OnOk
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
            onOk?.Invoke(this, new EventArgs());
        }
    }
}
