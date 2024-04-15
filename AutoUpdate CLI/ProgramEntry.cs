using AutoUpdate_CLI.Classes.Diagnostic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI
{
    public class ProgramEntry
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Program.Execute(args);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;

                Console.WriteLine("Program encountered an exception: " + e.GetType().Name);
                Console.WriteLine("Exception Message: " + e.Message);
                Console.WriteLine("Stack Trace: " + e.StackTrace);
                Console.WriteLine("--- BEGIN DIAGNOSTIC INFO ---");
                Console.WriteLine("At: " + DateTime.Now.ToLongTimeString());
                Console.WriteLine("Process Identity: " + WindowsIdentity.GetCurrent().Name);
                Console.WriteLine("Machine Name: " + Environment.MachineName);
                Console.WriteLine("Operating System: " + Environment.OSVersion.ToString());
                Console.WriteLine("Printing Disk Space Report...");
                DiskSpace.WriteReport();
                Console.WriteLine("\nPrinting memory and processing report...");
                Processing.PrintMemoryAndProcessing();

                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nTHIS IS AN ERROR! PLEASE REPORT IT TO BE FIXED.\n\nOpen an issue on the repository at https://github.com/larryr1/AutoUpdateCLI/issues or email larrywroweiii@gmail.com with this entire error log.");
                Console.WriteLine("Press enter to exit the program.");
                Console.ReadLine();
                Environment.Exit(1);
            }
        }
    }
}
