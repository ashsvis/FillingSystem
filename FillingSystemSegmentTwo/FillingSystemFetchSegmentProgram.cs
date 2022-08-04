using FillingSystemHelper;
using System.ServiceProcess;

namespace FillingSystemSegmentTwo
{
    static class FillingSystemFetchSegmentProgram
    {
        public static string ServiceName { get; internal set; } = "FillingSystemSegmentTwo";
        public static string DisplayName { get; internal set; } = "FillingSystemSegmentTwo";

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
