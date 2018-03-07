using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;

namespace Anticaptcha_example.Api
{
    internal class CustomCaptcha : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public string ImageUrl { protected get; set; }
        public string Assignment { protected get; set; }
        public JArray Forms { get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "CustomCaptchaTask"},
                {"imageUrl", ImageUrl},
                {"assignment", Assignment},
                {"forms", Forms}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}