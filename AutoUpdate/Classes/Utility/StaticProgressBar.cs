using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.Utility
{
    internal static class StaticProgressBar
    {
        public static void Draw(double percent, int chars)
        {
            double value = Math.Floor((percent * 100) / chars);
            Console.Write("[");
            for (int i = 0; i < value; i++)
            {
                Console.Write("|");
                chars -= 1;
            }
            for (int i = 0; i < value; i++)
            {
                Console.Write(" ");
            }
            Console.Write("]");

        }
    }
}
