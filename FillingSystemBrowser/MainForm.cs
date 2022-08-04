using System;
using System.Drawing;
using System.Windows.Forms;

namespace FillingSystemBrowser
{
    public partial class MainForm : Form
    {
        private FillingSystemBrowserForm[] panels;
        private bool masterClose;
        private bool closeWithoutQuestions;

        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        /// <summary>
        /// Прячем главную форму из панели диспетчера задач
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Size = new Size();
            Location = new Point(-2, -2);

            Screen[] monitors = Screen.AllScreens;
            panels = new FillingSystemBrowserForm[monitors.Length];
            for (int i = 0; i < monitors.Length; i++)
            {
                var panel = new FillingSystemBrowserForm() //this
                {
                    Location = monitors[i].Bounds.Location,
                    Size = monitors[i].Bounds.Size,
                    StartPosition = FormStartPosition.Manual,
                    WindowState = FormWindowState.Maximized,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    FormBorderStyle = FormBorderStyle.None
                };
                panels[i] = panel;
                panel.Show();
            }
        }
    }
}
