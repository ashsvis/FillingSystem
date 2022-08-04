using FillingSystemHelper;
using System.ServiceProcess;

namespace FillingSystemSegmentTwo
{
    static class FillingSystemFetchSegmentProgram
    {
        public static string ServiceName { get; internal set; } = "FillingSystemSegmentOne";
        public static string DisplayName { get; internal set; } = "FillingSystemSegmentOne";

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
