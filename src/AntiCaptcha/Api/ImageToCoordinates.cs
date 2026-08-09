using System;
using System.IO;
using AntiCaptcha.ApiResponse;
using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Image where the worker clicks on objects and returns their coordinates.
    /// https://anti-captcha.com/apidoc/task-types/ImageToCoordinatesTask
    /// </summary>
    public class ImageToCoordinates : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        /// <summary>
        /// "points" to get single click coordinates, "rectangles" to get selection boxes.
        /// </summary>
        public string Mode { get; set; } = "points";

        /// <summary>
        /// Captcha image encoded in base64.
        /// </summary>
        public string BodyBase64 { get; set; } = "";

        /// <summary>
        /// Path to the captcha image file. Setting it fills <see cref="BodyBase64" />.
        /// </summary>
        public string FilePath
        {
            set
            {
                if (!File.Exists(value))
                {
                    DebugHelper.Out("File " + value + " not found", DebugHelper.Type.Error);

                    return;
                }

                BodyBase64 = StringHelper.FileToBase64String(value);
            }
        }

        /// <summary>Instruction for the worker, for example "select objects in the specified order".</summary>
        public string Comment { get; set; }

        /// <summary>Optional, lets you group the statistics in the dashboard by website.</summary>
        public Uri WebsiteUrl { get; set; }

        public override JObject GetPostData()
        {
            if (string.IsNullOrEmpty(BodyBase64))
            {
                DebugHelper.Out("Captcha image is not set, use BodyBase64 or FilePath", DebugHelper.Type.Error);

                return null;
            }

            var postData = new JObject
            {
                ["type"] = "ImageToCoordinatesTask",
                ["body"] = BodyBase64.Replace("\r", "").Replace("\n", ""),
                ["mode"] = Mode
            };

            SetIfNotEmpty(postData, "comment", Comment);
            SetIfNotEmpty(postData, "websiteURL", WebsiteUrl?.ToString());

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}
