using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Utility;
using Facebook.Winforms.Components;
using System.IO;

namespace UpPhoto
{
    class AlbumDoesNotExistException : Exception { }
    class PhotoDoesNotExistException : Exception { }

    static class FacebookInterfaces
    {
        static readonly FacebookService fbService = new FacebookService();

        const String UpPhotoPath = @"~/Desktop/UpPhoto";
        const String UpPhotoCaption = @"Uploaded using UpPhoto";
        const String DownloadedPhotoExtension = @".png";

        static Dictionary<String, AID> AIDCache = new Dictionary<String, AID>();

        static FacebookInterfaces()
        {
            ConnectToFacebook();
            PopulateAIDCache();
        }

        private static void PopulateAIDCache()
        {
            AIDCache = new Dictionary<String, AID>();
            IList<album> allAlbums = fbService.Photos.GetAlbums();
            foreach (album curAlbum in allAlbums)
            {
                AIDCache[curAlbum.name] = new AID(curAlbum.aid);
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

        public static void DeletePhotos(String AlbumName, String FileName)
        {
            //this may need to change dramatically to account for things like multiple
            //albums with the same name
            //throw new NotImplementedException(); do nothing for now
        }

        public static AID GetAlbumAID(String AlbumName)
        {
            if (AIDCache.ContainsKey(AlbumName)) 
            {
                return AIDCache[AlbumName];
            }
            PopulateAIDCache();
            if (AIDCache.ContainsKey(AlbumName))
            {
                return AIDCache[AlbumName];
            }
            throw new AlbumDoesNotExistException();
        }

        public static photo UploadPhoto(PhotoToUpload photo)
        {
            CreateAlbumIfNotExists(photo.albumName);
            photo newPhoto = fbService.Photos.Upload(GetAlbumAID(photo.albumName).ToString(), UpPhotoCaption, new FileInfo(photo.photoPath));
            return newPhoto;
        }

        public static void UploadVideo(String FilePath)
        {
            fbService.Video.Upload(Path.GetFileNameWithoutExtension(FilePath), UpPhotoCaption, new FileInfo(FilePath));
        }

        public static void CreateAlbumIfNotExists(String AlbumName)
        {
            if (!AlbumExists(AlbumName))
            {
                AID newAID = new AID(fbService.Photos.CreateAlbum(AlbumName, null, UpPhotoCaption).aid);
                //this is going to cause problems if there are multiple albums with the same name. Not many people
                //have multiple albums with the same name, so no need to worry about it for now.
                AIDCache[AlbumName] = newAID;
            }
        }

        static bool AlbumExists(String AlbumName)
        {
            PopulateAIDCache();
            return AIDCache.ContainsKey(AlbumName);
        }

        public static photo DownloadPhoto(PID pid)
        {
            //note: must do stuff if the photo doesn't exist.
            IList<photo> PhotosWithAid = fbService.Photos.Get(null, null, new List<String> { pid.ToString() });
            if (PhotosWithAid.Count() == 0)
            {
                throw new PhotoDoesNotExistException();
            }
            return PhotosWithAid[0];
        }
        
        public static List<PID> AllFacebookPhotos()
        {
            //need to handle WebException and FacebookException

            //fine to use get all albums instead of the wrapper here
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

        public static String AlbumName(AID aid)
        {
            //no way to get the key from the value in AIDCache, so just get it from Facebook.
            return fbService.Photos.GetAlbums(0, new List<String> { aid.ToString() })[0].name;
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
