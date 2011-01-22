using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FaceBox
{
    public class MonitorFolder
    {
        public FileSystemWatcher watcher;

        string pathToFolder = @"/c/TestFolder";

        public void Initiate()
        {
            watcher = new FileSystemWatcher { Path = pathToFolder, IncludeSubdirectories = true};
            watcher.Created += new FileSystemEventHandler(WatcherCreated);
        }

        void WatcherCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("works");
            Console.Read();
        }


    }
}
