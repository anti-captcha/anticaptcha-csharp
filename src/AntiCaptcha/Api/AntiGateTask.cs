using System;
using System.Collections.Generic;
using AntiCaptcha.ApiResponse;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// A custom scenario executed by a worker in a browser, described by a template you
    /// pick or create in the AntiGate templates directory.
    /// https://anti-captcha.com/apidoc/task-types/AntiGateTask
    /// </summary>
    public class AntiGateTask : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>Address of the page the worker is sent to.</summary>
        public Uri WebsiteUrl { get; set; }

        /// <summary>
        /// Name of the template from https://anti-captcha.com/apidoc/antigate-templates
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>Values of the template's variables.</summary>
        public JObject Variables { get; set; }

        /// <summary>Domains you want the cookies and localStorage of.</summary>
        public List<string> DomainsOfInterest { get; } = new List<string>();

        public ProxyTypeOption ProxyType { get; set; } = ProxyTypeOption.Http;
        public string ProxyAddress { get; set; }
        public int? ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                ["type"] = "AntiGateTask",
                ["websiteURL"] = WebsiteUrl?.ToString(),
                ["templateName"] = TemplateName
            };

            if (Variables != null)
            {
                postData["variables"] = Variables;
            }

            if (DomainsOfInterest.Count > 0)
            {
                postData["domainsOfInterest"] = JArray.FromObject(DomainsOfInterest);
            }

            // The proxy is optional for AntiGate tasks, but once an address is given the
            // whole set has to be valid.
            if (!string.IsNullOrEmpty(ProxyAddress) &&
                !AddProxyData(postData, ProxyType, ProxyAddress, ProxyPort, ProxyLogin, ProxyPassword))
            {
                return null;
            }

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
