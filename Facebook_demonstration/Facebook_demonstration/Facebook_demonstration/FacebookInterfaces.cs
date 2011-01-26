using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Utility;
using Facebook.Winforms.Components;
using FacebookController;
using System.IO;

namespace Facebook_demonstration
{
    static class FacebookInterfaces
    {
        static readonly FacebookService fbService = new FacebookService();

        const string FaceboxPath = @"C:\Facebox";

        static void PublishAsyncCompleted(string result, Object state, FacebookException e)
        {

        }

        public static void SyncPhotos()
        {
            fbService.ApplicationKey = "120183071389019";
            List<Enums.ExtendedPermissions> perms = new List<Enums.ExtendedPermissions>
            {
                Enums.ExtendedPermissions.offline_access,
                Enums.ExtendedPermissions.user_photos
            };
            fbService.ConnectToFacebook(perms);
            IList<album> albums = fbService.Photos.GetAlbums();

            foreach (album album in albums)
            {

                string albumFolder = Path.Combine(FaceboxPath, album.name) + @"\";
                Directory.CreateDirectory(albumFolder);
                var photos = fbService.Photos.Get(null, album.aid, null);

                int photoCounter = 0;

                foreach (photo photo in photos)
                {
                    photoCounter++;
                    string fullFilePath = albumFolder + photoCounter.ToString() + ".bmp";
                    try
                    {
                        System.Drawing.Bitmap imageData = new System.Drawing.Bitmap(photo.picture_big);
                        imageData.Save(fullFilePath);
                    }
                    catch (System.Net.WebException)
                    {
                        //just leave it for now
                    }
                }
            }
        }

        public static void DeletePhotos(String AlbumName, String FileName)
        {
            //this may need to change dramatically to account for things like multiple
            //albums with the same name
            throw new NotImplementedException();
        }

        public static void PublishPhotos(String AlbumName, String FileName)
        {
            fbService.ApplicationKey = "120183071389019";
            List<Enums.ExtendedPermissions> perms = new List<Enums.ExtendedPermissions>
            {
                Enums.ExtendedPermissions.photo_upload,
                Enums.ExtendedPermissions.offline_access,
                Enums.ExtendedPermissions.user_photos
            };
            fbService.ConnectToFacebook(perms);
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

            fbService.Photos.Upload(albumAid, "Uploaded from Facebox", new FileInfo(FileName));
        }

        private static void CreateAlbumCallback(album album, object state, FacebookException e)
        {

        }

        private static void UploadCallback(photo p, object state, FacebookException e)
        {

        }

        private static void PhotoTagger(string photoPid)
        {

        }
    }
}
