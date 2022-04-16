using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;
using System;

namespace Anticaptcha_example.Api
{
    public class RecaptchaV2Proxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string WebsiteSToken { protected get; set; }
        public bool IsInvisible { protected get; set; }
        public string DataSValue { protected get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "RecaptchaV2TaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey},
                {"websiteSToken", WebsiteSToken},
                {"recaptchaDataSValue", DataSValue},
                {"isInvisible", IsInvisible}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}