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
<<<<<<< HEAD
            EventLog.WriteEntry("OnStart!");           
=======
            EventLog.WriteEntry("OnStart!");

            //MonitorFolder testFolder = new MonitorFolder();
            //testFolder.Initiate();

>>>>>>> 27133f7deb5f7124fa0b4212b91f13689f7a61f7
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
