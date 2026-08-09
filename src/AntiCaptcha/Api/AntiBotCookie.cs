using System;
using AntiCaptcha.ApiResponse;
using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Makes a worker open the target page through your proxy and returns the anti-bot
    /// cookies, localStorage and browser fingerprint collected there, so you can reuse
    /// them in your own requests.
    /// https://anti-captcha.com/apidoc/task-types/AntiBotCookieTask
    /// </summary>
    public class AntiBotCookie : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page to visit.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>
        /// Proxy the worker browses through. Required: the cookies are only valid for the
        /// IP address they were issued to, so it has to be the same proxy you use later.
        /// </summary>
        public string ProxyAddress { get; set; }

        public int? ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }

        public override JObject GetPostData()
        {
            if (ProxyPort == null || ProxyPort < 1 || ProxyPort > 65535 || string.IsNullOrEmpty(ProxyAddress))
            {
                DebugHelper.Out("Proxy data is incorrect!", DebugHelper.Type.Error);

                return null;
            }

            // This task type takes no proxyType, only http proxies are supported.
            var postData = new JObject
            {
                ["type"] = "AntiBotCookieTask",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["proxyAddress"] = ProxyAddress,
                ["proxyPort"] = ProxyPort
            };

            SetIfNotEmpty(postData, "proxyLogin", ProxyLogin);
            SetIfNotEmpty(postData, "proxyPassword", ProxyPassword);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
