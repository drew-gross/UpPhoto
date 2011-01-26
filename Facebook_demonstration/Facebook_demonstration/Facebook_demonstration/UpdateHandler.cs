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
        public void FaceboxWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //MessageBox.Show(e.FullPath + " changed!");
        }

        public void FaceboxWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //MessageBox.Show(e.FullPath + " created!");
            if (Path.GetExtension(e.FullPath) == ".jpg" ||
                Path.GetExtension(e.FullPath) == ".JPG")
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                FacebookInterfaces.PublishPhotos(album, e.FullPath);
            }
        }

        public void FaceboxWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            //MessageBox.Show(e.FullPath + " deleted!");
            if (Path.GetExtension(e.FullPath) == ".jpg")
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                FacebookInterfaces.DeletePhotos(album, e.FullPath);
            }
        }

        public void FaceboxWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            //MessageBox.Show(e.FullPath + " renamed!");
        }

        public string GetAlbumFromPath(string path)
        {
            string[] folders = path.Split('\\');
            //returns second last item (C:\Facebox\albumname\picture.jpg
            //becomes {"C:", "Facebox", "albumname", "picture.jpg"}
            return folders[folders.Length - 2];
        }
    }
}
