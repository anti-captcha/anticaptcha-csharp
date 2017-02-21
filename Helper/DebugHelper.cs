using System;

namespace Anticaptcha_example.Helper
{
    public class DebugHelper
    {
        public enum Type
        {
            Error,
            Info,
            Good
        }

        public static bool VerboseMode { set; private get; }

        public static void Out(string message, Type? severity = null)
        {
            if (!VerboseMode)
            {
                return;
            }

            if (severity.Equals(Type.Error))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (severity.Equals(Type.Info))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (severity.Equals(Type.Good))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}