using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using UpPhotoLibrary;
using System.Threading;

namespace UpPhotoUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateInfo info = new UpdateInfo();

            WebClient downloader = new WebClient();
            Mutex waitForExit = new Mutex(false, "UpPhotoSingleProeccessMutex");
            waitForExit.WaitOne();
            downloader.DownloadFile(info.WindowExecutablePath(), "UpPhoto.exe");
            Process.Start("UpPhoto.exe");
        }
    }
}
