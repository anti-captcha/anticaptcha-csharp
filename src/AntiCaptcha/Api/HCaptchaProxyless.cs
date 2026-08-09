using System;
using System.Collections.Generic;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// hCaptcha solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/HCaptchaTaskProxyless
    /// </summary>
    public class HCaptchaProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The "data-sitekey" value of the captcha element.</summary>
        public string WebsiteKey { get; set; }

        /// <summary>Set to true when solving an invisible hCaptcha.</summary>
        public bool IsInvisible { get; set; }

        /// <summary>Set to true when solving an hCaptcha Enterprise.</summary>
        public bool IsEnterprise { get; set; }

        /// <summary>
        /// hCaptcha Enterprise parameters such as rqdata, sentry, apiEndpoint, endpoint,
        /// reportapi, assethost and imghost.
        /// </summary>
        public Dictionary<string, string> EnterprisePayload { get; } = new Dictionary<string, string>();

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "HCaptchaTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["websiteKey"] = WebsiteKey,
                ["isInvisible"] = IsInvisible,
                ["isEnterprise"] = IsEnterprise
            };

            if (EnterprisePayload.Count > 0)
            {
                postData["enterprisePayload"] = JObject.FromObject(EnterprisePayload);
            }

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
