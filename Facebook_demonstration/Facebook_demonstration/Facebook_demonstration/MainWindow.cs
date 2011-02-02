using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Facebook_demonstration;
using System.Runtime.Serialization.Formatters.Binary;

namespace FacebookApplication
{
    public partial class MainWindow : Form
    {
        private List<WatchedFolder> watchList = new List<WatchedFolder>();
        private UpdateHandler handler = new UpdateHandler();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void WatchFolder(string path)
        {
            WatchedFolder watcher = new WatchedFolder(path, this);
            
            watchList.Add(watcher);
            WatchFolderItem.DropDownItems.Add(watcher.menuItem);
            
        }

        public void UnwatchFolder(WatchedFolder folder)
        {
            WatchFolderItem.DropDownItems.Remove(folder.menuItem);
            watchList.Remove(folder);
        }

        public List<string> trackedFolders()
        {
            List<string> ret = new List<string>();
            foreach(WatchedFolder watcher in watchList){
                ret.Add(watcher.Path());
            }
            return ret;
        }

        private void ChangeAccountItem_Click(object sender, EventArgs e)
        {
            // Logout, then show prompt again.
            throw new NotImplementedException();
        }

        private void LogoutItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            UpdateHandler.StopUpdating();
            SaveData();
            Close();
        }

        private void SaveData()
        {
            SavedData data = new SavedData(watchList);
            Stream dataStream = File.Open("UpPhotoData.upd", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(dataStream, data);
            dataStream.Close();
        }

        private void UpPhotoTrayMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void AddWatchedFolderItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                WatchFolder(dialog.SelectedPath);
            }
        }
    }
}
