using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace UpPhotoLibrary
{
    public class UpdateInfo
    {
        Dictionary<String, String> ParsedUpdateFile = new Dictionary<String, String>();

        public UpdateInfo()
        {
            HttpWebRequest UpdateInfoRequest = (HttpWebRequest)WebRequest.Create(@"http://www.upphoto.ca/updates/updates.txt");
            WebResponse UpdateInfoResponse = UpdateInfoRequest.GetResponse();

            StreamReader UpdateInfoReader = new StreamReader(UpdateInfoResponse.GetResponseStream());
            String UpdateInfoText = UpdateInfoReader.ReadToEnd();
            ParseUpdateString(UpdateInfoText);
            UpdateInfoReader.Close();
            UpdateInfoResponse.Close();
        }

        void ParseUpdateString(String data) 
        {
            data = data.Replace("\r\n", "\n");
            String[] vars = data.Split('\n');
            foreach (String var in vars)
            {
                if (var.Length != 0 && var.Contains('='))
                {
                    ParsedUpdateFile[var.Split('=')[0]] = var.Split('=')[1];
                }
            }
        }

        public String WindowExecutablePath()
        {
            return ParsedUpdateFile["WindowsExecutable"];
        }

        public String WindowsUpdater()
        {
            return ParsedUpdateFile["WindowsUpdater"];
        }

        public int WindowsVersion()
        {
            return int.Parse(ParsedUpdateFile["WindowsVersion"]);
        }
    }
}
