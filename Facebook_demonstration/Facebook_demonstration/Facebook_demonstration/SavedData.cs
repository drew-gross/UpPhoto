using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Facebook_demonstration
{
    class PIDtoPathMap : Dictionary<String, String> { }

    [Serializable()]
    class SavedData : ISerializable
    {
        List<String> WatchedFolders;
        PIDtoPathMap AllPhotos;

        public SavedData(List<String> newWatchList)
        {
            WatchedFolders = newWatchList;
        }

        public SavedData(SerializationInfo info, StreamingContext ctxt)
        {
            WatchedFolders = (List<String>)info.GetValue("WatchList", typeof(List<String>));
            AllPhotos = (PIDtoPathMap)info.GetValue("AllPhotos", typeof(PIDtoPathMap));
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
