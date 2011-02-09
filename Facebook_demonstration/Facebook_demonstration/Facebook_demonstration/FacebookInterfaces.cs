﻿using System;
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
    [Serializable]
    public class PID
    {
        public String pidStr = String.Empty;
        public PID()
        {

        }
        public PID(String newPid)
        {
            pidStr = newPid;
        }
        public override string ToString()
        {
            return pidStr;
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || (obj as PID == null)) return false;
            return pidStr.Equals((obj as PID).pidStr);
        }
        public override int GetHashCode()
        {
            return pidStr.GetHashCode();
        }
    }

    public class AID
    {
        public String aidStr = String.Empty;
        public AID()
        {

        }
        public AID(String newAID)
        {
            aidStr = newAID;
        }
        public override string ToString()
        {
            return aidStr;
        }
    }

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

            AID albumAid = new AID();
            foreach (album album in albums)
            {
                if (album.name == photo.albumName)
                {
                    albumAid = new AID(album.aid);
                    break;
                }
            }

            if (albumAid.ToString() == String.Empty)
            {
                albumAid = new AID(fbService.Photos.CreateAlbum(photo.albumName, null, UpPhotoCaption).aid);
            }

            photo newPhoto = fbService.Photos.Upload(albumAid.ToString(), UpPhotoCaption, new FileInfo(photo.photoPath));
            return newPhoto;
        }

        public static photo DownloadPhoto(PID pid)
        {
            //note: must do stuff if the photo doesn't exist.
            return fbService.Photos.Get(null, null, new List<String> { pid.ToString() })[0];
        }
        
        public static List<PID> AllFacebookPhotos()
        {
            //need to handle WebException
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
