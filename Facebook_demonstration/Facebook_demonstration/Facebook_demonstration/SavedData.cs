using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Facebook_demonstration
{
    [Serializable()]
    class SavedData : ISerializable
    {
        List<String> WatchedFolders;
        Dictionary<String, String> AllPhotos;

        public SavedData(List<String> newWatchList, Dictionary<String, String> newAllPhotos)
        {
            WatchedFolders = newWatchList;
            AllPhotos = newAllPhotos; 
        }

        public SavedData(SerializationInfo info, StreamingContext ctxt)
        {
            WatchedFolders = (List<String>)info.GetValue("WatchList", typeof(List<String>));
            AllPhotos = (Dictionary<String, String>)info.GetValue("AllPhotos", typeof(Dictionary<String, String>));
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

        public Dictionary<String, String> SavedPIDtoPhotoMap()
        {
            return AllPhotos;
        }
    }
}
