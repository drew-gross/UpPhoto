using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FacebookApplication
{
    public partial class MainWindow : Form
    {
        private List<FileSystemWatcher> watchList = new List<FileSystemWatcher>();
        private UpdateHandler handler = new UpdateHandler();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddFolderToTrack(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path);
            watcher.EnableRaisingEvents = true;
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.FileName |
                NotifyFilters.DirectoryName |
                NotifyFilters.LastAccess;

            watcher.SynchronizingObject = this;

            watcher.Changed += new FileSystemEventHandler(handler.FaceboxWatcher_Changed);
            //watcher.Renamed += new FileSystemEventHandler(handler.FaceboxWatcher_Renamed);
            watcher.Created += new FileSystemEventHandler(handler.FaceboxWatcher_Created);
            watcher.Deleted += new FileSystemEventHandler(handler.FaceboxWatcher_Deleted);
            
            watchList.Add(watcher);
        }

        public List<string> trackedFolders()
        {
            List<string> ret = new List<string>();
            foreach(FileSystemWatcher watcher in watchList){
                ret.Add(watcher.Path);
            }
            return ret;
        }

        private void ChangeAccountItem_Click(object sender, EventArgs e)
        {
            // Logout, then show prompt again.
        }

        private void LogoutItem_Click(object sender, EventArgs e)
        {
            // TODO: Implement logging out.
        }

        private void WatchFolderItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                AddFolderToTrack(dialog.SelectedPath);
            }

        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
