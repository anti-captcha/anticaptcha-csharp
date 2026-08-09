using System;
using System.IO;

namespace AntiCaptcha.Helper
{
    public static class StringHelper
    {
        /// <summary>
        /// Reads an image file and returns its base64 representation, or null when the file
        /// cannot be read.
        /// </summary>
        public static string FileToBase64String(string path)
        {
            try
            {
                return Convert.ToBase64String(File.ReadAllBytes(path));
            }
            catch (Exception ex)
            {
                DebugHelper.Out("Could not read " + path + ": " + ex.Message, DebugHelper.Type.Error);

                return null;
            }
        }
    }
}
