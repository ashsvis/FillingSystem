using System;
using System.Drawing;

namespace FillingSystemViewHelper
{
    public delegate void CloseFormEventHandler(object sender, CloseFormEventArgs e);

    public class CloseFormEventArgs : EventArgs
    {
        public Point Location  { get; set; }
    }
}
