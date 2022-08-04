using System;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class DialogYesNoUserControl : UserControl
    {
        public DialogYesNoUserControl(string message, string caption = null)
        {
            InitializeComponent();
            labMessage.Text = message;
            labCaption.Text = caption ?? "Требуется подтверждение";
        }

        private event EventHandler onNo;

        public event EventHandler OnNo
        {
            add
            {
                onNo += value;
            }
            remove
            {
                onNo -= value;
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            onNo?.Invoke(this, new EventArgs());
        }

        private event EventHandler onYes;

        public event EventHandler OnYes
        {
            add
            {
                onYes += value;
            }
            remove
            {
                onYes -= value;
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            onYes?.Invoke(this, new EventArgs());
        }
    }
}
