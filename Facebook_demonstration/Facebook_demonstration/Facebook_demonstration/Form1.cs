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
using System.Diagnostics;

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
            fbService.ApplicationKey = "120183071389019";
            List<Enums.ExtendedPermissions> perms = new List<Enums.ExtendedPermissions>
            {
                Enums.ExtendedPermissions.photo_upload,
                Enums.ExtendedPermissions.offline_access
            };
            fbService.ConnectToFacebook(perms);
        }

        #region Button Handlers
        private void screenShotButton_Click(object sender, EventArgs e)
        {
            PublishPhotos("Test Album 1", "C:/uploadthisfile.png");
        }
        #endregion

        #region Facebook Interfaces
        private static void PublishAsyncCompleted(string result, Object state, FacebookException e)
        {

        }

        public bool PublishPhotos(String AlbumName, String FileName)
        {            
            try
            {
                IList<album> albums = fbService.Photos.GetAlbums();

                string albumAid = "";
                foreach (album album in albums)
                {
                    if (album.name == AlbumName)
                    {
                        albumAid = album.aid;
                        break;
                    }
                }

                if (albumAid == "")
                {
                    albumAid = fbService.Photos.CreateAlbum(AlbumName, null, "Album description").aid;
                }

                //fbService.Photos.UploadAsync(albumAid, "Uploaded from Facebox", Photo, Enums.FileType.png, UploadCallback, null);
                fbService.Photos.Upload(albumAid, "Uploaded from Facebox", new FileInfo(FileName));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CreateAlbumCallback(album album, object state, FacebookException e)
        {
            
        }

        private void UploadCallback(photo p, object state, FacebookException e)
        {
            
        }

        private void PhotoTagger(string photoPid)
        {
            
        }
        #endregion

        private void FaceboxWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            MessageBox.Show("hooray!" + e.FullPath);
        }
    }
}
