﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUApiLib;
using static System.Collections.Specialized.BitVector32;

namespace AutoUpdate_CLI.Classes.Update
{
    internal class SearchManager
    {
        public void Search(UpdateSession session, out UpdateCollection outDownloadTarget, out UpdateCollection outInstallTarget)
        {
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
                }
                else
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
                    Console.WriteLine("This update requires exclusive handling. No others will be installed. The machine will reboot and rerun the program.");
                    System.Threading.Thread.Sleep(5000);
                    exclusiveFlag = true;
                }
                else if (!exclusiveFlag)
                {
                    installTarget.Add(update);
                    Console.WriteLine("Added update to install target.");
                }
            }

            outDownloadTarget = downloadTarget;
            outInstallTarget = installTarget;
        }
    }
}