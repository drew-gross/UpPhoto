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

namespace FacebookApplication
{
    class UpdateHandler
    {
        public static Queue<PhotoToUpload> uploadQueue = new Queue<PhotoToUpload>();
        static Queue<PID> downloadQueue = new Queue<PID>();

        static Thread uploadThread;
        static bool abortUploadThread = false;
        static bool pauseUploadThread = false;

        static Thread downloadThread;
        static bool abortDownloadThread = false;
        static bool pauseDownloadThread = false;

        const String DownloadedPhotoExtension = @".png";
        const int threadSleepTime = 500; //this is the time in milliseconds between checking if there are photos to be uploaded.
        MainWindow parent;

        static UpdateHandler()
        {
            uploadThread = new Thread(UpdateHandler.UploadPhotos);
            uploadThread.SetApartmentState(ApartmentState.STA);
            uploadThread.Start();

            downloadThread = new Thread(UpdateHandler.DownloadPhotos);
            downloadThread.SetApartmentState(ApartmentState.STA);
            downloadThread.Start();
        }

        public UpdateHandler(MainWindow newParent)
        {
            parent = newParent;
        }

        public static void StopThreads()
        {
            abortUploadThread = true;
            abortDownloadThread = true;
        }

        //Only to be used by the uploadThread. Do not call directly.
        static private void UploadPhotos()
        {
            while (abortUploadThread == false)
            {
                Thread.Sleep(threadSleepTime);
                while (uploadQueue.Count > 0 && abortUploadThread == false)
                {
                    while (pauseUploadThread == true)
                    {
                        Thread.Sleep(threadSleepTime);
                    }
                    lock (uploadQueue)
                    {
                        PhotoToUpload curPhoto = uploadQueue.Dequeue();
                        photo UploadedPhoto = FacebookInterfaces.UploadPhoto(curPhoto);
                        MainWindow.AddUploadedPhoto(new FacebookPhoto(UploadedPhoto, curPhoto.photoPath));
                    }
                }
            }
        }

        //Only to be used by the downloadThread. Do not call directly.
        static private void DownloadPhotos()
        {
            while (abortDownloadThread == false)
            {
                Thread.Sleep(threadSleepTime);
                while (downloadQueue.Count > 0 && abortDownloadThread == false)
                {
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
                            int PhotoCounter = 1;
                            String albumName = FacebookInterfaces.AlbumName(DownloadedPhoto.aid);
                            String upPhotoPath = MainWindow.UpPhotoPath();
                            String path = upPhotoPath + albumName + @"\Photo " + PhotoCounter.ToString() + DownloadedPhotoExtension;
                            while (File.Exists(path))
                            {
                                PhotoCounter++;
                                path = upPhotoPath + albumName + @"\Photo " + PhotoCounter.ToString() + DownloadedPhotoExtension;
                            }
                            Directory.CreateDirectory(StringUtils.GetFullFolderPathFromPath(path));
                            System.Drawing.Bitmap imageData = new System.Drawing.Bitmap((System.Drawing.Bitmap)DownloadedPhoto.picture_big.Clone());
                            MainWindow.WatchersIgnoreFile(path);
                            imageData.Save(path, ImageFormat.Png);
                            MainWindow.WatchersUnIgnoreFile(path);
                            MainWindow.AddUploadedPhoto(new FacebookPhoto(DownloadedPhoto, path));
                        }
                        catch (System.Net.WebException ex)
                        {
                        	//add the photo back to the queue and try again later
                            downloadQueue.Enqueue(pidToDownload);
                            Thread.Sleep(threadSleepTime);
                        }
                    }
                }
            }
        }

        static public void SnycPhotos()
        {
            List<PID> allPIDs = FacebookInterfaces.AllFacebookPhotos();
            pauseDownloadThread = true;
            foreach (PID pid in allPIDs)
            {
                if (!MainWindow.HasPhoto(pid))
                {
                    lock (downloadQueue)
                    {
                        downloadQueue.Enqueue(pid);
                    }
                }
            }
            pauseDownloadThread = false;
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
