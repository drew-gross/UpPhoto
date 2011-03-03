﻿using System;
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

namespace UpPhoto
{
    public partial class MainWindow
    {
        public UpPhotoGUI gui;
        const String IdleIconPath = "Idle";
        const String UploadingIconPath = "Uploading";
        const String DownloadingIconPath = "Downloading";
        const String UploadingAndDownloadingIconPath = "UploadingAndDownloading";
        const String NotConnectedIconPath = "NotConnected";

        List<WatchedFolder> watchList = new List<WatchedFolder>();
        Dictionary<PID, String> AllPhotos; //initialized in LoadData;
        public UpdateHandler updateHandler;

        bool Connected = true;
        bool Uploading = false;
        bool Downloading = false;

        String SavedDataPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\UpPhotoData.upd";
        String ErrorLogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\UpPhotoError.log";

        public MainWindow()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(GlobalThreadExceptionHandler);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalUnhandledExceptionHandler);

            gui = new UpPhotoGUI(this);

            updateHandler = new UpdateHandler(this);

            LoadData();

            Application.Run();
        }

        public void SetUploadingStatus(bool newStatus)
        {
            Uploading = newStatus;
            UpdateTrayIcon();
        }

        public void SetDownloadingStatus(bool newStatus)
        {
            Downloading = newStatus;
            UpdateTrayIcon();
        }

        public void SetConnectedStatus(bool newStatus)
        {
            Connected = newStatus;
            UpdateTrayIcon();
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

        public void WatchFolder(String path)
        {
            Directory.CreateDirectory(path);
            WatchedFolder watcher = new WatchedFolder(path, this);
            watchList.Add(watcher);
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

        public void SaveData()
        {
            SavedData data = new SavedData(WatchedFolderPaths(), AllPhotos);
            Stream dataStream = File.Open(SavedDataPath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(dataStream, data);
            dataStream.Close();
        }

        public void QuitUpPhoto()
        {
            updateHandler.StopThreads();
            SaveData();
            Application.Exit();
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

        public void AddUploadedPhoto(FacebookPhoto UploadedPhoto)
        {
            AllPhotos.Add(new PID(UploadedPhoto.pid), UploadedPhoto.path);
            SaveData();
        }

        public bool HasPhoto(PID pid)
        {
            return AllPhotos.ContainsKey(pid);
        }

        static public String UpPhotoPath()
        {
            String result = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\UpPhoto\";
            if (!System.IO.Directory.Exists(result))
                System.IO.Directory.CreateDirectory(result);
            return result;
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

        private void UpdateTrayIcon()
        {
            if (!Connected)
            {
                return;
            }
            if (Uploading && Downloading)
            {
                gui.SetTrayIcon(UploadingAndDownloadingIconPath);
                return;
            }
            if (Uploading)
            {
                gui.SetTrayIcon(UploadingIconPath);
                return;
            }
            if (Downloading)
            {
                gui.SetTrayIcon(DownloadingIconPath);
                return;
            }
            gui.SetTrayIcon(IdleIconPath);
        }

        private void GlobalUnhandledExceptionHandler(Object sender, UnhandledExceptionEventArgs t)
        {
            LogException((System.Exception)t.ExceptionObject);
        }

        private void GlobalThreadExceptionHandler(Object sender, System.Threading.ThreadExceptionEventArgs t)
        {
            LogException(t.Exception);
        }

        public void LogException(System.Exception ex)
        {
            try
            {
                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(ErrorLogFilePath, true);
                file.WriteLine(ex.ToString());
                file.Close();
                QuitUpPhoto();
            }
            catch (Exception)
            {
                //can't write to logfile :(
                QuitUpPhoto();
            }
        }
    }
}
