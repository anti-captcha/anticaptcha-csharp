using System;
using System.Threading;
using System.Threading.Tasks;
using AntiCaptcha.ApiResponse;
using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha
{
    /// <summary>
    /// Base class of every captcha task. Handles talking to the API: creating a task,
    /// polling for its result and reading the account balance.
    /// </summary>
    public abstract class AnticaptchaBase
    {
        public enum ProxyTypeOption
        {
            Http,
            Socks4,
            Socks5
        }

        private const string ApiEndpoint = "https://api.anti-captcha.com/";

        /// <summary>
        /// Your API key from https://anti-captcha.com/clients/settings/apisetup
        /// </summary>
        public string ClientKey { get; set; }

        /// <summary>
        /// Specify softId to earn 10% commission with your app.
        /// Get your softId here: https://anti-captcha.com/clients/tools/devcenter
        /// </summary>
        public int SoftId { get; set; }

        /// <summary>
        /// Description of the last failure, if any.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Id of the task created by <see cref="CreateTask" />.
        /// </summary>
        public int TaskId { get; private set; }

        /// <summary>
        /// Full result of the last <see cref="WaitForResult" /> call.
        /// </summary>
        public TaskResultResponse TaskInfo { get; protected set; }

        /// <summary>
        /// Seconds to wait before the first status request.
        /// </summary>
        public int FirstAttemptWaitingInterval { get; set; } = 3;

        /// <summary>
        /// Seconds to wait between the following status requests.
        /// </summary>
        public int NormalWaitingInterval { get; set; } = 1;

        public abstract JObject GetPostData();

        /// <summary>
        /// Creates the task and waits until it is solved. Shorthand for
        /// <see cref="CreateTask" /> followed by <see cref="WaitForResult" />.
        /// </summary>
        /// <returns>The solution, or null when the captcha could not be solved.</returns>
        public TaskResultResponse.SolutionData Solve(int maxSeconds = 120)
        {
            return RunSync(ct => SolveAsync(maxSeconds, ct));
        }

        /// <inheritdoc cref="Solve" />
        public async Task<TaskResultResponse.SolutionData> SolveAsync(int maxSeconds = 120,
            CancellationToken cancellationToken = default)
        {
            if (!await CreateTaskAsync(cancellationToken).ConfigureAwait(false))
            {
                return null;
            }

            return await WaitForResultAsync(maxSeconds, cancellationToken).ConfigureAwait(false)
                ? TaskInfo.Solution
                : null;
        }

        /// <summary>
        /// Submits the task to the API. On success <see cref="TaskId" /> is filled.
        /// </summary>
        public bool CreateTask()
        {
            return RunSync(CreateTaskAsync);
        }

        /// <inheritdoc cref="CreateTask" />
        public async Task<bool> CreateTaskAsync(CancellationToken cancellationToken = default)
        {
            var taskJson = GetPostData();

            if (taskJson == null)
            {
                Fail("A task preparing error.");

                return false;
            }

            DebugHelper.Out(taskJson.ToString());

            var postData = new JObject
            {
                ["clientKey"] = ClientKey,
                ["softId"] = SoftId,
                ["task"] = taskJson
            };

            var response = await JsonPostRequest(ApiMethod.CreateTask, postData, cancellationToken)
                .ConfigureAwait(false);

            if (response == null)
            {
                return false;
            }

            var createTaskResponse = new CreateTaskResponse(response);

            if (createTaskResponse.ErrorId != 0)
            {
                Fail("API error " + createTaskResponse.ErrorId + ": " + createTaskResponse.ErrorDescription,
                    createTaskResponse.ErrorDescription);

                return false;
            }

            if (createTaskResponse.TaskId == null)
            {
                DebugHelper.JsonFieldParseError("taskId", response);
                Fail("API did not return a task id");

                return false;
            }

            TaskId = createTaskResponse.TaskId.Value;
            DebugHelper.Out("Task ID: " + TaskId, DebugHelper.Type.Success);

            return true;
        }

        /// <summary>
        /// Polls the API until the task is solved or <paramref name="maxSeconds" /> elapses.
        /// On success the solution is available via GetTaskSolution().
        /// </summary>
        public bool WaitForResult(int maxSeconds = 120)
        {
            return RunSync(ct => WaitForResultAsync(maxSeconds, ct));
        }

        /// <inheritdoc cref="WaitForResult" />
        public async Task<bool> WaitForResultAsync(int maxSeconds = 120,
            CancellationToken cancellationToken = default)
        {
            var deadline = DateTime.UtcNow.AddSeconds(maxSeconds);
            var firstAttempt = true;

            while (true)
            {
                var interval = firstAttempt ? FirstAttemptWaitingInterval : NormalWaitingInterval;
                firstAttempt = false;

                DebugHelper.Out("Waiting for " + interval + " seconds...");
                await Task.Delay(TimeSpan.FromSeconds(interval), cancellationToken).ConfigureAwait(false);

                DebugHelper.Out("Requesting the task status");

                var postData = new JObject
                {
                    ["clientKey"] = ClientKey,
                    ["taskId"] = TaskId
                };

                var response = await JsonPostRequest(ApiMethod.GetTaskResult, postData, cancellationToken)
                    .ConfigureAwait(false);

                if (response == null)
                {
                    return false;
                }

                TaskInfo = new TaskResultResponse(response);

                if (TaskInfo.ErrorId != 0)
                {
                    Fail("API error " + TaskInfo.ErrorId + ": " + TaskInfo.ErrorDescription,
                        TaskInfo.ErrorDescription);

                    return false;
                }

                if (TaskInfo.Status == TaskResultResponse.StatusType.Ready)
                {
                    if (TaskInfo.Solution == null || TaskInfo.Solution.IsEmpty)
                    {
                        Fail("Got no 'solution' field from API");

                        return false;
                    }

                    DebugHelper.Out("The task is complete!", DebugHelper.Type.Success);

                    return true;
                }

                if (TaskInfo.Status != TaskResultResponse.StatusType.Processing)
                {
                    Fail("An unknown API status, please update your software");

                    return false;
                }

                if (DateTime.UtcNow >= deadline)
                {
                    Fail("Time's out.");

                    return false;
                }

                DebugHelper.Out("The task is still processing...");
            }
        }

        /// <summary>
        /// Account balance in US dollars, or null on error.
        /// </summary>
        public double? GetBalance()
        {
            return RunSync(GetBalanceAsync);
        }

        /// <inheritdoc cref="GetBalance" />
        public async Task<double?> GetBalanceAsync(CancellationToken cancellationToken = default)
        {
            var response = await RequestBalance(cancellationToken).ConfigureAwait(false);

            return response?.Balance;
        }

        /// <summary>
        /// Number of prepaid captcha credits on the account, or null on error.
        /// </summary>
        public double? GetCreditsBalance()
        {
            return RunSync(GetCreditsBalanceAsync);
        }

        /// <inheritdoc cref="GetCreditsBalance" />
        public async Task<double?> GetCreditsBalanceAsync(CancellationToken cancellationToken = default)
        {
            var response = await RequestBalance(cancellationToken).ConfigureAwait(false);

            return response?.CaptchaCredits;
        }

        /// <summary>
        /// Validates the proxy settings and appends them to the task, following the
        /// https://anti-captcha.com/apidoc/task-types conventions. Returns false when the
        /// proxy is not configured correctly.
        /// </summary>
        protected bool AddProxyData(JObject postData, ProxyTypeOption proxyType, string proxyAddress,
            int? proxyPort, string proxyLogin, string proxyPassword)
        {
            if (proxyPort == null || proxyPort < 1 || proxyPort > 65535 || string.IsNullOrEmpty(proxyAddress))
            {
                Fail("Proxy data is incorrect!");

                return false;
            }

            postData["proxyType"] = proxyType.ToString().ToLowerInvariant();
            postData["proxyAddress"] = proxyAddress;
            postData["proxyPort"] = proxyPort;

            if (!string.IsNullOrEmpty(proxyLogin))
            {
                postData["proxyLogin"] = proxyLogin;
                postData["proxyPassword"] = proxyPassword;
            }

            return true;
        }

        /// <summary>
        /// Adds an optional string parameter, skipping it when the caller left it unset.
        /// </summary>
        protected static void SetIfNotEmpty(JObject postData, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                postData[name] = value;
            }
        }

        private async Task<BalanceResponse> RequestBalance(CancellationToken cancellationToken)
        {
            var postData = new JObject
            {
                ["clientKey"] = ClientKey
            };

            var response = await JsonPostRequest(ApiMethod.GetBalance, postData, cancellationToken)
                .ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            var balanceResponse = new BalanceResponse(response);

            if (balanceResponse.ErrorId == 0)
            {
                return balanceResponse;
            }

            Fail("API error " + balanceResponse.ErrorId + ": " + balanceResponse.ErrorDescription,
                balanceResponse.ErrorDescription);

            return null;
        }

        private async Task<JObject> JsonPostRequest(ApiMethod methodName, JObject postData,
            CancellationToken cancellationToken)
        {
            var method = char.ToLowerInvariant(methodName.ToString()[0]) + methodName.ToString().Substring(1);
            var url = new Uri(new Uri(ApiEndpoint), method);

            DebugHelper.Out("Connecting to " + url.Host);

            var result = await HttpHelper.PostAsync(url, postData, cancellationToken).ConfigureAwait(false);

            if (result.Json != null)
            {
                return result.Json;
            }

            Fail("HTTP or JSON error: " + result.Error);

            return null;
        }

        private void Fail(string logMessage, string errorMessage = null)
        {
            ErrorMessage = errorMessage ?? logMessage;
            DebugHelper.Out(logMessage, DebugHelper.Type.Error);
        }

        /// <summary>
        /// Runs an async operation from synchronous code without risking a deadlock on
        /// single-threaded synchronization contexts (WinForms/WPF/classic ASP.NET).
        /// </summary>
        private static T RunSync<T>(Func<CancellationToken, Task<T>> operation)
        {
            return Task.Run(() => operation(CancellationToken.None)).GetAwaiter().GetResult();
        }

        private enum ApiMethod
        {
            CreateTask,
            GetTaskResult,
            GetBalance
        }
    }
}
