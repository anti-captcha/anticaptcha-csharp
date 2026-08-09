using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Prosopo Procaptcha solved through your own proxy.
    /// https://anti-captcha.com/apidoc/task-types/ProsopoTask
    /// </summary>
    public class Prosopo : ProsopoProxyless
    {
        public ProxyTypeOption ProxyType { get; set; } = ProxyTypeOption.Http;
        public string ProxyAddress { get; set; }
        public int? ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }

        /// <summary>Browser user-agent the proxy is used with.</summary>
        public string UserAgent { get; set; }

        public override JObject GetPostData()
        {
            var postData = base.GetPostData();

            if (postData == null)
            {
                return null;
            }

            postData["type"] = "ProsopoTask";

            if (!AddProxyData(postData, ProxyType, ProxyAddress, ProxyPort, ProxyLogin, ProxyPassword))
            {
                return null;
            }

            SetIfNotEmpty(postData, "userAgent", UserAgent);

            return postData;
        }
    }
}
