using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FillingSystemX
{
    internal static class ComRegistration
    {
        const int OlemiscRecomposeonresize = 1;
        const int OlemiscCantlinkinside = 16;
        const int OlemiscInsideout = 128;
        const int OlemiscActivatewhenvisible = 256;
        const int OlemiscSetclientsitefirst = 131072;

        public static void RegisterControl(Type type)
        {
            try
            {
                GuardNullType(type, "type");
                GuardTypeIsControl(type);

                //CLSID
                var key = @"CLSID\" + type.GUID.ToString("B");

                using (var subkey = Registry.ClassesRoot.OpenSubKey(key, true))
                {

                    //InProcServer32
                    if (subkey != null)
                    {
                        var inprocKey = subkey.OpenSubKey("InprocServer32", true);
                        if (inprocKey != null)
                        {
                            inprocKey.SetValue(null, Environment.SystemDirectory + @"\mscoree.dll");
                        }
                    }

                    //Control
                    if (subkey != null)
                    {
                        using (var controlKey = subkey.CreateSubKey("Control"))
                        { }

                        //Misc
                        using (var miscKey = subkey.CreateSubKey("MiscStatus"))
                        {
                            const int miscStatusValue = OlemiscRecomposeonresize +
                                                        OlemiscCantlinkinside + OlemiscInsideout +
                                                        OlemiscActivatewhenvisible + OlemiscSetclientsitefirst;

                            if (miscKey != null) miscKey.SetValue("", miscStatusValue.ToString("0"), RegistryValueKind.String);
                        }

                        //ToolBoxBitmap32
                        using (var bitmapKey = subkey.CreateSubKey("ToolBoxBitmap32"))
                        {
                            //'If you want to have different icons for each control in this assembly
                            //'you can modify this section to specify a different icon each time.
                            //'Each specified icon must be embedded as a win32resource in the
                            //'assembly; the default one is at index 101, but you can add additional ones.
                            if (bitmapKey != null)
                                bitmapKey.SetValue("", Assembly.GetExecutingAssembly().Location + ", 101",
                                                   RegistryValueKind.String);
                        }

                        //TypeLib
                        using (var typeLibKey = subkey.CreateSubKey("TypeLib"))
                        {
                            var libId = Marshal.GetTypeLibGuidForAssembly(type.Assembly);
                            if (typeLibKey != null) typeLibKey.SetValue("", libId.ToString("B"), RegistryValueKind.String);
                        }

                        //Version
                        using (var versionKey = subkey.CreateSubKey("Version"))
                        {
                            int major, minor;
                            Marshal.GetTypeLibVersionForAssembly(type.Assembly, out major, out minor);
                            if (versionKey != null) versionKey.SetValue("", String.Format("{0}.{1}", major, minor));
                        }
                    }
                }

                //const string sSource = "Host .NET Interop UserControl in VB6";
                //const string sLog = "Application";
                var sEvent = "Registration successful: key = " + key;

                //if (!EventLog.SourceExists(sSource))
                //    EventLog.CreateEventSource(sSource, sLog);

                //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);
                //MessageBox.Show(@"COM Register function complete.");
            }
            catch (Exception ex)
            {
                LogAndRethrowException("ComRegisterFunction failed.", type, ex);
            }
        }

        public static void UnregisterControl(Type type)
        {
            try
            {
                GuardNullType(type, "type");
                GuardTypeIsControl(type);

                //CLSID
                var key = @"CLSID\" + type.GUID.ToString("B");
                Registry.ClassesRoot.DeleteSubKeyTree(key);
            }
            catch (Exception ex)
            {
                LogAndRethrowException("ComUnregisterFunction failed.", type, ex);
            }

        }

        private static void GuardNullType(Type type, string param)
        {
            if (null == type)
            {
                throw new ArgumentException(@"The CLR type must be specified.", param);
            }
        }

        private static void GuardTypeIsControl(Type type)
        {
            if (!typeof(Control).IsAssignableFrom(type))
            {
                throw new ArgumentException("Type argument must be a Windows Forms control.");
            }
        }

        private static void LogAndRethrowException(string message, Type type, Exception ex)
        {
            try
            {
                if (null != type)
                {
                    message += Environment.NewLine + String.Format("CLR class '{0}'", type.FullName);
                }

                throw new ComRegistrationException(message, ex);
            }
            catch (Exception ex2)
            {
                //const string sSource = "Host .NET Interop UserControl in VB6";
                //const string sLog = "Application";
                if (type == null) return;
                var sEvent = type.GUID.ToString("B") + " registration failed: " + Environment.NewLine + ex2.Message;

                //if (!EventLog.SourceExists(sSource))
                //    EventLog.CreateEventSource(sSource, sLog);

                //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);
            }
        }
    }

    [Serializable]
    public class ComRegistrationException : Exception
    {
        public ComRegistrationException() { }
        public ComRegistrationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
