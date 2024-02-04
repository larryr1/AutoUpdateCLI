using AutoUpdate_CLI.Classes.Update.Display;
using System;
using System.Collections.Generic;
using WUApiLib;

namespace AutoUpdate_CLI.Classes.Update
{
    internal class DownloadManager
    {
        public void Download(UpdateSession session, UpdateCollection downloadTarget)
        {
            // Know all downloaders
            List<UpdateDownloader> downloaders = new List<UpdateDownloader>();
            List<IDownloadJob> jobs = new List<IDownloadJob>();

            DownloadProgressDisplay progressDisplay = new DownloadProgressDisplay();

            // Create a new downloader for each update
            for (int i = 0; i < downloadTarget.Count; i++)
            {
                IUpdate currentUpdate = downloadTarget[i];
                UpdateDownloader thisDownloader = session.CreateUpdateDownloader();
                thisDownloader.Updates = new UpdateCollection() { downloadTarget[i] };
                downloaders.Add(thisDownloader);
            }

            // Start each job and maintain a reference to it
            downloaders.ForEach(downloader =>
            {
                jobs.Add(downloader.BeginDownload(progressDisplay, progressDisplay, null));
            });

            // Wait until all jobs are completed.
            bool allCompleted = false;
            while (!allCompleted)
            {
                int numCompleted = 0;
                jobs.ForEach(job =>
                {
                    if (job.IsCompleted) numCompleted++;
                });

                allCompleted = (numCompleted == jobs.Count);
            }

            // Clean up all jobs
            Console.WriteLine("Cleaning up after download process...");
            jobs.ForEach((job) =>
            {
                job.CleanUp();
            });
        }
    }
}
