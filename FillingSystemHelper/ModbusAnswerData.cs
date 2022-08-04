namespace FillingSystemHelper
{
    public class ModbusAnswerData
    {
        public bool Active { get; set; }
        public byte ErrorCode { get; set; }
        public byte Node { get; set; }
        public byte Func { get; set; }
        public ushort RegAddr { get; set; }
        public ushort[] Registers { get; set; } = new ushort[] { };
    }
}
