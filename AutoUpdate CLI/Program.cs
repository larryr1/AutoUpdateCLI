using AutoUpdate_CLI.Classes.Network;
using AutoUpdate_CLI.Classes.Network.API;
using AutoUpdate_CLI.Classes.SystemAbstract;
using AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract;
using AutoUpdate_CLI.Classes.Update;
using AutoUpdate_CLI.Classes.Utility;
using System;
using System.Net;
using System.Security.Principal;
using WUApiLib;

namespace AutoUpdate_CLI
{
    public class Program
    {
        [STAThread]
        public static void Execute(string[] args)
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

            // Disable AutoLogon and restore Legal Notice ASAP incase something goes wrong.
            AutoLogon.Disable();
            LegalNotice.Enable();

            // Setup
            SleepPrevention.DisableSleep();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("SCPA AutoUpdate CLI");
            Console.WriteLine("Download or contribute: https://github.com/larryr1/AutoUpdate");
            Console.WriteLine();

            // Find config server
            Console.WriteLine("Searching for configuration server... (timeout after 10 seconds)");
            IPEndPoint serverEndPoint = new DiscoveryClient().DiscoverServer(10);
            if (serverEndPoint == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("A configuration server was not broadcasted. Proceeding with default configuration.");
                Console.ForegroundColor = ConsoleColor.Cyan;
            } else
            {
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Received broadcast.");
                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("Contacting configuration server at " + serverEndPoint.ToString());
            }

            // Create api configuration
            ClientConfiguration apiConfig = new ClientConfiguration
            {
                ServerEndpoint = serverEndPoint,
                ClientIdentifier = Environment.MachineName,
                ClientDomain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName
            };

            APIClient apiClient = new APIClient(apiConfig);

            Console.WriteLine("Searching for available updates.");

            // Create the update session
            UpdateSession session = new UpdateSession
            {
                ClientApplicationID = "com.github.larryr1.AutoUpdate"
            };

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

            if (PostUpdateCheck.GetChecked())
            {
                Console.WriteLine("Cycle finished. Deleting registry keys...");
                AutoLogon.Disable();
                LegalNotice.Enable();
                RegistryController.DeleteApplicationKey();
                Console.WriteLine("Updates finished. Shutting down system.");
                Power.Shutdown("Updates are finished. The system will shut down in 10 seconds.");
                System.Threading.Thread.Sleep(120000);

            } else
            {
                PostUpdateCheck.SetChecked();
                AutoLogon.Enable("user", "user");
                LegalNotice.Disable();
                AutoRun.SetExecutableRunOnceKey();
                Console.WriteLine("Restarting the system for the post-update check.");
                Power.Restart("The machine is restarting in 10 seconds to complete updates.");
                System.Threading.Thread.Sleep(120000);
            }

            Console.WriteLine("Done! Press enter to exit.");
            Console.ReadLine();
            SleepPrevention.AllowSleep();
        }
    }
}