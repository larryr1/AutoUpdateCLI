using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.Diagnostic
{
    public class Processing
    {
        public static void PrintMemoryAndProcessing()
        {
            using (ManagementObjectSearcher win32Proc = new ManagementObjectSearcher("select * from Win32_Processor"))
            {
                foreach (ManagementObject obj in win32Proc.Get())
                {
                    string clockSpeed = obj["CurrentClockSpeed"].ToString();
                    string procName = obj["Name"].ToString();

                    Console.WriteLine($"Processor: {procName} at {clockSpeed}MHz");
                }
            }



            Process proc = Process.GetCurrentProcess();
            Console.WriteLine($"Logical Processor Core Count: {Environment.ProcessorCount} cores");
            Console.WriteLine("Installed Memory: [UNKNOWN, NOT IMPLEMENTED]");
            Console.WriteLine($"Allocated Process Memory: {proc.PrivateMemorySize64} bytes");
        }
    }
}
