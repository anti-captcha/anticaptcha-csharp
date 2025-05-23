﻿using Anticaptcha_example.Helper;
using Newtonsoft.Json.Linq;

namespace Anticaptcha_example.Api
{
    public class Amazon : AmazonProxyless
    {
        public string ProxyLogin { protected get; set; }
        public string ProxyPassword { protected get; set; }
        public int? ProxyPort { protected get; set; }
        public ProxyTypeOption? ProxyType { protected get; set; }
        public string ProxyAddress { protected get; set; }

        public override JObject GetPostData()
        {
            var postData = base.GetPostData();
            postData["type"] = "AmazonTask";

            if (ProxyType == null || ProxyPort == null || ProxyPort < 1 || ProxyPort > 65535 ||
                string.IsNullOrEmpty(ProxyAddress))
            {
                DebugHelper.Out("Proxy data is incorrect!", DebugHelper.Type.Error);

                return null;
            }

            postData.Add("proxyType", ProxyType.ToString().ToLower());
            postData.Add("proxyAddress", ProxyAddress);
            postData.Add("proxyPort", ProxyPort);
            postData.Add("proxyLogin", ProxyLogin);
            postData.Add("proxyPassword", ProxyPassword);

            return postData;
        }
    }
}