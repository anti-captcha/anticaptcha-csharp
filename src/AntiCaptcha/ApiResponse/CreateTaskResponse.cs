using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.ApiResponse
{
    /// <summary>
    /// Response of the createTask API method.
    /// </summary>
    public class CreateTaskResponse
    {
        public CreateTaskResponse(JObject json)
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
                TaskId = JsonHelper.ExtractInt(json, "taskId");
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
        public int? TaskId { get; }
    }
}
