using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Facebook.Schema;
using UpPhotoLibrary;

namespace UpPhoto
{
    public class UpdateHandler
    {
        public Queue<PhotoToUpload> uploadQueue = new Queue<PhotoToUpload>();
        Queue<PID> downloadQueue = new Queue<PID>();

        AutoStartThread uploadThread;
        bool abortUploadThread = false;
        bool pauseUploadThread = false;

        AutoStartThread downloadThread;
        bool abortDownloadThread = false;
        bool pauseDownloadThread = false;

        AutoStartThread detectPIDthread;
        bool abortDetectPIDthread = false;

        const String DownloadedPhotoExtension = @".png";
        const int threadSleepTime = 500; //this is the time in milliseconds between checking if there are photos to be uploaded.
        MainWindow parent;

        public UpdateHandler(MainWindow newParent)
        {
            parent = newParent;

            detectPIDthread = new AutoStartThread(SyncPhotos, ApartmentState.STA);
            uploadThread = new AutoStartThread(UploadPhotos, ApartmentState.STA);
            downloadThread = new AutoStartThread(DownloadPhotos, ApartmentState.STA);
        }

        public void StopThreads()
        {
            abortDownloadThread = true;
            abortUploadThread = true;
            abortDetectPIDthread = true;

            uploadThread.Join();
            downloadThread.Join();

            detectPIDthread.Abort();
        }

        //Only to be used by the uploadThread. Do not call directly.
        private void UploadPhotos()
        {
            try
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
                        UploadNextQueuedPhoto();
                    }
                }
            }
            catch (Exception ex)
            {
                //On error, restart thread. If something is causing exceptions indefinitely, we should catch that specific type of exception and handle/ignore it.
                ErrorHandler.LogException(ex);

                uploadThread = new AutoStartThread(UploadPhotos, ApartmentState.STA);
            }
        }

        private void UploadNextQueuedPhoto()
        {
            lock (uploadQueue)
            {
                PhotoToUpload curPhoto = uploadQueue.Dequeue();
                try
                {
                    photo UploadedPhoto = FacebookInterfaces.UploadPhoto(curPhoto);
                    //if we are here, the uploading worked and we can set the tray icon to connected
                    //if the uploading failed, an exception would have been thrown
                    parent.SetConnectedStatus(true);
                    parent.AddUploadedPhoto(new FacebookPhoto(UploadedPhoto, curPhoto.photoPath));
                }
                catch (System.Net.WebException)
                {
                    //could not upload, add photo back to queue and set connected status to false
                    uploadQueue.Enqueue(curPhoto);
                    parent.SetConnectedStatus(false);
                }
            }
        }

        private Boolean IsPhotoDownloaded(photo DownloadedPhoto)
        {
            try
            {
                foreach (String filename in Directory.GetFiles(parent.UpPhotoPath() + FacebookInterfaces.AlbumName(new AID(DownloadedPhoto.aid))))
                {
                    Image b = Image.FromFile(filename);
                    if (b.Equals(DownloadedPhoto.picture_big))
                    {
                        return true;
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            return false;
        }

        //Only to be used by the downloadThread. Do not call directly.
        private void DownloadPhotos()
        {
            try
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
                            DownloadNextQueuedPhoto();
                        }
                    }
                }
            }
            catch (System.Net.WebException)
            {
                parent.SetConnectedStatus(false);
                System.Threading.Thread.Sleep(parent.WaitForInternetConnectionTime);
                downloadThread = new AutoStartThread(DownloadPhotos, ApartmentState.STA);
            }
            catch (Exception ex)
            {
                //On error, restart thread. If something is causing exceptions indefinitely, we should catch that specific type of exception and handle/ignore it.
                ErrorHandler.LogException(ex);
                downloadThread = new AutoStartThread(DownloadPhotos, ApartmentState.STA);
            }
        }

        private void DownloadNextQueuedPhoto()
        {
            PID pidToDownload = downloadQueue.Dequeue();
            try
            {
                photo DownloadedPhoto = FacebookInterfaces.DownloadPhoto(pidToDownload);
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
            catch (PhotoDoesNotExistException)
            {
                //photo has been deleted between detecting it and downloading it.
                //fine to do nothing since it has already been removed from the queue
            }
            catch (System.Net.WebException)
            {
                //add the photo back to the queue
                downloadQueue.Enqueue(pidToDownload);
                throw;
            }
        }

        public void SyncPhotos()
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
                System.Threading.Thread.Sleep(parent.WaitForInternetConnectionTime);
                if (abortDetectPIDthread == false)
                {
                    detectPIDthread = new AutoStartThread(SyncPhotos, ApartmentState.STA);
                }
            }
            catch (ThreadAbortException)
            {
                //we are aborting the thread, no need to do anything.
            }
            catch (Exception ex)
            {
                //On error, restart thread. If something is causing exceptions indefinitely, we should catch that specific type of exception and handle/ignore it.
                ErrorHandler.LogException(ex);

                detectPIDthread = new AutoStartThread(SyncPhotos, ApartmentState.STA);
            }
        }

        private String GeneratePath(photo DownloadedPhoto)
        {
            int PhotoCounter = 1;
            String albumName = FacebookInterfaces.AlbumName(new AID(DownloadedPhoto.aid));
            String upPhotoPath = parent.UpPhotoPath();
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
