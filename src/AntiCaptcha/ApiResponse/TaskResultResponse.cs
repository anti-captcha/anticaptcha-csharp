using System;
using System.Collections.Generic;
using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.ApiResponse
{
    /// <summary>
    /// Response of the getTaskResult API method.
    /// </summary>
    public class TaskResultResponse
    {
        public enum StatusType
        {
            Processing,
            Ready
        }

        public TaskResultResponse(JObject json)
        {
            ErrorId = JsonHelper.ExtractInt(json, "errorId");

            if (ErrorId == null)
            {
                ErrorDescription = "Unknown error";
                DebugHelper.Out(ErrorDescription, DebugHelper.Type.Error);

                return;
            }

            if (ErrorId != 0)
            {
                ErrorCode = JsonHelper.ExtractStr(json, "errorCode");
                ErrorDescription = JsonHelper.ExtractStr(json, "errorDescription") ?? "(no error description)";

                DebugHelper.Out(ErrorDescription, DebugHelper.Type.Error);

                return;
            }

            Status = ParseStatus(JsonHelper.ExtractStr(json, "status"));

            if (Status != StatusType.Ready)
            {
                return;
            }

            Cost = JsonHelper.ExtractDouble(json, "cost", null, true);
            Ip = JsonHelper.ExtractStr(json, "ip", null, true);
            SolveCount = JsonHelper.ExtractInt(json, "solveCount", null, true);
            CreateTime = UnixTimeStampToDateTime(JsonHelper.ExtractDouble(json, "createTime", null, true));
            EndTime = UnixTimeStampToDateTime(JsonHelper.ExtractDouble(json, "endTime", null, true));

            var solution = JsonHelper.ExtractObject(json, "solution");

            if (solution == null)
            {
                DebugHelper.Out("Got no 'solution' field from API", DebugHelper.Type.Error);

                return;
            }

            Solution = new SolutionData(solution);

            if (!Solution.IsEmpty)
            {
                return;
            }

            DebugHelper.Out("Got an empty 'solution' field from API", DebugHelper.Type.Error);
        }

        public int? ErrorId { get; }
        public string ErrorCode { get; }
        public string ErrorDescription { get; }
        public StatusType? Status { get; }
        public SolutionData Solution { get; }
        public double? Cost { get; }
        public string Ip { get; }

        /// <summary>
        /// Task create time in UTC.
        /// </summary>
        public DateTime? CreateTime { get; }

        /// <summary>
        /// Task end time in UTC.
        /// </summary>
        public DateTime? EndTime { get; }

        public int? SolveCount { get; }

        private static StatusType? ParseStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return null;
            }

            StatusType parsed;

            return Enum.TryParse(status, true, out parsed) ? parsed : (StatusType?)null;
        }

        private static DateTime? UnixTimeStampToDateTime(double? unixTimeStamp)
        {
            if (unixTimeStamp == null)
            {
                return null;
            }

            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp.Value);
        }

        /// <summary>
        /// The "solution" object of a completed task. Which properties are filled depends on
        /// the task type; see https://anti-captcha.com/apidoc for the per-type description.
        /// </summary>
        public class SolutionData
        {
            public SolutionData(JObject solution)
            {
                Raw = solution;

                Token = JsonHelper.ExtractStr(solution, "token", null, true);
                GRecaptchaResponse = JsonHelper.ExtractStr(solution, "gRecaptchaResponse", null, true);
                GRecaptchaResponseMd5 = JsonHelper.ExtractStr(solution, "gRecaptchaResponseMd5", null, true);
                RespKey = JsonHelper.ExtractStr(solution, "respKey", null, true);
                UserAgent = JsonHelper.ExtractStr(solution, "userAgent", null, true);
                Text = JsonHelper.ExtractStr(solution, "text", null, true);
                Url = JsonHelper.ExtractStr(solution, "url", null, true);
                Challenge = JsonHelper.ExtractStr(solution, "challenge", null, true);
                Seccode = JsonHelper.ExtractStr(solution, "seccode", null, true);
                Validate = JsonHelper.ExtractStr(solution, "validate", null, true);
                CaptchaId = JsonHelper.ExtractStr(solution, "captcha_id", null, true);
                LotNumber = JsonHelper.ExtractStr(solution, "lot_number", null, true);
                PassToken = JsonHelper.ExtractStr(solution, "pass_token", null, true);
                GenTime = JsonHelper.ExtractStr(solution, "gen_time", null, true);
                CaptchaOutput = JsonHelper.ExtractStr(solution, "captcha_output", null, true);
                Domain = JsonHelper.ExtractStr(solution, "domain", null, true);

                Cookies = JsonHelper.ExtractObject(solution, "cookies");
                LocalStorage = JsonHelper.ExtractObject(solution, "localStorage");
                Fingerprint = JsonHelper.ExtractObject(solution, "fingerprint");
                Answers = JsonHelper.ExtractObject(solution, "answers");
                Coordinates = JsonHelper.ExtractArray(solution, "coordinates");
                LastRequestHeaders = JsonHelper.ExtractArray(solution, "lastRequestHeaders");

                var cellNumbers = JsonHelper.ExtractArray(solution, "cellNumbers");

                if (cellNumbers != null)
                {
                    CellNumbers = cellNumbers.ToObject<List<int>>();
                }
            }

            /// <summary>
            /// The whole "solution" object, in case you need a field this class does not expose yet.
            /// </summary>
            public JObject Raw { get; }

            /// <summary>Recaptcha, hCaptcha.</summary>
            public string GRecaptchaResponse { get; }

            /// <summary>Recaptcha with isExtended=true.</summary>
            public string GRecaptchaResponseMd5 { get; }

            /// <summary>hCaptcha.</summary>
            public string RespKey { get; }

            /// <summary>User-agent of the worker who solved the captcha. Submit the form with it.</summary>
            public string UserAgent { get; }

            /// <summary>Image captcha.</summary>
            public string Text { get; }

            /// <summary>AntiGate and AntiBotCookie tasks.</summary>
            public string Url { get; }

            /// <summary>FunCaptcha, Turnstile, Prosopo, Friendly Captcha, Altcha, Amazon WAF.</summary>
            public string Token { get; }

            /// <summary>GeeTest v3.</summary>
            public string Challenge { get; }

            /// <summary>GeeTest v3.</summary>
            public string Seccode { get; }

            /// <summary>GeeTest v3.</summary>
            public string Validate { get; }

            /// <summary>GeeTest v4.</summary>
            public string CaptchaId { get; }

            /// <summary>GeeTest v4.</summary>
            public string LotNumber { get; }

            /// <summary>GeeTest v4.</summary>
            public string PassToken { get; }

            /// <summary>GeeTest v4.</summary>
            public string GenTime { get; }

            /// <summary>GeeTest v4.</summary>
            public string CaptchaOutput { get; }

            /// <summary>Amazon WAF: the domain the aws-waf-token cookie belongs to.</summary>
            public string Domain { get; }

            /// <summary>AntiGate and AntiBotCookie tasks.</summary>
            public JObject Cookies { get; }

            /// <summary>AntiGate and AntiBotCookie tasks.</summary>
            public JObject LocalStorage { get; }

            /// <summary>AntiGate and AntiBotCookie tasks.</summary>
            public JObject Fingerprint { get; }

            /// <summary>AntiBotCookie tasks: headers the worker's browser sent last.</summary>
            public JArray LastRequestHeaders { get; }

            /// <summary>Custom captcha tasks.</summary>
            public JObject Answers { get; }

            /// <summary>ImageToCoordinates tasks: a list of [x, y] or [x1, y1, x2, y2] boxes.</summary>
            public JArray Coordinates { get; }

            /// <summary>Square-net tasks.</summary>
            public List<int> CellNumbers { get; } = new List<int>();

            /// <summary>
            /// True when the API returned a "solution" object without any of the known result fields.
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    return GRecaptchaResponse == null && Text == null && Token == null && Challenge == null &&
                           Seccode == null && Validate == null && CaptchaId == null && Url == null &&
                           Answers == null && Coordinates == null && Cookies == null && LocalStorage == null &&
                           Fingerprint == null && CellNumbers.Count == 0;
                }
            }
        }
    }
}
