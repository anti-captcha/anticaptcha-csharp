using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;
using System;

namespace Anticaptcha_example.Api
{
    public class NoCaptchaProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string WebsiteSToken { protected get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "NoCaptchaTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey},
                {"websiteSToken", WebsiteSToken}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}