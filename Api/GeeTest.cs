using System;
using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;

namespace Anticaptcha_example.Api
{
    public class GeeTest : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string WebsiteChallenge { protected get; set; }
        public string GeetestApiServerSubdomain { protected get; set; }
        public string ProxyLogin { protected get; set; }
        public string ProxyPassword { protected get; set; }
        public int? ProxyPort { protected get; set; }
        public ProxyTypeOption? ProxyType { protected get; set; }
        public string ProxyAddress { protected get; set; }
        public string UserAgent { protected get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                {"type", "GeeTestTask"},
                {"websiteURL", WebsiteUrl},
                {"gt", WebsiteKey},
                {"challenge", WebsiteChallenge},
                {"proxyType", ProxyType.ToString().ToLower()},
                {"proxyAddress", ProxyAddress},
                {"proxyPort", ProxyPort},
                {"proxyLogin", ProxyLogin},
                {"proxyPassword", ProxyPassword},
                {"userAgent", UserAgent}
            };

            if (!string.IsNullOrEmpty(GeetestApiServerSubdomain))
            {
                postData["geetestApiServerSubdomain"] = GeetestApiServerSubdomain;
            }

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}