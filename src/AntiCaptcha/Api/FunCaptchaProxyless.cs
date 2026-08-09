using System;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// FunCaptcha (Arkose Labs) solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/FunCaptchaTaskProxyless
    /// </summary>
    public class FunCaptchaProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>The "data-pkey" value of the FunCaptcha element.</summary>
        public string WebsitePublicKey { get; set; }

        /// <summary>
        /// Custom Arkose Labs subdomain, found in URLs like
        /// https://somewebsite-api.arkoselabs.com/v2/00000000-1111-2222-3333-444444444444/api.js
        /// </summary>
        public string ApiJSSubdomain { get; set; }

        /// <summary>The "blob" value from the Arkose Labs configuration, as a JSON string.</summary>
        public string DataBlob { get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "FunCaptchaTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["websitePublicKey"] = WebsitePublicKey
            };

            SetIfNotEmpty(postData, "funcaptchaApiJSSubdomain", ApiJSSubdomain);
            SetIfNotEmpty(postData, "data", DataBlob);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
