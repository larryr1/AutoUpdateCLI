using Microsoft.Win32;

namespace AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract
{
    /// <summary>
    /// Class to manage Winlogon's AutoLogon functionality.
    /// </summary>
    internal class AutoLogon
    {
        /// <summary>
        /// Configures Winlogon to log in the specified user automatically.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        public static void Enable(string username, string password, string domain)
        {
            // Set the proper keys for Winlogon to interpret.
            RegistryKey alk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            alk.SetValue("AutoAdminLogon", "1", RegistryValueKind.String);
            alk.SetValue("AutoLogonCount", 1, RegistryValueKind.DWord);
            alk.SetValue("DefaultUsername", username, RegistryValueKind.String);
            alk.SetValue("DefaultPassword", password, RegistryValueKind.String);
            alk.SetValue("DefaultDomainName", domain, RegistryValueKind.String);
            alk.SetValue("LastUsedUsername", username);
            alk.Close();
        }

        /// <summary>
        /// Disables Winlogon's AutoLogon functionality.
        /// </summary>
        public static void Disable()
        {
            // Emulates the default Winlogon behavior of deleting or modifying certain values when AutoLogon is disabled.
            RegistryKey alk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            alk.SetValue("AutoAdminLogon", "0", RegistryValueKind.String);
            alk.DeleteValue("AutoLogonCount");
            alk.DeleteValue("DefaultPassword");
            alk.Close();
        }
    }
}
