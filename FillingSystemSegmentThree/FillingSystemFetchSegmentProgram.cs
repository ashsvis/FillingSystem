using FillingSystemHelper;
using System.ServiceProcess;

namespace FillingSystemSegmentThree
{
    static class FillingSystemFetchSegmentProgram
    {
        public static string ServiceName { get; internal set; } = "FillingSystemSegmentThree";
        public static string DisplayName { get; internal set; } = "FillingSystemSegmentThree";

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            LogReport.LogName = ServiceName;
            var servicesToRun = new ServiceBase[] { new WinService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
