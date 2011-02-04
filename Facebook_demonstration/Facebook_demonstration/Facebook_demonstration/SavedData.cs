using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Facebook_demonstration
{
    class PhotoAIDtoPhotoPathMap : Dictionary<String, String> { }

    [Serializable()]
    class SavedData : ISerializable
    {
        List<String> WatchedFolders;
        PhotoAIDtoPhotoPathMap AllPhotos;

        public SavedData(List<String> newWatchList)
        {
            WatchedFolders = newWatchList;
        }

        public SavedData(SerializationInfo info, StreamingContext ctxt)
        {
            WatchedFolders = (List<String>)info.GetValue("WatchList", typeof(List<String>));
            AllPhotos = (PhotoAIDtoPhotoPathMap)info.GetValue("AllPhotos", typeof(PhotoAIDtoPhotoPathMap));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("WatchList", WatchedFolders);
            info.AddValue("AllPhotos", AllPhotos);
        }

        public List<String> SavedWatchedFolders()
        {
            return WatchedFolders;
        }
    }
}
