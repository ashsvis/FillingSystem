using System;

namespace FillingSystemHelper
{
    public delegate void OperatorEventHandler(object sender, OperatorEventArgs e);

    public class OperatorEventArgs : EventArgs
    {
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public int Access { get; set; }
        public string Department { get; set; }
        public string Appointment { get; set; }
        public string Password { get; set; }
    }

}
