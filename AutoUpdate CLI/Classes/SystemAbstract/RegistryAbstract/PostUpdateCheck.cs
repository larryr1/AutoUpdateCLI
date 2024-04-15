using Microsoft.Win32;
using System;

namespace AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract
{
    /// <summary>
    /// Class to manage Winlogon's AutoLogon functionality.
    /// </summary>
    internal class PostUpdateCheck
    {
        public static void SetChecked()
        {
            
            _enable();
        }

        public static void SetUnchecked()
        {
            _disable();
        }

        public static bool GetChecked()
        {
            return _getChecked();
        }

        private static void _enable()
        {
            RegistryKey ak = RegistryController.GetApplicationKey();
            ak.SetValue("PostUpdateCheck", 1);
            ak.Close();
        }

        private static void _disable()
        {
            RegistryKey ak = RegistryController.GetApplicationKey();
            ak.SetValue("PostUpdateCheck", 0);
            ak.Close();
        }

        private static bool _getChecked()
        {
            RegistryKey ak = RegistryController.GetApplicationKey();
            int uc = (int)ak.GetValue("PostUpdateCheck");
            return uc == 1;
        }
    }
}
