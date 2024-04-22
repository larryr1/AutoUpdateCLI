using AutoUpdate_CLI.Classes.Network;
using AutoUpdate_CLI.Classes.Network.API;
using AutoUpdate_CLI.Classes.SystemAbstract;
using AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract;
using AutoUpdate_CLI.Classes.Update;
using AutoUpdate_CLI.Classes.Utility;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
            Console.WriteLine("Searching for configuration server...");
            IPEndPoint serverEndPoint = new DiscoveryClient().DiscoverServer(10);
            if (serverEndPoint == null)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nA configuration server was not broadcasted. Please check the network and firewall settings and try again.");
                Console.WriteLine("\nPress enter to exit.");
                Console.ReadLine();
                Environment.Exit(1);
            }

            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Received broadcast.");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("Contacting configuration server at " + serverEndPoint.ToString());

            // Start obtaining values for API configuration
            string mac = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .First();

            string clientDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string clientHostname = Environment.MachineName;

            // Create api configuration
            ClientConfiguration apiConfig = new ClientConfiguration
            {
                ServerEndpoint = serverEndPoint,
                ClientIdentifier = mac,
                ClientDomain = clientDomain,
                ClientHostname = clientHostname
            };

            APIClient.InitializeClient(apiConfig);
            APIClient.RegisterClient();

            Console.WriteLine("Searching for available updates.");
            APIClient.SetPhase(APIClient.ApplicationPhase.SCAN);

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
                APIClient.SetPhase(APIClient.ApplicationPhase.DOWNLOAD);
                IUpdate[] updates = { };
                for (int i = 0; i < downloadTarget.Count; i++)
                {
                    updates.Append(downloadTarget[i]);
                }
                APIClient.SetUpdates(updates);

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
                APIClient.SetPhase(APIClient.ApplicationPhase.INSTALL);
                IUpdate[] updates = { };
                for (int i = 0; i < installTarget.Count; i++)
                {
                    updates.Append(installTarget[i]);
                }
                APIClient.SetUpdates(updates);
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
                APIClient.SetPhase(APIClient.ApplicationPhase.COMPLETE);
                //Power.Shutdown("Updates are finished. The system will shut down in 10 seconds.");
                Power.Restart("The machine is restarting to log in to the testing account.");
                AutoLogon.Enable("fsa@testing", "student", "testing");
                System.Threading.Thread.Sleep(120000);

            } else
            {
                PostUpdateCheck.SetChecked();
                APIClient.SetPhase(APIClient.ApplicationPhase.CHECK);
                AutoLogon.Enable("sa-admin", "?");
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