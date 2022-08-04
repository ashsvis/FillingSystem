using System.IO;
using System.Net;

namespace FillingSystemHelper
{
    public class EthernetTuning : Tuning
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }

        public EthernetTuning(string ipAddress, int port)
        {
            Address = IPAddress.Parse(ipAddress);
            Port = port;
        }

        public EthernetTuning(string baseDirectory, string serviceName)
        {
            var mif = new MemIniFile(Path.Combine(baseDirectory, $"{serviceName}.ini"));
            ConnectionString = mif.ReadString("SqlServer", "ConnectionString", "");
            if (int.TryParse(mif.ReadString("Segment", "Overpass", "0"), out int overpass) &&
                int.TryParse(mif.ReadString("Segment", "Way", "0"), out int way))
            {
                var product = mif.ReadString("Segment", "Product", "");
                var server = new FillingSqlServer() { Connection = ConnectionString };
                RiserKeys.AddRange(server.GetRisers(overpass, way, product));
            }
        }
    }
}
