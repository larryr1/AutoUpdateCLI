using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.SystemAbstract.RegistryAbstract
{
    internal class AutoRun
    {
        public static void SetExecutableRunOnceKey()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true);
            key.SetValue("AACLI", $"\"{System.Reflection.Assembly.GetExecutingAssembly().Location}\"");
        }
    }
}
