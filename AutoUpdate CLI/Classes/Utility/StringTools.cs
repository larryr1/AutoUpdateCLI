namespace AutoUpdate_CLI.Classes.Utility
{
    internal class StringTools
    {
        public static string TruncateString(string text, int desiredLength)
        {
            return text.Length > desiredLength ? text.Remove(desiredLength) + "..." : text;
        }

        public static string FormatBytes(decimal bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            decimal dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024;
            }

            return string.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }
    }
}
