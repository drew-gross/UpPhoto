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

        public SavedData(List<String> newWatchList)
        {
            WatchedFolders = newWatchList;
        }

        public SavedData(SerializationInfo info, StreamingContext ctxt)
        {
            WatchedFolders = (List<String>)info.GetValue("WatchList", typeof(List<String>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("WatchList", WatchedFolders);
        }

        public List<String> SavedWatchedFolders()
        {
            return WatchedFolders;
        }
    }
}
