using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpPhoto
{
    class ErrorHandler
    {
        static String ErrorLogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\UpPhoto\UpPhotoError.log";

        public static void LogException(System.Exception ex)
        {
            try
            {
                WriteException(ex);
            }
            catch (Exception ex2)
            {
                LogFatalException(ex2); 
            }
        }

        public static void LogFatalException(System.Exception ex)
        {
            try
            {
                WriteException(ex);
                MainWindow.QuitUpPhoto();
            }
            catch (Exception)
            {
                MainWindow.QuitUpPhoto();
            }
        }

        static void WriteException(System.Exception ex)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(ErrorLogFilePath, true);
            file.WriteLine(ex.ToString());
            file.Close();
        }
    }
}
