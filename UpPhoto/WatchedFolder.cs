using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using FacebookApplication;
using System.Runtime.Serialization;

namespace UpPhoto
{
    public class WatchedFolder
    {
        MainWindow parent;
        UpdateHandler handler;
        FileSystemWatcher watcher;
        List<String> IgnoreList = new List<String>();

        public ToolStripMenuItem menuItem;

        public WatchedFolder(string path, MainWindow newParent)
        {
            parent = newParent;
            handler = parent.updateHandler;

            watcher = new FileSystemWatcher(path);
            watcher.EnableRaisingEvents = true;
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.FileName |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.LastAccess;

            watcher.SynchronizingObject = parent;

            watcher.Changed += new FileSystemEventHandler(FileChangedEvent);
            //watcher.Renamed += new FileSystemEventHandler(handler.FaceboxWatcher_Renamed);
            watcher.Created += new FileSystemEventHandler(FileCreatedEvent);
            watcher.Deleted += new FileSystemEventHandler(handler.FaceboxWatcher_Deleted);

            menuItem = new ToolStripMenuItem(path);

            ToolStripMenuItem UnwatchItem = new ToolStripMenuItem("Unwatch");
            UnwatchItem.Click += new EventHandler(this.UnwatchItem_Click);
            menuItem.DropDownItems.Add(UnwatchItem);

            ToolStripMenuItem ViewItem = new ToolStripMenuItem("View");
            ViewItem.Click += new EventHandler(this.ViewItem_Click);
            menuItem.DropDownItems.Add(ViewItem);
        }

        public void IgnoreFile(String path)
        {
            IgnoreList.Add(path);
        }

        public void UnIgnoreFile(String path)
        {
            IgnoreList.Remove(path);
        }

        public void FileCreatedEvent(object sender, FileSystemEventArgs e)
        {
            if (StringUtils.IsImageExtension(System.IO.Path.GetExtension(e.FullPath)) && !IgnoreList.Contains(e.FullPath))
            {
                string album = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(e.FullPath));
                lock (handler.uploadQueue)
                {
                    handler.uploadQueue.Enqueue(new PhotoToUpload(album, e.FullPath));
                }
            }
        }

        public void FileChangedEvent(object sender, FileSystemEventArgs e)
        {
            //lets deal with changed photos the same way we deal with new photos. we add them to fb, but dont delete the original photo.
            FileCreatedEvent(sender, e);
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

        public void DisableWatching()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void EnableWatching()
        {
            watcher.EnableRaisingEvents = true;
        }
    }
}
