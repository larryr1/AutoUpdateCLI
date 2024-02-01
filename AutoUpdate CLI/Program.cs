using AutoUpdate_CLI.Classes.Network;
using AutoUpdate_CLI.Classes.Network.API;
using AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract;
using AutoUpdate_CLI.Classes.Update;
using AutoUpdate_CLI.Classes.Utility;
using System;
using System.Net;
using System.Security.Principal;
using WUApiLib;

namespace AutoUpdate_CLI
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Ensure Adminstrator
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(principal));
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
            SleepPrevention.DisableSleep();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("SCPA AutoUpdate CLI");
            Console.WriteLine("Download or contribute: https://github.com/larryr1/AutoUpdate");
            Console.WriteLine();

            // Find config server
            Console.WriteLine("Searching for configuration server... (10 seconds max)");
            IPEndPoint serverEndPoint = new DiscoveryClient().DiscoverServer(10);
            if (serverEndPoint == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("A configuration server was not broadcasted. Proceeding with default configuration.");
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
             
            // Create api configuration
            ClientConfiguration apiConfig = new ClientConfiguration();
            apiConfig.serverEndpoint = serverEndPoint;
            apiConfig.clientIdentifier = Environment.MachineName;
            apiConfig.clientDomain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

            APIClient apiClient = new APIClient(apiConfig);

            Console.WriteLine("Searching for available updates.");

            // Create the update session
            UpdateSession session = new UpdateSession();
            session.ClientApplicationID = "com.github.larryr1.AutoUpdate";

            // Create searcher.
            SearchManager searchManager = new SearchManager();
            UpdateCollection downloadTarget = new UpdateCollection();
            UpdateCollection installTarget = new UpdateCollection();
            searchManager.Search(session, out downloadTarget, out installTarget);

            
            if (downloadTarget.Count > 0)
            {
                DownloadManager downloadManager = new DownloadManager();
                downloadManager.Download(session, downloadTarget);
            }
            else
            {
                Console.WriteLine("There are no updates to download.");
                System.Threading.Thread.Sleep(3000);
            }

            if (installTarget.Count > 0)
            {
                InstallManager installManager = new InstallManager();
                installManager.Install(session, installTarget);
            }
            else
            {
                Console.WriteLine("There are no updates to install.");
                System.Threading.Thread.Sleep(3000);
            }

            Console.WriteLine("Done! Press enter to exit.");
            Console.ReadLine();
            SleepPrevention.AllowSleep();
        }
    }
}