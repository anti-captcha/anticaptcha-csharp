using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Recaptcha V2 solved through your own proxy.
    /// https://anti-captcha.com/apidoc/task-types/RecaptchaV2Task
    /// </summary>
    public class RecaptchaV2 : RecaptchaV2Proxyless
    {
        public ProxyTypeOption? ProxyType { get; set; }
        public string ProxyAddress { get; set; }
        public int? ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }

        /// <summary>Browser user-agent the proxy is used with.</summary>
        public string UserAgent { get; set; }

        /// <summary>Cookies of the target page, in "name1=value1; name2=value2" format.</summary>
        public string Cookies { get; set; }

        public override JObject GetPostData()
        {
            var postData = base.GetPostData();

            if (postData == null)
            {
                return null;
            }

            postData["type"] = "RecaptchaV2Task";

            if (!AddProxyData(postData, ProxyType, ProxyAddress, ProxyPort, ProxyLogin, ProxyPassword))
            {
                return null;
            }

            SetIfNotEmpty(postData, "userAgent", UserAgent);
            SetIfNotEmpty(postData, "cookies", Cookies);

            return postData;
        }
    }
}
