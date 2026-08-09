## Official Anti-Captcha.com C# / .NET package ##

Official anti-captcha.com .NET package for solving images with text, Recaptcha v2/v3 Enterprise/non-Enterprise, Funcaptcha, GeeTest, HCaptcha Enterprise/non-Enterprise, Turnstile, Amazon WAF, Prosopo, Friendly Captcha and Altcha.

[Anti-captcha](https://anti-captcha.com) is an oldest and cheapest web service dedicated to solving captchas by human workers from around the world. By solving captchas with us you help people in poorest regions of the world to earn money, which not only cover their basic needs, but also gives them ability to financially help their families, study and avoid jobs where they're simply not happy.

To use the service you need to [register](https://anti-captcha.com/clients/) and topup your balance. Prices start from $0.0005 per image captcha and $0.002 for Recaptcha. That's $0.5 per 1000 for images and $2 for 1000 Recaptchas.

For more technical information and articles visit our [documentation](https://anti-captcha.com/apidoc) page.

**Install the package**:
```bash
dotnet add package AntiCaptchaOfficial
```
or, in the Package Manager Console:
```powershell
Install-Package AntiCaptchaOfficial
```

The package targets .NET Standard 2.0 and .NET 8, so it runs on .NET 5 and newer, .NET Core 2.0+, and .NET Framework 4.6.1+.

**Examples how to solve:**

- [Image Captcha](#solve-image-captcha)
- [Recaptcha V2](#solve-recaptcha-v2)
- [Recaptcha V2 Enterprise](#solve-recaptcha-v2-enterprise)
- [Recaptcha V3](#solve-recaptcha-v3)
- [hCaptcha](#solve-hcaptcha)
- [FunCaptcha](#solve-funcaptcha)
- [GeeTest](#solve-geetest)
- [Turnstile](#solve-turnstile)
- [Image to coordinates](#image-to-coordinates)
- [AntiGate (custom tasks)](#solve-antigate-custom-tasks)
- [Prosopo](#solve-prosopo)
- [Friendly Captcha](#solve-friendly-captcha)
- [Amazon WAF](#solve-amazon-waf)
- [Altcha](#solve-altcha)

Every task type also has an `async` API: `SolveAsync()`, `CreateTaskAsync()`, `WaitForResultAsync()`, `GetBalanceAsync()`. See [Async usage](#async-usage) below.

### Solve image captcha
```csharp
using System;
using AntiCaptcha.Api;
using AntiCaptcha.Helper;

class Program
{
    static void Main()
    {
        // Set to 'false' to turn off debug output
        DebugHelper.VerboseMode = true;

        var api = new ImageToText
        {
            ClientKey = "API_KEY_HERE",

            // Specify softId to earn 10% commission with your app.
            // Get your softId here: https://anti-captcha.com/clients/tools/devcenter
            SoftId = 0,

            FilePath = "captcha.jpg",
            // OR
            // BodyBase64 = "image-encoded-in-base64",

            // Optional settings, see https://anti-captcha.com/apidoc/task-types/ImageToTextTask for details
            // Phrase = true,                            // Set to 'true' if the image has 2 or more words
            // Case = true,                              // Set to 'true' if the image is case-sensitive
            // Numeric = ImageToText.NumericOption.NumbersOnly,
            // Math = 1,                                 // Set to 1 if the answer is a math operation, like 50+5
            // MinLength = 1,                            // Minimum length of the text
            // MaxLength = 10,                           // Maximum length of the text
            // LanguagePool = "en",                      // 'en' for English, 'rn' for Russian
            // Comment = "Type in green characters",     // Optional comment for the task
            // WebsiteUrl = new Uri("https://some-website.com/"), // Optional, to collect stats by website
        };

        // Make sure the API key funds balance is positive
        var balance = api.GetBalance();
        if (balance == null || balance <= 0)
        {
            // Exit the program to make sure you don't DDoS the API with requests while having empty balance
            Console.WriteLine("Balance error: " + api.ErrorMessage);
            return;
        }
        Console.WriteLine("Balance: " + balance);

        var solution = api.Solve();
        if (solution == null)
        {
            Console.WriteLine("Could not solve the captcha: " + api.ErrorMessage);
            return;
        }

        Console.WriteLine("Captcha Solution: " + solution.Text);
    }
}
```
&nbsp;

### Solve Recaptcha V2
```csharp
using System;
using AntiCaptcha.Api;
using AntiCaptcha.Helper;

class Program
{
    static void Main()
    {
        DebugHelper.VerboseMode = true;

        var api = new RecaptchaV2Proxyless
        {
            ClientKey = "API_KEY_HERE",
            SoftId = 0,
            WebsiteUrl = new Uri("https://www.website.com/"),
            WebsiteKey = "6Lcyu8UZAAAAACwSh6Xf58WrNXTu0LLu4F85xf20",
            IsInvisible = false,  // Set to 'true' if you are solving an invisible Recaptcha V2
            DataSValue = "",      // Fill this if you are solving a Recaptcha V2 with the 'data-s' parameter,
                                  // typically found at google.com websites
        };

        var solution = api.Solve();
        if (solution == null)
        {
            Console.WriteLine("Could not solve the captcha: " + api.ErrorMessage);
            return;
        }

        Console.WriteLine("Recaptcha g-response token: " + solution.GRecaptchaResponse);
        // In case you need the worker's user-agent
        Console.WriteLine("User-Agent: " + solution.UserAgent);
    }
}
```
Also with [proxy](https://anti-captcha.com/apidoc/task-types/RecaptchaV2Task):
```csharp
var api = new RecaptchaV2
{
    ClientKey = "API_KEY_HERE",
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "6Lcyu8UZAAAAACwSh6Xf58WrNXTu0LLu4F85xf20",
    IsInvisible = false,
    DataSValue = "",
    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36",
    ProxyType = AnticaptchaBase.ProxyTypeOption.Http,  // Http, Socks4 or Socks5
    ProxyAddress = "1.2.3.4",
    ProxyPort = 1234,
    ProxyLogin = "login-optional",
    ProxyPassword = "password-optional",
};

var solution = api.Solve();
```
&nbsp;

### Solve Recaptcha V2 Enterprise
```csharp
var api = new RecaptchaV2EnterpriseProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://store.steampowered.com/join"),
    WebsiteKey = "6LdIFr0ZAAAAAO3vz0O0OQrtAefzdJcWQM2TMYQH",
    // ApiDomain = "recaptcha.net",   // Only if the website loads the script from a non-google.com domain
};

// Additional parameters passed to the grecaptcha.enterprise.render call
api.EnterprisePayload.Add("s", "SOME_ADDITIONAL_TOKEN");

var solution = api.Solve();
Console.WriteLine("Recaptcha g-response token: " + solution?.GRecaptchaResponse);
```
The proxy-on version is `RecaptchaV2Enterprise` and takes the same proxy properties as `RecaptchaV2` above.

&nbsp;

### Solve Recaptcha V3
```csharp
var api = new RecaptchaV3Proxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "6LcvNcwdAAAAAMWAuNRXH74u3QePsEzTm6GEjx0J",
    PageAction = "somefun",
    MinScore = 0.9,           // One of 0.3, 0.7, 0.9
    // IsEnterprise = true,   // Set to 'true' if you are solving a Recaptcha V3 Enterprise
};

var solution = api.Solve();
Console.WriteLine("Recaptcha g-response token: " + solution?.GRecaptchaResponse);
```
Recaptcha V3 has no proxy-on version.

&nbsp;

### Solve Hcaptcha
```csharp
var api = new HCaptchaProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "00000000-1111-2222-3333-444444444444",
    // IsInvisible = true,
    // IsEnterprise = true,
};

// hCaptcha Enterprise parameters like rqdata, sentry, apiEndpoint, endpoint, reportapi, assethost, imghost
// api.EnterprisePayload.Add("rqdata", "rqdata value from the target website");
// api.EnterprisePayload.Add("sentry", "true");

var solution = api.Solve();
if (solution != null)
{
    Console.WriteLine("Hcaptcha Token: " + solution.GRecaptchaResponse);
    // Use this user-agent for the form submission
    Console.WriteLine("User-Agent: " + solution.UserAgent);
    // Optional "respkey" value, you may need it too
    Console.WriteLine("respkey: " + solution.RespKey);
}
```
Also with [proxy](https://anti-captcha.com/apidoc/task-types/HCaptchaTask):
```csharp
var api = new HCaptcha
{
    ClientKey = "API_KEY_HERE",
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "00000000-1111-2222-3333-444444444444",
    ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
    ProxyAddress = "1.2.3.4",
    ProxyPort = 1234,
    ProxyLogin = "login-optional",
    ProxyPassword = "password-optional",
};
```
&nbsp;

### Solve FunCaptcha
```csharp
var api = new FunCaptchaProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsitePublicKey = "00000000-1111-2222-3333-444444444444",
    // Make sure to find and set this correctly, look for a URL like
    // https://somewebsite-api.arkoselabs.com/v2/00000000-1111-2222-3333-444444444444/api.js
    ApiJSSubdomain = "somewebsite-api.arkoselabs.com",
    // DataBlob = "{\"blob\":\"HERE_COMES_THE_blob_VALUE\"}",
};

var solution = api.Solve();
Console.WriteLine("Funcaptcha Token: " + solution?.Token);
```
Also with [proxy](https://anti-captcha.com/apidoc/task-types/FunCaptchaTask):
```csharp
var api = new FunCaptcha
{
    ClientKey = "API_KEY_HERE",
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsitePublicKey = "00000000-1111-2222-3333-444444444444",
    ApiJSSubdomain = "somewebsite-api.arkoselabs.com",
    ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
    ProxyAddress = "1.2.3.4",
    ProxyPort = 1234,
    ProxyLogin = "login-optional",
    ProxyPassword = "password-optional",
};
```
&nbsp;

### Solve Turnstile
```csharp
var api = new TurnstileProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "0x4AAAAAAABD2Inoxs-yJ8bz",
    // Action = "optional page action",
    // CData = "cdata token for cloudflare",
    // ChlPageData = "chlPageData token for cloudflare",
};

var solution = api.Solve();
Console.WriteLine("Turnstile Token: " + solution?.Token);
// In case you need the worker's user-agent
Console.WriteLine("User-Agent: " + solution?.UserAgent);
```
Also with [proxy](https://anti-captcha.com/apidoc/task-types/TurnstileTask):
```csharp
var api = new Turnstile
{
    ClientKey = "API_KEY_HERE",
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "0x4AAAAAAABD2Inoxs-yJ8bz",
    ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
    ProxyAddress = "1.2.3.4",
    ProxyPort = 1234,
    ProxyLogin = "login-optional",
    ProxyPassword = "password-optional",
};
```
&nbsp;

### Solve GeeTest
GeeTest has 2 versions, number 3 and 4. Number 3 requires the parameter "challenge". Number 4 has the optional setting "InitParameters".

GeeTest v3:
```csharp
var api = new GeeTestProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "e9ca9c9ca19ad540a8017f5c107b2d0f",
    // You need to get a new "challenge" each time
    WebsiteChallenge = "1234567890abcdef1234567890abcdef",
};

var solution = api.Solve();
if (solution != null)
{
    Console.WriteLine("challenge: " + solution.Challenge);
    Console.WriteLine("seccode: " + solution.Seccode);
    Console.WriteLine("validate: " + solution.Validate);
}
```

GeeTest v4:
```csharp
var api = new GeeTestV4Proxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://bitget.com/"),
    WebsiteKey = "e9ca9c9ca19ad540a8017f5c107b2d0f",
};

api.InitParameters.Add("riskType", "slide");

var solution = api.Solve();
if (solution != null)
{
    Console.WriteLine("captcha_id: " + solution.CaptchaId);
    Console.WriteLine("lot_number: " + solution.LotNumber);
    Console.WriteLine("pass_token: " + solution.PassToken);
    Console.WriteLine("gen_time: " + solution.GenTime);
    Console.WriteLine("captcha_output: " + solution.CaptchaOutput);
}
```
Also with [proxy](https://anti-captcha.com/apidoc/task-types/GeeTestTask) — use `GeeTest` for v3 and `GeeTestV4` for v4:
```csharp
var api = new GeeTestV4
{
    ClientKey = "API_KEY_HERE",
    WebsiteUrl = new Uri("https://bitget.com/"),
    WebsiteKey = "e9ca9c9ca19ad540a8017f5c107b2d0f",
    ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
    ProxyAddress = "1.2.3.4",
    ProxyPort = 1234,
    ProxyLogin = "login-optional",
    ProxyPassword = "password-optional",
};
```
&nbsp;

### Image to coordinates
```csharp
var api = new ImageToCoordinates
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    FilePath = "coordinates.jpg",
    // OR
    // BodyBase64 = "image-encoded-in-base64",
    Mode = "points",   // "points" or "rectangles"
    Comment = "Select objects in the specified order",
};

var solution = api.Solve();
Console.WriteLine("Objects X,Y coordinates: " + solution?.Coordinates);
```
&nbsp;

### Solve AntiGate (custom tasks)
```csharp
using Newtonsoft.Json.Linq;

var api = new AntiGateTask
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("http://antigate.com/logintest.php"),
    TemplateName = "Sign-in and wait for control text",
    Variables = new JObject
    {
        { "login_input_css", "#login" },
        { "login_input_value", "the login" },
        { "password_input_css", "#password" },
        { "password_input_value", "the password" },
        { "control_text", "You have been logged successfully" },
    },
    // The proxy is optional for AntiGate tasks
    ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
    ProxyAddress = "1.2.3.4",
    ProxyPort = 1234,
    ProxyLogin = "login-optional",
    ProxyPassword = "password-optional",
};

// api.DomainsOfInterest.Add("example.com");

var solution = api.Solve();
if (solution != null)
{
    Console.WriteLine("Cookies: " + solution.Cookies);
    Console.WriteLine("localStorage: " + solution.LocalStorage);
    Console.WriteLine("Fingerprint: " + solution.Fingerprint);
    Console.WriteLine("URL: " + solution.Url);
}
```
&nbsp;

### Solve Prosopo
```csharp
var api = new ProsopoProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "sitekey-here",
};

var solution = api.Solve();
Console.WriteLine("Prosopo Token: " + solution?.Token);
```
Also with [proxy](https://anti-captcha.com/apidoc/task-types/ProsopoTask) — use the `Prosopo` class with the same proxy properties as above.

&nbsp;

### Solve Friendly Captcha
```csharp
var api = new FriendlyCaptchaProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "sitekey-here",
};

var solution = api.Solve();
Console.WriteLine("Friendly Captcha Token: " + solution?.Token);
```
Also with [proxy](https://anti-captcha.com/apidoc/task-types/FriendlyCaptchaTask) — use the `FriendlyCaptcha` class with the same proxy properties as above.

&nbsp;

### Solve Amazon WAF
Two options here:

1. When the captcha is at the bot filtering page and you need the `aws-waf-token` cookie:
```csharp
var api = new AmazonProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "key_value_from_window.gokuProps_object",
    Iv = "iv_value_from_window.gokuProps_object",
    Context = "context_value_from_window.gokuProps_object",
    // CaptchaScript = "optional_captcha.js_script_url",
    // ChallengeScript = "optional_challenge.js_script_url",
};

var solution = api.Solve();
Console.WriteLine("aws-waf-token: " + solution?.Token);
```

2. When the captcha is a standalone widget triggered by a user's action:
```csharp
var api = new AmazonProxyless
{
    ClientKey = "API_KEY_HERE",
    WebsiteUrl = new Uri("https://www.website.com/"),
    // Captcha widget's API key from the AwsWafCaptcha.renderCaptcha function
    WebsiteKey = "captcha_key_value",
    WafType = "widget",
    // Full URL to jsapi.js
    JsapiScript = "https://164cb210e333.edge.captcha-sdk.awswaf.com/164cb210e333/jsapi.js",
};
```
Both options have a [proxy-on](https://anti-captcha.com/apidoc/task-types/AmazonTask) version — use the `Amazon` class with the same proxy properties as above.

&nbsp;

### Solve Altcha
```csharp
var api = new AltchaProxyless
{
    ClientKey = "API_KEY_HERE",
    SoftId = 0,
    WebsiteUrl = new Uri("https://www.website.com/"),

    // Option 1: use the challenge URL (use one of the options!)
    ChallengeUrl = "/some/path/to/challenge/url",

    // Option 2: use the challenge JSON
    // ChallengeJson = "{\"algorithm\":\"SHA-256\",\"challenge\":\"1a40f7ba3393f9513016879de41c7221f14e563856de2f647233a00accf9c28b\",\"salt\":\"0887f273d79df143355b9e5f\",\"signature\":\"1de2bbf282420aef6ca0a84c38c85e2b1e40023d28bef72278d735555a8f47fb\"}",
};

var solution = api.Solve();
Console.WriteLine("Altcha Token: " + solution?.Token);
```
Also with [proxy](https://anti-captcha.com/apidoc/task-types/AltchaTask) — use the `Altcha` class with the same proxy properties as above.

&nbsp;

### Async usage
Every synchronous call has an `async` twin that accepts a `CancellationToken`:
```csharp
var api = new RecaptchaV2Proxyless
{
    ClientKey = "API_KEY_HERE",
    WebsiteUrl = new Uri("https://www.website.com/"),
    WebsiteKey = "6Lcyu8UZAAAAACwSh6Xf58WrNXTu0LLu4F85xf20",
};

var balance = await api.GetBalanceAsync();
var solution = await api.SolveAsync(maxSeconds: 120, cancellationToken: token);
```

If you would rather drive the task yourself instead of using `Solve()`:
```csharp
if (!await api.CreateTaskAsync())
{
    Console.WriteLine("Could not create the task: " + api.ErrorMessage);
}
else if (!await api.WaitForResultAsync())
{
    Console.WriteLine("Could not solve the captcha: " + api.ErrorMessage);
}
else
{
    Console.WriteLine(api.GetTaskSolution().GRecaptchaResponse);
    Console.WriteLine("Task id: " + api.TaskId + ", cost: " + api.TaskInfo.Cost);
}
```

### Debug output
The library is quiet by default. Turn the log on with:
```csharp
AntiCaptcha.Helper.DebugHelper.VerboseMode = true;
```
or send it to your own logger instead of the console:
```csharp
AntiCaptcha.Helper.DebugHelper.Sink = (message, type) => myLogger.Log(type + ": " + message);
```

### Running the examples
```bash
git clone https://github.com/anti-captcha/anticaptcha-csharp.git
cd anticaptcha-csharp
# put your API key into examples/AntiCaptcha.Examples/Program.cs
dotnet run --project examples/AntiCaptcha.Examples -- image
```
Run it without an argument to see the full list of examples.
