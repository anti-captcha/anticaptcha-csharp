using System;
using AntiCaptcha.ApiResponse;
using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Altcha solved from our workers' IP addresses.
    /// https://anti-captcha.com/apidoc/task-types/AltchaTaskProxyless
    /// </summary>
    public class AltchaProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page where the captcha is.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>
        /// URL the challenge is fetched from. Provide this or <see cref="ChallengeJson" />.
        /// </summary>
        public string ChallengeUrl { get; set; }

        /// <summary>
        /// The challenge itself as a JSON string. Provide this or <see cref="ChallengeUrl" />.
        /// </summary>
        public string ChallengeJson { get; set; }

        public override JObject GetPostData()
        {
            if (string.IsNullOrEmpty(ChallengeUrl) && string.IsNullOrEmpty(ChallengeJson))
            {
                DebugHelper.Out("Set either ChallengeUrl or ChallengeJson", DebugHelper.Type.Error);

                return null;
            }

            var postData = new JObject
            {
                ["type"] = "AltchaTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString()
            };

            SetIfNotEmpty(postData, "challengeURL", ChallengeUrl);
            SetIfNotEmpty(postData, "challengeJSON", ChallengeJson);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
