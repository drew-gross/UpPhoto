using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostInstallScript
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Process.Start("UpPhoto.exe");
            Environment.Exit(0);
        }
    }
}
