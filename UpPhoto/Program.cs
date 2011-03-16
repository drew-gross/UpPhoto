using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using UpPhoto;
using System.Diagnostics;
using System.Threading;

namespace UpPhoto
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            //prevents 2 instances of UpPhoto running at once.
            using (Mutex mutex = new Mutex(true, "UpPhotoSingleProeccessMutex", out createdNew))
            {
                if (createdNew)
                {
                    new MainWindow();
                }
            }
        }
    }
}
