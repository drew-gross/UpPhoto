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

        static void PublishAsyncCompleted(string result, Object state, FacebookException e)
        {

        }

        public static bool PublishPhotos(String AlbumName, String FileName)
        {

            fbService.ApplicationKey = "120183071389019";
            List<Enums.ExtendedPermissions> perms = new List<Enums.ExtendedPermissions>
            {
                Enums.ExtendedPermissions.photo_upload,
                Enums.ExtendedPermissions.offline_access
            };
            fbService.ConnectToFacebook(perms);
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

        private static void CreateAlbumCallback(album album, object state, FacebookException e)
        {

        }

        private static void UploadCallback(photo p, object state, FacebookException e)
        {

        }

        private static  void PhotoTagger(string photoPid)
        {

        }
    }
}
