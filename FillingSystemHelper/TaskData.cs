namespace FillingSystemHelper
{
    public class TaskData
    {
        public string Number { get; set; }
        public int NType { get; set; }
        public int Diameter { get; set; }
        public int Throat { get; set; }
        public int RealHeight { get; set; }
        public int Setpoint { get; set; }
        public int DeepLevel { get; set; }
        public int WorkRange { get; set; }
        public int State { get; set; }

        private TaskData()
        {
            Number = string.Empty;
            NType = 0;
            Diameter = 0;
            Throat = 0;
            RealHeight = 0;
            Setpoint = 0;
            DeepLevel = 0;
            WorkRange = 0;
            State = 0;
        }

        public TaskData(string number, int ntype, int realHeight, int setpoint)
        {
            Number = number;
            NType = ntype;
            RealHeight = realHeight;
            Setpoint = setpoint;
        }

    }
}
