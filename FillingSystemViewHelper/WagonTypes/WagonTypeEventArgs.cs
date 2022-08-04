using System;

namespace FillingSystemViewHelper
{
    public delegate void WagonTypeEventHandler(object sender, WagonTypeEventArgs e);

    public class WagonTypeEventArgs : EventArgs
    {
        public int Ntype { get; set; }
        public int Diameter { get; set; }
        public int Throat { get; set; }
        public int DefLevel { get; set; }
    }

}
