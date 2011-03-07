using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MakeHardLink
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

        static void Main(string[] args)
        {
            CreateSymbolicLink(@"UpPhoto", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\UpPhoto", 1);
        }
    }
}
