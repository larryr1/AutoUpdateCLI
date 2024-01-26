using System;
using System.Security.Principal;
using AutoUpdate_CLI.Classes.Utility;
using WUApiLib;

namespace AutoUpdate_CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ensure Adminstrator
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if (!isElevated)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error: Not running as administrator. Restart the program as adminstrator.");
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
                Environment.Exit(0);
            }

            // Setup
            PreventSleep.DisableSleep();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("SCPA AutoUpdate CLI");
            Console.WriteLine("(C) 2024 Larry Rowe (https://github.com/larryr1/AutoUpdate)");

            Console.WriteLine();
            Console.WriteLine("Searching for available updates.");

            // Create the update session
            UpdateSession session = new UpdateSession();
            session.ClientApplicationID = "com.github.larryr1.AutoUpdate";

            // Create searcher, using Microsoft Update service.
            IUpdateSearcher searcher = session.CreateUpdateSearcher();
            searcher.ServerSelection = WUApiLib.ServerSelection.ssOthers;
            searcher.ServiceID = "7971f918-a847-4430-9279-4a52d1efe18d";

            ISearchResult result = searcher.Search("IsInstalled=0 and Type='Software' and IsHidden=0");

            Console.WriteLine("Found " + result.Updates.Count + " updates.");

            UpdateCollection downloadTarget = new UpdateCollection();
            UpdateCollection installTarget = new UpdateCollection();
            bool exclusiveFlag = false;
            for (int i = 0; i < result.Updates.Count; i++)
            {
                IUpdate update = result.Updates[i];
                Console.WriteLine("Found update: " + update.Title);
                if (!update.EulaAccepted)
                {
                    update.AcceptEula();
                    Console.WriteLine("Accepted the update's EULA.");
                }

                if (update.IsDownloaded)
                {
                    Console.WriteLine("This update is already download.");
                } else
                {
                    downloadTarget.Add(update);
                    Console.WriteLine("Added update to download target.");
                }

                if (update.IsInstalled)
                {
                    Console.WriteLine("This update is already installed.");
                }
                else if (!exclusiveFlag && update.InstallationBehavior.Impact == InstallationImpact.iiRequiresExclusiveHandling)
                {
                    Console.WriteLine("This update requires exclusive handling. No others will be installed.");
                    exclusiveFlag = true;
                }
                else if (!exclusiveFlag)
                {
                    installTarget.Add(update);
                    Console.WriteLine("Added update to install target.");
                }
            }

            
            if (downloadTarget.Count > 0)
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Starting downloader...");
                UpdateDownloader downloader = session.CreateUpdateDownloader();
                DownloadProgressDisplay progressDisplay = new DownloadProgressDisplay();
                downloader.Updates = downloadTarget;
                IDownloadJob job = downloader.BeginDownload(progressDisplay, progressDisplay, null);

                // Do nothing and wait for the job to complete
                while (!job.IsCompleted) { }

                Console.WriteLine("Cleaning up after download process...");
                job.CleanUp();
            }
            else
            {
                Console.WriteLine("There are no updates to download.");
            }

            if (installTarget.Count > 0)
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Starting installer...");
                IUpdateInstaller installer = session.CreateUpdateInstaller();
                installer.Updates = installTarget;
                InstallProgressDisplay progressDisplay = new InstallProgressDisplay();
                IInstallationJob job = installer.BeginInstall(progressDisplay, progressDisplay, null);

                // Do nothing and wait for the job to complete
                while (!job.IsCompleted) { }

                Console.Clear();
                Console.WriteLine("Cleaning up after download process...");
                job.CleanUp();

                // Analyze for unfinished updates
                for (int i = 0; i < job.Updates.Count; i++)
                {
                    IUpdate update = job.Updates[i];
                    IUpdateInstallationResult updateResult = job.GetProgress().GetUpdateResult(i);
                    if (updateResult.HResult == -2145116147)
                    {
                        Console.WriteLine("An update needs additional downloaded content. Rerun the program.");
                    }
                    if (updateResult.RebootRequired)
                    {
                        Console.WriteLine("The system needs a reboot.");
                    }
                }
            } else
            {
                Console.WriteLine("There are no updates to install.");
            }

            Console.WriteLine("Done! Press enter to exit.");
            Console.ReadLine();
            PreventSleep.AllowSleep();

        }
    }
}