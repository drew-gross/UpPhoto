using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using FacebookApplication;

namespace Facebook_demonstration
{
    public class WatchedFolder
    {
        FileSystemWatcher watcher;
        UpdateHandler handler = new UpdateHandler();
        MainWindow parent;

        ToolStripMenuItem removeFolderItem;

        public WatchedFolder(string path, MainWindow parentWindow)
        {
            parent = parentWindow;

            watcher = new FileSystemWatcher(path);
            watcher.EnableRaisingEvents = true;
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.FileName |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.LastAccess;

            watcher.SynchronizingObject = parent;

            watcher.Changed += new FileSystemEventHandler(handler.FaceboxWatcher_Changed);
            //watcher.Renamed += new FileSystemEventHandler(handler.FaceboxWatcher_Renamed);
            watcher.Created += new FileSystemEventHandler(handler.FaceboxWatcher_Created);
            watcher.Deleted += new FileSystemEventHandler(handler.FaceboxWatcher_Deleted);
        }

        public string Path()
        {
            return watcher.Path;
        }
    }
}
