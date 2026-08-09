using System;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Cloudflare Turnstile solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/TurnstileTaskProxyless
    /// </summary>
    public class TurnstileProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The Turnstile sitekey, for example "0x4AAAAAAABD2Inoxs-yJ8bz".</summary>
        public string WebsiteKey { get; set; }

        /// <summary>Optional widget action.</summary>
        public string Action { get; set; }

        /// <summary>The "cData" token, for Cloudflare Challenge pages.</summary>
        public string CData { get; set; }

        /// <summary>The "chlPageData" token, for Cloudflare Challenge pages.</summary>
        public string ChlPageData { get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "TurnstileTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["websiteKey"] = WebsiteKey
            };

            SetIfNotEmpty(postData, "action", Action);
            SetIfNotEmpty(postData, "cData", CData);
            SetIfNotEmpty(postData, "chlPageData", ChlPageData);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
