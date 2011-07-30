using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using UpPhoto;
using System.Runtime.Serialization;

namespace UpPhoto
{
    public class WatchedFolder
    {
        MainWindow parent;
        UpdateHandler handler;
        FileSystemWatcher watcher;
        List<String> IgnoreList = new List<String>();

        public WatchedFolder(string path, MainWindow newParent)
        {
            parent = newParent;
            handler = parent.updateHandler;

            watcher = new FileSystemWatcher(path);
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.FileName |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.LastAccess;

            watcher.SynchronizingObject = parent.gui;

            watcher.Changed += FileChangedEvent;
            watcher.Created += FileCreatedEvent;

            watcher.InternalBufferSize = 64000;
            watcher.EnableRaisingEvents = true;
        }

        public void IgnoreFile(String path)
        {
            lock (IgnoreList)
            {
                IgnoreList.Add(path);
            }
        }

        public void UnIgnoreFile(String path)
        {
            lock (IgnoreList)
            {
                IgnoreList.Remove(path);
            }
        }

        public void FileCreatedEvent(object sender, FileSystemEventArgs e)
        {
            lock (IgnoreList)
            {
                if (StringUtils.IsImageExtension(System.IO.Path.GetExtension(e.FullPath)) && !IgnoreList.Contains(e.FullPath))
                {
                    handler.EnquePhotoFromPath(e.FullPath);
                }
            }
        }

        public void FileChangedEvent(object sender, FileSystemEventArgs e)
        {
            //lets deal with changed photos the same way we deal with new photos. we add them to fb, but dont delete the original photo.
            FileCreatedEvent(sender, e);
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
