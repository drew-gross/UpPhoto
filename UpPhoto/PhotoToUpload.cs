using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpPhoto
{
    class PhotoToUpload
    {
        public string albumName;
        public string photoPath;

        public PhotoToUpload(string newAlbumName, string newPhotoPath)
        {
            albumName = newAlbumName;
            photoPath = newPhotoPath;
        }
    }
}
