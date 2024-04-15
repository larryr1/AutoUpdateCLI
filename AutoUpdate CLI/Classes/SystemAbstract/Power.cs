using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.SystemAbstract
{
    internal class Power
    {
        public static void Shutdown(string reason)
        {
            System.Diagnostics.Process.Start("shutdown.exe", $"-s -t 10 -c \"{reason}\"");
        }

        public static void Restart(string reason)
        {
            System.Diagnostics.Process.Start("shutdown.exe", $"-r -t 10 -c \"{reason}\"");
        }
    }
}
