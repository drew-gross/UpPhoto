using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace UpPhoto
{
    [Serializable()]
    class SavedData : ISerializable
    {
        List<String> WatchedFolders;
        Dictionary<PID, String> AllPhotos;

        public SavedData(List<String> newWatchList, Dictionary<PID, String> newAllPhotos)
        {
            WatchedFolders = newWatchList;
            AllPhotos = newAllPhotos; 
        }

        public SavedData(SerializationInfo info, StreamingContext ctxt)
        {
            lock (this)
            {
                WatchedFolders = (List<String>)info.GetValue("WatchList", typeof(List<String>));
                AllPhotos = (Dictionary<PID, String>)info.GetValue("AllPhotos", typeof(Dictionary<PID, String>));
            }
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

        public Dictionary<PID, String> SavedPIDtoPhotoMap()
        {
            return AllPhotos;
        }
    }
}
