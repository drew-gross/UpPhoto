using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpPhotoLibrary;
using System.Net;

namespace UpPhoto
{
    class SilentUpdater
    {
        UpdateInfo info = new UpdateInfo();
        int CurrentVersion = 1;

        public SilentUpdater()
        {
        }

        public bool NeedsUpdate()
        {
            try
            {
                if (info.WindowsVersion() > CurrentVersion)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogException(ex);
                return false;
            }
        }

        public void DownloadUpdater()
        {
            WebClient client = new WebClient();
            client.DownloadFile(info.WindowsUpdater(), "UpPhotoUpdater.exe");
        }
    }
}
