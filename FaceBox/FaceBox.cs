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
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("OnStop!");
        }

        private void FaceBoxWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {s
            System.Console.WriteLine(e.ChangeType + ": " + e.FullPath);
        }
    }
}
