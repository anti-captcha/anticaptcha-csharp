using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Helper
{
    /// <summary>
    /// Thin JSON-over-HTTP transport for the Anti-Captcha API.
    /// </summary>
    public static class HttpHelper
    {
        private static readonly HttpClient Client = new HttpClient
        {
            // The per-request timeout is enforced with a CancellationToken instead, so that
            // RequestTimeout stays changeable after the first request has been sent.
            Timeout = System.Threading.Timeout.InfiniteTimeSpan
        };

        /// <summary>
        /// Timeout of a single API call. Does not limit how long WaitForResult() polls.
        /// </summary>
        public static TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Posts a JSON body and parses the JSON object that comes back.
        /// </summary>
        public static async Task<HttpResult> PostAsync(Uri url, JObject postData,
            CancellationToken cancellationToken)
        {
            using (var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                timeout.CancelAfter(RequestTimeout);

                try
                {
                    var body = new StringContent(
                        JsonConvert.SerializeObject(postData, Formatting.Indented),
                        Encoding.UTF8,
                        "application/json"
                    );

                    using (var response = await Client.PostAsync(url, body, timeout.Token).ConfigureAwait(false))
                    {
                        var raw = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        if (!response.IsSuccessStatusCode)
                        {
                            return HttpResult.Failure("HTTP " + (int)response.StatusCode + " " +
                                                      response.ReasonPhrase + ": " + Shorten(raw));
                        }

                        try
                        {
                            return HttpResult.Success(JObject.Parse(raw));
                        }
                        catch (JsonException ex)
                        {
                            return HttpResult.Failure("Could not parse the API response as JSON: " + ex.Message +
                                                      "; raw response: " + Shorten(raw));
                        }
                    }
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    return HttpResult.Failure("The request timed out after " + RequestTimeout.TotalSeconds +
                                              " seconds");
                }
                catch (HttpRequestException ex)
                {
                    return HttpResult.Failure(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                }
            }
        }

        private static string Shorten(string raw)
        {
            if (string.IsNullOrEmpty(raw))
            {
                return "(empty)";
            }

            return raw.Length <= 500 ? raw : raw.Substring(0, 500) + "...";
        }

        /// <summary>
        /// Either a parsed JSON response or an error description.
        /// </summary>
        public sealed class HttpResult
        {
            private HttpResult(JObject json, string error)
            {
                Json = json;
                Error = error;
            }

            public JObject Json { get; }
            public string Error { get; }

            public static HttpResult Success(JObject json)
            {
                return new HttpResult(json, null);
            }

            public static HttpResult Failure(string error)
            {
                return new HttpResult(null, error);
            }
        }
    }
}
