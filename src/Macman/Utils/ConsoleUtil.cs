using System;

namespace Macman.Utils
{
    public static class ConsoleUtil
    {
        public static void Error(object o)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(o);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Yellow(object o)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(o);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Green(object o)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(o);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void NewLine()
        {
            Console.WriteLine();
        }
    }
}