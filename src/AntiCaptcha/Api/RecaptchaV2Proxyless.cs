using System;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Recaptcha V2 solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/RecaptchaV2TaskProxyless
    /// </summary>
    public class RecaptchaV2Proxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The "data-sitekey" value of the captcha element.</summary>
        public string WebsiteKey { get; set; }

        public string WebsiteSToken { get; set; }

        /// <summary>Set to true when solving an invisible Recaptcha V2.</summary>
        public bool IsInvisible { get; set; }

        /// <summary>The "data-s" parameter, usually found on google.com websites.</summary>
        public string DataSValue { get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "RecaptchaV2TaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["websiteKey"] = WebsiteKey,
                ["isInvisible"] = IsInvisible
            };

            SetIfNotEmpty(postData, "websiteSToken", WebsiteSToken);
            SetIfNotEmpty(postData, "recaptchaDataSValue", DataSValue);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
