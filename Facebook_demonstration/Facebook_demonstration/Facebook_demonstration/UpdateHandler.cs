using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
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
        const int uploadCheckTime = 500; //this is the time in milliseconds between checking if there are photos to be uploaded.
        MainWindow parent;

        static UpdateHandler() {
            uploadThread = new Thread(UpdateHandler.UploadPhotos);
            uploadThread.SetApartmentState(ApartmentState.STA);
            uploadThread.Start();
        }

        public UpdateHandler(MainWindow newParent)
        {
            parent = newParent;
        }

        public static void StopUpdating()
        {
            continueUploadThread = false;
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

        public void FaceboxWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //lets deal with changed photos the same way we deal with new photos. we add them to fb, but dont delete the original photo.
            FaceboxWatcher_Created(sender, e);

        }

        public void FaceboxWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (IsImageExtension(Path.GetExtension(e.FullPath)))
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                photosQueue.Enqueue(new PhotoToUpload(album, e.FullPath));
            }
        }

        public void FaceboxWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (IsImageExtension(Path.GetExtension(e.FullPath)))
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                FacebookInterfaces.DeletePhotos(album, e.FullPath);
            }
        }

        public void FaceboxWatcher_Renamed(object sender, RenamedEventArgs e)
        {
        }

        //this should probably go elsewhere
        public string GetAlbumFromPath(string path)
        {
            string[] folders = path.Split('\\');
            //returns second last item (C:\Facebox\albumname\picture.jpg
            //becomes {"C:", "Facebox", "albumname", "picture.jpg"}
            return folders[folders.Length - 2];
        }

        //this should probably go elsewhere
        public bool IsImageExtension(string extension)
        {
            string[] validExtensions = { @".jpg", @".JPG", @".jpeg", @".JPEG", @".bmp", @".BMP", @".png", @".PNG", @".tiff", @".TIFF", @".raw", @".RAW" };
            return validExtensions.Contains(extension);
        }
    }
}
