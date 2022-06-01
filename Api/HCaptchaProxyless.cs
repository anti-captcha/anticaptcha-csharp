using System;
using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Anticaptcha_example.Api
{
    public class HCaptchaProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string UserAgent { protected get; set; }
        public bool IsInvisible { protected get; set; }
        public Dictionary<string, string> EnterprisePayload = new Dictionary<string, string>();

        public override JObject GetPostData()
        {
            var jsonObject = new JObject
            {
                {"type", "HCaptchaTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey},
                {"userAgent", UserAgent},
                {"isInvisible", IsInvisible}
            };

            if (EnterprisePayload.Count > 0)
            {
                jsonObject["enterprisePayload"] = JObject.FromObject(EnterprisePayload);
            }

            return jsonObject;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}