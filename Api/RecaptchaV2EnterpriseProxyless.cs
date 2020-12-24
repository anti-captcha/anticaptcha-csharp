using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;
using System;

namespace Anticaptcha_example.Api
{
    public class RecaptchaV2EnterpriseProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string EnterprisePayload { protected get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "RecaptchaV2EnterpriseTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey},
                {"enterprisePayload", EnterprisePayload}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}