using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using FacebookApplication;
using System.Runtime.Serialization;

namespace Facebook_demonstration
{
    public class WatchedFolder
    {
        MainWindow parent;
        UpdateHandler handler;
        FileSystemWatcher watcher;

        public ToolStripMenuItem menuItem;

        public WatchedFolder(string path, MainWindow newParent)
        {
            parent = newParent;
            handler = new UpdateHandler(parent);

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
            UnwatchItem.Click += new EventHandler(this.UnwatchItem_Click);
            menuItem.DropDownItems.Add(UnwatchItem);

            ToolStripMenuItem ViewItem = new ToolStripMenuItem("View");
            ViewItem.Click += new EventHandler(this.ViewItem_Click);
            menuItem.DropDownItems.Add(ViewItem);
        }

        public void UnwatchItem_Click(Object sender, EventArgs e)
        {
            parent.UnwatchFolder(this);
            watcher.Dispose();
        }
        
        public void ViewItem_Click(object sender, EventArgs e)
        {
            Process.Start(watcher.Path);
        }

        public string Path()
        {
            return watcher.Path;
        }
    }
}
