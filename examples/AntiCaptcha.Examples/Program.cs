using System;
using System.Collections.Generic;
using AntiCaptcha.Api;
using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Examples
{
    /// <summary>
    /// Runnable examples for every task type. Put your API key below and run:
    ///     dotnet run --project examples/AntiCaptcha.Examples -- image
    /// Run without an argument to see the list of available examples.
    /// </summary>
    internal static class Program
    {
        private const string ClientKey = "API_KEY_HERE";

        /// <summary>
        /// Specify softId to earn 10% commission with your app.
        /// Get your softId here: https://anti-captcha.com/clients/tools/devcenter
        /// </summary>
        private const int SoftId = 0;

        private static readonly Dictionary<string, Action> Examples = new Dictionary<string, Action>
        {
            { "balance", ExampleBalance },
            { "image", ExampleImageToText },
            { "coordinates", ExampleImageToCoordinates },
            { "recaptcha2", ExampleRecaptchaV2 },
            { "recaptcha2-proxy", ExampleRecaptchaV2WithProxy },
            { "recaptcha2-enterprise", ExampleRecaptchaV2Enterprise },
            { "recaptcha3", ExampleRecaptchaV3 },
            { "hcaptcha", ExampleHCaptcha },
            { "funcaptcha", ExampleFunCaptcha },
            { "geetest3", ExampleGeeTestV3 },
            { "geetest4", ExampleGeeTestV4 },
            { "turnstile", ExampleTurnstile },
            { "prosopo", ExampleProsopo },
            { "friendly", ExampleFriendlyCaptcha },
            { "amazon", ExampleAmazonWaf },
            { "altcha", ExampleAltcha },
            { "antigate", ExampleAntiGate }
        };

        private static void Main(string[] args)
        {
            // Set to false to turn the debug output off
            DebugHelper.VerboseMode = true;

            if (args.Length == 0 || !Examples.ContainsKey(args[0]))
            {
                Console.WriteLine("Usage: dotnet run -- <example>");
                Console.WriteLine("Available examples: " + string.Join(", ", Examples.Keys));

                return;
            }

            Examples[args[0]]();
        }

        private static void ExampleBalance()
        {
            var api = new ImageToText { ClientKey = ClientKey, SoftId = SoftId };

            var balance = api.GetBalance();

            if (balance == null)
            {
                Console.WriteLine("GetBalance() failed: " + api.ErrorMessage);

                return;
            }

            Console.WriteLine("Balance: " + balance);
            Console.WriteLine("Captcha credits: " + api.GetCreditsBalance());
        }

        private static void ExampleImageToText()
        {
            var api = new ImageToText
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                FilePath = "captcha.jpg"

                // Optional, see https://anti-captcha.com/apidoc/task-types/ImageToTextTask
                // Phrase = true,                        // the image has 2 or more words
                // Case = true,                          // the answer is case sensitive
                // Numeric = ImageToText.NumericOption.NumbersOnly,
                // Math = 1,                             // the answer is the result of 50+5
                // MinLength = 1,
                // MaxLength = 10,
                // LanguagePool = "en",                  // "en" or "rn"
                // Comment = "Type in green characters"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "Captcha text: " + solution.Text);
        }

        private static void ExampleImageToCoordinates()
        {
            var api = new ImageToCoordinates
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                FilePath = "coordinates.jpg",
                Mode = "points",
                Comment = "Select objects in the specified order"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "Objects X,Y coordinates: " + solution.Coordinates);
        }

        private static void ExampleRecaptchaV2()
        {
            var api = new RecaptchaV2Proxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "6Lcyu8UZAAAAACwSh6Xf58WrNXTu0LLu4F85xf20"

                // IsInvisible = true,     // solving an invisible Recaptcha V2
                // DataSValue = "..."      // the "data-s" parameter, typical for google.com
            };

            var solution = api.Solve();

            if (solution == null)
            {
                Console.WriteLine("Could not solve the captcha: " + api.ErrorMessage);

                return;
            }

            Console.WriteLine("g-response token: " + solution.GRecaptchaResponse);
            Console.WriteLine("Worker's user-agent: " + solution.UserAgent);
        }

        private static void ExampleRecaptchaV2WithProxy()
        {
            var api = new RecaptchaV2
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "6Lcyu8UZAAAAACwSh6Xf58WrNXTu0LLu4F85xf20",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                            "(KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36",
                ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
                ProxyAddress = "1.2.3.4",
                ProxyPort = 1234,
                ProxyLogin = "login-optional",
                ProxyPassword = "password-optional"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "g-response token: " + solution.GRecaptchaResponse);
        }

        private static void ExampleRecaptchaV2Enterprise()
        {
            var api = new RecaptchaV2EnterpriseProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://store.steampowered.com/join"),
                WebsiteKey = "6LdIFr0ZAAAAAO3vz0O0OQrtAefzdJcWQM2TMYQH"
            };

            api.EnterprisePayload.Add("s", "SOME_ADDITIONAL_TOKEN");

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "g-response token: " + solution.GRecaptchaResponse);
        }

        private static void ExampleRecaptchaV3()
        {
            var api = new RecaptchaV3Proxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "6LcvNcwdAAAAAMWAuNRXH74u3QePsEzTm6GEjx0J",
                PageAction = "somefun",
                MinScore = 0.9

                // IsEnterprise = true     // solving a Recaptcha V3 Enterprise
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "g-response token: " + solution.GRecaptchaResponse);
        }

        private static void ExampleHCaptcha()
        {
            var api = new HCaptchaProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "00000000-1111-2222-3333-444444444444"

                // IsInvisible = true,
                // IsEnterprise = true
            };

            // hCaptcha Enterprise parameters
            // api.EnterprisePayload.Add("rqdata", "rqdata value from the target website");
            // api.EnterprisePayload.Add("sentry", "true");

            var solution = api.Solve();

            if (solution == null)
            {
                Console.WriteLine("Could not solve the captcha: " + api.ErrorMessage);

                return;
            }

            Console.WriteLine("hCaptcha token: " + solution.GRecaptchaResponse);
            Console.WriteLine("Use this user-agent for the form submission: " + solution.UserAgent);
            Console.WriteLine("respkey: " + solution.RespKey);
        }

        private static void ExampleFunCaptcha()
        {
            var api = new FunCaptchaProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsitePublicKey = "DE0B0BB7-1EE4-4D70-1853-31B835D4506B",
                // Look for a URL like
                // https://somewebsite-api.arkoselabs.com/v2/00000000-1111-2222-3333-444444444444/api.js
                ApiJSSubdomain = "somewebsite-api.arkoselabs.com",
                DataBlob = "{\"blob\":\"HERE_COMES_THE_blob_VALUE\"}"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "FunCaptcha token: " + solution.Token);
        }

        private static void ExampleGeeTestV3()
        {
            var api = new GeeTestProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "b6e21f90a91a3c2d4a31fe84e10d0442",
                // The challenge is one-time, grab a fresh one for every task
                WebsiteChallenge = "169acd4a58f2c99770322dfa5270c221"
            };

            var solution = api.Solve();

            if (solution == null)
            {
                Console.WriteLine("Could not solve the captcha: " + api.ErrorMessage);

                return;
            }

            Console.WriteLine("challenge: " + solution.Challenge);
            Console.WriteLine("seccode: " + solution.Seccode);
            Console.WriteLine("validate: " + solution.Validate);
        }

        private static void ExampleGeeTestV4()
        {
            var api = new GeeTestV4Proxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "e9ca9c9ca19ad540a8017f5c107b2d0f"
            };

            api.InitParameters.Add("riskType", "slide");

            var solution = api.Solve();

            if (solution == null)
            {
                Console.WriteLine("Could not solve the captcha: " + api.ErrorMessage);

                return;
            }

            Console.WriteLine("captcha_id: " + solution.CaptchaId);
            Console.WriteLine("lot_number: " + solution.LotNumber);
            Console.WriteLine("pass_token: " + solution.PassToken);
            Console.WriteLine("gen_time: " + solution.GenTime);
            Console.WriteLine("captcha_output: " + solution.CaptchaOutput);
        }

        private static void ExampleTurnstile()
        {
            var api = new TurnstileProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "0x4AAAAAAABD2Inoxs-yJ8bz"

                // Action = "optional page action",
                // CData = "cdata token for cloudflare",
                // ChlPageData = "chlPageData token for cloudflare"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "Turnstile token: " + solution.Token);
        }

        private static void ExampleProsopo()
        {
            var api = new ProsopoProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "sitekey-here"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "Prosopo token: " + solution.Token);
        }

        private static void ExampleFriendlyCaptcha()
        {
            var api = new FriendlyCaptchaProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "sitekey-here"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "Friendly Captcha token: " + solution.Token);
        }

        private static void ExampleAmazonWaf()
        {
            var api = new AmazonProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                WebsiteKey = "key_value_from_window.gokuProps_object",
                Iv = "iv_value_from_window.gokuProps_object",
                Context = "context_value_from_window.gokuProps_object"

                // Standalone widget instead of the bot filtering page:
                // WafType = "widget",
                // JsapiScript = "https://164cb210e333.edge.captcha-sdk.awswaf.com/164cb210e333/jsapi.js"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "aws-waf-token: " + solution.Token);
        }

        private static void ExampleAltcha()
        {
            var api = new AltchaProxyless
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("https://www.website.com/"),
                // Use ChallengeURL or ChallengeJson, not both
                ChallengeURL = "/some/path/to/challenge/url"
                // ChallengeJson = "{\"algorithm\":\"SHA-256\",\"challenge\":\"...\"}"
            };

            var solution = api.Solve();

            Console.WriteLine(solution == null
                ? "Could not solve the captcha: " + api.ErrorMessage
                : "Altcha token: " + solution.Token);
        }

        private static void ExampleAntiGate()
        {
            var api = new AntiGateTask
            {
                ClientKey = ClientKey,
                SoftId = SoftId,
                WebsiteUrl = new Uri("http://antigate.com/logintest.php"),
                TemplateName = "Sign-in and wait for control text",
                Variables = new JObject
                {
                    { "login_input_css", "#login" },
                    { "login_input_value", "the login" },
                    { "password_input_css", "#password" },
                    { "password_input_value", "the password" },
                    { "control_text", "You have been logged successfully" }
                }

                // ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
                // ProxyAddress = "1.2.3.4",
                // ProxyPort = 1234,
                // ProxyLogin = "login-optional",
                // ProxyPassword = "password-optional"
            };

            var solution = api.Solve();

            if (solution == null)
            {
                Console.WriteLine("Could not solve the task: " + api.ErrorMessage);

                return;
            }

            Console.WriteLine("Cookies: " + solution.Cookies);
            Console.WriteLine("localStorage: " + solution.LocalStorage);
            Console.WriteLine("Fingerprint: " + solution.Fingerprint);
            Console.WriteLine("URL: " + solution.Url);
        }
    }
}
