using System;

namespace FillingSystemViewHelper
{
    public delegate void CheckPasswordEventHandler(object sender, CheckPasswordEventArgs e);

    public class CheckPasswordEventArgs : EventArgs
    {
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Password { get; set; }
        public bool IsValid { get; set; }
    }

}
