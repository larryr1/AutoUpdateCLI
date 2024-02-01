namespace AutoUpdate_CLI.Classes.Utility
{
    internal class StringTools
    {
        public static string TruncateString(string text, int desiredLength)
        {
            return text.Length > desiredLength ? text.Remove(desiredLength) + "..." : text;
        }
    }
}
