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
    class FacebookPhoto : ISerializable
    {
        String photoAID;
        String photoPath;

        public FacebookPhoto(photo newPhoto)
        {
            photoAID = newPhoto.aid;
        }

        public FacebookPhoto(SerializationInfo info, StreamingContext ctxt)
        {
            photoAID = (String)info.GetValue("photoAID", typeof(String));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("photoAID", photoAID);
        }
    }
}
