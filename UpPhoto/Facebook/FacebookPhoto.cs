using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using UpPhoto;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Utility;
using Facebook.Winforms.Components;

namespace UpPhoto
{
    [Serializable()]
    public class FacebookPhoto : ISerializable
    {
        public String pid;
        public String path;

        public FacebookPhoto(photo newPhoto, String newPath)
        {
            pid = newPhoto.pid;
            path = newPath;
        }

        public FacebookPhoto(SerializationInfo info, StreamingContext ctxt)
        {
            pid = (String)info.GetValue("photoPID", typeof(String));
            pid = (String)info.GetValue("path", typeof(String));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("photoPID", pid);
            info.AddValue("path", path);
        }
    }
}
