﻿using Anticaptcha_example.ApiResponse;
using Newtonsoft.Json.Linq;
using System;

namespace Anticaptcha_example.Api
{
    public class TurnstileProxyless : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string Action { protected get; set; }
        public string CData { protected get; set; }
        public string ChlPageData { protected get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "TurnstileTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey},
                {"action", Action},
                {"cData", CData},
                {"chlPageData", ChlPageData}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}