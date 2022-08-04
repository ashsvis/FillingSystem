using FillingSystemView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FillingSystemHelper
{
    public class FillingSqlServer : SqlServer
    {
        public void ResetCommand(int overpass, int way, string product, int riser, ControlCommand command)
        {
            var exists = GetCommand(overpass, way, product, riser);
            StoreCommand(overpass, way, product, riser, exists ^ command);
        }

        public void PutCommand(int overpass, int way, string product, int riser, ControlCommand command)
        {
            var exists = GetCommand(overpass, way, product, riser);
            StoreCommand(overpass, way, product, riser, exists | command);
        }

        private void StoreCommand(int overpass, int way, string product, int riser, ControlCommand command)
        {
            var sql = "UPDATE [FETCHING] SET HR06=@HR06 WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@HR06", (int)command);
                        cmd.Parameters.AddWithValue("@Overpass", overpass);
                        cmd.Parameters.AddWithValue("@Way", way);
                        cmd.Parameters.AddWithValue("@Product", product);
                        cmd.Parameters.AddWithValue("@Riser", riser);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
        }

        public void SetOffline(DateTime snaptime, int overpass, int way, string productCode, int riser, string offlineCase)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        try
                        {
                            var sql = "SELECT COUNT(*) FROM [FETCHING] WHERE [Active]<>@Active AND [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                            var changeActiveFound = false;
                            using (var cmd = new SqlCommand(sql, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Active", false);
                                cmd.Parameters.AddWithValue("@Overpass", overpass);
                                cmd.Parameters.AddWithValue("@Way", way);
                                cmd.Parameters.AddWithValue("@Product", productCode);
                                cmd.Parameters.AddWithValue("@Riser", riser);
                                changeActiveFound = (int)cmd.ExecuteScalar() > 0;
                            }
                            sql = "UPDATE [FETCHING] SET Active=@Active WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                            using (var cmd = new SqlCommand(sql, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Active", false);
                                cmd.Parameters.AddWithValue("@Overpass", overpass);
                                cmd.Parameters.AddWithValue("@Way", way);
                                cmd.Parameters.AddWithValue("@Product", productCode);
                                cmd.Parameters.AddWithValue("@Riser", riser);
                                cmd.ExecuteNonQuery();
                            }
                            if (changeActiveFound)
                                UpdateLog(snaptime, false, overpass, way, productCode, riser, con, transaction, offlineCase);
                            transaction.Commit();
                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LastError = ex;
                        }
                    }
                    con.Close();
                    LastError = null;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
        }

        private static string UpdateLog(DateTime snaptime, bool active, int overpass, int way, string productCode, int riser, SqlConnection con, SqlTransaction transaction, string offlineCase = null)
        {
            string sql = "INSERT INTO [LOGREPORT] ([Snaptime],[Overpass],[Way],[Product],[Riser],[Category],[EventInfo],[Number],[NType],[MaxHeight],[Source],[SetLevel]) VALUES(@Snaptime,@Overpass,@Way,@Product,@Riser,@Category,@EventInfo,@Number,@NType,@MaxHeight,@Source,@SetLevel)";
            using (var cmd = new SqlCommand(sql, con, transaction))
            {
                cmd.Parameters.AddWithValue("@Snaptime", snaptime);
                cmd.Parameters.AddWithValue("@Overpass", overpass);
                cmd.Parameters.AddWithValue("@Way", way);
                cmd.Parameters.AddWithValue("@Product", productCode);
                cmd.Parameters.AddWithValue("@Riser", riser);
                cmd.Parameters.AddWithValue("@Category", active ? "Connect" : "Disconnect");
                cmd.Parameters.AddWithValue("@EventInfo", active ? "Установка соединения" : offlineCase ?? "Обрыв соединения");
                cmd.Parameters.AddWithValue("@Number", "");
                cmd.Parameters.AddWithValue("@NType", 0);
                cmd.Parameters.AddWithValue("@MaxHeight", 0);
                cmd.Parameters.AddWithValue("@Source", "");
                cmd.Parameters.AddWithValue("@SetLevel", 0);
                cmd.ExecuteNonQuery();
            }

            return sql;
        }

        public virtual void InsertIntoLogReport(DateTime snaptime, string category, string eventInfo,
            int overpass = 0, int way = 0, string productCode = "", int riser = 0,
            string number = "", int ntype = 0, int maxHeight = 0, string source = "", int setLevel = 0)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    try
                    {
                        // формирование запроса для вставки
                        var sql = "INSERT INTO [LOGREPORT] ([Snaptime],[Overpass],[Way],[Product],[Riser],[Category],[EventInfo],[Number],[NType],[MaxHeight],[Source],[SetLevel]) VALUES(@Snaptime,@Overpass,@Way,@Product,@Riser,@Category,@EventInfo,@Number,@NType,@MaxHeight,@Source,@SetLevel)";
                        using (var cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@Snaptime", snaptime);
                            cmd.Parameters.AddWithValue("@Overpass", overpass);
                            cmd.Parameters.AddWithValue("@Way", way);
                            cmd.Parameters.AddWithValue("@Product", productCode);
                            cmd.Parameters.AddWithValue("@Riser", riser);
                            cmd.Parameters.AddWithValue("@Category", category);
                            cmd.Parameters.AddWithValue("@EventInfo", eventInfo);
                            cmd.Parameters.AddWithValue("@Number", number);
                            cmd.Parameters.AddWithValue("@NType", ntype);
                            cmd.Parameters.AddWithValue("@MaxHeight", maxHeight);
                            cmd.Parameters.AddWithValue("@Source", source);
                            cmd.Parameters.AddWithValue("@SetLevel", setLevel);
                            cmd.ExecuteNonQuery();
                        }
                        LastError = null;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
        }

        public int GetStopCount(int overpass, int way, string product, int riser)
        {
            int result = 0;
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    var sql = "SELECT [HR01] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Overpass", overpass);
                        cmd.Parameters.AddWithValue("@Way", way);
                        cmd.Parameters.AddWithValue("@Product", product);
                        cmd.Parameters.AddWithValue("@Riser", riser);

                        var value = cmd.ExecuteScalar();
                        if (value != null)
                            result = (ushort)ConvertToInt(value) & 0xFF;
                        else
                            result = 0;
                    }
                    con.Close();
                    LastError = null;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return 0;
                }
            }
            return result;
        }

        public ControlCommand GetCommand(int overpass, int way, string product, int riser)
        {
            var command = ControlCommand.None;
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    var sql = "SELECT [HR06] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Overpass", overpass);
                        cmd.Parameters.AddWithValue("@Way", way);
                        cmd.Parameters.AddWithValue("@Product", product);
                        cmd.Parameters.AddWithValue("@Riser", riser);
                        var value = cmd.ExecuteScalar();
                        command = (ControlCommand)ConvertToInt(value);
                    }
                    con.Close();
                    LastError = null;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
            return command;
        }

        public bool InsertIntoFetching(int overpass, int way, string product, int riser, string ipAddress, int ipPort, byte node, byte func)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        try
                        {
                            // формирование запроса для вставки
                            var sql = $"INSERT INTO [FETCHING] ([Npp],[Overpass],[Way],[Product],[Riser],[IpAddress],[IpPort],[Node],[Func]) VALUES(@Npp,@Overpass,@Way,@Product,@Riser,@IpAddress,@IpPort,@Node,@Func)";
                            using (var cmd = new SqlCommand(sql, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Npp", 0);
                                cmd.Parameters.AddWithValue("@Overpass", overpass);
                                cmd.Parameters.AddWithValue("@Way", way);
                                cmd.Parameters.AddWithValue("@Product", product);
                                cmd.Parameters.AddWithValue("@Riser", riser);
                                cmd.Parameters.AddWithValue("@IpAddress", ipAddress);
                                cmd.Parameters.AddWithValue("@IpPort", ipPort);
                                cmd.Parameters.AddWithValue("@Node", node);
                                cmd.Parameters.AddWithValue("@Func", func);
                                cmd.ExecuteNonQuery();
                            }
                            LastError = null;
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                            transaction.Rollback();
                        }
                    }
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool UpdateIntoFetching(int overpass, int way, string product, int riser, string ipAddress, int ipPort, byte node, byte func)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        try
                        {
                            // формирование запроса для изменения
                            var sql = "UPDATE [FETCHING] SET [IpAddress]=@IpAddress,[IpPort]=@IpPort,[Node]=@Node,[Func]=@Func WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                            using (var cmd = new SqlCommand(sql, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@IpAddress", ipAddress);
                                cmd.Parameters.AddWithValue("@IpPort", ipPort);
                                cmd.Parameters.AddWithValue("@Node", node);
                                cmd.Parameters.AddWithValue("@Func", func);
                                cmd.Parameters.AddWithValue("@Overpass", overpass);
                                cmd.Parameters.AddWithValue("@Way", way);
                                cmd.Parameters.AddWithValue("@Product", product);
                                cmd.Parameters.AddWithValue("@Riser", riser);
                                var result = cmd.ExecuteNonQuery();
                            }
                            LastError = null;
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                            transaction.Rollback();
                        }
                    }
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public void DeleteIntoFetching(int overpass, int way, string product, int riser)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    var sql = "DELETE FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Overpass", overpass);
                        cmd.Parameters.AddWithValue("@Way", way);
                        cmd.Parameters.AddWithValue("@Product", product);
                        cmd.Parameters.AddWithValue("@Riser", riser);
                        cmd.ExecuteNonQuery();
                    }
                    LastError = null;
                    con.Close();
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
        }

        public bool ReplaceIntoFetching(bool active, int overpass, int way, string product, int riser, int regAddr, ushort[] values)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        try
                        {
                            // формирование запроса для замены или вставки
                            var sql = "SELECT COUNT(*) FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                            var found = false;
                            using (var cmd = new SqlCommand(sql, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Overpass", overpass);
                                cmd.Parameters.AddWithValue("@Way", way);
                                cmd.Parameters.AddWithValue("@Product", product);
                                cmd.Parameters.AddWithValue("@Riser", riser);
                                found = (int)cmd.ExecuteScalar() > 0;
                            }
                            var offset = regAddr - 1;
                            if (found)
                            {
                                sql = "SELECT COUNT(*) FROM [FETCHING] WHERE [Active]<>@Active AND [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                                var changeActiveFound = false;
                                using (var cmd = new SqlCommand(sql, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@Active", active);
                                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                                    cmd.Parameters.AddWithValue("@Way", way);
                                    cmd.Parameters.AddWithValue("@Product", product);
                                    cmd.Parameters.AddWithValue("@Riser", riser);
                                    changeActiveFound = (int)cmd.ExecuteScalar() > 0;
                                }
                                // формирование запроса для изменения
                                var list = new List<string> { "[Active] = @Active" };
                                //var list = new List<string>();
                                list.AddRange(Enumerable.Range(offset, values.Length).Select(item => $"HR{item:X2}=@HR{item:X2}"));
                                sql = $"UPDATE [FETCHING] SET {string.Join(",", list)} WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                                using (var cmd = new SqlCommand(sql, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@Active", active);
                                    for (var i = 0; i < values.Length; i++)
                                        cmd.Parameters.AddWithValue($"@HR{(i + offset):X2}", (int)values[i]);
                                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                                    cmd.Parameters.AddWithValue("@Way", way);
                                    cmd.Parameters.AddWithValue("@Product", product);
                                    cmd.Parameters.AddWithValue("@Riser", riser);
                                    cmd.ExecuteNonQuery();
                                }
                                if (changeActiveFound)
                                    UpdateLog(DateTime.Now, active, overpass, way, product, riser, con, transaction);
                            }
                            else
                            {
                                // формирование запроса для вставки
                                var names = new List<string> { "Active" };
                                //var names = new List<string>();
                                names.AddRange(Enumerable.Range(offset, values.Length).Select(item => $"HR{item:X2}"));
                                var pars = new List<string> { "@Active" };
                                //var pars = new List<string>();
                                pars.AddRange(Enumerable.Range(offset, values.Length).Select(item => $"@HR{item:X2}"));
                                sql = $"INSERT INTO [FETCHING] (Overpass,Way,Product,Riser,{string.Join(",", names)}) VALUES(@Overpass,@Way,@Product,@Riser,{string.Join(",", pars)})";
                                using (var cmd = new SqlCommand(sql, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                                    cmd.Parameters.AddWithValue("@Way", way);
                                    cmd.Parameters.AddWithValue("@Product", product);
                                    cmd.Parameters.AddWithValue("@Riser", riser);
                                    cmd.Parameters.AddWithValue("@Active", active);
                                    for (var i = 0; i < values.Length; i++)
                                        cmd.Parameters.AddWithValue($"@HR{(i + offset):X2}", (int)values[i]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            LastError = null;
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                            transaction.Rollback();
                        }
                    }
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public void ClearRiserTask(int overpass, int way, string productCode, int riser)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    // формирование запроса для изменения
                    var sql = "UPDATE [FETCHING] SET Number=@Number,NType=@NType,HR07=@HR07,HR08=@HR08 WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Number", DBNull.Value);
                        cmd.Parameters.AddWithValue("@NType", DBNull.Value);
                        cmd.Parameters.AddWithValue("@HR07", DBNull.Value);
                        cmd.Parameters.AddWithValue("@HR08", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Overpass", overpass);
                        cmd.Parameters.AddWithValue("@Way", way);
                        cmd.Parameters.AddWithValue("@Product", productCode);
                        cmd.Parameters.AddWithValue("@Riser", riser);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
        }

        public TaskData FindTask(int overpass, int way, string productCode, int riser)
        {
            var result = new TaskData("", 0, 0, 0);
            using (var con = new SqlConnection(Connection))
            {
                var sql = "SELECT [Number],[NType],[HR05],[HR07],[HR08],[HR35],[HR36] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                    cmd.Parameters.AddWithValue("@Way", way);
                    cmd.Parameters.AddWithValue("@Product", productCode);
                    cmd.Parameters.AddWithValue("@Riser", riser);
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    result.Number = ConvertToStr(reader["Number"]);
                                    result.NType = ConvertToInt(reader["NType"]);
                                    result.State = ConvertToInt(reader["HR05"]);
                                    result.RealHeight = ConvertToInt(reader["HR07"]);
                                    result.Setpoint = ConvertToInt(reader["HR08"]);
                                    result.DeepLevel = ConvertToInt(reader["HR35"]);
                                    result.WorkRange = ConvertToInt(reader["HR36"]);
                                    break;
                                }
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                }
            }
            return result;
        }

        private string ConvertToStr(object value)
        {
            if (Convert.IsDBNull(value))
                return string.Empty;
            return Convert.ToString(value);
        }

        private int ConvertToInt(object value)
        {
            if (Convert.IsDBNull(value))
                return 0;
            return Convert.ToInt32(value);
        }

        private bool ConvertToBoolean(object value)
        {
            if (Convert.IsDBNull(value))
                return false;
            return Convert.ToBoolean(value);
        }

        public void UpdateTask(int overpass, int way, string productCode, int riser, string number, int ntype, int realHeight, int setpoint)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    try
                    {
                        using (var transaction = con.BeginTransaction())
                        {
                            try
                            {
                                string sql = $"SELECT Count(*) FROM [WAGONS] WHERE [Number]=@Number";
                                bool found;
                                using (var cmd = new SqlCommand(sql, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@Number", number);
                                    LastError = null;
                                    found = (int)cmd.ExecuteScalar() > 0;
                                }
                                if (!found)
                                {
                                    sql = "INSERT INTO [WAGONS] (Number,NType,RealHeight,FillCount) VALUES(@Number,@NType,@RealHeight,@FillCount)";
                                    using (var cmd = new SqlCommand(sql, con, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@Number", number);
                                        cmd.Parameters.AddWithValue("@NType", ntype);
                                        cmd.Parameters.AddWithValue("@RealHeight", realHeight);
                                        cmd.Parameters.AddWithValue("@FillCount", 0);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                sql = "UPDATE [FETCHING] SET Number=@Number,NType=@NType,HR07=@HR07,HR08=@HR08 WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                                using (var cmd = new SqlCommand(sql, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@Number", number);
                                    cmd.Parameters.AddWithValue("@NType", ntype);
                                    cmd.Parameters.AddWithValue("@HR07", realHeight);
                                    cmd.Parameters.AddWithValue("@HR08", setpoint);
                                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                                    cmd.Parameters.AddWithValue("@Way", way);
                                    cmd.Parameters.AddWithValue("@Product", productCode);
                                    cmd.Parameters.AddWithValue("@Riser", riser);
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                                LastError = null;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                LastError = ex;
                            }
                        }
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
        }

        public void UpdateConfig(int overpass, int way, string product, int riser, int regAddr, ushort[] values)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    var offset = regAddr - 1;
                    var count = 11; // код команды и до десяти параметров
                    // формирование запроса для изменения
                    var list = new List<string>();
                    list.AddRange(Enumerable.Range(offset, count).Select(item => $"HR{item:X2}=@HR{item:X2}"));
                    var sql = $"UPDATE [FETCHING] SET {string.Join(",", list)} WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        for (var i = 0; i < values.Length; i++)
                            cmd.Parameters.AddWithValue($"@HR{(i + offset):X2}", (int)values[i]);
                        for (var i = values.Length; i < count; i++)
                            cmd.Parameters.AddWithValue($"@HR{(i + offset):X2}", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Overpass", overpass);
                        cmd.Parameters.AddWithValue("@Way", way);
                        cmd.Parameters.AddWithValue("@Product", product);
                        cmd.Parameters.AddWithValue("@Riser", riser);
                        cmd.ExecuteNonQuery();
                    }
                    LastError = null;
                    con.Close();
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
        }

        public ushort[] FindConfig(int overpass, int way, string productCode, int riser)
        {
            var result = new List<ushort>();
            using (var con = new SqlConnection(Connection))
            {
                var list = new List<string>();
                list.AddRange(Enumerable.Range(9, 11).Select(item => $"HR{item:X2}"));
                var sql = $"SELECT {string.Join(",", list)} FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                    cmd.Parameters.AddWithValue("@Way", way);
                    cmd.Parameters.AddWithValue("@Product", productCode);
                    cmd.Parameters.AddWithValue("@Riser", riser);
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                foreach (var name in list)
                                {
                                    var value = reader[name];
                                    if (value == DBNull.Value) break;
                                    result.Add((ushort)(int)reader[name]);
                                }
                                break;
                            }
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                }
            }
            return result.ToArray();
        }

        public string[] GetProductList(int overpass, int way)
        {
            var list = new List<string>();
            using (var con = new SqlConnection(Connection))
            {
                var sql = "SELECT DISTINCT [Product] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                    cmd.Parameters.AddWithValue("@Way", way);
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    list.Add((string)reader[0]);
                                }
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                    return list.ToArray();
                }
            }
        }

        public int[] GetWayList(int overpass)
        {
            var list = new List<int>();
            using (var con = new SqlConnection(Connection))
            {
                var sql = "SELECT DISTINCT [Way] FROM [FETCHING] WHERE [Overpass]=@Overpass";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            while (reader.Read())
                            {
                                list.Add((int)reader[0]);
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                    return list.ToArray();
                }
            }
        }

        public int[] GetOverpassList()
        {
            var list = new List<int>();
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    var sql = "SELECT DISTINCT [Overpass] FROM [FETCHING]";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            while (reader.Read())
                            {
                                list.Add((int)reader[0]);
                            }
                            LastError = null;
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex;
                }
            }
            return list.ToArray();
        }

        public DataTable GetWagonTypes()
        {
            using (var con = new SqlConnection(Connection))
            {
                var sql = "SELECT [Ntype],[Diameter],[Throat],[Deflevel] FROM [TYPES]";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "TYPES");
                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                        }
                        return ds.Tables.Count == 1 ? ds.Tables[0] : null;
                    }
                }
            }
        }

        public int GetNtype(int overpass, int way, string product, int riser)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    try
                    {
                        var sql = "SELECT [NType] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                        using (var cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@Overpass", overpass);
                            cmd.Parameters.AddWithValue("@Way", way);
                            cmd.Parameters.AddWithValue("@Product", product);
                            cmd.Parameters.AddWithValue("@Riser", riser);
                            LastError = null;
                            return ConvertToInt(cmd.ExecuteScalar());
                        }
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return 0;
                }
            }
        }

        public bool InsertIntoWagonTypes(int ntype, int diameter, int throat, int defLevel)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    // формирование запроса для вставки
                    var sql = $"INSERT INTO [TYPES] (NType,Diameter,Throat,DefLevel) VALUES(@NType,@Diameter,@Throat,@DefLevel)";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@NType", ntype);
                        cmd.Parameters.AddWithValue("@Diameter", diameter);
                        cmd.Parameters.AddWithValue("@Throat", throat);
                        cmd.Parameters.AddWithValue("@DefLevel", defLevel);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool DeleteIntoWagonTypes(int ntype)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    var sql = "DELETE FROM [TYPES] WHERE [NType]=@NType";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@NType", ntype);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool UpdateIntoWagonTypes(int ntype, int diameter, int throat, int defLevel)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    // формирование запроса для изменения
                    var sql = "UPDATE [TYPES] SET Diameter=@Diameter,Throat=@Throat,DefLevel=@DefLevel WHERE [NType]=@NType";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Diameter", diameter);
                        cmd.Parameters.AddWithValue("@Throat", throat);
                        cmd.Parameters.AddWithValue("@DefLevel", defLevel);
                        cmd.Parameters.AddWithValue("@NType", ntype);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public WagonTypeData FindWagonType(int ntype)
        {
            var data = new WagonTypeData(0, 0, 0, 0);
            using (var con = new SqlConnection(Connection))
            {
                string sql = "SELECT [NType],[Diameter],[Throat],[Deflevel] FROM [TYPES] WHERE [NType]=@NType";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@NType", ntype);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "TYPES");
                            if (ds.Tables.Count == 1)
                            {
                                foreach (var row in ds.Tables[0].Rows.Cast<DataRow>())
                                {
                                    data.NType = (int)row["NType"];
                                    data.Diameter = (int)row["Diameter"];
                                    data.Throat = (int)row["Throat"];
                                    data.Deflevel = (int)row["Deflevel"];
                                    break;
                                }
                            }
                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                        }
                    }
                }
            }
            return data;
        }

        public WagonData FindWagon(string number)
        {
            var data = new WagonData(string.Empty, 0, 0);
            using (var con = new SqlConnection(Connection))
            {
                string sql = "SELECT [Number],[NType],[RealHeight],[FillCount] FROM [WAGONS] WHERE [Number]=@Number";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Number", number);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "WAGONS");
                            if (ds.Tables.Count == 1)
                            {
                                foreach (var row in ds.Tables[0].Rows.Cast<DataRow>())
                                {
                                    data.Number = (string)row["Number"];
                                    data.NType = (int)row["NType"];
                                    data.RealHeight = (int)row["RealHeight"];
                                    data.FillCount = (int)row["FillCount"];
                                    break;
                                }
                            }

                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                        }
                    }
                }
            }
            return data;
        }

        public DataTable GetOperators()
        {
            using (var con = new SqlConnection(Connection))
            {
                string sql = "SELECT [Lastname],[Firstname],[Secondname],[Access],[Department],[Appointment],[Password] FROM [OPERATORS]";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "OPERATORS");
                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                        }
                        return ds.Tables.Count == 1 ? ds.Tables[0] : null;
                    }
                }
            }
        }

        public OperatorData GetOperator(string lastname, string firstname, string secondname)
        {
            var oper = new OperatorData("", "", "", 0, "", "", "");
            using (var con = new SqlConnection(Connection))
            {
                string sql = "SELECT [Access],[Department],[Appointment],[Password] FROM [OPERATORS] WHERE [Lastname]=@Lastname AND [Firstname]=@Firstname AND [Secondname]=@Secondname";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Lastname", lastname);
                    cmd.Parameters.AddWithValue("@Firstname", firstname);
                    cmd.Parameters.AddWithValue("@Secondname", secondname);
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    oper = new OperatorData(
                                        lastname,
                                        firstname,
                                        secondname,
                                        (int)reader["Access"],
                                        (string)reader["Department"],
                                        (string)reader["Appointment"],
                                        (string)reader["Password"]);
                                    break;
                                }
                                LastError = null;
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                }
            }
            return oper;
        }

        public bool CheckOperatorPassword(string lastname, string firstname, string secondname, string password)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    var found = false;
                    con.Open();
                    // формирование запроса для замены или вставки
                    var sql = "SELECT COUNT(*) FROM [OPERATORS] WHERE [Lastname]=@Lastname AND [Firstname]=@Firstname AND [Secondname]=@Secondname AND [Password]=@Password";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Lastname", lastname);
                        cmd.Parameters.AddWithValue("@Firstname", firstname);
                        cmd.Parameters.AddWithValue("@Secondname", secondname);
                        cmd.Parameters.AddWithValue("@Password", password);
                        found = (int)cmd.ExecuteScalar() > 0;
                    }
                    con.Close();
                    LastError = null;
                    return found;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool CheckOperatorAccessExists(int access)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    var found = false;
                    con.Open();
                    // формирование запроса для замены или вставки
                    var sql = "SELECT COUNT(*) FROM [OPERATORS] WHERE [Access]=@Access";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Access", access);
                        found = (int)cmd.ExecuteScalar() > 0;
                    }
                    con.Close();
                    LastError = null;
                    return found;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool InsertIntoOperators(string lastname, string firstname, string secondname, int access, string department, string appointment, string password)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    // формирование запроса для вставки
                    var sql = "INSERT INTO [OPERATORS] ([Lastname],[Firstname],[Secondname],[Access],[Department],[Appointment],[Password]) VALUES(@Lastname,@Firstname,@Secondname,@Access,@Department,@Appointment,@Password)";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Lastname", lastname);
                        cmd.Parameters.AddWithValue("@Firstname", firstname);
                        cmd.Parameters.AddWithValue("@Secondname", secondname);
                        cmd.Parameters.AddWithValue("@Access", access);
                        cmd.Parameters.AddWithValue("@Department", department);
                        cmd.Parameters.AddWithValue("@Appointment", appointment);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool UpdateIntoOperators(string lastname, string firstname, string secondname, string password)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    // формирование запроса для изменения
                    var sql = "UPDATE [OPERATORS] SET [Password]=@Password WHERE [Lastname]=@Lastname AND [Firstname]=@Firstname AND [Secondname]=@Secondname";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Lastname", lastname);
                        cmd.Parameters.AddWithValue("@Firstname", firstname);
                        cmd.Parameters.AddWithValue("@Secondname", secondname);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool DeleteIntoOperators(string lastname, string firstname, string secondname)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    var sql = "DELETE FROM [OPERATORS] WHERE [Lastname]=@Lastname AND [Firstname]=@Firstname AND [Secondname]=@Secondname";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Lastname", lastname);
                        cmd.Parameters.AddWithValue("@Firstname", firstname);
                        cmd.Parameters.AddWithValue("@Secondname", secondname);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public int GetLogReportRowsCount()
        {
            using (var con = new SqlConnection(Connection))
            {
                con.Open();
                string sql = "SELECT COUNT(*)" +
                             " FROM [LOGREPORT]" +
                             " WHERE Snaptime >= @FirstTime AND Snaptime <= @LastTime";
                if (CommonData.FilterOverpassList.Length > 0)
                {
                    sql += $" AND Overpass=@Overpass";
                    if (CommonData.FilterWayList.Length > 0)
                    {
                        sql += $" AND Way=@Way";
                        if (CommonData.FilterProductList.Length > 0)
                            sql += $" AND Product=@Product AND Riser>=@Riser1st AND Riser<=@Riser2nd";
                    }
                }
                int countRows;
                using (var cmd = new SqlCommand(sql, con))
                {
                    var rangeSnaps = CommonData.FilterDateTimeRange;
                    cmd.Parameters.AddWithValue("@FirstTime", rangeSnaps.FirstExists ? rangeSnaps.First : new DateTime(2000, 1, 1));
                    cmd.Parameters.AddWithValue("@LastTime", rangeSnaps.LastExists ? rangeSnaps.Last : new DateTime(2099, 12, 31));
                    if (CommonData.FilterOverpassList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@Overpass", CommonData.FilterOverpassList.Length > 0 ? int.Parse(CommonData.FilterOverpassList[0]) : 0);
                        if (CommonData.FilterWayList.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Way", CommonData.FilterWayList.Length > 0 ? int.Parse(CommonData.FilterWayList[0]) : 0);
                            if (CommonData.FilterProductList.Length > 0)
                            {
                                cmd.Parameters.AddWithValue("@Product", CommonData.FilterProductList.Length > 0 ? CommonData.FilterProductList[0] : "%");
                                var rangeRisers = CommonData.FilterRiserRange;
                                cmd.Parameters.AddWithValue("@Riser1st", rangeRisers.Item1);
                                cmd.Parameters.AddWithValue("@Riser2nd", rangeRisers.Item2);
                            }
                        }
                    }
                    countRows = (int)cmd.ExecuteScalar();
                }
                con.Close();
                return countRows;
            }
        }

        //public int GetCountPages(int rowsCount)
        //{
        //    using (var con = new SqlConnection(Connection))
        //    {
        //        con.Open();
        //        string sql = $"SELECT FLOOR(COUNT(*)/{rowsCount}) FROM [LOGREPORT] WHERE Snaptime >= @FirstTime AND Snaptime <= @LastTime";
        //        if (CommonData.FilterOverpassList.Length > 0)
        //        {
        //            sql += $" AND Overpass=@Overpass";
        //            if (CommonData.FilterWayList.Length > 0)
        //            {
        //                sql += $" AND Way=@Way";
        //                if (CommonData.FilterProductList.Length > 0)
        //                {
        //                    sql += $" AND Product=@Product AND Riser>=@Riser1st AND Riser<=@Riser2nd";

        //                }
        //            }
        //        }
        //        int countPages;
        //        using (var cmd = new SqlCommand(sql, con))
        //        {
        //            var rangeSnaps = CommonData.FilterDateTimeRange;
        //            cmd.Parameters.AddWithValue("@FirstTime", rangeSnaps.FirstExists ? rangeSnaps.First : new DateTime(2000, 1, 1));
        //            cmd.Parameters.AddWithValue("@LastTime", rangeSnaps.LastExists ? rangeSnaps.Last : new DateTime(2099, 12, 31));
        //            if (CommonData.FilterOverpassList.Length > 0)
        //            {
        //                cmd.Parameters.AddWithValue("@Overpass", CommonData.FilterOverpassList.Length > 0 ? int.Parse(CommonData.FilterOverpassList[0]) : 0);
        //                if (CommonData.FilterWayList.Length > 0)
        //                {
        //                    cmd.Parameters.AddWithValue("@Way", CommonData.FilterWayList.Length > 0 ? int.Parse(CommonData.FilterWayList[0]) : 0);
        //                    if (CommonData.FilterProductList.Length > 0)
        //                    {
        //                        cmd.Parameters.AddWithValue("@Product", CommonData.FilterProductList.Length > 0 ? CommonData.FilterProductList[0] : "%");
        //                        var rangeRisers = CommonData.FilterRiserRange;
        //                        cmd.Parameters.AddWithValue("@Riser1st", rangeRisers.Item1);
        //                        cmd.Parameters.AddWithValue("@Riser2nd", rangeRisers.Item2);
        //                    }
        //                }
        //            }
        //            countPages = (int)cmd.ExecuteScalar() + 1;
        //        }
        //        con.Close();
        //        return countPages;
        //    }
        //}

        //private string GetSQLTime(DateTime date)
        //{
        //    return $"{date.Month}/{date.Day}/{date.Year} {date.Hour}:{date.Minute}:{date.Second}";
        //}

        //public DataTable GetLogReport(int pageNumber, int rowsCount)
        //{
        //    using (var con = new SqlConnection(Connection))
        //    {
        //        string sql = "SELECT Snaptime,Overpass,Way,Product,Riser,EventInfo,Number,NType,MaxHeight,Source,SetLevel" +
        //                     " FROM [Logreport_GetPage](@PageNumber,@RowsCount,@FirstTime,@LastTime,@Overpass,@Way,@Product,@Riser1st,@Riser2nd)" +
        //                     " ORDER BY Snaptime ASC";
        //        using (var cmd = new SqlCommand(sql, con))
        //        {
        //            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //            cmd.Parameters.AddWithValue("@RowsCount", rowsCount);
        //            var rangeSnaps = CommonData.FilterDateTimeRange;
        //            cmd.Parameters.AddWithValue("@FirstTime", rangeSnaps.FirstExists ? rangeSnaps.First : new DateTime(2000, 1, 1));
        //            cmd.Parameters.AddWithValue("@LastTime", rangeSnaps.LastExists ? rangeSnaps.Last : new DateTime(2099, 12, 31));
        //            cmd.Parameters.AddWithValue("@Overpass", CommonData.FilterOverpassList.Length > 0 ? int.Parse(CommonData.FilterOverpassList[0]) : 0);
        //            cmd.Parameters.AddWithValue("@Way", CommonData.FilterWayList.Length > 0 ? int.Parse(CommonData.FilterWayList[0]) : 0);
        //            cmd.Parameters.AddWithValue("@Product", CommonData.FilterProductList.Length > 0 ? CommonData.FilterProductList[0] : "%");
        //            var rangeRisers = CommonData.FilterRiserRange;
        //            cmd.Parameters.AddWithValue("@Riser1st", rangeRisers.Item1);
        //            cmd.Parameters.AddWithValue("@Riser2nd", rangeRisers.Item2);
        //            using (var da = new SqlDataAdapter(cmd))
        //            {
        //                var ds = new DataSet();
        //                try
        //                {
        //                    da.Fill(ds, "LOGREPORT");
        //                    LastError = null;
        //                }
        //                catch (Exception ex)
        //                {
        //                    LastError = ex;
        //                }
        //                return ds.Tables.Count == 1 ? ds.Tables[0] : null;
        //            }
        //        }
        //    }
        //}

        public DataTable GetLogReport(int startIndex, int endIndex)
        {
            using (var con = new SqlConnection(Connection))
            {
                string sql = "SELECT [Snaptime],[Overpass],[Way],[Product],[Riser],[Category],[EventInfo],[Number],[NType],[MaxHeight],[Source],[SetLevel]" +
                             " FROM [LOGREPORT]" +
                             " WHERE Snaptime >= @FirstTime AND Snaptime <= @LastTime";
                if (CommonData.FilterOverpassList.Length > 0)
                {
                    sql += $" AND Overpass=@Overpass";
                    if (CommonData.FilterWayList.Length > 0)
                    {
                        sql += $" AND Way=@Way";
                        if (CommonData.FilterProductList.Length > 0)
                            sql += $" AND Product=@Product AND Riser>=@Riser1st AND Riser<=@Riser2nd";
                    }
                }
                sql += " ORDER BY [Snaptime]";
                var count = endIndex - startIndex;
                if (count <= 0) count = 1;
                sql += $" OFFSET {startIndex} ROWS FETCH NEXT {count} ROWS ONLY";
                using (var cmd = new SqlCommand(sql, con))
                {
                    var rangeSnaps = CommonData.FilterDateTimeRange;
                    cmd.Parameters.AddWithValue("@FirstTime", rangeSnaps.FirstExists ? rangeSnaps.First : new DateTime(2000, 1, 1));
                    cmd.Parameters.AddWithValue("@LastTime", rangeSnaps.LastExists ? rangeSnaps.Last : new DateTime(2099, 12, 31));
                    if (CommonData.FilterOverpassList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@Overpass", CommonData.FilterOverpassList.Length > 0 ? int.Parse(CommonData.FilterOverpassList[0]) : 0);
                        if (CommonData.FilterWayList.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Way", CommonData.FilterWayList.Length > 0 ? int.Parse(CommonData.FilterWayList[0]) : 0);
                            if (CommonData.FilterProductList.Length > 0)
                            {
                                cmd.Parameters.AddWithValue("@Product", CommonData.FilterProductList.Length > 0 ? CommonData.FilterProductList[0] : "%");
                                var rangeRisers = CommonData.FilterRiserRange;
                                cmd.Parameters.AddWithValue("@Riser1st", rangeRisers.Item1);
                                cmd.Parameters.AddWithValue("@Riser2nd", rangeRisers.Item2);
                            }
                        }
                    }
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "LOGREPORT");
                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                        }
                        return ds.Tables.Count == 1 ? ds.Tables[0] : null;
                    }
                }
            }
        }

        public DataTable GetLogReport()
        {
            using (var con = new SqlConnection(Connection))
            {
                string sql = "SELECT [Snaptime],[Overpass],[Way],[Product],[Riser],[Category],[EventInfo],[Number],[NType],[MaxHeight],[Source],[SetLevel] FROM [LOGREPORT]";
                var whereList = new List<string>();
                if (CommonData.FilterOverpassList.Length > 0)
                    whereList.Add($"({string.Join(" OR ", CommonData.FilterOverpassList.Select(item => "[Overpass]=" + item))})");
                if (CommonData.FilterWayList.Length > 0)
                    whereList.Add($"({string.Join(" OR ", CommonData.FilterWayList.Select(item => "[Way]=" + item))})");
                if (CommonData.FilterProductList.Length > 0)
                    whereList.Add($"({string.Join(" OR ", CommonData.FilterProductList.Select(item => $"[Product]='{item}'"))})");

                var rangeRisers = CommonData.FilterRiserRange;
                whereList.Add($"([Riser]=0 OR [Riser] BETWEEN {rangeRisers.Item1} AND {rangeRisers.Item2})");

                var rangeSnaps = CommonData.FilterDateTimeRange;
                if (rangeSnaps.FirstExists)
                    whereList.Add($"[Snaptime] >= @FirstTime");
                if (rangeSnaps.LastExists)
                    whereList.Add($"[Snaptime] <= @LastTime");

                var events = CommonData.FilterEventsList;
                if (events.Length > 0)
                    whereList.Add($"({string.Join(" OR ", events.Select(item => $"[EventInfo] LIKE '{item}%'"))})");

                if (whereList.Count > 0)
                    sql += $" WHERE ({string.Join(" AND ", whereList)})";
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (rangeSnaps.FirstExists)
                        cmd.Parameters.AddWithValue("@FirstTime", rangeSnaps.First);
                    if (rangeSnaps.LastExists)
                        cmd.Parameters.AddWithValue("@LastTime", rangeSnaps.Last);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "LOGREPORT");
                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                        }
                        return ds.Tables.Count == 1 ? ds.Tables[0] : null;
                    }
                }
            }
        }

        public DataTable GetTopLogReport(int count)
        {
            using (var con = new SqlConnection(Connection))
            {
                string sql = $"SELECT TOP {count} [Snaptime],[Overpass],[Way],[Product],[Riser],[Category],[EventInfo],[Number],[NType],[MaxHeight],[Source],[SetLevel] FROM [LOGREPORT] ORDER BY [Snaptime] DESC";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "LOGREPORT");
                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                        }
                        return ds.Tables.Count == 1 ? ds.Tables[0] : null;
                    }
                }
            }
        }

        public int GetWagonsRowsCount()
        {
            using (var con = new SqlConnection(Connection))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM [WAGONS]";
                int countRows;
                using (var cmd = new SqlCommand(sql, con))
                {
                    countRows = (int)cmd.ExecuteScalar();
                }
                con.Close();
                return countRows;
            }
        }

        public DataTable GetWagons(int startIndex, int endIndex)
        {
            using (var con = new SqlConnection(Connection))
            {
                string sql = "SELECT [Number],[Ntype],[RealHeight],[FillCount] FROM [WAGONS] ORDER BY [Number]";
                var count = endIndex - startIndex;
                if (count <= 0) count = 1;
                sql += $" OFFSET {startIndex} ROWS FETCH NEXT {count} ROWS ONLY";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "WAGONS");
                            LastError = null;
                        }
                        catch (Exception ex)
                        {
                            LastError = ex;
                        }
                        return ds.Tables.Count == 1 ? ds.Tables[0] : null;
                    }
                }
            }
        }

        public string GetWagon(int index)
        {
            using (var con = new SqlConnection(Connection))
            {
                con.Open();
                string sql = "SELECT [Number] FROM [WAGONS] ORDER BY [Number]";
                sql += $" OFFSET {index} ROWS FETCH NEXT 1 ROWS ONLY";
                string number = "";
                using (var cmd = new SqlCommand(sql, con))
                {
                    number = (string)cmd.ExecuteScalar();
                }
                con.Close();
                return number;
            }
        }

        public int GetWagonIndex(string number)
        {
            using (var con = new SqlConnection(Connection))
            {
                con.Open();
                string sql = "SELECT RowNumber FROM (SELECT ROW_NUMBER() OVER (ORDER BY [Number] ASC) AS RowNumber, [Number]" +
                             " FROM [PNVC].[dbo].[WAGONS]) AS W" +
                             " WHERE W.Number = @Number";
                int index = -1;
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Number", number);
                    var value = cmd.ExecuteScalar();
                    index = Convert.ToInt32(value);
                }
                con.Close();
                return index;
            }
        }

        public bool InsertIntoWagons(string number, int ntype, int realHeight)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    // формирование запроса для вставки
                    var sql = "INSERT INTO [WAGONS] (Number,NType,RealHeight,FillCount) VALUES(@Number,@NType,@RealHeight,@FillCount)";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Number", number);
                        cmd.Parameters.AddWithValue("@NType", ntype);
                        cmd.Parameters.AddWithValue("@RealHeight", realHeight);
                        cmd.Parameters.AddWithValue("@FillCount", 0);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool UpdateIntoWagons(string number, int ntype, int realHeight)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    // формирование запроса для изменения
                    var sql = "UPDATE [WAGONS] SET NType=@NType,RealHeight=@RealHeight WHERE [Number]=@Number";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@NType", ntype);
                        cmd.Parameters.AddWithValue("@RealHeight", realHeight);
                        cmd.Parameters.AddWithValue("@Number", number);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool IncreaseFillCountIntoWagons(string number)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    // формирование запроса для изменения
                    var sql = "UPDATE [WAGONS] SET FillCount=FillCount+1 WHERE [Number]=@Number";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Number", number);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public bool DeleteIntoWagons(string number)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    var sql = "DELETE FROM [WAGONS] WHERE [Number]=@Number";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Number", number);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    LastError = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
        }

        public int GetSetpoint(int overpass, int way, string product, int riser)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    try
                    {
                        var sql = "SELECT [HR08] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                        using (var cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@Overpass", overpass);
                            cmd.Parameters.AddWithValue("@Way", way);
                            cmd.Parameters.AddWithValue("@Product", product);
                            cmd.Parameters.AddWithValue("@Riser", riser);
                            LastError = null;
                            return ConvertToInt(cmd.ExecuteScalar());
                        }
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return 0;
                }
            }
        }

        public int GetCurrent(int overpass, int way, string product, int riser)
        {
            using (var con = new SqlConnection(Connection))
            {
                try
                {
                    con.Open();
                    try
                    {
                        var sql = "SELECT [HR24] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                        using (var cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@Overpass", overpass);
                            cmd.Parameters.AddWithValue("@Way", way);
                            cmd.Parameters.AddWithValue("@Product", product);
                            cmd.Parameters.AddWithValue("@Riser", riser);
                            LastError = null;
                            return ConvertToInt(cmd.ExecuteScalar());
                        }
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return 0;
                }
            }
        }

        public ushort[] GetRegisters(int overpass, int way, string product, int riser, int regAddr, int regCount)
        {
            var offset = regAddr - 1;
            var names = new List<string>() { "Active" };
            names.AddRange(Enumerable.Range(offset, regCount).Select(item => $"HR{item:X2}"));
            var list = new List<ushort>();
            using (var con = new SqlConnection(Connection))
            {
                string sql = $"SELECT {string.Join(",", names)} FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                    cmd.Parameters.AddWithValue("@Way", way);
                    cmd.Parameters.AddWithValue("@Product", product);
                    cmd.Parameters.AddWithValue("@Riser", riser);
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    var active = ConvertToBoolean(reader["Active"]);
                                    if (!active) break;
                                    foreach (var name in names.Skip(1))
                                        list.Add((ushort)ConvertToInt(reader[name]));
                                }
                                LastError = null;
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                }
            }
            return list.ToArray();
        }

        public List<RiserKey> GetRisers(int overpass, int way, string product)
        {
            var list = new List<RiserKey>();
            using (var con = new SqlConnection(Connection))
            {
                string sql = "SELECT [Npp],[Riser],[IpAddress],[IpPort],[Node],[Func] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                    cmd.Parameters.AddWithValue("@Way", way);
                    cmd.Parameters.AddWithValue("@Product", product ?? "");
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    var npp = (int)reader["Npp"];
                                    var riser = (int)reader["Riser"];
                                    var ipAddr = (string)reader["IpAddress"];
                                    var ipPort = (int)reader["IpPort"];
                                    var nodeAddr = (int)reader["Node"];
                                    var func = (int)reader["Func"];
                                    list.Add(new RiserKey(npp, overpass, way, product, riser, ipAddr, ipPort, nodeAddr, func));
                                }
                                LastError = null;
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                }
            }
            return list;
        }

        public RiserKey GetRiser(int overpass, int way, string product, int riser)
        {
            var key = new RiserKey();
            using (var con = new SqlConnection(Connection))
            {
                string sql = $"SELECT [Npp],[Riser],[IpAddress],[IpPort],[Node],[Func] FROM [FETCHING] WHERE [Overpass]=@Overpass AND [Way]=@Way AND [Product]=@Product AND [Riser]=@Riser";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Overpass", overpass);
                    cmd.Parameters.AddWithValue("@Way", way);
                    cmd.Parameters.AddWithValue("@Product", product ?? "");
                    cmd.Parameters.AddWithValue("@Riser", riser);
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    key = new RiserKey(
                                        (int)reader["Npp"],
                                        overpass,
                                        way,
                                        product,
                                        (int)reader["Riser"],
                                        (string)reader["IpAddress"],
                                        (int)reader["IpPort"],
                                        (int)reader["Node"],
                                        (int)reader["Func"]);
                                    break;
                                }
                                LastError = null;
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                }
            }
            return key;
        }


        public RiserKey GetRiser(int npp)
        {
            var key = new RiserKey();
            using (var con = new SqlConnection(Connection))
            {
                string sql = $"SELECT [Overpass],[Way],[Product],[Riser] FROM [FETCHING] WHERE [Npp]=@Npp";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Npp", npp);
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    key = new RiserKey(
                                        npp,
                                        (int)reader["Overpass"],
                                        (int)reader["Way"],
                                        (string)reader["Product"],
                                        (int)reader["Riser"], "", 0, 0, 0);
                                    break;
                                }
                                LastError = null;
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = ex;
                    }
                }
            }
            return key;
        }

    }
}
