using System;
using System.IO;

namespace FillingSystemHelper
{
    public static class LogReport
    {
        private static string logName = "LogReport";
        public static string LogName { get { return logName; } set { logName = value; } }

        private static string _message = null;

        public static void AppendToLog(Exception ex)
        {
            if (ex == null) return;
            AppendToLog(ex.Message);
            AppendToLog(ex.StackTrace);
            AppendToLog(ex.InnerException);
        }

        private static readonly object logLocker = new object();

        public static void AppendToLog(string message)
        {
            string templateBasePath;
            if (Environment.UserInteractive)
                templateBasePath = AppDomain.CurrentDomain.BaseDirectory;
            else
            {
                templateBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), logName);
                if (!Directory.Exists(templateBasePath)) Directory.CreateDirectory(templateBasePath);
            }
            var path = Path.Combine(templateBasePath, $"{logName}.{DateTime.Now:yyyy-MM-dd}.log");

            // This text is added only once to the file. 
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Log created at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    sw.WriteLine("-----------------------------------");
                }
            }

            if (_message == message) return;
            _message = message;

            lock (logLocker)
            {
                // This text is always added, making the file longer over time 
                // if it is not deleted. 
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss}\t{message}");
                }
            }
        }

    }
}
