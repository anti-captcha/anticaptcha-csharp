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
        /// URL the challenge is fetched from. Provide this or <see cref="ChallengeJSON" />.
        /// </summary>
        public string ChallengeURL { get; set; }

        /// <summary>
        /// The challenge itself as a JSON string. Provide this or <see cref="ChallengeURL" />.
        /// </summary>
        public string ChallengeJSON { get; set; }

        public override JObject GetPostData()
        {
            if (string.IsNullOrEmpty(ChallengeURL) && string.IsNullOrEmpty(ChallengeJSON))
            {
                DebugHelper.Out("Set either ChallengeURL or ChallengeJSON", DebugHelper.Type.Error);

                return null;
            }

            var postData = new JObject
            {
                ["type"] = "AltchaTaskProxyless",
                ["websiteURL"] = WebsiteUrl?.ToString()
            };

            SetIfNotEmpty(postData, "challengeURL", ChallengeURL);
            SetIfNotEmpty(postData, "challengeJSON", ChallengeJSON);

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
