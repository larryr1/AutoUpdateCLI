using AutoUpdate_CLI.Classes.Utility.Progress;
using System;
using WUApiLib;

namespace AutoUpdate_CLI.Classes.Utility
{
    internal class InstallProgressDisplay : IInstallationProgressChangedCallback, IInstallationCompletedCallback
    {
        void IInstallationProgressChangedCallback.Invoke(IInstallationJob installationJob, IInstallationProgressChangedCallbackArgs callbackArgs)
        {
            IInstallationProgress progress = installationJob.GetProgress();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.WriteLine(installationJob.Updates[callbackArgs.Progress.CurrentUpdateIndex].Title);
            Console.WriteLine("Installation Progress");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            double updatePercentComplete = progress.CurrentUpdatePercentComplete / 100;
            double jobPercentComplete = progress.PercentComplete / 100;


            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Update Install Progress: " + ProgressTools.GenerateProgressBar(updatePercentComplete, 20, '|'));
            Console.WriteLine();
            Console.WriteLine("Job Download Progress: " + ProgressTools.GenerateProgressBar(jobPercentComplete, 20, '|'));
        }

        void IInstallationCompletedCallback.Invoke(IInstallationJob installationJob, IInstallationCompletedCallbackArgs callbackArgs)
        {
            Console.WriteLine("Installation completed.");
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
