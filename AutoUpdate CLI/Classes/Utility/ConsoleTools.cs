using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.Utility
{
    internal class ConsoleTools
    {
        public static void WritePair(string key, string value)
        {
            ConsoleColor storedForeground = Console.ForegroundColor;
            ConsoleColor storedBackground = Console.BackgroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;

            Console.Write(key + ":");

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($" {value}");

            Console.ForegroundColor = storedForeground;
            Console.BackgroundColor = storedBackground;
        }
    }
}
