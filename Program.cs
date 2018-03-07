using System;
using Anticaptcha_example.Api;
using Anticaptcha_example.Helper;
using Newtonsoft.Json.Linq;

namespace Anticaptcha_example
{
    internal class Program
    {
        private static void Main()
        {
            ExampleGetBalance();
            ExampleImageToText();
            ExampleNoCaptchaProxyless();
            ExampleNoCaptcha();
            ExampleCustomCaptcha();
            ExampleFunCaptcha();

            Console.ReadKey();
        }

        private static void ExampleGetBalance()
        {
            DebugHelper.VerboseMode = true;

            var api = new ImageToText
            {
                ClientKey = "1234567890123456789012"
            };

            var balance = api.GetBalance();

            if (balance == null)
                DebugHelper.Out("GetBalance() failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else
                DebugHelper.Out("Balance: " + balance, DebugHelper.Type.Success);
        }

        private static void ExampleImageToText()
        {
            DebugHelper.VerboseMode = true;

            var api = new ImageToText
            {
                ClientKey = "1234567890123456789012",
                FilePath = "captcha.jpg"
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
                DebugHelper.Out("Result: " + api.GetTaskSolution().Text, DebugHelper.Type.Success);
        }

        private static void ExampleNoCaptchaProxyless()
        {
            DebugHelper.VerboseMode = true;

            var api = new NoCaptchaProxyless
            {
                ClientKey = "1234567890123456789012",
                WebsiteUrl = new Uri("http://http.myjino.ru/recaptcha/test-get.php"),
                WebsiteKey = "6Lc_aCMTAAAAABx7u2W0WPXnVbI_v6ZdbM6rYf16"
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
                DebugHelper.Out("Result: " + api.GetTaskSolution().GRecaptchaResponse, DebugHelper.Type.Success);
        }

        private static void ExampleNoCaptcha()
        {
            DebugHelper.VerboseMode = true;

            var api = new NoCaptcha
            {
                ClientKey = "1234567890123456789012",
                WebsiteUrl = new Uri("http://http.myjino.ru/recaptcha/test-get.php"),
                WebsiteKey = "6Lc_aCMTAAAAABx7u2W0WPXnVbI_v6ZdbM6rYf16",
                UserAgent =
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116",
                // proxy access parameters
                ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
                ProxyAddress = "xx.xx.xx.xx",
                ProxyPort = 8282,
                ProxyLogin = "123",
                ProxyPassword = "456"
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
                DebugHelper.Out("Result: " + api.GetTaskSolution().GRecaptchaResponse, DebugHelper.Type.Success);
        }

        private static void ExampleCustomCaptcha()
        {
            DebugHelper.VerboseMode = true;
            var randInt = new Random().Next(0, 10000);

            var api = new CustomCaptcha
            {
                ClientKey = "1234567890123456789012",
                // random here to let the same task to be assigned to the same workers
                ImageUrl = "https://files.anti-captcha.com/26/41f/c23/7c50ff19.jpg?random=" + randInt,
                Assignment = "Enter the licence plate number",
                Forms = new JArray
                {
                    new JObject
                    {
                        {"label", "Number"},
                        {"labelHint", false},
                        {"contentType", false},
                        {"name", "license_plate"},
                        {"inputType", "text"},
                        {
                            "inputOptions", new JObject
                            {
                                {"width", "100"},
                                {"placeHolder", "Enter letters and numbers without spaces"}
                            }
                        }
                    },
                    new JObject
                    {
                        {"label", "Car color"},
                        {"labelHint", "Select the car color"},
                        {"contentType", false},
                        {"name", "color"},
                        {"inputType", "select"},
                        {
                            "inputOptions", new JArray
                            {
                                new JObject
                                {
                                    {"value", "white"},
                                    {"caption", "White color"}
                                },
                                new JObject
                                {
                                    {"value", "black"},
                                    {"caption", "Black color"}
                                },
                                new JObject
                                {
                                    {"value", "gray"},
                                    {"caption", "Gray color"}
                                }
                            }
                        }
                    }
                }
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
                foreach (var answer in api.GetTaskSolution().Answers)
                    DebugHelper.Out(
                        "The answer for the question '" + answer.Key + "' : " + answer.Value,
                        DebugHelper.Type.Success
                    );
        }

        private static void ExampleFunCaptcha()
        {
            DebugHelper.VerboseMode = true;

            var api = new FunCaptcha
            {
                ClientKey = "1234567890123456789012",
                WebsiteUrl = new Uri("http://http.myjino.ru/funcaptcha_test/"),
                WebsitePublicKey = "DE0B0BB7-1EE4-4D70-1853-31B835D4506B",
                UserAgent =
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116",
                // proxy access parameters
                ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
                ProxyAddress = "xx.xx.xx.xx",
                ProxyPort = 8282,
                ProxyLogin = "123",
                ProxyPassword = "456"
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
                DebugHelper.Out("Result: " + api.GetTaskSolution().Token, DebugHelper.Type.Success);
        }
    }
}