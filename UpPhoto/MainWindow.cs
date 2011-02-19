using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using UpPhoto;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FacebookApplication
{
    public partial class MainWindow : Form
    {
        ComponentResourceManager trayIcons = new ComponentResourceManager(typeof(SystemTrayIcons));

        List<WatchedFolder> watchList = new List<WatchedFolder>();
        Dictionary<PID, String> AllPhotos; //initialized in LoadData;
        UpdateHandler updateHandler;

        public bool Connected = false;
        bool exiting = false;

        const String SavedDataPath = @"UpPhotoData.upd";
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            updateHandler = new UpdateHandler(this);
            Thread detectPIDthread = new Thread(updateHandler.SnycPhotos);
            detectPIDthread.SetApartmentState(ApartmentState.STA);
            detectPIDthread.Start();

            Thread trayIconUpdater = new Thread(UpdateTrayIcon);
            trayIconUpdater.Start();
        }

        void UpdateTrayIcon()
        {
            while (exiting == false)
            {
                UpPhotoIcon.Icon = ((System.Drawing.Icon)(trayIcons.GetObject("NotConnected")));
            }
        }

        //Removes the "Form1" window from the alt-tab box
        protected override CreateParams CreateParams
        {
            get
            {
                // Turn on WS_EX_TOOLWINDOW style bit
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        public void ResumeWatchers()
        {
            foreach (WatchedFolder watcher in watchList)
            {
                watcher.EnableWatching();
            }
        }

        public void PauseWatchers()
        {
            foreach (WatchedFolder watcher in watchList)
            {
                watcher.DisableWatching();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void WatchFolder(String path)
        {
            Directory.CreateDirectory(path);
            WatchedFolder watcher = new WatchedFolder(path, this);
            watchList.Add(watcher);
            WatchFolderItem.DropDownItems.Add(watcher.menuItem);
        }

        public void WatchFolders(List<String> paths)
        {
            foreach (String path in paths)
            {
                WatchFolder(path);
            }
        }

        public void UnwatchFolder(WatchedFolder folder)
        {
            WatchFolderItem.DropDownItems.Remove(folder.menuItem);
            watchList.Remove(folder);
        }

        public List<String> WatchedFolderPaths()
        {
            List<string> ret = new List<string>();
            foreach (WatchedFolder watcher in watchList)
            {
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
            FacebookInterfaces.LogOut();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            updateHandler.StopThreads();
            exiting = true;
            SaveData();
            Close();
        }

        private void SaveData()
        {
            SavedData data = new SavedData(WatchedFolderPaths(), AllPhotos);
            Stream dataStream = File.Open(SavedDataPath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(dataStream, data);
            dataStream.Close();
        }

        private void LoadData()
        {
            try
            {
                Stream dataStream = File.Open(SavedDataPath, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                SavedData data = (SavedData)formatter.Deserialize(dataStream);
                WatchFolders(data.SavedWatchedFolders());
                AllPhotos = data.SavedPIDtoPhotoMap();
            }
            catch (System.IO.FileNotFoundException)
            {
                WatchFolder(UpPhotoPath());
                AllPhotos = new Dictionary<PID, String>();
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                //Bad save file :( needs a better handling method
                WatchFolder(UpPhotoPath());
                AllPhotos = new Dictionary<PID, String>();
            }
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

        public void AddUploadedPhoto(FacebookPhoto UploadedPhoto)
        {
            AllPhotos.Add(new PID(UploadedPhoto.pid), UploadedPhoto.path);
        }

        public bool HasPhoto(PID pid)
        {
            return AllPhotos.ContainsKey(pid);
        }

        static public String UpPhotoPath()
        {
            String result = Application.ExecutablePath;
            result = StringUtils.GetFullFolderPathFromPath(result);
            return result + @"\Facebook\Photos\";
        }

        public void WatchersIgnoreFile(String path)
        {
            foreach (WatchedFolder watcher in watchList)
            {
                watcher.IgnoreFile(path);
            }
        }

        public void WatchersUnIgnoreFile(String path)
        {
            foreach (WatchedFolder watcher in watchList)
            {
                watcher.UnIgnoreFile(path);
            }
        }

        //allows left click on icon to open menu
        [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            //SetForegroundWindow(new HandleRef(this, this.Handle));
            //UpPhotoTrayMenu.Show(this, this.PointToClient(Cursor.Position));
        }

        private void UpPhotoIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
