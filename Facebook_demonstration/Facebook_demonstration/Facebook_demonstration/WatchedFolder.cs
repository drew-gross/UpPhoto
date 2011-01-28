using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FacebookApplication;

namespace Facebook_demonstration
{
    class WatchedFolder
    {
        FileSystemWatcher watcher;
        UpdateHandler handler = new UpdateHandler();

        public WatchedFolder(string path)
        {
            watcher = new FileSystemWatcher(path);
            watcher.EnableRaisingEvents = true;
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.FileName |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.LastAccess;

            watcher.Changed += new FileSystemEventHandler(handler.FaceboxWatcher_Changed);
            //watcher.Renamed += new FileSystemEventHandler(handler.FaceboxWatcher_Renamed);
            watcher.Created += new FileSystemEventHandler(handler.FaceboxWatcher_Created);
            watcher.Deleted += new FileSystemEventHandler(handler.FaceboxWatcher_Deleted);
        }
    }
}
