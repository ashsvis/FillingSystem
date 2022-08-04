using System;

namespace FillingSystemHelper
{
    public delegate void RiserConfigEventHandler(object sender, RiserConfigEventArgs e);

    public class RiserConfigEventArgs : EventArgs
    {
        public int Overpass { get; set; }
        public int Way { get; set; }
        public string Product { get; set; }
        public int Riser { get; set; }
        public string IpAddress { get; set; }
        public int IpPort { get; set; }
        public byte Node { get; set; }
        public byte Func { get; set; }
    }

}
