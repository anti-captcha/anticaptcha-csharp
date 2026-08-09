using System;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Amazon WAF captcha solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/AmazonTaskProxyless
    /// </summary>
    public class AmazonProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>
        /// The "key" value from the window.gokuProps object, or the widget API key from the
        /// AwsWafCaptcha.renderCaptcha call when <see cref="WafType" /> is "widget".
        /// </summary>
        public string WebsiteKey { get; set; }

        /// <summary>The "iv" value from the window.gokuProps object.</summary>
        public string Iv { get; set; }

        /// <summary>The "context" value from the window.gokuProps object.</summary>
        public string Context { get; set; }

        /// <summary>Optional URL of the captcha.js script.</summary>
        public string CaptchaScript { get; set; }

        /// <summary>Optional URL of the challenge.js script.</summary>
        public string ChallengeScript { get; set; }

        /// <summary>Full URL of jsapi.js, required when <see cref="WafType" /> is "widget".</summary>
        public string JsapiScript { get; set; }

        /// <summary>
        /// Set to "widget" when the captcha is a standalone widget triggered by a user action.
        /// Leave empty for the bot filtering page which returns an aws-waf-token cookie.
        /// </summary>
        public string WafType { get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "AmazonTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["websiteKey"] = WebsiteKey
            };

            SetIfNotEmpty(postData, "wafType", WafType);
            SetIfNotEmpty(postData, "iv", Iv);
            SetIfNotEmpty(postData, "context", Context);
            SetIfNotEmpty(postData, "captchaScript", CaptchaScript);
            SetIfNotEmpty(postData, "challengeScript", ChallengeScript);
            SetIfNotEmpty(postData, "jsapiScript", JsapiScript);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
