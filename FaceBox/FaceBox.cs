using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace FaceBox
{
    public partial class FaceBox : ServiceBase
    {
        public FaceBox()
        {
            InitializeComponent();
            EventLog.WriteEntry("Constructor!");
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("OnStart!");

           
            EventLog.WriteEntry("test1");
            MonitorFolder.Initiate();
            EventLog.WriteEntry("test2");

        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("OnStop!");
        }
    }
}
