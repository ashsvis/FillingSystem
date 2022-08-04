using System;

namespace FillingSystemViewHelper
{
    public class LogReportItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime Snap { get; set; }
        public string Context { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            var dt = Snap.ToString("[dd.MM.yyyy HH:mm:ss]");
            return $"{dt} {Message}";
        }
    }
}
