using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Utility;
using Facebook.Winforms.Components;
using FacebookController;
using SHDocVw;

namespace Facebook_demonstration
{
    public partial class Form1 : Form
    {
        #region Class members
        readonly FacebookService fbService = new FacebookService();
        #endregion


        public Form1()
        {
            InitializeComponent();
        }

        #region Button Handlers
        private void screenShotButton_Click(object sender, EventArgs e)
        {
            FacebookInterfaces.PublishPhotos("Test Album 1", "C:/uploadthisfile.png");
        }
        #endregion

        #region Facebook Interfaces
        //private static void PublishAsyncCompleted(string result, Object state, FacebookException e)
        //{

        //}

        //public bool PublishPhotos(String AlbumName, String FileName)
        //{            
        //    try
        //    {
        //        IList<album> albums = fbService.Photos.GetAlbums();

        //        string albumAid = "";
        //        foreach (album album in albums)
        //        {
        //            if (album.name == AlbumName)
        //            {
        //                albumAid = album.aid;
        //                break;
        //            }
        //        }

        //        if (albumAid == "")
        //        {
        //            albumAid = fbService.Photos.CreateAlbum(AlbumName, null, "Album description").aid;
        //        }

        //        //fbService.Photos.UploadAsync(albumAid, "Uploaded from Facebox", Photo, Enums.FileType.png, UploadCallback, null);
        //        fbService.Photos.Upload(albumAid, "Uploaded from Facebox", new FileInfo(FileName));
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //private void CreateAlbumCallback(album album, object state, FacebookException e)
        //{
            
        //}

        //private void UploadCallback(photo p, object state, FacebookException e)
        //{
            
        //}

        //private void PhotoTagger(string photoPid)
        //{
            

        //}
        #endregion

        private void FaceboxWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show(e.FullPath + " changed!");
        }

        private void FaceboxWatcher_Created(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show(e.FullPath + " created!");
            if (Path.GetExtension(e.FullPath) == ".jpg")
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                try
                {
                    FacebookInterfaces.PublishPhotos(album, e.FullPath);
                }
                catch (System.UnauthorizedAccessException)
                {
                    //could not access e.fullPath (user possibly created a folder)
                }
            }
        }

        private void FaceboxWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show(e.FullPath + " deleted!");
            if (Path.GetExtension(e.FullPath) == ".jpg")
            {
                string album = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
                FacebookInterfaces.DeletePhotos(album, e.FullPath);
            }
        }

        private void FaceboxWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            MessageBox.Show(e.FullPath + " renamed!");
        }

        private string GetAlbumFromPath( string path )
        {
            string[] folders = path.Split('\\');
            //returns second last item (C:\Facebox\albumname\picture.jpg
            //becomes {"C:", "Facebox", "albumname", "picture.jpg"}
            return folders[folders.Length - 2];
        }
    }
}
