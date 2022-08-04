namespace FillingSystemView
{
    public class WagonData
    {
        public string Number { get; set; }
        public int NType { get; set; }
        public int RealHeight { get; set; }
        public int FillCount { get; set; }

        private WagonData()
        {
            Number = string.Empty;
            NType = 0;
            RealHeight = 0;
        }

        public WagonData(string number, int ntype, int realHeight)
        {
            Number = number;
            NType = ntype;
            RealHeight = realHeight;
        }

    }
}
