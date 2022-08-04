using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FillingSystemHelper
{
    public static class CommonData
    {
        public static int AccessCode { get; set; }
        public static string AccessName { get; set; }

        public static string FullUserName { get; set; }
        public static string ShortUserName { get; set; }
        public static string Lastname { get; private set; }
        public static string Firstname { get; private set; }
        public static string Secondname { get; private set; }

        public static void Logout()
        {
            AccessCode = 0;
            AccessName = null;
            Lastname = null;
            Firstname = null;
            Secondname = null;
            FullUserName = null;
            ShortUserName = null;
        }

        public static void Login(string lastname, string firstname, string secondname, int access)
        {
            AccessCode = access;
            AccessName = OperatorData.GetNameByCode(access);
            Lastname = lastname;
            Firstname = firstname;
            Secondname = secondname;
            FullUserName = $"{lastname} {firstname} {secondname}".Replace(" *", "");
            ShortUserName = $"{lastname} {firstname[0]}.{secondname[0]}.".Replace("*.","");
        }

        public static bool EnteredAsAdmin()
        {
            return !string.IsNullOrEmpty(AccessName) && AccessCode == 2;
        }

        public static bool EnteredAsSupervisor()
        {
            return EnteredAsAdmin() || !string.IsNullOrEmpty(AccessName) && AccessCode == 1;
        }

        public static bool EnteredAsOperator()
        {
            return EnteredAsAdmin() || EnteredAsSupervisor() || !string.IsNullOrEmpty(AccessName) && AccessCode == 0;
        }

        private static string[] _filterEventsList;
        private static DateTimeRange _filterDateTimeRange;
        private static string[] _filterOverpassList;
        private static string[] _filterWayList;
        private static string[] _filterProductList;
        private static Tuple<int, int> _filterRiserRange;
        private static RiserConfigEventArgs _addressTreePath;
        private static OperatorEventArgs _operatorEvent;

        public static MemIniFile Config { get; set; } = 
            new MemIniFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "FillingSystemStation.ini"));

        public static string[] FilterEventsList
        {
            get
            {
                if (_filterEventsList == null)
                {
                    var list = new List<string>();
                    const string section = "FilterEvents";
                    if (Config.SectionExists(section))
                        list.AddRange(Config.ReadSectionValues(section));
                    _filterEventsList = list.ToArray();
                }
                return _filterEventsList;
            }
            set { _filterEventsList = value; }
        }

        public static DateTime MinRangeDate()
        {
            return new DateTime(2000, 1, 1);
        }

        public static DateTime MaxRangeDate()
        {
            return new DateTime(2099, 12, 31, 23, 59, 59);
        }

        public static DateTimeRange FilterDateTimeRange
        {
            get
            {
                if (_filterDateTimeRange == null)
                {
                    const string section = "FilterDateTimeRange";
                    var firstExists = Config.ReadBool(section, "FirstExists", false);
                    var first = Config.ReadDateTime(section, "First", MinRangeDate());
                    var lastExists = Config.ReadBool(section, "LastExists", false);
                    var last = Config.ReadDateTime(section, "Last", MaxRangeDate());
                    _filterDateTimeRange = new DateTimeRange() 
                    { 
                        FirstExists = firstExists, First = first, 
                        LastExists = lastExists, Last = last 
                    };
                }
                return _filterDateTimeRange;
            }
            set { _filterDateTimeRange = value; }
        }

        public static string[] FilterOverpassList
        {
            get
            {
                if (_filterOverpassList == null)
                {
                    const string section = "FilterAddress";
                    var list = Config.ReadString(section, "Overpass", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    _filterOverpassList = list.ToArray();
                }
                return _filterOverpassList;
            }
            set { _filterOverpassList = value; }
        }

        public static string[] FilterWayList
        {
            get
            {
                if (_filterWayList == null)
                {
                    const string section = "FilterAddress";
                    var list = Config.ReadString(section, "Way", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    _filterWayList = list.ToArray();
                }
                return _filterWayList;
            }
            set { _filterWayList = value; }
        }

        public static string[] FilterProductList
        {
            get
            {
                if (_filterProductList == null)
                {
                    const string section = "FilterAddress";
                    var list = Config.ReadString(section, "Product", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    _filterProductList = list.ToArray();
                }
                return _filterProductList;
            }
            set { _filterProductList = value; }
        }

        public static Tuple<int, int> FilterRiserRange
        {
            get
            {
                if (_filterRiserRange == null)
                {
                    const string section = "FilterAddress";
                    _filterRiserRange = new Tuple<int, int>(
                        Config.ReadInteger(section, "FirstRiser", 1),
                        Config.ReadInteger(section, "LastRiser", 247));
                }
                return _filterRiserRange;
            }
            set { _filterRiserRange = value; }
        }

        public static void UpdateFilterEventsList(string[] list)
        {
            _filterEventsList = list;
            const string section = "FilterEvents";
            if (Config.SectionExists(section))
                Config.EraseSection(section);
            var n = 0;
            foreach (var s in list)
            {
                Config.WriteString(section, n.ToString("0"), s);
                n++;
            }
            Config.UpdateFile();
        }

        public static RiserConfigEventArgs AddressTreePath
        {
            get
            {
                if (_addressTreePath == null)
                {
                    const string section = "AddressTree";
                    if (Config.SectionExists(section))
                    {
                        _addressTreePath = new RiserConfigEventArgs()
                        {
                            Overpass = Config.ReadInteger(section, "Overpass", 0),
                            Way = Config.ReadInteger(section, "Way", 0),
                            Product = Config.ReadString(section, "Product", ""),
                        };
                    }
                    else
                        _addressTreePath = null;
                }
                return _addressTreePath;
            }
        }

        public static void UpdateAddressTreePath(int overpass, int way, string product)
        {
            _addressTreePath = new RiserConfigEventArgs() { Overpass = overpass, Way = way, Product = product };
            const string section = "AddressTree";
            Config.WriteInteger(section, "Overpass", overpass);
            Config.WriteInteger(section, "Way", way);
            Config.WriteString(section, "Product", product);
            Config.UpdateFile();
        }

        public static void UpdateFilterDateTimeRange(DateTimeItem first, DateTimeItem last)
        {
            _filterDateTimeRange = new DateTimeRange() 
            { 
                FirstExists = first.Exists, First = first.DateTime,
                LastExists = last.Exists, Last = last.DateTime 
            };
            const string section = "FilterDateTimeRange";
            Config.WriteBool(section, "FirstExists", first.Exists);
            Config.WriteDateTime(section, "First", first.DateTime);
            Config.WriteBool(section, "LastExists", last.Exists);
            Config.WriteDateTime(section, "Last", last.DateTime);
            Config.UpdateFile();
        }

        public static void UpdateFilterAddress(string[] overpass, string[] way, string[] product, int first, int last)
        {
            _filterOverpassList = overpass;
            _filterWayList = way;
            _filterProductList = product;
            _filterRiserRange = new Tuple<int, int>(first, last);
            const string section = "FilterAddress";
            Config.WriteString(section, "Overpass", string.Join(",", overpass));
            Config.WriteString(section, "Way", string.Join(",", way));
            Config.WriteString(section, "Product", string.Join(",", product));
            Config.WriteInteger(section, "FirstRiser", first);
            Config.WriteInteger(section, "LastRiser", last);
            Config.UpdateFile();
        }

        public static event EventHandler OnUpdateWorkLogFilter;

        public static void WorkLogFilterChanged()
        {
            OnUpdateWorkLogFilter?.Invoke(null, EventArgs.Empty);
        }

        public static OperatorEventArgs LoginInfo
        {
            get
            {
                if (_operatorEvent == null)
                {
                    const string section = "LoginInfo";
                    if (Config.SectionExists(section))
                    {
                        _operatorEvent = new OperatorEventArgs()
                        {
                            Lastname = Config.ReadString(section, "Lastname", ""),
                            Firstname = Config.ReadString(section, "Firstname", ""),
                            Secondname = Config.ReadString(section, "Secondname", ""),
                            Access = Config.ReadInteger(section, "Access", 0)
                        };
                    }
                    else
                        _operatorEvent = null;
                }
                return _operatorEvent;
            }
        }

        public static void UpdateLoginInfo(string lastname, string firstname, string secondname, int access)
        {
            _operatorEvent = new OperatorEventArgs()
            {
                Lastname = lastname,
                Firstname = firstname,
                Secondname = secondname,
                Access = access
            };
            const string section = "LoginInfo";
            Config.WriteString(section, "Lastname", lastname);
            Config.WriteString(section, "Firstname", firstname);
            Config.WriteString(section, "Secondname", secondname);
            Config.WriteInteger(section, "Access", access);
            Config.UpdateFile();
        }
    }

    public class DateTimeRange
    {
        public bool FirstExists { get; set; }
        public DateTime First { get; set; } = CommonData.MinRangeDate();
        public bool LastExists { get; set; }
        public DateTime Last { get; set; } = CommonData.MaxRangeDate();
    }

    public class DateTimeItem
    {
        public bool Exists { get; set; }
        public DateTime DateTime { get; set; }
    }
}
