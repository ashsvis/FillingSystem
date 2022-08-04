namespace FillingSystemHelper
{
    /// <summary>
    /// Адрес стояка в системе опроса
    /// </summary>
    public struct RiserKey
    {
        public RiserKey(int npp, int overpass, int way, string product, int riser, string ipAddr, int ipPort, int nodeAddr, int func)
        {
            Npp = npp;
            Overpass = overpass;
            Way = way;
            Product = product;
            Riser = riser;
            IpAddress = ipAddr;
            IpPort = ipPort;
            NodeAddr = nodeAddr;
            Func = func;
        }

        public int Npp { get; }
        public int Overpass { get; }
        public int Way { get; }
        public string Product { get; }
        public int Riser { get; }
        public string IpAddress { get; }
        public int IpPort { get; }
        public int NodeAddr { get; }
        public int Func { get; }

        public override string ToString()
        {
            return $"{Overpass}.{Way}.{Product}.{Riser}";
        }
    }
}
