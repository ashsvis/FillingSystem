using System;
using System.Linq;

namespace FillingSystemHelper
{
    public class DualSqlServer : FillingSqlServer
    {
        // строка подключения
        private string connection;

        private bool ServerA = false;

        private string ConnectionA { get; set; } = string.Empty; // строка подключения к серверу A
        private string ConnectionB { get; set; } = string.Empty; // строка подключения к серверу B

        public override string Connection
        {
            get { return connection; }
            set
            {
                //Data Source=PNVCSRVB;Initial Catalog=PNVC;User ID=pnvc_admin;Password=!PNVC@r400;Trusted_Connection=Yes;
                var vals = value.Split(';');
                var dataSource = "";
                var initialCatalog = "";
                var userID = "";
                var trustedConnection = "";
                var password = "";
                foreach (var val in vals.Select(item => item.Split('=')).Where(item => item.Length == 2))
                {
                    if (string.IsNullOrWhiteSpace(dataSource))
                        dataSource = val[0].ToLower() == "Data Source".ToLower() ? val[1] : "";
                    if (string.IsNullOrWhiteSpace(initialCatalog))
                        initialCatalog = val[0].ToLower() == "Initial Catalog".ToLower() ? val[1] : "";
                    if (string.IsNullOrWhiteSpace(userID))
                        userID = val[0].ToLower() == "User ID".ToLower() ? val[1] : "";
                    if (string.IsNullOrWhiteSpace(password))
                        password = val[0].ToLower() == "Password".ToLower() ? val[1] : "";
                    if (string.IsNullOrWhiteSpace(trustedConnection))
                        trustedConnection = val[0].ToLower() == "Trusted_Connection".ToLower() ? $"Trusted_Connection={val[1]}" : "";
                }
                var security = string.IsNullOrWhiteSpace(trustedConnection)
                    ? $"User ID={userID};Password={password};"
                    : "Trusted_Connection=Yes;";
                if (dataSource.ToLower().EndsWith("a"))
                {
                    
                    ConnectionA = $"Data Source={dataSource};Initial Catalog={initialCatalog};{security}";
                    ConnectionB = $"Data Source={dataSource.Substring(0, dataSource.Length - 1) + "B"};Initial Catalog={initialCatalog};{security}";
                    connection = ServerA ? ConnectionA : ConnectionB;
                }
                else if (dataSource.ToLower().EndsWith("b"))
                {
                    ConnectionB = $"Data Source={dataSource};Initial Catalog={initialCatalog};{security}";
                    ConnectionA = $"Data Source={dataSource.Substring(0, dataSource.Length - 1) + "A"};Initial Catalog={initialCatalog};{security}";
                    connection = ServerA ? ConnectionA : ConnectionB;
                }
                else
                    connection = value;
            }
        }

        public override void InsertIntoLogReport(DateTime snaptime, string category, string eventInfo,
            int overpass = 0, int way = 0, string productCode = "", int riser = 0,
            string number = "", int ntype = 0, int maxHeight = 0, string source = "", int setLevel = 0)
        {
            ServerA = false;
            base.InsertIntoLogReport(snaptime, category, eventInfo, overpass, way, productCode, riser, number, ntype, maxHeight, source, setLevel);
            ServerA = true;
            base.InsertIntoLogReport(snaptime, category, eventInfo, overpass, way, productCode, riser, number, ntype, maxHeight, source, setLevel);
        }
    }
}
