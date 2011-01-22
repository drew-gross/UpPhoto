using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Security.Permissions;

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
            FileSystemWatcher FaceBoxWatcher = new FileSystemWatcher(@"C:\TestFolder", @"");
            FaceBoxWatcher.NotifyFilter = NotifyFilters.LastAccess |
                                          NotifyFilters.LastWrite |
                                          NotifyFilters.FileName |
                                          NotifyFilters.DirectoryName;

            FaceBoxWatcher.Changed += new FileSystemEventHandler(OnChanged);
            FaceBoxWatcher.Error += new ErrorEventHandler(OnError);

            FaceBoxWatcher.EnableRaisingEvents = true;
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("OnStop!");
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            EventLog.WriteEntry("File: " + e.FullPath + " " + e.ChangeType);
        }

        //  This method is called when the FileSystemWatcher detects an error.
        private void OnError(object source, ErrorEventArgs e)
        {
            EventLog.WriteEntry("OnError!");
        }
    }
}
