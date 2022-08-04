using System;

namespace FillingSystemViewHelper
{
    public delegate void GetWagonTypesEventHandler(object sender, GetWagonTypesEventArgs e);

    public class GetWagonTypesEventArgs : EventArgs
    {
        public int[] WagonTypes { get; set; } = new int[] { };
    }

}
