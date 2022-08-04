using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FillingSystemX
{
    [ComVisible(true), Guid(FillingControl.EventsId), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IFillingEvents
    {
        [DispId(1)]
        void OnChangePanel(string nodeKey, int overpass, int way, string product);

        [DispId(2)]
        void OnMessage(string msg);

        [DispId(3)]
        void OnStartFilling(string pointname, string wagonNumber, int wagonType, int realHeight, int setpoint);
    }

    [Guid(FillingControl.InterfaceId), ComVisible(true)]
    public interface IFillingProperties
    {
        [DispId(1)]
        void LoadConfiguration(string connection);

        //[DispId(2)]
        //void AddFetchLine(string info);

        //[DispId(3)]
        //void RestorePanel(string nodeKey);

    }

    [Guid(ClassId), ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces("FillingSystemX.IFillingEvents")]
    [ComClass(ClassId, InterfaceId, EventsId)]
    public partial class FillingControl : UserControl, IFillingProperties //, IObjectSafety
    {
        #region "COM Registration"

        //These  GUIDs provide the COM identity for this class 
        //and its COM interfaces. If you change them, existing 
        //clients will no longer be able to access the class.

        private const string ClassId = "772DE39D-4FCC-4C79-8D37-30F2422FD299"; //
        public const string InterfaceId = "170EA515-6EE1-42D7-9450-FA9D0C21B920";
        public const string EventsId = "2DB2C3A5-B256-4AE7-84A1-6FBC0CFEB717";


        //These routines perform the additional COM registration needed by ActiveX controls
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction]
        private static void Register(Type type)
        {
            ComRegistration.RegisterControl(type);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction]
        private static void Unregister(Type type)
        {
            ComRegistration.UnregisterControl(type);
        }

        #endregion

        #region "Methods"

        public void LoadConfiguration(string connecton)
        {
            fillingPageUserControl1.OnStartFilling += FillingPageUserControl1_OnStartFilling;
            fillingPageUserControl1.LoadConfiguration(connecton);
        }

        private void FillingPageUserControl1_OnStartFilling(string pointname, string wagonNumber, int wagonType, int realHeight, int setpoint)
        {
            OnStartFilling?.Invoke(pointname, wagonNumber, wagonType, realHeight, setpoint);
        }

        //public void AddFetchLine(string info)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RestorePanel(string nodeKey)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region "Events delegate"

        public delegate void OnChangePanelEvenHandle(string nodeKey, int overpass, int way, string product);
        public delegate void OnMessageEvenHandle(string msg);
        public delegate void OnStartFillingEventHandle(string pointname, string wagonNumber, int wagonType, int realHeight, int setpoint);

        public event OnChangePanelEvenHandle OnChangePanel = null;
        public event OnMessageEvenHandle OnMessage = null;
        public event OnStartFillingEventHandle OnStartFilling = null;

        #endregion

        public FillingControl()
        {
            InitializeComponent();
        }

        public int GetInterfaceSafetyOptions(ref Guid riid, [MarshalAs(UnmanagedType.U4)] ref int pdwSupportedOptions, [MarshalAs(UnmanagedType.U4)] ref int pdwEnabledOptions)
        {
            var impl = new ObjectSafetyImpl();
            return impl.GetInterfaceSafetyOptions(ref riid, ref pdwSupportedOptions, ref pdwEnabledOptions);
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, [MarshalAs(UnmanagedType.U4)] int dwOptionSetMask, [MarshalAs(UnmanagedType.U4)] int dwEnabledOptions)
        {
            var impl = new ObjectSafetyImpl();
            return impl.SetInterfaceSafetyOptions(ref riid, dwOptionSetMask, dwEnabledOptions);
        }
    }

    [ComImport, GuidAttribute("6FEC2ACC-531F-432E-A969-A959C6376403")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IObjectSafety
    {
        [PreserveSig]
        int GetInterfaceSafetyOptions(ref Guid riid, [MarshalAs(UnmanagedType.U4)] ref int pdwSupportedOptions, [MarshalAs(UnmanagedType.U4)] ref int pdwEnabledOptions);

        [PreserveSig()]
        int SetInterfaceSafetyOptions(ref Guid riid, [MarshalAs(UnmanagedType.U4)] int dwOptionSetMask, [MarshalAs(UnmanagedType.U4)] int dwEnabledOptions);
    }

    public class ObjectSafetyImpl : IObjectSafety
    {
        private const string IidIDispatch = "{18C95610-341A-4613-A733-C178F237A6E8}";
        private const string IidIDispatchEx = "{425F08DC-A454-424D-B1B7-B395401F648E}";
        private const string IidIPersistStorage = "{0D951796-16E5-436A-BEC9-E78ED6BC26A4}";
        private const string IidIPersistStream = "{F44C46BE-28BD-45E6-953A-25593971BA0E}";
        private const string IidIPersistPropertyBag = "{6075946C-28A5-4963-8BC2-FAB7E9B1DFE7}";

        private const int InterfacesafeForUntrustedCaller = 0x00000001;
        private const int InterfacesafeForUntrustedData = 0x00000002;
        private const int Ok = 0;
        private const int Fail = unchecked((int)0x80004005);
        private const int Nointerface = unchecked((int)0x80004002);

        private const bool FSafeForScripting = true;
        private const bool FSafeForInitializing = true;

        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int result;

            var strGuid = riid.ToString("B");
            pdwSupportedOptions = InterfacesafeForUntrustedCaller | InterfacesafeForUntrustedData;
            switch (strGuid)
            {
                case IidIDispatch:
                case IidIDispatchEx:
                    result = Ok;
                    pdwEnabledOptions = InterfacesafeForUntrustedCaller;
                    break;
                case IidIPersistStorage:
                case IidIPersistStream:
                case IidIPersistPropertyBag:
                    result = Ok;
                    pdwEnabledOptions = InterfacesafeForUntrustedData;
                    break;
                default:
                    result = Nointerface;
                    break;
            }

            return result;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            var result = Fail;

            var strGuid = riid.ToString("B");
            switch (strGuid)
            {
                case IidIDispatch:
                case IidIDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == InterfacesafeForUntrustedCaller))
                        result = Ok;
                    break;
                case IidIPersistStorage:
                case IidIPersistStream:
                case IidIPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == InterfacesafeForUntrustedData))
                        result = Ok;
                    break;
                default:
                    result = Nointerface;
                    break;
            }

            return result;
        }
    }

}
