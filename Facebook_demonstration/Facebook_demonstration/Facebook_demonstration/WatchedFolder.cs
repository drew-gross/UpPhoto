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

        public ToolStripMenuItem menuItem;

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

            menuItem = new ToolStripMenuItem(path);

            ToolStripMenuItem UnwatchItem = new ToolStripMenuItem("Unwatch");
            UnwatchItem.Click += new EventHandler(this.UnwatchItem_click);
            menuItem.DropDownItems.Add(UnwatchItem);
        }

        public void UnwatchItem_click(Object sender, EventArgs e)
        {
            parent.UnwatchFolder(this);
        }

        public string Path()
        {
            return watcher.Path;
        }
    }
}
