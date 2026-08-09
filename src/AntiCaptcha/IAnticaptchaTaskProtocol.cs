using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha
{
    /// <summary>
    /// Contract implemented by every captcha task type.
    /// </summary>
    public interface IAnticaptchaTaskProtocol
    {
        /// <summary>
        /// Builds the "task" object sent to the createTask API method.
        /// Returns null when the task is misconfigured.
        /// </summary>
        JObject GetPostData();

        /// <summary>
        /// Solution of the completed task. Available after a successful WaitForResult() call.
        /// </summary>
        TaskResultResponse.SolutionData GetTaskSolution();
    }
}
