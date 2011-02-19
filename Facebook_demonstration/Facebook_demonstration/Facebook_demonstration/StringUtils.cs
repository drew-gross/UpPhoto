using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpPhoto
{
    static class StringUtils
    {
        public static string[] ImageExtensions = { ".gif", ".jpg", ".png", ".psd", ".tiff", ".jp2", ".iff", ".wbmp", ".xbm" };
        public static string[] VideoExtensions = { ".3g2", ".3gp", ".3gpp", ".asf", ".avi", ".dat", ".flv", ".m4v", ".mkv", ".mod", ".mov", ".mp4", ".mpe", ".mpeg", ".mpeg4", ".mpg", ".nsv", ".ogm", ".ogv", ".qt", ".tod", ".vob", ".wmv" };

        public static String GetFolderFromPath(string path)
        {
            string[] folders = path.Split('\\');
            //returns second last item (C:\Facebox\albumname\picture.jpg
            //becomes {"C:", "Facebox", "albumname", "picture.jpg"}
            return folders[folders.Length - 2];
        }

        public static String GetFullFolderPathFromPath(string path)
        {
            string[] folders = path.Split('\\');
            
            return String.Join(@"\", (string[])folders.Take(folders.Count() - 1).ToArray());
        }

        public static bool IsImageExtension(string Extension)
        {
            return ImageExtensions.Contains(Extension.ToLower());
        }

        public static bool IsVideoExtension(string Extension)
        {
            return VideoExtensions.Contains(Extension.ToLower());
        }
    }
}
