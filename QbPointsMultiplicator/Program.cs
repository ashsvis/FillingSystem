using FillingSystemHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QbPointsMultiplicator
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QBDB.pnt");
            if (!File.Exists(fileName)) return;
            var server = new FillingSqlServer { Connection = "Data Source=PNVCSRVB;Initial Catalog=PNVC;User ID=pnvc_admin;Password=!PNVC@r400" };

            var template = File.ReadAllLines(fileName, Encoding.Default);
            var key = "PNVC412B007";
            var nppKey = 206;
            var addrKey = 2051;
            var list = new List<string>();
            for (var npp = 1; npp <= 273; npp++) //273
            {
                var riser = server.GetRiser(npp);
                var baseAddr = (npp - 1) * 10 + 1;
                foreach (var line in template)
                {
                    var output = line.Replace(key, $"PNVC{riser.Overpass}{riser.Way:00}{riser.Product}{riser.Riser:000}");
                    if (output.StartsWith("PVSOURCE") || output.StartsWith("A1SOURCE") || output.StartsWith("A2SOURCE"))
                    {
                        var sourceline = output.Split('\t');
                        if (sourceline.Length == 2)
                        {
                            var vals = sourceline[1].Split(' ');
                            if (vals.Length == 4 && int.TryParse(vals[2], out int sourceAddr))
                            {
                                var newAddr = sourceAddr - addrKey + baseAddr;
                                vals[2] = $"{newAddr}";
                                output = $"{sourceline[0]}\t{string.Join(" ", vals)}";
                            }
                        }
                    }
                    if (output.StartsWith("AREA"))
                    {
                        var sourceline = output.Split('\t');
                        if (sourceline.Length == 3)
                        {
                            var vals = sourceline[2].Split(' ');
                            if (vals.Length == 2)
                            {
                                var way = riser.Way == 35 ? "3_5" : riser.Way.ToString();
                                vals[1] = $"PNVC_{riser.Overpass}_{way}_{riser.Product}";
                                output = $"{sourceline[0]}\t{string.Join(" ", vals)}";
                            }
                        }
                    }
                    if (output.StartsWith("PARAM"))
                    {
                        var sourceline = output.Split(' ');
                        if (sourceline.Length == 5 && sourceline[4].StartsWith("Npp"))
                        {
                            sourceline[4] = sourceline[4].Replace($",{nppKey}}}", $",{npp}}}");
                            output = $"{string.Join(" ", sourceline)}";
                        }
                    }
                    if (output.StartsWith("ADD"))
                    {
                        var sourceline = output.Split('\t');
                        if (sourceline.Length == 3 && sourceline[2].IndexOf("CON00000") >= 0)
                        {
                            var vals = sourceline[2].Split(' ');
                            if (vals.Length > 2)
                            {
                                sourceline[2] = $"{vals[0]} {vals[1]} Эстакада {riser.Overpass} путь {riser.Way} {new ProductSelection(riser.Product)} стояк {riser.Riser}";
                                output = string.Join("\t", sourceline);
                            }
                        }
                    }
                    list.Add(output);
                }
            }
            File.WriteAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMPORT.pnt"), list, Encoding.Default);
        }
    }
}
