using System;

namespace FillingSystemViewHelper
{
    public delegate void WagonEventHandler(object sender, WagonEventArgs e);

    public class WagonEventArgs : EventArgs
    {
        public string Number { get; set; }
        public int Ntype { get; set; }
        public int RealHeight { get; set; }
    }

}
