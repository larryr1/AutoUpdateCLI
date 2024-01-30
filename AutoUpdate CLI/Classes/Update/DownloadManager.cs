using AutoUpdate_CLI.Classes.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUApiLib;
using static System.Collections.Specialized.BitVector32;

namespace AutoUpdate_CLI.Classes.Update
{
    internal class DownloadManager
    {
        public void Download(UpdateSession session, UpdateCollection downloadTarget)
        {
            UpdateDownloader downloader = session.CreateUpdateDownloader();
            DownloadProgressDisplay progressDisplay = new DownloadProgressDisplay();
            downloader.Updates = downloadTarget;
            IDownloadJob job = downloader.BeginDownload(progressDisplay, progressDisplay, null);

            // Do nothing and wait for the job to complete
            while (!job.IsCompleted) { }

            Console.WriteLine("Cleaning up after download process...");
            job.CleanUp();
        }
    }
}
