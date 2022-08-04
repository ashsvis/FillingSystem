using ModbusIntegratorEvent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FillingSystemHelper
{
    public static class FetchingHelper
    {
        private static readonly List<BackgroundWorker> workers = new List<BackgroundWorker>();

        public static void RunFetchers(string baseDirectory, string serviceName)
        {
            StopFetchers();
            var tuning = new EthernetTuning(baseDirectory, serviceName);
            foreach (var grp in tuning.RiserKeys.GroupBy(key => key.IpAddress))
            {
                var ipAddr = grp.Key;
                var worker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
                worker.ProgressChanged += Fetcher_ProgressChanged;
                worker.DoWork += EthernetFetcher_DoWork;
                workers.Add(worker);

                tuning.Address = IPAddress.Parse(ipAddr);
                tuning.Port = grp.First().IpPort;
                tuning.RiserKeys = grp.ToList();
                worker.RunWorkerAsync(tuning);
            }
        }

        public static void StopFetchers()
        {
            workers.ForEach(item => item.CancelAsync());
            workers.Clear();
        }

        public static void EthernetFetching(EthernetTuning ethernetTuning, BackgroundWorker worker)
        {
            var server = new FillingSqlServer { Connection = ethernetTuning.ConnectionString };
            if (!(ethernetTuning is EthernetTuning pars)) return;

            ClientConnectionStatus clientConnectionStatus = ClientConnectionStatus.Closed;

            var locEvClient = new EventClient();
            locEvClient.Connect(new[] { "status" },
                (DateTime servertime, string category, string pointname, string propname, string value) => { }, //PropertyUpdate, 
                (errormessage) => { worker.ReportProgress(0, errormessage); },
                (clientId, connectionStatus) => { clientConnectionStatus = connectionStatus; });

            var dict = new Dictionary<string, DateTime>();
            pars.RiserKeys.ForEach(key => dict.Add(key.ToString(), DateTime.Now + TimeSpan.FromSeconds(1)));

            while (!worker.CancellationPending)
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.SendTimeout = 3000;
                    socket.ReceiveTimeout = 3000;
                    try
                    {
                        var remoteEp = new IPEndPoint(pars.Address, pars.Port);
                        socket.Connect(remoteEp);
                        Thread.Sleep(1);
                        if (socket.Connected)
                        {
                            worker.ReportProgress(0, $"Сокет {remoteEp} подключен");

                            var dt = DateTime.Now;
                            foreach (var key in pars.RiserKeys)
                            {
                                server.SetOffline(dt, key.Overpass, key.Way, key.Product, key.Riser, "Инициализация");
                            }

                            var lastsecond = DateTime.Now.Second;
                            var lastminute = DateTime.Now.Minute;
                            while (!worker.CancellationPending)
                            {
                                dt = DateTime.Now;

                                try
                                {
                                    foreach (var key in pars.RiserKeys)
                                    {
                                        var skey = key.ToString();

                                        if (dict[skey] > dt)
                                            continue;

                                        var rkey = server.GetRiser(key.Overpass, key.Way, key.Product, key.Riser);
                                        var answerStatus = ModbusHelper.FetchStatus(socket, rkey);
                                        ModbusAnswerData answerCurrents = null;

                                        if (answerStatus == null || answerStatus.ErrorCode > 0)
                                        {
                                            string offlineCase = null;
                                            if (answerStatus != null)
                                                offlineCase = $"Обрыв соединения: {ModbusHelper.DecodeError(answerStatus.ErrorCode)}";
                                            server.SetOffline(dt, key.Overpass, key.Way, key.Product, key.Riser, offlineCase);
                                            
                                            dict[skey] = dt + TimeSpan.FromSeconds(30);

                                            if (clientConnectionStatus == ClientConnectionStatus.Opened)
                                            {
                                                for (var i = 0; i < 7; i++)
                                                    locEvClient.UpdateProperty("status", $"{key.Npp}", $"{i}", "0");
                                            }
                                            continue;
                                        }
                                        else
                                        {
                                            // получение и передача двух регистров с аналоговыми значениями каналов ADC
                                            answerCurrents = ModbusHelper.FetchAnalogs(socket, rkey);
                                        }

                                        dict[skey] = dt + TimeSpan.FromSeconds(3);

                                        if (clientConnectionStatus == ClientConnectionStatus.Opened)
                                        {
                                            var n = 0;
                                            // передача признака наличия связи с полевым контроллером
                                            locEvClient.UpdateProperty("status", $"{key.Npp}", $"{n++}", "1");
                                            // передача шести первых регистров полевого контроллера
                                            for (var i = 0; i < answerStatus.Registers.Length; i++)
                                                locEvClient.UpdateProperty("status", $"{key.Npp}", $"{n++}", $"{answerStatus.Registers[i]}");
                                            if (answerCurrents != null && answerCurrents.ErrorCode == 0)
                                            {
                                                for (var i = 0; i < answerCurrents.Registers.Length; i++)
                                                    locEvClient.UpdateProperty("status", $"{key.Npp}", $"{n++}", $"{answerCurrents.Registers[i]}");
                                            }
                                        }

                                        if (answerStatus.Registers.Length >= 6 && answerStatus.Registers[5] > 1)
                                            dict[skey] = dt + TimeSpan.FromSeconds(0.5);

                                        // чтение количества остановов из контроллера стояка налива
                                        var modbusStopCount = answerStatus.Registers.Length == 6 ? answerStatus.Registers[1] & 0xFF : 0;
                                        // чтение количества остановов из базы данных опроса
                                        var serverStopCount = server.GetStopCount(key.Overpass, key.Way, key.Product, key.Riser);
                                        // определение факта останова налива, если значения не равны
                                        if (modbusStopCount != serverStopCount)
                                        {
                                            var task = server.FindTask(key.Overpass, key.Way, key.Product, key.Riser);
                                            if (!string.IsNullOrWhiteSpace(task.Number) && answerStatus.Registers.Length > 2)
                                            {
                                                // записываем данные налива в файл налива
                                                var finishLevel = answerStatus.Registers[0];
                                                var finishCode = answerStatus.Registers[2];
                                                // записываем данные налива в файл налива
                                                var reason = GetStopFillingReason(finishCode);
                                                if (!string.IsNullOrWhiteSpace(reason))
                                                {
                                                    server.InsertIntoLogReport(DateTime.Now, "Filled", reason, key.Overpass, key.Way, key.Product, key.Riser);
                                                    server.IncreaseFillCountIntoWagons(task.Number);
                                                }
                                            }
                                        }
                                        if (!server.ReplaceIntoFetching(answerStatus.Active, key.Overpass, key.Way, key.Product, key.Riser, answerStatus.RegAddr, answerStatus.Registers))
                                            worker.ReportProgress(0, server.LastError);
                                        var command = server.GetCommand(key.Overpass, key.Way, key.Product, key.Riser);

                                        if (command.HasFlag(ControlCommand.Run | ControlCommand.Stop | ControlCommand.GetDeepAndRange |
                                            ControlCommand.GetLinkData | ControlCommand.GetPlcData | ControlCommand.GetAdcData |
                                            ControlCommand.GetAlarmData | ControlCommand.GetLevelData | ControlCommand.WriteConfigData))
                                            dict[skey] = dt + TimeSpan.FromSeconds(0.5);
 
                                        if (command.HasFlag(ControlCommand.Run))
                                        {
                                            var task = server.FindTask(key.Overpass, key.Way, key.Product, key.Riser);
                                            ModbusHelper.FillingStart(socket, key, (ushort)task.RealHeight, (ushort)task.Setpoint);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.Run);
                                            if (!server.ReplaceIntoFetching(answerStatus.Active, key.Overpass, key.Way, key.Product, key.Riser, answerStatus.RegAddr, answerStatus.Registers))
                                                worker.ReportProgress(0, server.LastError);
                                        }
                                        if (command.HasFlag(ControlCommand.Stop))
                                        {
                                            ModbusHelper.FillingStop(socket, key);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.Stop);
                                            if (!server.ReplaceIntoFetching(answerStatus.Active, key.Overpass, key.Way, key.Product, key.Riser, answerStatus.RegAddr, answerStatus.Registers))
                                                worker.ReportProgress(0, server.LastError);
                                        }
                                        if (command.HasFlag(ControlCommand.GetDeepAndRange))
                                        {
                                            var answer = ModbusHelper.FetchDeepWork(socket, key);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.GetDeepAndRange);
                                            if (!server.ReplaceIntoFetching(answerStatus.Active, key.Overpass, key.Way, key.Product, key.Riser, answer.RegAddr, answer.Registers))
                                                worker.ReportProgress(0, server.LastError);
                                        }
                                        if (command.HasFlag(ControlCommand.GetLinkData))
                                        {
                                            var answer = ModbusHelper.FetchTuningParameters(socket, key);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.GetLinkData);
                                            if (!server.ReplaceIntoFetching(answer.Active, key.Overpass, key.Way, key.Product, key.Riser, answer.RegAddr, answer.Registers))
                                                worker.ReportProgress(0, server.LastError);
                                        }
                                        if (command.HasFlag(ControlCommand.GetPlcData))
                                        {
                                            var answer = ModbusHelper.FetchTuningParameters(socket, key);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.GetPlcData);
                                            if (!server.ReplaceIntoFetching(answer.Active, key.Overpass, key.Way, key.Product, key.Riser, answer.RegAddr, answer.Registers))
                                                worker.ReportProgress(0, server.LastError);
                                        }
                                        if (command.HasFlag(ControlCommand.GetAdcData))
                                        {
                                            var answer = ModbusHelper.FetchTuningParameters(socket, key);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.GetAdcData);
                                            if (!server.ReplaceIntoFetching(answer.Active, key.Overpass, key.Way, key.Product, key.Riser, answer.RegAddr, answer.Registers))
                                                worker.ReportProgress(0, server.LastError);
                                        }
                                        if (command.HasFlag(ControlCommand.GetAlarmData))
                                        {
                                            var answer = ModbusHelper.FetchTuningParameters(socket, key);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.GetAlarmData);
                                            if (!server.ReplaceIntoFetching(answer.Active, key.Overpass, key.Way, key.Product, key.Riser, answer.RegAddr, answer.Registers))
                                                worker.ReportProgress(0, server.LastError);
                                        }
                                        if (command.HasFlag(ControlCommand.GetLevelData))
                                        {
                                            var answer = ModbusHelper.FetchTuningParameters(socket, key);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.GetLevelData);
                                            if (!server.ReplaceIntoFetching(answer.Active, key.Overpass, key.Way, key.Product, key.Riser, answer.RegAddr, answer.Registers))
                                                worker.ReportProgress(0, server.LastError);
                                        }
                                        if (command.HasFlag(ControlCommand.WriteConfigData))
                                        {
                                            var data = server.FindConfig(key.Overpass, key.Way, key.Product, key.Riser);
                                            var buff = ModbusHelper.WriteConfigData(socket, key, data);
                                            server.ResetCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.WriteConfigData);
                                            string reason, category;
                                            if (buff.Length == 6)
                                            {
                                                reason = "Изменение внесено.";
                                                category = "Message";
                                            }
                                            else if (buff.Length == 3)
                                            {
                                                reason = $"Код ошибки: {buff[2]}";
                                                category = "Error";
                                            }
                                            else
                                            {
                                                reason = "Нет ответа.";
                                                category = "Error";
                                            }
                                            server.InsertIntoLogReport(DateTime.Now, category, reason, key.Overpass, key.Way, key.Product, key.Riser);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (socket.Connected)
                                        worker.ReportProgress(0, ex);
                                    else
                                        break;
                                }
                            }
                            worker.ReportProgress(0, $"Сокет {remoteEp} больше не используется");
                        }
                        else
                            worker.ReportProgress(0, $"Сокет {remoteEp} не подключен");
                    }
                    catch (Exception ex)
                    {
                        LogReport.AppendToLog(ex);
                        foreach (var key in pars.RiserKeys)
                            server.SetOffline(DateTime.Now, key.Overpass, key.Way, key.Product, key.Riser, $"Обрыв соединения: {ex.Message}");
                    }
                }
            }

            locEvClient.Disconnect();
        }

        public static string GetStopFillingReason(int flags)
        {
            if ((flags & 0x0001) > 0)
                return "Налив завершён аварийно. Сигнализатор аварийный";
            if ((flags & 0x0002) > 0)
                return "Налив завершён аварийно. Кнопка СТОП";
            if ((flags & 0x0004) > 0)
                return "Налив завершён аварийно. Неисправность цепи готовности";
            if ((flags & 0x0008) > 0)
                return "Налив завершён аварийно. Неисправность сигнализатора уровня";
            if ((flags & 0x0010) > 0)
                return "Налив завершён аварийно. Истекло время работы без связи";
            if ((flags & 0x0020) > 0)
                return "Налив завершён аварийно. Заземление отсутствует";
            if ((flags & 0x0040) > 0)
                return "Налив завершён аварийно. Ошибка клапана большого прохода";
            if ((flags & 0x0080) > 0)
                return "Налив завершён аварийно. Ошибка клапана малого прохода";
            if ((flags & 0x0100) > 0)
                return "Налив завершён аварийно. Ток сигнализатора уровня меньше минимального";
            if ((flags & 0x0200) > 0)
                return "Налив завершён аварийно. Ток сигнализатора уровня больше максимального";
            if ((flags & 0x0400) > 0)
                return "Налив завершён аварийно. Ток сигнализатора аварийного меньше минимального";
            if ((flags & 0x0800) > 0)
                return "Налив завершён аварийно. Ток сигнализатора аварийного больше максимального";
            if ((flags & 0x1000) > 0)
                return "Налив завершён аварийно. Сработал датчик рабочего положения";
            if ((flags & 0x2000) > 0)
                return "Автоматическое завершение налива (по заданию)";
            if ((flags & 0x4000) > 0)
                return "Налив завершён оператором АРМ";
            if ((flags & 0x8000) > 0)
                return "Налив завершён аварийно. Неверное задание налива";
            return "";
        }

        public static void EthernetFetcher_DoWork(object sender, DoWorkEventArgs e)
        {
            EthernetFetching((EthernetTuning)e.Argument, (BackgroundWorker)sender);
        }

        public static void Fetcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                if (e.UserState is Exception ex)
                    LogReport.AppendToLog(ex);
                else
                    LogReport.AppendToLog($"{e.UserState}");
            }
        }

    }
}
