using FillingSystemHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.ServiceProcess;

namespace FillingSystemReplication
{
    class ReplicationProgram
    {
        static void Main(string[] args)
        {
            #region Защита от повторного запуска
            var process = RunningInstance();
            if (process != null) return;
            #endregion
            LogReport.LogName = "FillingSystemReplication";

            var mif = new MemIniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{LogReport.LogName}.ini"));
            var connectionA = mif.ReadString("SqlServer", "connectionA", "");
            var connectionB = mif.ReadString("SqlServer", "connectionB", "");

            var worker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            worker.ProgressChanged += WorkerProgressChanged;
            worker.DoWork += WorkerDoWork;
            worker.RunWorkerAsync(new Tuple<string, string>(connectionA, connectionB));

            // если запускает пользователь сам
            if (Environment.UserInteractive)
            {
                Console.WriteLine("FillingSystemReplication. Ver. 0.1");
                Console.WriteLine($"\nConnectionString A is {connectionA}");
                Console.WriteLine($"\nConnectionString B is {connectionB}");
                Console.WriteLine("\nPress any key for exit...");
                Console.ReadKey();
            }
            else
            {
                // запуск в виде службы Windows
                var servicesToRun = new ServiceBase[] { new WinService() };
                ServiceBase.Run(servicesToRun);
            }
            worker.CancelAsync();
        }

        private static void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            // строка подключения к серверу B (всегда)
            var arg = (Tuple<string, string>)e.Argument;
            var connectionA = arg.Item1;
            var connectionB = arg.Item2;
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                var lastsecond = DateTime.Now.Second;
                var lastminute = DateTime.Now.Minute;
                while (!worker.CancellationPending)
                {
                    var dt = DateTime.Now;
                    if (lastsecond == dt.Second) continue;
                    lastsecond = dt.Second;
                    // прошла секунда
                    if (lastsecond % 10 != 0) continue;
                    // прошло 10 секунд
                    try
                    {
                        ReplicateTable(worker, connectionA, connectionB, "FETCHING", UpdateFetchingRecord);
                        ReplicateTable(worker, connectionA, connectionB, "TYPES", UpdateTypesRecord);
                        ReplicateTable(worker, connectionA, connectionB, "WAGONS", UpdateWagonsRecord);
                        ReplicateTable(worker, connectionA, connectionB, "OPERATORS", UpdateOperatorsRecord);
                        ReplicateTable(worker, connectionA, connectionB, "LOGREPORT", UpdateLogReportRecord);
                    }
                    catch (Exception ex)
                    {
                        worker.ReportProgress(0, ex);
                    }
                }
            }
        }

        private static void ReplicateTable(BackgroundWorker worker,
            string connectionA, string connectionB, string tableName, Action<FillingSqlServer, DataRow> method)
        {
            var snapTimeA = GetLastDate(worker, connectionA, tableName);
            var snapTimeB = GetLastDate(worker, connectionB, tableName);
            if (Math.Abs(snapTimeA.Ticks - snapTimeB.Ticks) >= TimeSpan.FromSeconds(10).Ticks)
            {
                var ds = new DataSet();
                var connection = "";
                var message = "";
                if (snapTimeA > snapTimeB)
                {
                    ds = GetLog(worker, connectionA, tableName, snapTimeB);
                    connection = connectionB;
                    message = $"Replicate table \"{tableName}\" from A to B";
                }
                else if (snapTimeB > snapTimeA)
                {
                    ds = GetLog(worker, connectionB, tableName, snapTimeA);
                    connection = connectionA;
                    message = $"Replicate table \"{tableName}\" from B to A";
                }
                if (ds.Tables.Count > 0)
                {
                    var server = new FillingSqlServer() { Connection = connection };
                    var table = ds.Tables[0];
                    foreach (var row in table.Rows.Cast<DataRow>())
                    {
                        method(server, row);
                        if (server.LastError != null)
                            throw server.LastError;
                    }
                    worker.ReportProgress(0, $"{message}, {table.Rows.Count} row(s)");
                }
            }
        }

        private static void UpdateFetchingRecord(FillingSqlServer server, DataRow row)
        {
            bool active = (bool)row["Active"];
            int overpass = (int)row["Overpass"];
            int way = (int)row["Way"];
            string product = (string)row["Product"];
            int riser = (int)row["Riser"];
            ushort[] values = new ushort[61];
            for (var i = 0; i < 61; i++)
            {
                var value = row[$"HR{i:X2}"];
                if (value != DBNull.Value)
                    values[i] = (ushort)(int)value;
            }
            server.ReplaceIntoFetching(active, overpass, way, product, riser, 1, values);     // пробуем обновить
        }

        private static void UpdateTypesRecord(FillingSqlServer server, DataRow row)
        {
            int ntype = (int)row["NType"];
            int diameter = (int)row["Diameter"];
            int throat = (int)row["Throat"];
            int defLevel = (int)row["DefLevel"];
            if (!server.InsertIntoWagonTypes(ntype, diameter, throat, defLevel))    // если добавить не получается
                server.UpdateIntoWagonTypes(ntype, diameter, throat, defLevel);     // то пробуем обновить
        }

        private static void UpdateWagonsRecord(FillingSqlServer server, DataRow row)
        {
            string number = (string)row["Number"];
            int ntype = (int)row["NType"];
            int realHeight = (int)row["RealHeight"];
            if (!server.InsertIntoWagons(number, ntype, realHeight))    // если добавить не получается
                server.UpdateIntoWagons(number, ntype, realHeight);     // то пробуем обновить
        }

        private static void UpdateOperatorsRecord(FillingSqlServer server, DataRow row)
        {
            string lastname = (string)row["Lastname"];
            string firstname = (string)row["Firstname"];
            string secondname = (string)row["Secondname"];
            int access = (int)row["Access"];
            string department = (string)row["Department"];
            string appointment = (string)row["Appointment"];
            string password = (string)row["Password"];
            if (!server.InsertIntoOperators(lastname, firstname, secondname, access, department, appointment, password))    // если добавить не получается
                server.UpdateIntoOperators(lastname, firstname, secondname, password);     // то пробуем обновить
        }

        private static void UpdateLogReportRecord(FillingSqlServer server, DataRow row)
        {
            DateTime snaptime = (DateTime)row["SnapTime"];
            string category = (string)row["Category"];
            string eventInfo = (string)row["EventInfo"];
            int overpass = (int)row["Overpass"];
            int way = (int)row["Way"];
            string productCode = (string)row["Product"];
            int riser = (int)row["Riser"];
            string number = (string)row["Number"];
            int ntype = (int)row["NType"];
            int maxHeight = (int)row["MaxHeight"];
            string source = (string)row["Source"];
            int setLevel = (int)row["SetLevel"];
            server.InsertIntoLogReport(snaptime, category, eventInfo, overpass, way, productCode, riser, number, ntype, maxHeight, source, setLevel);
        }

        private static DataSet GetLog(BackgroundWorker worker, string connection, string tableName)
        {
            var sql = $"SELECT * FROM [PNVC].[dbo].[{tableName}]";
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, tableName);
                        }
                        catch (Exception ex)
                        {
                            worker.ReportProgress(0, ex);
                        }
                        return ds;
                    }
                }
            }
        }

        private static DataSet GetLog(BackgroundWorker worker, string connection, string tableName, DateTime dateTime)
        {
            var sql = $"SELECT * FROM [PNVC].[dbo].[{tableName}] WHERE [SnapTime] > @SnapTime";
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@SnapTime", dateTime);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, tableName);
                        }
                        catch (Exception ex)
                        {
                            worker.ReportProgress(0, ex);
                        }
                        return ds;
                    }
                }
            }
        }

        private static DateTime GetLastDate(BackgroundWorker worker, string connection, string tableName)
        {
            var sql = $"SELECT MAX([Snaptime]) AS Snaptime FROM [PNVC].[dbo].[{tableName}]";
            using (var con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        var snaptime = cmd.ExecuteScalar();
                        return snaptime == DBNull.Value ? new DateTime(2000, 1, 1) : (DateTime)snaptime;
                    }
                }
                catch (Exception ex)
                {
                    worker.ReportProgress(0, ex);
                }
                con.Close();
            }
            return new DateTime(2000, 1, 1);
        }

        private static int GetRecordCount(BackgroundWorker worker, string connection, string tableName)
        {
            var sql = $"SELECT Count(*) AS Snaptime FROM [PNVC].[dbo].[{tableName}]";
            using (var con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        var count = cmd.ExecuteScalar();
                        return (int)count;
                    }
                }
                catch (Exception ex)
                {
                    worker.ReportProgress(0, ex);
                }
                con.Close();
            }
            return 0;
        }

        private static void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                if (e.UserState is Exception ex)
                    LogReport.AppendToLog(ex);
                else
                {
                    LogReport.AppendToLog($"{e.UserState}");
                    if (Environment.UserInteractive)
                        Console.WriteLine($"{e.UserState}");
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
