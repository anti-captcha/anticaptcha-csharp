using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.ApiResponse
{
    /// <summary>
    /// Response of the getBalance API method.
    /// </summary>
    public class BalanceResponse
    {
        public BalanceResponse(JObject json)
        {
            ErrorId = JsonHelper.ExtractInt(json, "errorId");

            if (ErrorId == null)
            {
                ErrorDescription = "Unknown error";
                DebugHelper.Out(ErrorDescription, DebugHelper.Type.Error);

                return;
            }

            if (ErrorId == 0)
            {
                Balance = JsonHelper.ExtractDouble(json, "balance");
                CaptchaCredits = JsonHelper.ExtractDouble(json, "captchaCredits", null, true) ?? 0;
            }
            else
            {
                ErrorCode = JsonHelper.ExtractStr(json, "errorCode");
                ErrorDescription = JsonHelper.ExtractStr(json, "errorDescription") ?? "(no error description)";
            }
        }

        public int? ErrorId { get; }
        public string ErrorCode { get; }
        public string ErrorDescription { get; }

        /// <summary>
        /// Account balance in US dollars.
        /// </summary>
        public double? Balance { get; }

        /// <summary>
        /// Number of prepaid captcha credits on the account.
        /// </summary>
        public double CaptchaCredits { get; }
    }
}
