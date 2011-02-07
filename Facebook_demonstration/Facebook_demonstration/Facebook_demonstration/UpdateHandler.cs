using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Facebook_demonstration;
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
        static Queue<PhotoToUpload> photosQueue = new Queue<PhotoToUpload>(); //not thread safe

        static Thread uploadThread;
        static bool continueUploadThread = true;

        static Thread downloadThread;
        static bool continueDownloadThread = true;

        const String DownloadedPhotoExtension = @".png";
        const int uploadCheckTime = 500; //this is the time in milliseconds between checking if there are photos to be uploaded.
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
            continueUploadThread = false;
            continueDownloadThread = false;
        }

        //Only to be used by the uploadThread. Do not call directly.
        static private void UploadPhotos()
        {
            while (continueUploadThread)
            {
                Thread.Sleep(uploadCheckTime);
                while (photosQueue.Count > 0)
                {
                    PhotoToUpload curPhoto = photosQueue.Dequeue();
                    FacebookInterfaces.ConnectToFacebook();
                    photo UploadedPhoto = FacebookInterfaces.UploadPhoto(curPhoto);
                    MainWindow.AddUploadedPhoto(new FacebookPhoto(UploadedPhoto, curPhoto.photoPath));
                }
            }
        }

        //Only to be used by the downloadThread. Do not call directly.
        static private void DownloadPhotos()
        {
            while (continueDownloadThread)
            {

            }
        }

        static public void SnycPhotos()
        {
            List<String> allPIDs = FacebookInterfaces.AllFacebookPhotos();
            foreach (String pid in allPIDs)
            {
                if (!MainWindow.HasPhoto(pid))
                {
                    photo DownloadedPhoto = FacebookInterfaces.DownloadPhoto(pid);
                    int PhotoCounter = 1;
                    String path = MainWindow.UpPhotoPath() + FacebookInterfaces.AlbumName(DownloadedPhoto.aid) + @"\" + @"photo " + PhotoCounter.ToString() + DownloadedPhotoExtension;
                    while (File.Exists(path))
                    {
                        PhotoCounter++;
                        path = MainWindow.UpPhotoPath() + FacebookInterfaces.AlbumName(DownloadedPhoto.aid) + @"\" + @"Photo " + PhotoCounter.ToString() + DownloadedPhotoExtension;
                    }
                    Directory.CreateDirectory(StringUtils.GetFullFolderPathFromPath(path));
                    System.Drawing.Bitmap imageData = new System.Drawing.Bitmap((System.Drawing.Bitmap)DownloadedPhoto.picture_big.Clone());
                    imageData.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                    MainWindow.AddUploadedPhoto(new FacebookPhoto(DownloadedPhoto, path));
                }
            }
        }

        public void FaceboxWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //lets deal with changed photos the same way we deal with new photos. we add them to fb, but dont delete the original photo.
            FaceboxWatcher_Created(sender, e);

        }

        public void FaceboxWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (StringUtils.IsImageExtension(Path.GetExtension(e.FullPath)))
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                photosQueue.Enqueue(new PhotoToUpload(album, e.FullPath));
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
