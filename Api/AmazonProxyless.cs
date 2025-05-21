using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;
using System;

namespace Anticaptcha_example.Api
{
    public class AmazonProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string Iv { protected get; set; }
        public string Context { protected get; set; }
        public string CaptchaScript { protected get; set; }
        public string ChallengeScript { protected get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "AmazonTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey},
                {"iv", Iv},
                {"context", Context},
                {"captchaScript", CaptchaScript},
                {"challengeScript", ChallengeScript}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}