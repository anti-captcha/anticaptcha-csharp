using System;
using System.Globalization;

namespace Anticaptcha_example
{
    public class AnticaptchaApiWrapper
    {
        public enum ProxyType
        {
            http
        }

        private static dynamic JsonPostRequest(string host, string methodName, string postData)
        {
            return HttpHelper.Post(new Uri("http://" + host + "/" + methodName), postData);
        }

        public static AnticaptchaTask CreateNoCaptchaTask(string host, string clientKey, string websiteUrl,
            string websiteKey, ProxyType proxyType,
            string proxyAddress, int proxyPort, string proxyLogin, string proxyPassword, string userAgent)
        {
            var json = "{\n" +
                       "  \"clientKey\": \"" + clientKey + "\",\n" +
                       "  \"task\": {\n" +
                       "    \"type\": \"NoCaptchaTask\",\n" +
                       "    \"websiteURL\": \"" + websiteUrl + "\",\n" +
                       "    \"websiteKey\": \"" + websiteKey + "\",\n" +
                       "    \"proxyType\": \"" + proxyType + "\",\n" +
                       "    \"proxyAddress\": \"" + proxyAddress + "\",\n" +
                       "    \"proxyPort\": " + proxyPort + ",\n" +
                       "    \"proxyLogin\": \"" + proxyLogin + "\",\n" +
                       "    \"proxyPassword\": \"" + proxyPassword + "\",\n" +
                       "    \"userAgent\": \"" + userAgent + "\"\n" +
                       "  }\n" +
                       "}";

            try
            {
                var resultJson = JsonPostRequest(host, "createTask", json);

                int? taskId = null;
                int? errorId = null;
                string errorCode = null;
                string errorDescription = null;

                try
                {
                    taskId = int.Parse(resultJson.taskId.ToString());
                }
                catch
                {
                    // ignored
                }

                try
                {
                    errorId = int.Parse(resultJson.errorId.ToString());
                }
                catch
                {
                    // ignored
                }

                try
                {
                    errorCode = resultJson.errorCode.ToString();
                }
                catch
                {
                    // ignored
                }

                try
                {
                    errorDescription = resultJson.errorDescription.ToString();
                }
                catch
                {
                    // ignored
                }

                return new AnticaptchaTask(
                    taskId,
                    errorId,
                    errorCode,
                    errorDescription
                    );
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public static AnticaptchaTask CreateImageToTextTask(string host, string clientKey, string body,
            bool? phrase = null, bool? _case = null, int? numeric = null,
            bool? math = null, int? minLength = null, int? maxLength = null)
        {
            var json = "{\n" +
                       "  \"clientKey\": \"" + clientKey + "\",\n" +
                       "  \"task\": {\n" +
                       "    \"type\": \"ImageToTextTask\",\n" +
                       (phrase != null ? "    \"phrase\": \"" + phrase + "\",\n" : "") +
                       (_case != null ? "    \"case\": \"" + _case + "\",\n" : "") +
                       (numeric != null ? "    \"numeric\": \"" + numeric + "\",\n" : "") +
                       (math != null ? "    \"math\": \"" + math + "\",\n" : "") +
                       (minLength != null ? "    \"minLength\": " + minLength + ",\n" : "") +
                       (maxLength != null ? "    \"maxLength\": \"" + maxLength + "\",\n" : "") +
                       "    \"body\": \"" + body + "\"\n" +
                       "  }\n" +
                       "}";

            try
            {
                var resultJson = JsonPostRequest(host, "createTask", json);

                int? taskId = null;
                int? errorId = null;
                string errorCode = null;
                string errorDescription = null;

                try
                {
                    taskId = int.Parse(resultJson.taskId.ToString());
                }
                catch
                {
                    // ignored
                }

                try
                {
                    errorId = int.Parse(resultJson.errorId.ToString());
                }
                catch
                {
                    // ignored
                }

                try
                {
                    errorCode = resultJson.errorCode.ToString();
                }
                catch
                {
                    // ignored
                }

                try
                {
                    errorDescription = resultJson.errorDescription.ToString();
                }
                catch
                {
                    // ignored
                }

                return new AnticaptchaTask(
                    taskId,
                    errorId,
                    errorCode,
                    errorDescription
                    );
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public static AnticaptchaResult GetTaskResult(string host, string clientKey, AnticaptchaTask task)
        {
            var json = "{\n" +
                       "  \"clientKey\": \"" + clientKey + "\",\n" +
                       "  \"taskId\": " + task.GetTaskId() + "\n" +
                       "}";

            try
            {
                dynamic resultJson = JsonPostRequest(host, "getTaskResult", json);

                var status = AnticaptchaResult.Status.unknown;

                try
                {
                    status = resultJson.status.ToString().Equals("ready")
                        ? AnticaptchaResult.Status.ready
                        : AnticaptchaResult.Status.processing;
                }
                catch
                {
                    // ignored
                }

                string solution;
                int? errorId = null;
                string errorCode = null;
                string errorDescription = null;
                double? cost = null;
                string ip = null;
                int? createTime = null;
                int? endTime = null;
                int? solveCount = null;

                try
                {
                    solution = resultJson.solution.gRecaptchaResponse.ToString();
                }
                catch
                {
                    try
                    {
                        solution = resultJson.solution.text.ToString();
                    }
                    catch
                    {
                        solution = null;
                    }
                }

                try
                {
                    errorId = resultJson.errorId;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    errorCode = resultJson.errorCode;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    errorDescription = resultJson.errorDescription;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    cost = double.Parse(resultJson.cost.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                }
                catch
                {
                    // ignored
                }

                try
                {
                    createTime = resultJson.createTime;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    endTime = resultJson.endTime;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    solveCount = resultJson.solveCount;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    ip = resultJson.ip;
                }
                catch
                {
                    // ignored
                }

                return new AnticaptchaResult(
                    status,
                    solution,
                    errorId,
                    errorCode,
                    errorDescription,
                    cost,
                    ip,
                    createTime,
                    endTime,
                    solveCount
                    );
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}