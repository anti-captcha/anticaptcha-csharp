using System;
using Anticaptcha_example.Api;
using Anticaptcha_example.Helper;
using Newtonsoft.Json.Linq;

namespace Anticaptcha_example
{
    internal class Program
    {
        private const string ClientKey = "1234567890";

        private static void Main()
        {
            ExampleGetBalance();
            ExampleImageToText();
            ExampleSquare();
            ExampleHCaptchaProxyless();
            ExampleRecaptcha2Proxyless();
            ExampleRecaptcha2();
            ExampleCustomCaptcha();
            ExampleFunCaptcha();
            ExampleRecaptchaV3Proxyless();
            ExampleGeeTestProxyless();

            Console.ReadKey();
        }

        private static void ExampleGetBalance()
        {
            DebugHelper.VerboseMode = true;

            var api = new ImageToText
            {
                ClientKey = ClientKey
            };

            var balance = api.GetBalance();

            if (balance == null)
                DebugHelper.Out("GetBalance() failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else
                DebugHelper.Out("Balance: " + balance, DebugHelper.Type.Success);
        }

        private static void ExampleRecaptchaV3Proxyless()
        {
            DebugHelper.VerboseMode = true;

            var api = new RecaptchaV3Proxyless
            {
                ClientKey = ClientKey,
                WebsiteUrl = new Uri("http://www.supremenewyork.com"),
                WebsiteKey = "6Leva6oUAAAAAMFYqdLAI8kJ5tw7BtkHYpK10RcD",
                PageAction = "testPageAction"
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
                DebugHelper.Out("Result: " + api.GetTaskSolution().GRecaptchaResponse, DebugHelper.Type.Success);
        }

        private static void ExampleGeeTestProxyless()
        {
            DebugHelper.VerboseMode = true;

            // website key ("gt") and "challenge" for testing you can get here: https://auth.geetest.com/api/init_captcha?time=1561554686474
            // you need to get a new "challenge" each time
            var api = new GeeTestProxyless()
            {
                ClientKey = ClientKey,
                WebsiteUrl = new Uri("http://www.supremenewyork.com"),
                WebsiteKey = "b6e21f90a91a3c2d4a31fe84e10d0442",
                WebsiteChallenge = "169acd4a58f2c99770322dfa5270c221"
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
            {
                DebugHelper.Out("Result CHALLENGE: " + api.GetTaskSolution().Challenge, DebugHelper.Type.Success);
                DebugHelper.Out("Result SECCODE: " + api.GetTaskSolution().Seccode, DebugHelper.Type.Success);
                DebugHelper.Out("Result VALIDATE: " + api.GetTaskSolution().Validate, DebugHelper.Type.Success);
            }
        }

        private static void ExampleImageToText()
        {
            DebugHelper.VerboseMode = true;

            var api = new ImageToText
            {
                ClientKey = ClientKey,
                FilePath = "captcha.jpg"
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
                DebugHelper.Out("Result: " + api.GetTaskSolution().Text, DebugHelper.Type.Success);
        }

        private static void ExampleSquare()
        {
            DebugHelper.VerboseMode = true;

            var api = new SquareCaptcha
            {
                ClientKey = ClientKey,
                FilePath = "square.jpg",
                ObjectName = "FISH AND HOUSE / РЫБА И ДОМ",
                ColumnsCount = 4,
                RowsCount = 4
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
            {
                string result = "";

                foreach (int cellNumber in api.GetTaskSolution().CellNumbers)
                {
                    result += cellNumber + " ";
                }

                DebugHelper.Out("Result cell numbers (starting 0): " + result, DebugHelper.Type.Success);
            }
        }

        private static void ExampleRecaptcha2Proxyless()
        {
            DebugHelper.VerboseMode = true;

            var api = new RecaptchaV2Proxyless
            {
                ClientKey = ClientKey,
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

        private static void ExampleHCaptchaProxyless()
        {
            DebugHelper.VerboseMode = true;

            var api = new HCaptchaProxyless
            {
                ClientKey = ClientKey,
                WebsiteUrl = new Uri("http://democaptcha.com/"),
                WebsiteKey = "51829642-2cda-4b09-896c-594f89d700cc"
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
                DebugHelper.Out("Result: " + api.GetTaskSolution().GRecaptchaResponse, DebugHelper.Type.Success);
        }

        private static void ExampleRecaptcha2()
        {
            DebugHelper.VerboseMode = true;

            var api = new RecaptchaV2
            {
                ClientKey = ClientKey,
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
                ClientKey = ClientKey,
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
                ClientKey = ClientKey,
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