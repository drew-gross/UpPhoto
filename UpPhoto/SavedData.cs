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
        //IMPORTANT!!!! Any change to the saved data format must include a method to detect the format!
        //I forgot to add one in the first version so checking if the format is the first format will
        // have to involve checking for something that doesn't exist and catching the exception.

        int SaveDataVersion;
        int MostRecentSaveDataVersion = 2;

        bool IsFirstTimeRun = true;

        public SavedData(List<String> newWatchList, Dictionary<PID, String> newAllPhotos)
        {
            WatchedFolders = newWatchList;
            AllPhotos = newAllPhotos;
            SaveDataVersion = 2;
        }

        public SavedData(SerializationInfo info, StreamingContext ctxt)
        {
            lock (this)
            {
                try
                {
                    SaveDataVersion = (int)info.GetValue("SaveDataVersion", typeof(int));
                }
                //change this to the right kind of exception
                catch (SerializationException)
                {
                    SaveDataVersion = 1;
                }
                switch (SaveDataVersion)
                {
                    case 1:
                        LoadSaveDataVersion1(info);
                        break;
                    case 2:
                        LoadSaveDataVersion2(info);
                        break;
                    default:
                        break;
                }
                while (SaveDataVersion < MostRecentSaveDataVersion)
                {
                    switch (SaveDataVersion)
                    {
                        case 1:
                            ConvertSaveData1ToSaveData2();
                            break;
                        case 2:
                            break;
                        default:
                            throw new SerializationException();
                    }
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("WatchList", WatchedFolders);
            info.AddValue("AllPhotos", AllPhotos);
            info.AddValue("SaveDataVersion", SaveDataVersion);
            info.AddValue("IsFirstTimeRun", IsFirstTimeRun);
        }

        public List<String> SavedWatchedFolders()
        {
            return WatchedFolders;
        }

        public Dictionary<PID, String> SavedPIDtoPhotoMap()
        {
            return AllPhotos;
        }

        public bool SavedIsFirstTimeRun()
        {
            return IsFirstTimeRun;
        }

        private void LoadSaveDataVersion1(SerializationInfo info)
        {
            WatchedFolders = (List<String>)info.GetValue("WatchList", typeof(List<String>));
            AllPhotos = (Dictionary<PID, String>)info.GetValue("AllPhotos", typeof(Dictionary<PID, String>));
        }

        private void LoadSaveDataVersion2(SerializationInfo info)
        {
            WatchedFolders = (List<String>)info.GetValue("WatchList", typeof(List<String>));
            AllPhotos = (Dictionary<PID, String>)info.GetValue("AllPhotos", typeof(Dictionary<PID, String>));
            IsFirstTimeRun = (bool)info.GetValue("IsFirstTimeRun", typeof(bool));
        }

        private void ConvertSaveData1ToSaveData2()
        {
            IsFirstTimeRun = false;
            SaveDataVersion = 2;
        }
    }
}
