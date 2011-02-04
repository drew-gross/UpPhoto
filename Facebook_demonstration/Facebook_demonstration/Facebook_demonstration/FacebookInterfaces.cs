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

        const String UpPhotoPath = @"C:\Facebox";
        const String UpPhotoCaption = @"Uploaded from UpPhoto";
        const String DownloadedPhotoExtension = @".png";

        static FacebookInterfaces()
        {
            ConnectToFacebook();
        }

        static void PublishAsyncCompleted(string result, Object state, FacebookException e)
        {

        }

        public static void ConnectToFacebook()
        {
            fbService.ApplicationKey = "120183071389019";
            List<Enums.ExtendedPermissions> perms = new List<Enums.ExtendedPermissions>
            {
                Enums.ExtendedPermissions.photo_upload,
                Enums.ExtendedPermissions.offline_access,
                Enums.ExtendedPermissions.user_photos
            };
            fbService.ConnectToFacebook(perms);
        }

        public static void DeletePhotos(String AlbumName, String FileName)
        {
            //this may need to change dramatically to account for things like multiple
            //albums with the same name
            throw new NotImplementedException();
        }

        public static photo UploadPhoto(PhotoToUpload photo)
        {
            IList<album> albums = fbService.Photos.GetAlbums();

            string albumAid = String.Empty;
            foreach (album album in albums)
            {
                if (album.name == photo.albumName)
                {
                    albumAid = album.aid;
                    break;
                }
            }

            if (albumAid == String.Empty)
            {
                albumAid = fbService.Photos.CreateAlbum(photo.albumName, null, UpPhotoCaption).aid;
            }

            photo newPhoto = fbService.Photos.Upload(albumAid, UpPhotoCaption, new FileInfo(photo.photoPath));
            return newPhoto;
        }

        public static photo DownloadPhoto(String pid)
        {
            //note: must do stuff if the photo doesn't exist.
            return fbService.Photos.Get(null, null, new List<String> { pid })[0];
        }
        
        public static List<String> AllFacebookPhotos()
        {
            //need to handle WebException
            IList<album> albums = fbService.Photos.GetAlbums();
            List<String> pidlist = new List<String>();
            foreach (album album in albums)
            {
                IList<photo> photos = fbService.Photos.Get(null, album.aid, null);
                foreach (photo photo in photos)
                {
                    pidlist.Add(photo.pid);
                }
            }
            return pidlist;
        }

        public static String AlbumName(String aid)
        {
            return fbService.Photos.GetAlbums(0, new List<String> { aid })[0].name;
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
