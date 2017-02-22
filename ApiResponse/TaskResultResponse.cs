using System;
using System.Globalization;
using Anticaptcha_example.Helper;

namespace Anticaptcha_example.ApiResponse
{
    public class TaskResultResponse
    {
        public enum StatusType
        {
            Processing,
            Ready
        }

        public TaskResultResponse(dynamic json)
        {
            ErrorId = JsonHelper.ExtractInt(json, "errorId");

            if (ErrorId != null)
            {
                if (ErrorId.Equals(0))
                {
                    Status = ParseStatus(JsonHelper.ExtractStr(json, "status"));

                    if (Status.Equals(StatusType.Ready))
                    {
                        Cost = JsonHelper.ExtractDouble(json, "cost");
                        Ip = JsonHelper.ExtractStr(json, "ip");
                        SolveCount = JsonHelper.ExtractInt(json, "solveCount");
                        CreateTime = UnixTimeStampToDateTime(JsonHelper.ExtractDouble(json, "createTime"));
                        EndTime = UnixTimeStampToDateTime(JsonHelper.ExtractDouble(json, "endTime"));

                        Solution = new SolutionData
                        {
                            GRecaptchaResponse =
                                JsonHelper.ExtractStr(json, "solution", "gRecaptchaResponse", silent: true),
                            GRecaptchaResponseMd5 =
                                JsonHelper.ExtractStr(json, "solution", "gRecaptchaResponseMd5", silent: true),
                            Text = JsonHelper.ExtractStr(json, "solution", "text", silent: true),
                            Url = JsonHelper.ExtractStr(json, "solution", "url", silent: true)
                        };

                        if (Solution.GRecaptchaResponse == null && Solution.Text == null)
                        {
                            DebugHelper.Out("Got no 'solution' field from API", DebugHelper.Type.Error);
                        }
                    }
                }
                else
                {
                    ErrorCode = JsonHelper.ExtractStr(json, "errorCode");
                    ErrorDescription = JsonHelper.ExtractStr(json, "errorDescription") ?? "(no error description)";

                    DebugHelper.Out(ErrorDescription, DebugHelper.Type.Error);
                }
            }
            else
            {
                DebugHelper.Out("Unknown error", DebugHelper.Type.Error);
            }
        }

        public int? ErrorId { get; private set; }
        public string ErrorCode { get; private set; }
        public string ErrorDescription { get; private set; }
        public StatusType? Status { get; private set; }
        public SolutionData Solution { get; private set; }
        public double? Cost { get; private set; }
        public string Ip { get; private set; }

        /// <summary>
        ///     Task create time in UTC
        /// </summary>
        public DateTime? CreateTime { get; private set; }

        /// <summary>
        ///     Task end time in UTC
        /// </summary>
        public DateTime? EndTime { get; private set; }

        public int? SolveCount { get; private set; }

        private StatusType? ParseStatus(string status)
        {
            if (String.IsNullOrEmpty(status))
            {
                return null;
            }

            try
            {
                return (StatusType) Enum.Parse(
                    typeof (StatusType),
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(status),
                    true
                    );
            }
            catch
            {
                return null;
            }
        }

        private static DateTime? UnixTimeStampToDateTime(double? unixTimeStamp)
        {
            if (unixTimeStamp == null)
            {
                return null;
            }

            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return dtDateTime.AddSeconds((double) unixTimeStamp).ToUniversalTime();
        }

        public class SolutionData
        {
            public string GRecaptchaResponse { get; internal set; } // Will be available for Recaptcha tasks only!
            public string GRecaptchaResponseMd5 { get; internal set; } // for Recaptcha with isExtended=true property
            public string Text { get; internal set; } // Will be available for ImageToText tasks only!
            public string Url { get; internal set; }
        }
    }
}