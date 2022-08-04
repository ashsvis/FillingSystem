using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Linq;

namespace FillingSystemHelper
{
    public static class ModbusHelper
    {
        /// <summary>
        /// Считывание статусных регистров HR00..HR05
        /// </summary>
        /// <param name="socket">Сокет</param>
        /// <param name="riser">Данные стояка налива</param>
        /// <returns></returns>
        public static ModbusAnswerData FetchStatus(Socket socket, RiserKey riser)
        {
            return Fetch(socket, riser, 1, 6);
        }

        /// <summary>
        /// Считывание статусных регистров HR00..HR05
        /// </summary>
        /// <param name="socket">Сокет</param>
        /// <param name="riser">Данные стояка налива</param>
        /// <returns></returns>
        public static ModbusAnswerData FetchAnalogs(Socket socket, RiserKey riser)
        {
            return Fetch(socket, riser, 37, 2);
        }

        /// <summary>
        /// Считывание регистров HR35 - глубины погружения и HR36 - рабочей длины сигнализатора уровня
        /// </summary>
        /// <param name="socket">Сокет</param>
        /// <param name="riser">Данные стояка налива</param>
        /// <returns></returns>
        public static ModbusAnswerData FetchDeepWork(Socket socket, RiserKey riser)
        {
            return Fetch(socket, riser, 54, 2);
        }

        /// <summary>
        /// Считывание регистров, начиная с HR14 и до конца таблицы
        /// </summary>
        /// <param name="socket">Сокет</param>
        /// <param name="riser">Данные стояка налива</param>
        /// <returns></returns>
        public static ModbusAnswerData FetchTuningParameters(Socket socket, RiserKey riser)
        {
            return Fetch(socket, riser, 21, 41);
        }

        public static byte[] WriteConfigData(Socket socket, RiserKey riser, ushort[] data)
        {
            return Write(socket, riser, 10, data);
        }

        public static void FillingStart(Socket socket, RiserKey riser, ushort height, ushort setpoint)
        {
            Write(socket, riser, 7, new ushort[] { 0x01, height, setpoint });
        }

        public static void FillingStop(Socket socket, RiserKey riser)
        {
            Write(socket, riser, 7, new ushort[] { 0x02 });
        }

        private static ModbusAnswerData Fetch(Socket socket, RiserKey riser, int regAddr, int regCount)
        {
            socket.Send(PrepareFetchParam(riser.NodeAddr, riser.Func, regAddr, regCount));
            Thread.Sleep(1);
            var buff = new byte[8192];
            try
            {
                var numBytes = socket.Receive(buff);
                if (numBytes > 0)
                {
                    var answer = CleanAnswer(buff);
                    if (CheckAnswer(answer, riser.NodeAddr, riser.Func, regCount))
                        return EncodeFetchAnswer(answer, riser.NodeAddr, riser.Func, regAddr, regCount);
                    else if (CheckError(answer, riser.NodeAddr, riser.Func, out int error, out string mess))
                    {
                        LogReport.AppendToLog($"Ошибка {error}. {mess}");
                        return EncodeFetchError(riser.NodeAddr, riser.Func, error);
                    }
                }
                else
                    return new ModbusAnswerData() { Active = false };
            }
            catch (Exception ex)
            {
                LogReport.AppendToLog(ex);
            }
            return null;
        }

        public static byte[] PrepareFetchParam(int node, int func, int regAddr, int regCount)
        {
            var datacount = 1 * regCount;
            var addr = regAddr - 1;
            return EncodeData(0, 0, 0, 0, 0, 6, (byte)node, (byte)func,
                                       (byte)(addr >> 8), (byte)(addr & 0xff),
                                       (byte)(datacount >> 8), (byte)(datacount & 0xff));
        }

        private static byte[] EncodeData(params byte[] list)
        {
            var result = new byte[list.Length];
            for (var i = 0; i < list.Length; i++) result[i] = list[i];
            return result;
        }

        private static byte[] Write(Socket socket, RiserKey riser, int regAddr, IEnumerable<ushort> writevals)
        {
            socket.Send(PrepareWriteParam(riser.NodeAddr, regAddr, writevals));
            Thread.Sleep(10);
            var buff = new byte[8192];
            try
            {
                var numBytes = socket.Receive(buff);
                if (numBytes > 0)
                {
                    var answer = CleanAnswer(buff);
                    return answer;
                }
                else
                    return new byte[] { };
            }
            catch (Exception ex)
            {
                LogReport.AppendToLog(ex);
            }
            return new byte[] { };
        }

        public static byte[] PrepareWriteParam(int node, int regAddr, IEnumerable<ushort> writevals)
        {
            byte func = 16;
            var datacount = writevals.Count();
            var addr = regAddr - 1;
            var list = new List<byte>();
            list.AddRange(new byte[] { 0, 0, 0, 0, 0, (byte)(7 + datacount * 2) });
            list.AddRange(new[] { (byte)node, func });
            list.AddRange(BitConverter.GetBytes(Swap((ushort)addr)));
            list.AddRange(BitConverter.GetBytes(Swap((ushort)datacount)));
            list.Add((byte)(datacount * 2));
            foreach (var writeval in writevals)
                list.AddRange(BitConverter.GetBytes(Swap(writeval)));
            return list.ToArray();
        }

        public static byte[] CleanAnswer(IEnumerable<byte> receivedBytes)
        {
            var source = new List<byte>();
            var length = 0;
            var n = 0;
            foreach (var b in receivedBytes)
            {
                if (n == 5)
                    length = b;
                else if (n > 5 && length > 0)
                {
                    source.Add(b);
                    if (source.Count == length)
                        break;
                }
                n++;
            }
            return source.ToArray();
        }

        public static bool CheckAnswer(byte[] answer, int node, int func, int regCount)
        {
            var datacount = 1 * regCount;
            if (datacount * 2 + 3 == answer.Length)
            {
                if (answer[0] == node && answer[1] == func && datacount * 2 == answer[2])
                    return true;
            }
            return false;
        }

        public static bool CheckError(byte[] answer, int node, int func, out int error, out string message)
        {
            message = string.Empty;
            error = 0;
            if (answer.Length == 3)
            {
                if (answer[0] == node && answer[1] == (func | 0x80))
                {
                    error = answer[2];
                    message = DecodeError(error);
                    return true;
                }
            }
            return false;
        }

        public static string DecodeError(int error)
        {
            var errors = new Dictionary<int, string>()
            {
                { 1, "Принятый код функции не может быть обработан."},
                { 2, "Адрес данных, указанный в запросе, недоступен."},
                { 3, "Значение, содержащееся в поле данных запроса, является недопустимой величиной."},
                { 4, "Невосстанавливаемая ошибка имела место, пока ведомое устройство пыталось выполнить затребованное действие."},
                { 5, "Ведомое устройство приняло запрос и обрабатывает его, но это требует много времени."},
                { 6, "Ведомое устройство занято обработкой команды."},
                { 7, "Ведомое устройство не может выполнить программную функцию, заданную в запросе."},
                { 8, "Ведомое устройство при чтении расширенной памяти обнаружило ошибку паритета."},
                { 10, "Шлюз неправильно настроен или перегружен запросами."},
                { 11, "Ведомого устройства нет в сети или от него нет ответа."},
            };
            return errors.ContainsKey(error) ? errors[error] : $"Неизвестная ошибка {error}.";
        }

        public static ModbusAnswerData EncodeFetchAnswer(byte[] answer, int node, int func, int regAddr, int regCount)
        {
            var registers = new List<ushort>();
            var data = new byte[2];
            var offset = 3;
            for (var i = 0; i < regCount; i++)
            {
                Array.Copy(answer, offset, data, 0, 2);
                registers.Add(BitConverter.ToUInt16(Swap(data, 0, "BA"), 0));
                offset += 2;
            }
            return new ModbusAnswerData()
            {
                Active = true,
                Node = (byte)node,
                Func = (byte)func,
                RegAddr = (ushort)regAddr,
                Registers = registers.ToArray()
            };
        }

        public static ModbusAnswerData EncodeFetchError(int node, int func, int error)
        {
            return new ModbusAnswerData()
            {
                Active = true,
                Node = (byte)node,
                Func = (byte)func,
                ErrorCode = (byte)error
            };
        }

        public static string EncodeBitData(ushort data)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 16; i++)
            {
                var bc = data & 0x01;
                if (bc > 0)
                    sb.Insert(0, "1");
                else
                    sb.Insert(0, "0");
                if (i % 4 == 3)
                    sb.Insert(0, " ");
            }
            return sb.ToString().Trim();
        }

        public static DateTime ConvertFromUnixTimestamp(uint timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static uint ConvertToUnixTimestamp(DateTime dateTime)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (uint)(dateTime - origin).TotalSeconds;
        }

        public static ushort Swap(ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            var buff = bytes[0];
            bytes[0] = bytes[1];
            bytes[1] = buff;
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static byte[] Swap(IEnumerable<byte> buff, int startIndex, string typeSwap)
        {
            var list = buff.Skip(startIndex).ToArray();
            if (list.Length == 2)
            {
                switch (typeSwap)
                {
                    case "AB":
                        return new byte[] { list[0], list[1] };
                    case "BA":
                        return new byte[] { list[1], list[0] };
                    default:
                        return list;
                }
            }
            else if (list.Length == 4)
            {
                switch (typeSwap)
                {
                    case "ABCD":
                        return new byte[] { list[0], list[1], list[2], list[3] };
                    case "CDAB":
                        return new byte[] { list[2], list[3], list[0], list[1] };
                    case "BADC":
                        return new byte[] { list[1], list[0], list[3], list[2] };
                    case "DCBA":
                        return new byte[] { list[3], list[2], list[1], list[0] };
                    default:
                        return list;
                }
            }
            else if (list.Length == 8)
            {
                switch (typeSwap)
                {
                    case "ABCDEFGH":
                        return new byte[] { list[0], list[1], list[2], list[3], list[4], list[5], list[6], list[7] };
                    case "GHEFCDAB":
                        return new byte[] { list[6], list[7], list[4], list[5], list[2], list[3], list[0], list[1] };
                    case "BADCFEHG":
                        return new byte[] { list[1], list[0], list[3], list[2], list[5], list[4], list[7], list[6] };
                    case "HGFEDCBA":
                        return new byte[] { list[7], list[6], list[5], list[4], list[3], list[2], list[1], list[0] };
                    default:
                        return list;
                }
            }
            else
                return list;
        }

        public static void ModbusListener_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            try
            {
                var listener = new TcpListener(IPAddress.Any, 502) { Server = { SendTimeout = 5000, ReceiveTimeout = 5000 } };
                do
                {
                    Thread.Sleep(100);
                    try
                    {
                        listener.Start(10);
                        // Buffer for reading data
                        var bytes = new byte[256];

                        while (!listener.Pending())
                        {
                            Thread.Sleep(1);
                            if (!worker.CancellationPending) continue;
                            listener.Stop();
                            worker.ReportProgress(0, "Not listening");
                            return;
                        }
                        var clientData = listener.AcceptTcpClient();
                        // создаем отдельный поток для каждого подключения клиента
                        ThreadPool.QueueUserWorkItem(arg =>
                        {
                            try
                            {
                                // Get a stream object for reading and writing
                                var stream = clientData.GetStream();
                                int count;
                                // Loop to receive all the data sent by the client.
                                while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    Thread.Sleep(1);
                                    var list = new List<string>();
                                    for (var i = 0; i < count; i++) list.Add(string.Format("{0}", bytes[i]));
                                    if (count < 6) continue;
                                    var header1 = Convert.ToUInt16(bytes[0] * 256 + bytes[1]);
                                    var header2 = Convert.ToUInt16(bytes[2] * 256 + bytes[3]);
                                    var packetLen = Convert.ToUInt16(bytes[4] * 256 + bytes[5]);
                                    if (count != packetLen + 6) continue;
                                    var nodeAddr = bytes[6];
                                    var funcCode = bytes[7];
                                    var startAddr = Convert.ToUInt16(bytes[8] * 256 + bytes[9]);
                                    var regCount = Convert.ToUInt16(bytes[10] * 256 + bytes[11]);
                                    var singleValue = Convert.ToUInt16(bytes[10] * 256 + bytes[11]);
                                    List<byte> answer;
                                    ushort regAddr;
                                    byte bytesCount;
                                    byte[] msg;
                                    switch (funcCode)
                                    {
                                        case 3: // - read holding registers
                                        case 4: // - read input registers
                                            answer = new List<byte>();
                                            answer.AddRange(BitConverter.GetBytes(Swap(header1)));
                                            answer.AddRange(BitConverter.GetBytes(Swap(header2)));
                                            bytesCount = Convert.ToByte(regCount * 2);
                                            packetLen = Convert.ToUInt16(bytesCount + 3);
                                            answer.AddRange(BitConverter.GetBytes(Swap(packetLen)));
                                            answer.Add(nodeAddr);
                                            answer.Add(funcCode);
                                            answer.Add(bytesCount);
                                            //
                                            for (ushort i = 0; i < regCount; i++)
                                            {
                                                regAddr = ModifyToModbusRegisterAddress((ushort)(i + startAddr), funcCode);
                                                ushort value = GetRegisterValue(nodeAddr, regAddr);
                                                answer.AddRange(BitConverter.GetBytes(value));
                                            }
                                            //
                                            msg = answer.ToArray();
                                            stream.Write(msg, 0, msg.Length);
                                            break;
                                        case 6: // write one register
                                            answer = new List<byte>();
                                            answer.AddRange(BitConverter.GetBytes(Swap(header1)));
                                            answer.AddRange(BitConverter.GetBytes(Swap(header2)));
                                            packetLen = Convert.ToUInt16(6);
                                            answer.AddRange(BitConverter.GetBytes(Swap(packetLen)));
                                            answer.Add(nodeAddr);
                                            answer.Add(funcCode);
                                            answer.AddRange(BitConverter.GetBytes(Swap(startAddr)));
                                            answer.AddRange(BitConverter.GetBytes(Swap(singleValue)));
                                            regAddr = ModifyToModbusRegisterAddress((ushort)startAddr, 3);
                                            SetRegisterValue(nodeAddr, regAddr, Swap(singleValue));
                                            //
                                            msg = answer.ToArray();
                                            stream.Write(msg, 0, msg.Length);
                                            break;
                                        case 16: // write several registers
                                            answer = new List<byte>();
                                            answer.AddRange(BitConverter.GetBytes(Swap(header1)));
                                            answer.AddRange(BitConverter.GetBytes(Swap(header2)));
                                            answer.AddRange(BitConverter.GetBytes(Swap(6)));
                                            answer.Add(nodeAddr);
                                            answer.Add(funcCode);
                                            answer.AddRange(BitConverter.GetBytes(Swap(startAddr)));
                                            answer.AddRange(BitConverter.GetBytes(Swap(regCount)));
                                            var bytesToWrite = bytes[12];
                                            if (bytesToWrite != regCount * 2) break;
                                            var n = 13;
                                            for (var i = 0; i < regCount; i++)
                                            {
                                                var value = Convert.ToUInt16(bytes[n] * 256 + bytes[n + 1]);
                                                regAddr = ModifyToModbusRegisterAddress((ushort)(i + startAddr), 3);
                                                SetRegisterValue(nodeAddr, regAddr, Swap(value));
                                                n += 2;
                                            }
                                            //
                                            msg = answer.ToArray();
                                            stream.Write(msg, 0, msg.Length);
                                            break;
                                    }
                                }
                                // Shutdown and end connection
                                clientData.Close();
                            }
                            catch (Exception ex)
                            {
                                if (!worker.CancellationPending)
                                    worker.ReportProgress(0, ex.Message);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        if (!worker.CancellationPending)
                            worker.ReportProgress(0, ex.Message);
                    }
                } while (!worker.CancellationPending);
                listener.Stop();
            }
            catch
            {
            }
        }

        private static readonly ushort[,] registers = new ushort[247, 50000];

        private static readonly object locker = new object();

        private static ushort GetRegisterValue(byte node, ushort index)
        {
            lock (locker)
            {
                return registers[node - 1, index - 1];
            }
        }

        public static void SetRegisterValue(byte node, ushort index, ushort value)
        {
            lock (locker)
            {
                registers[node - 1, index - 1] = value;
            }
        }

        public static void ModbusListener_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (Environment.UserInteractive)
                Console.WriteLine($"{e.UserState}");
        }

        public static ushort ModifyToModbusRegisterAddress(int startAddr, byte funcCode)
        {
            switch (funcCode)
            {
                case 1:
                    return Convert.ToUInt16(1 + startAddr);       // coils
                case 2:
                    return Convert.ToUInt16(10001 + startAddr);   // contacts
                case 3:
                    return Convert.ToUInt16(40001 + startAddr);   // holdings
                case 4:
                    return Convert.ToUInt16(30001 + startAddr);   // inputs
            }
            throw new NotImplementedException();
        }
    }
}
