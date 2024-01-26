using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.Utility
{
    internal static class StaticProgressBar
    {
        public static string Generate(double percent, int chars, char marker)
        {
            int markers = Convert.ToInt32(Math.Floor(percent * chars));
            int spaces = chars - markers;
            return "[" + new string(marker, markers) + new string(' ', spaces) + "] " + (percent * 100).ToString() + "%";
        }
    }
}
