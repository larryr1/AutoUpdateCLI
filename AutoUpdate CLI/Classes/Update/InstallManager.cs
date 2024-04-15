using AutoUpdate_CLI.Classes.Network.API;
using AutoUpdate_CLI.Classes.SystemAbstract;
using AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract;
using AutoUpdate_CLI.Classes.Update.Display;
using System;
using WUApiLib;

namespace AutoUpdate_CLI.Classes.Update
{
    internal class InstallManager
    {
        public void Install(UpdateSession session, UpdateCollection installTarget)
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
                IUpdateInstallationResult updateResult = job.GetProgress().GetUpdateResult(i);
                if (updateResult.HResult == -2145116147)
                {
                    Console.WriteLine("An update needs additional downloaded content. Rerun the program.");
                }
                if (updateResult.RebootRequired)
                {
                    Console.WriteLine("The system needs a reboot. Configuring autologon.");
                    AutoLogon.Enable("user", "user");
                    LegalNotice.Disable();
                    AutoRun.SetExecutableRunOnceKey();
                    PostUpdateCheck.SetUnchecked();
                    Power.Restart("The system is restarting in 10 seconds to apply newly-installed updates.");
                }
            }
        }
    }
}
