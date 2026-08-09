using System;
using System.Collections.Generic;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// GeeTest v4 solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/GeeTestTaskProxyless
    /// </summary>
    public class GeeTestV4Proxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The GeeTest v4 "captcha_id" value.</summary>
        public string WebsiteKey { get; set; }

        /// <summary>Custom GeeTest API subdomain, when the website uses one.</summary>
        public string GeetestApiServerSubdomain { get; set; }

        /// <summary>
        /// Captcha parameters, for example "riskType" => "slide".
        /// </summary>
        public Dictionary<string, string> InitParameters { get; } = new Dictionary<string, string>();

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "GeeTestTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["gt"] = WebsiteKey,
                ["version"] = 4
            };

            SetIfNotEmpty(postData, "geetestApiServerSubdomain", GeetestApiServerSubdomain);

            if (InitParameters.Count > 0)
            {
                postData["initParameters"] = JObject.FromObject(InitParameters);
            }

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
