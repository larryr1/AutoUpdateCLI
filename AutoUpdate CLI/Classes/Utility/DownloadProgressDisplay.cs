using System;
using WUApiLib;

namespace AutoUpdate_CLI.Classes.Utility
{
    internal class DownloadProgressDisplay : IDownloadProgressChangedCallback, IDownloadCompletedCallback
    {
        void IDownloadProgressChangedCallback.Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs callbackArgs)
        {
            IDownloadProgress progress = downloadJob.GetProgress();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.WriteLine(downloadJob.Updates[callbackArgs.Progress.CurrentUpdateIndex].Title);
            Console.WriteLine("Download Progress");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            double updatePercentComplete = progress.CurrentUpdatePercentComplete / 100;
            double jobPercentComplete = progress.PercentComplete / 100;

            decimal updateTotalBytes = progress.CurrentUpdateBytesDownloaded + progress.CurrentUpdateBytesToDownload;
            decimal jobTotalBytes = progress.TotalBytesDownloaded + progress.TotalBytesToDownload;


            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Update Download Progress: " + StaticProgressBar.Generate(updatePercentComplete, 20, '|'));
            Console.WriteLine("Total Update Size: " + FormatBytes(updateTotalBytes));
            Console.WriteLine();
            Console.WriteLine("Job Download Progress: " + StaticProgressBar.Generate(jobPercentComplete, 20, '|'));
            Console.WriteLine("Job Update Size: " + FormatBytes(jobTotalBytes));
        }

        void IDownloadCompletedCallback.Invoke(IDownloadJob downloadJob, IDownloadCompletedCallbackArgs callbackArgs)
        {
            Console.WriteLine("Download completed.");
        }

        private static string FormatBytes(decimal bytes)
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
