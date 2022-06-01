using System;
using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Anticaptcha_example.Api
{
    public class GeeTestV4Proxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string GeetestApiServerSubdomain { protected get; set; }
        public Dictionary<string, string> initParameters = new Dictionary<string, string>();

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                {"type", "GeeTestTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"gt", WebsiteKey},
                {"version", 4},
            };

            if (!string.IsNullOrEmpty(GeetestApiServerSubdomain))
            {
                postData["geetestApiServerSubdomain"] = GeetestApiServerSubdomain;
            }
            if (initParameters.Count > 0)
            {
                postData["initParameters"] = JObject.FromObject(initParameters);
            }

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}