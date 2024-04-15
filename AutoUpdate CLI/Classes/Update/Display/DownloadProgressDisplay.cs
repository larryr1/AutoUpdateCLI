using AutoUpdate_CLI.Classes.Utility;
using System;
using System.Collections.Generic;
using WUApiLib;

namespace AutoUpdate_CLI.Classes.Update.Display
{
    internal class DownloadProgressDisplay : IDownloadProgressChangedCallback, IDownloadCompletedCallback
    {
        public static void ShowProgress(List<IDownloadJob> jobs)
        {
            /*// Progress Header
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("Download Job Progress");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            jobs.ForEach(job =>
            {
                IDownloadProgress progress = job.GetProgress();
                Console.WriteLine("-----------------------------------------------");
                ConsoleTools.WritePair("Update", StringTools.TruncateString(job.Updates[0].Title, 40));
                ConsoleTools.WritePair("Phase", nameof(progress.CurrentUpdateDownloadPhase));
                ConsoleTools.WritePair("Percent Complete", progress.PercentComplete.ToString() + "%");
            });*/
        }

        void IDownloadProgressChangedCallback.Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs callbackArgs)
        {
            ShowProgress(downloadJob);
        }

        void IDownloadCompletedCallback.Invoke(IDownloadJob downloadJob, IDownloadCompletedCallbackArgs callbackArgs)
        {
            ShowProgress(downloadJob);
        }

        private void ShowProgress(IDownloadJob job)
        {
            IDownloadProgress progress = job.GetProgress();
            Console.WriteLine("--[ Download Job ]---------------------------------------------");
            ConsoleTools.WritePair("Update", StringTools.TruncateString(job.Updates[0].Title, 40));
            ConsoleTools.WritePair("Phase", nameof(progress.CurrentUpdateDownloadPhase));
            ConsoleTools.WritePair("Percent Complete", progress.PercentComplete.ToString() + "%");
        }
    }
}
