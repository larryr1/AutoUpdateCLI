using System;

namespace AutoUpdate_CLI.Classes.Utility.Progress
{
    internal static class ProgressTools
    {
        public static string GenerateProgressBar(double percent, int chars, char marker)
        {
            int markers = Convert.ToInt32(Math.Floor(percent * chars));
            int spaces = chars - markers;
            return "[" + new string(marker, markers) + new string(' ', spaces) + "] " + (percent * 100).ToString() + "%";
        }
    }
}
