using FillingSystemHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace FillingSystemBrowser
{
    public partial class FillingSystemBrowserForm : Form
    {
#if (DEBUG)   
//      readonly List<BackgroundWorker> fetchers;
#endif
        public FillingSystemBrowserForm()
        {
            InitializeComponent();
#if (DEBUG)
            //fetchers = new List<BackgroundWorker>();
            //FetchingHelper.RunFetchers(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileNameWithoutExtension(Application.ExecutablePath));
#endif
        }

        private void FillingSystemBrowserForm_Load(object sender, EventArgs e)
        {
            var mif = new MemIniFile(Path.ChangeExtension(Application.ExecutablePath, ".ini"));
            ucFillingPage.LoadConfiguration(mif.ReadString("SqlServer", "ConnectionString", ""));
        }

        private void FillingSystemBrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
#if (DEBUG)
            //fetchers.ForEach(item => item.CancelAsync());
#endif
        }
    }
}
