using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Utility;
using Facebook.Winforms.Components;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace UpPhoto
{
    static class FacebookInterfaces
    {
        static readonly FacebookService fbService = new FacebookService();

        const String UpPhotoPath = @"~/Desktop/UpPhoto";
        const String UpPhotoCaption = @"Uploaded using UpPhoto";
        const String DownloadedPhotoExtension = @".png";

        static FacebookInterfaces()
        {
            if(IsInternetConnection())
            {
                ConnectToFacebook();
            }
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

        public static bool IsInternetConnection()
        {
            HttpWebRequest facebookRequest = (HttpWebRequest)WebRequest.Create("http://www.facebook.com");
            facebookRequest.AllowAutoRedirect = false;
            HttpWebResponse facebookResponse;
            Uri facebookUri = new Uri("http://www.facebook.com");
            while (true)
            {
                try
                {
                    facebookResponse = (HttpWebResponse)facebookRequest.GetResponse();
                    if (facebookResponse.ResponseUri == facebookUri)
                        return true;
                    return false;
                }
                catch (System.Net.WebException e)
                {
                    throw e;
                }
            }            

        }

        public static AID GetAlbumAID(String AlbumName)
        {
            IList<album> albums = fbService.Photos.GetAlbums();
            throw new System.Net.WebException();
            foreach (album x in albums)
            {
                if (x.name == AlbumName)
                {
                    return new AID(x.aid);
                }
            }
            return null;
        }

        public static photo UploadPhoto(PhotoToUpload photo)
        {
            AID albumAid = GetAlbumAID(photo.albumName);

            if (albumAid == null)
            {
                albumAid = new AID(fbService.Photos.CreateAlbum(photo.albumName, null, UpPhotoCaption).aid);
            }

            photo newPhoto = fbService.Photos.Upload(albumAid.ToString(), UpPhotoCaption, new FileInfo(photo.photoPath));
            return newPhoto;
        }

        public static void UploadVideo(String FilePath)
        {
            fbService.Video.Upload(Path.GetFileNameWithoutExtension(FilePath), UpPhotoCaption, new FileInfo(FilePath));
        }

        public static photo DownloadPhoto(PID pid)
        {
            //note: must do stuff if the photo doesn't exist.
            return fbService.Photos.Get(null, null, new List<String> { pid.ToString() })[0];
        }
        
        public static List<PID> AllFacebookPhotos()
        {
            //need to handle WebException and FacebookException
            IList<album> albums = fbService.Photos.GetAlbums();
            List<PID> pidlist = new List<PID>();
            foreach (album album in albums)
            {
                IList<photo> photos = fbService.Photos.Get(null, album.aid, null);
                foreach (photo photo in photos)
                {
                    pidlist.Add(new PID(photo.pid));
                }
            }
            return pidlist;
        }

        public static String AlbumName(String aid)
        {
            return fbService.Photos.GetAlbums(0, new List<String> { aid })[0].name;
        }

        private static void PhotoTagger(string photoPid)
        {

        }

        public static void LogOut()
        {
            throw new NotImplementedException();
        }
    }
}
