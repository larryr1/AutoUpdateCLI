using Microsoft.Win32;

namespace AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract
{
    internal class RegistryController
    {
        public static RegistryKey GetApplicationKey()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\", true);
            RegistryKey aaKey = key.OpenSubKey("AutoUpdateCLI", true);

            if (aaKey == null) 
            {
                aaKey = key.CreateSubKey("AutoUpdateCLI");
                CreateKeyValues();
            }
            
            return aaKey;
        }

        public static void DeleteApplicationKey()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\", true);
            key.DeleteSubKeyTree("AutoUpdateCLI");
            key.Close();
        }

        public static void CreateKeyValues()
        {
            // Get key and existing values
            RegistryKey ak = GetApplicationKey();
            string[] valueNames = ak.GetValueNames();

            // Set required values
            EnsureValueExists(ak, "PostUpdateCheck", 0, RegistryValueKind.DWord);
            EnsureValueExists(ak, "StoredLegalCaption", "", RegistryValueKind.String);
            EnsureValueExists(ak, "StoredLegalText", "", RegistryValueKind.String);
            EnsureValueExists(ak, "LegalNoticeDisabled", 0, RegistryValueKind.DWord);

            ak.Close();
        }

        public static void EnsureValueExists(RegistryKey key, string name, object value, RegistryValueKind kind)
        {
            if (key.GetValue(name) != null) return;
            key.SetValue(name, value, kind);
        }
    }
}
