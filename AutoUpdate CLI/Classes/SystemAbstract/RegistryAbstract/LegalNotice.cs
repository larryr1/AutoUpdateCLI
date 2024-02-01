using Microsoft.Win32;

namespace AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract
{
    /// <summary>
    /// Class to manage the status of Winlogon's legal caption feature.
    /// </summary>
    internal class LegalNotice
    {
        /// <summary>
        /// Stores the legal notice's caption and text and disables it.
        /// </summary>
        public static void Disable()
        {

            // Get keys, cancel if already disabled
            RegistryKey ak = RegistryController.GetApplicationKey();
            if ((int) ak.GetValue("LegalNoticeDisabled") == 1)
            {
                ak.Close();
                return;
            }

            RegistryKey alk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);

            // Ensure source caption values exist
            RegistryController.EnsureValueExists(alk, "LegalNoticeCaption", "", RegistryValueKind.String);
            RegistryController.EnsureValueExists(alk, "LegalNoticeText", "", RegistryValueKind.String);

            // Store values
            ak.SetValue("StoredLegalCaption", alk.GetValue("LegalNoticeCaption"), RegistryValueKind.String);
            ak.SetValue("StoredLegalText", alk.GetValue("LegalNoticeText"), RegistryValueKind.String);

            // Remove values
            alk.DeleteValue("LegalNoticeCaption");
            alk.DeleteValue("LegalNoticeText");

            // Mark as disabled
            ak.SetValue("LegalNoticeDisabled", 1, RegistryValueKind.DWord);

            // Close keys
            ak.Close();
            alk.Close();
        }

        /// <summary>
        /// Restores the legal notice's caption and text and enables it.
        /// </summary>
        public static void Enable()
        {
            // Get keys, cancel if already enabled
            RegistryKey ak = RegistryController.GetApplicationKey();
            if ((int) ak.GetValue("LegalNoticeDisabled") == 0)
            {
                ak.Close();
                return;
            }
            RegistryKey alk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);

            // Ensure source caption values exist
            RegistryController.EnsureValueExists(ak, "StoredLegalCaption", "", RegistryValueKind.String);
            RegistryController.EnsureValueExists(ak, "StoredLegalText", "", RegistryValueKind.String);

            // Restore values
            alk.SetValue("LegalNoticeCaption", ak.GetValue("StoredLegalCaption"), RegistryValueKind.String);
            alk.SetValue("LegalNoticeText", ak.GetValue("StoredLegalText"), RegistryValueKind.String);

            // Reset storage values
            ak.SetValue("StoredLegalCaption", "", RegistryValueKind.String);
            ak.SetValue("StoredLegalText", "", RegistryValueKind.String);

            // Mark as enabled
            ak.SetValue("LegalNoticeDisabled", 0, RegistryValueKind.DWord);

            // Close keys
            ak.Close();
            alk.Close();
        }
    }
}
