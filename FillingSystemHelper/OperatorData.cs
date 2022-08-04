
using System.Collections.Generic;
using System.Linq;

namespace FillingSystemHelper
{
    public class OperatorData
    {
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public int Access { get; set; }
        public string Department { get; set; }
        public string Appointment { get; set; }
        public string Password { get; set; }

        private OperatorData()
        {
            Lastname = string.Empty;
            Firstname = string.Empty;
            Secondname = string.Empty;
            Access = 0;
            Department = string.Empty;
            Appointment = string.Empty;
            Password = string.Empty;
        }

        public OperatorData(string lastname, string firstname, string secondname, int access, string department, string appointment, string password)
        {
            Lastname = lastname;
            Firstname = firstname;
            Secondname = secondname;
            Access = access;
            Department = department;
            Appointment = appointment;
            Password = password;
        }

        public override string ToString()
        {
            return $"{Lastname} {Firstname.Trim('*')} {Secondname.Trim('*')}";
        }

        public static object[] Operators
        {
            get
            {
                return new object []
                {
                    new OperatorAccess { Code = 0, Name = "Оператор"},
                    new OperatorAccess { Code = 1, Name = "Технолог"},
                    new OperatorAccess { Code = 2, Name = "Администратор"}
                };
            }
        }

        public static int GetCodeByName(string name)
        {
            var operators = Operators.Cast<OperatorAccess>();
            var oper = operators.FirstOrDefault(item => item.Name == name);
            return oper != null ? oper.Code : 0;
        }

        public static string GetNameByCode(int code)
        {
            var operators = Operators.Cast<OperatorAccess>();
            var oper = operators.FirstOrDefault(item => item.Code == code);
            return oper != null ? oper.Name : "(нет)";
        }
    }
}
