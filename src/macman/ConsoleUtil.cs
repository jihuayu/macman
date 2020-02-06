using System;

namespace macman
{
    public class ConsoleUtil
    {
        public static void Error(Object o)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(o);
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        public static void Yellow(Object o)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(o);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Green(Object o)
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