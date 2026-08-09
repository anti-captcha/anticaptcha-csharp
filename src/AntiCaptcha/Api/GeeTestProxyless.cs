using System;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// GeeTest v3 solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/GeeTestTaskProxyless
    /// </summary>
    public class GeeTestProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The GeeTest "gt" key.</summary>
        public string WebsiteKey { get; set; }

        /// <summary>
        /// The "challenge" value. It is one-time, so grab a fresh one for every task.
        /// </summary>
        public string WebsiteChallenge { get; set; }

        /// <summary>Custom GeeTest API subdomain, when the website uses one.</summary>
        public string GeetestApiServerSubdomain { get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "GeeTestTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["gt"] = WebsiteKey,
                ["version"] = 3
            };

            SetIfNotEmpty(postData, "challenge", WebsiteChallenge);
            SetIfNotEmpty(postData, "geetestApiServerSubdomain", GeetestApiServerSubdomain);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
