using System;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Helper
{
    /// <summary>
    /// Diagnostic output of the library. Disabled by default.
    /// </summary>
    public static class DebugHelper
    {
        public enum Type
        {
            Error,
            Info,
            Success
        }

        /// <summary>
        /// Set to true to print what the library is doing. Off by default.
        /// </summary>
        public static bool VerboseMode { get; set; }

        /// <summary>
        /// Redirects the output to your own logger. When null, messages go to the console.
        /// </summary>
        public static Action<string, Type> Sink { get; set; }

        public static void JsonFieldParseError(string field, JToken rawResponse)
        {
            Out(field + " could not be parsed. Raw response: " + JsonHelper.AsString(rawResponse), Type.Error);
        }

        public static void Out(string message, Type type = Type.Info)
        {
            if (Sink != null)
            {
                Sink(message, type);

                return;
            }

            if (!VerboseMode)
            {
                return;
            }

            switch (type)
            {
                case Type.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Type.Info:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Type.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
