using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using UpPhoto;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Utility;
using Facebook.Winforms.Components;
using FacebookController;
using System.Threading;
using System.Drawing;

namespace UpPhoto
{
    public class UpdateHandler
    {
        public Queue<PhotoToUpload> uploadQueue = new Queue<PhotoToUpload>();
        Queue<PID> downloadQueue = new Queue<PID>();

        Thread uploadThread;
        bool abortUploadThread = false;
        bool pauseUploadThread = false;

        Thread downloadThread;
        bool abortDownloadThread = false;
        bool pauseDownloadThread = false;

        const String DownloadedPhotoExtension = @".png";
        const int threadSleepTime = 500; //this is the time in milliseconds between checking if there are photos to be uploaded.
        MainWindow parent;

        public UpdateHandler(MainWindow newParent)
        {
            parent = newParent;

            Thread detectPIDthread = new Thread(SnycPhotos);
            detectPIDthread.SetApartmentState(ApartmentState.STA);
            detectPIDthread.Start();

            uploadThread = new Thread(UploadPhotos);
            uploadThread.SetApartmentState(ApartmentState.STA);
            uploadThread.Start();

            downloadThread = new Thread(DownloadPhotos);
            downloadThread.SetApartmentState(ApartmentState.STA);
            downloadThread.Start();
        }

        public void StopThreads()
        {
            abortDownloadThread = true;
            abortUploadThread = true;
            uploadThread.Join();
            downloadThread.Join();
        }

        //Only to be used by the uploadThread. Do not call directly.
        private void UploadPhotos()
        {
            while (abortUploadThread == false)
            {
                Thread.Sleep(threadSleepTime);
                parent.SetUploadingStatus(false);
                while (uploadQueue.Count > 0 && abortUploadThread == false)
                {
                    System.Windows.Forms.Application.DoEvents();//prevents ContextSwitchingDeadlocks
                    parent.SetUploadingStatus(true);
                    while (pauseUploadThread == true)
                    {
                        Thread.Sleep(threadSleepTime);
                    }
                    lock (uploadQueue)
                    {
                        PhotoToUpload curPhoto = uploadQueue.Dequeue();
                        photo UploadedPhoto = FacebookInterfaces.UploadPhoto(curPhoto);
                        //if we are here, the uploading worked and we can set the tray icon to connected
                        //if the uploading failed, an exception would have been thrown
                        parent.SetConnectedStatus(true);
                        parent.AddUploadedPhoto(new FacebookPhoto(UploadedPhoto, curPhoto.photoPath));
                    }
                }
            }
        }

        private Boolean IsPhotoDownloaded(photo DownloadedPhoto)
        {
            try
            {
                foreach (String filename in Directory.GetFiles(MainWindow.UpPhotoPath() + FacebookInterfaces.AlbumName(DownloadedPhoto.aid)))
                {
                    Image b = Image.FromFile(filename);
                    if (b.Equals(DownloadedPhoto.picture_big))
                    {
                        return true;
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                return false;
            }
            return false;
        }

        //Only to be used by the downloadThread. Do not call directly.
        private void DownloadPhotos()
        {
            while (abortDownloadThread == false)
            {
                Thread.Sleep(threadSleepTime);
                parent.SetDownloadingStatus(false);
                while (downloadQueue.Count > 0 && abortDownloadThread == false)
                {
                    System.Windows.Forms.Application.DoEvents();//prevents ContextSwitchingDeadlocks
                    parent.SetDownloadingStatus(true);
                    while (pauseDownloadThread == true)
                    {
                        Thread.Sleep(threadSleepTime);
                    }
                    lock (downloadQueue)
                    {
                        PID pidToDownload = downloadQueue.Dequeue();
                        try
                        {
                            photo DownloadedPhoto = FacebookInterfaces.DownloadPhoto(pidToDownload);
                            if (!IsPhotoDownloaded(DownloadedPhoto))
                            {
                                String path = GeneratePath(DownloadedPhoto);
                                try
                                {
                                    SaveDownloadedPhoto(DownloadedPhoto, path);
                                }
                                catch (Facebook.Utility.FacebookException)
                                {
                                    //Unable to connect to database
                                    throw;
                                }
                            }
                        }
                        catch (System.Net.WebException)
                        {
                            //add the photo back to the queue and try again later
                            parent.SetConnectedStatus(false);
                            downloadQueue.Enqueue(pidToDownload);
                            Thread.Sleep(threadSleepTime);
                        }
                    }
                }
            }
        }

        private String GeneratePath(photo DownloadedPhoto)
        {
            int PhotoCounter = 1;
            String albumName = FacebookInterfaces.AlbumName(DownloadedPhoto.aid);
            String upPhotoPath = MainWindow.UpPhotoPath();
            String path = upPhotoPath + albumName + @"\Photo " + PhotoCounter.ToString() + DownloadedPhotoExtension;
            while (File.Exists(path))
            {
                PhotoCounter++;
                path = upPhotoPath + albumName + @"\Photo " + PhotoCounter.ToString() + DownloadedPhotoExtension;
            }
            return path;
        }

        private void SaveDownloadedPhoto(photo DownloadedPhoto, String path)
        {
            Directory.CreateDirectory(StringUtils.GetFullFolderPathFromPath(path));
            System.Drawing.Bitmap imageData = new System.Drawing.Bitmap((System.Drawing.Bitmap)DownloadedPhoto.picture_big.Clone());
            parent.WatchersIgnoreFile(path);
            imageData.Save(path, ImageFormat.Png);
            parent.WatchersUnIgnoreFile(path);
            parent.AddUploadedPhoto(new FacebookPhoto(DownloadedPhoto, path));
        }

        public void SnycPhotos()
        {
            try
            {
                List<PID> allPIDs = FacebookInterfaces.AllFacebookPhotos();
                pauseDownloadThread = true;
                foreach (PID pid in allPIDs)
                {
                    System.Windows.Forms.Application.DoEvents();//prevents ContextSwitchingDeadlocks
                    if (!parent.HasPhoto(pid))
                    {
                        lock (downloadQueue)
                        {
                            downloadQueue.Enqueue(pid);
                        }
                    }
                }
                pauseDownloadThread = false;
            }
            catch (System.Net.WebException)
            {
                parent.SetConnectedStatus(false);
            }
        }

        public void FaceboxWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (StringUtils.IsImageExtension(Path.GetExtension(e.FullPath)))
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                FacebookInterfaces.DeletePhotos(album, e.FullPath);
            }
        }

        public void FaceboxWatcher_Renamed(object sender, RenamedEventArgs e)
        {
        }
    }
}
