namespace FillingSystemHelper
{
    public class WagonTypeData
    {
        public int NType { get; set; }
        public int Diameter { get; set; }
        public int Throat { get; set; }
        public int Deflevel { get; set; }

        private WagonTypeData()
        {
            NType = 0;
            Diameter = 0;
            Throat = 0;
            Deflevel = 0;
        }

        public WagonTypeData(int ntype, int diameter, int throat, int deflevel)
        {
            NType = ntype;
            Diameter = diameter;
            Throat = throat;
            Deflevel = deflevel;
        }

    }
}
