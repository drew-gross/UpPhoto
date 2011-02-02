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
        List<WatchedFolder> WatchList;

        public SavedData(List<WatchedFolder> newWatchList)
        {
            WatchList = newWatchList;
        }

        public SavedData(SerializationInfo info, StreamingContext ctxt)
        {
            WatchList = (List<WatchedFolder>)info.GetValue("WatchList", typeof(List<WatchedFolder>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("WatchList", WatchList);
        }
    }
}
