using System;
using System.Globalization;

namespace Anticaptcha_example.Helper
{
    public class JsonHelper
    {
        public static string ExtractStr(dynamic json, string firstLevel, string secondLevel = null)
        {
            try
            {
                object result = json[firstLevel];

                if (result != null && secondLevel != null)
                {
                    result = json[firstLevel][secondLevel];
                }

                if (result == null)
                {
                    return null;
                }

                return result.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static double? ExtractDouble(dynamic json, string firstLevel, string secondLevel = null)
        {
            double outDouble;
            string numberAsStr = ExtractStr(json, firstLevel, secondLevel);

            if (numberAsStr == null ||
                !double.TryParse(numberAsStr.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture,
                    out outDouble))
            {
                var path = firstLevel + (secondLevel == null ? "" : "=>" + secondLevel);
                Error(path, json);

                return null;
            }

            return outDouble;
        }

        public static int? ExtractInt(dynamic json, string firstLevel, string secondLevel = null)
        {
            int outInt;
            string numberAsStr = JsonHelper.ExtractStr(json, firstLevel, secondLevel);

            if (!int.TryParse(numberAsStr, out outInt))
            {
                var path = firstLevel + (secondLevel == null ? "" : "=>" + secondLevel);
                Error(path, json);

                return null;
            }

            return outInt;
        }

        public static void Error(string field, dynamic submitResult, Exception exception = null)
        {
            string error = field + " could not be parsed. Raw response: " + submitResult.ToString();

            if (exception != null)
            {
                error += "; Exception message: " + exception.Message;
            }

            DebugHelper.Out(error, DebugHelper.Type.Error);
        }
    }
}