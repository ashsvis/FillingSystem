using System;

namespace FillingSystemViewHelper
{
    public delegate void TaskEventHandler(object sender, TaskEventArgs e);

    public class TaskEventArgs : EventArgs
    {
        public int Overpass { get; set; }
        public int Way { get; set; }
        public string Product { get; set; }
        public int Riser { get; set; }
        public string Number { get; set; }
        public int Ntype { get; set; }
        public int RealHeight { get; set; }
        public int Setpoint { get; set; }
    }

}
