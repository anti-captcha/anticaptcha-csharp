using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Helper
{
    /// <summary>
    /// Tolerant readers for the API responses: a missing or malformed field never throws,
    /// it returns null and (unless silenced) reports the problem through <see cref="DebugHelper" />.
    /// </summary>
    public static class JsonHelper
    {
        public static string ExtractStr(JObject json, string firstLevel, string secondLevel = null, bool silent = false)
        {
            var token = Extract(json, firstLevel, secondLevel);

            if (token == null || token.Type == JTokenType.Null)
            {
                if (!silent)
                {
                    DebugHelper.JsonFieldParseError(Path(firstLevel, secondLevel), json);
                }

                return null;
            }

            return token.Type == JTokenType.String ? token.Value<string>() : token.ToString(Formatting.None);
        }

        public static double? ExtractDouble(JObject json, string firstLevel, string secondLevel = null,
            bool silent = false)
        {
            var numberAsStr = ExtractStr(json, firstLevel, secondLevel, silent);

            double result;

            if (numberAsStr == null || !double.TryParse(numberAsStr.Replace(",", "."), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out result))
            {
                if (!silent)
                {
                    DebugHelper.JsonFieldParseError(Path(firstLevel, secondLevel), json);
                }

                return null;
            }

            return result;
        }

        public static int? ExtractInt(JObject json, string firstLevel, string secondLevel = null, bool silent = false)
        {
            var numberAsStr = ExtractStr(json, firstLevel, secondLevel, silent);

            int result;

            if (numberAsStr == null || !int.TryParse(numberAsStr, NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out result))
            {
                if (!silent)
                {
                    DebugHelper.JsonFieldParseError(Path(firstLevel, secondLevel), json);
                }

                return null;
            }

            return result;
        }

        /// <summary>
        /// Returns a nested object as-is, or null when it is absent or is not an object.
        /// </summary>
        public static JObject ExtractObject(JObject json, string firstLevel, string secondLevel = null)
        {
            return Extract(json, firstLevel, secondLevel) as JObject;
        }

        /// <summary>
        /// Returns a nested array as-is, or null when it is absent or is not an array.
        /// </summary>
        public static JArray ExtractArray(JObject json, string firstLevel, string secondLevel = null)
        {
            return Extract(json, firstLevel, secondLevel) as JArray;
        }

        public static string AsString(JToken json)
        {
            return json == null ? "(null)" : json.ToString(Formatting.Indented);
        }

        private static JToken Extract(JObject json, string firstLevel, string secondLevel)
        {
            if (json == null)
            {
                return null;
            }

            var token = json[firstLevel];

            if (token == null || secondLevel == null)
            {
                return token;
            }

            var nested = token as JObject;

            return nested == null ? null : nested[secondLevel];
        }

        private static string Path(string firstLevel, string secondLevel)
        {
            return secondLevel == null ? firstLevel : firstLevel + "=>" + secondLevel;
        }
    }
}
