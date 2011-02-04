using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Facebook_demonstration;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Utility;
using Facebook.Winforms.Components;
using FacebookController;

namespace Facebook_demonstration
{
    [Serializable()]
    public class FacebookPhoto : ISerializable
    {
        public String pid;
        public String path;

        public FacebookPhoto(photo newPid, String newPath)
        {
            pid = newPid.aid;
            path = newPath;
        }

        public FacebookPhoto(SerializationInfo info, StreamingContext ctxt)
        {
            pid = (String)info.GetValue("photoAID", typeof(String));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("photoAID", pid);
        }
    }
}
