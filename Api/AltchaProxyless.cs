using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;
using System;

namespace Anticaptcha_example.Api
{
    public class AltchaProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string ChallengeURL { protected get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "AltchaTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"challengeURL", ChallengeURL}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}