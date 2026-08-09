using System;
using System.Collections.Generic;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Recaptcha V2 Enterprise solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/RecaptchaV2EnterpriseTaskProxyless
    /// </summary>
    public class RecaptchaV2EnterpriseProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The "data-sitekey" value of the captcha element.</summary>
        public string WebsiteKey { get; set; }

        /// <summary>
        /// Additional parameters passed to the grecaptcha.enterprise.render call, like "s".
        /// </summary>
        public Dictionary<string, string> EnterprisePayload { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Domain serving the Recaptcha script when it is not google.com, for example
        /// "recaptcha.net".
        /// </summary>
        public string ApiDomain { get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "RecaptchaV2EnterpriseTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["websiteKey"] = WebsiteKey
            };

            if (EnterprisePayload.Count > 0)
            {
                postData["enterprisePayload"] = JObject.FromObject(EnterprisePayload);
            }

            SetIfNotEmpty(postData, "apiDomain", ApiDomain);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
