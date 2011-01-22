using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceProcess;
using System.Diagnostics;

namespace FaceBox
{
    public class MonitorFolder
    {
        public static FileSystemWatcher watcher;

        static string pathToFolder = @"C:\TestFolder";

        public static void Initiate()
        {
            Console.WriteLine("yo1");


            watcher = new FileSystemWatcher { Path = pathToFolder, IncludeSubdirectories = true};
            Console.WriteLine("yo2");
            watcher.Created += new FileSystemEventHandler(WatcherCreated);
        }

        public static void WatcherCreated(object source, FileSystemEventArgs e)
        {
            StreamWriter fout = new StreamWriter("output.txt");
            
            fout.WriteLine("works");
      
        }


    }
}
