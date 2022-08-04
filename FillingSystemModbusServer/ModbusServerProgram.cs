using FillingSystemHelper;
using ModbusIntegratorEvent;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.ServiceProcess;

namespace FillingSystemModbusServer
{
    class ModbusServerProgram
    {
        static void Main(string[] args)
        {
            #region Защита от повторного запуска
            var process = RunningInstance();
            if (process != null) return;
            #endregion
            LogReport.LogName = "FillingSystemModbusServer";

            var mif = new MemIniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{LogReport.LogName}.ini"));

            var locEvClient = new EventClient();
            locEvClient.Connect(new[] { "status" },
                PropertyUpdate,
                (string errorMessage) => LogReport.AppendToLog(errorMessage),
                (Guid clientId, ClientConnectionStatus status) => { });

            // запуск потока для прослушивания запросов от устройства по протоколу Modbus Tcp
            var listener = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            listener.DoWork += ModbusHelper.ModbusListener_DoWork;
            listener.ProgressChanged += ModbusHelper.ModbusListener_ProgressChanged;
            listener.RunWorkerAsync();

            // если запускает пользователь сам
            if (Environment.UserInteractive)
            {
                var s = WcfEventService.EventService;
                s.Start();
                try
                {
                    Console.WriteLine("FillingSystemModbusServer. Ver. 0.1");
                    Console.WriteLine("\nPress any key for exit...");
                    Console.ReadKey();
                }
                finally
                {
                    s.Stop();
                }
            }
            else
            {
                // запуск в виде службы Windows
                var servicesToRun = new ServiceBase[] { new WinService() };
                ServiceBase.Run(servicesToRun);
            }
            listener.CancelAsync();
        }

        private static void PropertyUpdate(DateTime servertime, string category, string pointname, string propname, string value)
        {
            if (category == "status")
            {
                if (int.TryParse(pointname, out int npp) && npp > 0 &&
                    int.TryParse(propname, out int offset) &&
                    ushort.TryParse(value, out ushort val))
                {
                    var startAddr = (npp - 1) * 10 + offset; // порядковый номер со смещением на 10 регистов + собственно номер регистра (от нуля)
                    var regAddr = ModbusHelper.ModifyToModbusRegisterAddress((ushort)startAddr, 4);
                    ModbusHelper.SetRegisterValue(1, regAddr, ModbusHelper.Swap(val));
                }
            }
        }

        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
        private static Process RunningInstance()
        {
            var current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);
            // Просматриваем все процессы
            return processes.Where(process => process.Id != current.Id).
                FirstOrDefault(process => Assembly.GetExecutingAssembly().
                    Location.Replace("/", "\\") == current.MainModule.FileName);
            // нет, таких процессов не найдено
        }
    }

}
