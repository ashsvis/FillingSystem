using System;

namespace FillingSystemHelper
{
    [Flags]
    public enum ControlCommand : ushort
    {
        None = 0x0000,
        Run = 0x0001,
        Stop = 0x0002,
        GetDeepAndRange = 0x0004,
        GetLinkData = 0x0008,
        GetPlcData = 0x0010,
        GetAdcData = 0x0020,
        GetAlarmData = 0x0040,
        GetLevelData = 0x0080,
        WriteConfigData = 0x0100,

        Unknown = 0x4000,
        Error = 0x8000,
        All = 0xFFFF
    }
}
