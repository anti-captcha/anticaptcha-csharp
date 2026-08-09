using System;
using AntiCaptcha.ApiResponse;
using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Recaptcha V3. This captcha has no proxy-on version.
    /// https://anti-captcha.com/apidoc/task-types/RecaptchaV3TaskProxyless
    /// </summary>
    public class RecaptchaV3Proxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        private double _minScore = 0.3;

        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The Recaptcha site key.</summary>
        public string WebsiteKey { get; set; }

        /// <summary>Widget action value, the "action" property of the grecaptcha.execute call.</summary>
        public string PageAction { get; set; }

        /// <summary>Set to true when solving a Recaptcha V3 Enterprise.</summary>
        public bool IsEnterprise { get; set; }

        /// <summary>
        /// Domain serving the Recaptcha script when it is not google.com, for example
        /// "recaptcha.net".
        /// </summary>
        public string ApiDomain { get; set; }

        /// <summary>
        /// Score you need, one of 0.3, 0.7 or 0.9. Defaults to 0.3.
        /// </summary>
        public double MinScore
        {
            get { return _minScore; }
            set
            {
                if (value != 0.3 && value != 0.7 && value != 0.9)
                {
                    DebugHelper.Out(
                        "minScore must be one of these: 0.3, 0.7, 0.9; you passed " + value + "; 0.3 will be set",
                        DebugHelper.Type.Error
                    );

                    return;
                }

                _minScore = value;
            }
        }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "RecaptchaV3TaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["websiteKey"] = WebsiteKey,
                ["minScore"] = MinScore,
                ["isEnterprise"] = IsEnterprise
            };

            SetIfNotEmpty(postData, "pageAction", PageAction);
            SetIfNotEmpty(postData, "apiDomain", ApiDomain);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
