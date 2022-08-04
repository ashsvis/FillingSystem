using FillingSystemHelper;
using System;

namespace FillingSystemViewHelper
{
    public delegate void WriteDataEventHandler(object sender, WriteDataEventArgs e);

    public class WriteDataEventArgs : EventArgs
    {
        public RiserKey RiserKey { get; set; }
        public int RegAddr { get; internal set; }
        public ushort[] WriteData { get; internal set; }
        public string[] LogData { get; internal set; }
    }
}
