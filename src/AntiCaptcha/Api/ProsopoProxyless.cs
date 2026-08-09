using System;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Prosopo Procaptcha solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/ProsopoTaskProxyless
    /// </summary>
    public class ProsopoProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The Prosopo sitekey.</summary>
        public string WebsiteKey { get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                ["type"] = "ProsopoTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["websiteKey"] = WebsiteKey
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
