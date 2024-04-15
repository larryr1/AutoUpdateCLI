using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.Diagnostic
{
    public class DiskSpace
    {
        public static void WriteReport()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive \"{0}\" at {1} ({2} {3} Drive)", d.VolumeLabel, d.Name, d.DriveType, d.DriveFormat);
                if (d.IsReady == true)
                {
                    Console.WriteLine(
                        "  Bytes Available to Current User:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Bytes Available:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Drive Total Bytes:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }
        }
    }
}
