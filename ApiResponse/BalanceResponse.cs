using Anticaptcha_example.Helper;

namespace Anticaptcha_example.ApiResponse
{
    public class BalanceResponse
    {
        public BalanceResponse(dynamic json)
        {
            ErrorId = JsonHelper.ExtractInt(json, "errorId");
            ErrorCode = JsonHelper.ExtractStr(json, "errorCode");
            ErrorDescription = JsonHelper.ExtractStr(json, "errorDescription");

            if (ErrorId.Equals(0))
            {
                Balance = JsonHelper.ExtractDouble(json, "balance");
            }
        }

        public int? ErrorId { get; private set; }
        public string ErrorCode { get; private set; }
        public string ErrorDescription { get; private set; }
        public double? Balance { get; private set; }
    }
}