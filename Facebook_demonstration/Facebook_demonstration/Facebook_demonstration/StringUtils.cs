using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpPhoto
{
    static class StringUtils
    {
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

        public static bool IsImageExtension(string extension)
        {
            string[] validExtensions = { @".jpg", @".JPG", @".jpeg", @".JPEG", @".bmp", @".BMP", @".png", @".PNG", @".tiff", @".TIFF", @".raw", @".RAW" };
            return validExtensions.Contains(extension);
        }
    }
}
