using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Facebook_demonstration
{
    class MonitorDocument
    {
        public FileSystemWatcher FaceboxMonitor;
        public string FaceboxPathName = @"C:\TestFolder";
        
        public MonitorDocument()
        {
            FaceboxMonitor = new FileSystemWatcher(FaceboxPathName, "");
            FaceboxMonitor.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            FaceboxMonitor.Changed += new FileSystemEventHandler(OnChange);
            FaceboxMonitor.EnableRaisingEvents  =true;
        
        }

        public void OnChange(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("hello world");
            Console.Read();

        }
    }
}
