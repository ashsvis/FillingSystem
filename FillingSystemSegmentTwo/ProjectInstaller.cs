using System.ComponentModel;

namespace FillingSystemSegmentTwo
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void ProjectInstaller_BeforeInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            ServiceInstaller.ServiceName = FillingSystemFetchSegmentProgram.ServiceName;
            ServiceInstaller.DisplayName = FillingSystemFetchSegmentProgram.DisplayName;
        }
    }
}
