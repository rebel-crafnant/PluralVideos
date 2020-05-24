using System;

namespace DecryptVideos.Option
{
    public class Utils
    {
        private static readonly ConsoleColor color_default;
        private static readonly object console_lock = new object();

        static Utils()
        {
            color_default = Console.ForegroundColor;
        }

        public static void WriteText(string text)
        {
            WriteToConsole(text);
        }

        public static void WriteRedText(string text)
        {
            WriteToConsole(text, ConsoleColor.Red);
        }

        public static void WriteYellowText(string text)
        {
            WriteToConsole(text, ConsoleColor.Yellow);
        }

        public static void WriteGreenText(string text)
        {
            WriteToConsole(text, ConsoleColor.Green);
        }

        public static void WriteBlueText(string text)
        {
            WriteToConsole(text, ConsoleColor.Blue);
        }

        public static void WriteCyanText(string text)
        {
            WriteToConsole(text, ConsoleColor.Cyan);
        }

        private static void WriteToConsole(string Text, ConsoleColor color = ConsoleColor.Gray)
        {
            lock (console_lock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(Text);
                Console.ForegroundColor = color_default;
            }
        }
    }
}