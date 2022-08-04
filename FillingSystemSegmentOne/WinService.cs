using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using FillingSystemHelper;

namespace FillingSystemSegmentTwo
{
    partial class WinService : ServiceBase
    {
        public WinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                FetchingHelper.RunFetchers(AppDomain.CurrentDomain.BaseDirectory, FillingSystemFetchSegmentProgram.ServiceName);
            }
            catch (Exception ex)
            {
                LogReport.AppendToLog(ex);
            }
        }

        protected override void OnStop()
        {
            FetchingHelper.StopFetchers();
        }
    }
}
