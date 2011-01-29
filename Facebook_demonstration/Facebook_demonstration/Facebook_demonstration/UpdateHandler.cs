using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Facebook_demonstration;

namespace FacebookApplication
{
    class UpdateHandler
    {
        static Queue<PhotoToUpload> photosQueue; //not thread safe

        public void FaceboxWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show(e.FullPath + " changed!");
        }

        public void FaceboxWatcher_Created(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show(e.FullPath + " created!");
            if (IsImageExtension(Path.GetExtension(e.FullPath)))
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                photosQueue.Enqueue(new PhotoToUpload(album, e.FullPath));
            }
        }

        public void FaceboxWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show(e.FullPath + " deleted!");
            if (IsImageExtension(Path.GetExtension(e.FullPath)))
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                FacebookInterfaces.DeletePhotos(album, e.FullPath);
            }
        }

        public void FaceboxWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            MessageBox.Show(e.FullPath + " renamed!");
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
            string[] validExtensions = { @".jpg", @".JPG", @".bmp", @".BMP" };
            return validExtensions.Contains(extension);
        }
    }
}
